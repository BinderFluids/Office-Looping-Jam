using Core.Managers;
using UnityEngine;
using UnityEngine.UI;
using Eflatun.SceneReference;
using Cysharp.Threading.Tasks;

namespace UI
{


    public class SceneButton : MonoBehaviour
    {
        [SerializeField] private SceneReference scene;

        public void Load()
        {
            SceneLoader.Instance.LoadScene(scene).Forget();
        }
    }
}