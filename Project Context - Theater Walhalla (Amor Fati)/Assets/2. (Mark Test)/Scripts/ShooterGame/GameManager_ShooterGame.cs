using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PoolingAndAudio;

namespace ShooterGame
{
    public class GameManager_ShooterGame : GameManager
    {
        private ShooterController shooterController;
        private TargetManager targetManager;

        protected override void Awake()
        {
            base.Awake();
             
            shooterController = GetComponentInChildren<ShooterController>();
            targetManager = GetComponentInChildren<TargetManager>();
        }

        protected override void Start()
        {
            base.Start();

            shooterController.OnStart(objectPool);
            targetManager.OnStart(objectPool);
        }
    }
}
