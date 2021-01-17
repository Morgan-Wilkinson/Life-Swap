using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sprite : MonoBehaviour
{
    [Header("Grid Variables")]
    public int row;
    public int column;
    public int index;
    public bool isMatched = false;
    
    // Private variables.
    private GridManager grid;
    private Vector2 tempPosition;

    // Start is called before the first frame update
    void Start()
    {
        grid = FindObjectOfType<GridManager>();
        //row = (int)(transform.position.x);
        //column = (int)(transform.position.y); 
        index = (row * grid.height) + column;
    }

    // Update is called once per frame
    void Update()
    {
        tempPosition = new Vector2(row, column);
        transform.position = Vector2.Lerp(transform.position, tempPosition, 0.2f);
        index = (row * grid.height) + column;
    }

    private void OnMouseDown()
    {
        BFSMatchedTiles(grid.allSpritesMatrix[index]);
        grid.DestroyMatches();
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
*/
    // A Breath First implementation of search for the matching sprites
    public void BFSMatchedTiles(GameObject sprite)
    {
        // Create a queue
        Queue<int> q = new Queue<int>();
        q.Enqueue(sprite.GetComponent<Sprite>().index);

        while (q.Count > 0)
        {
            int node = q.Dequeue();
            
            List<int> list = grid.spritesAdjacencyList[node];

            foreach(int i in list)
            {
                if(grid.allSpritesMatrix[i] != null && sprite.tag == grid.allSpritesMatrix[i].tag && grid.allSpritesMatrix[i].GetComponent<Sprite>().isMatched == false)
                {
                    grid.allSpritesMatrix[i].GetComponent<Sprite>().isMatched = true;
                    q.Enqueue(i);
                }
            }
        }
    }
}
