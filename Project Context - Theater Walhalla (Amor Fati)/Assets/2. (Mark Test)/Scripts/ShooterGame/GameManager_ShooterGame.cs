﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PoolingAndAudio;

namespace ShooterGame
{
    public class GameManager_ShooterGame : GameManager
    {
        [HideInInspector] public ScoreManager ScoreManager;
        private ShooterController shooterController;
        private TargetManager targetManager;
        private UIManager uiManager;

        protected override void Awake()
        {
            base.Awake();
             
            shooterController = GetComponentInChildren<ShooterController>();
            targetManager = GetComponentInChildren<TargetManager>();
            ScoreManager = GetComponentInChildren<ScoreManager>();
            uiManager = GetComponentInChildren<UIManager>();
        }

        protected override void Start()
        {
            base.Start();

            shooterController.OnStart(objectPool);
            targetManager.OnStart(objectPool, ScoreManager, uiManager);
            ScoreManager.OnStart(uiManager);
        }

        protected override void Update()
        {
            base.Update();

            ScoreManager.OnUpdate();
        }

        public void QuitGame()
        {
            Application.OpenURL("https://forms.gle/YRoE2q7dxnu4Xoek9");
            //Application.Quit();
        }
    }
}
