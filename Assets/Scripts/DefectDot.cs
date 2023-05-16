using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class DefectDot : MonoBehaviour
{
    [SerializeField] private NetworkManager networkManager;
    [SerializeField] private PlayerMovement playerMovement;
    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void CreateToServer(Vector3 pos , Vector3 rot)
    {
        networkManager.SetResidentsDefect(pos, rot, Camera.main.fieldOfView,Camera.main.transform.position , Camera.main.transform.eulerAngles);
    }

   
}
