using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PoolingAndAudio
{
    public class ACP_SingleClip : AC_SingleClip, IPooledObject
    {
        public string Key { get; set; }
        public System.Action<string, GameObject> OnDestruction;


        public void OnObjectDespawn()
        {

        }

        public void OnObjectSpawn()
        {

        }

        public void SetUpOnDestruction(System.Action<string, GameObject> _action)
        {
            OnDestruction += _action;
        }
    }
}
