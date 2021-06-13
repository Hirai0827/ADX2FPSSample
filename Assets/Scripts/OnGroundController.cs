using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class OnGroundController : MonoBehaviour
    {
        private bool isOnGround = true;
        public bool IsOnGround => isOnGround;

        public void Jump()
        {
            isOnGround = false;
        }

        private void OnTriggerStay(Collider other)
        {
            if (!other.CompareTag("Player"))
            {
                isOnGround = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Player"))
            {
                isOnGround = false;
            }
        }
    }
}