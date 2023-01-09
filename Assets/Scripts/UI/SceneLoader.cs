using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private Animator animator;

    [SerializeField] private bool dontDestroyOnLoad = false;
    [SerializeField] private bool loadSceneOnStart = false;
    [field: SerializeField] public int TargetScene { get; set; }

    bool isLoading = false;

    private void Start()
    {
        if (dontDestroyOnLoad)
        {
            DontDestroyOnLoad(gameObject);
        }

        if (loadSceneOnStart)
        {
            LoadScene(TargetScene);
        }
    }
    public void StartLoading()
    {
        if (isLoading) return;

        animator.SetTrigger("StartLoading");
    }

    public void LoadScene(string _name)
    {
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(_name);
    }

    public void LoadScene(int _index)
    {
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(_index);
    }

    public void ReloadScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
}
