﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState {
    wait,
    move
}

public class GridManager : MonoBehaviour
{
    [Header("Board Variables")]
    public GameSettings settings;
    // Height of the grid.
    public int height;
    // Width of the grid.
    public int width;
    // Maximum number of sprites on the grid.
    public int vertices;
    // Height from which the sprites drop in.
    public int offSet;
    
    private int majorAxis;

    public static GridManager Instance { get; private set; }

    [Header("GameObject Storage Lists and Arrays")]
    // Array that holds the types of sprites.
    public GameObject[] Sprites;
    // List of arrays that hold the various paths. An adjacency list.
    public List<int>[] spritesAdjacencyList;
    // An array of all sprites on the board now.
    public GameObject[] allSpritesMatrix;
    // List of arrays that hold the various paths of all possible matches. 
    public List<int>[] allMatches; 
    // An array holding the null positions of the array.
    public int[] nullSpriteArray;

    [Header("Game Progression")]
    public GameState currentState = GameState.move;

    void Awake(){
        Instance = this;
    }

    // Start is called before the first frame update
    void Start(){
        settings = FindObjectOfType<GameSettings>();
        height = settings.gridDimensions.height;
        width = settings.gridDimensions.width;
        offSet = settings.gridDimensions.offSet;
        vertices = width * height;
        majorAxis = height;
        allSpritesMatrix = new GameObject[vertices];
        spritesAdjacencyList = new List<int>[vertices];
        allMatches = new List<int>[vertices];
        nullSpriteArray = new int[width];

        InitGrid();
    }

    // Game Setup
    void InitGrid(){
        for (int i = 0; i < width; i++){
            for(int j = 0; j < height; j++){
                allSpritesMatrix[((i * majorAxis) + j)] = createSprite(i, j);
            }
        }

        adjacencyListBuilder();
        findAllMatches();
    }

    // This function creates the adjacencyList of ints.
    private void adjacencyListBuilder(){
        int index = 0;
        
        for (int i = 0; i < width; i++){
            for(int j = 0; j < height; j++){

                // Setup for determining rows and columns.
                int allSpritesIndex = ((i * majorAxis) + j);
                spritesAdjacencyList[allSpritesIndex] = new List<int>();

                // Down
                index = (((i - 1) * majorAxis) + j);
                if (index >= 0 && index < vertices)
                {
                    spritesAdjacencyList[allSpritesIndex].Add(index);
                }


                // Up
                index = (((i + 1) * majorAxis) + j);
                if (index >= 0 && index < vertices)
                {
                    spritesAdjacencyList[allSpritesIndex].Add(index);
                }

                // Left
                index = ((i * majorAxis) + (j - 1));
                if (index >= 0 && index < vertices && j != 0)
                {
                    spritesAdjacencyList[allSpritesIndex].Add(index);
                }

                // Right
                index = ((i * majorAxis) + (j + 1));
                if (index >= 0 && index < vertices && j != width)
                {
                    spritesAdjacencyList[allSpritesIndex].Add(index);
                }
            }
        }
    }

    // Creation of a sprite at row, column.
    private GameObject createSprite(int row, int column){
        Vector2 tempPosition = new Vector2(row, column);
        int spriteToUse = Random.Range(0, Sprites.Length);
        GameObject sprite = Instantiate(Sprites[spriteToUse], tempPosition, Quaternion.identity);
        sprite.transform.parent = this.transform;
        sprite.name = "(" + row + "," + column + ")";
        sprite.GetComponent<Sprite>().row = row;
        sprite.GetComponent<Sprite>().column = column;
        sprite.GetComponent<Sprite>().index = ((row * majorAxis) + column);
        return sprite;
    }

    // Creation of a sprite at row, column at offset.
    private GameObject createSpriteOffset(int row, int column){
        Vector2 tempPosition = new Vector2(row, column + offSet);
        int spriteToUse = Random.Range(0, Sprites.Length);
        GameObject sprite = Instantiate(Sprites[spriteToUse], tempPosition, Quaternion.identity);
        sprite.transform.parent = this.transform;
        sprite.name = "(" + row + "," + column + ")";
        sprite.GetComponent<Sprite>().row = row;
        sprite.GetComponent<Sprite>().column = column;
        sprite.GetComponent<Sprite>().index = ((row * majorAxis) + column); 
        return sprite;
    }

    // Function that checks each index for destruction.
    public void DestroyMatches(){
        for(int i = 0; i < width; i++)
        {
            if(nullSpriteArray[i] > 0)
            {
                for(int j = 0; j < height; j++)
                {
                    int index = (i * majorAxis) + j;
                    DestroyMatchesAt(index);
                }
            }
        }
        StartCoroutine(DecreaseRowCo());
    }

    // Destruction of sprite at index in array.
    private void DestroyMatchesAt(int index){
        if(allSpritesMatrix[index] != null && allSpritesMatrix[index].GetComponent<Sprite>().isMatched){
            Destroy(allSpritesMatrix[index]);
            allSpritesMatrix[index] = null; 
        }
    }

    // Function that slides down the sprites if the sprite below it is 
    // destroyed.
    private IEnumerator DecreaseRowCo(){
        int nullCount = 0;
        for(int i = 0; i < width; i++)
        {
            if(nullSpriteArray[i] > 0)
            {
                for(int j = 0; j < height; j++)
                {
                    int oldIndex = (i * majorAxis) + j;
                    if(allSpritesMatrix[oldIndex] == null){
                        nullCount++;
                    }
                    else if(nullCount > 0) 
                    {
                        int newIndex = (i * majorAxis) + (j - nullCount);
                        allSpritesMatrix[oldIndex].GetComponent<Sprite>().column -= nullCount;
                        allSpritesMatrix[newIndex] = allSpritesMatrix[oldIndex];
                        allSpritesMatrix[newIndex].name = "(" + i + "," + (j - nullCount) + ")";
                        allSpritesMatrix[oldIndex] = null;
                    }
                }
            }
            nullCount = 0;
        }
        yield return new WaitForSeconds(.2f);
        StartCoroutine(FillBoardCo());
    }

    private IEnumerator FillBoardCo(){
        RefillBoard();
        yield return new WaitForSeconds(.2f);

        currentState = GameState.move;
    }

    // Function to create new sprites
    private void RefillBoard(){
        for(int i = 0; i < width; i++)
        {
            if(nullSpriteArray[i] > 0)
            {
                for(int j = 0; j < height; j++)
                {
                    int index = (i * majorAxis) + j;
                    if(allSpritesMatrix[index] == null){
                        // Creation of the sprite.
                        allSpritesMatrix[index] = createSpriteOffset(i, j);
                    }
                }
            }
            nullSpriteArray[i] = 0;
        }
    }

    // A Breath First search implementation that keeps track of all possible matching sprites
    public void findAllMatches(){
        bool[] visited = new bool[vertices];
        // Create a queue
        Queue<int> q = new Queue<int>();
        for(int i = 0; i < vertices; i++)
        {
            if(visited[i] == false) 
            {
                q.Enqueue(i);

                // When count i empty then that would be a new section
                while (q.Count > 0)
                {
                    int node = q.Dequeue();
                    visited[node] = true;
                    List<int> list = spritesAdjacencyList[node];

                    foreach(int spriteIndex in list)
                    {
                        if(allSpritesMatrix[spriteIndex] != null && allSpritesMatrix[node].tag == allSpritesMatrix[spriteIndex].tag && visited[spriteIndex] == false)
                        {
                            if(allMatches[node] == null)
                            {
                                allMatches[node] = new List<int>();
                            }
                            q.Enqueue(spriteIndex);
                            allMatches[node].Add(spriteIndex);
                        }
                        
                    }
                }
            }
        }
    }

    // Need to test//
    public void clearAllMatches(int index){
        // Create a queue
        Queue<int> q = new Queue<int>();
        q.Enqueue(index);

        // When count i empty then that would be a new section
        while (q.Count > 0)
        {
            int node = q.Dequeue();
            List<int> list = allMatches[index];

            foreach(int spriteIndex in list)
            {
                q.Enqueue(spriteIndex);
            }
            allMatches[index].Clear();
        }
    }
}
