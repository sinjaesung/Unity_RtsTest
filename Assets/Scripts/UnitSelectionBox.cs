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
                UnitSelectionManager.Instance.DeselectAll();//LeftShiftŰ�� ������ not clickable���� Ŭ���Ǿ ������DeSelect
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
            selectionBox.xMin = Input.mousePosition.x;//�巡��x����Ʈ��ġ�� �巡�����x�������� �������,����x�������� �巡���ϴ°��� ����巡������Ʈx��ġ�� �ڽ��� xMin 
            selectionBox.xMax = startPosition.x;//����x�������� �巡���ϴ°��̱⿡ xMax�������� �巡�����x����Ʈ��.
        }
        else
        {
            selectionBox.xMin = startPosition.x;//�巡��x����Ʈ��ġ�� �巡�����x�������� ū���,����x�������� �巡���ϴ°��, �巡����� x��ġ�� �ڽ��� xMin
            selectionBox.xMax = Input.mousePosition.x;//����x�������� �巡���ϴ°��̱⿡ xMax�� ����巡���ϰ��ִ� ���콺x ��ġ�̴�.
        }

        if(Input.mousePosition.y < startPosition.y)
        {
            selectionBox.yMin = Input.mousePosition.y;//�巡��y����Ʈ��ġ�� �巡�����y�������� �� �������, �Ʒ�y�������� �巡���ϴ°���
                                                      //�Ʒ��������� �巡���ϰ��ִ����� ���콺y��ü�� yMin�̵�
            selectionBox.yMax = startPosition.y; //�Ʒ�y�������� �巡���ϴ°��̱⿡ yMax�� �巡�����y��ġ�̴�.
        }
        else
        {
            selectionBox.yMin = startPosition.y;//�巡��y����Ʈ��ġ�� �巡�����y�������� �� ū���,��y�������� �巡���ϴ°���
                                                //�巡�����y����Ʈ��ġ�� yMin�̵�
            selectionBox.yMax = Input.mousePosition.y; //��y�������� �巡���ϴ°��̱⿡ yMax�� ����巡���ϰ��ִ� ���콺y��ġ�̴�.
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
