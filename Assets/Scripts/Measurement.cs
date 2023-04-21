using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
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
  //  private MeasurementDot selectedMeasurementDot;
    private LineRenderer selectedLine;
   
    private TextMeshProUGUI selectedText;
    [SerializeField] private MeasurementDot measurementDot;

    bool startMeasureMent = false;
    private bool ondrag = false;

    public enum MeasurementState {None,DotCreating,DotCreateEnd,LineCreating,DotFixing}
    public MeasurementState measureMentState = MeasurementState.None;

    [SerializeField] private GameObject addUI;
    [SerializeField] private GameObject removeUI;

    private Transform dragDot;

    [SerializeField] private MeasurementUnit measurementUnit;
    private MeasurementUnit preMeasureUnit;
    private int selectedDotIndex;

    // Start is called before the first frame updates
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
       

        if (measureMentState == MeasurementState.DotCreating)
        {
            MeasureUI();

            //print("DotCreating:"+pos);
        }
        else if(measureMentState == MeasurementState.LineCreating)
        {
            MeasureUI();
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

    private void MeasureUI()
    {
        measureCamera.LookAt(cursorTransform);
        Vector3 pos = Camera.main.WorldToScreenPoint(cursorTransform.position) + measurePlusPos;
        measureRenderUI.position = pos;
    }

    public void InverseActivateMeasurement()
    {
        print("InverseActivateMeasurement");

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

        print("ActivateMeasurement" + onOff);

        if (onOff)
        {

        }
        else
        {


            if(preMeasureUnit != null)
            {
                
                CompleteSelectedMeasureUnit();
            }
        }
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
    
    }

    public void SelectDot(Transform dotObj)
    {


        Select(dotObj.parent.parent.gameObject, false);
        dotObj.parent.GetComponent<MeasurementDot>().Select(true);
        preMeasureUnit = dotObj.parent.parent.GetComponent<MeasurementUnit>();
        selectedDotIndex = preMeasureUnit.FindDotIndex(dotObj.parent.gameObject);
        //selectedDotIndex = dotObj
        print("SelectedDOtIndex :" + selectedDotIndex);
    }
    

    public void Select(GameObject o , bool onOff)
    {
        print("Select Called");

        if(measureMentState != MeasurementState.None && measureMentState != MeasurementState.DotFixing)
        {
            print("Select Returned");
            return;
        }

        Transform group = o.transform;

        int groupChildCount;

        if (preMeasureUnit != null)
        {
            groupChildCount = preMeasureUnit.transform.childCount;

            for (int i = 0; i < groupChildCount; ++i)
            {
                preMeasureUnit.transform.GetChild(i).GetComponent<MeasurementObject>().Select(false);
            }

            if (preMeasureUnit.transform == group)
            {
                preMeasureUnit = null;
                return;
            }

        }
        
        groupChildCount = group.childCount;

        preMeasureUnit = group.GetComponent<MeasurementUnit>();
        
        for(int i = 0; i < groupChildCount; ++i)
        {
            group.GetChild(i).GetComponent<MeasurementObject>().Select(onOff);
        }

    }


    private void InitSelect()
    {

        //print("InitSelect");
        //if(selectedMeasurementDot != null)
        //{
        //    print("SelectedMeasurementDot null!");
        //    selectedMeasurementDot.Select(false);
        //}

        //if(preMeasureUnit != null)
        //{
        //    int gropuChildCount = preMeasureUnit.transform.childCount;

        //    for(int i = 0; i < gropuChildCount; ++i)
        //    {
        //        preMeasureUnit.transform.GetChild(i).GetComponent<MeasurementObject>().Select(false);
        //    }
        //}

  
    }

    //Drag Fix Dot

    public void StartDrag()
    {
        if (measureMentState != MeasurementState.None)
            return;
            

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.transform.tag == "MeasurementDot")
            {
                inputSystem.SetControlState(InputSystem.ControlState.MeasureDot);
                measureMentState = MeasurementState.DotFixing;

                SelectDot(hit.transform);
       
                //return true;
            }
            else if (hit.transform.tag == "MeasurementLine")
            {
                Select(hit.transform.parent.parent.gameObject , true);
            }
        }

        //if (selectedMeasurementDot == null)
        //    return;

        //float distance = Vector3.Distance(cursor.GetCursorPoint(), selectedMeasurementDot.transform.position);

        //print("Measurement Drag:" + distance);

        //if (distance < 0.5f)
        //{
        //    dragDot = selectedMeasurementDot.transform;
        //    selectedLine = dragDot.parent.GetChild(1).GetComponent<LineRenderer>();

        //    ondrag = true;
        //}
    }

    public void Drag()
    {
        if(measureMentState == MeasurementState.DotFixing)
        {
            preMeasureUnit.FixDot(selectedDotIndex, cursorTransform.position , cursorTransform.eulerAngles);
        }

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

    public void DragStart()
    {

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
        }
        else if(measureMentState == MeasurementState.None)
        {
      
            if (preMeasureUnit == null)
                return;

            preMeasureUnit.Select(false);
            preMeasureUnit = null;
        }
        else if(measureMentState == MeasurementState.DotFixing)
        {
            preMeasureUnit.SetCollider();

            preMeasureUnit.Select(false);
            preMeasureUnit = null;

            inputSystem.SetControlState(InputSystem.ControlState.Measure);

            measureMentState = MeasurementState.None;

           
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
            if(preMeasureUnit != null)
                preMeasureUnit.Select(false);

            preMeasureUnit = null;

            measureMentState = MeasurementState.DotCreating;
            SetActiveMeasureUI(true);
        }
        else if(measureMentState == MeasurementState.DotCreateEnd)
        {
            measureMentState = MeasurementState.LineCreating;
            preMeasureUnit.GetPrevLine().SetBoolDottedLineOnOff(true);      
        }
    }

    private void SetActiveMeasureUI(bool onOff)
    {
        if (onOff)
        {
            measureRenderUI.gameObject.SetActive(true);
            measureCamera.gameObject.SetActive(true);
            cursor.SetMeasureCursorMode();
            cursor.SetGaugeVisible(false);
            print("GaugeVisible False");
        }
        else
        {
            measureRenderUI.gameObject.SetActive(false);
            measureCamera.gameObject.SetActive(false);
            cursor.SetNormalCursorMode();
            cursor.SetGaugeVisible(true);
            print("GaugeVisible True");
        }
    }

    public void DestroySelectedMeasureUnit()
    {
        if (preMeasureUnit == null)
            return;

        Destroy(preMeasureUnit.gameObject);

        preMeasureUnit = null;

        measureMentState = MeasurementState.None;
    }

    public void CompleteSelectedMeasureUnit()
    {
        if(measureMentState == MeasurementState.DotCreateEnd)
        {
            measureMentState = MeasurementState.None;

            if (preMeasureUnit == null)
            {
                return;
            }

            preMeasureUnit.CompleteUnit();
            preMeasureUnit.Select(false);
            preMeasureUnit = null;
        }
        else if(measureMentState == MeasurementState.None)
        {
            measureMentState = MeasurementState.None;

            if (preMeasureUnit == null)
            {
                return;
            }

            Destroy(preMeasureUnit);
            SetActiveMeasureUI(false);
        }
        else if (measureMentState == MeasurementState.LineCreating)
        {
            measureMentState = MeasurementState.None;

            if (preMeasureUnit == null)
            {
                return;
            }

            preMeasureUnit.CompleteUnit();
            preMeasureUnit.Select(false);
            preMeasureUnit = null;
        }
    }

    //public bool CheckMeasurementDotCollider()
    //{

    //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

    //    if (Physics.Raycast(ray, out RaycastHit hit))
    //    {
    //        if (hit.transform.tag == "MeasurementDot")
    //        {
               
    //        }
    //        else if (hit.transform.tag == "Measurement")
    //        {

    //        }
    //    }


    //}

}
