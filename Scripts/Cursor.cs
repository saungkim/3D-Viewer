using UnityEngine;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine.UIElements;

public class Cursor : MonoBehaviour
{
    [SerializeField] private Collider coll;
    [SerializeField] private Transform cursor;

    [SerializeField] private Transform gauge;

    private Vector3 originScale;

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

            gauge.position = hit.point + hit.normal * 0.001f;
            gauge.LookAt(hit.point + hit.normal);
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
  
}