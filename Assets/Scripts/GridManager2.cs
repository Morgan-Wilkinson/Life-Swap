﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager2 : MonoBehaviour
{
    public GameObject[] Sprites;
    public GameObject TilePrefab;
    public int height = 8; // x
    public int width = 8; // y
    public int offSet = 15;
    public float Distance = 1.0f;
    private GameObject[,] Grid;
    private GameObject[,] allSprites;

    public static GridManager2 Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Grid = new GameObject[width, height];
        allSprites = new GameObject[width, height];
        InitGrid();
    }

    // Change init so that it allows for the creation of multiple 3 or more matches.
    void InitGrid()
    {
        //Vector3 positionOffset = transform.position - new Vector3(GridDimension * Distance / 2.0f, GridDimension * Distance / 2.0f, 0);
        for (int x = 0; x < width; x++) 
            for (int y = 0; y < height; y++) 
            {
                Vector2 tempPosition = new Vector2(x, y + offSet);
                GameObject backgroundTile = Instantiate(TilePrefab, tempPosition, Quaternion.identity) as GameObject;
                backgroundTile.transform.parent = this.transform;
                backgroundTile.name = "(" + x + "," + y + ")";
                
                int spriteToUse = Random.Range(0, Sprites.Length);
                GameObject sprite = Instantiate(Sprites[spriteToUse], tempPosition, Quaternion.identity);
                sprite.transform.parent = this.transform;
                sprite.name = "(" + x + "," + y + ")";
                allSprites[x, y] = sprite;
            }
    }

/*
    public void MatchTiles(Vector2Int tilePosition)
    {
        GameObject tile = Grid[tilePosition.x, tilePosition.y];
        SpriteRenderer renderer = tile.GetComponent<SpriteRenderer>();

        // CheckMatches in y and x.
        // Add those sprite to be deleted.  
        bool changesOccurs = CheckAdjacentMatches(tilePosition);
        if(!changesOccurs)
        {
            // Enable Animator for Shake for this one tile.
            //tile
        }
        else
        {
            do
            {
                FillHoles();
            } while (CheckAdjacentMatches(tilePosition));
        }
    }

    // Checks if the y and x are in legal bounds. If so then it returns the tile
    // GameObject sprite at that location Grid[y, x];
    Sprite GetSpriteAt(int y, int x)
    {
        if (y < 0 || y >= width
            || x < 0 || x >= height)
            return null;
        GameObject tile = Grid[y, x];
        SpriteRenderer renderer = tile.GetComponent<SpriteRenderer>();
        return renderer.sprite;
    }

    // Checks if the y and x are in legal bounds. If so then it returns the tile
    // GameObject SpriteRenderer at that location Grid[y, x];
    SpriteRenderer GetSpriteRendererAt(int y, int x)
    {
        if (y < 0 || y >= width
            || x < 0 || x >= height)
            return null;
        GameObject tile = Grid[y, x];
        SpriteRenderer renderer = tile.GetComponent<SpriteRenderer>();
        return renderer;
    }

    // Returns a list of SpriteRenderers where the SpriteRenderer all match the initial sprite
    // parameter and are adjacent to each other y wise.
    List<SpriteRenderer> FindYMatchForTile(int col, int x, Sprite sprite)
    {
        List<SpriteRenderer> result = new List<SpriteRenderer>();
        for (int i = col + 1; i < width; i++)
        {
            SpriteRenderer nextY = GetSpriteRendererAt(i, x);
            if (nextY.sprite != sprite)
            {
                break;
            }
            result.Add(nextY);
        }
        for (int i = col - 1; i >= 0; i--)
        {
            SpriteRenderer nextY = GetSpriteRendererAt(i, x);
            if (nextY.sprite != sprite)
            {
                break;
            }
            result.Add(nextY);
        }
        return result;
    }

    // Returns a list of SpriteRenderers where the SpriteRenderer all match the initial sprite
    // parameter and are adjacent to each other x wise.
    List<SpriteRenderer> FindXMatchForTile(int col, int x, Sprite sprite)
    {
        List<SpriteRenderer> result = new List<SpriteRenderer>();
        for (int i = x + 1; i < height; i++)
        {
            SpriteRenderer nextX = GetSpriteRendererAt(col, i);
            if (nextX.sprite != sprite)
            {
                break;
            }
            result.Add(nextX);
        }
        for (int i = x - 1; i >= 0; i--)
        {
            SpriteRenderer nextX = GetSpriteRendererAt(col, i);
            if (nextX.sprite != sprite)
            {
                break;
            }
            result.Add(nextX);
        }
        return result;
    }
   
    // Check both the x and ys of the selected position and only stop until
    // A non match is found in that particular direction
    bool CheckAdjacentMatches(Vector2Int tilePosition)
    {
        HashSet<SpriteRenderer> matchedTiles = new HashSet<SpriteRenderer>();
        
        SpriteRenderer current = GetSpriteRendererAt(tilePosition.x, tilePosition.y);

        List<SpriteRenderer> horizontalMatches = FindYMatchForTile(tilePosition.x, tilePosition.y, current.sprite); 
        if (horizontalMatches.Count >= 1)
        {
            matchedTiles.UnionWith(horizontalMatches);
            matchedTiles.Add(current);
        }

        List<SpriteRenderer> verticalMatches = FindXMatchForTile(tilePosition.x, tilePosition.y, current.sprite);
        if (verticalMatches.Count >= 1)
        {
            matchedTiles.UnionWith(verticalMatches);
            matchedTiles.Add(current);
        }

        // Effectively deletes the sprites in the match.
        foreach (SpriteRenderer renderer in matchedTiles)
        {
            renderer.sprite = null;
        }

        return matchedTiles.Count > 0;
    }

    void FillHoles()
    {
        for (int y = 0; y < width; y++)
        {
            for (int x = 0; x < height; x++) // 1
            {
                while (GetSpriteRendererAt(y, x).sprite == null) // 2
                {
                    for (int filler = x; filler < height - 1; filler++) // 3
                    {
                        SpriteRenderer current = GetSpriteRendererAt(y, filler); // 4
                        SpriteRenderer next = GetSpriteRendererAt(y, filler + 1);
                        current.sprite = next.sprite;
                    }
                    SpriteRenderer last = GetSpriteRendererAt(y, height - 1);
                    //last.sprite = Sprites[Random.Range(0, Sprites.Length)]; // 5
                }
            }
        }
    }
    */
}
