/*	  - Codeby Bui Thanh Loc -
	contact : builoc08042004@gmail.com
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class ObjectAbstract : MonoBehaviour
{
    [SerializeField] protected float maxHP;
    [SerializeField] protected float currentHP;
    [SerializeField] protected float speed = 1;

    [SerializeField] protected Slider healthBar;

    protected virtual void Start()
    {
        healthBar.maxValue = maxHP;
        currentHP = maxHP;
        UpdateHealthBar();
    }

    //Khi gọi hàm này, nên nhớ set UIShowDame(amount là damage) với color tùy chỉnh: UIManager.Instance.ShowDamage(-damage, transform.position + new Vector3(0, 1f, -10), Color.white);
    public virtual void TakeDamage(float amount)
    {
        if (gameObject.activeInHierarchy && gameObject != null)
        {
            currentHP -= amount;
            if (currentHP < 0)
            {
                currentHP = 0;
                OnDeath();
            }
            UpdateHealthBar();
        }
        else
        {
            Debug.Log($"{GetType().Name} has been deactived or is null");
        }
    }

    public virtual void Heal(float amount)
    {
        currentHP += amount;
        if (currentHP > maxHP)
        {
            currentHP = maxHP;
        }
        UpdateHealthBar();
    }

    protected void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.value = currentHP;
        }
    }

    protected virtual void OnDeath()
    {
        Debug.Log($"{GetType().Name} has died");
    }
}
