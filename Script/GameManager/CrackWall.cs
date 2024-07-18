/*    - Codeby Bui Thanh Loc -
    contact : builoc08042004@gmail.com
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrackWall : ObjectAbstact
{
    [SerializeField] private ParticleSystem hitEffect;

    protected override void Start()
    {
        base.Start();
        currentHP = 50;
        maxHP = 100;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && GameManager.Instance.isHaveDagger)
        {
            StartCoroutine(CheckForDamage());
        }
    }

    private IEnumerator CheckForDamage()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                TakeDamage(10);

                ParticleSystem particle = ParticlePool.Instance.GetObject();
                particle.transform.position = transform.position;
                particle.gameObject.SetActive(true);


                yield return new WaitForSeconds(particle.main.duration);

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

    protected override void OnDeath()
    {
        base.OnDeath();
        gameObject.SetActive(false);
    }
}
