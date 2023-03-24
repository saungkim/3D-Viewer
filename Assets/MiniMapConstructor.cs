using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MiniMapConstructor : MonoBehaviour
{

    [SerializeField] private GameObject miniMapPosition;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private GameObject miniMapCamera;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ActiveMiniMap()
    {
        miniMapPosition.SetActive(true);
        uiManager.ActivateMiniMap(true);
        miniMapCamera.SetActive(true);
    }
}
