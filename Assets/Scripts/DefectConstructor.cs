using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.Networking;
using static DefectConstructor;
using Newtonsoft.Json;

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



    // Start is called before the first frame update
    void Start()
    {

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

    public void CreateDot(Vector3 pos, Vector3 rot,bool sendMessage) // TO DO SendMessage have to be devided
    {
        GameObject o = Instantiate(dot, pos, Quaternion.Euler(rot), defectDot);
        o.SetActive(true);

        Defect defect = new Defect();
        defect.id =  o.transform.GetSiblingIndex().ToString();
        defect.type = "defect";
        defect.position = pos;
        defect.rotation = rot;
        

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

        if (sendMessage) // For Creating
            nativeMessanger.NativeSendMessage("CreateDot," + JsonUtility.ToJson(defect));
    }

    public void CreateDot(Defect defect) // TO DO SendMessage have to be devided
    {
        GameObject o = Instantiate(dot, defect.position, Quaternion.Euler(defect.rotation), defectDot);
        o.SetActive(true);

        if (defect.status != null && defect.status != "")
        {
            if(defect.status == "")
            {

            }
            else if(defect.status == "")
            {

            }
            else if(defect.status == "")
            {

            }
        }
        else if (defect.color != null && defect.color != "")
        {
            Color outColor;

            if (ColorUtility.TryParseHtmlString(defect.color, out outColor))
            {
                o.transform.GetChild(0).GetComponent<Image>().color = outColor;
            }
            else
            {
                print("ParseHtmlColorString Failed :" + defect.color);
            }
        }

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
      
    public void AddDefectsWithJson(string json, Action<string> CallBack)
    {
        DefectArray defectArrayFromJson = JsonUtility.FromJson<DefectArray>(json);

        defectArray = defectList.ToArray();

        foreach (Defect defect in defectArrayFromJson.defect)
        {
           // CreateDot(defect.position, defect.rotation, defect.color , defect);
        }

        CallBack("Success");
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

    public void SendMessageSelectDefect(int id)
    {
        nativeMessanger.NativeSendMessage("SelectDefect." + JsonUtility.ToJson(defectArray[id]));
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

    public void SetDefectsColor(string json , Action<string> callback)
    {
        Defect[] defects = JsonToDefectArray(json).defect;

        foreach(Defect defect in defects)
        {
            int index = 0;

            foreach(Defect defect2 in defectList)
            {
                if(defect.id == defect2.id)
                {
                    defects[index].color = defect2.color;
                   

                    Color outColor;

                    if (ColorUtility.TryParseHtmlString(defects[index].color, out outColor))                    {
                        defectDot.GetChild(index + 1).GetChild(0).GetComponent<Image>().color = outColor;
                    }
                    else 
                    {
                        print("SetDefectsColor Color Parse Failed :" + defects[index].color);
                    }
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

    private DefectArray JsonToDefectArray(string json)
    {
        return JsonUtility.FromJson<DefectArray>(json);
    }



    [Serializable]
    public class Defect
    {
        public string id;
        public string type;
        public string color;
        public string status;
        public Vector3 position;
        public Vector3 rotation;
        public View view;
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
