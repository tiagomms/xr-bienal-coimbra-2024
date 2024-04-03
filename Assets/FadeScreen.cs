using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Events;

public class FadeScreen : MonoBehaviour
{
    [SerializeField] private bool _fadeOnStart = true;
    [SerializeField] private float _defaultFadeDuration = 3;
    [SerializeField] private Color _fadeColor = new Color(0,0,0,1);

    private Renderer _renderer;
    private static readonly int SHADER_COLOR = Shader.PropertyToID("_BaseColor");

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _renderer.material.SetColor(SHADER_COLOR, _fadeColor);
    }

    private void OnEnable()
    {
        AbstractPortalToSomeNewPlace.OnPortalActivated += FadeOutFromPortal;
    }

    private void OnDisable()
    {
        AbstractPortalToSomeNewPlace.OnPortalActivated -= FadeOutFromPortal;
    }

    private void Start()
    {
        if (_fadeOnStart)
        {
            FadeIn(_defaultFadeDuration);
        }
    }

    private void FadeOutFromPortal(PortalSettings portalSettings)
    {
        FadeOut(portalSettings.fadeAnimationDuration);
    }

    public void FadeIn(float duration = 0f)
    {
        Fade(1, 0, duration);
    }
    
    public void FadeOut(float duration = 0f)
    {
        Fade(0, 1, duration);
    }

    public void Fade(float alphaIn, float alphaOut, float duration)
    {
        float fadeDuration = Math.Abs(duration - (0f)) < 0.01 ? _defaultFadeDuration : duration;
        StartCoroutine(FadeRoutine(alphaIn, alphaOut, fadeDuration));
    }

    private IEnumerator FadeRoutine(float alphaIn, float alphaOut, float duration)
    {
        float timer = 0;
        Color newColor = _fadeColor;

        while (timer <= duration)
        {
            newColor.a = Mathf.Lerp(alphaIn, alphaOut, timer / duration);

            _renderer.material.SetColor(SHADER_COLOR, newColor);
            timer += Time.deltaTime;
            yield return null;
        }

        newColor.a = alphaOut;
        _renderer.material.SetColor(SHADER_COLOR, newColor);
    }
}
