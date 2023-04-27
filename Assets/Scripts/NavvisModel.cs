using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavvisModel : MonoBehaviour
{
    Renderer navvisRenderer;
    // Start is called before the first frame update
    void Start()
    {
        navvisRenderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            NavvisModelOnOff(); 
        }
    }

    public void NavvisModelOnOff()
    {
        // gameObject.SetActive(!gameObject.activeSelf);
       // GetComponent<Renderer>().enabled =
             navvisRenderer.enabled = !navvisRenderer.enabled;
    }

}
