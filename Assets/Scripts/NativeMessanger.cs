using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class NativeMessanger : MonoBehaviour
{
    [SerializeField] private Constructor constructor;
    [SerializeField] private DefectConstructor defectConstructor;

    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private InputSystem inputSystem;

    enum LoadState { None, Loading, Done }
    LoadState readEnvState = LoadState.None;
    LoadState readDefectState = LoadState.None;

    // Start is called before the first frame update
    void Start()
    {
#if UNITY_EDITOR
        ReadEnv("input");
        ReadDefect("input");
        ViewDefect("NeedToChange3");
        //ViewStage("19");
#endif
   
    }

    public void EnableDotCreateMode()
    {
        inputSystem.SetEnableDot();
    }

    public void ReadEnv(string fileName)
    {
        if (readEnvState == LoadState.Done)
        {
            ReadEnvCallBack("");
            return;
        }

        readEnvState = LoadState.Loading;

#if UNITY_EDITOR
        fileName = Application.dataPath + "/Sources/" + fileName + ".env";
#endif
      
        constructor.FileOpen(fileName , ReadEnvCallBack);
    }

    public void ReadEnvCallBack(string message)
    {
        if(message == "Success")
        {
            readEnvState = LoadState.Done;
            return;
        }
        readEnvState = LoadState.None;         
    }

    public void ReadDefect(string filePath)
    {
#if UNITY_EDITOR
        filePath = Application.dataPath + "/Sources/" + filePath;
        filePath += ".json";
#endif
        if (readDefectState == LoadState.Done)
        {
            ReadEnvCallBack("");
            return;
        }
        readDefectState = LoadState.Loading;
        StartCoroutine(defectConstructor.ReadAllDefect(filePath , ReadDefectCallBack));
    }

    public void ReadDefectCallBack(string message)
    {
        if (message == "Succcess")
        {
            readEnvState = LoadState.Done;       
            return;
        }
        readEnvState = LoadState.None;
    }

    public void ViewDefect(string defectId)
    {
        StartCoroutine(defectConstructor.MoveCamInstant(defectId));
    }

    public void ViewStage(string rStage)
    {
        int stage = int.Parse(rStage);
        StartCoroutine(constructor.InitStage(stage));
    }

    public void NativeSendMessage(string message)
    { 
#if UNITY_ANDROID
        try
        {
            AndroidJavaClass jc = new AndroidJavaClass("com.parallel.viewer3d.RoomViewerActivity");
            AndroidJavaObject overrideActivity = jc.GetStatic<AndroidJavaObject>("instance");
            overrideActivity.Call("RoomViewerReceiveMessage", message);
        }
        catch (Exception e)
        {
            //appendToText("Exception during showHostMainWindow");
            //appendToText(e.Message);
        }
#elif UNITY_IOS || UNITY_TVOS
        NativeAPI.showHostMainWindow(lastStringColor);
#elif UNITY_EDITOR
     
#endif
        print(message);
    }
}
