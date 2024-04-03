using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    private float defaultSceneTransitionDuration = 3f;

    private void OnEnable()
    {
        AbstractPortalToSomeNewPlace.OnPortalActivated += LoadSceneFromPortalIfToNewScene;
    }

    private void OnDisable()
    {
        AbstractPortalToSomeNewPlace.OnPortalActivated -= LoadSceneFromPortalIfToNewScene;
    }

    public void GoToScene(string sceneName)
    {
        StartCoroutine(GoToSceneRoutine(sceneName));
    }

    private IEnumerator GoToSceneRoutine(string sceneName)
    {
        yield return new WaitForSeconds(defaultSceneTransitionDuration);
        SceneManager.LoadScene(sceneName);
    }

    private void LoadSceneFromPortalIfToNewScene(PortalSettings portalSettings)
    {
        if (portalSettings.PortalType == PortalType.ToNewScene)
        {
            StartCoroutine(GoToSceneAsyncRoutine(portalSettings.NextSceneName, portalSettings.fadeAnimationDuration));
        }
    }
    
    public void GoToSceneAsync(string sceneName)
    {
        StartCoroutine(GoToSceneAsyncRoutine(sceneName, defaultSceneTransitionDuration));
    }

    private IEnumerator GoToSceneAsyncRoutine(string sceneName, float fadeDuration)
    {

        Application.backgroundLoadingPriority = ThreadPriority.Low;
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        
        float timer = 0;
        while (timer <= fadeDuration && !operation.isDone)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        Application.backgroundLoadingPriority = ThreadPriority.Normal;
        operation.allowSceneActivation = true;
    }

}
