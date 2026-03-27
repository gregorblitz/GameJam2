using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{
    [SerializeField] private Image fill;
    
    [Header("Conexión con el Juego")]
    // Cambiamos el tipo a MonoBehaviour para evitar el error de compilación
    public MonoBehaviour batteryLogic; 

    [Header("Configuración de Colores")]
    public Color highEnergyColor = Color.green;
    public Color midEnergyColor = Color.yellow;
    public Color lowEnergyColor = Color.red;

    [Header("Configuración de Audio")]
    public AudioClip lowEnergySound;
    public float alertThreshold = 0.2f; 
    public float beepInterval = 0.8f; 
    
    private float nextBeepTime;

    private void Update()
    {
        if (batteryLogic != null)
        {
            // Accedemos a los datos usando reflexión simple para saltar el error de namespaces
            // Asumimos que los nombres en el script del grupo son estos:
            float current = (float)batteryLogic.GetType().GetField("currentBattery").GetValue(batteryLogic);
            float max = (float)batteryLogic.GetType().GetField("maxBattery").GetValue(batteryLogic);

            if (max > 0)
            {
                SetEnergy(current / max);
            }
        }
    }

    public void SetEnergy(float value)
    {
        if (fill != null) fill.fillAmount = value;
        UpdateBarColor(value);

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
        if (fill == null) return;
        if (value > 0.6f) fill.color = highEnergyColor;
        else if (value > alertThreshold) fill.color = midEnergyColor;
        else fill.color = lowEnergyColor;
    }

    private void PlayAlert()
    {
        if (AudioManager.Instance != null && lowEnergySound != null)
        {
            AudioManager.Instance.PlaySFX(lowEnergySound);
        }
    }
}