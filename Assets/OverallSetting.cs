using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverallSetting : MonoBehaviour
{
    private bool zoomInit = false;
    [SerializeField] Cursor cursor;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetZoomInitWhenMove(string value) {
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

    public void SetMoveTime(string value){
        float time = int.Parse(value);
    }

    public void SetCursor(string value)
    {
        bool onOff = bool.Parse(value);
        print(onOff);
        cursor.SetActivate(onOff);
    }

    public void SetDefectHeight()
    {

    }

    
   
    
}
