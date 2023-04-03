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
    [SerializeField] private InputSystem inputSystem;


    private Vector3 measurePlusPos = new Vector3(0,330,0);
    private MeasurementDot selectedMeasurementDot;
    private LineRenderer selectedLine;
    private TextMeshProUGUI selectedText;
    [SerializeField] private MeasurementDot measurementDot;

    bool startMeasureMent = false;

    enum MeasurementState {None,Start,Connect,End}
    MeasurementState measureMentState = MeasurementState.None;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (measureMentState == MeasurementState.None)
            return;

        if (inputSystem.GetCursorOnUI())
        {
            measureRenderUI.gameObject.SetActive(false);
            return;
        }
        else
        {
            measureRenderUI.gameObject.SetActive(true);
        }

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
        bool onOff = !measureCamera.gameObject.activeSelf;

        measureRenderUI.gameObject.SetActive(onOff);
        measureCamera.gameObject.SetActive(onOff);

        if (onOff)
        {
            measureMentState = MeasurementState.Start;
            cursor.SetMeasureCursorMode();
        }
        else
        {
            measureMentState = MeasurementState.None;
            DestroySelectedObjects();
            cursor.SetNormalCursorMode();
        }

    }

    public void ActivateMeasurement(bool onOff)
    {
        measureRenderUI.gameObject.SetActive(onOff);
        measureCamera.gameObject.SetActive(onOff);

        if (!onOff)
        {
            DestroySelectedObjects();
            measureMentState = MeasurementState.None;
            selectedLine = null;
        } 

           
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
            selectedLine.transform.parent = measurement;
            selectedLine.gameObject.SetActive(true);
            selectedLine.GetComponent<MeasurementLine>().SetStartDot(selectedMeasurementDot.transform);
        }else if (measureMentState == MeasurementState.Connect)
        {
            selectedMeasurementDot.SelectDot(false);
            print("Done");
            //selectedLine.GetComponent<MeasurementLine>().SetStartDot(selectedMeasurementDot.transform);
            measureMentState = MeasurementState.Start;
            selectedLine = null;
        }
       
  
   
    }

    public void SelectMeasurementDone()
    {
        startMeasureMent = true;
        //CreateMeasurementDot();
    }

    private void DestroySelectedObjects()
    {
        if (selectedLine == null)
            return;
        Destroy(selectedLine);
        Destroy(selectedMeasurementDot.gameObject);
        Destroy(selectedText);
    }

}
