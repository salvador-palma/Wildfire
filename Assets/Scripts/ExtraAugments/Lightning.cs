using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
                Enemy[] targets = Physics2D.OverlapCircleAll(WorldPos, 0.5f, FlareManager.EnemyMask).Select(e => e.GetComponent<Enemy>()).ToArray();
                if(targets.Length > 0){
                    GameObject ex = Instantiate(Prefab);
                    ex.transform.position = WorldPos;
                }
                if(SkillTreeManager.Instance.getLevel("Thunder")>=2){
                    foreach(Enemy col in targets){
                        if(!col.canTarget()){continue;}
                        col.Hitted(LightningEffect.Instance.dmg, 6, ignoreArmor: false, onHit: false);
                        col.Stun(2f);
                    }
                }else{
                    foreach(Enemy col in targets){
                        col.Hitted(LightningEffect.Instance.dmg, 6, ignoreArmor: false, onHit: false);
                    }
                }
                
                if(SkillTreeManager.Instance.getLevel("Thunder")>=1){
                    Flamey.Instance.ApplyOnLand(WorldPos);
                }
                Flamey.Instance.ApplyOnLand(WorldPos);
                
            }
        }
    }

}
