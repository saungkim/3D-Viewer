using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class RenderQueueOnCamera : MonoBehaviour
{
    [SerializeField] Material[] materials;


    Image image;
    MeshRenderer meshRenderer;

    int wallLayer;
    int defaultLayer;
    int initLayer;

    GameObject canvas;

    [SerializeField] private TextMeshPro tmp;
    // Start is called before the first frame update
    void Start()
    {
        materials = new Material[2];

        materials[0] = GetComponent<Material>();
        //materials[1] = transform.parent.GetComponent<Material>();

        print("Text Mesh Pro RenderQueue" + tmp.material.renderQueue);

        tmp.fontSharedMaterial = Instantiate(tmp.fontSharedMaterial);
//        image = transform.GetChild(0).GetComponent<Image>();
 //       meshRenderer = transform.GetChild(0).GetComponent<MeshRenderer>();

        //if (materials[0] == null && meshRenderer != null)
        //    materials[0] = meshRenderer.sharedMaterial;

        //if (materials[0] == null && image != null)
        //    materials[0] = image.material;

        //if (materials[0] == null)
        //    print("RenderQueueOnCamera is Null");

        image = transform.parent.GetComponent<Image>();
        image.material = Instantiate(image.material);
        meshRenderer = transform.parent.GetComponent<MeshRenderer>();

        if (materials[1] == null && meshRenderer != null)
            materials[1] = meshRenderer.sharedMaterial;

        if (materials[1] == null && image != null)
            materials[1] = image.material;

        if (materials[1] == null)
            print("RenderQueueOnCamera is Null");

        wallLayer = LayerMask.GetMask("Construction");
        defaultLayer = LayerMask.GetMask("Default");
        initLayer = LayerMask.GetMask("UIOnTop");

        canvas = transform.parent.parent.gameObject;

        tmp = GetComponent<TextMeshPro>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckCameraOnView();
    }

    public void CheckCameraOnView()
    {
        Vector3 direction = transform.position - Camera.main.transform.position ;
        RaycastHit hit;

      
            materials[1].renderQueue = 2001;

            tmp.material.renderQueue = 2001;
        tmp.fontSharedMaterial.renderQueue = 2001;
        print("Text Mesh Pro RenderQueue" + tmp.material.renderQueue);
        canvas.layer = 6;
            transform.parent.gameObject.layer = 6;
            gameObject.layer = 6;
        
        if (Physics.Raycast(Camera.main.transform.position, direction, out hit, Vector3.Distance(transform.position,Camera.main.transform.position) * 0.9f, wallLayer))
        {
 
                materials[1].renderQueue = 3001;

            tmp.material.renderQueue = 3001;

            tmp.fontSharedMaterial.renderQueue = 3001;
            print("Text Mesh Pro RenderQueue" + tmp.material.renderQueue);
            canvas.layer = 10;
                transform.parent.gameObject.layer = 10;
                gameObject.layer =  10;
     
        }
        else
        {
           
        }
    }
}
