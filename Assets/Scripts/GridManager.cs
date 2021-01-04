using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject[] Sprites;
    public int height = 8; // x
    public int width = 8; // y
    private int vertices; 
    public int offSet = 15;
    public float Distance = 1.0f;
    public GameObject destroyParticle;
    public List<GameObject>[] allSprites;
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
        allSprites = new List<GameObject>[vertices];
        InitGrid();
    }

    void InitGrid()
    {
        int row = 0;
        int column = 0;
        // Maybe make a 1d arrage and mod by the height to get the number of 
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
            int h = ((row * height) + column);
            allSpritesMatrix[((row * height) + column)] = sprite; // (row * height) + column)           
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
            allSprites[allSpritesIndex] = new List<GameObject>();

            if ((((row - 1) * height) + column) >= 0 && (((row - 1) * height) + column) < vertices)
            {
                int index = (((row - 1) * height) + column);
                allSprites[allSpritesIndex].Add(allSpritesMatrix[index]);
            }

            if ((((row + 1) * height) + column) >= 0 && (((row + 1) * height) + column) < vertices)
            {
                int index = (((row + 1) * height) + column);
                allSprites[allSpritesIndex].Add(allSpritesMatrix[index]);
            }

            if (((row * height) + (column - 1)) >= 0 && ((row * height) + (column - 1)) < vertices)
            {
                int index = ((row * height) + (column - 1));
                allSprites[allSpritesIndex].Add(allSpritesMatrix[index]);
            }

            if (((row * height) + (column + 1)) >= 0 && ((row * height) + (column + 1)) < vertices)
            {
                int index = ((row * height) + (column + 1));
                allSprites[allSpritesIndex].Add(allSpritesMatrix[index]);
            }
        }
    }
    
/*
    private bool MatchesAt(int column, int row, GameObject sprite)
    {
        if(column > 1 && row > 1){
            if(allSprites[column -1, row].tag == sprite.tag && allSprites[column -2, row].tag == sprite.tag){
                return true;
            }
            if (allSprites[column, row-1].tag == sprite.tag && allSprites[column, row-2].tag == sprite.tag)
            {
                return true;
            }

        }else if(column <= 1 || row <= 1){
            if(row > 1){
                if(allSprites[column, row - 1].tag == sprite.tag && allSprites[column, row -2].tag == sprite.tag){
                    return true;
                }
            }
            if (column > 1)
            {
                if (allSprites[column-1, row].tag == sprite.tag && allSprites[column-2, row].tag == sprite.tag)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private void DestroyMatchesAt(int column, int row){
        if(allSprites[column, row].GetComponent<Sprite>().isMatched){
            Destroy(allSprites[column, row]);
            allSprites[column, row] = null;
        }
    }

    public void DestroyMatches(){
        for (int i = 0; i < width; i++){
            for (int j = 0; j < height; j++){
                if (allSprites[i, j] != null){
                    DestroyMatchesAt(i, j);
                }
            }
        }
        StartCoroutine(DecreaseRowCo());
    }

    private IEnumerator DecreaseRowCo(){
        int nullCount = 0;
        for (int i = 0; i < width; i++){
            for (int j = 0; j < height; j++){
                if(allSprites[i, j] == null){
                    nullCount++;
                } else if(nullCount > 0){
                    allSprites[i, j].GetComponent<Sprite>().row -= nullCount;
                    allSprites[i, j - nullCount] = allSprites[i, j];
                    allSprites[i, j - nullCount].name = "(" + i + "," + (j - nullCount) + ")";
                    allSprites[i, j] = null;
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
                if(allSprites[i, j] == null){
                    Vector2 tempPosition = new Vector2(i, j);
                    int spriteToUse = Random.Range(0, Sprites.Length);
                    GameObject piece = Instantiate(Sprites[spriteToUse], tempPosition, Quaternion.identity);
                    allSprites[i, j] = piece;
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
                if(allSprites[i, j]!= null){
                    if(allSprites[i, j].GetComponent<Sprite>().isMatched){
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
