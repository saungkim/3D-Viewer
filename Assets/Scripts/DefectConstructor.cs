using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.Networking;
using static DefectConstructor;
using System.Reflection;

public class DefectConstructor : MonoBehaviour
{
    [Header("Containner")]
    [SerializeField] private Transform defectDot;
    [SerializeField] private GameObject dot;
    [SerializeField] private Constructor constructor;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private NativeMessanger nativeMessanger;
    
    List<Defect> defectList = new List<Defect>();
    Defect[] defectArray;

    [SerializeField] bool createDotRefresh = false;
     


    // Start is called before the first frame update
    void Awake()
    {
        initDefectDotLocalScale = dot.transform.localScale;  
    }

    // Update is called once per frame
    void Update()
    {

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Space))
        {
            WriteAllDefect(Application.dataPath + "/Sources/" + "input.json");
        }
#endif

    }


    public void Init()
    {
        defectArray = null;
        defectList.Clear();

        int defectDotChildCount = defectDot.childCount;

        for(int i = defectDotChildCount - 1; i >= 0 ; --i)
        {
            Destroy(defectDot.GetChild(i).gameObject);
        }
    }

    public void CreateDot(Vector3 pos, Vector3 rot, bool sendMessage) // TO DO SendMessage have to be devided
    {
        if (createDotRefresh)
            DestroyAllDefects();


        GameObject o = Instantiate(dot, pos, Quaternion.Euler(rot), defectDot);
        o.SetActive(true);

        Defect defect = new Defect();
        defect.id = o.transform.GetSiblingIndex().ToString();
        //defect.type = "defect";
        defect.position = pos;
        defect.rotation = rot;
        defect.status = "scheduled";
      
        //if(defect.color != null || defect.color != "") // TO DO Create Error Message 
        //{
        //    Color outColor;

        //    if (ColorUtility.TryParseHtmlString(defect.color, out outColor))
        //    {
        //        defectConstructor.SetDefectColor(id, outColor, nativeSendErrorMessage);
        //    }
        //    else
        //    {
        //        nativeSendErrorMessage("SetColorDefect Error :" + htmlColor + " " + "ParseHtmlString Failed");
        //    }
        //}

        defect.view = new View();
        defect.view.position = Camera.main.transform.position;
        defect.view.rotation = Camera.main.transform.eulerAngles;
        defect.view.fov = Camera.main.fieldOfView;

        defectList.Add(defect);

        if (sendMessage) // For Creating{
        {
            //foreach(Constructor.Boundary boundary in constructor.boundaries)
            //{
            //    if (constructor.IsPointInTrapezoid(new Vector2(pos.x,pos.z),boundary.polygon[0], boundary.polygon[1], boundary.polygon[2], boundary.polygon[3]))
            //    {
            //        defect.name = boundary.name;
            //    }
               
                        
            //            // boundary.polygon
            //}


            Constructor.RoomPositionInfo info = constructor.GetBoundary(new Vector2(pos.x,pos.z));

            defect.name = info.name;
            defect.flawPrtbGrpCd = info.flawPrtbGrpCd;
            defect.detailName = info.detailName;
            defect.flawPrtbCd = info.flawPrtbCd;
            
            //defect.name = bound;
            //nativeMessanger.NativeSendMessage("CreateDot," + JsonUtility.ToJson(defect));
        }       
    }

    public void CreateDot(Defect defect) // TO DO SendMessage have to be devided
    {
        print("Create Dot Defect ID" + defect.id);

        GameObject o = Instantiate(dot, defect.position, Quaternion.Euler(defect.rotation), defectDot);
        o.SetActive(true);

        if (defect.status != null && defect.status != "")
        {
            if(defect.status == "scheduled")
            {
                //o.transform.GetChild(0).GetComponent<Image>().color = Color.white;
            }
            else if(defect.status == "complete")
            {
                //o.transform.GetChild(0).GetComponent<Image>().color = Color.white;
            }
            else
            {
                
            }
        }
        //else if (defect.color != null && defect.color != "")
        //{
        //    Color outColor;

        //    if (ColorUtility.TryParseHtmlString(defect.color, out outColor))
        //    {
        //        o.transform.GetChild(0).GetComponent<Image>().color = outColor;
        //    }
        //    else
        //    {
        //        print("ParseHtmlColorString Failed :" + defect.color);
        //    }
        //}

        defectList.Add(defect);
    }


    public void WriteAllDefect(string filePath)
    {
        DefectArray defectArray = new DefectArray();
        defectArray.defect = defectList.ToArray();
        string jsonOutput = JsonUtility.ToJson(defectArray);

        File.WriteAllText(filePath, jsonOutput);
    }

    public void ReadAllDefectsWithFilePath(string filePath, Action<string> CallBack, bool init)
    {
        StartCoroutine(IEReadAllDefectsWithFilePath(filePath,CallBack,init));
    }

    public IEnumerator IEReadAllDefectsWithFilePath(string filePath, Action<string> CallBack , bool init)
    {
        float loadTime = 0;

        UnityWebRequest reader =  UnityWebRequest.Get(filePath);
        reader.SendWebRequest();

        while (!reader.isDone && loadTime <= 5)
        {
            loadTime += Time.deltaTime;
            yield return null;
        }

        if (loadTime > 5)
        {
            CallBack("The file does not exist.");
        }

        string json = reader.downloadHandler.text;

        DefectArray defectArrayFromJson = JsonUtility.FromJson<DefectArray>(json);

        defectArray = defectArrayFromJson.defect;

        foreach (Defect defect in defectArray)
        {
            CreateDot(defect.position, defect.rotation, false);
        }

        CallBack("Success");
    }

    public void ReadAllDefect(string json, Action<string> CallBack , bool init)
    {
        if (init)
        {
            DestroyAllDefects(CallBack);

            DefectArray defectArrayFromJson = JsonUtility.FromJson<DefectArray>(json);

            defectArray = defectArrayFromJson.defect;

            foreach (Defect defect in defectArray)
            {
                CreateDot(defect.position, defect.rotation, false);
            }
            CallBack("Success");
        }
        else
        {
            AddDefectsWithJson(json, CallBack);
        }
    }

    public void ReadDefects(String json , Action<string> callBack , bool init)
    {
        if (init)
        {
            Init();
        }

        AddDefectsWithJson(json, callBack);
    }

    public int GetDefectIndexFromDistance(string value)
    {
        Defect defectInput = JsonUtility.FromJson<Defect>(value); 

        int index = 0;
        int pick = -1;

        float minDistancne = 10000;

        foreach(Defect defect in defectList)
        {
            float distance = Vector3.Distance(defect.position, defectInput.position);

            if(distance < minDistancne)
            {
                minDistancne = distance;
                pick = index;
            }

            ++index;
        }

        return pick;
    }
      
    public void AddDefectsWithJson(string json, Action<string> CallBack)
    {
        DefectArray defectArrayFromJson = JsonUtility.FromJson<DefectArray>(json);

        defectArray = defectList.ToArray();

        foreach (Defect defect in defectArrayFromJson.defect)
        {
            //CreateDot(defect.position, defect.rotation, defect.color , defect);
            CreateDot(defect);
        }

        CallBack("Success");
    }

    public void AddDefectJson(string json)
    {
     
        Defect defect = JsonUtility.FromJson <Defect>(json);  
     
        if(defect.id == null)
        {
            print("defect Id is null");
        }
        
        CreateDot(defect);
    }

    public IEnumerator MoveCamInstant(string defectId)
    {
        yield return new WaitUntil(() => constructor.GetIsLoadDone());

        if (defectArray != null)
        {
            foreach (Defect defect in defectArray)
            {
                if (defect.id == defectId)
                {
                    playerMovement.MoveStageInstant(defect.view.position, defect.view.rotation, defect.view.fov);
                }
            }
        }
    }

    public IEnumerator MoveCamInstantIndex (int index)
    {
        yield return new WaitUntil(() => constructor.GetIsLoadDone());

        playerMovement.MoveStageInstant(defectArray[index].view.position, defectArray[index].view.rotation, defectArray[index].view.fov);
    }

    public IEnumerator VIewDefectJson(string value,Action<string> callBack)
    {
        yield return new WaitUntil(() => constructor.GetIsLoadDone());
        
        Defect defect = JsonUtility.FromJson<Defect>(value);

        CreateDot(defect);

        playerMovement.MoveStageInstant(defect.view.position, defect.view.rotation, defect.view.fov);

        callBack("LoadComplete");
    }

    public IEnumerator ViewDefectJsonArray(string value)
    {
        yield return new WaitUntil(() => constructor.GetIsLoadDone());

        DefectArray defects = JsonUtility.FromJson<DefectArray>(value);

        //CreateDot(defect);

        foreach (Defect defect in defects.defect)
        {
           
                playerMovement.MoveStageInstant(defect.view.position, defect.view.rotation, defect.view.fov);
                CreateDot(defect);
           
        }
    }

    public void SendMessageSelectDefect(int id)
    {
        nativeMessanger.OnViewerClicked(JsonUtility.ToJson(defectList[id - 1]));
    }
    public void SetColor(int id, Color color)
    {
    }

    public void DestroyDefect(string id, Action<string> callback)
    {
        bool destroied = false;

        int defectListCount = defectList.Count;

        if (defectListCount == 0)
        {
            callback("Destroy Defect Error :" + "There are no Defects.");
        }

        for (int i = 0; i < defectListCount; ++i)
        {
            if (defectList[i].id == id)
            {
                defectList.RemoveAt(i);
                Destroy(defectDot.GetChild(i + 1).gameObject);

                destroied = true;

                break;
            }
        }

        if (!destroied)
        {
            // callback("Destroy Defect Error :" + id + " " + "Id doesn't exist.");

            print("Destroy Defect Error :" + id + " " + "Id doesn't exist.");
        }

    }

    public void DestroyDefectJson(string json, Action<string> callback)
    {
        Defect[] defects = JsonToDefectArray(json).defect;

        foreach(Defect defect in defects)
        {
            int index = 0;

            foreach(Defect oriDefect in defectList)
            {
                if(defect.id == oriDefect.id)
                {
                    defectList.RemoveAt(index);
                    Destroy(defectDot.GetChild(index + 1).gameObject);
                }

                ++index;
            }
        }
    }

    public void DestroyDefects(string json ,Action<string> callback)
    {
        int defectListCount = defectList.Count;

        if (defectListCount == 0)
        {
            callback("Destroy Defect Error :" + "There are no Defects.");
        }
    }

    public void DestroyAllDefects(Action<string> callback)
    {
        int defectListCount = defectList.Count;

        if (defectListCount == 0)
        {
            callback("Destroy Defect Error :" + "There are no Defects.");
        }

        for (int i = 0; i < defectListCount; ++i)
        {
            defectList.RemoveAt(0);
            Destroy(defectDot.GetChild(i + 1).gameObject);
        }

        callback("Success");
    }

    public void DestroyAllDefects()
    {
        int defectListCount = defectList.Count;

        for (int i = 0; i < defectListCount; ++i)
        {
            defectList.RemoveAt(0);
            Destroy(defectDot.GetChild(i + 1).gameObject);
        }
    }

    public void SetDefectsColor(string json)
    {
        Defect[] defects = JsonToDefectArray(json).defect;

        foreach(Defect defect in defects)
        {
            int index = 0;

            foreach(Defect defect2 in defectList)
            {
                if(defect.id == defect2.id)
                {
                    //defects[index].color = defect2.color;
                   
                    //Color outColor;

                    //if (ColorUtility.TryParseHtmlString(defects[index].color, out outColor))                    {
                    //    defectDot.GetChild(index + 1).GetChild(0).GetComponent<Image>().color = outColor;
                    //}
                    //else 
                    //{
                    //    print("SetDefectsColor Color Parse Failed :" + defects[index].color);
                    //}
                }
                ++index;
            }
        }
    }

    public void SetDefectColor(string id, Color color, Action<string> callback)
    {
        bool changed = false;

        int defectListCount = defectList.Count;

        if (defectListCount == 0)
        {
            callback("SetDefectColor Error :" + "There are no Defects.");
        }

        for (int i = 0; i < defectListCount; ++i)
        {
            if (defectList[i].id == id)
            {

                defectDot.GetChild(i + 1).GetChild(0).GetComponent<Image>().color = color;

                print(defectDot.GetChild(i).name);

                changed = true;

                break;
            }
        }

        if (!changed)
        {
            callback("SetDefectColor Error :" + id + " " + "Id doesn't exist.");
        }
    }

    public string GetAllDefects() {
        return JsonUtility.ToJson(defectList);
    }

    public string GetDefect(string defectID)
    {
        foreach(Defect defect in defectList)
        {
            if(defect.id == defectID)
            {
                return JsonUtility.ToJson(defect);
            }   
        }

        return null;
       
    }

    Vector3 initDefectDotLocalScale;
    public void SetDefectLocalScale(float localSale)
    {

        print("iniDefectDotLocalScale" + initDefectDotLocalScale + ":" + localSale);
        dot.transform.localScale = initDefectDotLocalScale * localSale;
    }

    private DefectArray JsonToDefectArray(string json)
    {
        return JsonUtility.FromJson<DefectArray>(json);
    }

    bool IsPointInTrapezoid(Vector2 point, Vector2 a, Vector2 b, Vector2 c, Vector2 d)
    {
        float triangleArea(Vector2 p1, Vector2 p2, Vector2 p3)
        {
            return Mathf.Abs((p1.x * (p2.y - p3.y) + p2.x * (p3.y - p1.y) + p3.x * (p1.y - p2.y)) / 2.0f);
        }

        float trapezoidArea = triangleArea(a, b, c) + triangleArea(c, d, a);
        float pointArea = triangleArea(point, a, b) + triangleArea(point, b, c) + triangleArea(point, c, d) + triangleArea(point, d, a);

        return Mathf.Approximately(trapezoidArea, pointArea);
    }

    public string GetDefectInfo(int index)
    {
        return JsonUtility.ToJson(defectList[index]).ToString();
    }

    [Serializable]
    public class Defect
    {
        public string id; //하자 ID
        public string status; //하자 상태 
        public string name; //하자가 위치한 방 이름
        public string flawPrtbGrpCd; //위치명
        public string detailName; //하자 디테일 방 이름
        public string flawPrtbCd; //상세위치명
        public Vector3 position; //하자 위치
        public Vector3 rotation; //하자 각도
        public View view; //카메라 관점
    }

  

    public void SetCreateDotRefresh(bool onOff)
    {
        createDotRefresh = onOff;
    }

    public void SetDefaultColor(Color color)
    {
        dot.transform.GetChild(0).GetComponent<Image>().color = color;
    }

    public void SetColliderSize(Vector3 size)
    {
        dot.transform.GetComponent<BoxCollider>().size = size;
    }

    [Serializable]
    public class DefectDefault{
        public string id;
        public Vector3 position;
        public Vector3 rotation;
        public View view;
    }

    [Serializable]
    public class DefectArray
    {
        public Defect[] defect; //하자 배열
    }

    [Serializable]
    public class View // 카메라 관점
    {
        public Vector3 position; //카메라 위치
        public Vector3 rotation; //카메라 각도
        public float fov; //카메라 fov
    }



    
}
