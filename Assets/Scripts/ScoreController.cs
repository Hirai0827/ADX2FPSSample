using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class ScoreController : SingletonMonoBehaviour<ScoreController>
    {
        [SerializeField] private int score;
        [SerializeField] private string currentStage;

        public string CurrentScene => currentStage;

        [SerializeField]
        private PlayerUIPresenter playerUIPresenter;
        public int Score => score;

        public void Start()
        {
            DontDestroyOnLoad(this.gameObject);
        }

        public void AddScore()
        {
            score++;
            playerUIPresenter.UpdateScore(score);
        }

        public void SetStage(string stageName)
        {
            currentStage = stageName;
            playerUIPresenter.UpdateStage(stageName);
        }
    }
}