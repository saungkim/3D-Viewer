using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MiniMapConstructor : MonoBehaviour
{

    [SerializeField] private GameObject miniMapPosition;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private GameObject miniMapCamera;
    public Transform miniMap;

    public bool init = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ActiveMiniMap(bool onOff)
    {
        miniMapPosition.SetActive(onOff);
        uiManager.ActivateMiniMap(onOff);
        miniMapCamera.SetActive(onOff);
        init = onOff;
    }


}
