/*	  - Codeby Bui Thanh Loc -
	contact : builoc08042004@gmail.com
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrackWall : ObjectAbstract
{
    [SerializeField] private ParticleSystem hitEffect;

    protected override void Start()
    {
        base.Start();
        currentHP = 50;
        maxHP = 100;
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
        UIManager.Instance.ShowDamage(-damage, transform.position + new Vector3(0, 1f, -10), Color.white);
    }

    protected override void OnDeath()
    {
        base.OnDeath();
        gameObject.SetActive(false);
    }
}
