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
using UnityEngine.SceneManagement;
using static DefectConstructor;

using System.Text;
using static Constructor;


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

    [SerializeField] private Measurement measurement;
    [SerializeField] private DefectConstructor defectConstructor;

    List<byte[]> textures;

    [SerializeField] private HomeTourConstructor homeTourConstructor;
    // Start is called before the first frame update

    private bool waitForFixedUpdated = false;

    private int inverse = 1;

    void Start()
    {
        BoundTest();

#if UNITY_EDITOR
        //string url = Application.dataPath + "/Sources/";
        //StartCoroutine(FileLoad(url));
#endif
    }

    public void FileOpen(string url, Action<string> callback)
    {
#if UNITY_EDITOR_OSX
        StartCoroutine(FileLoadReadByte(url, callback));
#elif UNITY_EDITOR 
        StartCoroutine(FileLoad(url, callback));
#elif UNITY_IOS
        StartCoroutine(FileLoadReadByte(url, callback));
#elif UNITY_ANDROID
        //string url = Application.dataPath + "/Sources/";
        //StartCoroutine(FileLoadReadByte(url,callback));
        StartCoroutine(FileLoad(url, callback));
#elif UNITY_WEBGL
 StartCoroutine(FileLoad(url, callback));
#endif



    }

    IEnumerator FileLoad(string url, Action<string> callback)
    {
        print("StartCoruoutine FileLoad");

        if (File.Exists(url))
        {
            print("File Exist");
        }
        else
        {
            print("File Doesn't Exist");
        }

        float loadTime = 0;

        byte[] envData = null;

        UnityWebRequest reader = UnityWebRequest.Get(url);
        reader.SendWebRequest();
        while (!reader.isDone && loadTime <= 5)
        {
            loadTime += Time.deltaTime;
            yield return null;
        }

        if (loadTime >= 5)
        {
            print("Faile File Load");
        }

        {
            envData = reader.downloadHandler.data;
            print("ReadEnv" + envData.Length);
            envToModel(envData, callback);
        }
    }
    IEnumerator FileLoadReadByte(string url, Action<string> callback)
    {
        Debug.Log("File Load Start");

        if (File.Exists(url))
        {
            print("File Exist");
        }
        else
        {
            print("File Doesn't Exist");
        }

        byte[] tBytes = File.ReadAllBytes(url);

        print("TBytesLength"+tBytes.Length);  

        envToModel(tBytes,callback);

        yield return null;
    }

    void Update()
    {

    }

    public void MoveStage()
    {
        print("MoveStage:");
    }

    public byte[] GetTexture(int index)
    {
        return textures[index];
    }

    private void LoadDefects()
    {
        Action<Vector3[], Vector3[]> get = (Vector3[] defectPositions, Vector3[] defectRotations) => {

            int defectCount = defectPositions.Length;

            for (int i = 0; i < defectCount; ++i)
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
    private void envToModel(byte[] content, Action<string> callback)
    {

        print("Envtomodel");

        //int[] info = new int[4];
        //int index = 16;
        //float version;

        //for (int i = 0; i < index; i = i + 4)
        //{
        //    info[i / 4] = BitConverter.ToInt32(content, i);
        //}

        //version = BitConverter.ToSingle(content, index);
        //index += 4;

        //int[] verticeLengths = new int[info[2]];
        //int iterCount = info[2] * 4;

        //for (int i = 0; i < iterCount; i = i + 4)
        //{
        //    verticeLengths[i / 4] = BitConverter.ToInt32(content, index + i);
        //}

        //index += iterCount;
        //int[] indiciesLengths = new int[info[2]];
        //iterCount = info[2] * 4;

        //for (int i = 0; i < iterCount; i = i + 4)
        //{
        //    indiciesLengths[i / 4] = BitConverter.ToInt32(content, index + i);
        //}

        //index += iterCount;
        //int[] textureLengths = new int[info[3]];
        //iterCount = info[3] * 4;
        //for (int i = 0; i < iterCount; i = i + 4)
        //{
        //    textureLengths[i / 4] = BitConverter.ToInt32(content, index + i);
        //}

        //index += iterCount;
        //Vector3[] panoramaPosition = new Vector3[info[1]];
        //Vector3[] panoramaRotation = new Vector3[info[1]];
        //iterCount = info[1] * 12;
        //for (int i = 0; i < iterCount; i = i + 12)
        //{
        //    panoramaPosition[i / 12] = new Vector3(BitConverter.ToSingle(content, index + i), BitConverter.ToSingle(content, index + i + 4), BitConverter.ToSingle(content, index + i + 8));
        //}

        //index += iterCount;
        //iterCount = info[1] * 12;
        //for (int i = 0; i < iterCount; i = i + 12)
        //{
        //    panoramaRotation[i / 12] = new Vector3(BitConverter.ToSingle(content, index + i), BitConverter.ToSingle(content, index + i + 4), BitConverter.ToSingle(content, index + i + 8));
        //}

        //textures = new List<byte[]>();
        //List<byte[]> indiciesList = new List<byte[]>();
        //List<byte[]> verticiesList = new List<byte[]>();

        //float[] meshesTransform = new float[info[2] * 9];

        //index += iterCount;

        //foreach (int length in textureLengths)
        //{
        //    byte[] bytes = new byte[length];
        //    Array.Copy(content, index, bytes, 0, length);
        //    index += length;

        //    textures.Add(bytes);
        //}

        //foreach (int length in verticeLengths)
        //{
        //    byte[] bytes = new byte[length];
        //    Array.Copy(content, index, bytes, 0, length);
        //    index += length;

        //    verticiesList.Add(bytes);
        //}

        //foreach (int length in indiciesLengths)
        //{
        //    byte[] bytes = new byte[length];
        //    Array.Copy(content, index, bytes, 0, length);
        //    index += length;

        //    indiciesList.Add(bytes);
        //}

        //for (int i = 0; i < info[2]; ++i)
        //{
        //    for (int j = 0; j < 9; ++j)
        //    {
        //        byte[] bytes = new byte[4];
        //        Array.Copy(content, index, bytes, 0, 4);
        //        index += 4;

        //        meshesTransform[i * 9 + j] = BitConverter.ToSingle(bytes,0);
        //    }
        //}

        //List<Vector3[]> verticies = new List<Vector3[]>();
        //List<int[]> indicies = new List<int[]>();

        //Vector3[] meshesPositions = new Vector3[info[2]];
        //Vector3[] meshesRotations = new Vector3[info[2]];
        //Vector3[] meshesScales = new Vector3[info[2]];

        //foreach (byte[] verticesBytes in verticiesList)
        //{
        //    int byteLength = verticesBytes.Length;

        //    Vector3[] localVertices = new Vector3[byteLength / 12];
        //    for (int i = 0; i < byteLength; i = i + 12)
        //    {
        //        localVertices[i / 12] = new Vector3(BitConverter.ToSingle(verticesBytes, i), BitConverter.ToSingle(verticesBytes, i + 4), BitConverter.ToSingle(verticesBytes, i + 8));
        //    }
        //    verticies.Add(localVertices);

        //}

        //foreach (byte[] indiciesBytes in indiciesList)
        //{
        //    int byteLength = indiciesBytes.Length;
        //    int[] localIndicies = new int[byteLength / 4];
        //    for (int i = 0; i < byteLength; i = i + 4)
        //    {
        //        localIndicies[i / 4] = BitConverter.ToInt32(indiciesBytes, i);
        //    }
        //    indicies.Add(localIndicies);
        //}

        //for (int i = 0; i < info[2]; ++i)
        //{
        //    int byteLenngth = i * 9;

        //    meshesPositions[i] = new Vector3(meshesTransform[byteLenngth], meshesTransform[byteLenngth + 1], meshesTransform[byteLenngth + 2]);
        //    meshesRotations[i] = new Vector3(meshesTransform[byteLenngth + 3], meshesTransform[byteLenngth + 4], meshesTransform[byteLenngth + 5]);
        //    meshesScales[i] = new Vector3(meshesTransform[byteLenngth + 6], meshesTransform[byteLenngth + 7], meshesTransform[byteLenngth + 8]);
        //}



        //int[] info = new int[4];
        //int index = 16;
        //float version;
        //int modelType;
        //int boundBytesLength;
        //int panoramaTagLength;

        //for (int i = 0; i < index; i = i + 4)
        //{
        //    info[i / 4] = BitConverter.ToInt32(content, i);
        //}

        //version = BitConverter.ToSingle(content, index);
        //index += 4;
        //print("Version" + version);

        //modelType = BitConverter.ToInt32(content, index);
        //index += 4;

        //boundBytesLength = BitConverter.ToInt32(content, index);
        //index += 4;
        //print("bounBytesLength" + boundBytesLength);

        //panoramaTagLength = BitConverter.ToInt32(content, index);
        //index += 4;
        //print("panoramaTagLength" + panoramaTagLength);

        //int[] verticeLengths = new int[info[2]];
        //int iterCount = info[2] * 4;
        //for (int i = 0; i < iterCount; i = i + 4)
        //{
        //    verticeLengths[i / 4] = BitConverter.ToInt32(content, index + i);
        //}

        //index += iterCount;
        //int[] indiciesLengths = new int[info[2]];
        //iterCount = info[2] * 4;

        //for (int i = 0; i < iterCount; i = i + 4)
        //{
        //    indiciesLengths[i / 4] = BitConverter.ToInt32(content, index + i);
        //}

        //index += iterCount;
        //int[] textureLengths = new int[info[3]];

        //print("info[3] :" + info[3]);

        //iterCount = info[3] * 4;
        //for (int i = 0; i < iterCount; i = i + 4)
        //{
        //    textureLengths[i / 4] = BitConverter.ToInt32(content, index + i);
        //}

        //index += iterCount;
        //Vector3[] panoramaPosition = new Vector3[info[1]];
        //Vector3[] panoramaRotation = new Vector3[info[1]];
        //iterCount = info[1] * 12;
        //for (int i = 0; i < iterCount; i = i + 12)
        //{
        //    panoramaPosition[i / 12] = new Vector3(BitConverter.ToSingle(content, index + i), BitConverter.ToSingle(content, index + i + 4), BitConverter.ToSingle(content, index + i + 8));
        //}

        //index += iterCount;
        //iterCount = info[1] * 12;
        //for (int i = 0; i < iterCount; i = i + 12)
        //{
        //    panoramaRotation[i / 12] = new Vector3(BitConverter.ToSingle(content, index + i), BitConverter.ToSingle(content, index + i + 4), BitConverter.ToSingle(content, index + i + 8));
        //}

        //textures = new List<byte[]>();
        //List<byte[]> indiciesList = new List<byte[]>();
        //List<byte[]> verticiesList = new List<byte[]>();

        //float[] meshesTransform = new float[info[2] * 9];
        //string panoramaTagJson = null;
        //string boundaryJson = null;

        //index += iterCount;

        //foreach (int length in textureLengths)
        //{
        //    byte[] bytes = new byte[length];
        //    Array.Copy(content, index, bytes, 0, length);
        //    index += length;

        //    textures.Add(bytes);
        //}

        //foreach (int length in verticeLengths)
        //{
        //    byte[] bytes = new byte[length];
        //    Array.Copy(content, index, bytes, 0, length);
        //    index += length;

        //    verticiesList.Add(bytes);
        //}

        //foreach (int length in indiciesLengths)
        //{
        //    byte[] bytes = new byte[length];
        //    Array.Copy(content, index, bytes, 0, length);
        //    index += length;

        //    indiciesList.Add(bytes);
        //}

        //for (int i = 0; i < info[2]; ++i)
        //{
        //    for (int j = 0; j < 9; ++j)
        //    {
        //        byte[] bytes = new byte[4];
        //        Array.Copy(content, index, bytes, 0, 4);
        //        index += 4;

        //        meshesTransform[i * 9 + j] = BitConverter.ToSingle(bytes,0);
        //    }
        //}

        //byte[] boundBytes = new byte[boundBytesLength];
        //Array.Copy(content, index, boundBytes, 0, boundBytesLength);
        //index += boundBytesLength;

        //byte[] panoramaTagBytes = new byte[panoramaTagLength];
        //Array.Copy(content, index, panoramaTagBytes, 0, panoramaTagLength);
        //index += panoramaTagLength;


        //List<Vector3[]> verticies = new List<Vector3[]>();
        //List<int[]> indicies = new List<int[]>();

        //Vector3[] meshesPositions = new Vector3[info[2]];
        //Vector3[] meshesRotations = new Vector3[info[2]];
        //Vector3[] meshesScales = new Vector3[info[2]];

        //foreach (byte[] verticesBytes in verticiesList)
        //{
        //    int byteLength = verticesBytes.Length;

        //    Vector3[] localVertices = new Vector3[byteLength / 12];
        //    for (int i = 0; i < byteLength; i = i + 12)
        //    {
        //        localVertices[i / 12] = new Vector3(BitConverter.ToSingle(verticesBytes, i), BitConverter.ToSingle(verticesBytes, i + 4), BitConverter.ToSingle(verticesBytes, i + 8));
        //    }
        //    verticies.Add(localVertices);

        //}

        //foreach (byte[] indiciesBytes in indiciesList)
        //{
        //    int byteLength = indiciesBytes.Length;
        //    int[] localIndicies = new int[byteLength / 4];
        //    for (int i = 0; i < byteLength; i = i + 4)
        //    {
        //        localIndicies[i / 4] = BitConverter.ToInt32(indiciesBytes, i);
        //    }
        //    indicies.Add(localIndicies);
        //}

        //for (int i = 0; i < info[2]; ++i)
        //{
        //    int byteLenngth = i * 9;

        //    meshesPositions[i] = new Vector3(meshesTransform[byteLenngth], meshesTransform[byteLenngth + 1], meshesTransform[byteLenngth + 2]);
        //    meshesRotations[i] = new Vector3(meshesTransform[byteLenngth + 3], meshesTransform[byteLenngth + 4], meshesTransform[byteLenngth + 5]);
        //    meshesScales[i] = new Vector3(meshesTransform[byteLenngth + 6], meshesTransform[byteLenngth + 7], meshesTransform[byteLenngth + 8]);
        //}

        //panoramaTagJson = UTF8Encoding.UTF8.GetString(panoramaTagBytes);
        //boundaryJson = UTF8Encoding.UTF8.GetString(boundBytes);

        int[] info = new int[4];
        int index = 16;
        float version;
        int modelType;
        int boundBytesLength;
        int panoramaTagLength;
        int homeTourLength;

        for (int i = 0; i < index; i = i + 4)
        {
            info[i / 4] = BitConverter.ToInt32(content, i);
        }

        version = BitConverter.ToSingle(content, index);
        index += 4;
        print("Version" + version);

        modelType = BitConverter.ToInt32(content, index);
        index += 4;

        boundBytesLength = BitConverter.ToInt32(content, index);
        index += 4;
        print("bounBytesLength" + boundBytesLength);

        panoramaTagLength = BitConverter.ToInt32(content, index);
        index += 4;
        print("panoramaTagLength" + panoramaTagLength);

        homeTourLength = BitConverter.ToInt32(content, index);
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

        print("info[3] :" + info[3]);

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

        string panoramaTagJson = null;
        string boundaryJson = null;
        string homeTourJson = null;

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

                meshesTransform[i * 9 + j] = BitConverter.ToSingle(bytes,0);
            }
        }

        byte[] boundBytes = new byte[boundBytesLength];
        Array.Copy(content, index, boundBytes, 0, boundBytesLength);
        index += boundBytesLength;

        byte[] panoramaTagBytes = new byte[panoramaTagLength];
        Array.Copy(content, index, panoramaTagBytes, 0, panoramaTagLength);
        index += panoramaTagLength;

        byte[] homeTourBytes = new byte[homeTourLength];
        Array.Copy(content, index, homeTourBytes, 0, homeTourLength);
        index += homeTourLength;

        //byte[] hometourJson = new byte[hometourjson]


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

        panoramaTagJson = UTF8Encoding.UTF8.GetString(panoramaTagBytes);
        boundaryJson = UTF8Encoding.UTF8.GetString(boundBytes);
        homeTourJson = UTF8Encoding.UTF8.GetString(homeTourBytes);

        /////////////////////////////////////////////////////////////////////////////////////Above Same OutPutModelExporter


        print("파일 정보 길이:" + info[0]);
        print("파노라마 개수:" + info[1]);
        print("메쉬 개수:" + info[2]);
        print("텍스쳐 개수:" + info[3]);
        print("버젼:" + version);
        print("모델 타입" + modelType);


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

            o.transform.position = meshesPositions[i];
            o.transform.eulerAngles = meshesRotations[i];
            o.transform.localScale = meshesScales[i];
        }

        for (int i = 0; i < info[1]; ++i)
        {
            GameObject o = Instantiate(cubeMap);
            o.transform.parent = camGroup;
            o.transform.position = panoramaPosition[i];
            o.transform.localEulerAngles = panoramaRotation[i];
            o.transform.name = i.ToString();

            o.GetComponent<CubeMap>().SetMaterial(Instantiate(cubeMapMaterial));


        }

        modelFrame.localScale = new Vector3(-1, -1, -1 * inverse);

        playerMovement.Init();
        inputSystem.enabled = true;

        isLoadDone = true;

        CreateBounds(boundaryJson);
        
        CreatePanoramaTags(panoramaTagJson);

        homeTourConstructor.InitHomeTourData(homeTourJson);


    }

    public IEnumerator InitStage(int stage , Action<string> callBack)
    {
        yield return new WaitUntil(() => isLoadDone);
        playerMovement.InitStage(stage);
        
        yield return new WaitForFixedUpdate();

        waitForFixedUpdated = true;

        //callBack
        callBack("Success");
    }

    public IEnumerator InitStageTag(string tag ,Action<string> callBack)
    {
        yield return new WaitUntil(() => isLoadDone);
       // playerMovement.InitStage(stage);

        foreach(PanoramaView panoView in panormaViews.panoramaViews)
        {
            print(panoView.tag + ":" + tag);
            if(panoView.tag == tag)
            {
                playerMovement.InitStage(panoView.position , panoView.rotation);
                break;
            }
        }

        yield return new WaitForFixedUpdate();

        waitForFixedUpdated = true;

        //callBack
        callBack("Success");
    }

    public IEnumerator InitStageHome(Action<string> callBack)
    {
        yield return new WaitUntil(() => isLoadDone);

        HomeTourConstructor.Root root = homeTourConstructor.GetRoot();


        if(root != null)
        {
            playerMovement.InitStage(root.initHomePosition.position, root.initHomePosition.rotation);
        }

        yield return new WaitForFixedUpdate();

        waitForFixedUpdated = true;

        callBack("Success");
    }

    public bool GetIsLoadDone()
    {
        return isLoadDone;
    }

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
        yield return new WaitUntil(() => waitForFixedUpdated);

        int camGroupChild = camGroup.childCount;

        for (int i = 0; i < camGroupChild; ++i)
        {
            RaycastHit hit;
            if (Physics.Raycast(camGroup.GetChild(i).position, Vector3.down, out hit))
            {
                GameObject o = Instantiate(movePoint);
                o.SetActive(visible);
                o.transform.parent = movePointGroup;
                o.transform.position = hit.point + hit.normal * 0.001f;
            }
        }
    }
    public void Init()
    {

        print("init");

        isLoadDone = false;
        int camGroupChildCount = camGroup.childCount - 1;

        for (int i = camGroupChildCount; i >= 0; --i)
        {
            Destroy(camGroup.GetChild(i).gameObject);
        }

        //Destroy(modelFrame.GetChild(1).gameObject);
        
        for(int i = 1; i < modelFrame.childCount; ++i)
        {
           // print("modelframeChild" + modelFrameChild.name);
            Destroy(modelFrame.GetChild(i).gameObject);
        }


        defectConstructor.Init();
        measurement.Init();

        Resources.UnloadUnusedAssets();
    }



    Bounds bounds;
    public void CreateBounds(string json)
    {
        bounds = JsonUtility.FromJson<Bounds>(json);
        
        print("boundsLength:" + bounds.bounds.Count);
    }
    PanoramaViews panormaViews;
    public void CreatePanoramaTags(string json)
    {
        panormaViews = JsonUtility.FromJson<PanoramaViews>(json);
        print("PanoramaViews:" + panormaViews.panoramaViews.Length);
    }

    public void CreateBoundaryTest()
    {




        //isInsidePolygon();

        //boundaries = new Boundary[13];

        //int i = 0;

        //boundaries[i] = new Boundary();
        //boundaries[i].name = "침실1-욕실";
        //boundaries[i].polygon = new Vector2[4];
        //boundaries[i].polygon[0] = new Vector2(4.55231f, 6.52542f);
        //boundaries[i].polygon[1] = new Vector2(7.130107f, 6.56804f);
        //boundaries[i].polygon[2] = new Vector2(7.002285f, 4.671975f);
        //boundaries[i].polygon[3] = new Vector2(4.509701f, 4.608055f);
        //++i;

        //boundaries[i] = new Boundary();
        //boundaries[i].name = "침실1-화장대";
        //boundaries[i].polygon = new Vector2[4];
        //boundaries[i].polygon[0] = new Vector2(7.130107f, 6.56804f);
        //boundaries[i].polygon[1] = new Vector2(8.685311f, 6.141956f);
        //boundaries[i].polygon[2] = new Vector2(8.727916f, 4.331108f);
        //boundaries[i].polygon[3] = new Vector2(7.002285f, 4.671975f);
        //++i;

        //boundaries[i] = new Boundary();
        //boundaries[i].name = "침실1";
        //boundaries[i].polygon = new Vector2[4];
        //boundaries[i].polygon[0] = new Vector2(8.685311f, 6.141956f);
        //boundaries[i].polygon[1] = new Vector2(12.58396f, 6.482823f);
        //boundaries[i].polygon[2] = new Vector2(12.58395f, 2.328519f);
        //boundaries[i].polygon[3] = new Vector2(8.727903f, 2.328519f);
        //++i;

        //boundaries[i] = new Boundary();
        //boundaries[i].name = "발코니";
        //boundaries[i].polygon = new Vector2[4];
        //boundaries[i].polygon[0] = new Vector2(12.62657f, 4.416324f);
        //boundaries[i].polygon[1] = new Vector2(14.60785f, 4.416324f);
        //boundaries[i].polygon[2] = new Vector2(14.56524f, 2.307214f);
        //boundaries[i].polygon[3] = new Vector2(12.58395f, 2.328519f);
        //++i;

        //boundaries[i] = new Boundary();
        //boundaries[i].name = "거실";
        //boundaries[i].polygon = new Vector2[4];
        //boundaries[i].polygon[0] = new Vector2(8.727903f, 2.328519f);
        //boundaries[i].polygon[1] = new Vector2(14.56524f, 2.307214f);
        //boundaries[i].polygon[2] = new Vector2(14.3735f, -2.592732f);
        //boundaries[i].polygon[3] = new Vector2(9.771808f, -2.614036f);
        //++i;

        //boundaries[i] = new Boundary();
        //boundaries[i].name = "거실";
        //boundaries[i].polygon = new Vector2[4];
        //boundaries[i].polygon[0] = new Vector2(6.128814f, 2.413735f);
        //boundaries[i].polygon[1] = new Vector2(8.727903f, 2.328519f);
        //boundaries[i].polygon[2] = new Vector2(9.782036f, -1.643538f);
        //boundaries[i].polygon[3] = new Vector2(6.725424f, -1.274137f);
        //++i;

        //boundaries[i] = new Boundary();
        //boundaries[i].name = "주방/식당";
        //boundaries[i].polygon = new Vector2[4];
        //boundaries[i].polygon[0] = new Vector2(6.725424f, -1.274137f);
        //boundaries[i].polygon[1] = new Vector2(9.782036f, -1.643538f);
        //boundaries[i].polygon[2] = new Vector2(9.782012f, -3.601231f);
        //boundaries[i].polygon[3] = new Vector2(6.743129f, -3.649982f);
        //++i;

        //boundaries[i] = new Boundary();
        //boundaries[i].name = "발코니";
        //boundaries[i].polygon = new Vector2[4];
        //boundaries[i].polygon[0] = new Vector2(5.443068f, -0.2698274f);
        //boundaries[i].polygon[1] = new Vector2(6.701488f, -0.2826328f);
        //boundaries[i].polygon[2] = new Vector2(6.743129f, -3.649982f);
        //boundaries[i].polygon[3] = new Vector2(5.443068f, -3.763739f);
        //++i;

        //boundaries[i] = new Boundary();
        //boundaries[i].name = "침실";
        //boundaries[i].polygon = new Vector2[4];
        //boundaries[i].polygon[0] = new Vector2(2.273213f, 0.9304333f);
        //boundaries[i].polygon[1] = new Vector2(5.416673f, 0.9401765f);
        //boundaries[i].polygon[2] = new Vector2(5.428894f, -2.970673f);
        //boundaries[i].polygon[3] = new Vector2(2.27577f, -2.897345f);
        //++i;

        //boundaries[i] = new Boundary();
        //boundaries[i].name = "침실";
        //boundaries[i].polygon = new Vector2[4];
        //boundaries[i].polygon[0] = new Vector2(-1.205194f, 2.474407f);
        //boundaries[i].polygon[1] = new Vector2(2.208837f, 2.449724f);
        //boundaries[i].polygon[2] = new Vector2(2.251328f, -1.601876f);
        //boundaries[i].polygon[3] = new Vector2(-1.102343f, -1.668993f);
        //++i;

        //boundaries[i] = new Boundary();
        //boundaries[i].name = "거실";
        //boundaries[i].polygon = new Vector2[4];
        //boundaries[i].polygon[0] = new Vector2(2.208837f, 2.449724f);
        //boundaries[i].polygon[1] = new Vector2(6.128814f, 2.413735f);
        //boundaries[i].polygon[2] = new Vector2(6.650831f, 0.9304333f);
        //boundaries[i].polygon[3] = new Vector2(2.273213f, 0.9304333f);
        //++i;

        //boundaries[i] = new Boundary();
        //boundaries[i].name = "현관";
        //boundaries[i].polygon = new Vector2[4];
        //boundaries[i].polygon[0] = new Vector2(2.170209f, 4.445397f);
        //boundaries[i].polygon[1] = new Vector2(4.509701f, 4.608055f);
        //boundaries[i].polygon[2] = new Vector2(4.577899f, 2.320971f);
        //boundaries[i].polygon[3] = new Vector2(2.208837f, 2.449724f);
        //++i;

        //boundaries[i] = new Boundary();
        //boundaries[i].name = "욕실";
        //boundaries[i].polygon = new Vector2[4];
        //boundaries[i].polygon[0] = new Vector2(5.873165f, 3.905023f);
        //boundaries[i].polygon[1] = new Vector2(8.727916f, 4.331108f);
        //boundaries[i].polygon[2] = new Vector2(8.727903f, 2.328519f);
        //boundaries[i].polygon[3] = new Vector2(6.128814f, 2.413735f);
        //++i;
    }
    public void CreateBoundaries(string json)
    {
       
    }

    public RoomPositionInfo GetBoundary(Vector2 v2)
    {
        int count = 0;

        RoomPositionInfo roomPositionInfo = null;

        foreach (bound boundary in bounds.bounds)
        {
            List<Vector3> boundaryV3 = new List<Vector3>();

            foreach(Vector3 v3 in boundary.vertices)
            {
                boundaryV3.Add(new Vector3(v3.x,0,v3.y));
            }

            if (isInsidePolygon (boundaryV3, new Vector3(v2.x,0, v2.y)))
            {
                roomPositionInfo = new RoomPositionInfo();
                roomPositionInfo.name = boundary.name;
                roomPositionInfo.flawPrtbGrpCd = boundary.flawPrtbGrpCd;
                roomPositionInfo.detailName = boundary.detailName;
                roomPositionInfo.flawPrtbCd = boundary.flawPrtbCd;
                return roomPositionInfo;
            }

            boundaryV3.Clear();

            ++count;
       }

        if(roomPositionInfo == null)
        {
            return GetBoundary2(v2);
        }

        return null;
    }

    public RoomPositionInfo GetBoundary2(Vector2 v2)
    {
        int count = 0;

        RoomPositionInfo roomPositionInfo = null;

        v2 = GetNearPanoPosition(new Vector3(v2.x,0,v2.y));

        foreach (bound boundary in bounds.bounds)
        {
            List<Vector3> boundaryV3 = new List<Vector3>();

            foreach (Vector3 v3 in boundary.vertices)
            {
                boundaryV3.Add(new Vector3(v3.x, 0, v3.y));
            }

            if (isInsidePolygon(boundaryV3, new Vector3(v2.x, 0, v2.y)))
            {
                roomPositionInfo = new RoomPositionInfo();
                roomPositionInfo.name = boundary.name;
                roomPositionInfo.flawPrtbGrpCd = boundary.flawPrtbGrpCd;
                roomPositionInfo.detailName = boundary.detailName;
                roomPositionInfo.flawPrtbCd = boundary.flawPrtbCd;
                return roomPositionInfo;
            }

            boundaryV3.Clear();

            ++count;
        }

        return roomPositionInfo;
    }

    public Vector2 GetNearPanoPosition(Vector3 input)
    {
        float minDis = 9999999;
        Vector3 minDisPosition = input;

        foreach (Transform tf in camGroup)
        {
            float dis = Vector3.Distance(tf.position, input);
        
            if(dis < minDis)
            {
                minDis = dis;
                minDisPosition = tf.position;

                print("Min" + tf.name);
            }
        }

        print("minDisPosition" + minDisPosition);

        return new Vector2(minDisPosition.x,minDisPosition.z);
    }

    public void BoundTest()
    {
        List<Vector3> vector3s = new List<Vector3>();
        //vector3s.Add(new Vector3(0, 0, 0));
        //vector3s.Add(new Vector3(0, 2, 0));
        //vector3s.Add(new Vector3(2, 0, 0));
        //vector3s.Add(new Vector3(2, 2, 0));

        //Vector3 v3 = new Vector3(1, 1, 0);

        //vector3s.Add(new Vector3(0, 0, 0));
        //vector3s.Add(new Vector3(0, 0, 2));
        //vector3s.Add(new Vector3(2, 0, 2));
        //vector3s.Add(new Vector3(2, 0, 0));

        //Vector3 v3 = new Vector3(1, 0, 1);

        vector3s.Add(new Vector3(0, 0, 0));
        vector3s.Add(new Vector3(0, 2, 0));
        vector3s.Add(new Vector3(2, 0, 0));
        vector3s.Add(new Vector3(2, 2, 0));

        Vector3 v3 = new Vector3(1, 1, 0);


        print("boundtest" + isInsidePolygon(vector3s, v3));
    }

    public void SetBounds()
    {
        
    }

    bool checkDotInLine(Vector3 a, Vector3 b, Vector3 dot)
    {
        float epsilon = 0.00001f;
        float dAB = Vector3.Distance(a, b);
        float dADot = Vector3.Distance(a, dot);
        float dBDot = Vector3.Distance(b, dot);

        return ((dAB + epsilon) >= (dADot + dBDot));
    }

    bool crossCheck2D(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
    {
        // (x, 0, z)
        float x1, x2, x3, x4, z1, z2, z3, z4, X, Z;

        x1 = a.x; z1 = a.z;
        x2 = b.x; z2 = b.z;
        x3 = c.x; z3 = c.z;
        x4 = d.x; z4 = d.z;

        float cross = ((x1 - x2) * (z3 - z4) - (z1 - z2) * (x3 - x4));

        if (cross == 0 /* parallel */) return false;

        X = ((x1 * z2 - z1 * x2) * (x3 - x4) - (x1 - x2) * (x3 * z4 - z3 * x4)) / cross;
        Z = ((x1 * z2 - z1 * x2) * (z3 - z4) - (z1 - z2) * (x3 * z4 - z3 * x4)) / cross;

        return
            checkDotInLine(a, b, new Vector3(X, 0, Z))
            && checkDotInLine(c, d, new Vector3(X, 0, Z));
    }

    bool isInsidePolygon(List<Vector3> polygonPos, Vector3 dotPos)
    {
        Vector3 outsidePos = dotPos;
        outsidePos.x += 100.0f;

        int count = polygonPos.Count;
        int crossCount = 0;
        for (int i = 0; i < count - 1; i++)
        {
            if (crossCheck2D(polygonPos[i], polygonPos[i + 1], dotPos, outsidePos))
            {
                crossCount++;
            }
        }

        if (crossCheck2D(polygonPos[0], polygonPos[count - 1], dotPos, outsidePos))
            crossCount++;

        return (crossCount % 2) == 1; // 홀수면 inside
    }


    //[Serializable]
    //public class BoundaryArray
    //{
    //    public Boundary[] boundaries;
    //}

    //[Serializable]
    //public class Boundary
    //{
    //    public string name;
    //    public Vector2[] polygon;
    //}

    [Serializable]
    public class Bounds
    {
        public List<bound> bounds;
    }

    [Serializable]
    public class bound
    {
        public string name;
        public string flawPrtbGrpCd;
        public string detailName;
        public string flawPrtbCd;
        public List<Vector2> vertices = new List<Vector2>();
    }

    public class RoomPositionInfo
    {
        public string name;
        public string flawPrtbGrpCd;
        public string detailName;
        public string flawPrtbCd;
    }

    public void SetInverse()
    {
        inverse = -1;
    }

    [Serializable]
    public class PanoramaViews
    {
        public PanoramaView[] panoramaViews;
    }

    [Serializable]
    public class PanoramaView
    {
        public string tag;
        public Vector3 position;
        public Vector3 rotation;
    }

    //public class RoomPositionInf
}
