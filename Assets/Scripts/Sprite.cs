using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sprite : MonoBehaviour
{
    [Header("Grid Variables")]
    public int column;
    public int row;
    public int previousColumn;
    public int previousRow;
    public int targetX;
    public int targetY;
    public bool isMatched = false;

    //private FindMatches findMatches;
    private GridManager grid;
    public GameObject otherDot;
    private Vector2 firstTouchPosition;
    private Vector2 finalTouchPosition;
    private Vector2 tempPosition;

    // Start is called before the first frame update
    void Start()
    {
        grid = FindObjectOfType<GridManager>();
        column = (int)(transform.position.x);
        row = (int)(transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        /*FindMatches();
        if(isMatched){
            
            SpriteRenderer mySprite = GetComponent<SpriteRenderer>();
            Color currentColor = mySprite.color;
            mySprite.color = new Color(currentColor.r, currentColor.g, currentColor.b, .5f);
        }*/
    }

    private void OnMouseDown()
    {
        // On press check if sprite match anything on left and right then start Coroutine
        //StartCoroutine(CheckMoveCo());
        FindMatches();
        StartCoroutine(CheckMoveCo());
        /*if(isMatched){
            
            SpriteRenderer mySprite = GetComponent<SpriteRenderer>();
            Color currentColor = mySprite.color;
            mySprite.color = new Color(currentColor.r, currentColor.g, currentColor.b, .5f);
        }*/
    }

    public IEnumerator CheckMoveCo()
    {
        // Created in ep 7.
        if(!isMatched)
        {
            yield return new WaitForSeconds(.5f);
            // Enable shake animation
        }
        else
        {
            // DestroyMatches()
            grid.DestroyMatches();
            // Enable destroyed matches animation
        }
    }

    void FindMatches()
    {
        // Vertical
        if(column > 0 && column < grid.width - 1)
        {
            for(int i = column; i < grid.width - 1; i++)
            {
                GameObject tempSprite = grid.allSprites[i - 1, row];
                if (tempSprite != null)
                {
                    if (tempSprite.tag == this.gameObject.tag)
                    {
                        tempSprite.GetComponent<Sprite>().isMatched = true;
                        isMatched = true;
                        Debug.Log(i-1 +"," + row +" is a match");
                    }
                    else{
                        break;
                    }
                }
                //Debug.Log(i);
            }
            for(int i = column; i > 0; i--)
            {
                GameObject tempSprite = grid.allSprites[i + 1, row];
                if (tempSprite != null)
                {
                    if (tempSprite.tag == this.gameObject.tag)
                    {
                        tempSprite.GetComponent<Sprite>().isMatched = true;
                        isMatched = true;
                        //Debug.Log(2);
                        Debug.Log(i-1 +"," + row +" is a match");
                    }
                    else{
                        break;
                    }
                }
            }
        }

        // Horizontal
        if (row > 0 && row < grid.height - 1)
        {
            for(int i = row; i < grid.width - 1; i++)
            {
                GameObject tempSprite = grid.allSprites[column, i + 1];
                if (tempSprite != null)
                {
                    if (tempSprite.tag == this.gameObject.tag)
                    {
                        tempSprite.GetComponent<Sprite>().isMatched = true;
                        isMatched = true;
                        //Debug.Log(3);
                    }
                    else{
                        break;
                    }
                }
            }
            for(int i = row; i > 0; i--)
            {
                GameObject tempSprite = grid.allSprites[column, i - 1];
                if (tempSprite != null)
                {
                    if (tempSprite.tag == this.gameObject.tag)
                    {
                        tempSprite.GetComponent<Sprite>().isMatched = true;
                        isMatched = true;
                        //Debug.Log(4);
                    }
                    else{
                        break;
                    }
                }
            }
        }
    }
}
