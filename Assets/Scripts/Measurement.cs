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
    [SerializeField] private MeasurementDot measurementDot;
    [SerializeField] private GameObject addUI;
    [SerializeField] private GameObject removeUI;
    [SerializeField] private MeasurementUnit measurementUnit;
    [SerializeField] private Transform measurementUnitGroup;
    public enum MeasurementState {None,DotCreating,DotCreateEnd,LineCreating,DotFixing}
    public MeasurementState measureMentState = MeasurementState.None;

    private Vector3 measurePlusPos = new Vector3(0, 330, 0);
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
        }
        else if(measureMentState == MeasurementState.LineCreating)
        {
            MeasureUI();
            //Vector3 pos = Camera.main.WorldToScreenPoint(cursorTransform.position) + measurePlusPos;
            preMeasureUnit.GetPrevLine().SetLineEndPosition(cursorTransform.position);
        }

        if(preMeasureUnit != null)
        {
            removeUI.SetActive(true);
        }
        else
        {
            removeUI.SetActive(false);
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

    public void Init()
    {
        measureMentState = MeasurementState.None;

        int measurementUnitGroupChildCount = measurement.childCount;

        for(int i = measurementUnitGroupChildCount - 1; i >= 0; --i)
        {
            Destroy(measurement.GetChild(i).gameObject);
        }
    }

    private void MeasureUI()
    {
        measureCamera.LookAt(cursorTransform);
        Vector3 pos = Camera.main.WorldToScreenPoint(cursorTransform.position) + measurePlusPos;
      
        if (pos.x < 200)
        {
            pos = Camera.main.WorldToScreenPoint(cursorTransform.position) + new Vector3 (measurePlusPos.y,0,0);
        }
        else if (pos.x > Screen.width - 200)
        {
            pos = Camera.main.WorldToScreenPoint(cursorTransform.position) + new Vector3(- measurePlusPos.y, 0, 0);
        }
        
        if (pos.y > Screen.height - 200)
        {
           
           Vector3 heightPos = Camera.main.WorldToScreenPoint(cursorTransform.position) + new Vector3(0, - measurePlusPos.y, 0);
            pos.y = heightPos.y; 
        }
        else if (pos.y < 200)
        {
            Vector3 heightPos = Camera.main.WorldToScreenPoint(cursorTransform.position) + new Vector3(0, + measurePlusPos.y, 0);
            pos.y = heightPos.y;
        }

        measureRenderUI.position = pos;
    }

    public void ActivateMeasurement(bool onOff)
    {
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

    public void SelectDot(Transform dotObj)
    {
        Select(dotObj.parent.parent.gameObject, false);
        dotObj.parent.GetComponent<MeasurementDot>().Select(true);
        preMeasureUnit = dotObj.parent.parent.GetComponent<MeasurementUnit>();
        selectedDotIndex = preMeasureUnit.FindDotIndex(dotObj.parent.gameObject);  
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
                preMeasureUnit.transform.SetParent(measurementUnitGroup);
                preMeasureUnit.AddDot(cursor.cursor.position, cursor.cursor.eulerAngles );
            }

            removeUI.SetActive(true);
        }
        else if(measureMentState == MeasurementState.LineCreating)
        {
            measureMentState = MeasurementState.DotCreateEnd;
            preMeasureUnit.Select(true);
            preMeasureUnit.GetPrevLine().SetBoolDottedLineOnOff(false);
            preMeasureUnit.AddDot(cursor.cursor.position, cursor.cursor.eulerAngles);

            addUI.SetActive(true);
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
        }
        else
        {
            measureRenderUI.gameObject.SetActive(false);
            measureCamera.gameObject.SetActive(false);
            cursor.SetNormalCursorMode();
            cursor.SetGaugeVisible(true);
        }
    }

    public void DestroySelectedMeasureUnit()
    {
        addUI.SetActive(false);
        removeUI.SetActive(false);

        if (preMeasureUnit == null)
            return;

        Destroy(preMeasureUnit.gameObject);

        preMeasureUnit = null;

        measureMentState = MeasurementState.None;
    }

    public void CompleteSelectedMeasureUnit()
    {
        addUI.SetActive(false);
        removeUI.SetActive(false);

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
}
