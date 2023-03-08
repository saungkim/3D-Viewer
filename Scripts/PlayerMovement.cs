
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private Transform construction;
    private Vector3[] movePoints;

    [SerializeField] private Cursor cursor;

    [SerializeField] private CameraController camController;

    private int stage = 0;

    struct poi {
        internal int index;
        internal Vector3 value;
    }

    // Start is called before the first frame update
    void Start()
    {
        GetMovePoints();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    poi FindNearPointFromMouse()
    {
        float min = 1000;

        Vector3 cursorPoint = cursor.GetCursorPoint();
        Vector3 value =  Vector3.zero;

        int index = -1;

        int i = 0;

        foreach(Vector3 pos in movePoints)
        {
            float dis =  Vector3.Distance(cursorPoint, pos);

            if(dis < min)
            {
                min = dis;
                index = i;
            }

            ++i;
        }

        poi p = new poi();
        p.index = index;
        p.value = movePoints[index];
        

        return p;
    }


    private void GetMovePoints()
    {

        movePoints = new Vector3[construction.childCount - 1];

        int childCount = construction.childCount - 1;

        for(int i = 0; i < childCount; ++i)
        {
            movePoints[i] = construction.GetChild(i).position;
        }
    }

    public IEnumerator MoveStage()
    {
       
        poi p = FindNearPointFromMouse();
   
        if (stage != p.index  )
        {
            construction.GetChild(p.index).gameObject.SetActive(true);
            AllChildOff(construction.GetChild(p.index).GetChild(0), false);

            yield return StartCoroutine(camController.MoveCamInstant(p.value , GetChildMaterials(construction.GetChild(stage).GetChild(0))));
           
            construction.GetChild(stage).gameObject.SetActive(false);
            //AllChildOff(construction.GetChild(p.index), true);

            stage = p.index;
        }

    }

    private void AllChildOff(Transform tf,bool onOff)
    {
        int childCount = tf.childCount;
        for(int i = 0; i < childCount; ++i)
        {
            tf.GetChild(i).gameObject.SetActive(onOff);
        }
    }

    private Material[] GetChildMaterials(Transform tf)
    {
        int childCount = tf.childCount;
        Material[] materials = new Material[childCount];

        for(int i = 0; i < childCount; ++i)
        {
            materials[i] = tf.GetChild(i).GetComponent<MeshRenderer>().material ;
        }

        return materials;
    }

    public void SetPositionAndLook(int panoramaPosition )
    {

    }

    public int GetStage()
    {
        return stage;
    }


}


