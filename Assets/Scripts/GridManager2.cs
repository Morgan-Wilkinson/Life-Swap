using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager2 : MonoBehaviour
{
    public List<Sprite> Sprites = new List<Sprite>();
    public GameObject TilePrefab;
    public int GridDimension = 8;
    public float Distance = 1.0f;
    private GameObject[,] Grid;

    public static GridManager2 Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Grid = new GameObject[GridDimension, GridDimension];
        InitGrid();
    }

    // Change init so that it allows for the creation of multiple 3 or more matches.
    void InitGrid()
    {
        Vector3 positionOffset = transform.position - new Vector3(GridDimension * Distance / 2.0f, GridDimension * Distance / 2.0f, 0);

        for (int row = 0; row < GridDimension; row++)
            for (int column = 0; column < GridDimension; column++)
            {
                GameObject newTile = Instantiate(TilePrefab);
                newTile.name = "(" + row + "," + column + ")";

                List<Sprite> possibleSprites = new List<Sprite>(Sprites);

                 /* This sections prevents the creation of 3 in a row. Change this to force 3 or more in a row.
                //Choose what sprite to use for this cell
                Sprite left1 = GetSpriteAt(column - 1, row);
                Sprite left2 = GetSpriteAt(column - 2, row);
                if (left2 != null && left1 == left2)
                {
                    possibleSprites.Remove(left1);
                }

                Sprite down1 = GetSpriteAt(column, row - 1);
                Sprite down2 = GetSpriteAt(column, row - 2);
                if (down2 != null && down1 == down2)
                {
                    possibleSprites.Remove(down1);
                }
                */

                SpriteRenderer renderer = newTile.GetComponent<SpriteRenderer>();
                renderer.sprite = possibleSprites[Random.Range(0, possibleSprites.Count)];

                newTile.GetComponent<Tile>().Position = new Vector2Int(column, row);
                newTile.transform.parent = transform;
                newTile.transform.position = new Vector3(column * Distance, row * Distance, 0) + positionOffset;
                
                Grid[column, row] = newTile;
            }
    }

    public void MatchTiles(Vector2Int tilePosition)
    {
        GameObject tile = Grid[tilePosition.x, tilePosition.y];
        SpriteRenderer renderer = tile.GetComponent<SpriteRenderer>();

        // CheckMatches in column and row.
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

    // Checks if the column and row are in legal bounds. If so then it returns the tile
    // GameObject sprite at that location Grid[column, row];
    Sprite GetSpriteAt(int column, int row)
    {
        if (column < 0 || column >= GridDimension
            || row < 0 || row >= GridDimension)
            return null;
        GameObject tile = Grid[column, row];
        SpriteRenderer renderer = tile.GetComponent<SpriteRenderer>();
        return renderer.sprite;
    }

    // Checks if the column and row are in legal bounds. If so then it returns the tile
    // GameObject SpriteRenderer at that location Grid[column, row];
    SpriteRenderer GetSpriteRendererAt(int column, int row)
    {
        if (column < 0 || column >= GridDimension
            || row < 0 || row >= GridDimension)
            return null;
        GameObject tile = Grid[column, row];
        SpriteRenderer renderer = tile.GetComponent<SpriteRenderer>();
        return renderer;
    }

    // Returns a list of SpriteRenderers where the SpriteRenderer all match the initial sprite
    // parameter and are adjacent to each other column wise.
    List<SpriteRenderer> FindColumnMatchForTile(int col, int row, Sprite sprite)
    {
        List<SpriteRenderer> result = new List<SpriteRenderer>();
        for (int i = col + 1; i < GridDimension; i++)
        {
            SpriteRenderer nextColumn = GetSpriteRendererAt(i, row);
            if (nextColumn.sprite != sprite)
            {
                break;
            }
            result.Add(nextColumn);
        }
        for (int i = col - 1; i >= 0; i--)
        {
            SpriteRenderer nextColumn = GetSpriteRendererAt(i, row);
            if (nextColumn.sprite != sprite)
            {
                break;
            }
            result.Add(nextColumn);
        }
        return result;
    }

    // Returns a list of SpriteRenderers where the SpriteRenderer all match the initial sprite
    // parameter and are adjacent to each other row wise.
    List<SpriteRenderer> FindRowMatchForTile(int col, int row, Sprite sprite)
    {
        List<SpriteRenderer> result = new List<SpriteRenderer>();
        for (int i = row + 1; i < GridDimension; i++)
        {
            SpriteRenderer nextRow = GetSpriteRendererAt(col, i);
            if (nextRow.sprite != sprite)
            {
                break;
            }
            result.Add(nextRow);
        }
        for (int i = row - 1; i >= 0; i--)
        {
            SpriteRenderer nextRow = GetSpriteRendererAt(col, i);
            if (nextRow.sprite != sprite)
            {
                break;
            }
            result.Add(nextRow);
        }
        return result;
    }
   
    // Check both the row and columns of the selected position and only stop until
    // A non match is found in that particular direction
    bool CheckAdjacentMatches(Vector2Int tilePosition)
    {
        HashSet<SpriteRenderer> matchedTiles = new HashSet<SpriteRenderer>();
        
        SpriteRenderer current = GetSpriteRendererAt(tilePosition.x, tilePosition.y);

        List<SpriteRenderer> horizontalMatches = FindColumnMatchForTile(tilePosition.x, tilePosition.y, current.sprite); 
        if (horizontalMatches.Count >= 1)
        {
            matchedTiles.UnionWith(horizontalMatches);
            matchedTiles.Add(current);
        }

        List<SpriteRenderer> verticalMatches = FindRowMatchForTile(tilePosition.x, tilePosition.y, current.sprite);
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
        for (int column = 0; column < GridDimension; column++)
        {
            for (int row = 0; row < GridDimension; row++) // 1
            {
                while (GetSpriteRendererAt(column, row).sprite == null) // 2
                {
                    for (int filler = row; filler < GridDimension - 1; filler++) // 3
                    {
                        SpriteRenderer current = GetSpriteRendererAt(column, filler); // 4
                        SpriteRenderer next = GetSpriteRendererAt(column, filler + 1);
                        current.sprite = next.sprite;
                    }
                    SpriteRenderer last = GetSpriteRendererAt(column, GridDimension - 1);
                    last.sprite = Sprites[Random.Range(0, Sprites.Count)]; // 5
                }
            }
        }
    }
}
