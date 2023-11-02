using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static UnityEngine.GraphicsBuffer;
using Unity.VisualScripting;

public class DefectDot : MonoBehaviour
{
    [SerializeField] private NetworkManager networkManager;
    [SerializeField] private PlayerMovement playerMovement;

    private Vector3 initScale;
    public float minDistance = 2f; // �ּ� �Ÿ�
    public float maxDistance = 10f; // �ִ� �Ÿ�
    public float minSize = 1f; // �ּ� ũ��
    public float maxSize = 5f; // �ִ� ũ��

    [SerializeField] private Material material;
    // Start is called before the first frame update
    void Start()
    {
        initScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        CheckCameraOnView();

        Vector2 screenPosition = Camera.main.WorldToScreenPoint(transform.position);

        Vector2 scaler = Vector2.one - new Vector2(Mathf.Abs(screenPosition.x - Screen.width / 2) / Screen.width, Mathf.Abs(screenPosition.y - Screen.height / 2) / Screen.height);

        if(Mathf.Abs(scaler.x) > 1 || Mathf.Abs(scaler.y) > 1)
        {
            //print("Scaler Worng" + scaler.magnitude);
            return;
        }

        float fov = Camera.main.fieldOfView;
        if(fov > 100)
        {
            fov = 100;
        }
            
        transform.localScale = initScale * Vector3.Distance(Camera.main.transform.position, transform.position) * Mathf.Tan(fov * 0.5f * Mathf.Deg2Rad)
            * scaler;    
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

        material.renderQueue = 2001;

        if (Physics.Raycast(transform.position, direction, out hit, Vector3.Distance(Camera.main.transform.position, transform.position)))
        {
            if(hit.transform.tag != "Defect")
            {
                material.renderQueue = 3000;

               // print("Detected Wall");
            }
        }
    }
   
}
