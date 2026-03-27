using UnityEngine;

public class TargetPointer : MonoBehaviour
{
    [Tooltip("El objeto al que la flecha debe apuntar (Ej: La batería)")]
    public Transform target; 
    
    [Tooltip("La cámara principal del juego")]
    public Camera mainCamera;

    void Update()
    {
        if (target == null) return;

        // Convertimos la posición del mundo del objetivo a la pantalla
        Vector3 targetScreenPos = mainCamera.WorldToScreenPoint(target.position);

        // Si el objetivo está frente a la cámara
        if (targetScreenPos.z > 0)
        {
            // Calculamos la dirección desde la flecha al objetivo en 2D
            Vector3 dir = targetScreenPos - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            
            // Ajustamos la rotación (restamos 90 si tu flecha apunta hacia arriba por defecto)
            transform.rotation = Quaternion.Euler(0, 0, angle - 90);
        }
    }
}