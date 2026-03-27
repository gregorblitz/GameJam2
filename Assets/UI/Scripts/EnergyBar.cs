using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{
    [SerializeField] private Image fill;
    
    [Header("Configuración de Colores")]
    public Color highEnergyColor = Color.green;
    public Color midEnergyColor = Color.yellow;
    public Color lowEnergyColor = Color.red;

    [Header("Configuración de Audio")]
    public AudioClip lowEnergySound;
    public float alertThreshold = 0.2f; 
    public float beepInterval = 0.8f; 
    
    private float nextBeepTime;

    public void SetEnergy(float value)
    {
        // 1. Actualizar el llenado de la barra
        fill.fillAmount = value;

        // 2. Cambiar color dinámicamente
        UpdateBarColor(value);

        // 3. Sistema de audio de alerta
        if (value <= alertThreshold && value > 0)
        {
            if (Time.time >= nextBeepTime)
            {
                PlayAlert();
                nextBeepTime = Time.time + beepInterval;
            }
        }
    }

    private void UpdateBarColor(float value)
    {
        if (value > 0.6f) 
            fill.color = highEnergyColor;
        else if (value > alertThreshold) 
            fill.color = midEnergyColor;
        else 
            fill.color = lowEnergyColor;
    }

    private void PlayAlert()
    {
        if (AudioManager.Instance != null && lowEnergySound != null)
        {
            AudioManager.Instance.PlaySFX(lowEnergySound);
        }
    }
}