using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public enum GameState {
    wait,
    move
}

public class GridManager : MonoBehaviour
{
    public static GridManager Instance { get; private set; }
    private FindMatches findMatches;

    [Header("Board Variables")]
    public GameSettings settings;
    public GameLevels gameLevels;
    public GridDimensions gridDimensions;
    // The configuration of this levels breakable sprites
    public GameBreakableSpriteProgression breakablesConfig;

    public int level;    // Height of the grid.
    public int height;
    // Width of the grid.
    public int width;
    // Maximum number of sprites on the grid.
    public int vertices;
    // Height from which the sprites drop in.
    public int offSet;
    // Major direction of sprites
    private int majorAxis;

    [Header("GameObject Storage Lists and Arrays")]
    public GameObject destroyEffect;
    // Array that holds the types of sprites.
    public GameObject[] SpritesPrefab;
    public GameObject BreakablePrefab;
    public string[] levelBreakableTypes;
    public List<UnityEngine.Sprite>[] BreakableSprites;
    
    // List of arrays that hold the various paths. An adjacency list.
    public List<int>[] spritesAdjacencyList;
    // An array of all sprites on the board now.
    public GameObject[] allSpritesMatrix;
    // List of arrays that hold the various paths of all possible matches. 
    public List<int> allMatches; 
    // An array holding the null positions of the array.
    public int[] nullSpriteArray;

    [Header("Game Progression")]
    public GameState currentState = GameState.move;

    [Header("GamePlay Variables")]
    // Special sprites
    public int arrow;
    public int bomb;
    public int multiBomb; 

    // Score Manager
    public ScoreManager scoreManager;

    void Awake(){
        Instance = this;
    }

    // Start is called before the first frame update
    void Start(){
        // Get scripts
        settings = FindObjectOfType<GameSettings>();
        scoreManager = FindObjectOfType<ScoreManager>();
        findMatches = FindObjectOfType<FindMatches>();
        
        gameLevels = settings.gameLevels;
        gridDimensions = settings.gridDimensions;
        breakablesConfig = settings.breakableSpritesTypes;

        level = 0;

        // Board Variables
        height = gridDimensions.height;
        width = gridDimensions.width;
        offSet = gridDimensions.offSet;
        vertices = width * height;
        majorAxis = height;

        // GameObject Storage Lists and Arrays
        allSpritesMatrix = new GameObject[vertices];
        spritesAdjacencyList = new List<int>[vertices];
        allMatches = new List<int>();
        nullSpriteArray = new int[width];

        // GamePlay Variables
        arrow = settings.gridDimensions.arrow;
        bomb = settings.gridDimensions.bomb;
        multiBomb = settings.gridDimensions.multiBomb;

        FindBreakableType();
        InitGrid();
    }


    // Game Setup
    void InitGrid(){
        int index = 0;
        int[] array = gameLevels.levels[level].breakablesArray;
        if (array.Length > 0)
        {
            for(int i = 0; i < array.Length; i++)
            {
                index = array[i];
                int column = index % majorAxis;
                int row = index / majorAxis;
                allSpritesMatrix[index] = createBreakableSprite(row, column);
            }
        }

        for (int i = 0; i < width; i++){
            for(int j = 0; j < height; j++){
                index = (i * majorAxis) + j;
                if(allSpritesMatrix[index] == null)
                {
                    allSpritesMatrix[index] = createSpriteOffset(i, j);
                }
            }
        }
        adjacencyListBuilder();
        findMatches.FindAllMatchesAdj();
    }

    // This function creates the adjacencyList of int type.
    private void adjacencyListBuilder(){
        int index = 0;
        
        for (int i = 0; i < width; i++){
            for(int j = 0; j < height; j++){

                // Setup for determining rows and columns.
                int allSpritesIndex = ((i * majorAxis) + j);
                spritesAdjacencyList[allSpritesIndex] = new List<int>();

                // Down
                index = (((i - 1) * majorAxis) + j);
                if (index >= 0 && index < vertices)
                {
                    spritesAdjacencyList[allSpritesIndex].Add(index);
                }


                // Up
                index = (((i + 1) * majorAxis) + j);
                if (index >= 0 && index < vertices)
                {
                    spritesAdjacencyList[allSpritesIndex].Add(index);
                }

                // Left
                index = ((i * majorAxis) + (j - 1));
                if (index >= 0 && index < vertices && j != 0)
                {
                    spritesAdjacencyList[allSpritesIndex].Add(index);
                }

                // Right
                index = ((i * majorAxis) + (j + 1));
                if (index >= 0 && index < vertices && j != width)
                {
                    spritesAdjacencyList[allSpritesIndex].Add(index);
                }
            }
        }
    }

    private void FindBreakableType()
    {
        levelBreakableTypes = gameLevels.levels[level].breakableTypes;
        BreakableSprites = new List<UnityEngine.Sprite>[levelBreakableTypes.Length];
        for(int i = 0; i < levelBreakableTypes.Length; i++)
        {
            BreakableSprites[i] = new List<UnityEngine.Sprite>();
            foreach(BreakableSpriteProgression type in breakablesConfig.breakablesProgression)
            {
                if(type.spriteType == levelBreakableTypes[0].ToString())
                {
                    foreach(string sprite in type.breakableSprites)
                    {
                        BreakableSprites[i].Add(Resources.Load<UnityEngine.Sprite>("Images/Sprites/Breakables/"+sprite.ToString()));
                    }
                }
            }
        }
    }

    // Creation of breakable sprites
    private GameObject createBreakableSprite(int row, int column){
        Vector2 position = new Vector2(row, column);
        GameObject sprite = Instantiate(BreakablePrefab, position, Quaternion.identity);
        sprite.GetComponent<SpriteRenderer>().sprite = BreakableSprites[0][0];
        sprite.transform.parent = this.transform;
        sprite.name = "Breakable: (" + row + "," + column + ")";
        Sprite spriteClass = sprite.GetComponent<Sprite>();
        spriteClass.isBreakable = true;
        spriteClass.row = row;
        spriteClass.column = column;
        spriteClass.index = ((row * majorAxis) + column);
        spriteClass.BreakableSpriteSetup(0, 3);
        
        return sprite;
    }
    
    // Creation of a sprite at row, column.
    // private GameObject createSprite(int row, int column){
    //     Vector2 tempPosition = new Vector2(row, column);
    //     int spriteToUse = Random.Range(0, SpritesPrefab.Length);
    //     GameObject sprite = Instantiate(SpritesPrefab[spriteToUse], tempPosition, Quaternion.identity);
    //     sprite.transform.parent = this.transform;
    //     sprite.name = "(" + row + "," + column + ")";
    //     Sprite spriteClass = sprite.GetComponent<Sprite>();
    //     spriteClass.row = row;
    //     spriteClass.column = column;
    //     spriteClass.index = ((row * majorAxis) + column);
    //     return sprite;
    // }

    // Creation of a sprite at row, column at offset.
    private GameObject createSpriteOffset(int row, int column){
        Vector2 tempPosition = new Vector2(row, column + offSet);
        int spriteToUse = Random.Range(0, SpritesPrefab.Length);
        GameObject sprite = Instantiate(SpritesPrefab[spriteToUse], tempPosition, Quaternion.identity);
        sprite.transform.parent = this.transform;
        sprite.name = "(" + row + "," + column + ")";
        Sprite spriteClass = sprite.GetComponent<Sprite>();
        spriteClass.row = row;
        spriteClass.column = column;
        spriteClass.index = ((row * majorAxis) + column); 
        return sprite;
    }

    // Function that checks each index for destruction.
    public void DestroyMatches(){
        int numberOfDestroyedSprites = 0;
        for(int i = 0; i < width; i++)
        {
            if(nullSpriteArray[i] > 0)
            {
                for(int j = 0; j < height; j++)
                {
                    int index = (i * majorAxis) + j;
                    DestroyMatchesAt(index);
                    numberOfDestroyedSprites++;
                }
            }
        }
        // Call scoreing function;
        scoreManager.IncreaseScore(numberOfDestroyedSprites);

        // Decrease rows and fill them in.
        StartCoroutine(DecreaseRowCo());
    }

    private void DestroyMatchesAt(int index){
        if(allSpritesMatrix[index] != null && allSpritesMatrix[index].GetComponent<Sprite>().isMatched){
            GameObject particle = Instantiate(destroyEffect, allSpritesMatrix[index].transform.position, Quaternion.identity, this.transform);
            Destroy(particle, 0.5f);
            Destroy(allSpritesMatrix[index]);
            allSpritesMatrix[index] = null; 
        }
    }

    // Function that slides down the sprites if the sprite below it is 
    // destroyed.
    private IEnumerator DecreaseRowCo(){
        int nullCount = 0;
        yield return new WaitForSeconds(0.5f);
        for(int i = 0; i < width; i++)
        {
            if(nullSpriteArray[i] > 0)
            {
                for(int j = 0; j < height; j++)
                {
                    int oldIndex = (i * majorAxis) + j;
                    if(allSpritesMatrix[oldIndex] == null){
                        nullCount++;
                    }
                    else if(nullCount > 0) 
                    {
                        int newIndex = (i * majorAxis) + (j - nullCount);
                        allSpritesMatrix[oldIndex].GetComponent<Sprite>().column -= nullCount;
                        allSpritesMatrix[newIndex] = allSpritesMatrix[oldIndex];
                        allSpritesMatrix[newIndex].name = "(" + i + "," + (j - nullCount) + ")";
                        allSpritesMatrix[oldIndex] = null;
                    }
                }
            }
            nullCount = 0;
        }
        yield return new WaitForSeconds(0.4f);
        StartCoroutine(FillBoardCo());
    }

    private IEnumerator FillBoardCo(){
        RefillBoard();
        findMatches.FindAllMatchesAdj();
        yield return new WaitForSeconds(.2f);

        currentState = GameState.move;
    }

    // Function to create new sprites
    private void RefillBoard(){
        for(int i = 0; i < width; i++)
        {
            if(nullSpriteArray[i] > 0)
            {
                for(int j = 0; j < height; j++)
                {
                    int index = (i * majorAxis) + j;
                    if(allSpritesMatrix[index] == null){
                        // Creation of the sprite.
                        allSpritesMatrix[index] = createSpriteOffset(i, j);
                    }
                }
            }
            nullSpriteArray[i] = 0;
        }
    }
}
