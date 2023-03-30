using UnityEngine;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class ViewerCursor : MonoBehaviour
{
    
    [SerializeField] public Transform cursor;

    [SerializeField] private Transform gauge;

    private Vector3 originScale;

    [SerializeField] private GameObject cursorChild;

    private Vector3 gaugePlusPosition = new Vector3(120,170,0);

    [SerializeField] private InputSystem inputSystem;
    [SerializeField] private Transform measureRenderUI;
    //[SerializeField] private 

    void Start()
    {
       originScale = cursor.transform.localScale;
       UnityEngine.Cursor.visible = false;
       UnityEngine.Cursor.lockState = CursorLockMode.Confined;
       
    }

    void Update()
    {
        // Move this object to the position clicked by the mouse.

        SetScaleFov();

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

   
        if (Physics.Raycast(ray, out hit,100))
        {
            //Debug.Log("Hit:" + hit.transform.name);
            //transform.position = ray.GetPoint(100.0f);
            cursor.position = hit.point + hit.normal * 0.001f;
            cursor.LookAt(hit.point + hit.normal);
            
            if(inputSystem.GetControlState() == InputSystem.ControlState.Measure)
            {
                gauge.position = measureRenderUI.position;
            }
            else
            {
                gauge.position = Input.mousePosition + gaugePlusPosition;
            }
           
            //gauge.LookAt(hit.point + hit.normal);
        }
    }

    public Vector3 GetCursorPoint()
    {
        return cursor.position;
    }

    public void SetInvisible()
    {
        cursor.gameObject.SetActive(false);
    }

    public void SetVisible()
    {
        cursor.gameObject.SetActive(true);
    }

    public void SetGaugeVisible()
    {
        gauge.gameObject.SetActive(true);
    }

    public void SetGaugeInvisible()
    {
        gauge.gameObject.SetActive(false);
    }

    public void SetScaleFov()
    {
        cursor.transform.localScale = originScale   * Camera.main.fieldOfView / 60  ;
    }

    public void SetActivate(bool onOff)
    {
        cursorChild.SetActive(onOff);
    }
  
    public void SetMeasureCursorMode()
    {
        SetVisible();
        cursor.gameObject.layer = LayerMask.NameToLayer("CubeMap");
        cursor.localPosition = new Vector3(0.4f,0.005f,0.4f);
    }

    public void SetNormalcursorMode()
    {
        cursor.gameObject.layer = LayerMask.NameToLayer("Default");
        cursor.localPosition = new Vector3(0.14f, 0.005f, 0.14f);
    }
  
}