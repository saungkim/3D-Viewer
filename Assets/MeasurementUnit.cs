using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class MeasurementUnit : MonoBehaviour
{
    [SerializeField] List<Vector3> dotPositions;
    [SerializeField] private GameObject line;
    [SerializeField] private GameObject dot;
    [SerializeField] List<GameObject> dots = new List<GameObject>();
    [SerializeField] List<GameObject> lines = new List<GameObject>();

    static List<GameObject> poolDots = new List<GameObject>();
    static List<GameObject> poolLines = new List<GameObject>();
    [SerializeField] private Transform pool;
  
    // Start is called before the first frame update 
    void Start()
    {
        AddDot(new Vector3(0, 0, 0));

    }

    // Update is called once per frame
    void Update()
    {
       //if(dotPositions.Count != dots.Count)
       //{
       //     if(dotPositions.Count == 0)
       //     {
       //         Destroy(gameObject);

       //         return;
       //     }
       //     else if (dotPositions.Count > dots.Count)
       //     {
       //         for(int i = dots.Count; i < dotPositions.Count; ++i)
       //         {
       //             CreateDot();
       //             CreateLine();
       //         }
       //     }
       //     else
       //     {
       //         for(int i = dotPositions.Count; i < dots.Count; ++i)
       //         {
                    
       //         }
       //     }
            

       //}
       //else
       //{
       //     for(int i = 0; i < dots.Count; ++i)
       //     {
       //         if (dotPositions[i] != dots[i].transform.position)
       //         {
       //             dotPositions[i] = dots[i].transform.position;
       //         }
       //     }
       //}
    }

    private void RemoveDot(int index)
    {

    }

    private void AddDot(Vector3 position)
    {
        GameObject o = null;

        if(poolDots.Count > 0)
        {
            o = poolDots[poolDots.Count];
            poolDots.RemoveAt(poolDots.Count);

        }
        else
        {
            o = Instantiate(dot) as GameObject;
            o.transform.parent = transform;
        }

        dots.Add(o);
        o.transform.position = position;

        AddLine(position);
    }

    private void InsertDot(int index , Vector3 position)
    {
        GameObject o = null;

        if (poolDots.Count > 0)
        {
            o = poolDots[poolDots.Count];
            poolDots.RemoveAt(poolDots.Count);
        }
        else
        {
            o = Instantiate(dot) as GameObject;
            o.transform.parent = transform;
        }

        dots.Insert(index, o);
        o.transform.position = position;

        InsertLine(position,index);


       // AddLine();
    }

    private void AddLine(Vector3 position )
    {
        GameObject o = null;

        if (poolLines.Count > 0)
        {
            o = poolLines[poolLines.Count];
            poolLines.RemoveAt(poolLines.Count);
        }
        else
        {
            o = Instantiate(line) as GameObject;
            o.transform.parent = transform;
        }

        lines.Add(o);
        o.GetComponent<MeasurementLine>().SetLinePositions(position,position);

    }

    private void InsertLine(Vector3 position , int index)
    {
        GameObject o = null;

        if (poolLines.Count > 0)
        {
            o = poolLines[poolLines.Count];
            poolLines.RemoveAt(poolLines.Count);
        }
        else
        {
            o = Instantiate(line) as GameObject;
            o.transform.parent = transform;
        }

        lines.Add(o);
        o.GetComponent<MeasurementLine>().SetLinePositions(position, dots[index + 1].transform.position);

    }


}
