using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;

public class CantPassBehaviour : MonoBehaviour
{
    [SerializeField] private int thresholdScore;
    [SerializeField] private Text thresholdScoreText;
    private ScoreController scoreController;
    private void Start()
    {
        scoreController = ScoreController.Instance;
        thresholdScoreText.text = thresholdScore.ToString();
    }

    private void FixedUpdate()
    {
        if (scoreController.Score >= thresholdScore)
        {
            Destroy(this.gameObject);
        }
    }
}
