using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sprite : MonoBehaviour
{
     // Private variables.
    private GridManager grid;
    private FindMatches findMatches;
  
    [Header("Sprite Variables")]
    public int row;
    public int column;
    public int index;
    private Vector2 tempPosition;

    [Header("Special Bomb Type")]
    public bool isArrow = false;
    public bool isBomb = false;
    public bool isMultiBomb = false;

    [Header("Breakable Sprite Variables")]
    public int BreakableSpriteType;
    public int breakableSpriteProgress;
    public bool isBreakable;
    public int lifeforce;
    public int damageProgression = 0;

    [Header("Life Cycle Variables")]
    public bool isMatched = false;

    // Start is called before the first frame update
    void Start(){
        grid = FindObjectOfType<GridManager>();
        findMatches = FindObjectOfType<FindMatches>();
    }

    // Update is called once per frame
    void Update(){
        tempPosition = new Vector2(row, column);
        transform.position = Vector2.Lerp(transform.position, tempPosition, 0.25f);
        index = (row * grid.height) + column;
    }

    // Breakable Sprite Functions
    public void BreakableSpriteSetup(int BreakableSpriteType, int lifeforce)
    {
        this.BreakableSpriteType = BreakableSpriteType;
        breakableSpriteProgress = 0;
        isBreakable = true;
        this.lifeforce = lifeforce;
    }

    private void OnMouseDown(){
        if(grid.currentState == GameState.move) {
            if(isBreakable)
            {
                // Shake or some other action
            }
            else{
                grid.currentState = GameState.wait;
                findMatches.BFSMatchedTiles(grid.allSpritesMatrix[index]);
                // if(findMatches.BFSMatchedTiles(grid.allSpritesMatrix[index])){
                //     grid.DestroyMatches();
                // }
                // else{
                //     grid.currentState = GameState.move;
                //     // Sprites shake;
                // }
            }
        }
    }
}
