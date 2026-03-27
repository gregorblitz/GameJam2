using UnityEngine;

public class CarLightSystem : MonoBehaviour
{
    [Header("REFERENCIAS DE LUCES")]
    public Light leftLight;   // Luz del lado izquierdo del carro
    public Light rightLight;  // Luz del lado derecho del carro

    [Header("ENERGÍA")]
    public float maxEnergy = 100f;  // Energía máxima de las luces
    public float currentEnergy;     // Energía actual

    [Header("CONSUMO")]
    public float drainRate = 5f;    // Energía que se consume por segundo

    [Header("CONFIGURACIÓN DE LUZ")]
    public float maxIntensity = 2f; // Intensidad máxima de las luces
    public float minIntensity = 0f; // Intensidad mínima (cuando se apaga)
    public float maxRange = 25f;    // Alcance máximo de la luz
    public float minRange = 5f;     // Alcance mínimo

    [Header("EFECTOS")]
    public bool enableFlicker = true;      // Activar parpadeo cuando la energía es baja
    public float flickerThreshold = 0.2f;  // % de energía para empezar a parpadear

    void Start()
    {
        // Iniciar las luces con energía completa
        currentEnergy = maxEnergy;
    }

    void Update()
    {
        DrainEnergy();   // Reducir energía con el tiempo
        UpdateLights();  // Actualizar intensidad, alcance y parpadeo
    }

    /// <summary>
    /// Reduce la energía con el tiempo
    /// </summary>
    void DrainEnergy()
    {
        currentEnergy -= drainRate * Time.deltaTime;
        currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy); // Evitar valores fuera de rango
    }

    /// <summary>
    /// Actualiza intensidad, alcance y efectos de las luces según la energía
    /// </summary>
    void UpdateLights()
    {
        float normalized = currentEnergy / maxEnergy; // Normalizamos energía 0-1

        float intensity = Mathf.Lerp(minIntensity, maxIntensity, normalized);
        float range = Mathf.Lerp(minRange, maxRange, normalized);

        leftLight.intensity = intensity;
        rightLight.intensity = intensity;

        leftLight.range = range;
        rightLight.range = range;

        // Parpadeo si la energía está baja
        if (enableFlicker && normalized < flickerThreshold && currentEnergy > 0)
        {
            float flicker = Random.Range(0.8f, 1.2f);
            leftLight.intensity *= flicker;
            rightLight.intensity *= flicker;
        }

        // Apagar completamente si no hay energía
        if (currentEnergy <= 0)
        {
            leftLight.enabled = false;
            rightLight.enabled = false;
        }
        else
        {
            leftLight.enabled = true;
            rightLight.enabled = true;
        }
    }

    /// <summary>
    /// Función para recargar energía (usar con pickups o eventos)
    /// </summary>
    public void AddEnergy(float amount)
    {
        currentEnergy += amount;
        currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);
    }

    /// <summary>
    /// Función para quitar energía (efectos negativos)
    /// </summary>
    public void RemoveEnergy(float amount)
    {
        currentEnergy -= amount;
        currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);
    }
}