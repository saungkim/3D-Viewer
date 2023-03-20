using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class DefectConstructor : MonoBehaviour
{
    [SerializeField] private Transform defectDot;
    [SerializeField] private GameObject dot;

    List<Defect> defectList = new List<Defect>();

    [SerializeField] private Constructor constructor;
    [SerializeField] private PlayerMovement playerMovement;
    Defect[] defectArray;

    [SerializeField] private NativeMessanger nativeMessanger;
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

    public void CreateDot(Vector3 pos,Vector3 rot,bool sendMessage)
    {
        GameObject o = Instantiate(dot, pos, Quaternion.Euler(rot), defectDot);
        o.SetActive(true);
        //defectList.Add();
        Defect defect = new Defect();
        defect.id = "NeedToChange" + o.transform.GetSiblingIndex();
        defect.type = "°øÁ¤";
        defect.position = pos;
        defect.rotation = rot;
        defect.view = new View();
        defect.view.position = Camera.main.transform.position;
        defect.view.rotation = Camera.main.transform.eulerAngles;
        defect.view.fov = Camera.main.fieldOfView;
        
        defectList.Add(defect);
        if(sendMessage)
        nativeMessanger.AndroidSendMessage("CreateDot," + JsonUtility.ToJson(defect));
    }

    public void WriteAllDefect(string filePath)
    {
        DefectArray defectArray = new DefectArray();
        defectArray.defect = defectList.ToArray();
        string jsonOutput = JsonUtility.ToJson(defectArray);

        File.WriteAllText(filePath, jsonOutput);
    }

    public void ReadAllDefect(string filePath)
    {

        print(filePath);

        WWW reader = new WWW(filePath);
        while (!reader.isDone)
        {
        }
        string json = reader.text;


        DefectArray defectArrayFromJson = JsonUtility.FromJson<DefectArray>(json);

        defectArray = defectArrayFromJson.defect;

        foreach (Defect defect in defectArray)
        {
            CreateDot(defect.position,defect.rotation,false);
        }
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
        nativeMessanger.AndroidSendMessage("SelectDefect." + JsonUtility.ToJson(defectArray[id]));
    }

    [Serializable]
    public class Defect
    {
        public string id;
        public string type;
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
