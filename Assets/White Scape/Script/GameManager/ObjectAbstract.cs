/*	  - Codeby Bui Thanh Loc -
	contact : builoc08042004@gmail.com
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class ObjectAbstract : MonoBehaviour
{
    [SerializeField] protected float maxHP = 100;
    [SerializeField] protected float currentHP;
    [SerializeField] protected float speed = 1;

    [SerializeField] protected Slider healthBar;

    protected virtual void Start()
    {
        currentHP = maxHP;
        UpdateHealthBar();
    }

    public virtual void TakeDamage(float amount)
    {
        currentHP -= amount;
        if (currentHP < 0)
        {
            currentHP = 0;
            OnDeath();
        }
        UIManager.Instance.ShowDamage(-amount, transform.position + new Vector3(0, 1f, 0));
        UpdateHealthBar();
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
        Debug.Log("Object has died");
    }
}
