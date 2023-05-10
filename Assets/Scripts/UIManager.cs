using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject miniMap;
    [SerializeField] private OverallSetting overallSetting;

    [SerializeField] private Color selectedColor;
    [SerializeField] private GameObject defectDot;
    [SerializeField] private GameObject movePoint;

    [SerializeField] private GameObject screenGauage;
    [SerializeField] private InputSystem inputSystem;

    [SerializeField] private Measurement measureMent;
    [SerializeField] private GameObject measrueMentButton;

    [SerializeField] private PlayerMovement playerMovement;

    //[SerializeField] private GameObject ;
    [SerializeField] private GameObject developmentUI;


    Action<Image> controlAction;
    Image selectedImage;
   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Unload()
    {
        Application.Unload();
    }

    public void ActivateMiniMap(bool onOff) 
    {
        miniMap.SetActive(onOff);
    }


    public void InverseActivateMiniMap(Image img)
    {
        SetColorSelected(!miniMap.activeSelf, img);

        overallSetting.ActivateMinimap(!miniMap.activeSelf);
      
    }

    public void InverseActivateMovePoint(Image img)
    {
        SetColorSelected(!movePoint.activeSelf, img);
        movePoint.SetActive(!movePoint.activeSelf);
    }

    private void SetColorSelected(bool obj,Image img)
    {
        if (obj)
        {
            img.color = selectedColor;
        }
        else
        {
            img.color = Color.white;
        }
    }

    public void InverseActivateDefectDot(Image img)
    {
        if (controlAction == null)
        {
            controlAction = (img) => { InverseActivateDefectDot(img); };
            selectedImage = img;
        }
        else if (selectedImage == img)
        {
            controlAction = null;
            selectedImage = img;
        }
        else if (selectedImage != null)
        {
            Image localselectedImg = selectedImage;
            selectedImage = null;
            controlAction.Invoke(localselectedImg);

            controlAction = (Image img) => { InverseActivateDefectDot(img); };
            selectedImage = img;
        }

        SetColorSelected(!defectDot.activeSelf, img);
        inputSystem.SetEnableDot(!defectDot.activeSelf);
        defectDot.SetActive(!defectDot.activeSelf);
       
        if (defectDot.activeSelf == false)
        {
            screenGauage.SetActive(false);
        }
        else
        {
            //screenGauage.SetActive(true);
            inputSystem.SetControlState(InputSystem.ControlState.Defect);
        }
    }

    public void InverseActivateDefectDot()
    {
        inputSystem.SetEnableDot(!defectDot.activeSelf);
        defectDot.SetActive(!defectDot.activeSelf);

        if (defectDot.activeSelf == false)
        {
            screenGauage.SetActive(false);
        }
        else
        {
            //screenGauage.SetActive(true);
            inputSystem.SetControlState(InputSystem.ControlState.Defect);
        }
    }

    public void InverseMeasureMode(Image img)
    {
        if (controlAction == null)
        {
            controlAction = (Image img) => { InverseMeasureMode(img); };
            selectedImage = img;
        }
        else if (selectedImage == img)
        {
            controlAction = null;
            selectedImage = img;
        }
        else if (selectedImage != null)
        {
            Image localselectedImg = selectedImage;
            selectedImage = null;
            controlAction.Invoke(localselectedImg);
          
            controlAction = (Image img) => { InverseMeasureMode(img); };
            selectedImage = img;
        }

        bool onOff;

        if(inputSystem.GetControlState() != InputSystem.ControlState.Measure && inputSystem.GetControlState() != InputSystem.ControlState.MeasureDot)
        {
            onOff = true;
            measrueMentButton.SetActive(onOff);
            inputSystem.SetControlState(InputSystem.ControlState.Measure);
        } 
        else
        {
            onOff = false;
            measrueMentButton.SetActive(onOff);
            inputSystem.SetControlState(InputSystem.ControlState.None);
            measureMent.ActivateMeasurement(onOff);
        }
        SetColorSelected(onOff, img);
    }

    public void InverseMeasureFixMode()
    {
        measureMent.InverseActivateMeasurement();
        if(inputSystem.GetControlState() == InputSystem.ControlState.Measure)
        {
            inputSystem.SetControlState(InputSystem.ControlState.MeasureDot);
        }
        else if(inputSystem.GetControlState() == InputSystem.ControlState.MeasureDot)
        {
            inputSystem.SetControlState(InputSystem.ControlState.Measure);
        }
       
    }

    IEnumerator autoTour;
    public void InverseAutoTour(Image img)
    {
        if (controlAction == null)
        {
            controlAction = (Image img) => { InverseAutoTour(img); };
            selectedImage = img;
        }
        else if (selectedImage == img)
        {
            controlAction = null;
            selectedImage = img;
        }
        else if (selectedImage != null)
        {
            Image localselectedImg = selectedImage;
            selectedImage = null;
            controlAction.Invoke(localselectedImg);

            controlAction = (Image img) => { InverseAutoTour(img); };
            selectedImage = img;
        }

        bool onOff;

        if (inputSystem.GetControlState() != InputSystem.ControlState.AutoTour)
        {
            onOff = true;
            inputSystem.SetControlState(InputSystem.ControlState.AutoTour);

            autoTour = playerMovement.AutoTour(0, 0);
            StartCoroutine(autoTour);
        }
        else
        {
            onOff = false;
            inputSystem.SetControlState(InputSystem.ControlState.None);
            StopCoroutine(autoTour);
        }

        SetColorSelected(onOff, img);
    }

    public void InverseTag(Image img)
    {
        if (controlAction == null)
        {
            controlAction = (Image img) => { InverseTag(img); };
            selectedImage = img;
        }
        else if (selectedImage == img)
        {
            controlAction = null;
            selectedImage = img;
        }
        else if (selectedImage != null)
        {
            Image localselectedImg = selectedImage;
            selectedImage = null;
            controlAction.Invoke(localselectedImg);

            controlAction = (Image img) => { InverseTag(img); };
            selectedImage = img;
        }

        bool onOff;

        if (inputSystem.GetControlState() != InputSystem.ControlState.AutoTour)
        {
            onOff = true;
            inputSystem.SetControlState(InputSystem.ControlState.AutoTour);
        }
        else
        {
            onOff = false;
            inputSystem.SetControlState(InputSystem.ControlState.None);
        }

        SetColorSelected(onOff, img);
    }

    public void MeasurementDestroySelectedMeasureUnit()
    {
        measureMent.DestroySelectedMeasureUnit();
    }

    public void MeasurementCompleteSelectedMeasureUnit()
    {
        measureMent.CompleteSelectedMeasureUnit();
    }

    public void DevelopmentUISetActive(bool onOff)
    {
        developmentUI.SetActive(onOff);
    }

    public void Init()
    {
       
    }
}
