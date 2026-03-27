using System.Collections.Generic;
using UnityEngine;

public class TaxiPassengerSystem : MonoBehaviour
{
    [Header("Modo Oscuro")]
    public Light directionalLight;      // Luz global del sol
    
    // --- CAMBIO: Ahora es un arreglo de monstruos ---
    public GameObject[] monsters;       
    public float spawnDistance = 15f;   // A cuántos metros del taxi aparecerán
    public float monsterSeparation = 4f;// SeparaciOn de bichos para que no choquen al aparecer
    // ------------------------------------------------

    [Header("Luces del Carro")]
    public CarLightSystem carLightSystem;  

    [Header("Referencias")]
    public ArrowIndicator arrow;         
    public Transform destinationMarker;  

    [Header("Pasajeros")]
    public List<Passenger> passengers;   

    private Passenger currentPassenger;
    private int currentIndex = 0;
    private bool hasPassenger = false;   

    void Start()
    {
        if (destinationMarker != null)
            destinationMarker.gameObject.SetActive(false);

        SetNextPassenger(); 
    }

    void SetNextPassenger()
    {
        if (currentIndex >= passengers.Count)
        {
            Debug.Log("No hay más pasajeros");
            arrow.target = null;
            return;
        }

        currentPassenger = passengers[currentIndex];
        currentPassenger.gameObject.SetActive(true);
        arrow.target = currentPassenger.transform;

        Debug.Log("Nuevo pasajero activado");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Passenger") && !hasPassenger)
        {
            Passenger p = other.GetComponent<Passenger>();
            if (p == currentPassenger)
                PickUpPassenger(p);
        }

        if (other.CompareTag("Destination") && hasPassenger)
        {
            DropPassenger();
        }
    }

    void PickUpPassenger(Passenger passenger)
    {
        hasPassenger = true;
        passenger.PickUp();

        if (destinationMarker != null && passenger.destination != null)
        {
            destinationMarker.position = passenger.destination.position;
            destinationMarker.gameObject.SetActive(true);
        }

        arrow.target = passenger.destination;
        Debug.Log("Pasajero recogido");

        if (currentIndex == 1)
        {
            if (carLightSystem != null)
                carLightSystem.AddEnergy(carLightSystem.maxEnergy);

            ActivateDarkMode();
        }
    }

    void DropPassenger()
    {
        hasPassenger = false;

        if (destinationMarker != null)
            destinationMarker.gameObject.SetActive(false);

        currentIndex++; 
        Debug.Log("Pasajero entregado");

        SetNextPassenger(); 
    }

    void ActivateDarkMode()
    {
        Debug.Log("MODO OSCURO ACTIVADO - ¡ESCAPA DE LA HORDA!");

        if (directionalLight != null)
            directionalLight.intensity = 0f;

        // --- APARICION EN FORMACION ---
        for (int i = 0; i < monsters.Length; i++)
        {
            if (monsters[i] != null)
            {
                // Calcula posicion base frente al taxi (+) o detrás (-)
                Vector3 baseSpawnPosition = transform.position + (transform.forward * spawnDistance);

                // Desplazamiento lateral usando transform.right
                float lateralOffset = 0f;
                if (i == 1) lateralOffset = -monsterSeparation; // El segundo bicho va a la izquierda
                else if (i == 2) lateralOffset = monsterSeparation;  // El tercer bicho va a la derecha

                // Posicion final del bicho actual
                Vector3 finalPosition = baseSpawnPosition + (transform.right * lateralOffset);
                finalPosition.y = monsters[i].transform.position.y; // Mantenemos su altura original

                monsters[i].transform.position = finalPosition;
                monsters[i].transform.LookAt(transform.position); // Miran al taxi
                monsters[i].SetActive(true); // Despiertan
            }
        }
    }
}