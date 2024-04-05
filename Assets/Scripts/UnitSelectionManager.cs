using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelectionManager : MonoBehaviour
{
    public static UnitSelectionManager Instance { get; set; }

    public List<GameObject> allUnitsList = new List<GameObject>();
    public List<GameObject> unitsSelected = new List<GameObject>();

    public LayerMask clickable;
    public LayerMask ground;
    public LayerMask attackable;
    public bool attackCursorVisible;
    public GameObject groundMarker;

    public Camera cam;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            //If we are hitting a clickable object
            if(Physics.Raycast(ray,out hit, Mathf.Infinity, clickable))
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    MultiSelect(hit.collider.gameObject);
                }
                else
                {
                    SelectByClicking(hit.collider.gameObject);
                }
            }
            else //If we are not hitting a clickable object Left키를 누르지않은채로 not clickable영역을 클릭된경우(notclickable포함되는영역에전체적 드래그하고끝난경우)도 포함.
            {
                if (Input.GetKey(KeyCode.LeftShift) == false)
                {
                    Debug.Log("LeftShift키 누르지않고 not cilckable영역 좌마우스버튼클릭한이 된 모든 경우 DeselectAllTargets");
                    DeselectAll();
                }
                else
                {
                    Debug.Log("LeftShift키 누르고 not clickable영역 클릭(좌마우스버튼)한 경우");
                }
            }
        }

        if (Input.GetMouseButtonDown(1) && unitsSelected.Count > 0)
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            //If we are hitting a clickable object
            if(Physics.Raycast(ray,out hit, Mathf.Infinity, ground))
            {
                groundMarker.transform.position = hit.point;

                groundMarker.SetActive(false);
                groundMarker.SetActive(true);
            }
        }

        //Attack Target
        if(unitsSelected.Count > 0 && AtleastOneOffensiveUnit(unitsSelected))
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            //If we are hitting a clickable object
            if(Physics.Raycast(ray,out hit, Mathf.Infinity, attackable))
            {
                Debug.Log("Enemy Hovered with mouse");

                attackCursorVisible = true;

                if (Input.GetMouseButtonDown(1))
                {
                    Transform target = hit.transform;

                    foreach(GameObject unit in unitsSelected)
                    {
                        if (unit.GetComponent<AttackController>())
                        {
                            unit.GetComponent<AttackController>().targetToAttack = target;
                        }
                    }
                }
            }
            else
            {
                attackCursorVisible = false;
            }
        }
    }

    private bool AtleastOneOffensiveUnit(List<GameObject> unitsSelected)
    {
        foreach(GameObject unit in unitsSelected)
        {
            if (unit.GetComponent<AttackController>())
            {
                return true;
            }
        }
        return false;
    }
    private void MultiSelect(GameObject unit)
    {
        if(unitsSelected.Contains(unit) == false)
        {
            unitsSelected.Add(unit);
            //TriggerSelectionIndicator(unit, true);
            //EnableUnitMovement(unit, true);
            SelectUnit(unit, true);
        }
        else
        {
            //EnableUnitMovement(unit, false);//선택해제
            //TriggerSelectionIndicator(unit, false);
            SelectUnit(unit, false);
            unitsSelected.Remove(unit);
        }
    }
    public void DeselectAll()
    {
        foreach(var unit in unitsSelected)
        {
            //EnableUnitMovement(unit, false);
            //TriggerSelectionIndicator(unit, false);
            SelectUnit(unit, false);
        }
        groundMarker.SetActive(false);
        unitsSelected.Clear();
    }
    internal void DragSelect(GameObject unit)
    {
        if (unitsSelected.Contains(unit) == false)
        {
            unitsSelected.Add(unit);
            // TriggerSelectionIndicator(unit, true);
            // EnableUnitMovement(unit, true);
            SelectUnit(unit, true);
        }
    }
    private void SelectByClicking(GameObject unit)
    {
        DeselectAll();

        unitsSelected.Add(unit);

        //TriggerSelectionIndicator(unit, true);
        //EnableUnitMovement(unit, true);
        SelectUnit(unit, true);
    }
    private void SelectUnit(GameObject unit,bool isSelected)
    {
        TriggerSelectionIndicator(unit, isSelected);
        EnableUnitMovement(unit, isSelected);
    }

    private void EnableUnitMovement(GameObject unit,bool shouldMove)
    {
        unit.GetComponent<UnitMovement>().enabled = shouldMove;
    }

    private void TriggerSelectionIndicator(GameObject unit,bool isVisible)
    {
        unit.transform.GetChild(0).gameObject.SetActive(isVisible);
    }
}
