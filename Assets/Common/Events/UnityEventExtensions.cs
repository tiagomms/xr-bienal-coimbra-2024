using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Unity Event Extensions - to receive strings, floats, whatever needed later on
/// </summary>
[System.Serializable]
public class UnityStringEvent : UnityEvent<string> {}

[System.Serializable]
public class UnityFloatEvent : UnityEvent<float> {}