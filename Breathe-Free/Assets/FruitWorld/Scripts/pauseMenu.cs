using UnityEngine.SceneManagement;
using UnityEngine;

public class pauseMenu : MonoBehaviour
{
    public static bool isPaused = false;
    public GameObject pauseMenuUI;
    playAudio audio;
    void Start()
    {
        //audio = GameObject.Find("AudioPlayer").GetComponent<playAudio>();
    }


    void Update()
    {
        OVRInput.Update();
        if (Input.GetKeyDown(KeyCode.Escape) || OVRInput.Get(OVRInput.RawButton.B))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        //audio.audio[0].Play();
        Time.timeScale = 1f;
        isPaused = false;

        Debug.Log("Resume");

    }
    void Pause()
    {
        pauseMenuUI.SetActive(true);
        //audio.audio[0].Pause();
        
        Time.timeScale = 0f;
        isPaused = true;

        
    }
    public void QuitGame()
    {
        //UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
        Debug.Log("Pause");
    }
}
