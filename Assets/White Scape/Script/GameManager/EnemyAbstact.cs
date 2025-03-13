/*	  - Codeby Bui Thanh Loc -
	contact : builoc08042004@gmail.com
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public abstract class EnemyAbstract : ObjectAbstract
{
    [SerializeField] protected Animator animator;
    [SerializeField] protected float dameAttack;
    protected Player player;

    [SerializeField] private ParticleSystem hitEffect;

    [SerializeField] private Vector3 healthBarOffset = new Vector3(0, 0.5f, 0); // Điều chỉnh vị trí thanh máu
    [SerializeField] private GameObject healthBarPrefab; // Gán Prefab từ Inspector


    protected override void Start()
    {
        player = GameManager.Instance.player;

        // Tạo một instance của healthBar từ Prefab
        if (healthBarPrefab != null)
        {
            GameObject healthBarObject = Instantiate(healthBarPrefab, GameObject.Find("Canvas").transform);
            healthBar = healthBarObject.GetComponent<Slider>();
        }
        base.Start();
    }

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
    }

    protected virtual void Update()
    {
        RotateTowardsPlayer();

        UpdateHealthBarPosition();
    }

    private void UpdateHealthBarPosition()
    {
        if (healthBar != null)
        {
            healthBar.transform.position = Camera.main.WorldToScreenPoint(transform.position + healthBarOffset);
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
        if (player != null)
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
        else
        {
            Debug.Log("EnemyAbstract note: player is null");
        }
    }

    protected override void OnDeath()
    {
        base.OnDeath();
        UIManager.Instance.ShowLocalizedNoti("ENEMY_KILLED", 0.5f, GetType().Name);
        Destroy(healthBar.gameObject);
    }
}
