using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatterPortConstruction : MonoBehaviour
{
    [SerializeField] private Transform camGroup;
    [SerializeField] private List<Texture> texutres;
    [SerializeField] private GameObject cubeMap;

    [SerializeField] private Material cubeMapMaterial;
    // Start is called before the first frame update
    void Start()
    {
        cubeMapMaterial = cubeMap.transform.GetChild(0).GetComponent<Renderer>().sharedMaterial;


        int childCount = camGroup.childCount;

        int count = 0;

        for(int i = 0; i < childCount; ++i){

            GameObject o = Instantiate(cubeMap);
            o.transform.parent = camGroup.transform.GetChild(i);
            camGroup.GetChild(i).gameObject.SetActive(false);

            o.transform.localPosition = Vector3.zero;
            o.transform.localRotation = Quaternion.identity;

            for (int  j = 0; j < 6; ++j)
            {
                o.transform.GetChild(j).GetComponent<Renderer>().sharedMaterial = Instantiate(cubeMapMaterial);
                o.transform.GetChild(j).GetComponent<Renderer>().sharedMaterial.SetTexture("_BaseMap", texutres[count]);


                ++count;
            }
        
        }

        camGroup.GetChild(0).gameObject.SetActive(true);
        cubeMap.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
