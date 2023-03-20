using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkManager : MonoBehaviour
{
    [SerializeField] private Sprite sprite;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator GetRequest(string uri , Action<string> callback)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();
          

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    callback(webRequest.downloadHandler.text);
                    break;
            }
        }
    }

    IEnumerator PostRequest(string uri, List<IMultipartFormSection> form )
    {

        

        UnityWebRequest www = UnityWebRequest.Post(uri, form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
            Debug.Log(www.downloadHandler.text);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
            Debug.Log("Form upload complete!");
        }
    }

    static string aptUnitId = "6400356066a0704c58cd05a0";
    string urlResidentsDefects = $"http://192.168.0.55:3000/api/residents/myDefects/{aptUnitId}";

  
    
    public void SetResidentsDefect(Vector3 defectPosition , Vector3 defectRotation , float fov , Vector3 camPosition , Vector3 camRotation)
    {
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
     
       
        string location = "거실";
        string workType = "벽지";
        string detail = "들뜸";
        string defectImage = "0000000000000000000000000000000000000000\r\n0000000000000000000000000000000000000000\r\n1111100000000000000000000000000000011111\r\n1111100000000000000000000000000000001111\r\n1111000000000000000000000000000000001111\r\n0000000000000000000000000000000000000000\r\n0000000000000000000000001000000000000000\r\n0000000000000000000000000000011000000000\r\n0011111111100000000000000000111100000000\r\n0001111111111000000000000000011000000010\r\n0000000011110000000000000000010000000000\r\n0000000010010000000000000000010000000000\r\n0000001100000000000000000000000001000000\r\n0000000000000000000000000000000100000000\r\n0000000000000000000000000000000000000000\r\n0000000000000000000000000000000000000000\r\n0000000000000000000000000000000000000000\r\n0000000000000000000000000000000000000000\r\n0000000000000000000000000000000000000000\r\n0000000000000000000000000000000000000000\r\n";
        string defectPositionX = defectPosition.x.ToString();
        string defectPositionY = defectPosition.y.ToString();
        string defectPositionZ = defectPosition.z.ToString();
        string defectRotationX = defectRotation.x.ToString();
        string defectRotationY = defectRotation.y.ToString();
        string defectRotationZ = defectRotation.z.ToString();

        string camPositionX = camPosition.x.ToString();
        string camPositionY = camPosition.y.ToString();
        string camPositionZ = camPosition.z.ToString();
        string camRotationX = camRotation.x.ToString();
        string camRotationY = camRotation.y.ToString();
        string camRotationZ = camRotation.z.ToString();

        string camFov = fov.ToString();

        //string content = string.Format("location={0}&workType={1}&detail={2}&defectImage={3}&defectPositionX={4}&defectPositionY={5}&defectPositionZ={6}&defectRotationX={7}&defectRotationY={8}&defectRotationZ={9}&camFov={10}",
        //    location,workType,detail, defectImage, defectPositionX, defectPositionY, defectPositionZ,defectRotationX,defectRotationY,defectRotationZ,camFov);



        //print("Conntent:" + content);
        byte[] imageData = sprite.texture.EncodeToPNG();
        //imageData = imageData.enc

        formData.Add(new MultipartFormDataSection("location",location));
        formData.Add(new MultipartFormDataSection("workType", workType));
        formData.Add(new MultipartFormDataSection("detail", detail));
        formData.Add(new MultipartFormFileSection("defectImage", imageData, "test.png", "image/png"));
        formData.Add(new MultipartFormDataSection("defectPositionX", defectPositionX));
        formData.Add(new MultipartFormDataSection("defectPositionY", defectPositionY));
        formData.Add(new MultipartFormDataSection("defectPositionZ", defectPositionZ));
        formData.Add(new MultipartFormDataSection("defectRotationX", defectRotationX));
        formData.Add(new MultipartFormDataSection("defectRotationY", defectRotationY));
        formData.Add(new MultipartFormDataSection("defectRotationZ", defectRotationZ));

        formData.Add(new MultipartFormDataSection("camPositionX", camPositionX));
        formData.Add(new MultipartFormDataSection("camPositionY", camPositionY));
        formData.Add(new MultipartFormDataSection("camPositionZ", camPositionZ));
        formData.Add(new MultipartFormDataSection("camRotationX", camRotationX));
        formData.Add(new MultipartFormDataSection("camRotationY", camRotationY));
        formData.Add(new MultipartFormDataSection("camRotationZ", camRotationZ));

        formData.Add(new MultipartFormDataSection("camFov", camFov));
        
        StartCoroutine(PostRequest(urlResidentsDefects,formData));

        //playerMovement
    }

    [System.Serializable]
    public class MyFefects
    {
        public ViewrInfo viewerInfo;
    }

    [System.Serializable]
    public class ViewrInfo
    {
        public Vector3 defectPosition;
        public Vector3 defectRotation;
        public Vector3 camPosition;
        public Vector3 camRotation;
        public float camFov;
    }

    [System.Serializable]
    public class GetResidentsDefectType
    {
        public MyFefects[] myDefects;
    }




    string urlGetResidentsDefects = $"http://192.168.0.55:3000/api/residents/myDefects";

    public Vector3[] camPositions;
    public Vector3[] camRotations;
    public float[] camFovs;
    public IEnumerator GetResidentsDefectsPositions(Action<Vector3[], Vector3[]> callback)
    {
        Vector3[] positions;
        Vector3[] rotations;
       

        Action<string> get = (string s) => { MyFefects[] myFects = JsonUtility.FromJson<GetResidentsDefectType>(s).myDefects;
        
            positions = new Vector3[myFects.Length];
            rotations = new Vector3[myFects.Length];
            camPositions = new Vector3[myFects.Length];
            camRotations = new Vector3[myFects.Length];
            camFovs = new float[myFects.Length];

            int myFectsCount = myFects.Length;

            for(int i = 0; i < myFectsCount; ++i)
            {
               positions[i] = myFects[i].viewerInfo.defectPosition;
               rotations[i] = myFects[i].viewerInfo.defectRotation;

                camPositions[i] = myFects[i].viewerInfo.camPosition;
                camRotations[i] = myFects[i].viewerInfo.camRotation;

                camFovs[i] = myFects[i].viewerInfo.camFov;


                //positions[i] = new Vector3(8.298171997070313f,0.7981052398681641f,4.192317962646484f);
                //rotations[i] = new Vector3(-88.419f, -0.103f, 0);

                //print("Fov : ");

                //UnityEditor.TransformWorldPlacementJSON:{ "position":{ "x":8.298171997070313,"y":0.7981052398681641,"z":4.192317962646484},"rotation":{ "x":-0.6972823143005371,"y":-0.0006447452469728887,"z":-0.0006271929596550763,"w":0.7167960405349731},"scale":{ "x":0.003000000026077032,"y":0.003000000026077032,"z":0.003000000026077032} }
                //print(rotations[i]);

                //intList[i] = myFects[i].viewerInfo;
            }

            callback(positions, rotations);
        };
        yield return StartCoroutine(GetRequest(urlGetResidentsDefects, get));
        //callback();
    }

 

}
