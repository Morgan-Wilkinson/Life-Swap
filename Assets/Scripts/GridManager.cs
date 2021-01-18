using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState {
    wait,
    move
}

public class GridManager : MonoBehaviour
{
    [Header("Board Variables")]
    public GameSettings settings;
    // Height of the grid.
    public int height;
    // Width of the grid.
    public int width;
    // Maximum number of sprites on the grid.
    private int vertices;
    // Height from which the sprites drop in.
    public int offSet = 5;
    //public GameObject destroyParticle;

    public static GridManager Instance { get; private set; }

    [Header("GameObject Storage Lists and Arrays")]
    // Array that holds the types of sprites.
    public GameObject[] Sprites;
    // List of arrays that hold the various paths. An adjacency list.
    public List<int>[] spritesAdjacencyList;
    // An array of all sprites on the board now.
    public GameObject[] allSpritesMatrix;
    // An array holding the null positions of the array.
    public int[] nullSpriteArray;
    //private FindMatches findMatches;

    [Header("Game Progression")]
    public GameState currentState = GameState.move;

    void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        settings = FindObjectOfType<GameSettings>();
        height = settings.gridDimensions.height;
        width = settings.gridDimensions.width;
        vertices = width * height;
        allSpritesMatrix = new GameObject[vertices];
        spritesAdjacencyList = new List<int>[vertices];
        nullSpriteArray = new int[width];
        InitGrid();
    }

    // Game Setup
    void InitGrid(){
        int row = 0;
        int column = 0;
        int index = 0;

        for (int i = 0; i < vertices; i++)
        {
            column = i % height;
            if(column == 0 && i > 0)
            {
                row = i % width;
            }

            // Creation of the sprite.
            createSprite(row, column);
        }    

        row = 0;
        column = 0;
        for (int i = 0; i < vertices; i++)
        {
            // Setup for determining rows and columns.
            column = i % height;
            if(column == 0 && i > 0)
            {
                row = i % width;
            }
            int allSpritesIndex = ((row * height) + column);
            spritesAdjacencyList[allSpritesIndex] = new List<int>();

            // Down
            index = (((row - 1) * height) + column);
            if (index >= 0 && index < vertices)
            {
                spritesAdjacencyList[allSpritesIndex].Add(index);
            }


            // Up
            index = (((row + 1) * height) + column);
            if (index >= 0 && index < vertices)
            {
                spritesAdjacencyList[allSpritesIndex].Add(index);
            }

            // Left
            index = ((row * height) + (column - 1));
            if (index >= 0 && index < vertices && column != 0)
            {
                spritesAdjacencyList[allSpritesIndex].Add(index);
            }

            // Right
            index = ((row * height) + (column + 1));
            if (index >= 0 && index < vertices && column != width)
            {
                spritesAdjacencyList[allSpritesIndex].Add(index);
            }
        }
    }

    // Creation of a sprite at row, column.
    private void createSprite(int row, int column){
        // Creation of the sprite.
        Vector2 tempPosition = new Vector2(row, column);
        int spriteToUse = Random.Range(0, Sprites.Length);
        GameObject sprite = Instantiate(Sprites[spriteToUse], tempPosition, Quaternion.identity);
        sprite.transform.parent = this.transform;
        sprite.name = "(" + row + "," + column + ")";
        sprite.GetComponent<Sprite>().row = row;
        sprite.GetComponent<Sprite>().column = column;
        sprite.GetComponent<Sprite>().index = ((row * height) + column);
        allSpritesMatrix[((row * height) + column)] = sprite; 
    }

    // Creation of a sprite at row, column at offset.
    private void createSpriteOffset(int row, int column){
        // Creation of the sprite.
        Vector2 tempPosition = new Vector2(row, column + offSet);
        int spriteToUse = Random.Range(0, Sprites.Length);
        GameObject sprite = Instantiate(Sprites[spriteToUse], tempPosition, Quaternion.identity);
        sprite.transform.parent = this.transform;
        sprite.name = "(" + row + "," + column + ")";
        sprite.GetComponent<Sprite>().row = row;
        sprite.GetComponent<Sprite>().column = column;
        sprite.GetComponent<Sprite>().index = ((row * height) + column);
        allSpritesMatrix[((row * height) + column)] = sprite; 
    }

    // Function that checks each index for destruction
    public void DestroyMatches(){
        for (int i = 0; i < vertices; i++){
            DestroyMatchesAt(i);
        }
        StartCoroutine(DecreaseRowCo());
    }

    // Destruction of sprite at index in array.
    private void DestroyMatchesAt(int index){
        if(allSpritesMatrix[index] != null && allSpritesMatrix[index].GetComponent<Sprite>().isMatched){
            Destroy(allSpritesMatrix[index]);
            allSpritesMatrix[index] = null;
            nullSpriteArray[index / height]++; // Gets the column of the index
        }
    }

    // Function that slides down the sprites if the sprite below it is 
    // destroyed.
    private IEnumerator DecreaseRowCo(){
        int nullCount = 0;
        for(int i = 0; i < width; i++)
        {
            if(nullSpriteArray[i] > 0)
            {
                for(int j = 0; j < height; j++)
                {
                    int oldIndex = (i * height) + j;
                    if(allSpritesMatrix[oldIndex] == null){
                        nullCount++;
                    }
                    else if(nullCount > 0) 
                    {
                        int newIndex = (i * height) + (j - nullCount);
                        allSpritesMatrix[oldIndex].GetComponent<Sprite>().column -= nullCount;
                        allSpritesMatrix[newIndex] = allSpritesMatrix[oldIndex];
                        allSpritesMatrix[newIndex].name = "(" + i + "," + (j - nullCount) + ")";
                        allSpritesMatrix[oldIndex] = null;
                    }
                }
            }
            nullCount = 0;
        }
        yield return new WaitForSeconds(.4f);
        StartCoroutine(FillBoardCo());
    }

    // Function to create new sprites
    private void RefillBoard(){
        for(int i = 0; i < width; i++)
        {
            if(nullSpriteArray[i] > 0)
            {
                for(int j = 0; j < height; j++)
                {
                    int index = (i * height) + j;
                    if(allSpritesMatrix[index] == null){
                        // Creation of the sprite.
                        createSpriteOffset(i, j);
                    }
                }
            }
            nullSpriteArray[i] = 0;
        }
    }

    private IEnumerator FillBoardCo(){
        RefillBoard();
        yield return new WaitForSeconds(.5f);

        yield return new WaitForSeconds(.5f);
        currentState = GameState.move;
    }
}
