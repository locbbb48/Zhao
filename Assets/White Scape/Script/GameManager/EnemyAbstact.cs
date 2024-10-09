/*	  - Codeby Bui Thanh Loc -
	contact : builoc08042004@gmail.com
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAbstract : ObjectAbstract
{
    [SerializeField] protected Animator animator;
    [SerializeField] protected float dameAttack;
    protected Player player;

    protected override void Start()
    {
        base.Start();
        player = GameManager.Instance.player;

    }
    
    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
    }

    protected virtual void Update()
    {
        if (player != null)
        {
            RotateTowardsPlayer();
        }
        else
        {
            Debug.Log("EnemyAbstract player is null");
        }
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        UIManager.Instance.ShowDamage(-damage, transform.position + new Vector3(0, 1f, -10), Color.blue);
    }

    protected void RotateTowardsPlayer()
    {
        // Lấy scale ban đầu của Enemy
        Vector3 originalScale = transform.localScale;

        // Kiểm tra xem Player nằm bên trái hay bên phải của Enemy
        if (player.transform.transform.position.x < transform.position.x)
        {
            // Player ở bên trái, quay mặt sang trái
            transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        }
        else if (player.transform.transform.position.x > transform.position.x)
        {
            // Player ở bên phải, quay mặt sang phải
            transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        }
    }

    protected override void OnDeath()
    {
        base.OnDeath();
        UIManager.Instance.ShowNoti($"You've killed a <color=red>{GetType().Name}</color>");
    }
}
