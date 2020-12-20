using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private static Tile selected;
    private SpriteRenderer Renderer;

    public Vector2Int Position;
    Animator animator;

    private void Start()
    {
        Renderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    public void Select()
    {
        Renderer.color = Color.grey;
    }

    public void Unselect()
    {
        Renderer.color = Color.white;
    }

    private void OnMouseDown()
    {
        if (selected != null)
        {
            if (selected == this)
                return;
            selected.Unselect();
            //animator.SetTrigger("NoMatch");
        
            
            //Debug.Log("A = " + selected.Position + " B = " + Position + " Distance = " + Vector2Int.Distance(selected.Position, Position));
            if (Vector2Int.Distance(selected.Position, Position) == 1)
            {
                // No more swapping.
                //GridManager.Instance.SwapTiles(Position, selected.Position);
                GridManager2.Instance.MatchTiles(selected.Position);
                selected = null;
            } else {
                Debug.Log("No Match");
                selected = this;
                Select();
            }
            
            //GridManager2.Instance.MatchTiles(selected.Position);
        } else {
            Debug.Log("First select");
            selected = this;
            Select();
        }
    }
}