using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sprite : MonoBehaviour
{
    [Header("Grid Variables")]
    public int column;
    public int row;
    public int index;
    public bool isMatched = false;
    private bool visited = false;
    //private FindMatches findMatches;
    private GridManager grid;
    private Vector2 tempPosition;

    // Start is called before the first frame update
    void Start()
    {
        grid = FindObjectOfType<GridManager>();
        row = (int)(transform.position.x);
        column = (int)(transform.position.y); 
        index = (row * grid.height) + column;
    }

    // Update is called once per frame
    void Update()
    {
        tempPosition = new Vector2(row, column);
        transform.position = Vector2.Lerp(transform.position, tempPosition, .6f);
    }

    private void OnMouseDown()
    {
        // On press check if sprite match anything on left and right then start Coroutine
        //FindMatches();
        //BFSMatchedTiles(grid.allSprites[column, row]);
        //grid.DestroyMatches();
        //StartCoroutine(CheckMoveCo());
    }
/*
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

    List<GameObject> FindColumnMatchForTile(int col, int row, GameObject sprite)
    {
        List<GameObject> result = new List<GameObject>();
        for (int i = col + 1; i < grid.width; i++)
        {
            GameObject nextColumn = grid.allSprites[i, row];
            if (nextColumn.tag != sprite.tag)
            {
                break;
            }
            result.Add(nextColumn);
        }
        return result;
    }

    List<GameObject> FindRowMatchForTile(int col, int row, GameObject sprite)
    {
        List<GameObject> result = new List<GameObject>();
        for (int i = row + 1; i < grid.height; i++)
        {
            GameObject nextRow = grid.allSprites[col, i];
            if (nextRow.tag != sprite.tag)
            {
                break;
            }
            result.Add(nextRow);
        }
        return result;
    }

    public void BFSMatchedTiles(GameObject sprite)
    {
        // Create a queue
		Queue<GameObject> q = new Queue<GameObject>();
		//q.Enqueue(grid.allSprites[row, column]);
		q.Enqueue(sprite);

		while (q.Count > 0)
		{
			GameObject node = q.Dequeue();
			
            if (node.tag == sprite.tag && node.GetComponent<Sprite>().isMatched == false)
            {
                node.GetComponent<Sprite>().isMatched = true;
                //q.Enqueue(grid.allSprites[i, j]);
            }
            
            // Can create 4 for loops that check the up, down, right and left positions
            // and break on none match
		}
    }
*/
    public void BFSMatchedTiles(GameObject sprite)
    {
        // Create a queue
        Queue<GameObject> q = new Queue<GameObject>();
        //q.Enqueue(grid.allSprites[row, column]);
        q.Enqueue(sprite);

        while (q.Count > 0)
        {
            GameObject node = q.Dequeue();
            
            if (node.tag == sprite.tag && node.GetComponent<Sprite>().isMatched == false)
            {
                node.GetComponent<Sprite>().isMatched = true;
                //q.Enqueue(grid.allSprites[i, j]);
            }
            
            // Can create 4 for loops that check the up, down, right and left positions
            // and break on none match
        }
    }
}
