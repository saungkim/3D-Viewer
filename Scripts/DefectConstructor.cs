using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefectConstructor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {

        }
    }

    private void WriteExample()
    {
        DefectArray defectArray = new DefectArray();
        defectArray.defect = new Defect[10];
    }

    [Serializable]
    public class Defect
    {
        public string id;
        public string type;
        public Vector3 positionn;
        public Vector3 rotation;
        View view;
    }

    [Serializable]
    public class DefectArray
    {
        public Defect[] defect;

    }

    [Serializable]
    public class View
    {
        public Vector3 position;
        public Vector3 rotation;
        public float fov;
    }
}
