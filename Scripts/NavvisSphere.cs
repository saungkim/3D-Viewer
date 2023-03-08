using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class NavvisSphere : MonoBehaviour
{
    // [SerializeField] private Transform InputModel;
    [Header("Texture")]
    [SerializeField] private List<Texture> navvisTextures;
    [SerializeField] private List<Texture> ricoThetaTextures;
    [Header("Basic Mesh")]
    [SerializeField] private Material baseMaterial;
    [SerializeField] private GameObject sphere;

    // Start is called before the first frame update
    void Awake()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Init()
    {
        int childCount = transform.childCount - 1;
        GameObject o;

        for (int i = 0; i < childCount; ++i)
        {
            //Navvis
            o = Instantiate(sphere, transform.GetChild(i));
            o.GetComponent<Renderer>().sharedMaterial = Instantiate(baseMaterial);
            o.GetComponent<Renderer>().sharedMaterial.SetTexture("_BaseMap", navvisTextures[i]);

            o.transform.localPosition = Vector3.zero;
            o.transform.localEulerAngles = new Vector3(-90, 0, 0);
            o.transform.localScale = new Vector3(-30, 30, 30);

            o.layer = LayerMask.NameToLayer("Navvis");

            o.SetActive(false);

            //Rico
            o = Instantiate(sphere, transform.GetChild(i));
            o.GetComponent<Renderer>().sharedMaterial = Instantiate(baseMaterial);
            o.GetComponent<Renderer>().sharedMaterial.SetTexture("_BaseMap", ricoThetaTextures[i]);

            o.transform.localPosition = Vector3.zero;

            float y = o.transform.parent.localEulerAngles.y;

            y = ( (int)(y / 90) + 1) * 90 - y;
            y = y * -1;
            print("Y" + y);
            //if( y < -90)
            // {
            //    print("-90:" + o.transform.parent.name);
            //    y = y + 90 ;
            //}
            //else
            //{
            //    print("-90 Else :" + o.transform.parent.name + y);
            //    y = 0;
            //}
                        
            o.transform.localEulerAngles = new Vector3(-90, 0, 0) - new Vector3(o.transform.parent.localEulerAngles.x, y , o.transform.parent.localEulerAngles.z);
           // o.transform.localEulerAngles = new Vector3(-90, 0, 0);
            //o.transform.eulerAngles -= new Vector3(o.transform.parent.localEulerAngles.x ,0 , o.transform.parent.localEulerAngles.z);
            o.transform.localScale = new Vector3(-30, 30, 30);

            o.layer = LayerMask.NameToLayer("Rico");

            o.SetActive(false);

            //transform.GetChild(i)
        }
    }
}
