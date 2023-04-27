using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using TMPro;
using UnityEngine.AI;
using Unity.VisualScripting;
using System.IO;
using System.Linq;
using static UnityEngine.UI.Image;
using UnityEngine.Networking;

public class Constructor : MonoBehaviour
{
    [SerializeField] private NetworkManager networkManager;
    [SerializeField] private Transform camGroup;
    [SerializeField] private GameObject dot;

    [SerializeField] private Transform modelFrame;
    [SerializeField] private Material meshMaterial;
    [SerializeField] private Material cubeMapMaterial;
    [SerializeField] private GameObject cubeMap;

    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private InputSystem inputSystem;

    [SerializeField] private MiniMapConstructor miniMapConstructor;
    [SerializeField] private GameObject movePoint;
    [SerializeField] private Transform movePointGroup;
    
    List<byte[]> textures;
    // Start is called before the first frame update

    private bool waitForFixedUpdated = false;
   
    void Start()
    {
#if UNITY_EDITOR
        //string url = Application.dataPath + "/Sources/";
        //StartCoroutine(FileLoad(url));
#endif
    }

    public void FileOpen(string url , Action<string> callback)
    {
        StartCoroutine(FileLoad(url, callback));
    }

    IEnumerator FileLoad(string url,Action<string> callback)
    {
        float loadTime = 0;

        byte[] envData = null;

        UnityWebRequest reader = UnityWebRequest.Get(url);
        reader.SendWebRequest();
        while (!reader.isDone && loadTime <= 5)
        {
            loadTime += Time.deltaTime;
            yield return null;
        }

        {
            envData = reader.downloadHandler.data;
            envToModel(envData, callback);
        }
    }

    void Update()
    {
        
    }

    public void MoveStage()
    {
        print("MoveStage:" );
    }

    public byte[] GetTexture(int index)
    {
        return textures[index];
    }

    private void LoadDefects()
    {
        Action<Vector3[] , Vector3[]> get = (Vector3[] defectPositions , Vector3[] defectRotations) => {

            int defectCount = defectPositions.Length;

            for(int i = 0; i < defectCount; ++i)
            {
                GameObject o = Instantiate(dot, defectPositions[i], Quaternion.Euler(defectRotations[i]));
                o.SetActive(true);

                print("TransformPosition : " + o.transform.position);
                print("TransformRotation : " + o.transform.eulerAngles);
            }
        };
        StartCoroutine(networkManager.GetResidentsDefectsPositions(get));

        
    }
    bool isLoadDone = false;
    private void envToModel(byte[] content , Action<string> callback)
    {
        int[] info = new int[4];
        int index = 16;
        float version;

        for (int i = 0; i < index; i = i + 4)
        {
            info[i / 4] = BitConverter.ToInt32(content, i);
        }

        version = BitConverter.ToSingle(content, index);
        index += 4;

        int[] verticeLengths = new int[info[2]];
        int iterCount = info[2] * 4;

        for (int i = 0; i < iterCount; i = i + 4)
        {
            verticeLengths[i / 4] = BitConverter.ToInt32(content, index + i);
        }

        index += iterCount;
        int[] indiciesLengths = new int[info[2]];
        iterCount = info[2] * 4;

        for (int i = 0; i < iterCount; i = i + 4)
        {
            indiciesLengths[i / 4] = BitConverter.ToInt32(content, index + i);
        }

        index += iterCount;
        int[] textureLengths = new int[info[3]];
        iterCount = info[3] * 4;
        for (int i = 0; i < iterCount; i = i + 4)
        {
            textureLengths[i / 4] = BitConverter.ToInt32(content, index + i);
        }

        index += iterCount;
        Vector3[] panoramaPosition = new Vector3[info[1]];
        Vector3[] panoramaRotation = new Vector3[info[1]];
        iterCount = info[1] * 12;
        for (int i = 0; i < iterCount; i = i + 12)
        {
            panoramaPosition[i / 12] = new Vector3(BitConverter.ToSingle(content, index + i), BitConverter.ToSingle(content, index + i + 4), BitConverter.ToSingle(content, index + i + 8));
        }

        index += iterCount;
        iterCount = info[1] * 12;
        for (int i = 0; i < iterCount; i = i + 12)
        {
            panoramaRotation[i / 12] = new Vector3(BitConverter.ToSingle(content, index + i), BitConverter.ToSingle(content, index + i + 4), BitConverter.ToSingle(content, index + i + 8));
        }

        textures = new List<byte[]>();
        List<byte[]> indiciesList = new List<byte[]>();
        List<byte[]> verticiesList = new List<byte[]>();

        float[] meshesTransform = new float[info[2] * 9];

        index += iterCount;

        foreach (int length in textureLengths)
        {
            byte[] bytes = new byte[length];
            Array.Copy(content, index, bytes, 0, length);
            index += length;

            textures.Add(bytes);
        }

        foreach (int length in verticeLengths)
        {
            byte[] bytes = new byte[length];
            Array.Copy(content, index, bytes, 0, length);
            index += length;

            verticiesList.Add(bytes);
        }

        foreach (int length in indiciesLengths)
        {
            byte[] bytes = new byte[length];
            Array.Copy(content, index, bytes, 0, length);
            index += length;

            indiciesList.Add(bytes);
        }

        for (int i = 0; i < info[2]; ++i)
        {
            for (int j = 0; j < 9; ++j)
            {
                byte[] bytes = new byte[4];
                Array.Copy(content, index, bytes, 0, 4);
                index += 4;

                meshesTransform[i * 9 + j] = BitConverter.ToSingle(bytes);
            }
        }

        List<Vector3[]> verticies = new List<Vector3[]>();
        List<int[]> indicies = new List<int[]>();

        Vector3[] meshesPositions = new Vector3[info[2]];
        Vector3[] meshesRotations = new Vector3[info[2]];
        Vector3[] meshesScales = new Vector3[info[2]];

        foreach (byte[] verticesBytes in verticiesList)
        {
            int byteLength = verticesBytes.Length;

            Vector3[] localVertices = new Vector3[byteLength / 12];
            for (int i = 0; i < byteLength; i = i + 12)
            {
                localVertices[i / 12] = new Vector3(BitConverter.ToSingle(verticesBytes, i), BitConverter.ToSingle(verticesBytes, i + 4), BitConverter.ToSingle(verticesBytes, i + 8));
            }
            verticies.Add(localVertices);

        }

        foreach (byte[] indiciesBytes in indiciesList)
        {
            int byteLength = indiciesBytes.Length;
            int[] localIndicies = new int[byteLength / 4];
            for (int i = 0; i < byteLength; i = i + 4)
            {
                localIndicies[i / 4] = BitConverter.ToInt32(indiciesBytes, i);
            }
            indicies.Add(localIndicies);
        }

        for (int i = 0; i < info[2]; ++i)
        {
            int byteLenngth = i * 9;

            meshesPositions[i] = new Vector3(meshesTransform[byteLenngth], meshesTransform[byteLenngth + 1], meshesTransform[byteLenngth + 2]);
            meshesRotations[i] = new Vector3(meshesTransform[byteLenngth + 3], meshesTransform[byteLenngth + 4], meshesTransform[byteLenngth + 5]);
            meshesScales[i] = new Vector3(meshesTransform[byteLenngth + 6], meshesTransform[byteLenngth + 7], meshesTransform[byteLenngth + 8]);
        }


        /////////////////////////////////////////////////////////////////////////////////////Above Same OutPutModelExporter


        for (int i = 0; i < info[2]; ++i)
        {
            GameObject o = new GameObject();
            Mesh mesh = new Mesh();

            mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

            mesh.vertices = verticies[i];
            mesh.triangles = indicies[i];

            o.AddComponent<MeshFilter>().sharedMesh = mesh;
            o.isStatic = true;
            o.AddComponent<MeshRenderer>().sharedMaterial = meshMaterial;
            o.AddComponent<MeshCollider>().sharedMesh = mesh;
            o.transform.parent = modelFrame;
            o.transform.localRotation = Quaternion.identity;
            o.transform.localScale = Vector3.one;
        }
       
        for(int i = 0; i < info[1]; ++i)
        {
            GameObject o = Instantiate(cubeMap);
            o.transform.parent = camGroup;
            o.transform.position = panoramaPosition[i];
            o.transform.localEulerAngles = panoramaRotation[i];
            o.transform.name = i.ToString();

            o.GetComponent<CubeMap>().SetMaterial(Instantiate(cubeMapMaterial));


        }
        playerMovement.Init();
        //playerMovement.InitStage(0);
        inputSystem.enabled = true;
        isLoadDone = true;

        callback("Success");
    }

    public void LateCall()
    {
       
    }

    IEnumerator AddMeshCollider(GameObject o)
    {
        yield return new WaitForEndOfFrame();
        o.AddComponent<MeshCollider>();
    }

    public IEnumerator InitStage(int stage)
    {
        yield return new WaitUntil(()=>isLoadDone);
        playerMovement.InitStage(stage);

      
        yield return new WaitForFixedUpdate();
        //SetMiniMap();
        waitForFixedUpdated = true;

        //SetMovePointsVisible();
    }

    public bool GetIsLoadDone()
    {
        return isLoadDone;

    }

    //16.95838
    private float GetMeshVerticesMaxDistance()
    {
        int modelFrameChildCounnt = modelFrame.childCount;

        List<Vector3> vertice = new List<Vector3>();

        for (int k = 1; k < modelFrameChildCounnt; ++k)
        {
            vertice.AddRange(modelFrame.GetChild(k).GetComponent<MeshFilter>().mesh.vertices.ToList());     
        }

        Vector3[] vertices = vertice.ToArray();

        float maxDistance = 0f;

        for (int i = 0; i < vertices.Length; i++)
        {
            for (int j = i + 1; j < vertices.Length; j++)
            {
                float distance = Vector3.Distance(vertices[i], vertices[j]);

                if (distance > maxDistance)
                {
                    maxDistance = distance;
                }
            }
        }

        return maxDistance;
    }

    private Vector3 GetMeshVerticesCenterpos()
    {
        int modelFrameChildCounnt = modelFrame.childCount;

        List<Vector3[]> vertice = new List<Vector3[]>();

        for (int k = 1; k < modelFrameChildCounnt; ++k)
        {
            vertice.Add(modelFrame.GetChild(k).GetComponent<MeshFilter>().mesh.vertices);
        }

        int arrayLength = 0;

        foreach (Vector3[] v in vertice)
        {
            arrayLength += v.Length;
        }

        Vector3[] vertices = new Vector3[arrayLength];

        int count = 0; 

        foreach (Vector3[] v in vertice)
        {
            v.CopyTo(vertices, count);
            count += v.Length;
        }

        Vector3 sum = Vector3.zero;

        foreach (Vector3 vertex in vertices)
        {
            sum += vertex;
        }

        sum /= arrayLength;

        return sum;
    }

    [SerializeField] private Material minimapMaterial;
    public IEnumerator SetMiniMap()
    {
        yield return new WaitUntil(() => waitForFixedUpdated);

        miniMapConstructor.ActiveMiniMap(true);

        int modelFrameChildCounnt = modelFrame.childCount;

        List<Vector3[]> vertice = new List<Vector3[]>();
        Transform minimapParent = miniMapConstructor.miniMap;
        for (int k = 1; k < modelFrameChildCounnt; ++k)
        {
            Transform child = modelFrame.GetChild(k);
            Transform o = Instantiate(child, child.position, child.rotation);
            o.parent = minimapParent;
            o.GetComponent<MeshRenderer>().sharedMaterial = minimapMaterial;
            o.localScale = Vector3.one * -1;
            o.gameObject.layer = LayerMask.NameToLayer("MiniMapMesh");
        }      
    }

    public IEnumerator SetMovePointsVisible(bool visible)
    {
        yield return new WaitUntil(()=>waitForFixedUpdated);

        int camGroupChild = camGroup.childCount;
        
        for(int i = 0; i < camGroupChild; ++i)
        {
            RaycastHit hit;
            if (Physics.Raycast(camGroup.GetChild(i).position, Vector3.down , out hit))
            {
               GameObject o = Instantiate(movePoint);
               o.SetActive(visible);
               o.transform.parent = movePointGroup;
               o.transform.position = hit.point + hit.normal * 0.001f;
            }
        }
    }

    
}
