using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerPlayerLocationMatch : MonoBehaviour
{
    public UnityEvent onMatch;
    public UnityEvent<Transform> onMatchMoveTo;

    [SerializeField] private StringGameEvent onMatchSendString;
    
    //public UnityEvent noLongerMatch;
    [SerializeField] private float invokeOnMatchDelay = 2f;
    [SerializeField] private float dotForwardThreshold = 0.75f;
    [SerializeField] private bool doMatchForever = false;

    private bool _isMatching = false;
    private bool _alreadyMatched = false;
    private Transform _playerTransform;

    private Coroutine _currentCoroutine;
    private void OnTriggerStay(Collider other) {
        if (!_alreadyMatched)
        {
            if (other.gameObject.CompareTag(TagManager.PLAYER_COLLIDER_TAG))
            {
                if (!_isMatching && Vector3.Dot(transform.forward, other.transform.forward) > dotForwardThreshold)
                {
                    _playerTransform = other.transform;
                    _isMatching = true;
                    DebugManager.Instance.Log("isMatching!");
                    _currentCoroutine = StartCoroutine(InvokeOnMatchAfterXSeconds());
                }
                else if (!doMatchForever && _isMatching && Vector3.Dot(transform.forward, other.transform.forward) <= dotForwardThreshold)
                {
                    NoLongerMatching();
                }
            }
        }
    }

    private void NoLongerMatching()
    {
        _isMatching = false;
        _playerTransform = null;
        if (_currentCoroutine != null)
        {
            DebugManager.Instance.Log(" no longer Matching!");
            StopCoroutine(_currentCoroutine);
        }
        _currentCoroutine = null;
    }

    private void OnTriggerExit(Collider other) {
        if (!_alreadyMatched)
        {
            NoLongerMatching();
        }
    }

    IEnumerator InvokeOnMatchAfterXSeconds()
    {
        yield return new WaitForSeconds(invokeOnMatchDelay);

        if (_isMatching)
        {
            onMatch.Invoke();
            onMatchMoveTo.Invoke(_playerTransform);
            
            _alreadyMatched = true;
            string s = "Good to go!";
            DebugManager.Instance.Log(s);
            if (onMatchSendString != null)
            {
                onMatchSendString.Raise(s);
            }
        }
    }
}
