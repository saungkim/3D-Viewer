using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MeasurementLine : MonoBehaviour
{
    [SerializeField] GameObject m;
    LineRenderer lineRenderer;
    [SerializeField] TextMeshPro text;
    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(lineRenderer.GetPosition(0), lineRenderer.GetPosition(1));

        Vector3 centerPos = (lineRenderer.GetPosition(0) + lineRenderer.GetPosition(1))/2;

        m.transform.position = centerPos;

        m.transform.LookAt(Camera.main.transform.position);

        text.text = Math.Round(distance, 2).ToString() +"m";

  
    }
}
