using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakables : MonoBehaviour
{
    [Header("Breakables Variables")]
    public int row;
    public int column;
    public int index;
    public int hitPoints = 2;
    public bool adjacentMatched = false;
    
    // Private variables.
    private GridManager grid;
    private Vector2 tempPosition;

    // Start is called before the first frame update
    void Start(){
        grid = FindObjectOfType<GridManager>();
    }

    // Update is called once per frame
    void Update(){
        tempPosition = new Vector2(row, column);
        transform.position = Vector2.Lerp(transform.position, tempPosition, 0.2f);
        index = (row * grid.height) + column;
    }

    

}
