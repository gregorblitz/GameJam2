using UnityEngine;

public class CarController : MonoBehaviour
{
    [Header("MOVEMENT")]
    public float speed = 15f;
    public float turnSpeed = 100f;

    private float moveInput;
    private float turnInput;

    void Update()
    {
        // Input de teclado
        moveInput = Input.GetAxis("Vertical");   // W/S
        turnInput = Input.GetAxis("Horizontal"); // A/D
    }

    void FixedUpdate()
    {
        Move();
        Turn();
    }

    void Move()
    {
        // Movimiento hacia adelante
        transform.Translate(Vector3.forward * moveInput * speed * Time.fixedDeltaTime);
    }

    void Turn()
    {
        // Giro del carro
        transform.Rotate(Vector3.up * turnInput * turnSpeed * Time.fixedDeltaTime);
    }
}