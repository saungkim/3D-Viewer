using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MeasurementDot : MeasurementObject
{
    bool selected = false;
    [SerializeField] private Image image;
    [SerializeField] private BoxCollider boxCollider;

    static private Color selectedColor;
    static private Color redColor;
    [SerializeField] private Outline outline;

    private void Start()
    {
        ColorUtility.TryParseHtmlString("#FFBB00", out selectedColor);
        ColorUtility.TryParseHtmlString("#FF0800", out redColor);
        //outline = GetComponent<Outline>();
        //Select(transform.parent.GetComponent<MeasurementUnit>().GetSelect());
    }

    public void SelectDot(bool onOff)
    {
        selected = onOff;

        if (onOff)
        {
          

              image.color = Color.white;
            outline.effectColor = redColor;
              
        }
        else
        {
            image.color = selectedColor;
            outline.effectColor = selectedColor;

        
        }
        
    }

    public override void Select(bool onOff)
    {
        if (onOff)
        {


            image.color = Color.white;
            outline.effectColor = redColor;

        }
        else
        {
            image.color = selectedColor;
            outline.effectColor = selectedColor;


        }
    }

    public override void SetCollider()
    {
        boxCollider.enabled = true;
    }
    private void Update()
    {
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(transform.position);

        Vector2 scaler = Vector2.one - new Vector2(Mathf.Abs(screenPosition.x - Screen.width / 2) / Screen.width, Mathf.Abs(screenPosition.y - Screen.height / 2) / Screen.height);

        if (Mathf.Abs(scaler.x) > 1 || Mathf.Abs(scaler.y) > 1)
        {
            //print("Scaler Worng" + scaler.magnitude);
            return;
        }

        float fov = Camera.main.fieldOfView;
        if (fov > 100)
        {
            fov = 100;
        }

     
    }
}
