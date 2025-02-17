/*	  - Codeby Bui Thanh Loc -
	contact : builoc08042004@gmail.com
*/

using System.Collections;
using TMPro;
using UnityEngine;

public class Player : ObjectAbstract
{

    private Animator animator;
    [SerializeField] private float H, V;
    [SerializeField] private Vector2 movement;
    [SerializeField] Vector2 lastMovement;
    [SerializeField] private float _speed = 5f;

    [SerializeField] private Transform flashlight;

    public TMP_Text HPText;

    [SerializeField] private float playerDame = 10f; 
    [SerializeField] private float attackCooldown = 0.5f;  
    [SerializeField] private float attackRange = 1f;       // Bán kính tấn công theo một hướng
    private bool canAttack = true;

    protected override void Start()
    {
        base.Start();
        speed = _speed;
        animator = GetComponent<Animator>();
        HPText.text = currentHP.ToString();
    }


    void Update()
    {
        HPText.text = currentHP.ToString();

        H = Input.GetAxis("Horizontal");
        V = Input.GetAxis("Vertical");
        movement.x = H;
        movement.y = V;
        if (movement != Vector2.zero)
        {
            lastMovement = movement;
        }
        FaceMove();
        Move();
        if (Input.GetKeyDown(KeyCode.Return) && canAttack && GameManager.Instance.isHaveDagger)
        {
            StartCoroutine(Fight());
        }
        RotateFlashlight();
    }

    void FaceMove()
    {
        animator.SetFloat("H", movement.x);
        animator.SetFloat("V", movement.y);
        animator.SetFloat("walk", movement.sqrMagnitude);

        animator.SetFloat("lastH", lastMovement.x);
        animator.SetFloat("lastV", lastMovement.y);
    }
    void Move()
    {
        Vector3 move = new Vector3(H, V);
        transform.position += move * speed * Time.deltaTime;
    }

    IEnumerator Fight()
    {
        canAttack = false;

        // Bắt đầu tấn công, chạy animation tấn công
        animator.SetTrigger("fight");

        // Xác định hướng tấn công dựa trên hướng di chuyển của Player
        Vector2 attackDirection = lastMovement.normalized; // Hướng cuối cùng của Player
        Vector2 attackCenter = (Vector2)transform.position + attackDirection * attackRange / 2;

        // Tạo vùng tấn công hình chữ nhật (hoặc sử dụng hình vuông tùy ý)
        Vector2 attackSize = new Vector2(attackRange, attackRange / 2);  // Kích thước vùng tấn công

        // Tìm các đối tượng trong vùng tấn công theo hướng hiện tại
        Collider2D[] hits = Physics2D.OverlapBoxAll(attackCenter, attackSize, 0f);

        // Kiểm tra từng đối tượng
        if (hits.Length > 0)
        {
            foreach (var hit in hits)
            {
                ObjectAbstract enemy = hit.GetComponent<ObjectAbstract>();
                if (enemy != null && enemy.gameObject.activeInHierarchy && hit.gameObject.CompareTag("Enemy"))
                {
                    // Tấn công kẻ địch, trừ HP
                    enemy.TakeDamage(playerDame);
                }
            }
        }
        else
        {
            Debug.Log("No enemy in Range!");
        }

        // Đợi cooldown tấn công
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    // Vẽ vùng tấn công khi chọn nhân vật (dành cho debug)
    private void OnDrawGizmosSelected()
    {
        Vector2 attackDirection = lastMovement.normalized;
        Vector2 attackCenter = (Vector2)transform.position + attackDirection * attackRange / 2;
        Vector2 attackSize = new Vector2(attackRange, attackRange / 2);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackCenter, attackSize);
    }

    void RotateFlashlight()
    {
        if (movement != Vector2.zero)
        {
            float angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg;
            flashlight.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        UIManager.Instance.ShowDamage(-damage, transform.position + new Vector3(0, 1f, -10), Color.red);
    }

    public IEnumerator RemoveHPbyTime(float hp, float duration = -1f)
    {
        float elapsedTime = 0f;

        // Trường hợp trừ HP mãi mãi
        if (duration == -1f)
        {
            while (true) // Vòng lặp vô hạn
            {
                TakeDamage(hp);
                yield return new WaitForSeconds(1f);
            }
        }
        else // Trường hợp có thời gian giới hạn
        {
            while (elapsedTime < duration)
            {
                TakeDamage(hp);
                yield return new WaitForSeconds(1f);
                elapsedTime += 1f;
            }

            UIManager.Instance.ShowLocalizedNoti("POISON_EFFECT_END", 0.5f);
        }
    }


    public float getCurHP()
    {
        return currentHP;
    }

    public void SetCurHpPlayer(float hp)
    {
        if(currentHP > maxHP)
        {
            currentHP = maxHP;
        }
        else
        {
            currentHP = hp;
        }
    }

    public float getAttackCooldown()
    {
        return attackCooldown;
    }

    public void SetAttackCooldown(float att)
    {
        attackCooldown = att;
    }

    public void AddMaxHpPlayer(float hp)
    {
        maxHP += hp;
    }


}