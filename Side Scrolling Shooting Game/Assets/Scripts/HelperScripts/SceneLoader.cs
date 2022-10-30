using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
public class SceneLoader : GenericSingleton<SceneLoader>
{
    private void Start() {
        DontDestroyOnLoad(this);
    }
    public void LoadScene(string name)
    {
        StartCoroutine(Load(name));
    }

    private IEnumerator Load(string name)
    {
        Debug.Log("Loading scene: " + name);
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(name);
        loadOperation.allowSceneActivation = false;

        while(loadOperation.isDone == false)
        {
            Debug.Log("Load progress : " + loadOperation.progress);
            if (loadOperation.progress >= 0.9f)
            {
                loadOperation.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}
