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

    public Vector2 Position;  
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
    }

    // Update is called once per frame
    void Update()
    {
        FindMatches();
        if(isMatched){
            
            SpriteRenderer mySprite = GetComponent<SpriteRenderer>();
            Color currentColor = mySprite.color;
            mySprite.color = new Color(currentColor.r, currentColor.g, currentColor.b, .5f);
        }
    }

    private void OnMouseDown()
    {
        Position = new Vector2(transform.position.x, transform.position.y);
        //Debug.Log(Position.x);
        
    }

    void FindMatches(){
        column = (int)(Position.x);
        row = (int)(Position.y);
        if(column > 0 && column < grid.width - 1)
        {
            GameObject leftSprite1 = grid.allSprites[column - 1, row];
            GameObject rightSprite1 = grid.allSprites[column + 1, row];
            if (leftSprite1 != null && rightSprite1 != null)
            {
                if (leftSprite1.tag == this.gameObject.tag && rightSprite1.tag == this.gameObject.tag)
                {
                    leftSprite1.GetComponent<Sprite>().isMatched = true;
                    rightSprite1.GetComponent<Sprite>().isMatched = true;
                    isMatched = true;
                }
            }
        }
        if (row > 0 && row < grid.height - 1)
        {
            GameObject upSprite1 = grid.allSprites[column, row + 1];
            GameObject downSprite1 = grid.allSprites[column, row - 1];
            if (upSprite1 != null && downSprite1 != null)
            {
                if (upSprite1.tag == this.gameObject.tag && downSprite1.tag == this.gameObject.tag)
                {
                    upSprite1.GetComponent<Sprite>().isMatched = true;
                    downSprite1.GetComponent<Sprite>().isMatched = true;
                    isMatched = true;
                }
            }
        }
    }
}
