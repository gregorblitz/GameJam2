using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem; // ¡NUEVA LIBRERÍA NECESARIA!

public class PauseMenuManager : MonoBehaviour
{
    [Header("UI del Menú de Pausa")]
    public GameObject pausePanel;
    
    private bool isPaused = false;

    void Update()
    {
        // Nueva forma de detectar la tecla ESC con el Input System Package
        if (Keyboard.current.escapeKey.wasPressedThisFrame || Keyboard.current.pKey.wasPressedThisFrame)
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void ResumeGame()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1.0f;
        isPaused = false;
    }

    public void PauseGame()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void RestartLevel()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("MainMenu");
    }
}