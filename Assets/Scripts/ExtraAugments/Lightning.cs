using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour
{
   

    private ParticleSystem partycleSystem;
    private ParticleSystem.Particle[] particles;
    [SerializeField] GameObject Prefab;
    bool check;
    private void Start() {
        partycleSystem = GetComponent<ParticleSystem>();
       
        particles = new ParticleSystem.Particle[partycleSystem.main.maxParticles];
    
    }

    private void LateUpdate() {
        if(check){return;}
        int n = partycleSystem.GetParticles(particles);
        
        for(int i=0; i<n; i++){ 
            if(particles[i].remainingLifetime <= 0){
                check =true;
                Vector2 WorldPos = transform.TransformPoint(particles[i].position);
                Collider2D[] targets = Physics2D.OverlapCircleAll(WorldPos, 0.5f, FlareManager.EnemyMask);
                if(targets.Length > 0){
                    GameObject ex = Instantiate(Prefab);
                    ex.transform.position = WorldPos;
                }
                foreach(Collider2D col in targets){
                    col.GetComponent<Enemy>().Hitted(LightningEffect.Instance.dmg, 6, ignoreArmor: true, onHit: false);
                }
                Flamey.Instance.ApplyOnLand(WorldPos);
            }
        }
    }

}
