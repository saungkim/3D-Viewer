using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;

public class Construction : MonoBehaviour
{
    public Transform construction;

    public GameObject activator;

    public List<Texture> textures;
    [SerializeField] private GameObject cubemap;


    Material material;

    // Start is called before the first frame update
    void Start()
    {
        Init();
        SetCubeMapTexture();
       
    }

    // Update is called once per frame
    void Update()
    {
     
    }

   

    private void SetCubeMapTexture()
    {
        int childCount = construction.childCount - 1;

        GameObject o;

        for (int i = 0; i < childCount; ++i)
        {
            o = Instantiate(cubemap);
          
            o.transform.parent = construction.GetChild(i);
            o.transform.localPosition = Vector3.zero;
            o.transform.localScale = new Vector3(-100, 100, 100);
            o.transform.localRotation = Quaternion.Euler(-90,0,0);

            Material m = Instantiate(material);
            o.GetComponent<Renderer>().sharedMaterial = m;
            o.GetComponent<Renderer>().material.SetTexture("_BaseMap", textures[i]);

        }
    }

    private void Init()
    {
        construction.GetChild(0).gameObject.SetActive(true);
        material = cubemap.GetComponent<Renderer>().sharedMaterial;
    }

 
}
