﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FindMatches : MonoBehaviour
{
    private GridManager grid;
    public GameObject ArrowPrefab;
    public GameObject BombPrefab;
    public GameObject MultiBombPrefab;
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
                    List<int> list = grid.spritesAdjacencyList[node];

                    foreach(int spriteIndex in list)
                    {
                        if((grid.allSpritesMatrix[node].GetComponent<Sprite>().isBreakable == false && 
                        grid.allSpritesMatrix[node].tag == grid.allSpritesMatrix[spriteIndex].tag) && visited[spriteIndex] == false)
                        {
                            queue.Enqueue(spriteIndex);
                            visited[spriteIndex] = true;
                            
                            if(!grid.allMatches.Contains(node))
                            {
                                grid.allMatches.Add(node);
                                specialSprite.Add(node);
                            }
                            if(!grid.allMatches.Contains(spriteIndex))
                            {
                                grid.allMatches.Add(spriteIndex);
                                specialSprite.Add(spriteIndex);
                            }
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
            case "0":
                foreach(int index in sprites)
                {
                    grid.allSpritesMatrix[index].GetComponent<SpriteRenderer>().sprite = BombSprites[0];
                }
                break;
            case "1":
                foreach(int index in sprites)
                {
                    grid.allSpritesMatrix[index].GetComponent<SpriteRenderer>().sprite = BombSprites[1];
                }
                break;
            case "2":
                foreach(int index in sprites)
                {
                    grid.allSpritesMatrix[index].GetComponent<SpriteRenderer>().sprite = BombSprites[2];
                }
                break;
            case "3":
                foreach(int index in sprites)
                {
                    grid.allSpritesMatrix[index].GetComponent<SpriteRenderer>().sprite = BombSprites[3];
                }
                break;
            case "4":
                foreach(int index in sprites)
                {
                    grid.allSpritesMatrix[index].GetComponent<SpriteRenderer>().sprite = BombSprites[4];
                }
                break;
            case "5":
                foreach(int index in sprites)
                {
                    grid.allSpritesMatrix[index].GetComponent<SpriteRenderer>().sprite = BombSprites[5];
                }
                break;
            case "6":
                foreach(int index in sprites)
                {
                    grid.allSpritesMatrix[index].GetComponent<SpriteRenderer>().sprite = BombSprites[6];
                }
                break;
            case "7":
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
            case "0":
                foreach(int index in sprites)
                {
                    grid.allSpritesMatrix[index].GetComponent<SpriteRenderer>().sprite = ArrowSprites[0];
                }
                break;
            case "1":
                foreach(int index in sprites)
                {
                    grid.allSpritesMatrix[index].GetComponent<SpriteRenderer>().sprite = ArrowSprites[1];
                }
                break;
            case "2":
                foreach(int index in sprites)
                {
                    grid.allSpritesMatrix[index].GetComponent<SpriteRenderer>().sprite = ArrowSprites[2];
                }
                break;
            case "3":
                foreach(int index in sprites)
                {
                    grid.allSpritesMatrix[index].GetComponent<SpriteRenderer>().sprite = ArrowSprites[3];
                }
                break;
            case "4":
                foreach(int index in sprites)
                {
                    grid.allSpritesMatrix[index].GetComponent<SpriteRenderer>().sprite = ArrowSprites[4];
                }
                break;
            case "5":
                foreach(int index in sprites)
                {
                    grid.allSpritesMatrix[index].GetComponent<SpriteRenderer>().sprite = ArrowSprites[5];
                }
                break;
            case "6":
                foreach(int index in sprites)
                {
                    grid.allSpritesMatrix[index].GetComponent<SpriteRenderer>().sprite = ArrowSprites[6];
                }
                break;
            case "7":
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
            case "0":
                foreach(int index in sprites)
                {
                    grid.allSpritesMatrix[index].GetComponent<SpriteRenderer>().sprite = MultiBombSprites[0];
                }
                break;
            case "1":
                foreach(int index in sprites)
                {
                    grid.allSpritesMatrix[index].GetComponent<SpriteRenderer>().sprite = MultiBombSprites[1];
                }
                break;
            case "2":
                foreach(int index in sprites)
                {
                    grid.allSpritesMatrix[index].GetComponent<SpriteRenderer>().sprite = MultiBombSprites[2];
                }
                break;
            case "3":
                foreach(int index in sprites)
                {
                    grid.allSpritesMatrix[index].GetComponent<SpriteRenderer>().sprite = MultiBombSprites[3];
                }
                break;
            case "4":
                foreach(int index in sprites)
                {
                    grid.allSpritesMatrix[index].GetComponent<SpriteRenderer>().sprite = MultiBombSprites[4];
                }
                break;
            case "5":
                foreach(int index in sprites)
                {
                    grid.allSpritesMatrix[index].GetComponent<SpriteRenderer>().sprite = MultiBombSprites[5];
                }
                break;
            case "6":
                foreach(int index in sprites)
                {
                    grid.allSpritesMatrix[index].GetComponent<SpriteRenderer>().sprite = MultiBombSprites[6];
                }
                break;
            case "7":
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
                    case "0":
                        sprite.GetComponent<SpriteRenderer>().sprite = OriginalSprites[0];
                        break;
                    case "1":
                        sprite.GetComponent<SpriteRenderer>().sprite = OriginalSprites[1];
                        break;
                    case "2":
                        sprite.GetComponent<SpriteRenderer>().sprite = OriginalSprites[2];
                        break;
                    case "3":
                        sprite.GetComponent<SpriteRenderer>().sprite = OriginalSprites[3];
                        break;
                    case "4":
                        sprite.GetComponent<SpriteRenderer>().sprite = OriginalSprites[4];
                        break;
                    case "5":
                        sprite.GetComponent<SpriteRenderer>().sprite = OriginalSprites[5];
                        break;
                    case "6":
                        sprite.GetComponent<SpriteRenderer>().sprite = OriginalSprites[6];
                        break;
                    case "7":
                        sprite.GetComponent<SpriteRenderer>().sprite = OriginalSprites[7];
                        break;
                    
                }
            }
        }
    }

    // A Breath First implementation of search for the matching sprites
    public void BFSMatchedTiles(GameObject sprite){
        // For special Sprite if needed.
        int row = 0;
        int column = 0;
        int index = 0;

        bool[] visited = new bool[grid.vertices];
        bool matches = false;

        if(sprite.tag != "Arrow" && sprite.tag != "Bomb" && sprite.tag != "MultiBomb")
        {
            // Create a queue
            Queue<int> q = new Queue<int>();
            q.Enqueue(sprite.GetComponent<Sprite>().index);
            
            // For bombs or arrows
            List<int> specialSprite = new List<int>();

            while (q.Count > 0)
            {
                int node = q.Dequeue();
                
                List<int> list = grid.spritesAdjacencyList[node];

                foreach(int i in list)
                {
                    if(grid.allSpritesMatrix[i] != null && sprite.tag == grid.allSpritesMatrix[i].tag && grid.allSpritesMatrix[i].GetComponent<Sprite>().isMatched == false)
                    {
                        visited[i] = true;
                        matches = true;
                        grid.allSpritesMatrix[i].GetComponent<Sprite>().isMatched = true;
                        // Gets the column of the index
                        grid.nullSpriteArray[i / grid.height]++;

                        specialSprite.Add(i);
                        q.Enqueue(i);
                    }

                    else if(matches && grid.allSpritesMatrix[i].GetComponent<Sprite>().isBreakable && visited[i] == false)
                    {
                        visited[i] = true;
                        Sprite spriteI = grid.allSpritesMatrix[i].GetComponent<Sprite>();
                        spriteI.breakableSpriteProgress++;
                        spriteI.damageProgression += 1;
                        if(spriteI.damageProgression == spriteI.lifeforce)
                        {
                            spriteI.isMatched = true;
                            grid.nullSpriteArray[i / grid.height]++;
                        }
                        else
                        {
                            grid.allSpritesMatrix[i].GetComponent<SpriteRenderer>().sprite = grid.BreakableSprites[0][spriteI.damageProgression];
                        }
                    }
                }
            }
            
            if(matches){
                if(specialSprite.Count >= grid.gridDimensions.arrow)
                {
                    Sprite spriteInfo = sprite.GetComponent<Sprite>();
                    row = spriteInfo.row;
                    column = spriteInfo.column;
                    index = spriteInfo.index;
                }

                grid.DestroyMatches();
            }
            else{
                grid.currentState = GameState.move;
                // Sprites shake;
            }

            if(specialSprite.Count >= grid.gridDimensions.arrow && specialSprite.Count < grid.gridDimensions.bomb)
            {
                MakeSpecialSprite(ArrowPrefab, row, column, index);
            }
            else if(specialSprite.Count >= grid.gridDimensions.bomb && specialSprite.Count < grid.gridDimensions.multiBomb)
            {
                MakeSpecialSprite(BombPrefab, row, column, index);
            }
            else if(specialSprite.Count >= grid.gridDimensions.multiBomb)
            {
                MakeSpecialSprite(MultiBombPrefab, row, column, index);
            }
            specialSprite.Clear();
        }

        else if(sprite.tag == "Arrow")
        {
            ArrowActivation(sprite);
        }

        else if(sprite.tag == "Bomb")
        {
            BombActivation(sprite);
        }

        // Removes all Sprites
        else if(sprite.tag == "MultiBomb")
        {
           MultiBombActivation();
        }
    }

    public void MakeSpecialSprite(GameObject prefab, int row, int column, int index){
        Vector2 position = new Vector2(row, column);
        GameObject arrow = Instantiate(prefab, position, Quaternion.identity, this.transform);
        Sprite sprite = arrow.GetComponent<Sprite>();
        sprite.isArrow = true;
        sprite.row = row;
        sprite.column = column;
        grid.allSpritesMatrix[index] = arrow;
       //arrow.transform.parent = this.transform;
    }

    public void ArrowActivation(GameObject sprite)
    {
        Sprite spriteInfo = sprite.GetComponent<Sprite>();
        int row = spriteInfo.row;
        int column = spriteInfo.column;
        int index = spriteInfo.index;

        int startXPosition = index - (row * grid.height);
        int startYPosition = index - column;
        int endYPosition = startYPosition + grid.width;
        // Column Deletion
        for(int i = startXPosition; i < grid.vertices; i += grid.height)
        {
            grid.allSpritesMatrix[i].GetComponent<Sprite>().isMatched = true;
            grid.nullSpriteArray[i / grid.height]++;
        }

        for(int i = startYPosition; i <= endYPosition; i++)
        {
            grid.allSpritesMatrix[i].GetComponent<Sprite>().isMatched = true;
            grid.nullSpriteArray[i / grid.height]++;
        }
        grid.DestroyMatches();
    }
    public void BombActivation(GameObject sprite)
    {
        Sprite spriteInfo = sprite.GetComponent<Sprite>();
        int row = spriteInfo.row;
        int column = spriteInfo.column;
        int index = spriteInfo.index;

        int newRow = 0;
        int newColumn = 0;
        int currentSprite = 0;

        // Original Sprite
        grid.allSpritesMatrix[index].GetComponent<Sprite>().isMatched = true;
        grid.nullSpriteArray[index / grid.height]++;

        foreach(int i in grid.spritesAdjacencyList[index])
        {
            grid.allSpritesMatrix[i].GetComponent<Sprite>().isMatched = true;
            grid.nullSpriteArray[i / grid.height]++;
        }
        
        // Left Up
        newRow = row - 1;
        newColumn = column + 1;
        currentSprite = (newRow * grid.height) + newColumn;
        if (currentSprite >= 0 && currentSprite < grid.vertices && newColumn < grid.height && newRow >= 0)
        {
            grid.allSpritesMatrix[currentSprite].GetComponent<Sprite>().isMatched = true;
            grid.nullSpriteArray[currentSprite / grid.height]++;
        }

        // Left Down
        newRow = row - 1;
        newColumn = column - 1;
        currentSprite = (newRow * grid.height) + newColumn;
        if (currentSprite >= 0 && currentSprite < grid.vertices && newColumn >= 0 && newRow >= 0) 
        {
            grid.allSpritesMatrix[currentSprite].GetComponent<Sprite>().isMatched = true;
            grid.nullSpriteArray[currentSprite / grid.height]++;
        }

        // Right Up
        newRow = row + 1;
        newColumn = column + 1;
        currentSprite = (newRow * grid.height) + newColumn;
        if (currentSprite >= 0 && currentSprite < grid.vertices && newColumn < grid.height && newRow < grid.width)
        {
            grid.allSpritesMatrix[currentSprite].GetComponent<Sprite>().isMatched = true;
            grid.nullSpriteArray[currentSprite / grid.height]++;
        }

        // Right Down
        newRow = row + 1;
        newColumn = column - 1;
        currentSprite = (newRow * grid.height) + newColumn;
        if (currentSprite >= 0 && currentSprite < grid.vertices && newColumn >= 0 && newRow < grid.width)
        {
            grid.allSpritesMatrix[currentSprite].GetComponent<Sprite>().isMatched = true;
            grid.nullSpriteArray[currentSprite / grid.height]++;
        }

        grid.DestroyMatches();
    }
    public void MultiBombActivation()
    {
        for(int i = 0; i < grid.vertices; i++)
        {
            grid.allSpritesMatrix[i].GetComponent<Sprite>().isMatched = true;
            grid.nullSpriteArray[i / grid.height]++;
        }

        grid.DestroyMatches();
    }
}
