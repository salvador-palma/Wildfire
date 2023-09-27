using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rabbit : Enemy
{
    
    private void Start() {
        base.flame = Flamey.Instance;
        Speed = 0f;
        Health = 20;
    }
    private void Update() {
        base.Move();
    }
    
}
