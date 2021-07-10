using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class TargetBehaviour : MonoBehaviour,IShootableObject
{
    [SerializeField] private GameObject breakEffectPrefab;
    [SerializeField] private AudioSource audioSource;

    public void Shot()
    {
        Break();
    }
    
    private void Break()
    {
        var breakObject = Instantiate(breakEffectPrefab);
        ScoreController.Instance.AddScore();
        audioSource.Play();
        Destroy(breakObject,1.0f);
        Destroy(this.gameObject,0.1f);
        breakObject.transform.position = this.transform.position;
    }
}
