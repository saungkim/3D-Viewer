using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cunstructor : MonoBehaviour
{
    [SerializeField] private NetworkManager networkManager;
    [SerializeField] private Transform camGroup;
    [SerializeField] private GameObject dot;
    // Start is called before the first frame update
    void Start()
    {
        LoadDefects();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LoadDefects()
    {
        Action<Vector3[] , int[]> get = (Vector3[] pos , int[] ids) => {
            //foreach (Vector3 b in a)
            //{
            //    print(b);
            //}

            //foreach(int id in ids)
            //{
            //    camGroup[id]

            //    Ray ray = new Ray(pos[])
            //}

            int idCount = ids.Length;

            for(int i = 0; i < idCount; ++i)
            {
                Vector3 panPosition = camGroup.transform.GetChild(i).position;
                Ray RAY = new Ray(panPosition, pos[i] - panPosition );
                RaycastHit hit;
                if(Physics.Raycast(RAY,out hit))
                {
                    GameObject o = Instantiate(dot, pos[i], Quaternion.identity);
                    o.SetActive(true);
                    o.transform.LookAt(hit.normal);
                }
            }

        };
        StartCoroutine(networkManager.GetResidentsDefectsPositions(get));
        //networkManager.GetResidentsDefectsPositions();
    }
}
