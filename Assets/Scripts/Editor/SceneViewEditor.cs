using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEditor.Progress;
using UnityEditor;
using TMPro;

[CustomEditor(typeof(SceneView))]
public class SceneViewEditor : Editor
{
    public GameObject text;
    void Start()
    {
        SceneView sceneView = (SceneView)target;
        text = sceneView.text;
    }



    void OnSceneGUI()
    {
        Debug.Log("SceneView");

        if (Event.current.type != EventType.MouseDown || Event.current.button != 0) return;


        var mousePosition = Event.current.mousePosition * EditorGUIUtility.pixelsPerPoint;
        mousePosition.y = Camera.current.pixelHeight - mousePosition.y;

        var Ray = Camera.current.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(Ray, out RaycastHit hit))
        {
            SceneView sceneView = (SceneView)target;
         
            //클릭한 곳의 좌표 hit.point
            // item.transform.position = hit.point;
            Debug.Log(hit.point);
            GameObject text = sceneView.text;
            text = Instantiate(text);
            text.SetActive(true);
            text.transform.position = new Vector3(hit.point.x, text.transform.position.y , hit.point.z);
            text.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = sceneView.count.ToString() + "{" + hit.point.x +","+ hit.point.z + "}";
            ++sceneView.count;
        }
    }

}
