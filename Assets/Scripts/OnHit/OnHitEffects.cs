using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public interface OnHitEffects
{
   public void ApplyEffect(float dmg = 0, float health = 0);

}
public class VampOnHit : OnHitEffects
{
    private float perc;
    public VampOnHit(float perc){
        this.perc = perc;
    }
    public void ApplyEffect(float dmg, float health = 0)
    {
        Flamey.Instance.addHealth(0, dmg * perc);
    }
}
