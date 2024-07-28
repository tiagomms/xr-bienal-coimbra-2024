using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePortalToContinueGame : MonoBehaviour
{
    [SerializeField] Transform portalMesh;
    [SerializeField] GameObject UiWithContinue;
    [SerializeField] GameObject UiWithoutContinue;
    private void Awake() {
        // if there was a last scene
        if (portalMesh != null && UiWithContinue != null && UiWithoutContinue != null)
        {
            bool isPossibleToContinue = GlobalManager.Instance.LastSceneCageOrigin != null;
            if (isPossibleToContinue)
            {
                Transform lastStartLocation = GlobalManager.Instance.LastSceneCageOrigin.StartLocation;
                portalMesh.position = new Vector3(lastStartLocation.position.x, portalMesh.position.y, lastStartLocation.position.z);
                portalMesh.rotation = lastStartLocation.rotation;
            }
            UiWithContinue.SetActive(isPossibleToContinue);
            UiWithoutContinue.SetActive(!isPossibleToContinue);
            gameObject.SetActive(isPossibleToContinue);
        }
    }
}
