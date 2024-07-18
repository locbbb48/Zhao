/*	  - Codeby Bui Thanh Loc -
	contact : builoc08042004@gmail.com
*/

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : ObjectAbstact
{

    private Animator animator;
    [SerializeField] private float H, V;
    [SerializeField] private Vector2 movement;
    [SerializeField] private Vector2 lastMovement;
    [SerializeField] private float _speed = 5f;

    [SerializeField] private Transform flashlight;

    public TMP_Text HPText;

    protected override void Start()
    {
        base.Start();
        speed = _speed;
        animator = GetComponent<Animator>();
        HPText.text = currentHP.ToString();

        StartCoroutine(RemoveHP());
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
        Fight();
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

    void Fight()
    {
        if(Input.GetKeyDown(KeyCode.Return) && GameManager.Instance.isHaveDagger)
        {
            animator.SetTrigger("fight");
        }
    }

    void RotateFlashlight()
    {
        if (movement != Vector2.zero)
        {
            float angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg;
            flashlight.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }

    protected IEnumerator RemoveHP()
    {
        while (currentHP > 0)
        {
            TakeDamage(1);
            yield return new WaitForSeconds(1f);
        }
    }
}