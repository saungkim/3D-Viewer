using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static DefectConstructor;

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
    public string testJson;
    // Start is called before the first frame update
    void Start()
    {
        print("Start Unity");

        Action<string> nativeErrorMessanger = (string value) => { nativeSendErrorMessage(value);};

#if UNITY_EDITOR
        string fileName = "inputNHMatterport";
        fileName = Application.dataPath + "/Sources/" + fileName + ".env";
#endif
        ReadRoomViewerFile(Application.streamingAssetsPath + "/input.env");
        ViewPanorama("19");

        //SetEndMoveAlpha("0");
        //SetEndImageTransTime("1");
        //SetStartMoveAlpha("1");
        //SetStartImageTransTime("1"); 
        //sceneManagement

        //sceneManagement = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneManagement>();

    }

    private void Update()
    {

#if UNITY_EDITOR
        if(Input.GetKeyDown(KeyCode.Space)){
            ReadRoomViewerFile(Application.streamingAssetsPath + "/input.env");
            //ViewStage("0");
        }
 
        if (Input.GetKeyDown(KeyCode.A))
        {
            ReadRoomViewerFile(Application.streamingAssetsPath + "/input.env");
            //ReadDefectsWithFilePath(Application.streamingAssetsPath + "/input.json");
            //string json = "{\"id\":\"4\",\"type\":\"defect\",\"position\":{\"x\":8.248558044433594,\"y\":0.9674921631813049,\"z\":-3.4797799587249758},\"rotation\":{\"x\":1.5485010147094727,\"y\":356.21142578125,\"z\":6.672542962604666e-9},\"view\":{\"position\":{\"x\":8.277060508728028,\"y\":1.5085253715515137,\"z\":-2.141184091567993},\"rotation\":{\"x\":34.679908752441409,\"y\":177.30267333984376,\"z\":-6.488875214927248e-8},\"fov\":60.0}}";
            //ViewStage("19");
             
            AddDefectsJson(testJson);
            //AddDefectsFile(Application.streamingAssetsPath + "/input.json");
            //ActivateDefectCreateMode();
            //ReadRoomViewerFile();
            //ChangeDefectsJson();
            //ViewStage("19");
            //SetCursor("false");
            //SetMovePointsVisible("true");
            //SetMoveTime("1");
        }
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

    //public void ReadDefectsWithFilePath(string filePath)
    //{
    //    if (readDefectState == LoadState.Done)
    //    {
    //        ReadEnvCallBack("");
    //        return;
    //    }

    //    readDefectState = LoadState.Loading;
    //   // StartCoroutine(defectConstructor.ReadAllDefectWithFilePath(filePath, ReadDefectCallBack,false));
    //}

    /// <summary>
    /// ///////////////////////////////
    /// </summary>
    /// <param name="json"></param>

    public void ChangeDefectsJson(string json)
    {
        defectConstructor.ReadDefects(json,MessageReceiverCallBack,true);
    }

    public void AddDefectsFile(string filePath)
    {
        defectConstructor.ReadAllDefectsWithFilePath(filePath, MessageReceiverCallBack, true);
    }

    public void AddDefectsJson(string json)
    {
        defectConstructor.ReadDefects(json, MessageReceiverCallBack,false);
    }

    public void DestroyDefect(string defectID)
    {
        defectConstructor.DestroyDefect(defectID, nativeSendErrorMessage);
    }

    public void DestroyDefectsJson(string json)
    {
        defectConstructor.DestroyDefectJson(json, nativeSendErrorMessage);
    }

    public void DestroyAllDefects()
    {
        defectConstructor.DestroyAllDefects(MessageReceiverCallBack);
    }



    public void SetActiveDefectCreateMode(string value)
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
        defectConstructor.SetDefectsColor(json, NativeSendMessage);
    }

    public void GetAllDefects()
    {
        defectConstructor.GetAllDefects();
    }

    public void GetDefect(string value)
    {
        defectConstructor.GetDefect(value);
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

    public void SetCameraReverseDirectionVertical(string value)
    {


        float directionOut;

        if(float.TryParse(value,out directionOut))
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
        float directionOut;

        if (float.TryParse(value,out directionOut))
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
        int stage = int.Parse(value);
        StartCoroutine(constructor.InitStage(stage));
    }

    public void ViewDefect(string defectId)
    {
        StartCoroutine(defectConstructor.MoveCamInstant(defectId));
    }

    public void SetImageTransAlpha(string value)
    {
        string[] values = value.Split(",");

     
        camController.SetmoveStartAlpha(float.Parse(values[0]));
        camController.SetafterMoveEndAlpha(float.Parse(values[1]));
       
    }

    public void SetImageTransAlphaTime(string value)
    {
        string[] values = value.Split(",");

        camController.SetstartImageTransTime(float.Parse(values[0]));
        camController.SetendImageTransTime(float.Parse(values[1]));
    }

    public void SetActiveZoomInitOnMove(string value)
    {
        overallSetting.SetZoomInitWhenMove(value);
    }

    public void SetActiveCursor(string value)
    {
        overallSetting.SetCursor(value);
    }

    public void SetActiveMovePoints(string value)
    {
        bool visible = bool.Parse(value);
        StartCoroutine(constructor.SetMovePointsVisible(visible));
    }

    public void SetActiveMinimap(string value)
    {
        constructor.SetMiniMap();
    }

    public void SetActiveDevelopmentUI(string value)
    {
        bool onOff;

        if(bool.TryParse(value,out onOff))
        {
            uiMaanger.DevelopmentUISetActive(onOff);
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
        if (message == "Succcess")
        {
            readDefectState = LoadState.Done;
            return;
        }
        readDefectState = LoadState.None;
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
