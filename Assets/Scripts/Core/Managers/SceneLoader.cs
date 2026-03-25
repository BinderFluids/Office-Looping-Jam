using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Eflatun.SceneReference;
using EventBus;
using Events;

namespace Core.Managers
{
    public class SceneLoader : PersistentSingleton<SceneLoader> {
        [SerializeField] private List<SceneReference> sceneReferences = new List<SceneReference>();
        
        public async UniTask LoadScene(SceneReference scene, LoadSceneMode mode = LoadSceneMode.Single)
        {
            EventBus<SceneLoadingEvent>.Raise(new SceneLoadingEvent()
            {
                sceneName = scene.Name
            });
            
            await SceneManager.LoadSceneAsync(scene.Path, mode);
            
            EventBus<SceneLoadedEvent>.Raise(new SceneLoadedEvent()
            {
                sceneName = scene.Name
            });
        }
    }
}