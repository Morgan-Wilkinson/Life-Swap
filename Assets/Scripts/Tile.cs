using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
   private static Tile selected; // 1 
    private SpriteRenderer Renderer; // 2
    public Vector2Int Position;
    
    private void Start() // 3
    {
        Renderer = GetComponent<SpriteRenderer>();
    }

    public void Select() // 4
    {
        Renderer.color = Color.grey;
    }

    public void Unselect() // 5 
    {
        Renderer.color = Color.white;
    }

    private void OnMouseDown() //6
    {
        if (selected != null)
        {
            selected.Unselect();
        }
        selected = this;
        Select();
    }
}
