using System.Collections;
using UnityEngine;

public class Rikayon : MonoBehaviour
{
    [Header("Referencias")]
    public Animator animator;
    public Transform player; // Taxi

    [Header("Movimiento")]
    public float speed = 5f;

    [Header("Inicio")]
    public float delayBeforeChase = 5f;

    private bool isChasing = false;
    private bool isAttacking = false;

    void Start()
    {
        // Espera antes de empezar a perseguir
        StartCoroutine(StartChase());
    }

    IEnumerator StartChase()
    {
        yield return new WaitForSeconds(delayBeforeChase);

        isChasing = true;

        // Activa animación de caminar
        animator.SetBool("isWalking", true);
    }

    void Update()
    {
        // Si está atacando, no se mueve
        if (!isChasing || isAttacking) return;

        ChasePlayer();
    }

    void ChasePlayer()
    {
        // Dirección hacia el jugador (sin inclinación)
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0;

        // Rotar hacia el jugador
        transform.rotation = Quaternion.LookRotation(direction);

        // Moverse hacia el jugador
        transform.position += direction * speed * Time.deltaTime;
    }

    //  DETECTAR COLISIÓN CON EL TAXI
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            AttackPlayer();
        }
    }

    void AttackPlayer()
    {
        if (isAttacking) return;

        isAttacking = true;

        // Detener movimiento
        animator.SetBool("isWalking", false);

        // Activar animación de ataque
        animator.SetTrigger("Attack");

        // Aquí luego puedes poner Game Over
        Debug.Log("Jugador atrapado");
    }
}