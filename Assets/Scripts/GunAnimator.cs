using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class GunAnimator : MonoBehaviour
{
    private bool isAiming;
    private bool isShooting;
    private bool isReloading;
    private float swingAmount;
    [SerializeField] private Transform gunTransform;
    [SerializeField] private Vector3 stayingGunPos;
    [SerializeField] private Vector3 aimingGunPos;
    [SerializeField] private Vector3 stayGunRot;

    [SerializeField] private Vector3 reloadGunPos;

    [SerializeField] private Vector3 reloadGunRot;
    
    [SerializeField] private Vector3 shotDelta;

    [SerializeField] private float aimingDuration;
    [SerializeField] private float shotDuration;
    [SerializeField] private float recoilAmount;
    [SerializeField] private float recoilAttenuation;
    [SerializeField] private float swingScaleOnStay;
    [SerializeField] private float swingScaleOnAim;
    [SerializeField] private float swingAttenuation;
    private float shotTime;
    private float aimingTime;
    private float reloadTime;
    private Vector3 recoil;
    void FixedUpdate()
    {
        if (isShooting)
        {
            shotTime += Time.deltaTime;
            if (shotTime / shotDuration > 1.0f)
            {
                isShooting = false;
                shotTime = 0.0f;
            }
        }

        var shotT = shotTime / shotDuration;
        var delta = shotDelta * (1.0f - Mathf.Abs(0.5f - (1.0f - (1.0f - shotT)*(1.0f - shotT))) * 2.0f);
        delta += swingAmount * (isAiming ? swingScaleOnAim : swingScaleOnStay) * (Vector3.right * Mathf.Sin(Time.time*6.28f * 1.5f) - Vector3.up * Mathf.Abs(Mathf.Cos(Time.time*6.28f * 1.5f)));
        if (isAiming)
        {
            aimingTime += Time.deltaTime;
        }
        else
        {
            aimingTime -= Time.deltaTime;
        }
        if (isReloading)
        {
            reloadTime += Time.deltaTime;
            if (reloadTime > 2.0f)
            {
                isReloading = false;
                reloadTime = 0.25f;
            }
        }
        else
        {
            reloadTime -= Time.deltaTime;
            
        }

        reloadTime = Mathf.Max(0.0f, reloadTime);
        aimingTime = Mathf.Min(aimingDuration, Mathf.Max(0f, aimingTime));
        var aimT = aimingTime / aimingDuration;
        var pos = Vector3.Lerp(stayingGunPos, aimingGunPos, 1.0f - (1.0f - aimT)*(1.0f - aimT));
        pos += delta + recoil;
        pos = Vector3.Lerp(pos,reloadGunPos,Mathf.Clamp01(reloadTime * 4.0f));
        var rot = Vector3.Lerp(stayGunRot, reloadGunRot, Mathf.Clamp01(reloadTime * 4.0f));
        gunTransform.localPosition = pos;
        gunTransform.localEulerAngles = rot;
        recoil *= recoilAttenuation;
        swingAmount *= swingAttenuation;
    }

    public void Shot()
    {
        shotTime = 0.0f;
        isShooting = true;
        float theta = Random.Range(0.0f, 6.28f);
        recoil += (Vector3.right * Mathf.Cos(theta) + Vector3.up * (Mathf.Sin(theta) + 1.0f)) * recoilAmount;

    }

    public void MoveToAim()
    {
        isAiming = true;
    }

    public void MoveToStay()
    {
        isAiming = false;
    }

    public void MoveToReload()
    {
        
        isReloading = true;
    }

    public void RestoreFromReload()
    {
        isReloading = false;
    }

    public void SetSwingingAmount(float swingAmount)
    {
        this.swingAmount = swingAmount;
    }
}
