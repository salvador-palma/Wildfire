using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnityResonance;
using Unity.VisualScripting;
using UnityEngine;

public abstract class IAlliedObject : IPoolable
{
    public void KnockBack(Vector2 origin, bool retracting, float power, float time = 0.5f){
        StartCoroutine(KnockBackCouroutine(origin, retracting, power, time));
    }
    protected virtual IEnumerator KnockBackCouroutine(Vector2 origin, bool retracting, float power, float timer = 0.5f){
  

        Vector2 diff = (Vector2)transform.position - origin;
        diff.Normalize();
        diff *= retracting ? -1 : 1;

        while(timer > 0){
            timer -= Time.deltaTime;
            transform.position = (Vector2)transform.position + diff * Time.deltaTime * power;
            yield return null;

        }
       
    }
}
public class BurnAOE : IAlliedObject
{
    public int Damage;
    public float Timer = 1f;
    float t;
    public float LastingTime;
    float lt;
    List<Enemy> colliding;
    public bool EverLasting;
    static int poolID;
    int selfID;
    static Material[] materials;

    private EventInstance lavaSoundInstance;
    private void Awake()
    {
        selfID = poolID++;
        colliding = new List<Enemy>();
        if (materials == null)
        {
            materials = new Material[10];
            for (int i = 0; i < 10; i++)
            {
                SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
                materials[i] = new Material(renderer.material);
                materials[i].SetVector("_RandomVec", new Vector2(UnityEngine.Random.Range(-100f, 100f), UnityEngine.Random.Range(-100f, 100f)));
            }
        }
        else
        {
            SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
            renderer.material = materials[selfID % 10];
        }



    }
    void Update()
    {
        t -= Time.deltaTime;

        if (t <= 0)
        {
            t = Timer;
            try
            {
                if (colliding.Count > 0)
                {
                    AudioManager.PlayOneShot(FMODEvents.Instance.Sizzle, Vector2.zero);
                }

                if (SkillTreeManager.Instance.getLevel("Lava Pool") >= 2)
                {
                    foreach (Enemy item in colliding)
                    {
                        if (item == null) { continue; }
                        item.Hitted(EverLasting ? BurnOnLand.Instance.damage : Damage, 9, ignoreArmor: false, onHit: false, source: "Lava Pool");
                        item.Armor -= Math.Max(0, (int)(item.Armor * .02f));
                    }
                }
                else
                {
                    foreach (Enemy item in colliding)
                    {
                        if (item == null) { continue; }

                        item.Hitted(Damage, 9, ignoreArmor: false, onHit: false, source: "Lava Pool");

                    }
                }

            }
            catch (InvalidOperatorException e)
            {
                Debug.Log(e);
            }

        }
        if (!EverLasting)
        {
            lt -= Time.deltaTime;
            if (lt <= 0)
            {
                lt = LastingTime;
                GetComponent<Animator>().Play("EndAOEBurn");
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Enemy")
        {
            colliding.Add(collider.GetComponent<Enemy>());
        }
    }
    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag == "Enemy")
        {
            colliding.Remove(collider.GetComponent<Enemy>());
        }
    }
    public override void Pool()
    {
        colliding = new List<Enemy>();
        lt = BurnOnLand.Instance.lasting;
        Damage = BurnOnLand.Instance.damage;
        if (!EverLasting)
        {
            Vector2 scale = new Vector2(0.2232152f, 0.2232152f) * BurnOnLand.Instance.size;
            transform.localScale = scale;
        }
        if (EnemySpawner.Instance.isOnAugments) { UnPool(); }

        Color c = GetComponent<SpriteRenderer>().color;
        c.a = 1;
        GetComponent<SpriteRenderer>().color = c;

        lavaSoundInstance = AudioManager.CreateInstance(FMODEvents.Instance.Lava);
        lavaSoundInstance.start();

        int n = ObjectPooling.ActiveAmount(this) + 1;

        if (n >= 100 && GameVariables.hasQuest(13))
        {

            GameUI.Instance.CompleteQuestIfHasAndQueueDialogue(13, "Rowl", 14);

        }
    }
    public override string getReference()
    {
        return "Lava Pool";
    }
    public override void Define(float[] args)
    {
        transform.position = new Vector2(args[0], args[1]);
    }
    public override void UnPool()
    {
        Renderer renderer = gameObject.GetComponent<Renderer>();
        renderer.material.SetVector("_RandomVec", new Vector2(UnityEngine.Random.Range(-100f, 100f), UnityEngine.Random.Range(-100f, 100f)));


        lavaSoundInstance.stop(STOP_MODE.ALLOWFADEOUT);
        lavaSoundInstance.release();

        base.UnPool();
    }



}
