using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Header("GameObject Storage Lists and Arrays")]
    // Array that holds the types of sprites
    public GameObject[] Sprites;
    // List of arrays that hold the various paths. An adjacency list
    public List<int>[] spritesAdjacencyList;
    // An array of all sprites on the board now.
    public GameObject[] allSpritesMatrix;
    // An array holding the null positions of the array 
    public int[] nullSpriteArray;
    //private FindMatches findMatches;

    [Header("Board Variables")]
    public int height = 8; // x
    public int width = 8; // y
    private int vertices; 
    public int offSet = 15;
    public float Distance = 1.0f;
    //public GameObject destroyParticle;

    public static GridManager Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        vertices = width * height;
        //findMatches = FindObjectOfType<FindMatches>();
        allSpritesMatrix = new GameObject[vertices];
        spritesAdjacencyList = new List<int>[vertices];
        nullSpriteArray = new int[width];
        InitGrid();
    }

    void InitGrid()
    {
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

    private void createSprite(int row, int column)
    {
        // Creation of the sprite.
        Vector2 tempPosition = new Vector2(row, column);
        int spriteToUse = Random.Range(0, Sprites.Length);
        GameObject sprite = Instantiate(Sprites[spriteToUse], tempPosition, Quaternion.identity);
        sprite.transform.parent = this.transform;
        sprite.name = "(" + row + "," + column + ")";
        allSpritesMatrix[((row * height) + column)] = sprite; 
    }

 /*   
    private bool MatchesAt(int column, int row, GameObject sprite)
    {
        if(column > 1 && row > 1){
            if(spritesAdjacencyList[column -1, row].tag == sprite.tag && spritesAdjacencyList[column -2, row].tag == sprite.tag){
                return true;
            }
            if (spritesAdjacencyList[column, row-1].tag == sprite.tag && spritesAdjacencyList[column, row-2].tag == sprite.tag)
            {
                return true;
            }

        }else if(column <= 1 || row <= 1){
            if(row > 1){
                if(spritesAdjacencyList[column, row - 1].tag == sprite.tag && spritesAdjacencyList[column, row -2].tag == sprite.tag){
                    return true;
                }
            }
            if (column > 1)
            {
                if (spritesAdjacencyList[column-1, row].tag == sprite.tag && spritesAdjacencyList[column-2, row].tag == sprite.tag)
                {
                    return true;
                }
            }
        }

        return false;
    }
*/
    private void DestroyMatchesAt(int index){
        if(allSpritesMatrix[index] != null && allSpritesMatrix[index].GetComponent<Sprite>().isMatched){
            Destroy(allSpritesMatrix[index]);
            allSpritesMatrix[index] = null;
            nullSpriteArray[index / height]++; // Gets the column of the index
        }
    }

    public void DestroyMatches(){
        for (int i = 0; i < vertices; i++){
            DestroyMatchesAt(i);
        }
        StartCoroutine(DecreaseRowCo());
    }

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
                        createSprite(i, j);
                    }
                }
            }
            nullSpriteArray[i] = 0;
        }
    }


    private bool MatchesOnBoard(){
        for (int i = 0; i < width; i ++){
            for (int j = 0; j < height; j ++){
                int index = (i * height) + j;
                if(allSpritesMatrix[index]!= null){
                    if(allSpritesMatrix[index].GetComponent<Sprite>().isMatched){
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private IEnumerator FillBoardCo(){
        RefillBoard();
        yield return new WaitForSeconds(.5f);

        //while(MatchesOnBoard()){
        //    yield return new WaitForSeconds(.5f);
        //    DestroyMatches();
        //}
        //findMatches.currentMatches.Clear();
        //currentSprite = null;
        //yield return new WaitForSeconds(.5f);
        //currentState = GameState.move;

    }
}
