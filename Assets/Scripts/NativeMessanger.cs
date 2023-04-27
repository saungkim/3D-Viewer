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
        string fileName = "inputNHMatterport";
        fileName = Application.dataPath + "/Sources/" + fileName + ".env";
#endif
        ReadEnv(Application.streamingAssetsPath + "/input.env");
        ReadDefectWithFilePath(Application.streamingAssetsPath + "/input.json");
        ViewStage("19");
        //EnableDotCreateMode();
        SetCursor("false");
        SetMovePointsVisible("true");
        SetMoveTime("1");
        SetEndMoveAlpha("0");
        SetEndImageTransTime("1");
        SetStartMoveAlpha("1");
        SetStartImageTransTime("1");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetDefectColor("NeedToChange1,#FF0000");
        }
    }

    public void ActivateDefectCreateMode(string value)
    {
        bool createMode;

        if (bool.TryParse(value, out createMode))
        {
            inputSystem.SetEnableDot(createMode);
        }
        else
        {
            nativeSendErrorMessage("EnableDotCreateMode Error :" + value + " " + "Parse Failed");
        }

    
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

    public void ReadDefectWithFilePath(string filePath)
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

    public void ReadDefects(string json)
    {
        
    }

    public void AddDefects(string json)
    {

    }

    public void DestroyDefects(string json)
    {

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

    public void DestoryDefect(string defectId)
    {

        defectConstructor.DestroyDefect(defectId, nativeSendErrorMessage);
    }

    public void SetDefectColor(string value)
    {
        string[] values = value.Split(',');
        string htmlColor = values[1]; // "#FF0000"; Red
        string id = values[0];

        Color outColor;

        if (ColorUtility.TryParseHtmlString(htmlColor, out outColor))
        {
            defectConstructor.SetDefectColor(id, outColor, nativeSendErrorMessage);
        }
        else
        {
            nativeSendErrorMessage("SetColorDefect Error :" + htmlColor + " "+ "ParseHtmlString Failed");
        }
    }

    public void CreateTag(string id)
    {

    }

    public void SetMoveTime(string value)
    {
        float moveTime;
        if (float.TryParse(value, out moveTime))
        {
            camController.SetMoveTime(moveTime);
        }
        else
        {
            nativeSendErrorMessage("SetMoveTime Error :" + value +" "+ "Parse Failed");
        }
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

            print(e);
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
        print(message);
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

    public void SetActiveMinimap()
    {
        constructor.SetMiniMap();
    }

    public void SetStartImageTransTime(string value)
    {
        float time;

        if (float.TryParse(value, out time))
        {
            overallSetting.SetstartImageTransTime(time);
        }
        else
        {
            nativeSendErrorMessage("SetStartImageTransTime Error :" + value + " " + "Parse Failed");
        } 
    
    }

    public void SetEndImageTransTime(string value)
    {
        float time;
      

        if (float.TryParse(value, out time))
        {
            overallSetting.SetendImageTransTime(time);
        }
        else
        {
            nativeSendErrorMessage("SetEndImageTransTime Error :" + value + " " + "Parse Failed");
        }
    }

    public void SetStartMoveAlpha(string value)
    {
        float alpha;

        if (float.TryParse(value, out alpha))
        {
            overallSetting.SetmoveStartAlpha(alpha);
        }
        else
        {
            nativeSendErrorMessage("SetStartMoveAlpha Error :" + value + " " + "Parse Failed");
        }
    }

    public void SetEndMoveAlpha(string value)
    {
        float alpha;

        if (float.TryParse(value, out alpha))
        {
            overallSetting.SetafterMoveEndAlpha(alpha);
        }
        else
        {
            nativeSendErrorMessage("SetEndMoveAlpha Error :" + value + " " + "Parse Failed");
        }
        
    }
}
