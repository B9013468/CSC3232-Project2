using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataStorage : MonoBehaviour
{
    string mainScriptName;

    public int totalDiamonds;
    public int totalStars;
    public int levelProgress;
    public int maxHealth;
    public string currentState;

    void Awake()
    {
     
    }

    void Update()
    {
    }

    public void SaveData()
    {
        Debug.Log(mainScriptName);
        if (SceneManager.GetActiveScene().name == "Shop")
        {
            totalDiamonds = ShopScript.totalDiamonds;
            totalStars = ShopScript.totalStars;
            levelProgress = ShopScript.levelProgress;
            maxHealth = ShopScript.maxHealth;
            currentState = ShopScript.currentState;
        }
        else if (SceneManager.GetActiveScene().name == "MenuLevelWorldScene")
        {
            totalDiamonds = MenuLevelWorld.totalDiamonds;
            totalStars = MenuLevelWorld.totalStars;
            levelProgress = MenuLevelWorld.levelProgress;
            maxHealth = MenuLevelWorld.maxHealth;
            currentState = MenuLevelWorld.currentState;
        }
        else
        {
            totalDiamonds = Levels.totalDiamonds;
            totalStars = Levels.totalStars;
            levelProgress = Levels.levelProgress;
            maxHealth = Levels.maxHealth;
            currentState = Levels.currentState;
        }
        SaveProgress.SaveGameProgress(this);
    }

    public void LoadData()
    {
        ProgressData data = SaveProgress.LoadProgress();
        totalDiamonds = data.totalDiamonds;
        totalStars = data.totalStars;
        maxHealth = data.maxHealth;
        levelProgress = data.levelProgress;
        currentState = data.currentState;
    }
}
