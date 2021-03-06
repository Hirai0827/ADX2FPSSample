using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

public class CharacterController : MonoBehaviour
{
    private Transform transform;
    private Rigidbody rigidbody;
    private float swingAmount = 0.0f;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform cameraTransformY;
    [SerializeField] private Transform cameraTransformX;
    [SerializeField] private Transform gunTransform;
    [SerializeField] private OnGroundController onGroundController;
    [SerializeField] private LadderController ladderController;
    [SerializeField] private ParticleSystem shotParticle;
    [SerializeField] private GunAnimator gunAnimator;
    [SerializeField] private PlayerUIPresenter playerUIPresenter;
    [SerializeField] private AudioSource footStepSound;
    [SerializeField] private AudioSource reloadSound;
    private float coolDownTime;
    private float stepCoolDownTime;
    private float reloadTime;
    private float rotX;
    private float rotY;
    
    
    [SerializeField]
    private int maxBullet = 30;
    [SerializeField]
    private int currentBullet = 30;

    private void Start()
    {
        transform = this.gameObject.transform;
        rigidbody = this.gameObject.GetComponent<Rigidbody>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    

    private void Update()
    {
        Move();
        Rotate();
        Jump();
        swingAmount = Mathf.Max( 0.0f,swingAmount-Time.deltaTime * 2.0f);
        if (reloadTime > 0.0f)
        {
            reloadTime -= Time.deltaTime;
            return;
        }
        coolDownTime -= Time.deltaTime;
        //Swing();
        if (Input.GetMouseButton(0) && currentBullet != 0 && coolDownTime < 0.0f)
        {
            Shoot();
        }

        if (Input.GetMouseButtonDown(1))
        {
            gunAnimator.MoveToAim();
        }

        if (Input.GetMouseButtonUp(1))
        {
            gunAnimator.MoveToStay();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }
    }

    private void Rotate()
    {
        var mouseMoveX = Input.GetAxis("Mouse X");
        rotX += mouseMoveX * 2.0f;
        cameraTransformY.localRotation = Quaternion.Euler(0, rotX, 0) * Quaternion.LookRotation(Vector3.forward);
        var mouseMoveY = Input.GetAxis("Mouse Y");
        rotY += mouseMoveY * -2.0f;
        rotY = Mathf.Clamp(rotY,-90.0f,90f);
        cameraTransformX.localRotation = Quaternion.Euler(rotY, 0, 0) * Quaternion.LookRotation(Vector3.forward);
        
    }
    private void Move()
    {
        float moveSpeed = 5.0f;
        
        var forceVector = new Vector3();
        if (Input.GetKey(KeyCode.W))
        {
            forceVector += Vector3.forward;
        }

        if (Input.GetKey(KeyCode.A))
        {
            forceVector -= Vector3.right;
        }

        if (Input.GetKey(KeyCode.S))
        {
            forceVector -= Vector3.forward;
        }

        if (Input.GetKey(KeyCode.D))
        {
            forceVector += Vector3.right;
        }

        if (forceVector.x == 0 && forceVector.y == 0 && forceVector.z == 0)
        {
            rigidbody.velocity = Vector3.up * rigidbody.velocity.y;
            return;
        }

        if (forceVector.y == 0)
        {
            stepCoolDownTime -= Time.deltaTime;
            if (stepCoolDownTime < 0.0f)
            {
                stepCoolDownTime = 0.5f;
                footStepSound.Play();
            }
        }
        gunAnimator.SetSwingingAmount(1.0f);
        forceVector.Normalize();
        
        rigidbody.velocity = cameraTransformY.rotation * forceVector * moveSpeed + rigidbody.velocity.y * Vector3.up;
        if (ladderController.IsOnLadder)
        {
            transform.position = transform.position + Vector3.up * 2.5f * Time.deltaTime;
        }
        //rigidbody.AddForce(forceVector * moveSpeed);

    }

    private void Jump()
    {
        if (Input.GetKey(KeyCode.Space) && onGroundController.IsOnGround)
        {
            onGroundController.Jump();
            rigidbody.velocity = Vector3.up * 4.0f;
        }
    }

    private void Shoot()
    {
        currentBullet--;
        playerUIPresenter.UpdateBullet(currentBullet,maxBullet);
        gunAnimator.Shot();
        var bulletTransform = Instantiate(bulletPrefab).transform;
        bulletTransform.position = gunTransform.position;
        bulletTransform.rotation = gunTransform.rotation;
        var bulletBehaviour = bulletTransform.gameObject.GetComponent<BulletBehaviour>();
        bulletBehaviour.Init();
        coolDownTime = 0.1f;
        shotParticle.Emit(30);
    }

    private void Reload()
    {
        currentBullet = maxBullet;
        reloadSound.Play();
        gunAnimator.MoveToReload();
        reloadTime = 2.0f;
        playerUIPresenter.UpdateBullet(currentBullet,maxBullet);
    }

    public void GameClear()
    {
        playerUIPresenter.GameClear();
    }
}
