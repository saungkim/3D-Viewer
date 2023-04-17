using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MeasurementDot : MonoBehaviour
{
    bool selected = false;
    [SerializeField] private Image image;
    private void Start()
    {
       
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

}
