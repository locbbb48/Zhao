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

    [SerializeField] private ParticleSystem hitEffect;

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

    private IEnumerator MakeParticleSystem()
    {
        ParticleSystem particle = ParticlePool.Instance.GetParticle(hitEffect);
        particle.transform.position = transform.position;
        particle.gameObject.SetActive(true);

        yield return new WaitForSeconds(GameManager.Instance.player.getAttackCooldown());

        ParticlePool.Instance.ReturnParticle(particle);

    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        if (gameObject.activeInHierarchy)
        {
            StartCoroutine(MakeParticleSystem());
        }
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
        UIManager.Instance.ShowLocalizedNoti("ENEMY_KILLED", 0.5f, GetType().Name);
    }
}
