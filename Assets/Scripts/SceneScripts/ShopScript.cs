using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;
using System;

public class ShopScript : MonoBehaviour
{
    public static int totalDiamonds;
    public static int totalStars;
    public static int levelProgress;
    public static int maxHealth;
    public static string currentState;

    public static bool GamePaused = false;

    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject levelObject;
    [SerializeField] GameObject shopKeeper;

    [SerializeField] Text starsText;
    [SerializeField] Text diamondsText;

    // Start is called before the first frame update
    void Start()
    {
        LoadData();
        currentState = "Inside Game";
    }

    // Update is called once per frame
    void Update()
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

        starsText.text = Convert.ToString(totalStars);
        diamondsText.text = Convert.ToString(totalDiamonds);
    }

    void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        GamePaused = true;
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        GamePaused = false;
    }

    public void Menu()
    {
        levelObject.GetComponent<DataStorage>().SaveData();

        SceneManager.LoadScene("MenuLevelWorldScene");
    }

    public void Quit()
    {
        levelObject.GetComponent<DataStorage>().SaveData();
        Application.Quit();
    }

    // add diamonds for test purpsoses
    public void AddDiamonds()
    {
        totalDiamonds += 100;
        levelObject.GetComponent<DataStorage>().SaveData();
    }

    // add stars for test purpsoses
    public void AddStars()
    {
        totalStars += 1;
        levelObject.GetComponent<DataStorage>().SaveData();
    }

    // substract diamonds for test purpsoses
    public void SubstractDiamonds()
    {
        totalDiamonds -= 100;
        levelObject.GetComponent<DataStorage>().SaveData();
    }

    // substract stars for test purpsoses
    public void SubstractStars()
    {
        totalStars -= 1;
        levelObject.GetComponent<DataStorage>().SaveData();
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
