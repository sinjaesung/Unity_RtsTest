using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class RtsCameraController : MonoBehaviour
{
    public static RtsCameraController instance;

    [Header("General")]
    [SerializeField] Transform cameraTransform;
    public Transform followTransform;
    Vector3 newPosition;
    Vector3 dragStartPosition;
    Vector3 dragCurrentPosition;

    [Header("Optional Funtionality")]
    [SerializeField] bool moveWithKeyboard;
    [SerializeField] bool moveWithEdgeScrolling;
    [SerializeField] bool moveWithMouseDrag;

    [Header("Keyboard Movement")]
    [SerializeField] float fastSpeed = 0.05f;
    [SerializeField] float normalSpeed = 0.01f;
    [SerializeField] float movementSensitivity = 0.5f;
    float movementSpeed;

    [Header("Edge Scrolling Movement")]
    [SerializeField] float edgeSize = 50f;
    bool isCursorSet = false;
    public Texture2D cursorArrowUp;
    public Texture2D cursorArrowDown;
    public Texture2D cursorArrowLeft;
    public Texture2D cursorArrowRight;

    CursorArrow currentCursor = CursorArrow.DEFAULT;

    enum CursorArrow
    {
        UP,
        DOWN,
        LEFT,
        RIGHT,
        DEFAULT
    }

    private void Start()
    {
        instance = this;

        newPosition = transform.position;

        movementSpeed = normalSpeed;
    }

    private void Update()
    {
        //Allow Camera to follow Target
        if(followTransform != null)
        {
            transform.position = followTransform.position;
        }
        //Let us Control Camera
        else
        {
            HandleCameraMovement();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            followTransform = null;
        }
    }

    void HandleCameraMovement()
    {
        //Mouse Drag
        if (moveWithMouseDrag)
        {
            HandleMouseDragInput();
        }

        //Keyboard Control
        if (moveWithKeyboard)
        {
            if (Input.GetKey(KeyCode.LeftCommand))
            {
                movementSpeed = fastSpeed;
            }
            else
            {
                movementSpeed = normalSpeed;
            }

            if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                newPosition += (transform.forward * movementSpeed);
            }
            if(Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                newPosition += (transform.forward * -movementSpeed);
            }
            if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                newPosition += (transform.right * movementSpeed);
            }
            if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                newPosition += (transform.right * -movementSpeed);
            }
        }

        //Edge Scrolling
        if (moveWithEdgeScrolling)
        {

            //Move Right
            if(Input.mousePosition.x > Screen.width - edgeSize)
            {
                newPosition += (transform.right * movementSpeed);
                ChangeCursor(CursorArrow.RIGHT);
                isCursorSet = true;
            }

            //Move Left
            else if(Input.mousePosition.x < edgeSize)
            {
                newPosition += (transform.right * -movementSpeed);
                ChangeCursor(CursorArrow.LEFT);
                isCursorSet = true;
            }

            //Move Up
            else if(Input.mousePosition.y > Screen.height - edgeSize)
            {
                newPosition += (transform.forward * movementSpeed);
                ChangeCursor(CursorArrow.UP);
                isCursorSet = true;
            }

            //Move Down
            else if(Input.mousePosition.y < edgeSize)
            {
                newPosition += (transform.forward * -movementSpeed);
                ChangeCursor(CursorArrow.DOWN);
                isCursorSet = true;
            }

            else
            {
                if (isCursorSet)
                {
                    ChangeCursor(CursorArrow.DEFAULT);
                    isCursorSet = false;
                }
            }
        }

        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementSensitivity);

       // Cursor.lockState = CursorLockMode.Confined;//If we have an extra monitor we don't want to exit screen bounds
    }

    private void ChangeCursor(CursorArrow newCursor)
    {
        //Only change cursor if its not the same cursor
        if (currentCursor != newCursor)
        {
            switch (newCursor)
            {
                case CursorArrow.UP:
                    Cursor.SetCursor(cursorArrowUp, Vector2.zero, CursorMode.Auto);
                    break;
                case CursorArrow.DOWN:
                    Cursor.SetCursor(cursorArrowDown, new Vector2(cursorArrowDown.width, cursorArrowDown.height), CursorMode.Auto);
                    break;
                case CursorArrow.LEFT:
                    Cursor.SetCursor(cursorArrowLeft, Vector2.zero, CursorMode.Auto);
                    break;
                case CursorArrow.RIGHT:
                    Cursor.SetCursor(cursorArrowRight, new Vector2(cursorArrowRight.width, cursorArrowRight.height), CursorMode.Auto);
                    break;
                case CursorArrow.DEFAULT:
                    Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
                    break;
            }

            currentCursor = newCursor;
        }
    }

    private void HandleMouseDragInput()
    {
        if(Input.GetMouseButtonDown(2) && EventSystem.current.IsPointerOverGameObject() == false)
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float entry;

            if(plane.Raycast(ray,out entry))
            {
                Debug.Log("GetMouseButtonDown(2) ray and out entry:" + ray.origin + "," + ray.direction + ",|entry:" + entry);
                dragStartPosition = ray.GetPoint(entry);
                Debug.Log("GetMouseButtonDown(2) dragStartPosition:" + dragStartPosition);
            }
        }

        if(Input.GetMouseButton(2) && EventSystem.current.IsPointerOverGameObject() == false)
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float entry;

            if(plane.Raycast(ray,out entry))
            {
                Debug.Log("GetMouseButton(2) ray and out entry:" + ray.origin + "," + ray.direction + ",|entry:" + entry);
                dragCurrentPosition = ray.GetPoint(entry);
                Debug.Log("GetMouseButton(2) dragCurrentPosition:" + dragCurrentPosition);

                newPosition = transform.position + (dragStartPosition - dragCurrentPosition);
            }
        }
    }
}
