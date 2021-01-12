using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    // Array that holds the types of sprites
    public GameObject[] Sprites;
    public int height = 8; // x
    public int width = 8; // y
    private int vertices; 
    public int offSet = 15;
    public float Distance = 1.0f;
    public GameObject destroyParticle;
    // List of arrays that hold the various paths. An adjacency list
    public List<int>[] spritesAdjacencyList;
    // An array of all sprites on the board now.
    public GameObject[] allSpritesMatrix;
    private FindMatches findMatches;

    public static GridManager Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        vertices = width * height;
        findMatches = FindObjectOfType<FindMatches>();
        allSpritesMatrix = new GameObject[vertices];
        spritesAdjacencyList = new List<int>[vertices];
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

            Vector2 tempPosition = new Vector2(row, column);
            int spriteToUse = Random.Range(0, Sprites.Length);
            GameObject sprite = Instantiate(Sprites[spriteToUse], tempPosition, Quaternion.identity);
            sprite.transform.parent = this.transform;
            sprite.name = "(" + row + "," + column + ")";
            allSpritesMatrix[((row * height) + column)] = sprite;        
        }

        row = 0;
        column = 0;
        for (int i = 0; i < vertices; i++)
        {
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
        Debug.Log(spritesAdjacencyList);
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
        }
    }

    public void DestroyMatches(){
        for (int i = 0; i < vertices; i++){
            DestroyMatchesAt(i);
        }
        //StartCoroutine(DecreaseRowCo());
    }
/*
    private IEnumerator DecreaseRowCo(){
        int nullCount = 0;
        for (int i = 0; i < width; i++){
            for (int j = 0; j < height; j++){
                if(spritesAdjacencyList[i, j] == null){
                    nullCount++;
                } else if(nullCount > 0){
                    spritesAdjacencyList[i, j].GetComponent<Sprite>().row -= nullCount;
                    spritesAdjacencyList[i, j - nullCount] = spritesAdjacencyList[i, j];
                    spritesAdjacencyList[i, j - nullCount].name = "(" + i + "," + (j - nullCount) + ")";
                    spritesAdjacencyList[i, j] = null;
                }
            }
            nullCount = 0;
        }
        yield return new WaitForSeconds(.4f);
        StartCoroutine(FillBoardCo());
    }


    private void RefillBoard(){
        for (int i = 0; i < width; i++){
            for (int j = 0; j < height; j++){
                if(spritesAdjacencyList[i, j] == null){
                    Vector2 tempPosition = new Vector2(i, j);
                    int spriteToUse = Random.Range(0, Sprites.Length);
                    GameObject piece = Instantiate(Sprites[spriteToUse], tempPosition, Quaternion.identity);
                    spritesAdjacencyList[i, j] = piece;
                    piece.GetComponent<Sprite>().row = j;
                    piece.GetComponent<Sprite>().column = i;
                    piece.name = "(" + i + "," + j + ")";
                    piece.transform.parent = this.transform;
                    //tempPosition = new Vector2(i, j);
                    //piece.transform.position = Vector2.Lerp(transform.position, tempPosition, .6f);
                    
                }
            }
        }
    }

    private bool MatchesOnBoard(){
        for (int i = 0; i < width; i ++){
            for (int j = 0; j < height; j ++){
                if(spritesAdjacencyList[i, j]!= null){
                    if(spritesAdjacencyList[i, j].GetComponent<Sprite>().isMatched){
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
*/
}
