using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace PoolingAndAudio
{
    public interface IPooledObject
    {
        string Key { get; set; }
        void OnObjectSpawn();
        void OnObjectDespawn();
        void SetUpOnDestruction(Action<string, GameObject> _action);
    }
}