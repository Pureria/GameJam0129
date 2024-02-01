using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zombies.Manager;

namespace Zombies.Title
{
    public class TitleScript : MonoBehaviour
    {
        [SerializeField] private float _scTime;
        
        public void SceneChangeGame()
        {
            SceneManager.ChangeSceneWait(2, SceneChangeEffect.Fade, _scTime);
        }
    }
}
