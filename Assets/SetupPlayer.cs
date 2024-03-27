using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

public class SetupPlayer : MonoBehaviour
{
    [SerializeField]
    private GameObject _xrPlayer;

    [SerializeField]
    private GameObject _xrSimulator;

    [SerializeField]
    private bool _hideOnBuildHelpfulObjects = true;

    private void Awake() {
        #if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_OSX
        
        // (1) make sure xrSimulator is on
        _xrPlayer.SetActive(true);
        _xrSimulator.SetActive(true);


        #elif UNITY_ANDROID

        // (1) make sure xrSimulator is off 
        //(for some reason this does not work - had to put xrSimulator disabled manually)
        _xrPlayer.SetActive(true);
        _xrSimulator.SetActive(false);

        // (2) force hide of all game objects with tag 'HideOnBuild"
        if (_hideOnBuildHelpfulObjects)
        {
            GameObject[] hideObjs = GameObject.FindGameObjectsWithTag("HideOnBuild");
            foreach (GameObject item in hideObjs)
            {
                item.SetActive(false);
            }
        }
        #endif    
    }

    private void Update()
    {
        XRDevice.IsHMDMounted();
    }
}
