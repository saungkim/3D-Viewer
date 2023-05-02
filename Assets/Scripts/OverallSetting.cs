using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverallSetting : MonoBehaviour
{
    private bool zoomInit = false;

    [Header("Movement")]
    [SerializeField] float moveTime;

    [Header("Cotainer")]
    [SerializeField] ViewerCursor cursor;
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] CameraController camController;
    [SerializeField] MiniMapConstructor miniMapConstructor;
    [SerializeField] Constructor constructor;
    // Start is called before the first frame update



    bool initMiniMap = false;


    void Start()
    {
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetZoomInitWhenMove(string value)
    {
        zoomInit = bool.Parse(value);
    }

    public void SetCursorWhenZoom(string value)
    {

    }

    public void SetDefectCreateHeightPosition()
    {

    }

    public bool GetZoomInit()
    {
        return zoomInit;
    }

    public void SetMoveTime(string value)
    {
        float time = int.Parse(value);
    }

    public void SetCursor(string value)
    {
        bool onOff = bool.Parse(value);
        cursor.SetActivate(onOff);
    }

    public void SetDefectHeight()
    {

    }

    public void ApplyValues()
    {
        //camController.SetImageTransTime(imageTransTime);
        camController.SetMoveTime(moveTime);
    }

    public void SetstartImageTransTime(float value)
    {
        camController.SetstartImageTransTime(value);
    }

    public void SetendImageTransTime(float value)
    {
        camController.SetendImageTransTime(value);
    }

    public void SetmoveStartAlpha(float value)
    {
        camController.SetmoveStartAlpha(value);
    }

    public void SetafterMoveEndAlpha(float value)
    {
        camController.SetafterMoveEndAlpha(value);
    }

    public void ActivateMinimap(bool onOff)
    {
        if (initMiniMap)
        {
            miniMapConstructor.ActiveMiniMap(onOff);
        }
        else
        {
            initMiniMap = true;
            StartCoroutine(constructor.SetMiniMap());
        }
    }

    //Read File
    public void ReadRoomViewerFile(String filePath)
    {

    }

  

}
