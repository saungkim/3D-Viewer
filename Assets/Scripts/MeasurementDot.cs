using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MeasurementDot : MeasurementObject
{
    bool selected = false;
    [SerializeField] private Image image;

    private void Start()
    {
        Select(transform.parent.GetComponent<MeasurementUnit>().GetSelect());
    }

    public void SelectDot(bool onOff)
    {
        selected = onOff;

        if (onOff)
        {
            image.color = Color.yellow;
            print("selected Yellow");
        }
        else
        {
            image.color = Color.black;
            print("Select Black");
        }
    }

    public override void Select(bool onOff)
    {
        if (onOff)
        {
            image.color = Color.yellow;
        }
        else
        {
            image.color = Color.black;
        }
    }

}
