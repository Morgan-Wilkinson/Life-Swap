using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public GameSettings settings;
    private GridManager grid;

    [Header("Score Variables")]
    // Score Variables
    private int score = 0;
    public Text scoreText;
    private int multiplier;

    // Start is called before the first frame update
    void Start()
    {
        settings = FindObjectOfType<GameSettings>();
        grid = FindObjectOfType<GridManager>();
        multiplier = settings.gameLevels.levels[0].multiplier;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IncreaseScore(int amountOfDestroyedSprites)
    {
        score = score + (multiplier * amountOfDestroyedSprites);
        scoreText.text = score.ToString();

        if(score > 1000)
        {
            ObjectivesMet();
        }
    }

    public void ObjectivesMet()
    {
        SaveDataGameState playerData = new SaveDataGameState(grid.SavedData.currentLevel + 1, grid.SavedData.powerUpArrows + 1, 0, 0);

        GameManager.SaveGameState(playerData);

        // Show award go back to home screen and then upgrade level
        SceneManager.LoadScene("MainMenuScene");
    }
}
