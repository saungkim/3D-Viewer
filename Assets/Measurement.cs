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
   

    public enum MeasurementState {None,Start,Connect,ConnectMove,End}
    public MeasurementState measureMentState = MeasurementState.None;
    // Start is called before the first frame updates
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (measureMentState == MeasurementState.None)
            return;

        if (inputSystem.GetCursorOnUI() )
        {
            measureRenderUI.gameObject.SetActive(false);
            
            return;
        }
        else if (measureMentState != MeasurementState.ConnectMove)
        {
            measureRenderUI.gameObject.SetActive(true);
        }

        measureCamera.LookAt(cursorTransform);
        Vector3 pos = Camera.main.WorldToScreenPoint(cursorTransform.position) + measurePlusPos;
        measureRenderUI.position = pos;

        if(measureMentState == MeasurementState.Connect || measureMentState == MeasurementState.ConnectMove)
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

        print("OnOffCamera" + measureCamera.gameObject.activeSelf + ":" + measureRenderUI.gameObject.activeSelf);

        if (measureMentState == MeasurementState.Connect )
        {
            measureMentState = MeasurementState.ConnectMove;
            return;
        }else if(measureMentState == MeasurementState.ConnectMove)
        {
            measureMentState = MeasurementState.Connect;
            return;
        }
        else
        {
            measureMentState = MeasurementState.Start;
            cursor.SetMeasureCursorMode();
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
        selectedMeasurementDot.SelectDot(true);
        //print(selectedMeasurementDot.transform.name);

        if (measureMentState == MeasurementState.Start)
        {
          
            measureMentState = MeasurementState.Connect;
            selectedLine = Instantiate(line);
            selectedLine.transform.parent = measurement;
            selectedLine.gameObject.SetActive(true);
            selectedLine.GetComponent<MeasurementLine>().SetStartDot(selectedMeasurementDot.transform);
        }else if (measureMentState == MeasurementState.Connect)
        {
           
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
        {
            if(selectedMeasurementDot != null)
            {
                selectedMeasurementDot.SelectDot(false);
            }
            return;
        }
        
      

        Destroy(selectedLine.gameObject);
        Destroy(selectedMeasurementDot.gameObject);
      
    }

}
