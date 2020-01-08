using UnityEngine;
using System.Collections;
using System.Threading.Tasks;
using System;

public class ParticleStopAction : EffectStopAction
{
    void OnParticleSystemStopped()
    {
        Stop();
    }
}