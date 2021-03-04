using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenú : MonoBehaviour
{
    public static bool GamePause = false;
    public GameObject pauseMenu;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GamePause)
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
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        GamePause = false;
        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;
    }

    void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        GamePause = true;
        //Cursor.visible = true;
        //Cursor.lockState = CursorLockMode.None;

    }

    public void Menu()
    {
        SceneManager.LoadScene(0);
    }
}
