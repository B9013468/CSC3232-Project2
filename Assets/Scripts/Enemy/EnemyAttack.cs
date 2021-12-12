using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    BoxCollider weaponCollider;

    Animator m_animator;

    private bool attack = false;

    // Start is called before the first frame update
    void Start()
    {
        m_animator = GetComponent<Animator>();

        weaponCollider = GetComponent<BoxCollider>();

        weaponCollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (attack)
        {
            m_animator.SetTrigger("Hit");
            attack = false;
        }
    }

    public void AttackStart()
    {
        weaponCollider.enabled = true;
    }

    public void AttackEnd()
    {
        weaponCollider.enabled = false;
    }

    public void Hit()
    {
        attack = true;
    }

    /*void OnCollisionEnter(Collision collision)
    {
        // if enemy collides with sword
        if (collision.gameObject.CompareTag("Player"))
        {
            AttackEnd();
        }
    }*/
}

