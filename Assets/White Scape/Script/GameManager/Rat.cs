/*	  - Codeby Bui Thanh Loc -
	contact : builoc08042004@gmail.com
*/

using System.Collections;
using UnityEngine;

public class Rat : EnemyAbstract
{
    [SerializeField] private float detectionRadius = 5f;
    private Transform player;
    private bool isPlayerInRange = false;

    protected override void Awake()
    {
        base.Awake();
        _speed = 7f;
        dameAttack = 3f;
    }

    private void Update()
    {
        CheckPlayerInRange();

        if (isPlayerInRange)
        {
            RunTowardsPlayer();
        }
        else
        {
            Idle();
        }
    }

    private void CheckPlayerInRange()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        if (Vector3.Distance(transform.position, player.position) <= detectionRadius)
        {
            isPlayerInRange = true;
        }
        else
        {
            isPlayerInRange = false;
        }
    }

    private void RunTowardsPlayer()
    {
        animator.Play("Rat_Run");
        transform.position = Vector3.MoveTowards(transform.position, player.position, _speed * Time.deltaTime);
    }

    private void Idle()
    {
        animator.Play("Rat_Idle");
    }

    private void Attack()
    {
        animator.Play("Rat_Attack");
        // Add attack logic (e.g., reduce player HP)
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            UIManager.Instance.ShowNoti("Press Enter to fight!");
            // Detect player fight input (e.g., Enter key press)
        }
    }

    public override void TakeDamage(float amount)
    {
        base.TakeDamage(amount);
        if (currentHP <= 0)
        {
            OnDeath();
        }
        else
        {
            animator.Play("Rat_Hit");
        }
    }

    protected override void OnDeath()
    {
        animator.Play("Rat_Death");
        // Additional logic for death (e.g., removing the object from the game)
        Destroy(gameObject, 2f);  // Delay for animation
    }
}
