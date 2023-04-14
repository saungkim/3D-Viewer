

using System.Collections;
using System.Net;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private Transform construction;
    [SerializeField] private Vector3[] movePoints;

    [SerializeField] private ViewerCursor cursor;

    [SerializeField] private CameraController camController;
    

    private int stage = -1;

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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(AutoTour(0,19));
        }
    }

    poi FindNearPointFromMouse()
    {
        float min = 1000;

        Vector3 cursorPoint = cursor.GetCursorPoint();
        Vector3 value = Vector3.zero;

        int index = -1;

        int i = -1;

        foreach (Vector3 pos in movePoints)
        {
            ++i;

            float dis = Vector3.Distance(cursorPoint, pos);

            Vector3 direction = ( pos - cursorPoint).normalized ;

            Ray ray = new Ray(cursorPoint ,direction * dis );
        
            if (Physics.Raycast(ray, out RaycastHit hit , dis))
            {
                continue;
            }

            if (dis < min)
            {
                min = dis;
                index = i;         
            } 
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

    poi FindNextNearPointFrom(Vector3 input)
    {
        float min = 1000;

        Vector3 value = Vector3.zero;

        int index = -1;
        int index2 = -1;

        int i = 0;

        foreach (Vector3 pos in movePoints)
        {
            float dis = Vector3.Distance(input, pos);

            if (dis < min)
            {
                min = dis;
                index2 = index;
                index = i;
            }

            ++i;
        }

        poi p = new poi();
        p.index = index2;
        p.value = movePoints[index2];

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
        if (stage == -1)
            stage = 0;
        construction.GetChild(stage).gameObject.SetActive(false);
        construction.GetChild(panoramaID).gameObject.SetActive(true);
        transform.position = construction.GetChild(panoramaID).position;
        stage = panoramaID;
    }
    public IEnumerator MoveStage()
    {
        poi p = FindNearPointFromMouse();

        if (stage != p.index && cursor.GetCursorPoint().y < p.value.y - 0.5f)
        {
            if (overallSetting.GetZoomInit())
            {
                Camera.main.fieldOfView = 60;
            }

            construction.GetChild(p.index).gameObject.SetActive(true);

            yield return StartCoroutine(camController.MoveCam(p.value, GetChildMaterials(construction.GetChild(stage).GetChild(0)) , GetChildMaterials(construction.GetChild(p.index).GetChild(0))));
            construction.GetChild(stage).gameObject.SetActive(false);
            construction.GetChild(stage).GetComponent<LoadTextureFromStreamingAsset>().DestroyTex();

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

            yield return StartCoroutine(camController.MoveCam(p.value, GetChildMaterials(construction.GetChild(stage).GetChild(0)), GetChildMaterials(construction.GetChild(p.index).GetChild(0))));

            construction.GetChild(stage).gameObject.SetActive(false);
            construction.GetChild(stage).GetComponent<LoadTextureFromStreamingAsset>().DestroyTex();

            stage = p.index;
        }
    }

    public IEnumerator MoveStage(int index, float fov)
    {
        Camera.main.fieldOfView = fov;

        if (stage != index)
        {
            construction.GetChild(index).gameObject.SetActive(true);
            AllChildOff(construction.GetChild(index).GetChild(0), true);
         
            yield return StartCoroutine(camController.MoveCam(movePoints[index], GetChildMaterials(construction.GetChild(stage).GetChild(0)), GetChildMaterials(construction.GetChild(index).GetChild(0))));

            construction.GetChild(stage).GetComponent<LoadTextureFromStreamingAsset>().DestroyTex();
            construction.GetChild(stage).gameObject.SetActive(false);
            stage = index;
        }
    }

    public void MoveStageInstant(Vector3 pos, Vector3 rot, float fov)
    {
        if(stage != -1)
        {
            construction.GetChild(stage).gameObject.SetActive(false);
        }

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

    public IEnumerator AutoTour(int startIndex , int endIndex)
    {
        MoveStageInstant(movePoints[startIndex],transform.eulerAngles,60);
        int[] list = new int[8] { 0, 15, 2, 19, 25, 4, 29, 1 };
        //
        int tourIndex = 0;
        int nexttourIndex = 0;
        int routeIndex = list.Length - 1;
        bool turn = true;
        while (true)
        {
            if (turn)
            {
                ++tourIndex;
            }
            else
            {
                --tourIndex;
            }
        
            if(tourIndex == routeIndex || tourIndex == 0)
            {
                turn = !turn;
            }

            if (turn)
            {
                nexttourIndex = tourIndex + 1;
            }
            else
            {
                nexttourIndex = tourIndex - 1;
            }

            StartCoroutine(camController.LinearLookAt(movePoints[list[nexttourIndex]],2f));
         

            yield return new WaitForSeconds(2.5f);

            StartCoroutine(MoveStage(list[tourIndex], 60));
        }
    }
}


