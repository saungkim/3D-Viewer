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
    [SerializeField] private GameObject measureUnit;

    private Vector3 measurePlusPos = new Vector3(0,330,0);
    private MeasurementDot selectedMeasurementDot;
    private LineRenderer selectedLine;
    private Transform selectedMeasureUnit;
    private TextMeshProUGUI selectedText;
    [SerializeField] private MeasurementDot measurementDot;

    bool startMeasureMent = false;
    private bool ondrag = false;

    public enum MeasurementState {None,DotCreating,DotCreateEnd,LineCreating}
    public MeasurementState measureMentState = MeasurementState.None;

    [SerializeField] private GameObject addUI;
    [SerializeField] private GameObject removeUI;

    private Transform dragDot;

    [SerializeField] private MeasurementUnit measurementUnit;
    private MeasurementUnit preMeasureUnit;

    // Start is called before the first frame updates
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
       

        if (measureMentState == MeasurementState.DotCreating)
        {
            measureCamera.LookAt(cursorTransform);
            Vector3 pos = Camera.main.WorldToScreenPoint(cursorTransform.position) + measurePlusPos;
            measureRenderUI.position = pos;
        }
        else if(measureMentState == MeasurementState.LineCreating)
        {
            //Vector3 pos = Camera.main.WorldToScreenPoint(cursorTransform.position) + measurePlusPos;
            preMeasureUnit.GetPrevLine().SetLineEndPosition(cursorTransform.position);
        }

        //if (measureMentState == MeasurementState.None)
        //    return;

        //if (inputSystem.GetCursorOnUI() )
        //{
        //    measureRenderUI.gameObject.SetActive(false);

        //    return;
        //}
        //else if (measureMentState == MeasurementState.Connect ||
        //    measureMentState == MeasurementState.Start)
        //{
        //    measureRenderUI.gameObject.SetActive(true);
        //}

        //measureCamera.LookAt(cursorTransform);
        //Vector3 pos = Camera.main.WorldToScreenPoint(cursorTransform.position) + measurePlusPos;
        //measureRenderUI.position = pos;

        //if(measureMentState == MeasurementState.Connect || measureMentState == MeasurementState.ConnectMove )
        //{
        //    if (selectedLine == null)
        //        return;

        //    if (ondrag)
        //    {
        //        selectedLine.SetPosition(0, selectedLine.transform.parent.GetChild(0).position);
        //        selectedLine.SetPosition(1, selectedLine.transform.parent.GetChild(2).position);

        //    }
        //    else
        //    {
        //        selectedLine.SetPosition(0, selectedMeasurementDot.transform.position);
        //        selectedLine.SetPosition(1, cursor.transform.position);
        //    }

        //}
    }

    public void InverseActivateMeasurement()
    {
        //if(measureMentState == MeasurementState.None)
        //{
        //    measureMentState = MeasurementState.Start;
        //}
        //else if(measureMentState == MeasurementState.Start)
        //{
        //    measureMentState = MeasurementState.None;
        //}


        //bool onOff = !measureCamera.gameObject.activeSelf;

        //measureRenderUI.gameObject.SetActive(onOff);
        //measureCamera.gameObject.SetActive(onOff);

        //print("OnOffCamera" + measureCamera.gameObject.activeSelf + ":" + measureRenderUI.gameObject.activeSelf);

        //if (measureMentState == MeasurementState.Connect )
        //{
        //    print("Inver Connect Move");


        //    measureMentState = MeasurementState.ConnectMove;
        //    return;
        //}else if(measureMentState == MeasurementState.ConnectMove)
        //{



        //    measureMentState = MeasurementState.Connect;
        //    return;
        //}
        //else if(measureMentState == MeasurementState.Start)
        //{
        //    InitSelect();
        //    selectedMeasurementDot = null;
        //    selectedMeasureUnit = null;
        //    selectedLine = null;

        //    measureMentState = MeasurementState.None;
        //    cursor.SetNormalCursorMode();
        //}
        //else 
        //{
        //    measureMentState = MeasurementState.Start;
        //    cursor.SetMeasureCursorMode();
        //}
        
    }

    public void ActivateMeasurement(bool onOff)
    {
        //measureRenderUI.gameObject.SetActive(onOff);
        //measureCamera.gameObject.SetActive(onOff);

        //if (!onOff)
        //{
        //    DestroySelectedObjects();
        //    measureMentState = MeasurementState.None;
        //    selectedLine = null;
        //}           
    }

    public void CreateMeasurementDot(Vector3 pos , Vector3 rot)
    {
        //GameObject o = Instantiate(measurementDot, pos, Quaternion.Euler(rot)).gameObject;
        //o.SetActive(true);

        //o.transform.parent = measurement;

        //if (selectedMeasurementDot != null)
        //{
        //    selectedMeasurementDot.SelectDot(false);
           
        //}

        //InitSelect();

        //selectedMeasurementDot = o.GetComponent<MeasurementDot>();
        //selectedMeasurementDot.SelectDot(true);
        
        ////print(selectedMeasurementDot.transform.name);
        //if (measureMentState == MeasurementState.Start)
        //{
        //    measureUnit = new GameObject();
        //    measureUnit.name = "MeasureUnit";
        //    measureUnit.transform.parent = measurement;
        //    o.transform.parent = measureUnit.transform;
        //    measureMentState = MeasurementState.Connect;
        //    selectedLine = Instantiate(line);
        //    selectedLine.transform.parent = measureUnit.transform;
        //    selectedLine.gameObject.SetActive(true);
        //    selectedLine.GetComponent<MeasurementLine>().SetStartDot(selectedMeasurementDot.transform);
        //}else if (measureMentState == MeasurementState.Connect)
        //{
            
        //    o.transform.parent = measureUnit.transform;
        //    selectedLine.GetComponent<MeasurementLine>().SetCollider();
        //    //selectedLine.GetComponent<MeasurementLine>().SetStartDot(selectedMeasurementDot.transform);
        //    measureMentState = MeasurementState.Start;
        //    selectedLine = null;
        //}                                                                                                                      
    }

    public void SelectMeasurementDone()
    {
        startMeasureMent = true;
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

    public void ActivateAdd(bool onOff)
    {
        addUI.SetActive(onOff);
    }

    public void ActivateRemove(bool onOff)
    {
        removeUI.SetActive(onOff);
    }

    public void Add()
    {

    }

    public void Remove()
    {
        if(selectedMeasureUnit != null)
        {
            Destroy(selectedMeasureUnit);
            selectedMeasureUnit = null;
        }
    }

    public void Select(GameObject o)
    {
        Transform group = null;

        int groupChildCount;

        if (o.transform.parent.name == "MeasureUnit") //Dot
        {
            InitSelect();

            selectedMeasurementDot = o.GetComponent<MeasurementDot>();
            selectedMeasurementDot.Select(true);

            return;
        }
        else if(o.transform.parent.parent.name == "MeasureUnit") //Line
        {
            group = o.transform.parent.parent;
        }

        if (group == null)
        {
            print("MeasureUnit Not Exist");
            return;
        }



        InitSelect();

        if (selectedMeasureUnit == group)
        {
            selectedMeasureUnit = null;
            return;
        }

        selectedMeasureUnit = group;

        groupChildCount = selectedMeasureUnit.childCount;

        for(int i = 0; i < groupChildCount; ++i)
        {
            group.GetChild(i).GetComponent<MeasurementObject>().Select(true);
        }

    }


    private void InitSelect()
    {

        print("InitSelect");
        if(selectedMeasurementDot != null)
        {
            print("SelectedMeasurementDot null!");
            selectedMeasurementDot.Select(false);
        }

        if(selectedMeasureUnit != null)
        {
            int gropuChildCount = selectedMeasureUnit.childCount;

            for(int i = 0; i < gropuChildCount; ++i)
            {
                selectedMeasureUnit.GetChild(i).GetComponent<MeasurementObject>().Select(false);
            }
        }

  
    }

    //Drag Fix Dot

    public void StartDrag()
    {
        if (selectedMeasurementDot == null)
            return;

        float distance = Vector3.Distance(cursor.GetCursorPoint(), selectedMeasurementDot.transform.position);

        print("Measurement Drag:" + distance);

        if (distance < 0.5f)
        {
            dragDot = selectedMeasurementDot.transform;
            selectedLine = dragDot.parent.GetChild(1).GetComponent<LineRenderer>();

            ondrag = true;
        }
    }

    public void Drag()
    {
        //if (selectedMeasurementDot==null || selectedMeasurementDot.transform != dragDot)
        //    return;

        //float distance = Vector3.Distance(cursor.GetCursorPoint(), selectedMeasurementDot.transform.position);

        //print("Measurement Drag:" + distance);

        //if(distance < 0.5f)
        //{
        //    dragDot.position = cursor.GetCursorPoint();
        //    measureMentState = MeasurementState.Connect;

         
        //}
    }

    public void DragUp()
    {
        SetActiveMeasureUI(false);

        if (measureMentState == MeasurementState.DotCreating)
        {
            measureMentState = MeasurementState.DotCreateEnd;
            


            if(preMeasureUnit == null)
            {
                preMeasureUnit = Instantiate(measureUnit).GetComponent<MeasurementUnit>();
                preMeasureUnit.AddDot(cursor.cursor.position, cursor.cursor.eulerAngles);
                preMeasureUnit.Select(true);
            }
        }
        else if(measureMentState == MeasurementState.LineCreating)
        {
            measureMentState = MeasurementState.DotCreateEnd;
            preMeasureUnit.GetPrevLine().SetBoolDottedLineOnOff(false);
            preMeasureUnit.AddDot(cursor.cursor.position, cursor.cursor.eulerAngles);
         
          
            //print("LineCreating Done");
            //print();
        }

        //if (ondrag)
        //{
        //    measureMentState = MeasurementState.Start;
        //    selectedLine = null;

        //}

        //ondrag = false;
    }

  
    public void DotCreateMode()
    {
        SetActiveMeasureUI(true);

        if(measureMentState == MeasurementState.None)
        {
            measureMentState = MeasurementState.DotCreating;
            SetActiveMeasureUI(true);
        }
        else if(measureMentState == MeasurementState.DotCreateEnd)
        {

            measureMentState = MeasurementState.LineCreating;
            preMeasureUnit.GetPrevLine().SetBoolDottedLineOnOff(true);
            //if (preMeasureUnit != null)
            //{
            //    preMeasureUnit.AddDot(cursor.cursor.position, cursor.cursor.eulerAngles);
            //}
        }
    }

    private void SetActiveMeasureUI(bool onOff)
    {
        if (onOff)
        {
            measureRenderUI.gameObject.SetActive(true);
            measureCamera.gameObject.SetActive(true);
            cursor.SetMeasureCursorMode();
        }
        else
        {
            measureRenderUI.gameObject.SetActive(false);
            measureCamera.gameObject.SetActive(false);
            cursor.SetNormalCursorMode();
        }
      
    }

}
