using UnityEngine;
using UnityEngine.EventSystems; // ¡OBLIGATORIO para detectar el mouse!

// Este script DEBE ir en un objeto con un componente Image o Button
public class UIButtonFeedback : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    // Las variables deben ser PUBLICAS para que aparezcan en el Inspector
    [Header("Clips de Sonido (AudioClips)")]
    public AudioClip hoverSound;
    public AudioClip clickSound;

    // Se activa cuando el mouse pasa POR ENCIMA del botón
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Verifica si el AudioManager existe y si has asignado un sonido
        if (AudioManager.Instance != null && hoverSound != null)
        {
            AudioManager.Instance.PlaySFX(hoverSound);
        }
    }

    // Se activa cuando hacemos CLIC en el botón
    public void OnPointerClick(PointerEventData eventData)
    {
        // Verifica si el AudioManager existe y si has asignado un sonido
        if (AudioManager.Instance != null && clickSound != null)
        {
            AudioManager.Instance.PlaySFX(clickSound);
        }
    }
}