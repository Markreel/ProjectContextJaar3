using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PoolingAndAudio
{
    public interface IHasAudioComponent
    {
        AudioComponent AudioComponent { get; }
    }
}