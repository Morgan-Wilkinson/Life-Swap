using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public GameSettings settings;

    [Header("Score Variables")]
    // Score Variables
    private int score = 0;
    public Text scoreText;
    private int multiplier;

    // Start is called before the first frame update
    void Start()
    {
        settings = FindObjectOfType<GameSettings>();
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
    }
}
