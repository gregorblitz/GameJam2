using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadPowerUpsScene()
    {
        Time.timeScale = 1f; // Por si vienes de pausa
        SceneManager.LoadScene("PowerUps 2"); // 🔥 Nombre EXACTO
    }
}