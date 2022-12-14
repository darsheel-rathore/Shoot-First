using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private int lobbySceneIndex, mpSceneIndex;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip lobbySceneClip, mpSceneClip;

    [Range(1f, 3f)]
    [SerializeField] private float transitionSpeed = 2.2f;
    [SerializeField] private float minVolume, maxVolume;
    [SerializeField] private float lerpThreshold = 0.05f;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            AudioManager.Instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneFinishedLoading;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneFinishedLoading;
    }

    private void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {

        int loadedSceneIndex = scene.buildIndex;
        if (loadedSceneIndex == lobbySceneIndex)
        {
            StartCoroutine(ChagneAudioToRespectedScene(lobbySceneClip));
        }
        else if (loadedSceneIndex == mpSceneIndex)
        {
            StartCoroutine(ChagneAudioToRespectedScene(mpSceneClip));
        }
    }

    private IEnumerator ChagneAudioToRespectedScene(AudioClip clipSelected)
    {
        yield return StartCoroutine(ReduceVolume());

        audioSource.clip = clipSelected;
        audioSource.Play();
        
        yield return StartCoroutine(IncreaseVolume());
    }

    private IEnumerator ReduceVolume()
    {
        while(audioSource.volume > (minVolume + lerpThreshold))
        {
            audioSource.volume = Mathf.Lerp(audioSource.volume, minVolume, transitionSpeed * Time.deltaTime);
            yield return null;
        }
    }

    private IEnumerator IncreaseVolume()
    {
        while(audioSource.volume < (maxVolume - lerpThreshold))
        {
            audioSource.volume = Mathf.Lerp(audioSource.volume, maxVolume, transitionSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
