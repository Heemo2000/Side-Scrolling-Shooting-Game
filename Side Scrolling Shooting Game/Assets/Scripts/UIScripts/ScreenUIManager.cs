using System;
using System.Collections.Generic;
using UnityEngine;

public class ScreenUIManager : MonoBehaviour
{
    [SerializeField]private GameObject[] uiArray;

    [SerializeField]private GameObject startingUI;
    

    private GameObject _previousUI;
    private void Awake()
    {
        for(int i = 0; i < uiArray.Length; i++)
        {
            GameObject currentUI = uiArray[i];
            currentUI.gameObject.SetActive(false);
        }

        Open(startingUI);
    }
    
    public void Open(GameObject instance)
    {
        if(instance == null)
        {
            return;
        }

        if(_previousUI != null)
        {
            if(_previousUI == instance)
            {
                return;
            }
            _previousUI.gameObject.SetActive(false);
        }
        
        instance.gameObject.SetActive(true);
        _previousUI = instance;
    }


}
