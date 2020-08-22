using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using tpopl001.Questing;

namespace tpopl001.Util
{
    //loads a scene when the player hits its collider
    //enables navigation with no controller
    public class OpenSceneTrigger : MonoBehaviour
    {
        [SerializeField] private string scene;

        private void Start()
        {
            if (string.IsNullOrEmpty(scene))
            {
                scene = StatsManager.instance.GetScene();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag.Equals("Player"))
            {
                SceneManager.LoadScene(scene);
            }
        }
    }
}
