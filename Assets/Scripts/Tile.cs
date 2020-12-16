using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private static Tile selected;
    private SpriteRenderer Renderer;

    public Vector2Int Position;

    private void Start()
    {
        Renderer = GetComponent<SpriteRenderer>();
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
        
            Debug.Log("A = " + selected.Position + " B = " + Position + " Distance = " + Vector2Int.Distance(selected.Position, Position));
            if (Vector2Int.Distance(selected.Position, Position) == 1)
            {
                GridManager.Instance.SwapTiles(Position, selected.Position);
                //Debug.Log("Tile Pressed");
                selected = null;
            } else {
                //Debug.Log("Tile");
                selected = this;
                Select();
            }
        } else {
            //Debug.Log("gnfg");
            selected = this;
            Select();
        }
    }
}