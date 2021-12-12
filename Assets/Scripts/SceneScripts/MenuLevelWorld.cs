using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.IO;
using System.Linq;

public class MenuLevelWorld : MonoBehaviour
{
    [SerializeField]  GameObject mainMenu;
    [SerializeField] GameObject newGameQuestion;
    [SerializeField] GameObject MenuPanel;
    [SerializeField] GameObject interactionTextBox;
    [SerializeField] GameObject player;
    [SerializeField] GameObject portal1;
    [SerializeField] GameObject portal2;
    [SerializeField] GameObject portal3;
    [SerializeField] GameObject portal4;
    [SerializeField] GameObject portal5;
    [SerializeField] GameObject finalPortal;
    [SerializeField] GameObject shop;
    [SerializeField] GameObject levelObject;

    [SerializeField] Text stars;
    [SerializeField] Text diamonds;
    [SerializeField] Text interactionText;

    public static int levelProgress;
    public static int totalDiamonds;
    public static int totalStars;
    public static int maxHealth;
    public static string currentState;

    string pressF = "Press F to Enter";
    string notProgressed = "Can not enter yet. You have to complete previous Levels to progress to this one";
    string notAvailable = "Level not Available yet. Work in progress";

    public static bool GamePaused = false;

    void Awake()
    {
        Time.timeScale = 0f; // start game with timescale to 0
        if (System.IO.File.Exists(Path.GetFullPath("./") + "/saveProggess.save1"))
        {
            LoadData();
        }
        // if player came from inside the game, disable menu
        if (currentState == "Inside Game")
        {
            MenuPanel.SetActive(false);
            Time.timeScale = 1f; // make sure timescale is equal to 1 while changing between scenes
        }
        currentState = "Outside Game"; // change state to outside game
        levelObject.GetComponent<DataStorage>().SaveData(); // auto save on awake
    }
    
    void Start()
    {
       
    }

    void Update()
    {
        // handle change of scenes (levels)
        if ((player.transform.position - portal1.transform.position).magnitude < 1)
        {
            interactionTextBox.SetActive(true);
            if(levelProgress >= 0)
            {
                interactionText.text = pressF;
                if (Input.GetKeyDown(KeyCode.F))
                {
                    levelObject.GetComponent<DataStorage>().SaveData();
                    SceneManager.LoadScene("Level1");
                }
            }
            else
            {
                interactionText.text = notProgressed;
            }
        }
        else if ((player.transform.position - portal2.transform.position).magnitude < 1)
        {
            interactionTextBox.SetActive(true);
            // if level progress is more than 1, can enter this level
            if (levelProgress >= 1)
            {
                interactionText.text = pressF;
                if (Input.GetKeyDown(KeyCode.F))
                {
                    levelObject.GetComponent<DataStorage>().SaveData();
                    SceneManager.LoadScene("Level2");
                }
            }
            else
            {
                interactionText.text = notProgressed;
            }
        }
        else if ((player.transform.position - portal3.transform.position).magnitude < 1)
        {
            interactionTextBox.SetActive(true);
            interactionText.text = notAvailable;
        }
        else if ((player.transform.position - portal4.transform.position).magnitude < 1)
        {
            interactionTextBox.SetActive(true);
            interactionText.text = notAvailable;
        }
        else if ((player.transform.position - portal5.transform.position).magnitude < 1)
        {
            interactionTextBox.SetActive(true);
            interactionText.text = notAvailable;
        }
        else if ((player.transform.position - finalPortal.transform.position).magnitude < 1)
        {
            interactionTextBox.SetActive(true);
            interactionText.text = notAvailable;
        }
        else if ((player.transform.position - shop.transform.position).magnitude < 1)
        {
            interactionTextBox.SetActive(true);
            interactionText.text = pressF;
            if (Input.GetKeyDown(KeyCode.F))
            {
                levelObject.GetComponent<DataStorage>().SaveData();
                SceneManager.LoadScene("Shop");
            }
        }
        else
        {
            interactionTextBox.SetActive(false);
        }

        // pause game with "ESC"
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

        // update stars and diamonds
        stars.text = Convert.ToString(totalStars);
        diamonds.text = Convert.ToString(totalDiamonds);
    }

    public void YesNew()
    {
        New();
        newGameQuestion.SetActive(false);
    }

    public void NoNew()
    {
        newGameQuestion.SetActive(false);
    }

    public void StartNew()
    {
        newGameQuestion.SetActive(true);
    }

    // method for start new game button
    void New()
    {
        Time.timeScale = 1f;
        levelProgress = 0;
        totalDiamonds = 0;
        totalStars = 0;
        maxHealth = 50;
        MenuPanel.SetActive(false);
        levelObject.GetComponent<DataStorage>().SaveData();
        LoadData();
    }

    public void Load()
    {
        LoadData();
        MenuPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Quit()
    {
        levelObject.GetComponent<DataStorage>().SaveData();
        Application.Quit();
    }

    // loads data
    void LoadData()
    {
        levelObject.GetComponent<DataStorage>().LoadData();
        totalDiamonds = levelObject.GetComponent<DataStorage>().totalDiamonds;
        totalStars = levelObject.GetComponent<DataStorage>().totalStars;
        levelProgress = levelObject.GetComponent<DataStorage>().levelProgress;
        maxHealth = levelObject.GetComponent<DataStorage>().maxHealth;
        currentState = levelObject.GetComponent<DataStorage>().currentState;
    }

    // pause game
    void Pause()
    {
        MenuPanel.SetActive(true);
        Time.timeScale = 0f;
        GamePaused = true;
    }

    // resume game
    public void Resume()
    {
        MenuPanel.SetActive(false);
        Time.timeScale = 1f;
        GamePaused = false;
    }
}
