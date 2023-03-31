using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float dragSpeed = 0.5f;
    public float minPitch = -85;
    public float maxPitch = 85;
    public float lerpSpeed = 10;

    private bool isDragging = false;
    private bool onDragging = false;

    private Vector3 dragStartMousePosition;
    private Quaternion dragStartRotation;

    private float directionX = -1;
    private float directionY = -1;

    [SerializeField] private Camera uiCamera;
    [SerializeField] private Camera measureCamera;
    [SerializeField] private InputSystem inputSystem;

    private void Start()
    {
        Application.targetFrameRate = 60;
    }
    public void StartDrag()
    {
        // Start dragging the camera
        isDragging = true;
        onDragging = false;
        dragStartMousePosition = Input.mousePosition;
        dragStartRotation = transform.rotation;
    }

    public void UpdateRotation()
    {
        if (inputSystem.GetControlState() == InputSystem.ControlState.AutoTour)
            return;

        // Update the rotation of the camera based on the mouse delta
        Vector3 mouseDelta =  Input.mousePosition - dragStartMousePosition;

        mouseDelta = new Vector3(mouseDelta.x * directionX, mouseDelta.y * directionY, mouseDelta.z);

        if (mouseDelta.magnitude < 20 && !onDragging)
            return;

        onDragging = true;

        float angleX = dragStartRotation.eulerAngles.x;

        angleX %= 360;
        if (angleX > 180)
            angleX = angleX - 360;

        //print("dragStartRotation.eulerAngles.x - mouseDelta.y * dragSpeed" + (dragStartRotation.eulerAngles.x - mouseDelta.y * dragSpeed));
        float pitch = Mathf.Clamp(angleX - mouseDelta.y * dragSpeed, minPitch, maxPitch);

        //print("Pitch :" + pitch);
        Quaternion targetRotation = Quaternion.Euler(pitch, dragStartRotation.eulerAngles.y + mouseDelta.x * dragSpeed, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, lerpSpeed * Time.deltaTime);

        transform.eulerAngles = new Vector3(transform.eulerAngles.x , transform.eulerAngles.y , 0);
    }

    IEnumerator moveObject = null;
    IEnumerator moveCamera = null;
    private bool cameraOnMove = false;
    public IEnumerator MoveCam(Vector3 input , Material[] materials)
    {
        //transform.position = input;
        if (moveObject == null && moveCamera == null)
        {
            cameraOnMove = true;
            moveObject = MoveObject(input , materials);
            moveCamera = MoveCamera(input);
            StartCoroutine(moveCamera);
            yield return StartCoroutine(moveObject);
            cameraOnMove = false;
        }
    }

    public void MoveCamInstant(Vector3 pos , Vector3 rot, float fov)
    {
        transform.position = pos;
        transform.eulerAngles = rot;
        //Camera.main.fieldOfView = fov;
        SetFovTogether(fov);
    }

    public bool GetCameraOnMove()
    {
        return cameraOnMove;
    }

    float moveTime = 1f;    // 이동 시간
    float imageTransTime = 1f;

    public void SetMoveTime(float time)
    {
        moveTime = time;
    }


    public void SetImageTransTime(float time)
    {
        imageTransTime = time;
    }

    IEnumerator MoveCamera(Vector3 targetPosition)
    {   
        float elapsedTime = 0.0f;
        Vector3 startPosition = transform.position;
  
        float multMoveTime = moveTime ;

        while (elapsedTime < multMoveTime)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, (elapsedTime / multMoveTime));

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;

        moveObject = null;

    }
    IEnumerator MoveObject(Vector3 targetPosition , Material[] materials )
    {
        float elapsedTime = 0.0f;
        Vector3 startPosition = transform.position;
        float distance = Vector3.Distance(startPosition,targetPosition);

        while (elapsedTime < imageTransTime)
        {
            foreach (Material m in materials)
            {
                UnityEngine.Color color = UnityEngine.Color.white;
                color.a = 1 - elapsedTime / imageTransTime;
                m.SetColor("_Color" , color);
            }       

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        foreach (Material m in materials)
        {
            m.SetColor("_Color", UnityEngine.Color.white);
        }
        moveCamera = null;
        moveObject = null;
    }

    public void SetFovTogether(float fov)
    {
        uiCamera.fieldOfView = fov;
        Camera.main.fieldOfView = fov;
        //measureCamera.fieldOfView = fov;
    }

    public IEnumerator LinearLookAt(Vector3 pos , float iterTime)
    {
        float time = 0;

        Vector3 direction = pos - transform.position;
        direction.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        while (iterTime > time)
        {
            time += Time.deltaTime;
            transform.rotation = Quaternion.Slerp(transform.rotation,targetRotation, 1);
          
            yield return null;
        }

        //transform.LookAt(pos);
    }
}
