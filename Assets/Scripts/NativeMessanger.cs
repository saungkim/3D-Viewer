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
    [SerializeField] private UIManager uiMaanger;

    public enum LoadState { None, Loading, Done }
   public LoadState readEnvState = LoadState.None;
    LoadState readDefectState = LoadState.None;

    public SceneManagement sceneManagement;

    // Start is called before the first frame update
    void Start()
    {
        print("Start Unity");

        Action<string> nativeErrorMessanger = (string value) => { nativeSendErrorMessage(value);};

#if UNITY_EDITOR
        string fileName = "inputNHMatterport";
        fileName = Application.dataPath + "/Sources/" + fileName + ".env";
#endif

        //SetEndMoveAlpha("0");
        //SetEndImageTransTime("1");
        //SetStartMoveAlpha("1");
        //SetStartImageTransTime("1"); 
        //sceneManagement

        //sceneManagement = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneManagement>();
      
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)){
            ReadRoomViewerFile(Application.streamingAssetsPath + "/input.env");
            ViewStage("0");
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            ReadRoomViewerFile(Application.streamingAssetsPath + "/input.env");
            ReadDefectsWithFilePath(Application.streamingAssetsPath + "/input.json");
            ViewStage("19");
            SetCursor("false");
            SetMovePointsVisible("true");
            SetMoveTime("1");
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

    public void ReadRoomViewerFile(string fileName)
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
        print("ReadEnvCallBack:" + message );

        if (message == "Success")
        {

            readEnvState = LoadState.Done;
            print("ReadEnvState:" + readEnvState);
            return;
        }

        readEnvState = LoadState.None;

        print("ReadEnvState:" + readEnvState);
    }

    public void ReadDefectsWithFilePath(string filePath)
    {
        if (readDefectState == LoadState.Done)
        {
            ReadEnvCallBack("");
            return;
        }

        readDefectState = LoadState.Loading;
        StartCoroutine(defectConstructor.ReadAllDefectWithFilePath(filePath, ReadDefectCallBack,false));
    }

    public void ChangeDefectsJson(string json)
    {
        defectConstructor.ReadAllDefect(json,MessageReceiverCallBack,true);
    }

    public void AddDefectsFile(string filePath)
    {

    }

    public void AddDefectsJson(string json)
    {
        defectConstructor.ReadAllDefect(json, MessageReceiverCallBack,false);
    }

    public void DestroyDefect(int defectID)
    {
       
    }

    public void DestroyDefectsJson(string json)
    {
        defectConstructor.ReadAllDefect(json, MessageReceiverCallBack,false);
    }

    public void DestroyAllDefects()
    {
        defectConstructor.DestroyAllDefects(MessageReceiverCallBack);
    }

    public void SetActiveDefectCreateMode()
    {

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
            nativeSendErrorMessage("SetColorDefect Error :" + htmlColor + " " + "ParseHtmlString Failed");
        }
    }

    public void SetDefectsColorJson(string json)
    {

    }

    public void GetAllDefects()
    {

    }

    public void GetDefect(string value)
    {

    }

    public void SetCameraSensitivity(string value)
    {
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

    public void SetCameraReverseDirectionVertical()
    {

    }

    public void SEtCameraReverseDirectionHorizontal()
    {

    }

    public void SetCameraFov(string value)
    {
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

    }

    public void ViewDefect(string defectId)
    {
        StartCoroutine(defectConstructor.MoveCamInstant(defectId));
    }

    public void SetImageTransAlpha(string value)
    {

    }

    public void SetImageTransAlphaTime(string value)
    {

    }

    public void SetActiveZoomInitOnMove()
    {

    }

    public void SetActiveCursor()
    {

    }

    public void SetActiveMinimap(bool onOff)
    {
       
    }

    public void SetActiveDevelopmentUI()
    {

    }



    public void MessageReceiverCallBack(string message)
    {

    }

    public void ReadDefectCallBack(string message)
    {
        if (message == "Succcess")
        {
            readDefectState = LoadState.Done;
            return;
        }
        readDefectState = LoadState.None;
    }

 

    public void DestoryDefect(string defectId)
    {

        defectConstructor.DestroyDefect(defectId, nativeSendErrorMessage);
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






}
