using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageChanger : MonoBehaviour
{
    [SerializeField] private string stageName;
    [SerializeField] private bool isNewScene;
    [SerializeField] private string sceneName;
    private void OnTriggerEnter(Collider other)
    {
        //TODO 侵入したときにステージの切り替えを行う.
        if (other.gameObject.CompareTag("Player"))
        {
            ScoreController.Instance.SetStage(stageName);
            if (isNewScene)
            {
                SceneManager.LoadScene(sceneName);
            }
        }
    }
}
