using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sprite : MonoBehaviour
{
    [Header("Sprite Variables")]
    public int row;
    public int column;
    public int index;
    public bool isMatched = false;
    
    // Private variables.
    private GridManager grid;
    private Vector2 tempPosition;

    // Start is called before the first frame update
    void Start(){
        grid = FindObjectOfType<GridManager>();
    }

    // Update is called once per frame
    void Update(){
        tempPosition = new Vector2(row, column);
        transform.position = Vector2.Lerp(transform.position, tempPosition, 0.2f);
        index = (row * grid.height) + column;
    }

    private void OnMouseDown(){
        if(grid.currentState == GameState.move) {
            grid.currentState = GameState.wait;
            if(BFSMatchedTiles(grid.allSpritesMatrix[index])){
                grid.DestroyMatches();
            }
            else{
                grid.currentState = GameState.move;
                // Sprites shake;
            }
        }
    }

    // A Breath First implementation of search for the matching sprites
    public bool BFSMatchedTiles(GameObject sprite){
        bool matches = false;
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
                    matches = true;
                    grid.allSpritesMatrix[i].GetComponent<Sprite>().isMatched = true;
                    // Gets the column of the index
                    grid.nullSpriteArray[i / grid.height]++; 
                    q.Enqueue(i);
                }
            }
        }
        return matches;
    }
}
