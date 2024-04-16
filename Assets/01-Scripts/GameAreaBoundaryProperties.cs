using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAreaBoundaryProperties : MonoBehaviour
{
    [SerializeField] private Transform _startLocation;

    public Transform StartLocation => _startLocation;
}
