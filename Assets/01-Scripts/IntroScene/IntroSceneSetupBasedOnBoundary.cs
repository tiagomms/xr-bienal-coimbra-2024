using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Timeline:
/// - CalculateBoundary - Rotate environment 90 degrees if needed
/// - Show Start Game Origin: people walk there and triggers next event (show objects)
/// - Show objects / UI / little background music
/// </summary>
public class IntroSceneSetupBasedOnBoundary : MonoBehaviour
{
    [SerializeField] private SetupPlayer _setupPlayer;
    [SerializeField] private Transform _environmentParent;
    [SerializeField] private Transform _startGameSetup;
    [SerializeField] private Transform _environment;
    
    [SerializeField] 
    private bool shouldUseDebugUiText = false;
    
    [SerializeField] 
    private StringGameEvent writeToDebugUi;
    
    private Vector3 _boundarySize;
    private Vector3[] _points;
    
    private int _boundaryCalculationsCounter = 0;

    private void Awake()
    {
        _environment.gameObject.SetActive(false);
        _startGameSetup.gameObject.SetActive(false);
        _environmentParent.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PerformCalibrationSetup());
    }

    private IEnumerator PerformCalibrationSetup()
    {
        yield return new WaitForSeconds(3f);
        CalculateBoundary();
        
        // set environment active so you can see
        _environmentParent.gameObject.SetActive(true);
        
        // We setup the cage as the biggest size the forward, and not the right
        // if boundary size width is bigger than length, we should rotate the environment 90 degrees over the Y axis
        if (_environmentParent != null && _boundarySize.x > _boundarySize.z)
        {
            _environmentParent.eulerAngles = new Vector3(0f, 90f, 0f);
        }

        if (shouldUseDebugUiText && _points.Length != 0)
        {
            string pointsStr = "(" + _boundaryCalculationsCounter + ") PlayArea size:\n" + _boundarySize + "\n\n" 
                               + string.Join("\n", _points.Select(p => p.ToString()));
            if (writeToDebugUi != null)
            {
                writeToDebugUi.Raise("All good!\n\nPronto para começar!");
            }
            DebugManager.Instance.Log(pointsStr);
        }

        StartCoroutine(ShowPlayerCalibrationPosition());
        
        _boundaryCalculationsCounter++;
    }

    private void CalculateBoundary()
    {
        if (OVRManager.boundary.GetConfigured())
        {
            _boundarySize = OVRManager.boundary.GetDimensions(OVRBoundary.BoundaryType.PlayArea);
            _points = OVRManager.boundary.GetGeometry(OVRBoundary.BoundaryType.PlayArea);
        }
    }
    
    private IEnumerator ShowPlayerCalibrationPosition()
    {
        yield return new WaitForEndOfFrame();
        _startGameSetup.gameObject.SetActive(true);
    }

    public void StartIntroSceneNowAllSetup()
    {
        
    }
    
    
}
