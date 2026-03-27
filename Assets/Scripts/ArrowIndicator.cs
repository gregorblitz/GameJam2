using UnityEngine;

public class ArrowIndicator : MonoBehaviour
{
    public Transform target; // a quién apunta

    void Update()
    {
        if (target == null) return;

        Vector3 direction = target.position - transform.position;
        direction.y = 0; // evita inclinación

        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }
}