using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public void LoadLevel(string level)
    {
        SceneManager.LoadScene(level);
    }
    
    public void LoadNextAfterDelay(float delaySeconds)
    {
        StartCoroutine(LoadSceneAfterDelay(delaySeconds));
    }
    
    IEnumerator LoadSceneAfterDelay(string name, float delaySeconds)
    {
        yield return new WaitForSeconds(delaySeconds);

        StartCoroutine(LoadAsync(name));
    }

    IEnumerator LoadSceneAfterDelay(float delaySeconds)
    {
        yield return new WaitForSeconds(delaySeconds);

        StartCoroutine(LoadAsync(SceneManager.GetActiveScene().buildIndex + 1));
    }

    IEnumerator LoadAsync(string sceneId)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneId);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }


    IEnumerator LoadAsync(int sceneId)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneId);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
