using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class Measurement : MonoBehaviour
{

    [SerializeField] private Transform measureCamera;
    [SerializeField] private Transform cursorTransform;
    [SerializeField] private ViewerCursor cursor;
    [SerializeField] private RectTransform measureRenderUI;
    [SerializeField] private LineRenderer line;
    [SerializeField] private Transform measurement;

    private Vector3 measurePlusPos = new Vector3(0,330,0);
    private MeasurementDot selectedMeasurementDot;
    private LineRenderer selectedLine;
    private TextMeshProUGUI selectedText;
    [SerializeField] private MeasurementDot measurementDot;

    bool startMeasureMent = false;

    enum MeasurementState {Start,Connect,End}
    MeasurementState measureMentState = MeasurementState.Start;
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

        if(measureMentState == MeasurementState.Connect)
        {
            selectedLine.SetPosition(0, selectedMeasurementDot.transform.position);
            selectedLine.SetPosition(1, cursor.transform.position);
        }
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
            cursor.SetNormalCursorMode();
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

        o.transform.parent = measurement;

        if (selectedMeasurementDot != null)
        {
            selectedMeasurementDot.SelectDot(false);
           
        }

        selectedMeasurementDot = o.GetComponent<MeasurementDot>();

        //print(selectedMeasurementDot.transform.name);

        if(measureMentState == MeasurementState.Start)
        {
            measureMentState = MeasurementState.Connect;
            selectedLine = Instantiate(line);
            selectedLine.gameObject.SetActive(true);
        }else if (measureMentState == MeasurementState.Connect)
        {
            measureMentState = MeasurementState.Start;
            selectedLine = null;
        }
       
  
   
    }

    public void SelectMeasurementDone()
    {
        startMeasureMent = true;
        //CreateMeasurementDot();
    }

}
