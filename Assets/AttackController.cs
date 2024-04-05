using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    public Transform targetToAttack;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy") && targetToAttack == null)
        {
            Debug.Log("OnTriggerEnter¿Ã∞≈??:" + other.transform.name);
            targetToAttack = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Enemy") && targetToAttack != null)
        {
            targetToAttack = null;
        }
    }
}
