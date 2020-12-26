using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindMatches : MonoBehaviour
{private GridManager grid;
    public List<GameObject> currentMatches = new List<GameObject>();

	// Use this for initialization
	void Start () {
        grid = FindObjectOfType<GridManager>();
	}

    public void FindAllMatches(){
        StartCoroutine(FindAllMatchesCo());
    }

    private void AddToListAndMatch(GameObject sprite){
        if (!currentMatches.Contains(sprite))
        {
            currentMatches.Add(sprite);
        }
        sprite.GetComponent<Sprite>().isMatched = true;
    }

    private void GetNearbyPieces(GameObject sprite1, GameObject sprite2, GameObject sprite3){
        AddToListAndMatch(sprite1);
        AddToListAndMatch(sprite2);
        AddToListAndMatch(sprite3);
    }

    private IEnumerator FindAllMatchesCo(){
        yield return new WaitForSeconds(.2f);
        for (int i = 0; i < grid.width; i ++){
            for (int j = 0; j < grid.height; j ++){
                GameObject currentSprite = grid.allSprites[i, j];
                Sprite currentSpriteSprite = currentSprite.GetComponent<Sprite>();
                if(currentSprite != null){
                    if(i > 0 && i < grid.width - 1){
                        GameObject leftSprite = grid.allSprites[i - 1, j];
                        Sprite leftSpriteSprite = leftSprite.GetComponent<Sprite>();
                        GameObject rightSprite = grid.allSprites[i + 1, j];
                        Sprite rightSpriteSprite = rightSprite.GetComponent<Sprite>();
                        if(leftSprite != null && rightSprite != null){
                            if(leftSprite.tag == currentSprite.tag && rightSprite.tag == currentSprite.tag){

                                //.Union(IsRowBomb(leftSpriteSprite, currentSpriteSprite, rightSpriteSprite));

                                //currentMatches.Union(IsColumnBomb(leftSpriteSprite, currentSpriteSprite, rightSpriteSprite));

                                GetNearbyPieces(leftSprite, currentSprite, rightSprite);

                            }
                        }
                    }

                    if (j > 0 && j < grid.height - 1)
                    {
                        GameObject upSprite = grid.allSprites[i, j + 1];
                        Sprite upSpriteSprite = upSprite.GetComponent<Sprite>();
                        GameObject downSprite = grid.allSprites[i, j - 1];
                        Sprite downSpriteSprite = downSprite.GetComponent<Sprite>();
                        if (upSprite != null && downSprite != null)
                        {
                            if (upSprite.tag == currentSprite.tag && downSprite.tag == currentSprite.tag)
                            {

                                //currentMatches.Union(IsColumnBomb(upSpriteSprite, currentSpriteSprite, downSpriteSprite));

                                //currentMatches.Union(IsRowBomb(upSpriteSprite, currentSpriteSprite, downSpriteSprite));

                                GetNearbyPieces(upSprite, currentSprite, downSprite);

                            }
                        }
                    }

                }
            }
        }

    }

    public void MatchPiecesOfColor(string color){
        for (int i = 0; i < grid.width; i ++){
            for (int j = 0; j < grid.height; j ++){
                //Check if that piece exists
                if(grid.allSprites[i, j] != null){
                    //Check the tag on that Sprite
                    if(grid.allSprites[i, j].tag == color){
                        //Set that Sprite to be matched
                        grid.allSprites[i, j].GetComponent<Sprite>().isMatched = true;
                    }
                }
            }
        }
    }

    List<GameObject> GetColumnPieces(int column){
        List<GameObject> sprites = new List<GameObject>();
        for (int i = 0; i < grid.height; i ++){
            if(grid.allSprites[column, i]!= null){
                sprites.Add(grid.allSprites[column, i]);
                grid.allSprites[column, i].GetComponent<Sprite>().isMatched = true;
            }
        }
        return sprites;
    }

    List<GameObject> GetRowPieces(int row)
    {
        List<GameObject> sprites = new List<GameObject>();
        for (int i = 0; i < grid.width; i++)
        {
            if (grid.allSprites[i, row] != null)
            {
                sprites.Add(grid.allSprites[i, row]);
                grid.allSprites[i, row].GetComponent<Sprite>().isMatched = true;
            }
        }
        return sprites;
    }

}
