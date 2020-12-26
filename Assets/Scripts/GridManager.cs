using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject[] Sprites;
    public int height = 8; // x
    public int width = 8; // y
    public int offSet = 15;
    public float Distance = 1.0f;
    public GameObject destroyParticle;
    public GameObject[,] allSprites;
    private FindMatches findMatches;

    public static GridManager Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        findMatches = FindObjectOfType<FindMatches>();
        allSprites = new GameObject[width, height];
        InitGrid();
    }

    // Change init so that it allows for the creation of multiple 3 or more matches.
    void InitGrid()
    {
        for (int x = 0; x < width; x++) 
            for (int y = 0; y < height; y++) 
            {
                
                Vector2 tempPosition = new Vector2(x, y);
                int spriteToUse = Random.Range(0, Sprites.Length);
                GameObject sprite = Instantiate(Sprites[spriteToUse], tempPosition, Quaternion.identity);
                sprite.transform.parent = this.transform;
                sprite.name = "(" + x + "," + y + ")";
                allSprites[x, y] = sprite;
            }
    }

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
            //How many elements are in the matched pieces list from findmatches?
            ///if(findMatches.currentMatches.Count == 4 || findMatches.currentMatches.Count == 7){
                //findMatches.CheckBombs();
            //}

            //GameObject particle = Instantiate(destroyParticle, 
             //                                 allSprites[column, row].transform.position, 
               //                               Quaternion.identity);
            //Destroy(particle, .5f);
            Destroy(allSprites[column, row]);
            allSprites[column, row] = null;
            //Debug.Log(column +" , " + row);
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
        for (int i = 0; i < width; i ++){
            for (int j = 0; j < height; j ++){
                if(allSprites[i, j] == null){
                    nullCount++;
                }else if(nullCount > 0){
                    // Fix row dropping
                    //Debug.Log("Row before "+ allSprites[i, j].GetComponent<Sprite>().row);+
                    
                    allSprites[i, j].GetComponent<Sprite>().row -= nullCount;
                    //Debug.Log("Row after "+ allSprites[i, j].GetComponent<Sprite>().row);
                    allSprites[i, nullCount] = allSprites[i, j];
                    allSprites[i, j] = null;
                    Debug.Log(i +" , "+ j + " AND "+ i + " , " + nullCount);

                }
            }
            nullCount = 0;
        }
        yield return new WaitForSeconds(.4f);
        StartCoroutine(FillBoardCo());
    }

    private void RefillBoard(){
        for (int i = 0; i < width; i ++){
            for (int j = 0; j < height; j ++){
                if(allSprites[i, j] == null){
                    Vector2 tempPosition = new Vector2(i, j);
                    int spriteToUse = Random.Range(0, Sprites.Length);
                    GameObject piece = Instantiate(Sprites[spriteToUse], tempPosition, Quaternion.identity);
                    allSprites[i, j] = piece;
                    piece.GetComponent<Sprite>().row = j;
                    piece.GetComponent<Sprite>().column = i;
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

        /*while(MatchesOnBoard()){
            yield return new WaitForSeconds(.5f);
            DestroyMatches();
        }*/
        //findMatches.currentMatches.Clear();
        //currentSprite = null;
        //yield return new WaitForSeconds(.5f);
        //currentState = GameState.move;

    }
}
