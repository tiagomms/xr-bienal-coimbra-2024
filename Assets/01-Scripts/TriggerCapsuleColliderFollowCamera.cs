using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// TriggerCapsuleColliderFollowCamera - it matches the target position and orientation on the y axis only
/// the - initialTargetHeight / 2 is just for one thing: the collider height match the supposed player height from top to bottom 
/// </summary>
public class TriggerCapsuleColliderFollowCamera : MonoBehaviour
{
    public Vector3 offset;
    [SerializeField] private Transform _followTarget;

    private Vector3 initialTargetHeight;
    private new CapsuleCollider collider;

    void Awake()
    {
        collider = GetComponent<CapsuleCollider>();
    }

    void Start() {
        initialTargetHeight = new Vector3(0, _followTarget.position.y, 0);
        collider.height = _followTarget.position.y;
        transform.rotation = _followTarget.rotation;
    }
    // Update is called once per frame
    void Update()
    {
        transform.position = _followTarget.position + offset - (initialTargetHeight / 2);
        transform.rotation = Quaternion.Euler(0f, _followTarget.rotation.eulerAngles.y, 0f);
    }
}
