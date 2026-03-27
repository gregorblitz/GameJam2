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
        pausePanel.SetActive(false);
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenu()
    {
        pausePanel.SetActive(false); // Oculta el menú
        Time.timeScale = 1.0f;       // Reanuda el tiempo
        isPaused = false;            // Reset estado

        SceneManager.LoadScene("MainMenu"); 
    }
}