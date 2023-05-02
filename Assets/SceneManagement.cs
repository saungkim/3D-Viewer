using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManagement : MonoBehaviour
{
    private string messageFilePath;
    [SerializeField] private NativeMessanger nativeMessanger;
    // Start is called before the first frame update
    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("SceneManager");

        if (objs.Length > 1)
        {

            foreach(GameObject o in objs)
            {
                if(o != gameObject)
                {
                    nativeMessanger.ReadRoomViewerFile(o.GetComponent<SceneManagement>().GetMessageFilePath());
                    break;
                }
            }

            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string GetMessageFilePath()
    {
        return messageFilePath;
    }

    public void SetMessageFilePath(string path)
    {
        messageFilePath = path;
    }
}
