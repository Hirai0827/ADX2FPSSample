using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public class BulletBehaviour : MonoBehaviour
    {
        [SerializeField] private GameObject holePrefab;
        [SerializeField] private GameObject shellPrefab;
        private GameObject shell;
        private bool isInitialized;
        private Vector3 rot;
        public void Init()
        {
            Destroy(this.gameObject,0.25f);
            shell = Instantiate(shellPrefab);
            shell.transform.position = transform.position;
            var shellRigid = shell.GetComponent<Rigidbody>();
            float theta = Random.Range(0.0f, 6.28f);
            shellRigid.velocity = Vector3.up * 1.5f+Vector3.right*Mathf.Cos(theta)+Vector3.forward*Mathf.Sin(theta);
            rot = new Vector3(Random.Range(0.0f, 200.0f), Random.Range(0.0f, 200.0f), Random.Range(0.0f, 200.0f));
            Destroy(shell,3.0f);
            ShotRay();
            isInitialized = true;
        }

        public void FixedUpdate()
        {
            if (isInitialized)
            {
                shell.transform.rotation =shell.transform.rotation * Quaternion.Euler(rot);
            }
        }

        private void ShotRay()
        {
            Physics.Raycast(transform.position,transform.rotation * Vector3.up,out RaycastHit hit);
            if (hit.point != Vector3.zero)
            {
                var hole = Instantiate(holePrefab).transform;
                hole.position = hit.point;
                Destroy(hole.gameObject,0.5f);
                var go = hit.collider.gameObject;
                var shootableObject = go.GetComponent<IShootableObject>();
                shootableObject?.Shot();
            }
        }
    }
}