using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMap : MonoBehaviour
{
    [SerializeField] private MeshRenderer[] planes;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMaterial(Material material)
    {
        foreach (MeshRenderer renderer in planes) 
        {
            renderer.sharedMaterial = material;
        }
    }
}
