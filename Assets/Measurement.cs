using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Measurement : MonoBehaviour
{

    [SerializeField] private Transform measureCamera;
    [SerializeField] private Transform cursorTransform;
    [SerializeField] private ViewerCursor cursor;
    [SerializeField] private RectTransform measureRenderUI;
    private Vector3 measurePlusPos = new Vector3(0,330,0);
    private MeasurementDot selectedMeasurementDot;
    [SerializeField] private MeasurementDot measurementDot;

    bool startMeasureMent = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        measureCamera.LookAt(cursorTransform);
        Vector3 pos = Camera.main.WorldToScreenPoint(cursorTransform.position) + measurePlusPos;
        measureRenderUI.position = pos;
    }

    public void InverseActivateMeasurement()
    {
        bool onOff = !measureRenderUI.gameObject.activeSelf;

        measureRenderUI.gameObject.SetActive(onOff);
        measureCamera.gameObject.SetActive(onOff);

        if (onOff)
        {
            cursor.SetMeasureCursorMode();
        }
        else
        {

        }

    }

    public void ActivateMeasurement(bool onOff)
    {
        measureRenderUI.gameObject.SetActive(onOff);
        measureCamera.gameObject.SetActive(onOff);
    }

    public void CreateMeasurementDot(Vector3 pos , Vector3 rot)
    {
        GameObject o = Instantiate(measurementDot, pos, Quaternion.Euler(rot)).gameObject;
        o.SetActive(true);
        if(selectedMeasurementDot != null)
        {
            selectedMeasurementDot.SelectDot(false);
            selectedMeasurementDot = o.GetComponent<MeasurementDot>();
        }
   
    }

    public void SelectMeasurementDone()
    {
        startMeasureMent = true;
        //CreateMeasurementDot();
    }

}
