using System.Collections;
using UnityEngine;

public class Rikayon : MonoBehaviour
{
    [Header("Referencias")]
    public Animator animator;
    public Transform player;

    [Header("Movimiento")]
    public float speed = 5f;

    [Header("Inicio")]
    public float delayBeforeChase = 5f;

    [Header("Ataque")]
    public float attackRecoveryTime = 2f;
    public float attackRange = 3.5f;

    [Header("PELIGRO UI")]
    public GameObject dangerFlash;
    public float dangerDistance = 12f;
    public float flickerSpeed = 8f;

    private bool isChasing = false;
    private bool isAttacking = false;

    private CanvasGroup dangerCanvas;

    void Start()
    {
        StartCoroutine(StartChase());
    }

    //  CLAVE: se ejecuta cada vez que el bicho se activa
    void OnEnable()
    {
        if (dangerFlash == null)
            dangerFlash = GameObject.Find("DangerFlash");

        if (dangerFlash != null)
        {
            dangerCanvas = dangerFlash.GetComponent<CanvasGroup>();

            if (dangerCanvas == null)
                dangerCanvas = dangerFlash.AddComponent<CanvasGroup>();

            //  RESET TOTAL (evita pantalla roja al aparecer)
            dangerCanvas.alpha = 0f;
        }
    }

    IEnumerator StartChase()
    {
        yield return new WaitForSeconds(delayBeforeChase);

        isChasing = true;
        animator.SetBool("isWalking", true);
    }

    void Update()
    {
        if (!isChasing || isAttacking) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        UpdateDangerEffect(distanceToPlayer);

        if (distanceToPlayer <= attackRange)
        {
            AttackPlayer(player.gameObject);
            return;
        }

        ChasePlayer();
    }

    void UpdateDangerEffect(float distance)
    {
        if (dangerCanvas == null) return;

        //  Normalizar distancia
        float normalized = Mathf.Clamp01(1 - (distance / dangerDistance));

        float alpha = normalized;

        //  Parpadeo cuando está muy cerca
        if (distance < dangerDistance * 0.5f)
        {
            float flicker = Mathf.Sin(Time.time * flickerSpeed) * 0.5f + 0.5f;
            alpha *= flicker;
        }

        dangerCanvas.alpha = alpha;
    }

    void ChasePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0;

        transform.rotation = Quaternion.LookRotation(direction);
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isAttacking)
        {
            AttackPlayer(collision.gameObject);
        }
    }

    void AttackPlayer(GameObject taxi)
    {
        isAttacking = true;

        animator.SetBool("isWalking", false);
        animator.SetTrigger("Attack");

        PlayerHealth taxiHealth = taxi.GetComponent<PlayerHealth>();
        if (taxiHealth != null)
        {
            taxiHealth.LoseLife();
        }

        Debug.Log("¡Jugador atrapado!");

        StartCoroutine(RecoverFromAttack());
    }

    IEnumerator RecoverFromAttack()
    {
        yield return new WaitForSeconds(attackRecoveryTime);

        isAttacking = false;
        animator.SetBool("isWalking", true);
    }
}