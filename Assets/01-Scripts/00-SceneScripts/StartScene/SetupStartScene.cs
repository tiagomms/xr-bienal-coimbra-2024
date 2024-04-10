using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupStartScene : MonoBehaviour
{
    [SerializeField] private DefaultFadeSceneSetup defaultFadeSceneSetup;

    public void GoToSceneNumber2()
    {
        DefaultFadeSceneSetup.TriggerFadeOut.Invoke(defaultFadeSceneSetup.DefaultFadeOutDuration, defaultFadeSceneSetup.DefaultFadeOutColor);
        SceneTransitionManager.Instance.GoToSceneAsyncByIndexInXSeconds(2, defaultFadeSceneSetup.DefaultFadeOutDuration);
    }
}
