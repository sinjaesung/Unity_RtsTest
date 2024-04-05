using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    void Start()
    {
        UnitSelectionManager.Instance.allUnitsList.Add(gameObject);
    }

    private void OnDestroy()
    {
        Debug.Log("Unit Object Destroyed");
        UnitSelectionManager.Instance.allUnitsList.Remove(gameObject);
    }
}
