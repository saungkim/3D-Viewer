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
    [SerializeField] private OverallSetting overallSetting;
    [SerializeField] private CameraController camController;
    //[SerializeField] private 
    
    enum LoadState { None, Loading, Done }
    LoadState readEnvState = LoadState.None;
    LoadState readDefectState = LoadState.None;

    // Start is called before the first frame update
    void Start()
    {
        Action<string> nativeErrorMessanger = (string value) => { nativeSendErrorMessage(value);};

#if UNITY_EDITOR
        string fileName = "input";
        fileName = Application.dataPath + "/Sources/" + fileName + ".env";
        //ReadEnv("input");
        //ReadDefect("input");
        //ViewStage("19");
        //ViewDefect("NeedToChange3");
#endif
        ReadEnv(Application.streamingAssetsPath + "/input.env");
        ReadDefect(Application.streamingAssetsPath + "/input.json");
        ViewStage("19");
        EnableDotCreateMode();
        SetCursor("false");
        SetMovePointsVisible("true");
        //SetZoomInitWhenMove("true");
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


        constructor.FileOpen(fileName, ReadEnvCallBack);
    }

    public void ReadEnvCallBack(string message)
    {
        if (message == "Success")
        {
            readEnvState = LoadState.Done;
            return;
        }
        readEnvState = LoadState.None;
    }

    public void ReadDefect(string filePath)
    {
//#if UNITY_EDITOR
//        filePath = Application.dataPath + "/Sources/" + filePath;
//        filePath += ".json";
//#endif
        if (readDefectState == LoadState.Done)
        {
            ReadEnvCallBack("");
            return;
        }
        readDefectState = LoadState.Loading;
        StartCoroutine(defectConstructor.ReadAllDefect(filePath, ReadDefectCallBack));
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

    public void DestoryDefect(string value)
    {

        defectConstructor.DestroyDefect(value , nativeSendErrorMessage);
    }

    public void SetColorDefect(string value)
    {
        string[] values = value.Split(',');
        string htmlColor = values[1]; // "#FF0000"; Red
        string id = values[0];

        Color color;

        if (ColorUtility.TryParseHtmlString(htmlColor, out color))
        {
            // Color 값을 사용합니다. 
        }
        else
        {
            nativeSendErrorMessage("SetColorDefect Error :" + htmlColor + " "+ "ParseHtmlString Failed");
        }
    }

    public void CreateTag(string id)
    {

    }

    public void SetMoveTime()
    {

    }

    public void SetCameraSensitivity(string value)
    {
        string[] values = value.Split(',');

        float dragSpeed;
        float lerpSpeed;

        if (float.TryParse(values[0],out dragSpeed))
        {
            camController.dragSpeed = dragSpeed;
        }
        else
        {
            nativeSendErrorMessage("SetCameraSensitivity :" + values[0] + " " + "Parse Failed");
        }

        if (float.TryParse(values[1],out lerpSpeed))
        {
            camController.lerpSpeed = lerpSpeed;
        }
        else
        {
           nativeSendErrorMessage("SetCameraSensitivity :" + values[1] +" " + "Parse Failed");
        }
    }

    public void SetCameraFov(string value)
    {
        float fov;

        if(float.TryParse(value,out fov))
        {
            Camera.main.fieldOfView = fov;
        }
        else
        {
            nativeSendErrorMessage("SetCameraFov Error :" + value + " " + "Parse Failed");
        }
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

    public void nativeSendErrorMessage(string message)
    {

    }

    public void SetZoomInitWhenMove(string value)
    {
        overallSetting.SetZoomInitWhenMove(value);
    }

    public void SetCursor(string value)
    {
        overallSetting.SetCursor(value);
    }

    public void SetMovePointsVisible(string value)
    {
        bool visible = bool.Parse(value);
        StartCoroutine(constructor.SetMovePointsVisible(visible));
    }

}
