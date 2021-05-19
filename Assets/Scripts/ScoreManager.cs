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

        if(numberOfObjectives == 1)
        {
            // Change Sprites Images
            objectiveImages[0].sprite = findMatches.OriginalSprites[grid.gameLevels.levels[grid.level].spriteTypeGoal[0]];

            // Change Goal initial values
            objectiveNumber[0] = grid.gameLevels.levels[grid.level].spriteTypeAmount[0];
            objectiveTexts[0].text = objectiveNumber[0].ToString();

            // Turn on GamObjects 
            objectives[0].SetActive(true);
        }

        if(numberOfObjectives == 2)
        {
            // Change GameObjects positions
            objectives[0].transform.position = new Vector3(-45, 0, 0);
            objectives[1].transform.position = new Vector3(45, 0, 0);

            // Change Sprites Images
            objectiveImages[0].sprite = findMatches.OriginalSprites[grid.gameLevels.levels[grid.level].spriteTypeGoal[0]];
            objectiveImages[1].sprite = findMatches.OriginalSprites[grid.gameLevels.levels[grid.level].spriteTypeGoal[1]];

            // Change Goal initial values
            objectiveNumber[0] = grid.gameLevels.levels[grid.level].spriteTypeAmount[0];
            objectiveTexts[0].text = objectiveNumber[0].ToString();

            objectiveNumber[1] = grid.gameLevels.levels[grid.level].spriteTypeAmount[1];
            objectiveTexts[1].text = objectiveNumber[1].ToString();

            // Turn on GamObjects 
            objectives[0].SetActive(true);
            objectives[1].SetActive(true);
        }

        if(numberOfObjectives == 3)
        {
            // Change Sprites Images
            objectiveImages[0].sprite = findMatches.OriginalSprites[grid.gameLevels.levels[grid.level].spriteTypeGoal[0]];
            objectiveImages[1].sprite = findMatches.OriginalSprites[grid.gameLevels.levels[grid.level].spriteTypeGoal[1]];
            objectiveImages[2].sprite = findMatches.OriginalSprites[grid.gameLevels.levels[grid.level].spriteTypeGoal[2]];

            // Change Goal initial values
            objectiveNumber[0] = grid.gameLevels.levels[grid.level].spriteTypeAmount[0];
            objectiveTexts[0].text = objectiveNumber[0].ToString();

            objectiveNumber[1] = grid.gameLevels.levels[grid.level].spriteTypeAmount[1];
            objectiveTexts[1].text = objectiveNumber[1].ToString();

            objectiveNumber[2] = grid.gameLevels.levels[grid.level].spriteTypeAmount[2];
            objectiveTexts[2].text = objectiveNumber[2].ToString();

            objectives[0].SetActive(true);
            objectives[1].SetActive(true);
            objectives[2].SetActive(true);
        }
    }

    public void IncreaseScore(int[] sprites)
    {
        for(int i = 0; i < sprites.Length; i++)
        {
            if(sprites[i] > 0)
            {
                score = score + (multiplier * sprites[i]);
                scoreText.text = score.ToString();
            }
        }

        for(int i = 0; i < numberOfObjectives; i++)
        {
            // Decrease Objectives
            int type = grid.gameLevels.levels[grid.level].spriteTypeGoal[i];
            if(sprites[type] > 0)
            {
                // Plus 1 for offset of 0;
                Debug.Log(type);
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
                Debug.Log("ObjectivesMet not met");
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
