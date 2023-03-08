using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkManager : MonoBehaviour
{



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

  
    static string aptUnitId = "1234";
    string urlResidentsDefects = $"http://192.168.0.55:3000/api/residents/myDefects/{aptUnitId}";
    public void SetResidentsDefect(Vector3 pos,int panoramaId)
    {
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("location=거실&type=벽지&detail=들뜸&posX="));
       
        string location = "거실";
        string type = "벽지";
        string detail = "들뜸";
        string posX = pos.x.ToString();
        string posY = pos.y.ToString();
        string posZ = pos.z.ToString();
        string id = panoramaId.ToString();

        string content = string.Format("location={0}&type={1}&detail={2}&posX={3}&posY={4}&posZ={5}&panoramaId={6}",location,type,detail,posX,posY,posZ, id);

        string defects = null;
    }

    [System.Serializable]
    public class MyFefects
    {
        public string location;
        public Vector3 coordinate;
        public int panoramaId;
    }

    [System.Serializable]
    public class GetResidentsDefectType
    {
        public MyFefects[] myDefects;
    }




    string urlGetResidentsDefects = $"http://192.168.0.55:3000/api/residents/myDefects";
    public IEnumerator GetResidentsDefectsPositions(Action<Vector3[], int[]> callback)
    {
        Vector3[] v3List;
        int[] intList;
        Action<string> get = (string s) => { MyFefects[] myFects = JsonUtility.FromJson<GetResidentsDefectType>(s).myDefects;
            v3List = new Vector3[myFects.Length];
            intList = new int[myFects.Length];
            int myFectsCount = myFects.Length;

            for(int i = 0; i < myFectsCount; ++i)
            {
                v3List[i] = myFects[i].coordinate;
                intList[i] = myFects[i].panoramaId;
            }

            callback(v3List, intList);
        };
        yield return StartCoroutine(GetRequest(urlGetResidentsDefects, get));
        //callback();
    }
}
