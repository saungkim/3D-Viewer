using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class MeasurementLine : MeasurementObject
{
    [SerializeField] GameObject textObj;
    [SerializeField] TextMeshPro text;
    private Transform startDot;
    private Transform endDot;

    [SerializeField] private BoxCollider boxCollider;
    [SerializeField] private bool dottedLineOnOff;

    [SerializeField] private DottedLine dottedLine;
    [SerializeField] private LineRenderer lineRenderer;

    [SerializeField] private Vector3 startPos;
    [SerializeField] private Vector3 endPos;

    private int connectedStartIndex;
    private int connectedEndIndex;

    // Start is called before the first frame update
    void Start()
    {
        //Select(transform.parent.GetComponent<MeasurementUnit>().GetSelect());

        //   lineRenderer = GetComponent<LineRenderer>();
    }
    
    // Update is called once per frame
    void Update()
    {
        if (lineRenderer == null)
            return;

        if (dottedLineOnOff != dottedLine.gameObject.activeSelf || dottedLineOnOff != !lineRenderer.gameObject.activeSelf)
        {
            print("If Go : " + dottedLineOnOff);
            if (dottedLineOnOff)
            {
                dottedLine.gameObject.SetActive(true);
                lineRenderer.gameObject.SetActive(false);
            }
            else
            {
                print("LineRendereTrue");
                dottedLine.gameObject.SetActive(false);
                lineRenderer.gameObject.SetActive(true);
            }
        }

        if (dottedLineOnOff) 
        {
            dottedLine.SetPositions(startPos, endPos);
        }
        else
        {
            lineRenderer.SetPosition(0, startPos);
            lineRenderer.SetPosition(1, endPos);
        }

        if(startPos == endPos)
        {
            textObj.SetActive(false);

            return;
        }

        if (!textObj.activeSelf)
        {
            textObj.SetActive(true);
        }

        float distance = Vector3.Distance(startPos, endPos);

        Vector3 centerPos = (startPos + endPos) /2;

        textObj.transform.position = centerPos;

        textObj.transform.LookAt(Camera.main.transform.position);

        text.text = Math.Round(distance, 2).ToString() +"m";

    }

    public void SetStartDot(Transform tf)
    {
        startDot = tf;
    }

    public void SetEndDot(Transform tf)
    {
        endDot = tf;
    }

    public override void SetCollider()
    {
        //Collider
        boxCollider.size = new Vector3(0.03f, 0.03f, 0.9f * Vector3.Distance(lineRenderer.GetPosition(0), lineRenderer.GetPosition(1)));
        boxCollider.transform.position = (lineRenderer.GetPosition(0) + lineRenderer.GetPosition(1)) / 2;
        boxCollider.transform.LookAt(lineRenderer.GetPosition(1));
    }

    public override void Select(bool onOff)
    {
        if (onOff)
        {
            lineRenderer.material.color = Color.yellow;
        }
        else
        {
            lineRenderer.material.color = Color.white;
        }
    }

    public void SetLinePositions(Vector3 inputStartPos , Vector3 inputEndPos )
    {
        startPos = inputStartPos;
        endPos = inputEndPos;
    }

    public void SetLineStartPosition(Vector3 inputStartPos)
    {
        startPos= inputStartPos;   
    }

    public void SetLineEndPosition(Vector3 inputEndPos)
    {
        endPos = inputEndPos;   
    }

    public void SetBoolDottedLineOnOff(bool onOff)
    {
        dottedLineOnOff = onOff;
    }

}
