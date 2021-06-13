using UnityEngine;

namespace DefaultNamespace
{
    public class WalkSoundController : MonoBehaviour
    {
        [SerializeField] private AudioSource normalStepSound;
        [SerializeField] private AudioSource glassStepSound;
        [SerializeField] private AudioSource waterStepSound;

        private void ChangeVolume()
        {
            
        }
    }
}