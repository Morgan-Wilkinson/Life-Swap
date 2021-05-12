using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FindMatches : MonoBehaviour
{
    private GridManager grid;
    public UnityEngine.Sprite[] OriginalSprites;
    public UnityEngine.Sprite[] ArrowSprites;
    public UnityEngine.Sprite[] BombSprites;
    public UnityEngine.Sprite[] MultiBombSprites;

    // Start is called before the first frame update
    void Start(){
        grid = FindObjectOfType<GridManager>();
    }


    // Based of Fisher–Yates shuffle algorthim.
    public void ShuffleNoMatches(){
        for (int i = 0; i < grid.allSpritesMatrix.Length; i++ )
        {
            if(grid.allSpritesMatrix[i].GetComponent<Sprite>().isBreakable == false)
            {
                int rando = Random.Range(i, grid.allSpritesMatrix.Length);
                while(grid.allSpritesMatrix[rando].GetComponent<Sprite>().isBreakable == true)
                {
                    rando = Random.Range(i, grid.allSpritesMatrix.Length);
                }
                GameObject temp = grid.allSpritesMatrix[i];
                grid.allSpritesMatrix[i] = grid.allSpritesMatrix[rando];
                grid.allSpritesMatrix[rando] = temp;

                // index i
                SpriteVariablesReorderSetup(i);
                SpriteVariablesReorderSetup(rando);
            }
        }

        FindAllMatchesAdj();
    }

    public void SpriteVariablesReorderSetup(int index){
        int height = grid.height;
        int column = index % height;
        int row = (index - column) / height;
        Sprite sprite = grid.allSpritesMatrix[index].GetComponent<Sprite>();
        sprite.index = index;
        sprite.row = row;
        sprite.column = column;
        grid.allSpritesMatrix[index].name = "(" + row + "," + column + ")";
    }

    // A Breath First search implementation that keeps track of all possible matching sprites. If the first
    // few sprites in this list is gone then that means we should recalculate the array. If empty
    // then we should shuffle the board.
    public void FindAllMatchesAdj(){
        bool[] visited = new bool[grid.vertices];

        ResetSprites();
        // Clear any left over objects
        grid.allMatches.Clear();
        // Create a queue
        Queue<int> queue = new Queue<int>();
        for(int i = 0; i < grid.vertices; i++)
        {
            if(visited[i] == false) 
            {
                queue.Enqueue(i);
                List<int> specialSprite = new List<int>();

                // When count i empty then that would be a new section
                while (queue.Count > 0)
                {
                    int node = queue.Dequeue();
                    visited[node] = true;
                    List<int> list = grid.spritesAdjacencyList[node];

                    foreach(int spriteIndex in list)
                    {

                        //visited[spriteIndex] = true;
                        if(grid.allSpritesMatrix[spriteIndex] != null && (grid.allSpritesMatrix[node].GetComponent<Sprite>().isBreakable == false && 
                        grid.allSpritesMatrix[node].tag == grid.allSpritesMatrix[spriteIndex].tag) && visited[spriteIndex] == false)
                        {
                            queue.Enqueue(spriteIndex);
                            grid.allMatches.Add(node);
                            grid.allMatches.Add(spriteIndex);

                            specialSprite.Add(node);
                            specialSprite.Add(spriteIndex);
                        }
                    }
                }
                if(specialSprite.Count >= grid.gridDimensions.arrow && specialSprite.Count < grid.gridDimensions.bomb)
                {
                    ArrowReplacement(grid.allSpritesMatrix[specialSprite[0]].tag, specialSprite);
                }
                else if(specialSprite.Count >= grid.gridDimensions.bomb && specialSprite.Count < grid.gridDimensions.multiBomb)
                {
                    BombReplacement(grid.allSpritesMatrix[specialSprite[0]].tag, specialSprite);
                }
                else if(specialSprite.Count >= grid.gridDimensions.multiBomb)
                {
                    MultiBomb(grid.allSpritesMatrix[specialSprite[0]].tag, specialSprite);
                }
                specialSprite.Clear();
            }
        }

        if(!grid.allMatches.Any())
        {
            ShuffleNoMatches();
        }
    }

    public void BombReplacement(string tag, List<int> sprites){
        switch(tag)
        {
            case "1":
                foreach(int index in sprites)
                {
                    grid.allSpritesMatrix[index].GetComponent<SpriteRenderer>().sprite = BombSprites[0];
                }
                break;
            case "2":
                foreach(int index in sprites)
                {
                    grid.allSpritesMatrix[index].GetComponent<SpriteRenderer>().sprite = BombSprites[1];
                }
                break;
            case "3":
                foreach(int index in sprites)
                {
                    grid.allSpritesMatrix[index].GetComponent<SpriteRenderer>().sprite = BombSprites[2];
                }
                break;
            case "4":
                foreach(int index in sprites)
                {
                    grid.allSpritesMatrix[index].GetComponent<SpriteRenderer>().sprite = BombSprites[3];
                }
                break;
            case "5":
                foreach(int index in sprites)
                {
                    grid.allSpritesMatrix[index].GetComponent<SpriteRenderer>().sprite = BombSprites[4];
                }
                break;
            case "6":
                foreach(int index in sprites)
                {
                    grid.allSpritesMatrix[index].GetComponent<SpriteRenderer>().sprite = BombSprites[5];
                }
                break;
            case "7":
                foreach(int index in sprites)
                {
                    grid.allSpritesMatrix[index].GetComponent<SpriteRenderer>().sprite = BombSprites[6];
                }
                break;
            case "8":
                foreach(int index in sprites)
                {
                    grid.allSpritesMatrix[index].GetComponent<SpriteRenderer>().sprite = BombSprites[7];
                }
                break;
            
        }
    }

    public void ArrowReplacement(string tag, List<int> sprites){
        switch(tag)
        {
            case "1":
                foreach(int index in sprites)
                {
                    grid.allSpritesMatrix[index].GetComponent<SpriteRenderer>().sprite = ArrowSprites[0];
                }
                break;
            case "2":
                foreach(int index in sprites)
                {
                    grid.allSpritesMatrix[index].GetComponent<SpriteRenderer>().sprite = ArrowSprites[1];
                }
                break;
            case "3":
                foreach(int index in sprites)
                {
                    grid.allSpritesMatrix[index].GetComponent<SpriteRenderer>().sprite = ArrowSprites[2];
                }
                break;
            case "4":
                foreach(int index in sprites)
                {
                    grid.allSpritesMatrix[index].GetComponent<SpriteRenderer>().sprite = ArrowSprites[3];
                }
                break;
            case "5":
                foreach(int index in sprites)
                {
                    grid.allSpritesMatrix[index].GetComponent<SpriteRenderer>().sprite = ArrowSprites[4];
                }
                break;
            case "6":
                foreach(int index in sprites)
                {
                    grid.allSpritesMatrix[index].GetComponent<SpriteRenderer>().sprite = ArrowSprites[5];
                }
                break;
            case "7":
                foreach(int index in sprites)
                {
                    grid.allSpritesMatrix[index].GetComponent<SpriteRenderer>().sprite = ArrowSprites[6];
                }
                break;
            case "8":
                foreach(int index in sprites)
                {
                    grid.allSpritesMatrix[index].GetComponent<SpriteRenderer>().sprite = ArrowSprites[7];
                }
                break;
            
        }
    }

    public void MultiBomb(string tag, List<int> sprites){
        switch(tag)
        {
            case "1":
                foreach(int index in sprites)
                {
                    grid.allSpritesMatrix[index].GetComponent<SpriteRenderer>().sprite = MultiBombSprites[0];
                }
                break;
            case "2":
                foreach(int index in sprites)
                {
                    grid.allSpritesMatrix[index].GetComponent<SpriteRenderer>().sprite = MultiBombSprites[1];
                }
                break;
            case "3":
                foreach(int index in sprites)
                {
                    grid.allSpritesMatrix[index].GetComponent<SpriteRenderer>().sprite = MultiBombSprites[2];
                }
                break;
            case "4":
                foreach(int index in sprites)
                {
                    grid.allSpritesMatrix[index].GetComponent<SpriteRenderer>().sprite = MultiBombSprites[3];
                }
                break;
            case "5":
                foreach(int index in sprites)
                {
                    grid.allSpritesMatrix[index].GetComponent<SpriteRenderer>().sprite = MultiBombSprites[4];
                }
                break;
            case "6":
                foreach(int index in sprites)
                {
                    grid.allSpritesMatrix[index].GetComponent<SpriteRenderer>().sprite = MultiBombSprites[5];
                }
                break;
            case "7":
                foreach(int index in sprites)
                {
                    grid.allSpritesMatrix[index].GetComponent<SpriteRenderer>().sprite = MultiBombSprites[6];
                }
                break;
            case "8":
                foreach(int index in sprites)
                {
                    grid.allSpritesMatrix[index].GetComponent<SpriteRenderer>().sprite = MultiBombSprites[7];
                }
                break;
            
        }
    }

    public void ResetSprites()
    {
        foreach(GameObject sprite in grid.allSpritesMatrix)
        {
            if(!sprite.GetComponent<Sprite>().isBreakable)
            {
                switch(sprite.tag)
                {
                    case "1":
                        sprite.GetComponent<SpriteRenderer>().sprite = OriginalSprites[0];
                        break;
                    case "2":
                        sprite.GetComponent<SpriteRenderer>().sprite = OriginalSprites[1];
                        break;
                    case "3":
                        sprite.GetComponent<SpriteRenderer>().sprite = OriginalSprites[2];
                        break;
                    case "4":
                        sprite.GetComponent<SpriteRenderer>().sprite = OriginalSprites[3];
                        break;
                    case "5":
                        sprite.GetComponent<SpriteRenderer>().sprite = OriginalSprites[4];
                        break;
                    case "6":
                        sprite.GetComponent<SpriteRenderer>().sprite = OriginalSprites[5];
                        break;
                    case "7":
                        sprite.GetComponent<SpriteRenderer>().sprite = OriginalSprites[6];
                        break;
                    case "8":
                        sprite.GetComponent<SpriteRenderer>().sprite = OriginalSprites[7];
                        break;
                    
                }
            }
        }
    }
}
