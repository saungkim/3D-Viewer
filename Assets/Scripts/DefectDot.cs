using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static UnityEngine.GraphicsBuffer;

public class DefectDot : MonoBehaviour
{
    [SerializeField] private NetworkManager networkManager;
    [SerializeField] private PlayerMovement playerMovement;

    private Vector3 initScale;
    public float minDistance = 2f; // 최소 거리
    public float maxDistance = 10f; // 최대 거리
    public float minSize = 1f; // 최소 크기
    public float maxSize = 5f; // 최대 크기

    [SerializeField] private Material material;
    // Start is called before the first frame update
    void Start()
    {
        initScale = transform.localScale;
        //material = GetComponent<Material>();
    }

    // Update is called once per frame
    void Update()
    {

        CheckCameraOnView();
        //print( "" + Camera.main.transform.position);

        //if(initScale)

        // transform.localScale = initScale * (Camera.main.transform.position - transform.position).magnitude ;
        //(Camera.main.transform.position -  transform.position);

        //float distance = Vector3.Distance(transform.position, Camera.main.transform.position);

        // 거리에 따라 크기를 보간(interpolate)하여 계산
        //float normalizedDistance = Mathf.Clamp01((distance - minDistance) / (maxDistance - minDistance));
        //float targetSize = Mathf.Lerp(minSize, maxSize, normalizedDistance);


        // 오브젝트의 스케일 조정
        //transform.localScale = initScale * targetSize;
    }

    public void CreateToServer(Vector3 pos, Vector3 rot)
    {
        networkManager.SetResidentsDefect(pos, rot, Camera.main.fieldOfView, Camera.main.transform.position, Camera.main.transform.eulerAngles);
    }
    Ray ray;
    RaycastHit hit;
    public void CheckCameraOnView()
    {
        Vector3 direction = Camera.main.transform.position - transform.position;
        RaycastHit hit;

        material.renderQueue = 2000;

        if (Physics.Raycast(transform.position, direction, out hit, Vector3.Distance(Camera.main.transform.position, transform.position)))
        {
            if(hit.transform.tag != "Defect")
            {
                material.renderQueue = 3000;

                print("Detected Wall");
            }
        }
    }
   
}
