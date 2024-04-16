using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : GenericSingleton<SceneTransitionManager>
{

    private float defaultSceneTransitionDuration = 3f;
    
    private void OnEnable()
    {
        AbstractPortalToSomeNewPlace.OnPortalEnter += LoadSceneFromPortalIfToNewScene;
    }

    private void OnDisable()
    {
        AbstractPortalToSomeNewPlace.OnPortalEnter -= LoadSceneFromPortalIfToNewScene;
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
            StartCoroutine(GoToSceneAsyncRoutine(portalSettings.NextSceneName, -1, portalSettings.enterPortalAnimDuration));
        }
    }

    public void GoToSceneAsyncInXSeconds(string sceneName, float duration)
    {
        StartCoroutine(GoToSceneAsyncRoutine(sceneName, -1, duration));
    }    
    public void GoToSceneAsyncByIndexInXSeconds(int sceneIndex, float duration)
    {
        StartCoroutine(GoToSceneAsyncRoutine(null, sceneIndex, duration));
    }
    
    public void GoToSceneAsync(string sceneName)
    {
        StartCoroutine(GoToSceneAsyncRoutine(sceneName, -1, defaultSceneTransitionDuration));
    }

    public void GoToSceneAsyncByIndex(int sceneIndex)
    {
        StartCoroutine(GoToSceneAsyncRoutine(null, sceneIndex, defaultSceneTransitionDuration));
    }
    
    private IEnumerator GoToSceneAsyncRoutine(string sceneName, int sceneIndex, float fadeDuration)
    {
        AsyncOperation operation = sceneIndex < 0 ? SceneManager.LoadSceneAsync(sceneName) : SceneManager.LoadSceneAsync(sceneIndex);
        Application.backgroundLoadingPriority = ThreadPriority.Low;
        
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

