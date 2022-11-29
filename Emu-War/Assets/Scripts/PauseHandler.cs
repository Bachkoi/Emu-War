using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseHandler : MonoBehaviour
{
    #region Fields
    public bool gamePaused = false;
    [SerializeField]
    private GameObject _pausePanel;
    #endregion Fields
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            if(!gamePaused)
            {
                Time.timeScale = 0.0f;
                _pausePanel.SetActive(true);
            }
            else
            {
                Time.timeScale = 1.0f;
                _pausePanel.SetActive(false);
            }

            gamePaused = !gamePaused;
        }
    }

    public void QuitGame()
    {
        UnpauseGame();
        SceneManager.LoadScene("MainMenu");
    }

    public void UnpauseGame()
    {
        Time.timeScale = 1.0f;
        gamePaused = false;
        _pausePanel.SetActive(false);
    }
}
