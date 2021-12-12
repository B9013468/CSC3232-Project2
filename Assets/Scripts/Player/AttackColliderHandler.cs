using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttackColliderHandler : MonoBehaviour
{
    /* Functions to use in weapon animation event */

    public void AttackStart()
    {
        GetComponent<BoxCollider>().enabled = true;
    }

    public void AttackEnd()
    {
        GetComponent<BoxCollider>().enabled = false;
    }

}
