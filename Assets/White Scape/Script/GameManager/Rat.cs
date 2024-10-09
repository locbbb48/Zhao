/*	  - Codeby Bui Thanh Loc -
	contact : builoc08042004@gmail.com
*/

using System.Collections;
using UnityEngine;

public class Rat : EnemyAbstract
{
    [SerializeField] private float detectionRadius = 3f;
    [SerializeField] private float attackCooldown = 2f;
    private bool isPlayerInRange = false;
    private bool canAttack = true;

    protected override void Start()
    {
        maxHP = 30f;
        base.Start();
        speed = 3f;
        dameAttack = 1f;
    }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Update()
    {
        base.Update();
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
        if (Vector3.Distance(transform.position, player.transform.position) <= detectionRadius)
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
        animator.SetBool("Run",true);
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
    }

    private void Idle()
    {
        animator.SetBool("Run", false);
    }

    private IEnumerator Attack()
    {
        canAttack = false;
        animator.SetTrigger("Attack");
        GameManager.Instance.player.TakeDamage(dameAttack); // Tấn công player bằng dameAttack
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject == GameManager.Instance.player.gameObject && canAttack)
        {

            StartCoroutine(Attack()); // Bắt đầu tấn công nếu chạm vào Player
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject == GameManager.Instance.player.gameObject)
        {
            StopCoroutine(Attack()); // Ngừng tấn công khi Player rời đi
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
            animator.SetTrigger("Hit");
        }
    }

    protected override void OnDeath()
    {
        base.OnDeath();
        animator.SetTrigger("Death");
        //UIManager.Instance.ShowNoti($"You've kill a <color=blue>Rat</color>");
        gameObject.SetActive(false);
    }
}
