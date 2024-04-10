using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Global Settings", menuName = "Settings/Global Settings")]
public class GlobalSettings : ScriptableObject
{
    [Tooltip("Calibration should be enabled when building app at the end")]
    [SerializeField] private bool enableCalibrationOnTakingHeadsetOff = true;
    [Tooltip("Disable Hand Model When Grabbing Object")]
    [SerializeField] private bool disableHandModelWhenGrabbing = true;

    public bool EnableCalibrationOnTakingHeadsetOff => enableCalibrationOnTakingHeadsetOff;

    public bool DisableHandModelWhenGrabbing => disableHandModelWhenGrabbing;
}
