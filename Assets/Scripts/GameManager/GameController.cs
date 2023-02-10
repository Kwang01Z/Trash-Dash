using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SPStudios.Tools;
public class GameController : ReRegisterMonoSingleton
{
    private void Start()
    {
        Application.targetFrameRate = 30;
    }
    public void LoadScence(string scenceName)
    {
        StartCoroutine(LoadAsynchronously(scenceName));
    }
    IEnumerator LoadAsynchronously(string scenceName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(scenceName);
        while (!operation.isDone)
        {
            yield return null;
        }
    }
    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else   
        Application.Quit();
#endif
    }
}
