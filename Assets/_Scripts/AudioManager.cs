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
            audioSource.clip = lobbySceneClip;
            audioSource.Play();
        }
        else if (loadedSceneIndex == mpSceneIndex)
        {
            audioSource.clip = mpSceneClip;
            audioSource.Play();
        }
    }
}
