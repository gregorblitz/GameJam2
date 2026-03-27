using UnityEngine;
using System.Collections.Generic;

public class PowerupSpawner : MonoBehaviour
{
    [Header("TUS POWER UPS")]
    public GameObject[] powerUpPrefabs; 
    
    [Header("CANTIDAD")]
    public int totalPowerUpsToSpawn = 38; // Objetos repartidos en total

    [Header("PUNTOS EN LA CALLE")]
    public List<Transform> spawnPoints;

    void Start()
    {
        SpawnAllPowerUps();
    }

    public void SpawnAllPowerUps()
    {
        // Verificacion
        if (spawnPoints.Count == 0 || powerUpPrefabs.Length == 0)
        {
            Debug.LogWarning("¡Faltan puntos de aparición o prefabs de power ups en el Spawner!");
            return;
        }

        // Hacemos una copia de los puntos para ir borrando los ya usados
        // evita que aparezcan dos Power Ups fusionados en el mismo lugar
        List<Transform> availablePoints = new List<Transform>(spawnPoints);

        for (int i = 0; i < totalPowerUpsToSpawn; i++)
        {
            // Si no quedan puntos libres en la calle para el ciclo
            if (availablePoints.Count == 0) break; 

            // Elige un punto al azar en la calle
            int randomPointIndex = Random.Range(0, availablePoints.Count);
            Transform selectedPoint = availablePoints[randomPointIndex];
            
            // Lo borra de la lista de disponibles
            availablePoints.RemoveAt(randomPointIndex);

            // Elige un Power Up al azar sea Bateria o Velocidad
            int randomPowerUpIndex = Random.Range(0, powerUpPrefabs.Length);
            GameObject selectedPowerUp = powerUpPrefabs[randomPowerUpIndex];

            //Lo crea en la escena le sumo 0.5f en Y para que no quede enterrado en el asfalto
            Instantiate(selectedPowerUp, selectedPoint.position + (Vector3.up * 0.5f), selectedPoint.rotation);
        }
    }
}
