

using System.Collections;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private Transform construction;
    [SerializeField] private Vector3[] movePoints;

    [SerializeField] private ViewerCursor cursor;

    [SerializeField] private CameraController camController;

    private int stage = 0;

    [SerializeField] private NetworkManager networkManager;
    [SerializeField] private OverallSetting overallSetting;

    struct poi
    {
        internal int index;
        internal Vector3 value;
    }

    // Start is called before the first frame update
    public void Init()
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

    poi FindNearPointFrom(Vector3 input)
    {
        float min = 1000;

        Vector3 value = Vector3.zero;

        int index = -1;

        int i = 0;

        foreach (Vector3 pos in movePoints)
        {
            float dis = Vector3.Distance(input, pos);

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
        movePoints = new Vector3[construction.childCount - 1];

        int childCount = construction.childCount - 1;

        for (int i = 0; i < childCount; ++i)
        {
            movePoints[i] = construction.GetChild(i).position;
        }
    }
    public void InitStage(int panoramaID)
    {
        construction.GetChild(stage).gameObject.SetActive(false);
        construction.GetChild(panoramaID).gameObject.SetActive(true);
        transform.position = construction.GetChild(panoramaID).position;
        stage = panoramaID;
    }
    public IEnumerator MoveStage()
    {

        poi p = FindNearPointFromMouse();

        //print(construction.GetChild(p.index).position.y + ":" + p.value.y);

        if (stage != p.index && cursor.GetCursorPoint().y < p.value.y - 0.5f)
        {


            if (overallSetting.GetZoomInit())
            {
                Camera.main.fieldOfView = 60;
            }

            construction.GetChild(p.index).gameObject.SetActive(true);
            //AllChildOff(construction.GetChild(p.index).GetChild(0), false);

            yield return StartCoroutine(camController.MoveCam(p.value, GetChildMaterials(construction.GetChild(stage).GetChild(0))));
            construction.GetChild(stage).gameObject.SetActive(false);
            construction.GetChild(stage).GetComponent<LoadTextureFromStreamingAsset>().DestroyTex();

            //AllChildOff(construction.GetChild(p.index), true);

            stage = p.index;
        }

    }



    int countStageOnUI = 0;
    public void MoveStageOnUI()
    {
        StartCoroutine(MoveStage(networkManager.camPositions[countStageOnUI], networkManager.camRotations[countStageOnUI], networkManager.camFovs[countStageOnUI]));


        ++countStageOnUI;

        if (countStageOnUI == networkManager.camPositions.Length)
        {
            countStageOnUI = 0;
        }
    }

    public IEnumerator MoveStage(Vector3 pos, Vector3 rot, float fov)
    {
        poi p = FindNearPointFrom(pos);

        Camera.main.transform.eulerAngles = rot;
        Camera.main.fieldOfView = fov;

        if (stage != p.index)
        {


            construction.GetChild(p.index).gameObject.SetActive(true);
            AllChildOff(construction.GetChild(p.index).GetChild(0), false);

            yield return StartCoroutine(camController.MoveCam(p.value, GetChildMaterials(construction.GetChild(stage).GetChild(0))));

            construction.GetChild(stage).gameObject.SetActive(false);

            stage = p.index;
        }
    }

    public void MoveStageInstant(Vector3 pos, Vector3 rot, float fov)
    {
        poi p = FindNearPointFrom(pos);
        construction.GetChild(p.index).gameObject.SetActive(true);
        stage = p.index;
        camController.MoveCamInstant(pos, rot, fov);
    }

    private void AllChildOff(Transform tf, bool onOff)
    {
        int childCount = tf.childCount;
        for (int i = 0; i < childCount; ++i)
        {
            tf.GetChild(i).gameObject.SetActive(onOff);
        }
    }

    private Material[] GetChildMaterials(Transform tf)
    {
        int childCount = tf.childCount;
        Material[] materials = new Material[childCount];

        for (int i = 0; i < childCount; ++i)
        {
            materials[i] = tf.GetChild(i).GetComponent<MeshRenderer>().material;
        }

        return materials;
    }

    public void SetPositionAndLook(int panoramaPosition)
    {

    }

    public int GetStage()
    {
        return stage;
    }

  

}


