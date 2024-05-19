using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// To handle audio between scenes, hardcoded in a hurry. 
/// TODO: improve this feature of transition and handling sounds between scenes
/// </summary>.
public class PervasiveAudioBetweenScenes : GenericSingleton<PervasiveAudioBetweenScenes>
{
    private AudioClip _currentAudioClip;
    private float _currentVolume = 0.2f;
    [SerializeField] private AudioSource _audioSource;

    [SerializeField] private float maxVolume = 0.7f;
    [SerializeField] private float minVolume = 0.2f;

    [Space(height:30)]
    [Tooltip("Names of the scenes this object should stay alive in when transitioning into")]
    [SerializeField] private List<String> sceneNames;

    public override void Awake()
    {
        base.Awake();

        if (_audioSource != null)
        {
            _currentAudioClip = _audioSource.clip;
            _currentVolume = _audioSource.volume;
        }
    }

    private void Start() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void PlayMusic()
    {
        if (_audioSource != null && _currentAudioClip != null && !_audioSource.isPlaying)
        {
            _audioSource.volume = _currentVolume;
            _audioSource.clip = _currentAudioClip;
            _audioSource.Play();
        }
    }

    public void PauseMusic()
    {
        if (_audioSource != null && _audioSource.isPlaying)
        {
            _audioSource.Pause();
        }
    }

    public void StopMusic()
    {
        if (_audioSource != null)
        {
            _audioSource.Stop();
        }
    }

    public void SetVolume(float volume)
    {
        _currentVolume = volume;
        _audioSource.volume = _currentVolume;
    }

    public void SetMinVolume()
    {
        SetVolume(minVolume);
    }

    public void SetMaxVolume()
    {
        SetVolume(maxVolume);
    }

    public void ChangeAudioClip(AudioClip audioClip)
    {
        _currentAudioClip = audioClip;

        StopMusic();
    }

    public void ChangeTransform(Transform newPosition)
    {
        transform.position = newPosition.position;
        transform.rotation = newPosition.rotation;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // check if this object should be deleted based on the input scene names given 
        CheckIfSceneInList();
    }

    void CheckIfSceneInList()
    {
        // check what scene we are in and compare it to the list of strings 
        string currentScene = SceneManager.GetActiveScene().name;

        if (sceneNames.Contains(currentScene))
        {
            // keep the object alive 
        }
        else
        {
            // unsubscribe to the scene load callback
            SceneManager.sceneLoaded -= OnSceneLoaded;

            DestroyImmediate(this.gameObject);
        }
    }
}
