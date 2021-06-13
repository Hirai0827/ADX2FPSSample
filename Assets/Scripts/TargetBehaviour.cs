using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class TargetBehaviour : MonoBehaviour,IShootableObject
{
    [SerializeField] private GameObject breakEffectPrefab;

    public void Shot()
    {
        Break();
    }
    
    private void Break()
    {
        var breakObject = Instantiate(breakEffectPrefab);
        ScoreController.Instance.AddScore();
        Destroy(breakObject,1.0f);
        Destroy(this.gameObject,0.1f);
        breakObject.transform.position = this.transform.position;
    }
}
