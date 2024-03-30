using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
[CreateAssetMenu(fileName = "StringGameEvent", menuName = "Game Events/String Event")]
public class StringGameEvent : BaseGameEvent<string> { }

[CreateAssetMenu(fileName = "FloatGameEvent", menuName = "Game Events/Float Event")]
public class FloatGameEvent : BaseGameEvent<float> { }

[CreateAssetMenu(fileName = "IntGameEvent", menuName = "Game Events/Int Event")]
public class IntGameEvent : BaseGameEvent<int> { }



