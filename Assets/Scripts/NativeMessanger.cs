using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using static DefectConstructor;
using ColorUtility = UnityEngine.ColorUtility;

public class NativeMessanger : MonoBehaviour
{
#if UNITY_IOS
     [DllImport("__Internal")]
    public static extern void onViewerLoaded(string message);
     [DllImport("__Internal")]
    public static extern void onViewerClicked(string message);
#endif
    [SerializeField] private Constructor constructor;
    [SerializeField] private DefectConstructor defectConstructor;

    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private InputSystem inputSystem;
    [SerializeField] private OverallSetting overallSetting;
    [SerializeField] private CameraController camController;
    [SerializeField] private UIManager uiManager;

    public enum LoadState { None, Loading, Done }
    public LoadState readEnvState = LoadState.None;

    public SceneManagement sceneManagement;

    public enum AndroidState {None,Fragment,Activity}

    private AndroidState androidState;

    private string className = "com.parallel.viewer3d.RoomViewerActivity";
    private string funcName = "RoomViewerReceiveMessage";
 

    // Start is called before the first frame update
    void Start()
    {
        Action<string> nativeErrorMessanger = (string value) => { nativeSendErrorMessage(value); };
        constructor.CreateBoundaryTest();
        SetActiveDefectCreateModeRefresh("True");

#if UNITY_EDITOR
        ReadRoomViewerFile(Application.dataPath + "/Sources/Models/temp/output.env");
        SetActiveDefectCreateMode("True");   
        ViewPanorama("19"); 
#endif
    }


    private void Update()
    {
#if UNITY_ANDROID
      
#endif
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

    public void ReadRoomViewerFile(string fileName)
    {
        print("ReadRoomViewerFile Input Value : " + fileName);

#if UNITY_EDITOR

#elif UNITY_ANDROID
fileName = "jar:file://" + fileName;
      print("Fixed Input FileName Android: " + fileName);
#endif
        if (readEnvState == LoadState.Done)
        {
            constructor.Init();
        }


        readEnvState = LoadState.Loading;
        constructor.FileOpen(fileName, ReadEnvCallBack);
    }

    public void ReadRoomViewerFileDebug(string fileName)
    {
  
        if (readEnvState == LoadState.Done)
        {
            constructor.Init();
        }

        readEnvState = LoadState.Loading;
        constructor.FileOpen(fileName, ReadEnvCallBack);
    }

    public void ReadEnvCallBack(string message)
    {
        print("ReadEnvCallBack:" + message);

        if (message == "Success")
        {

            readEnvState = LoadState.Done;
            print("ReadEnvState:" + readEnvState);
            return;
        }

        readEnvState = LoadState.None;

        print("ReadEnvState:" + readEnvState);
    }

    public void ChangeDefectsJson(string json)
    {
        print("ChangeDefectsJson Input Value : " + json);

        defectConstructor.ReadDefects(json, MessageReceiverCallBack, true);
    }

    public void AddDefectsFile(string filePath)
    {
        print("AddDefectsFile Input Value : " + filePath);

        defectConstructor.ReadAllDefectsWithFilePath(filePath, MessageReceiverCallBack, true);
    }

    public void AddDefectsJson(string json)
    {
        print("AddDefectsJson Input Value : " + json);

        defectConstructor.ReadDefects(json, MessageReceiverCallBack, false);
    }

    public void DestroyDefect(string defectID)
    {
        print("DestroyDefect Input Value : " + defectID);

        defectConstructor.DestroyDefect(defectID, nativeSendErrorMessage);
    }

    public void DestroyDefectsJson(string json)
    {
        print("DestroyDefectsJson Input Value : " + json);

        defectConstructor.DestroyDefectJson(json, nativeSendErrorMessage);
    }

    public void DestroyAllDefects()
    {
        print("DestroyAllDefects");

        defectConstructor.DestroyAllDefects(MessageReceiverCallBack);
    }

    public void SetDefectColor(string value)
    {
        print("SetDefectColor Input Value : " + value);

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
            nativeSendErrorMessage("SetColorDefect Error :" + htmlColor + " " + "ParseHtmlString Failed");
        }
    }

    public void SetDefectsColorJson(string json)
    {
        print("SetDefectsColorJson Input Value : " + json);

        defectConstructor.SetDefectsColor(json);
    }

    public void GetAllDefects()
    {
        print("GetAllDefects");

        defectConstructor.GetAllDefects();
    }

    public void GetDefect(string value)
    {
        print("GetDefect Input Value : " + value);

        defectConstructor.GetDefect(value);
    }

    public void SetCameraSensitivity(string value)
    {
        print("SetCameraSensitivity Input Value : " + value);

        string[] values = value.Split(',');

        float dragSpeed;
        float lerpSpeed;

        if (float.TryParse(values[0], out dragSpeed))
        {
            camController.dragSpeed = dragSpeed;
        }
        else
        {
            nativeSendErrorMessage("SetCameraSensitivity :" + values[0] + " " + "Parse Failed");
        }

        if (float.TryParse(values[1], out lerpSpeed))
        {
            camController.lerpSpeed = lerpSpeed;
        }
        else
        {
            nativeSendErrorMessage("SetCameraSensitivity :" + values[1] + " " + "Parse Failed");
        }
    }

    public void SetCameraReverseDirectionVertical(string value)
    {
        print("SetCameraReverseDirectionVertical Input Value : " + value);

        float directionOut;

        if (float.TryParse(value, out directionOut))
        {
            camController.SetDirectionX(directionOut);
        }
        else
        {
            print("SetCameraReverseDirectionVertical Parse Faile :" + value);
        }
    }

    public void SetCameraReverseDirectionHorizontal(string value)
    {
        print("SetCameraReverseDirectionHorizontal Input Value : " + value);

        float directionOut;

        if (float.TryParse(value, out directionOut))
        {
            camController.SetDirectionY(directionOut);
        }
        else
        {
            print("SetCameraReverseDirectionHorizontal Parse Faile :" + value);
        }
    }

    public void SetCameraFov(string value)
    {
        print("SetCameraFov Input Value : " + value);

        float fov;

        if (float.TryParse(value, out fov))
        {
            Camera.main.fieldOfView = fov;
        }
        else
        {
            nativeSendErrorMessage("SetCameraFov Error :" + value + " " + "Parse Failed");
        }
    }

    public void SetMoveTime(string value)
    {
        print("SetMoveTime Input Value : " + value);

        float moveTime;
        if (float.TryParse(value, out moveTime))
        {
            camController.SetMoveTime(moveTime);
        }
        else
        {
            nativeSendErrorMessage("SetMoveTime Error :" + value + " " + "Parse Failed");
        }
    }

    public void ViewPanorama(string value)
    {
        print("ViewPanorama Input Value : " + value);

        int stage = int.Parse(value);
        StartCoroutine(constructor.InitStage(stage,OnViewerLoaded));
    }

    public void ViewPanoramaTag(string name)
    {

        print("ViewPanoramaTag Input Value : " + name);
        print("ViewPanoramaTag Start");

        if (name == "1")
        {
            ViewPanorama("30");
        }
        else if (name == "2")
        {
            ViewPanorama("3");
        }
        else if (name == "3")
        {
            ViewPanorama("24");
        }
        else if (name == "4")
        {
            ViewPanorama("13");
        }
        else if (name == "5")
        {
            ViewPanorama("13");
        }
        else if (name == "6")
        {
            ViewPanorama("16");
        }
        else if (name == "7")
        {
            ViewPanorama("6");
        }
        else if (name == "8")
        {
            ViewPanorama("31");
        }
        else if (name == "9")
        {
            ViewPanorama("27");
        }
        else if (name == "10")
        {
            ViewPanorama("27");
        }
        else if (name == "11")
        {
            ViewPanorama("5");
        }
        else if (name == "12")
        {
            ViewPanorama("7");
        }
        else if (name == "13")
        {
            ViewPanorama("11");
        }
    }

    public void ViewDefectJson(string json)
    {
        print("VIewDefectJson Input Value:" + json);
        StartCoroutine(defectConstructor.VIewDefectJson(json, OnViewerLoaded));
    }


    public void ViewDefectJsonArray(string json)
    {
        print("ViewDefectJsonArray Input Value:" + json);
        StartCoroutine(defectConstructor.ViewDefectJsonArray(json));
      
    }

    public void ViewDefect(string defectId)
    {
        print("ViewDefect Input Value:" + defectId);
        StartCoroutine(defectConstructor.MoveCamInstant(defectId));
    }

    public void SetImageTransAlpha(string value)
    {
        print("SetImageTransAlpha Input Value:" + value);
        string[] values = value.Split(",");


        camController.SetmoveStartAlpha(float.Parse(values[0]));
        camController.SetafterMoveEndAlpha(float.Parse(values[1]));

    }

    public void SetImageTransAlphaTime(string value)
    {
        print("SetImageTransAlphaTime Input Value:" + value);
        string[] values = value.Split(",");

        camController.SetstartImageTransTime(float.Parse(values[0]));
        camController.SetendImageTransTime(float.Parse(values[1]));
    }

    public void SetActiveZoomInitOnMove(string value)
    {
        print("SetActiveZoomInitOnMove Input Value:" + value);
        overallSetting.SetZoomInitWhenMove(value);
    }

    public void SetActiveCursor(string value)
    {
        print("SetActiveCursor Input Value:" + value);
        overallSetting.SetCursor(value);
    }

    public void SetActiveMovePoints(string value)
    {
        print("SetActiveMovePoints Input Value:" + value);
        bool visible = bool.Parse(value);
        StartCoroutine(constructor.SetMovePointsVisible(visible));
    }

    public void SetActiveMinimap(string value)
    {
        print("SetActiveMinimap Input Value:" + value);
        constructor.SetMiniMap();
    }

    public void SetActiveDevelopmentUI(string value)
    {
        print("SetActiveDevelopmentUI Input Value:" + value);
        bool onOff;

        if (bool.TryParse(value, out onOff))
        {
            uiManager.DevelopmentUISetActive(onOff);
        }
        else
        {
            print("SetActiveDevelopmentUI Bool Parse Failed :" + value);
        }
    }

    public void MessageReceiverCallBack(string message)
    {

    }

    public void ReadDefectCallBack(string message)
    {
        
    }

    public void OnViewerClicked(string message)
    {
#if UNITY_ANDROID
        try
        {
            AndroidJavaClass jc = new AndroidJavaClass(className);
            AndroidJavaObject overrideActivity = jc.GetStatic<AndroidJavaObject>("instance");
            overrideActivity.Call("onViewerClicked", message);
        }
        catch (Exception e)
        {
            print(e);
        }
#elif UNITY_IOS || UNITY_TVOS
       onViewerClicked(message);
#elif UNITY_EDITOR
#endif
        print("OnViewerClicked" + message);
    }

    public void OnViewerLoaded(string message) 
    {
#if UNITY_ANDROID
        try
        {
            AndroidJavaClass jc = new AndroidJavaClass(className);
            AndroidJavaObject overrideActivity = jc.GetStatic<AndroidJavaObject>("instance");
            overrideActivity.Call("onViewerLoaded", message);
        }
        catch (Exception e)
        {
            print(e);
        }
#elif UNITY_IOS || UNITY_TVOS
       onViewerLoaded(message);
       // NativeAPI.showHostMainWindow(lastStringColor);
#elif UNITY_EDITOR
#endif
        print("OnViewerLoaded" + message);
    }
    
    public void nativeSendErrorMessage(string message)
    {
        print(message);
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

    public void Unload()
    {
        Application.Unload();
    }

    public void SetClassName(string value)
    {
        className = value;
    }

    public void SetFuncName(string value)
    {
        funcName= value; 
    }

    public void AddDefectJson(string json)
    {
        print("AddDefectJson Input Value:" + json);

        defectConstructor.AddDefectJson(json);
    }

    public void SetSafeArea(string value)
    {
        print("SetSafeArea Input Value:" + value);

        bool safeAreaBool = Boolean.Parse(value);
        uiManager.SetActiveSafeArea(safeAreaBool);
    }

    public void SetSafeAreaColor(string value)
    {
        print("SetSafeAreaColor Input Value:" + value);

        Color color;
        ColorUtility.TryParseHtmlString(value, out color);
        uiManager.SetSafeAreaHTMLColor(color);      
    }

    public void SetActiveDefectCreateMode(string value)
    {
        print("SetActiveDefectCreateMode Input Value:" + value);

        bool onOff = bool.Parse(value);

        uiManager.SetActiveDefectCreateMode(onOff);
    }

    public void SetActiveDefectCreateModeRefresh(string value)
    {
        print("SetActiveDefectCreateModeRefresh Input Value:" + value);

        bool onOff = bool.Parse(value);

        defectConstructor.SetCreateDotRefresh(onOff);
    }

    public void SetDefectCreateDefaultColor(string value)
    {
        print("SetDefectCreateDefaultColor Input Value:" + value);

        Color outColor; 
        ColorUtility.TryParseHtmlString(value,out outColor);

        defectConstructor.SetDefaultColor(outColor);
    }

    public void SetDefectColliderSize(string value)
    {
        print("SetDefectColliderSize Input Value:" + value);

        string[] valueOut = value.Split(',');
        Vector3 size = new Vector3(float.Parse(valueOut[0]), float.Parse(valueOut[1]), float.Parse(valueOut[2]));
        defectConstructor.SetColliderSize(size);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
