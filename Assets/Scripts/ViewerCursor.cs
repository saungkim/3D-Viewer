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
    LayerMask layermask;
    void Start()
    {
       originScale = cursor.transform.localScale;
       UnityEngine.Cursor.visible = false;
       UnityEngine.Cursor.lockState = CursorLockMode.Confined;

       layermask = ~( 1 << LayerMask.NameToLayer("Measurement"));

    }

    void Update()
    {
        // Move this object to the position clicked by the mouse.

        SetScaleFov();

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit,100, layermask))
        {
            //Debug.Log("Hit:" + hit.transform.name);
            //transform.position = ray.GetPoint(100.0f);
            cursor.position = hit.point + hit.normal * 0.001f;
            cursor.LookAt(hit.point + hit.normal);

            gauge.position = Input.mousePosition + gaugePlusPosition;

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

    public void SetGaugeVisible(bool onOff)
    {
        gauge.gameObject.SetActive(onOff);
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
        print("SetMeasureCursorMode");
   
        cursorChild.SetActive(true);
        cursorChild.layer = LayerMask.NameToLayer("Cubemap");
        cursorChild.transform.localScale = new Vector3(0.04f,0.005f,0.04f);
    }

    public void SetNormalCursorMode()
    {
        print("SetMeasureCursorMode");
        //OverallSeting
        cursorChild.SetActive(false);
        cursor.gameObject.layer = LayerMask.NameToLayer("Default");
        cursor.localScale = new Vector3(0.14f, 0.005f, 0.14f);
    }
  
}