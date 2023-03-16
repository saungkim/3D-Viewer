using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;

public class LoadTextureFromStreamingAsset : MonoBehaviour
{
    [SerializeField] private Constructor constructor;
    Material[] materials;
    Texture2D[] textures;
    int parentIndex;
    int childCubeCount;
    // Start is called before the first frame update
    void Awake()
    {

        //bytes[] pngBytes = System.IO.File.ReadAllBytes(path);

        parentIndex = transform.GetSiblingIndex();

        Transform childCube = transform.GetChild(0);

        childCubeCount = transform.GetChild(0).childCount;

        materials = new Material[childCubeCount];
        textures = new Texture2D[childCubeCount];

        for (int i = 0; i < childCubeCount; ++i)
        {
            materials[i] = childCube.GetChild(i).GetComponent<MeshRenderer>().material;
        }

      

        //materials =
    }

    // Update is called once per frame
    void Update()
    {
     
    }
    private void OnEnable()
    {
        print("OnEnable");
        //StartCoroutine(SetAllTexture());
        SetAllTexture();
    }

    public void DestroyTex()
    {
        print("OnDisbale");

        foreach(Texture2D tex in textures)
        {
            DestroyImmediate(tex);
        }
    }

   

    public void SetAllTexture()
    {
    
        for (int i = 0; i < childCubeCount; ++i)
        {
            //yield return StartCoroutine(SetTextureFromStreamingAsset(parentIndex, i));
            SetTextureFromMemory(parentIndex,i);
        }

        for (int i = 0; i < childCubeCount; ++i)
        {
            transform.GetChild(0).GetChild(i).gameObject.SetActive(true);

            print("aCTIVATED");
        }
    }

    public IEnumerator SetTextureFromStreamingAsset(int parentIndex, int childIndex)
    {
        string url = Path.Combine(Application.streamingAssetsPath +"/" + parentIndex.ToString() + "_" + childIndex.ToString() +".jpg");
        byte[] imgData = null;
        Texture2D tex = new Texture2D(2, 2);

        textures[childIndex] = tex;
        //imgData = File.ReadAllBytes(url);
        WWW reader = new WWW(url);
        while (!reader.isDone)
        {
            yield return null;
        }

        imgData = reader.bytes;
        tex.wrapMode = TextureWrapMode.Clamp;
        tex.LoadImage(imgData);

        materials[childIndex].SetTexture("_MainTex",tex);
    }

    public void SetTextureFromMemory(int parentIndex, int childIndex)
    {

        byte[] imgData = constructor.GetTexture(parentIndex * 6 + childIndex);
        Texture2D tex = new Texture2D(2, 2);
        
        textures[childIndex] = tex;
   
        tex.wrapMode = TextureWrapMode.Clamp;
        tex.LoadImage(imgData);

        materials[childIndex].SetTexture("_MainTex", tex);

        
    }


    //IEnumerator GetFile()
    //{

    //}
}
