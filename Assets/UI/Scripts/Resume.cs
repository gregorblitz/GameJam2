using UnityEngine;

public class ResumeButton : MonoBehaviour
{
    public GameObject pauseMenu; // Arrastra aquí el PauseMenu

    public void ResumeGame()
    {
        Time.timeScale = 1f;          // Reanuda el juego
        pauseMenu.SetActive(false);   // Oculta el menú
    }
}