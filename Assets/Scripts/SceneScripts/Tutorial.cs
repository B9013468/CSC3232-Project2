using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.IO;
using System.Linq;

public class Tutorial : MonoBehaviour
{
    [SerializeField] Text tutorialText;

    [SerializeField] GameObject tutorialPanel;

    [SerializeField] GameObject player;

    [SerializeField] GameObject obstacle1;
    [SerializeField] GameObject obstacle2;
    [SerializeField] GameObject enemyTutorial;
    [SerializeField] GameObject heartTutorial;
    [SerializeField] GameObject tutorialEndPoint;
    [SerializeField] GameObject starTurtorial;
    GameObject diamond;


    bool attackedEnemy = false;

    // Start is called before the first frame update
    void Start()
    {
        tutorialPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update() //IEnumerator
    {
        diamond = GameObject.FindWithTag("Diamond");

        if (Input.GetKeyDown(KeyCode.Mouse0) && !Levels.GamePaused)
        {
            attackedEnemy = true;
        }

        if ((player.transform.position - tutorialEndPoint.transform.position).magnitude < 3)
        {
            tutorialPanel.SetActive(false);
            GetComponent<Tutorial>().enabled = false;
        }
        else if ((player.transform.position - starTurtorial.transform.position).magnitude < 13)
        {
            tutorialText.text = "Defeat all the oponents, to reach the Winning Star and finish the level.";
            tutorialPanel.SetActive(true);
        }
        else if ((player.transform.position - heartTutorial.transform.position).magnitude < 5 && heartTutorial.activeSelf)
        {
            tutorialText.text = "By consuming hearts, player can get back some of the lost Health.";
            tutorialPanel.SetActive(true);
        }
        else if (diamond != null)
        {
            if ((player.transform.position - diamond.transform.position).magnitude < 5 && !enemyTutorial.activeSelf)
            {
                tutorialText.text = "Enemies when killed, can drop diamonds.\nThe chances are that they drop only one, but if you are a little lucky you can get 2 or even 3.\nDiamonds can be used in the shop (N/A yet) to buy weapons or level up attributes.";
                tutorialPanel.SetActive(true);
            }
        }
        else if ((player.transform.position - enemyTutorial.transform.position).magnitude < 3 && !attackedEnemy)
        {
            tutorialText.text = "When you are out of enemy's  view, enemy cannot see you and won't attack you.\nTry sneeking behind enemies, so you always have the first attack of the fight.\n Press Mouse Left Click to attack them.";
            tutorialPanel.SetActive(true);
        }
        else if ((player.transform.position - obstacle2.transform.position).magnitude < 3)
        {
            tutorialText.text = "Press Space twice to Double Jump";
            tutorialPanel.SetActive(true);
        }
        else if ((player.transform.position - obstacle1.transform.position).magnitude < 3)
        {
            tutorialPanel.SetActive(true);
            tutorialText.text = "Press Space once to Jump";
        }
        
        else
        {
            tutorialPanel.SetActive(false);
        }
    }
}