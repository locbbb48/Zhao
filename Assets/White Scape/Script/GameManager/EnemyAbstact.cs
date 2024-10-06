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
    [SerializeField] protected float _speed;



    protected virtual void Awake()
    {

    }
}
