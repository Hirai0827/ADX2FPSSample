using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class Barrel : MonoBehaviour,IShootableObject
{
    [SerializeField] private GameObject BombEffectPrefab;
    private bool isBombed = false;
    public void Shot()
    {
        if (isBombed)
        {
            return;
        }
        isBombed = true;
        var bombEffect = Instantiate(BombEffectPrefab);
        var bombTransform = bombEffect.transform;
        bombTransform.position = transform.position;
        Destroy(bombEffect,1f);
        Destroy(this.gameObject,0f);
        var hits = Physics.SphereCastAll(transform.position,2.0f,Vector3.back,0.00001f);
        foreach (var hit in hits)
        {
            var go = hit.collider.gameObject;
            if (go != this.gameObject && go.TryGetComponent<IShootableObject>(out var shootableObject))
            {
                shootableObject.Shot();
            }
        }
    }
}
