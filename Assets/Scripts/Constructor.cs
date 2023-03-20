using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using TMPro;
using UnityEngine.AI;
using Unity.VisualScripting;

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

    List<byte[]> textures;
    // Start is called before the first frame update


   
    void Start()
    {
#if UNITY_EDITOR
        //string url = Application.dataPath + "/Sources/";
        //StartCoroutine(FileLoad(url));
#endif
    }
    bool isLoaded = false;
    public void FileOpen(string url)
    {
        //if (isLoaded)
        //{
        //    int camGroupChildCount = camGroup.childCount;

        //    for(int i = 0; i < camGroupChildCount; ++i)
        //    {
        //        if (camGroup.GetChild(i).gameObject.activeSelf)
        //        {
        //            camGroup.GetChild(i).GetComponent<LoadTextureFromStreamingAsset>().SetAllTexture();

        //            print("isLoaded True:" + camGroup.GetChild(i).GetSiblingIndex());
        //        }
        //    }

        //    return;
        //}
        //isLoaded = true;
  

        StartCoroutine(FileLoad(url));
    }

    IEnumerator FileLoad(string url)
    {
       
        byte[] envData = null;
       
        WWW reader = new WWW(url);
        while (!reader.isDone)
        {
            yield return null;
        }

        envData = reader.bytes;

        envToModel(envData);
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
    private void envToModel(byte[] content)
    {  
        int index = 4;
        int[] verticeLengths = new int[content[2]];
        int iterCount = content[2] * 4;
        for (int i = 0; i < iterCount; i = i + 4)
        {
            verticeLengths[i / 4] = BitConverter.ToInt32(content, index + i);
        }

        index += iterCount;
        int[] indiciesLengths = new int[content[2]];
        iterCount = content[2] * 4;

        for (int i = 0; i < iterCount; i = i + 4)
        {
            indiciesLengths[i / 4] = BitConverter.ToInt32(content, index + i);
        }

        index += iterCount;
        int[] textureLengths = new int[content[3]];
        iterCount = content[3] * 4;
        for (int i = 0; i < iterCount; i = i + 4)
        {
            textureLengths[i / 4] = BitConverter.ToInt32(content, index + i);
        }

        index += iterCount;
        Vector3[] panoramaPosition = new Vector3[content[1]];
        Vector3[] panoramaRotation = new Vector3[content[1]];
        iterCount = content[1] * 12;
        for (int i = 0; i < iterCount; i = i + 12)
        {
            panoramaPosition[i / 12] = new Vector3(BitConverter.ToSingle(content, index + i), BitConverter.ToSingle(content, index + i + 4), BitConverter.ToSingle(content, index + i + 8));
        }

        index += iterCount;
        iterCount = content[1] * 12;
        for (int i = 0; i < iterCount; i = i + 12)
        {
            panoramaRotation[i / 12] = new Vector3(BitConverter.ToSingle(content, index + i), BitConverter.ToSingle(content, index + i + 4), BitConverter.ToSingle(content, index + i + 8));
        }

        textures = new List<byte[]>();
        List<byte[]> indiciesList = new List<byte[]>();
        List<byte[]> verticiesList = new List<byte[]>();

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

        List<Vector3[]> verticies = new List<Vector3[]>();
        List<int[]> indicies = new List<int[]>();

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
       
        for (int i = 0; i < content[2]; ++i)
        {
            GameObject o = new GameObject();
            Mesh mesh = new Mesh();
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
       
        for(int i = 0; i < content[1]; ++i)
        {
            GameObject o = Instantiate(cubeMap);
            o.transform.parent = camGroup;
            o.transform.position = panoramaPosition[i];
            o.transform.localEulerAngles = panoramaRotation[i];

            o.GetComponent<CubeMap>().SetMaterial(Instantiate(cubeMapMaterial));


        }
        playerMovement.Init();
        playerMovement.InitStage(0);
        inputSystem.enabled = true;
        isLoadDone = true;


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
    }

    public bool GetIsLoadDone()
    {
        return isLoadDone;
    }
 
}
