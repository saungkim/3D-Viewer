using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavvisModel : MonoBehaviour
{
    Renderer renderer;
    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();
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
             renderer.enabled = !renderer.enabled;
    }

}
