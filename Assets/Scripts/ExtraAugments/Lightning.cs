using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Lightning : IPoolable
{
   

    private ParticleSystem partycleSystem;
    private ParticleSystem.Particle[] particles;
    [SerializeField] GameObject Prefab;
    bool check;
    public float Timer = 1f;
    private void Start() {
        partycleSystem = GetComponent<ParticleSystem>();
       
        particles = new ParticleSystem.Particle[partycleSystem.main.maxParticles];
    
    }

    private void LateUpdate() {
        Timer-=Time.deltaTime;
        if(Timer <= 0){
            UnPool();
        }
        if(check){return;}
        int n = partycleSystem.GetParticles(particles);
        
        for(int i=0; i<n; i++){ 
            if(particles[i].remainingLifetime <= 0){
                check =true;
                Vector2 WorldPos = transform.TransformPoint(particles[i].position);
                Enemy[] targets = Physics2D.OverlapCircleAll(WorldPos, 0.5f, Flamey.EnemyMask).Select(e => e.GetComponent<Enemy>()).ToArray();
                if(targets.Length > 0){
                    GameObject ex = Instantiate(Prefab);
                    ex.transform.position = WorldPos;
                }
                
                if(SkillTreeManager.Instance.getLevel("Thunder")>=2){
                   
                    foreach(Enemy col in targets){
                        if(col==null || !col.canTarget()){continue;}
                        
                        col.Hitted(LightningEffect.Instance.dmg, 6, ignoreArmor: false, onHit: false);
                        col.Stun(1f, source:"Thunder");
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

    public override string getReference()
    {
        return "Thunder";
    }
    public override void Pool()
    {
        check = false;
        Timer = 1f;
        GetComponent<ParticleSystem>().Clear();
        GetComponent<ParticleSystem>().Play();
    }
    public override void Define(float[] args)
    {
        transform.position = new Vector2(args[0], args[1]);
    }
}
