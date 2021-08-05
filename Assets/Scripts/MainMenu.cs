using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;
public class MainMenu : MonoBehaviour
{
    public SaveDataGameState SavedData;
    public TMP_Text playButtonText;

    void Awake()
    {
        SavedData = GameManager.LoadData();
    }
    void Start() 
    {
        playButtonText.text = "Play Level " + SavedData.currentLevel.ToString() + "!";
    }
    public static void LoadScene()
    {
        // Only specifying the sceneName or sceneBuildIndex will load the Scene with the Single mode
        SceneManager.LoadScene("GamePlayScene");
    }
}
