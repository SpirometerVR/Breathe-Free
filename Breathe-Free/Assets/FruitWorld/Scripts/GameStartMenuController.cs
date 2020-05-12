using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.EventSystems;
using System;

public class GameStartMenuController : MonoBehaviour
{

    public AudioSource audio;
    public AudioSource clickAudioSource;
    public AudioSource errorAudioSource;
    public AudioSource backgroundAudioSource;

    // Private varibales that are present on the start menu.
    private int inhaleTime;
    private int exhaleTime;
    private int numOfCycles;
    private String username;
    private GameObject game;
    private int typeCount = 0;

    public GameObject startMenu;
    public GameObject inputsMenu;
    public GameObject usernameMenu;
    public GameObject readyMenu;

    [SerializeField] Animator MenuAnimator;
    [SerializeField] TextMeshProUGUI inhaleText;
    [SerializeField] TextMeshProUGUI exhaleText;
    [SerializeField] TextMeshProUGUI cyclesText;
    [SerializeField] TextMeshProUGUI userNameText;

    [SerializeField] GameObject cantBeEmptyText;

    /**
     * Update is called once per frame.
     */
    private void Update()
    {
        if (!backgroundAudioSource.isPlaying)
        {
            backgroundAudioSource.Play();
        }
    }

    /**
     * Start is called once before the first frame.
     */
    private void Start()
    {
        // Initialize the variables to 1 on the start menu.
        inhaleTime = 1;
        exhaleTime = 1;
        numOfCycles = 1;
    }

    /**
     * Set the first screen active. This screen is the first screen used to enter the game.
     */
    public void startToInputsMenu()
    {
        startMenu.SetActive(false);
        inputsMenu.SetActive(true);
        audio.PlayOneShot(audio.clip);
        game = EventSystem.current.currentSelectedGameObject;
    }

    /**
     * Once the player has clicked the "ready" button, they will be prompted to the username
     * screen. Here, they will enter the player's username which will be used for the game leaderboards.
     */
    public void startToUsernameMenu()
    {
        startMenu.SetActive(false);
        usernameMenu.SetActive(true);
        audio.PlayOneShot(audio.clip);
    }

    /**
     * After choosing the username, the player will be able to choose which game they want to play. The
     * two available games are FruitWorld and SpaceQuest.
     */
    public void usernameToStartMenu()
    {
        // Ensure that the player entered a username.
        if (typeCount == 0 || userNameText.text.Length == 0)
        {
            cantBeEmptyText.SetActive(true);
            errorAudioSource.PlayOneShot(errorAudioSource.clip);
        }
        else if (userNameText.text.Length >= 1)
        {
            startMenu.SetActive(true);
            usernameMenu.SetActive(false);
            audio.PlayOneShot(audio.clip);
            username = userNameText.text;
        }
    }

    /**
     * After choosing a username and game to play, the therapy leader will determine the cycle parameters.
     * These parameters are present on this screen and include the inhale target time, exhale target time,
     * and number of cycles. The parameters chosen will feed into the game and be used for that game's therapy.
     */
    public void inputsToStartMenu()
    {
        startMenu.SetActive(true);
        inputsMenu.SetActive(false);
        audio.PlayOneShot(audio.clip);

    }

    /**
     * Increase the inhale time UI based on the therapy overseer's adjustments.
     */
    public void incrementInhale()
    {
        // Max inhale time of 15 seconds.
        if (inhaleTime < 15)
            inhaleTime++;
        inhaleText.text = inhaleTime.ToString();
        clickAudioSource.PlayOneShot(clickAudioSource.clip);
    }

    /**
     * Increase the exhale time UI based on the therapy overseer's adjustments.
     */
    public void incrementExhale()
    {
        if (exhaleTime < 15)
            exhaleTime++;
        exhaleText.text = exhaleTime.ToString();
        clickAudioSource.PlayOneShot(clickAudioSource.clip);
    }

    /**
     * Increase the cycle counter UI based on the therapy overseer's adjustments.
     */
    public void incrementCycles()
    {
        if (numOfCycles < 20)
            numOfCycles++;
        cyclesText.text = numOfCycles.ToString();
        clickAudioSource.PlayOneShot(clickAudioSource.clip);
    }

    /**
     * Decrease the inhale time UI based on the therapy overseer's adjustments.
     */
    public void decrementInhale()
    {
        if (inhaleTime > 1)
            inhaleTime--;
        inhaleText.text = inhaleTime.ToString();
        clickAudioSource.PlayOneShot(clickAudioSource.clip);
    }

    /**
     * Decrease the exhale time UI based on the therapy overseer's adjustments.
     */
    public void decrementExhale()
    {
        if (exhaleTime > 1)
            exhaleTime--;
        exhaleText.text = exhaleTime.ToString();
        clickAudioSource.PlayOneShot(clickAudioSource.clip);
    }

    /**
     * Decrease the cycle counter UI based on the therapy overseer's adjustments.
     */
    public void decrementCycles()
    {
        if (numOfCycles > 1)
            numOfCycles--;
        cyclesText.text = numOfCycles.ToString();
        clickAudioSource.PlayOneShot(clickAudioSource.clip);
    }

    /**
     * Quit the game if the player hits Quit.
     */
    public void quitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
        audio.PlayOneShot(audio.clip);
    }

    /**
     * Start the game once the player selects Start. The therapy parameters chosen on the
     * parmater screen will feed into the game that was chosen and be used as parameters for the
     * actual game.
     */
    public void startGame()
    {
        Debug.Log(game);
        FruitWorldController.inhaleTime = inhaleTime;
        FruitWorldController.exhaleTime = exhaleTime;
        FruitWorldController.numOfCycles = numOfCycles;
        FruitWorldController.userName = username;
        RocketController.inhaleTargetTime = inhaleTime;
        RocketController.exhaleTargetTime = exhaleTime;
        RocketController.cycles = numOfCycles;
        RocketController.userName = username;

        // Load the appropriate game based on the button selection
        if (game.name == "FruitWorldButton")
            SceneManager.LoadScene("FruitWorld");
        else
            SceneManager.LoadScene("SpaceQuest");
        audio.PlayOneShot(audio.clip);
    }

    /**
     * Enter keys for the username screen.
     */
    public void pressKey()
    {
        if (userNameText.text.Length < 15)
        {
            Debug.Log(EventSystem.current.currentSelectedGameObject.ToString()[0]);
            var chr = EventSystem.current.currentSelectedGameObject.ToString()[0];
            userNameText.text = userNameText.text + chr;
            typeCount++;
            cantBeEmptyText.SetActive(false);

        }
        clickAudioSource.PlayOneShot(clickAudioSource.clip);


    }

    /**
     * Remove keys for the username screen.
     */
    public void backspace()
    {
        if (userNameText.text.Length > 0)
        {
            userNameText.text = userNameText.text.Remove(userNameText.text.Length - 1, 1);
        }


        clickAudioSource.PlayOneShot(clickAudioSource.clip);

    }

    /**
     * Check the username to ensure that it is appropriate.
     */
    public void readyToUsernameMenu()
    {
        readyMenu.SetActive(false);
        usernameMenu.SetActive(true);
        audio.PlayOneShot(audio.clip);

    }


}