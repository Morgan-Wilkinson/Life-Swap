using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour {
    [Header("Game Settings")]
    public TextAsset gridDimensionsSettingsJsonFile;
    public TextAsset levelsSettingsJsonFile;

    [Header("Game Variables")]
    public GridDimensions gridDimensions;
    public GameLevels gameLevels;

    void Start()
    {
        gridDimensions = GridDimensions.CreateFromJSON(gridDimensionsSettingsJsonFile.text);

        gameLevels = GameLevels.CreateFromJSON(levelsSettingsJsonFile.text);
    }
}

[System.Serializable]
public class GridDimensions
{
    public int height;
    public int width;
    public int offSet;
    
    public static GridDimensions CreateFromJSON(string jsonFileData)
    {
        return JsonUtility.FromJson<GridDimensions>(jsonFileData);
    }
}

[System.Serializable]
public class GameLevels 
{
    public Level[] levels;

    public static GameLevels CreateFromJSON(string jsonFileData)
    {
        return JsonUtility.FromJson<GameLevels>(jsonFileData);
    }
}

[System.Serializable]
public class Level 
{
    public int goal;
    public int starOne;
    public int startTwo;
    public int starThree;
    public int multiplier;
    
    public static Level CreateFromJSON(string jsonFileData)
    {
        return JsonUtility.FromJson<Level>(jsonFileData);
    }
}


