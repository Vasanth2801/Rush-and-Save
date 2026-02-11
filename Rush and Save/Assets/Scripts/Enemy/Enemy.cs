using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    Rigidbody2D rb;
    bool isChasing;
    Animator animator;

    [Header("Enemy Settings")]
    [SerializeField] float speed = 4f;
    [SerializeField] int facingDirection = 1;
    [SerializeField] Transform attackPoint;
    [SerializeField] float attackRange = 2;
    public LayerMask playerLayer;

    [Header("Attack Settings")]
    [SerializeField] float attackCoolDown = 1f;
    bool isAttacking = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if(player == null)
        {
            return;
        }

        float distance = Vector2.Distance(player.position,transform.position);

        if(distance <= attackRange)
        {
            isChasing = false;
            rb.linearVelocity = Vector2.zero;
            animator.SetBool("isRunning", false);

            if(!isAttacking)
            {
                StartCoroutine(AttackCoroutine());
            }

            return;
        }

        if(isChasing == true)
        {
            if(player.position.x > transform.position.x && facingDirection == -1 || player.position.x < transform.position.x && facingDirection == 1)
            {
                Flip();
            }
        }

        if(isChasing == true)
        {
            Chase();
        }
    }

    void Chase()
    {
        animator.SetBool("isRunning", true);
        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = direction * speed;
    }

    void Flip()
    {
        facingDirection *= -1;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }

    IEnumerator AttackCoroutine()
    {
        isAttacking = true;

        while(true)
        {
            if(player == null)
            {
                break;
            }

            float distance = Vector2.Distance(player.position, transform.position);
            if(distance  > attackRange)
            {
                break;
            }

            Attack();

            yield return new WaitForSeconds(attackCoolDown);
        }
        isAttacking = false;
    }

    void Attack()
    {
        if(animator != null)
        {
            animator.SetTrigger("Attack");
        }
        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);

        foreach(Collider2D hitCollider in hitPlayer)
        {
            if(player == null)
            {
                continue;
            }

            Debug.Log("Attacking");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            isChasing = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            animator.SetBool("isRunning", false);
            isChasing = false;
            rb.linearVelocity = Vector2.zero;
        }
    }
}
