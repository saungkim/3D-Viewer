using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class InputSystem : MonoBehaviour
{
    private bool isDragging = false;
    private bool onClickStart = false;
    private bool hold = false;
    private bool imgsFDDone = false;
    private bool enableDot = false;

    [Header("Container")]
    [SerializeField] private ViewerCursor cursor;
    [SerializeField] private CameraController camController;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private ImgsFillDynamic ImgsFD;

    [SerializeField] private GameObject dot;
    [SerializeField] private DefectConstructor defectConstructor;
    [SerializeField] private Measurement measurement;
    [SerializeField] private OverallSetting overallSetting;

    [SerializeField] private Transform gauage;

    float dragTime;
    float holdTime;

    private Vector3 firstMousePos;
    float currentPinchDistance;
    // Start is called before the first frame update

    private float enableDotDistance = 50;

    public enum ControlState { None, Defect, Measure , AutoTour , Tag , MeasureDot }
    public ControlState controlState = ControlState.None;

    private bool cursorOnUI = false;

    int pointerID;
    void Start()
    {
#if UNITY_EDITOR
        pointerID = -1; //PC나 유니티 상에서는 -1
#elif UNITY_IOS || UNITY_IPHONE || UNITY_ANDROID
        pointerID = 0;  // 휴대폰이나 이외에서 터치 상에서는 0 
#endif
    }

    public void Init()
    {
        controlState = ControlState.None;
        cursorOnUI = false;
        this.enabled = false;
        isDragging = false;
        onClickStart = false;
        hold = false;
        imgsFDDone = false;
        dragTime = 0;
        holdTime = 0;
        firstMousePos = Vector3.zero;
        currentPinchDistance = 0;
    }

    // Update is called once per frame
    void Update()
    {


        if (Pinch())
        {
            onClickStart = false;
            return;
        }

         zoomInOutWithWheel();
   
        if (EventSystem.current.IsPointerOverGameObject(pointerID))
        {
            cursorOnUI = true;
        }
        else
        {
            cursorOnUI = false;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (cursorOnUI == true)
                return;

            MouseButtonDown();
        }
        else if (Input.GetMouseButton(0))
        {
            MouseButtonHold();
        }
        else if(Input.GetMouseButtonUp(0))
        {
            MouseButtonUp();
        }
    }

    const float zoomSpeed = 2.5f;
    private void zoomInOutWithWheel()
    {
        float fov = Camera.main.fieldOfView;
        fov -= Input.mouseScrollDelta.y * zoomSpeed;
        //Camera.main.fieldOfView = Mathf.Clamp(fov, 20, 70);
        camController.SetFovTogether(Mathf.Clamp(fov, 20, 70));
    }

    public bool GetCursorOnUI()
    {
        return cursorOnUI;
    }

    private void MouseButtonDown()
    {
        dragTime = 0;

        holdTime = 0;

        firstMousePos = Input.mousePosition;

        onClickStart = true;

        imgsFDDone = false;

        camController.StartDrag();
        measurement.StartDrag();
    }

    private void MouseButtonHold()
    {
        if (!onClickStart)
            return;

        dragTime += Time.deltaTime;

        if (isDragging)
        {
            if (Vector3.Distance(firstMousePos, Input.mousePosition) < enableDotDistance && controlState != ControlState.None)
            {
                holdTime += Time.deltaTime;

                if (holdTime > 0.7f)
                {
                    hold = true;
                }

                if (controlState == ControlState.Defect || controlState == ControlState.Measure)
                {
                    if ((holdTime - 0.7f) / 1f > 1)
                    {
                        StartCoroutine(ImgsFD.DelaySetActive(false));
                    }
                    else
                    {
                        StartCoroutine(ImgsFD.DelaySetActive(true));
                        ImgsFD.SetValue((holdTime - 0.7f) / 1f, true);
                    }
                }

                if (holdTime - 0.7f >= 1f)
                {
                    if (!imgsFDDone)
                    {
                        imgsFDDone = true;

                        if(controlState == ControlState.Defect)
                        {
                            defectConstructor.CreateDot(cursor.cursor.position, cursor.cursor.eulerAngles, true);
                        }
                        else if (controlState == ControlState.Measure)
                        {
                            measurement.DotCreateMode();
                        }
                    }
                }
            }
            else
            {
                holdTime = 0;
                StartCoroutine(ImgsFD.DelaySetActive(false));
            }

            if (!hold)
            {
                if(controlState != ControlState.MeasureDot)
                {

                    if (EventSystem.current.IsPointerOverGameObject(pointerID))
                    {
                        return;
                    }

                    camController.UpdateRotation();

                }
                else
                {
                    measurement.Drag();
                }
            }
        }
        else
        {
            isDragging = true;
            ImgsFD.SetActive(false);
        }
    }

    private void MouseButtonUp()
    {
        if (!onClickStart)
            return;

        if (holdTime < 0.5f)
        {
            if (controlState == ControlState.Defect && CheckDefectCollider())
                return;

            if (controlState == ControlState.Measure && CheckMeasurementCollider())
                return;
        }

        measurement.DragUp();

        if (isDragging)
        {
            Action delayCall = () => cursor.SetVisible();
            StartCoroutine(DelayCall(delayCall));
        }

        if (Vector3.Distance(firstMousePos, Input.mousePosition) < 10 && dragTime < 0.5f && controlState != ControlState.MeasureDot)
        {
            if (!camController.GetCameraOnMove())
            {
                StartCoroutine(playerMovement.MoveStage());
            }
        }

        isDragging = false;
        hold = false;
        ImgsFD.SetActive(false);
        onClickStart = false;
    }

    private bool CheckDefectCollider()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if(hit.transform.tag == "Defect")
            {           
                defectConstructor.SendMessageSelectDefect(hit.transform.GetSiblingIndex());
                return true;
            }
        }

        return false;
    }

    private bool CheckMeasurementCollider()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.transform.tag == "MeasurementDot")
            {    
                return true;
            }else if(hit.transform.tag == "MeasurementLine")
            {
                return true;
            }
        }

        return false;
    }

    private bool Pinch()
    {
        if (Input.touchCount == 2)
        {
            cursor.SetInvisible();

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
           
            camController.SetFovTogether(Mathf.Clamp(fov, 20, 70));

            currentPinchDistance = distance;

            return true;
        }
        return false;
    }

    public void SetEnableDot(bool onOff)
    {
        enableDot = onOff;
    }
    
    public IEnumerator DelayCall(Action func)
    {
        yield return new WaitForEndOfFrame();
        func.Invoke();
    }

    public void SetControlState(ControlState state)
    {
        controlState = state;
    }

    public ControlState GetControlState()
    {
        return controlState;
    }
}
