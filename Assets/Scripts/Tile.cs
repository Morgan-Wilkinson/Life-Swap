using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private static Tile selected;
    private SpriteRenderer Renderer;

    public Vector2Int Position;
    public int column;
    public int row;
    
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
        Debug.Log("Selected 1");
        selected = this;
        Select();
        //GridManager2.Instance.MatchTiles(selected.Position);
        Unselect();
        selected = null;
    }
}