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
        tempPosition = new Vector2(column, row);
        transform.position = Vector2.Lerp(transform.position, tempPosition, .6f);

    }

    private void OnMouseDown()
    {
        // On press check if sprite match anything on left and right then start Coroutine
        FindMatches();
        grid.DestroyMatches();
        StartCoroutine(CheckMoveCo());
    }

    public IEnumerator CheckMoveCo()
    {
        if(!isMatched)
        {
            yield return new WaitForSeconds(.5f);
            // Enable shake animation
        }
        else
        {
            // DestroyMatches()
            //grid.DestroyMatches();
            // Enable destroyed matches animation
        }
    }

    void FindMatches()
    {
        // Horizontal
        if(column >= 0 && column <= grid.width - 1)
        {
            // Right
            for(int i = column; i < grid.width - 1; i++)
            {
                if (i + 1 < grid.width)
                {
                    GameObject tempSprite = grid.allSprites[i + 1, row];
                    if (tempSprite != null)
                    {
                        if (tempSprite.tag == this.gameObject.tag)
                        {
                            tempSprite.GetComponent<Sprite>().isMatched = true;
                            isMatched = true;
                        }
                        else{
                            break;
                        }
                    }
                }
            }
            
            // Left
            for(int i = column; i >= 0; i--)
            {
                if (i - 1 >= 0)
                {
                    GameObject tempSprite = grid.allSprites[i - 1, row];
                    if (tempSprite != null)
                    {
                        if (tempSprite.tag == this.gameObject.tag)
                        {
                            tempSprite.GetComponent<Sprite>().isMatched = true;
                            isMatched = true;
                        }
                        else{
                            break;
                        }
                    }
                }
            }
        }
        
        // Vertical
        if (row >= 0 && row <= grid.height - 1)
        {
            // Up
            for(int i = row; i < grid.width - 1; i++)
            {
                GameObject tempSprite = grid.allSprites[column, i + 1];
                if (tempSprite != null)
                {
                    if (tempSprite.tag == this.gameObject.tag)
                    {
                        tempSprite.GetComponent<Sprite>().isMatched = true;
                        isMatched = true;
                    }
                    else{
                        break;
                    }
                }
            }

            // Down
            for(int i = row; i > 0; i--)
            {
                GameObject tempSprite = grid.allSprites[column, i - 1];
                if (tempSprite != null)
                {
                    if (tempSprite.tag == this.gameObject.tag)
                    {
                        tempSprite.GetComponent<Sprite>().isMatched = true;
                        isMatched = true;
                    }
                    else{
                        break;
                    }
                }
            }
        }
        
    }
}
