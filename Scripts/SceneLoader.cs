using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [ReadOnly][SerializeField] private string gameScene = "GameScene";
    [SerializeField] private SplashScreen splashScreen;
    
    private void Start()
    {
        Application.targetFrameRate = 60;
        LoadGameScene();
    }

    private void LoadGameScene()
    {
        StartCoroutine(LoadAsyncSceneAdditive(gameScene));
    }

    IEnumerator LoadAsyncSceneAdditive(string scene)
    {
        var async = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
        
        
        async.allowSceneActivation = false;
        while (!async.isDone)
        {
            if (async.progress >= .9f) async.allowSceneActivation = true;
            yield return null;
        }

        yield return new WaitForSeconds(2f);

        var tween = splashScreen.FadeOut();
        yield return tween.WaitForCompletion();

            // For lightining
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(gameScene));
    }
    
}
