using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSystem : MonoBehaviour
{
    private bool isDragging = false;
    private bool onClickStart = false;
    private bool click = false;
    private bool hold = false;
    private bool imgsFDDone = false;

    [Header("Container")]
    [SerializeField] private Cursor cursor;
    [SerializeField] private CameraController camController;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private ImgsFillDynamic ImgsFD;

    [SerializeField] private GameObject dot;

    float dragTime;
    float holdTime;

    private Vector3 firstMousePos;
    float currentPinchDistance;
    // Start is called before the first frame update



    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PinCh())
        {
            onClickStart = false;
            return;
        }

        zoomInOutWithWheel();

        if (Input.GetMouseButtonDown(0))
        {
           dragTime = 0;

           holdTime = 0;

           firstMousePos = Input.mousePosition;

           onClickStart = true;

           imgsFDDone = false;
        }
        else if (Input.GetMouseButton(0))
        {
            if (!onClickStart)
                return;

            dragTime += Time.deltaTime;

            //print("Dragging");
            if (isDragging)
            {
                if(Vector3.Distance(firstMousePos,Input.mousePosition) < 10)
                {
                    holdTime += Time.deltaTime;
                    
                    if(holdTime > 0.7f)
                    {
                        hold = true;
                    }

                    if (hold)
                    {
                        //ImgsFD.AllChildActivate(true);
                        //ImgsFD.SetValue( (holdTime - 0.7f) / 1f, true);
                    }

                    if (holdTime - 0.7f >= 1f)
                    {
                        if (!imgsFDDone)
                        {
                            //Instantiate(ImgsFD.transform.parent.parent.parent.gameObject);
                            //ImgsFD.CreateInstance();
                            imgsFDDone = true;
                            GameObject o = Instantiate(dot);
                            o.SetActive(true);
                            o.transform.position = ImgsFD.transform.position;
                            o.transform.rotation = ImgsFD.transform.rotation;

                            dot.GetComponent<DefectDot>().CreateToServer(o.transform.position);
                        }
                        
                    }
                }
                else
                {
                    holdTime = 0;
                }

                if(!hold)
                {
                    camController.UpdateRotation();

                }

            }
            else
            {
                cursor.SetInvisible();
                camController.StartDrag();
                // Start dragging the camera
                isDragging = true;
              
            }
        }
        else if(Input.GetMouseButtonUp(0))
        {
            if (isDragging)
            {
                cursor.SetVisible();
            }

            if(Vector3.Distance(firstMousePos,Input.mousePosition) < 10 && dragTime < 0.5f)
            {
                if (!camController.GetCameraOnMove())
                {
                    StartCoroutine(playerMovement.MoveStage());
                }
            }

            isDragging = false;

            //ImgsFD.AllChildActivate(false);
            //ImgsFD.SetValue(0, true);

            

            hold = false;
        }

        
    }

    const float zoomSpeed = 2.5f;
    private void zoomInOutWithWheel()
    {
       //float distance = Input.mouseScrollDelta.y;
       

        float fov = Camera.main.fieldOfView;
        fov -= Input.mouseScrollDelta.y * zoomSpeed;
        Camera.main.fieldOfView = Mathf.Clamp(fov, 20, 70);
    }

    private bool PinCh()
    {
        if (Input.touchCount == 2)
        {

            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPos = touchZero.position;
            Vector2 touchOnePos = touchOne.position;

            float distance = Vector2.Distance(touchZeroPos, touchOnePos);

            if (touchZero.phase == TouchPhase.Began || touchOne.phase == TouchPhase.Began)
            {
                currentPinchDistance = distance;
            }

            float deltaDistance = distance - currentPinchDistance;
            float zoomSpeed = 0.1f;

            float fov = Camera.main.fieldOfView;
            fov -= deltaDistance * zoomSpeed;
           
            Camera.main.fieldOfView = Mathf.Clamp(fov, 20, 70);

           currentPinchDistance = distance;

           return true;
        }

        return false;
    }
}
