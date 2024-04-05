using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitMovement : MonoBehaviour
{
    Camera cam;
    NavMeshAgent agent;
    public LayerMask ground;

    public bool isCommandedToMove;

    private void Start()
    {
        cam = Camera.main;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray,out hit,Mathf.Infinity,ground))
            {
                isCommandedToMove = true;
                agent.SetDestination(hit.point);//이동을 가능하게 NavMeshAgent를 이동가능Enable된 녀석들만 이동된다(선택한 녀석들만)
            }
        }

        //Agent reached destination
        if(agent.hasPath == false || agent.remainingDistance <= agent.stoppingDistance)
        {
            isCommandedToMove = false;
        }
    }
}
