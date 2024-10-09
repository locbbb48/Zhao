/*	  - Codeby Bui Thanh Loc -
	contact : builoc08042004@gmail.com
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrackWall : ObjectAbstract
{
    [SerializeField] private ParticleSystem hitEffect;
    [SerializeField] private bool isCol = false;

    protected override void Start()
    {
        base.Start();
        currentHP = 50;
        maxHP = 100;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == GameManager.Instance.player.gameObject)
        {
            isCol = true;
            StartCoroutine(CheckForDamage(collision));
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject == GameManager.Instance.player.gameObject)
        {
            isCol = false;
        }
    }

    private IEnumerator CheckForDamage(Collision2D collision)
    {
        while (isCol)
        {
            if (Input.GetKeyDown(KeyCode.Return) && GameManager.Instance.isHaveDagger)
            {

                ParticleSystem particle = ParticlePool.Instance.GetObject();
                particle.transform.position = transform.position;
                particle.gameObject.SetActive(true);

                yield return new WaitForSeconds(GameManager.Instance.player.getAttackCooldown());

                ParticlePool.Instance.ReturnObject(particle);
            }

            if (currentHP <= 0)
            {
                gameObject.SetActive(false);
                yield break;
            }

            yield return null;
        }
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        UIManager.Instance.ShowDamage(-damage, transform.position + new Vector3(0, 1f, -10), Color.white);
    }

    protected override void OnDeath()
    {
        base.OnDeath();
        gameObject.SetActive(false);
    }
}
