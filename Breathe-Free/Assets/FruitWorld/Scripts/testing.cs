using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.EventSystems;
using System;

public class testing : MonoBehaviour
{

    public AudioSource audio;
    public AudioSource clickAudioSource;
    public AudioSource errorAudioSource;
    public AudioSource backgroundAudioSource;


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

    private void Update()
    {
        if (!backgroundAudioSource.isPlaying)
        {
            backgroundAudioSource.Play();
        }
    }
    private void Start()
    {
        inhaleTime = 1;
        exhaleTime = 1;
        numOfCycles = 1;
    }



    public void startToInputsMenu()
    {
        startMenu.SetActive(false);
        inputsMenu.SetActive(true);
        audio.PlayOneShot(audio.clip);
        game = EventSystem.current.currentSelectedGameObject;
    }

    public void startToUsernameMenu()
    {
        startMenu.SetActive(false);
        usernameMenu.SetActive(true);
        audio.PlayOneShot(audio.clip);
    }
    public void inputsToStartMenu()
    {
        startMenu.SetActive(true);
        inputsMenu.SetActive(false);
        audio.PlayOneShot(audio.clip);

    }

    public void usernameToStartMenu()
    {
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

    public void incrementInhale()
    {
        if (inhaleTime < 15)
            inhaleTime++;
        inhaleText.text = inhaleTime.ToString();
        clickAudioSource.PlayOneShot(clickAudioSource.clip);
    }
    public void incrementExhale()
    {
        if (exhaleTime < 15)
            exhaleTime++;
        exhaleText.text = exhaleTime.ToString();
        clickAudioSource.PlayOneShot(clickAudioSource.clip);
    }
    public void incrementCycles()
    {
        if (numOfCycles < 20)
            numOfCycles++;
        cyclesText.text = numOfCycles.ToString();
        clickAudioSource.PlayOneShot(clickAudioSource.clip);
    }

    public void decrementInhale()
    {
        if (inhaleTime > 1)
            inhaleTime--;
        inhaleText.text = inhaleTime.ToString();
        clickAudioSource.PlayOneShot(clickAudioSource.clip);
    }
    public void decrementExhale()
    {
        if (exhaleTime > 1)
            exhaleTime--;
        exhaleText.text = exhaleTime.ToString();
        clickAudioSource.PlayOneShot(clickAudioSource.clip);
    }
    public void decrementCycles()
    {
        if (numOfCycles > 1)
            numOfCycles--;
        cyclesText.text = numOfCycles.ToString();
        clickAudioSource.PlayOneShot(clickAudioSource.clip);
    }

    public void quitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
        audio.PlayOneShot(audio.clip);
    }

    public void startGame()
    {
        Debug.Log(game);
        mechanics.inhaleTime = inhaleTime;
        mechanics.exhaleTime = exhaleTime;
        mechanics.numOfCycles = numOfCycles;
        RocketController.inhaleTargetTime = inhaleTime;
        RocketController.exhaleTargetTime = exhaleTime;
        RocketController.cycles = numOfCycles;


        if (game.name == "FruitWorldButton")
            SceneManager.LoadScene("FruitWorld");
        else
            SceneManager.LoadScene("SpaceQuest");
        audio.PlayOneShot(audio.clip);
    }


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

    public void backspace()
    {
        if (userNameText.text.Length > 0)
        {
            userNameText.text = userNameText.text.Remove(userNameText.text.Length - 1, 1);
        }


        clickAudioSource.PlayOneShot(clickAudioSource.clip);

    }

    public void readyToUsernameMenu()
    {
        readyMenu.SetActive(false);
        usernameMenu.SetActive(true);
        audio.PlayOneShot(audio.clip);

    }


}