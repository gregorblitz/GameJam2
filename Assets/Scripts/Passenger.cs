using UnityEngine;

public class Passenger : MonoBehaviour
{
    public Transform destination; // destino asignado

    public void PickUp()
    {
        gameObject.SetActive(false); // desaparece al recogerlo
    }
}