using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShooterGame
{
    [RequireComponent(typeof(Animator))]
    public class Weight : MonoBehaviour, IShootable
    {
        [SerializeField] private GameObject target;

        private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public void ResetValues()
        {
            target.SetActive(true);
            animator.SetTrigger("Intro");
        }

        public void OnShot(GameObject _shooter)
        {
            animator.SetTrigger("FallDown");
            target.SetActive(false);
        }
    }
}
