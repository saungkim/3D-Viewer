using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NativeMessanger : MonoBehaviour
{
    [SerializeField] private Constructor constructor;
    [SerializeField] private DefectConstructor defectConstructor;

    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private InputSystem inputSystem;
    // Start is called before the first frame update
    void Start()
    {
#if UNITY_EDITOR
        ReadEnv("input");
        ReadDefect("input");
        //ViewDefect("NeedToChange3");
        ViewStage(0);
        //EnableDotCreateMode();
#endif
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnableDotCreateMode()
    {
        inputSystem.SetEnableDot();
    }

    public void ReadEnv(string fileName)
    {
        //string url = Application.streamingAssetsPath + "/" + fileName + ".env";

#if UNITY_EDITOR
        fileName = Application.dataPath + "/Sources/" + fileName + ".env";
#endif

        constructor.FileOpen(fileName);
    }

    public void ReadDefect(string filePath)
    {

#if UNITY_EDITOR
        filePath = Application.dataPath + "/Sources/" + filePath;
        filePath += ".json";
#endif

        defectConstructor.ReadAllDefect(filePath);
    }

    public void ViewDefect(string defectId)
    {
        StartCoroutine(defectConstructor.MoveCamInstant(defectId));
    }

    public void ViewStage(int stage)
    {
        StartCoroutine(constructor.InitStage(stage));
    }

    public void AndroidSendMessage(string message)
    {
        print("AndroidSendMessage :" + message);
#if UNITY_ANDROID
        try
        {
            AndroidJavaClass jc = new AndroidJavaClass("com.parallel.viewer3d.RoomViewerActivity");
            AndroidJavaObject overrideActivity = jc.GetStatic<AndroidJavaObject>("instance");
            overrideActivity.Call("UnityReceiveMessage", message);
        }
        catch (Exception e)
        {
            //appendToText("Exception during showHostMainWindow");
            //appendToText(e.Message);
        }
#elif UNITY_IOS || UNITY_TVOS
        NativeAPI.showHostMainWindow(lastStringColor);
#endif
    }




}
