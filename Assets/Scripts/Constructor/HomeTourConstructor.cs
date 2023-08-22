using UnityEngine;
using System.Collections;

public class HomeTourConstructor : MonoBehaviour
{

    Root root;

	// Use this for initialization
	void Start()
	{
       
	}

	// Update is called once per frame
	void Update()
	{
			
	}

    public void InitHomeTourData(string json)
    {
        root = JsonUtility.FromJson<Root>(json);
    }

    public Root GetRoot()
    {
        return root;
    }

    //public string GetStringa()
    //{
    //    //return "d";
    //}

    [System.Serializable]
    public class Root
    {
        public InitHomePosition initHomePosition;
    }

    [System.Serializable]
    public class InitHomePosition
    {
        public Vector3 position;
        public Vector3 rotation;
    }


}

