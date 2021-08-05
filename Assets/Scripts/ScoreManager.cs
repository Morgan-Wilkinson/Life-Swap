using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public GameSettings settings;
    private GridManager grid;
    private FindMatches findMatches;

    [Header("Score Variables")]
    // Score Variables
    private int score = 0;
    public Text scoreText;
    private int multiplier;
    int numberOfObjectives;

    [Header("Objective GameObjects")]
    public GameObject[] objectives;

    [Header("Objective Images")]
    public Image[] objectiveImages;
    
    [Header("Objective Text")]
    public int[] objectiveNumber;
    public TMP_Text[] objectiveTexts;

    [Header("Objective Completion Checkmarks")]
    public GameObject[] objectiveCompletedChecks;

    // Start is called before the first frame update
    void Start()
    {
        settings = FindObjectOfType<GameSettings>();
        grid = FindObjectOfType<GridManager>();
        findMatches = FindObjectOfType<FindMatches>();
        multiplier = settings.gameLevels.levels[0].multiplier;
        
    }

    public void SetUpScorePlacement()
    {
        numberOfObjectives = grid.gameLevels.levels[grid.SavedData.currentLevel].spriteTypeGoal.Length;
        objectiveNumber = new int[numberOfObjectives];

        for(int i = 0; i < numberOfObjectives; i++)
        {
            // Change Sprites Images
            if(grid.gameLevels.levels[grid.SavedData.currentLevel].spriteTypeGoal[i] == 8)
            {
                objectiveImages[i].sprite = BreakbleSpriteObjective(grid.levelBreakableTypes[0]);
            }
            else
            {
                objectiveImages[i].sprite = findMatches.OriginalSprites[grid.gameLevels.levels[grid.level].spriteTypeGoal[i]];
            }

            // Change Goal initial values
            objectiveNumber[i] = grid.gameLevels.levels[grid.level].spriteTypeAmount[i];
            objectiveTexts[i].text = objectiveNumber[i].ToString();

            // Turn on GamObjects 
            objectives[i].SetActive(true);
        }
    }

    public UnityEngine.Sprite BreakbleSpriteObjective(string tag)
    {
       UnityEngine.Sprite spriteType = null;

        switch(tag)
        {
            case "Crystals":
                spriteType = grid.BreakableSprites[0][0];
                break;
        }

        return spriteType;
    }
    public void IncreaseScore(int[] sprites)
    {
        for(int i = 0; i < sprites.Length; i++)
        {
            if(sprites[i] > 0)
            {
                score = score + (multiplier * sprites[i]);
                //scoreText.text = score.ToString();
            }
        }

        for(int i = 0; i < numberOfObjectives; i++)
        {
            // Decrease Objectives
            int type = grid.gameLevels.levels[grid.level].spriteTypeGoal[i];
            if(sprites[type] > 0)
            {
                objectiveNumber[i] -= sprites[type];
                if(objectiveNumber[i] <= 0)
                {
                    objectiveTexts[i].transform.gameObject.SetActive(false);
                    objectiveCompletedChecks[i].SetActive(true);
                }
                else
                {
                    objectiveTexts[i].text = objectiveNumber[i].ToString();
                }
            }
        }

        if(CheckIfObjectiveMet())
        {
            ObjectivesMet();
        }
    }

    public bool CheckIfObjectiveMet()
    {
        bool objectivesMet = false;

        if(score >= grid.gameLevels.levels[grid.level].starOne)
            objectivesMet = true;
        for(int i = 0; i < numberOfObjectives; i++)
        {
            if(!objectiveCompletedChecks[i].activeInHierarchy)
            {
                objectivesMet = false;
            }
        }

        return objectivesMet;
    }

    public void ObjectivesMet()
    {
        SaveDataGameState playerData = new SaveDataGameState(grid.SavedData.currentLevel + 1, grid.SavedData.powerUpArrows + 1, 0, 0);

        GameManager.SaveGameState(playerData);

        // Show award go back to home screen and then upgrade level
        SceneManager.LoadScene("MainMenuScene");
    }
}
