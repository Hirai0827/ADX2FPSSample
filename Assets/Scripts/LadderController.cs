using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class LadderController : MonoBehaviour
    {
        private bool isOnLadder = false;
        public bool IsOnLadder => isOnLadder;

        public void Jump()
        {
            isOnLadder = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Ladder"))
            {
                Debug.Log("Hang Ladder.");
                isOnLadder = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Ladder"))
            {
                Debug.Log("Release Ladder.");
                isOnLadder = false;
            }
        }
    }
}