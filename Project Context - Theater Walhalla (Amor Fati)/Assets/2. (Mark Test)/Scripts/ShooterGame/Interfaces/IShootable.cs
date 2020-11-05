using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShooterGame
{
    public interface IShootable
    {
        void OnShot(GameObject _shooter);
    }
}