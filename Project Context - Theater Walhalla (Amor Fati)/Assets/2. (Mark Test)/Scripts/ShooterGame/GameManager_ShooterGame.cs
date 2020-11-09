using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PoolingAndAudio;

namespace ShooterGame
{
    public class GameManager_ShooterGame : GameManager
    {
        private ShooterController shooterController;

        protected override void Awake()
        {
            base.Awake();
             
            shooterController = GetComponentInChildren<ShooterController>();
        }

        protected override void Start()
        {
            base.Start();

            shooterController.OnStart(objectPool);
        }
    }
}
