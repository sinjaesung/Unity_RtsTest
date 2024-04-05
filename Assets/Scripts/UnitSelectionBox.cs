using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UnitSelectionBox : MonoBehaviour
{
    Camera myCam;

    [SerializeField]
    RectTransform boxVisual;

    Rect selectionBox;

    Vector2 startPosition;
    Vector2 endPosition;

    private void Start()
    {
        myCam = Camera.main;
        startPosition = Vector2.zero;
        endPosition = Vector2.zero;
        DrawVisual();
    }

    private void Update()
    {
        //When Clicked
        if (Input.GetMouseButtonDown(0))
        {
            startPosition = Input.mousePosition;

            //For selection the Units
            selectionBox = new Rect();
        }

        //When Dragging
        if (Input.GetMouseButton(0))
        {
            if(boxVisual.rect.width >0 || boxVisual.rect.height > 0)
            {
                UnitSelectionManager.Instance.DeselectAll();//LeftShift키를 누르고 not clickable영역 클릭되어도 무조건DeSelect
                SelectUnits();
            }

            endPosition = Input.mousePosition;
            DrawVisual();
            DrawSelection();
        }

        //When Releasing
        if (Input.GetMouseButtonUp(0))
        {
            SelectUnits();

            startPosition = Vector2.zero;
            endPosition = Vector2.zero;
            DrawVisual();
        }
    }

    void DrawVisual()
    {
        //Calculate the starting and ending positions of the selection box.
        Vector2 boxStart = startPosition;
        Vector2 boxEnd = endPosition;

        //Calculate the center of the selection box
        Vector2 boxCenter = (boxStart + boxEnd) / 2;

        //Set the position of the visual selection box based on its center
        boxVisual.position = boxCenter;

        //Calculate the size of the selection box in both width and height.
        Vector2 boxSize = new Vector2(Mathf.Abs(boxStart.x - boxEnd.x), Mathf.Abs(boxStart.y - boxEnd.y));

        //Set the size of the visual selection box based on its calculated size
        boxVisual.sizeDelta = boxSize;
    }

    void DrawSelection()
    {
        if(Input.mousePosition.x < startPosition.x)
        {
            selectionBox.xMin = Input.mousePosition.x;//드래깅x포인트위치가 드래깅시작x지점보다 작은경우,좌측x방향으로 드래깅하는경우로 현재드래깅포인트x위치가 박스의 xMin 
            selectionBox.xMax = startPosition.x;//좌측x방향으로 드래깅하는것이기에 xMax마지막은 드래깅시작x포인트다.
        }
        else
        {
            selectionBox.xMin = startPosition.x;//드래깅x포인트위치가 드래깅시작x지점보다 큰경우,우측x방향으로 드래깅하는경우, 드래깅시작 x위치가 박스의 xMin
            selectionBox.xMax = Input.mousePosition.x;//우측x방향으로 드래깅하는것이기에 xMax는 현재드래깅하고있는 마우스x 위치이다.
        }

        if(Input.mousePosition.y < startPosition.y)
        {
            selectionBox.yMin = Input.mousePosition.y;//드래깅y포인트위치가 드래깅시작y지점보다 더 작은경우, 아래y방향으로 드래깅하는경우로
                                                      //아래뱡향으로 드래깅하고있는현재 마우스y자체가 yMin이됨
            selectionBox.yMax = startPosition.y; //아래y방향으로 드래깅하는것이기에 yMax는 드래깅시작y위치이다.
        }
        else
        {
            selectionBox.yMin = startPosition.y;//드래깅y포인트위치가 드래깅시작y지점보다 더 큰경우,위y방향으로 드래깅하는경우로
                                                //드래깅시작y포인트위치가 yMin이됨
            selectionBox.yMax = Input.mousePosition.y; //위y방향으로 드래깅하는것이기에 yMax는 현재드래깅하고있는 마우스y위치이다.
        }
    }

    void SelectUnits()
    {
        foreach(var unit in UnitSelectionManager.Instance.allUnitsList)
        {
            if (selectionBox.Contains(myCam.WorldToScreenPoint(unit.transform.position))){
                UnitSelectionManager.Instance.DragSelect(unit);
            }
        }
    }
}
