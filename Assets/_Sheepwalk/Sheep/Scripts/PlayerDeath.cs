using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace sheepwalk
{

    public class PlayerDeath : MonoBehaviour
    {
        public ScoreMenu scoreMenu;
        public void HandlePlayerDeath()
        {
            //Endgame screen
            if (scoreMenu != null) scoreMenu.StartCoroutine(scoreMenu.DisplayScore());
            //Respawn
            //Should probably lie on some respawn button
            //StartCoroutine(nameof(RespawnCoroutine));
        }

        private IEnumerator RespawnCoroutine()
        {
            yield return new WaitForSeconds(2.5f);
            Respawn();
        }

        private void Respawn()
        {
            var scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }

    }
}