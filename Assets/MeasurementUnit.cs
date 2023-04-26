using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
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

    private GameObject prevDot;
    private MeasurementLine prevLine;

    private bool selected = false;
    // Start is called before the first frame update 


    void Start()
    {
        //AddDot(new Vector3(0, 0, 0));
        //AddDot(new Vector3(0, 1, 0));

        //InsertDot(1, new Vector3(0.5f, 0.5f, 0));

        //FixDot(1, new Vector3(0, 1, 0));
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

   

    public void AddDot(Vector3 position,Vector3 rot)
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
            o.transform.SetParent(transform);
        }

        dots.Add(o);
        o.transform.position = position;
        o.transform.eulerAngles = rot;

        if(prevLine != null)
        {
            prevLine.SetLineEndPosition(position);
        }
      
        AddLine(position, position);
        

    }

    public void InsertDot(int index , Vector3 position)
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
            o.transform.SetParent(transform);
        }

        dots.Insert(index, o);
        o.transform.position = position;

        InsertLine(position,index);


       // AddLine();
    }

    public void AddLine(Vector3 startPos, Vector3 endPos )
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
        o.GetComponent<MeasurementLine>().SetLinePositions(startPos,endPos);

        prevLine = o.GetComponent<MeasurementLine>();
    }

    public void InsertLine(Vector3 position , int index)
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

        lines[index - 1].GetComponent<MeasurementLine>().SetLineEndPosition(position);
        o.GetComponent<MeasurementLine>().SetLinePositions(position, dots[index + 1].transform.position);
        lines.Insert(index,o);
       
    }

    public void FixDot(int index , Vector3 position , Vector3 rot)
    {
        dots[index].transform.position = position;
        dots[index].transform.eulerAngles = rot;

        if (index == 0)
        {
            lines[index].GetComponent<MeasurementLine>().SetLineStartPosition(position);
        }
        else
        {
            lines[index-1].GetComponent<MeasurementLine>().SetLineEndPosition(position);

            if(lines.Count > index)
            lines[index].GetComponent<MeasurementLine>().SetLineStartPosition(position);
        }

      
       
    }

    public MeasurementLine GetPrevLine()
    {
        return prevLine;
    }

    public void Select(bool inputSelected)
    {
        print("Select");

        selected = inputSelected;

        int childCount = transform.childCount;

        for(int i = 0; i < childCount; ++i)
        {
            transform.GetChild(i).GetComponent<MeasurementObject>().Select(selected);
        }
    }

    public bool GetSelect()
    {
        return selected;
    }

    //public void 

    public void CompleteUnit()
    {
        int childCount = transform.childCount;

        for(int i = 0; i < childCount; ++i)
        {
            //transform.GetChild(i)
            transform.GetChild(i).GetComponent<MeasurementObject>().SetCollider();
        }

        //Remove Last Line
        Destroy(lines[lines.Count - 1]);
        lines.RemoveAt(lines.Count - 1);
    }

    public void SetCollider()
    {
        int childCount = transform.childCount;

        for (int i = 0; i < childCount; ++i)
        {
            //transform.GetChild(i)
            transform.GetChild(i).GetComponent<MeasurementObject>().SetCollider();
        }
    }

    public int FindDotIndex(GameObject dot)
    {
        int index = 0;

        foreach(GameObject obj in dots)
        {
            if(dot == obj)
            {
                break;
            }
            ++index;
        }

        return index;
    }

}
