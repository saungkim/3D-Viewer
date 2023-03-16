using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavvisCamera : MonoBehaviour
{
    [SerializeField] private Transform inputModel;
  
    int inputModelChildCount;

    private Vector3[] movePoints;

    [SerializeField] private Cursor cursor;

    [SerializeField] private CameraController camController;

    private int stage = 0;

    struct poi
    {
        internal int index;
        internal Vector3 value;
    }

    // Start is called before the first frame update
    void Start()
    {
        inputModel.GetChild(0).gameObject.SetActive(true);

        transform.position = inputModel.GetChild(0).position;
        inputModelChildCount = inputModel.childCount;

        GetMovePoints();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Next()
    {
        if (inputModelChildCount < stage + 2)
            return;

        inputModel.GetChild(stage).gameObject.SetActive(false);

         ++stage;
         transform.position = inputModel.GetChild(stage).position;

        inputModel.GetChild(stage).gameObject.SetActive(true);
    }

    public void Prev()
    {
        if (1 > stage)
            return;

        inputModel.GetChild(stage).gameObject.SetActive(false);

        --stage;
        transform.position = inputModel.GetChild(stage).position;

        inputModel.GetChild(stage).gameObject.SetActive(true);
    }

    poi FindNearPointFromMouse()
    {
        float min = 1000;

        Vector3 cursorPoint = cursor.GetCursorPoint();
        Vector3 value = Vector3.zero;

        int index = -1;

        int i = 0;

        foreach (Vector3 pos in movePoints)
        {
            float dis = Vector3.Distance(cursorPoint, pos);

            if (dis < min)
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
        movePoints = new Vector3[inputModelChildCount];

        int childCount = inputModelChildCount;

        for (int i = 0; i < childCount; ++i)
        {
            movePoints[i] = inputModel.GetChild(i).position;
        }
    }

    public void MoveStage() 
    {
        poi p = FindNearPointFromMouse();

        //camController.MoveCamInstant(p.value);
 
        inputModel.GetChild(stage).gameObject.SetActive(false);
        inputModel.GetChild(p.index).gameObject.SetActive(true);

        stage = p.index;
    }
}
