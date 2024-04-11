using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Global Settings", menuName = "Settings/Global Settings")]
public class GlobalSettings : ScriptableObject
{
    [Tooltip("Calibration should be enabled when building app at the end")]
    public bool enableCalibrationOnTakingHeadsetOff = true;

    [Tooltip("Disable Hand Model When Grabbing Object")] 
    public bool disableHandModelWhenGrabbing = true;
    
    [Tooltip("Hide On Build Helpful Objects for testing")]
    public bool hideOnBuildHelpfulObjects = true;

    public static GlobalSettings CreateSimulatorInstance()
    {
        GlobalSettings globalSettings = ScriptableObject.CreateInstance<GlobalSettings>();
        globalSettings.disableHandModelWhenGrabbing = false;
        globalSettings.enableCalibrationOnTakingHeadsetOff = false;
        globalSettings.hideOnBuildHelpfulObjects = false;

        return globalSettings;
    }

}
