using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.IO;
using System.Linq; 

public class Levels : MonoBehaviour
{
    public static int totalDiamonds;
    public static int totalStars;
    public static int levelProgress ;
    public static int maxHealth;
    public static string currentState;

    public static bool GamePaused = false;

    public string sceneName;

    public int currentLevel;

    public GameObject pauseMenu;
    public GameObject loseMenu;
    public GameObject winMenu;

    public GameObject star;
    public GameObject starEffect;
    public GameObject stairs;

    [SerializeField] GameObject levelObject;
    [SerializeField] Text stars;
    [SerializeField] Text diamonds;

    int diamondScore = 0;
    int starScore = 0;

    [Header("Audio")]
    [SerializeField] AudioSource battleSound;
    [SerializeField] AudioSource normalSound;

    void Awake()
    {
        normalSound.Play();
        LoadData();
    }

    void Start()
    {
        Time.timeScale = 1f; // make sure the timescale is equal to 1 after changing scenes
        sceneName = SceneManager.GetActiveScene().name;
        currentLevel = (int)Char.GetNumericValue(sceneName[(sceneName.Length - 1)]); // get last char of scene's name and convert it to int to find the current level number (*IMPRTANT* Always put level number last in the level scene names!)

        currentState = "Inside Game";

    }

    void Update()
    {
        // get list of all enemies in map
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        // if no more enemies alive, set stairs that lead to the star active
        if(enemies.Length == 0)
        {
            stairs.SetActive(true);
            starEffect.SetActive(true);
        }

        // if player's hp is 0 or lower, game lost
        if (PlayerStateMachine._health <= 0)
        {
            loseMenu.SetActive(true);
            Time.timeScale = 0f;
            GamePaused = true;
        }
        // if star has been consumed win game
        else if(!star.activeSelf)
        {
            winMenu.SetActive(true);
            Time.timeScale = 0f;
            GamePaused = true;
        }
        // pause and resume game by pressing "ESC"
        else
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (GamePaused)
                {
                    Resume();
                }
                else
                {
                    Pause();
                }
            }
            // updating current consumed diamonds and stars
            stars.text = Convert.ToString(PlayerStateMachine._winningStarCollected);
            diamonds.text = Convert.ToString(PlayerStateMachine._moneyCollected);

            Debug.Log(currentLevel);
        }

        // change the game music depending on if any of the enemies is in battle or not
        if (EnemyStateMachine._inBattle)
        {
            normalSound.Stop();
            if (!battleSound.isPlaying)
            {
                battleSound.Play();
            }
        }
        else
        {
            battleSound.Stop();
            if (!normalSound.isPlaying)
            {
                normalSound.Play();
            }
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(sceneName);
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        GamePaused = false;
    }

    public void Menu()
    {
        Save();

        SceneManager.LoadScene("MenuLevelWorldScene");
    }

    public void Quit()
    {
        Save();
        Application.Quit();
    }

    void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        GamePaused = true;
    }

    // save game data
    void Save()
    {
        // if level has finished
        if (!star.activeSelf)
        {
            totalStars += PlayerStateMachine._winningStarCollected;
            totalDiamonds += PlayerStateMachine._moneyCollected;
            // if current level number is bigger than saved level progress than change the saved level progress to current level, else keep it as it is
            if (currentLevel > levelObject.GetComponent<DataStorage>().levelProgress)
            {
                levelProgress = currentLevel;
            }
            else
            {
                levelProgress = levelObject.GetComponent<DataStorage>().levelProgress;
            }
        }
        // if the level is not finished, just load saved data again and leave level
        else
        {
            LoadData();
            currentState = "Inside Game";
        }
        levelObject.GetComponent<DataStorage>().SaveData(); // finally save game
    }

    // loads saved data
    void LoadData()
    {
        levelObject.GetComponent<DataStorage>().LoadData();
        totalDiamonds = levelObject.GetComponent<DataStorage>().totalDiamonds;
        totalStars = levelObject.GetComponent<DataStorage>().totalStars;
        levelProgress = levelObject.GetComponent<DataStorage>().levelProgress;
        maxHealth = levelObject.GetComponent<DataStorage>().maxHealth;
        currentState = levelObject.GetComponent<DataStorage>().currentState;
    }
}
