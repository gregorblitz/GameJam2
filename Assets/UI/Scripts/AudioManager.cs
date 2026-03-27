using UnityEngine;
using UnityEngine.Audio; // Necesario para interactuar con el Mixer

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Configuración de Canales")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    private void Awake()
    {
        // Implementación del Singleton
        if (Instance == null) 
        {
            Instance = this;
            // Esto evita que el sonido se detenga al cambiar de nivel
            DontDestroyOnLoad(gameObject); 
        }
        else 
        {
            Destroy(gameObject);
        }
    }

    // Método para música de fondo (reemplaza la anterior)
    public void PlayMusic(AudioClip clip)
    {
        if (clip == null) return;
        musicSource.clip = clip;
        musicSource.Play();
    }

    // Método para efectos instantáneos (disparos, saltos, UI)
    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;
        sfxSource.PlayOneShot(clip);
    }
}