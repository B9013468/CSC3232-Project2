using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ProgressData
{
    public int totalDiamonds;
    public int totalStars;
    public int levelProgress;
    public int maxHealth;
    public string currentState;

    public ProgressData (DataStorage dataStorage)
    {
        totalDiamonds = dataStorage.totalDiamonds;
        totalStars = dataStorage.totalStars;
        levelProgress = dataStorage.levelProgress;
        maxHealth = dataStorage.maxHealth;
        currentState = dataStorage.currentState;
    }
}
