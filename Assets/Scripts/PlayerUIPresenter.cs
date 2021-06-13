using System;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class PlayerUIPresenter : MonoBehaviour
    {
        [SerializeField] private Text bulletText;
        [SerializeField] private Text currentTimeText;
        [SerializeField] private Text currentScoreText;
        [SerializeField] private Text currentStageText;

        public void UpdateBullet(int currentBullet,int maxBullet)
        {
            bulletText.text = currentBullet + "/" + maxBullet;
        }

        public void UpdateScore(int score)
        {
            currentScoreText.text = score.ToString();
        }

        public void UpdateStage(string stageName)
        {
            currentStageText.text = stageName;
        }
    }
}