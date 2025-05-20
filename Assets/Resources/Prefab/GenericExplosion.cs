using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericExplosion : ExplosionOnDeath
{
   
    public override void Pool()
    {
        Timer = 1f;
        GetComponent<ParticleSystem>().Clear();
        GetComponent<ParticleSystem>().Play();
    }
}
