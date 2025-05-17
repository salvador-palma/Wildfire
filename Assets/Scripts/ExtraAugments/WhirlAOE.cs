using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WhirlAOE : IPoolable
{
    public float Timer = 1f;
    public float t;
    public float force;
    float lt = 2f;
    List<Enemy> colliding;

    public bool isCarnivore;

    float size = 0.2232152f;

    public int merges = 0;

    //CHARACTER
    float MegaWhirlPoolTime = 3f;



    void Update()
    {
        t -= Time.deltaTime;
        lt -= Time.deltaTime;
        if (EnemySpawner.Instance.isOnAugments) { UnPool(); }
        if (t <= 0)
        {
            t = Timer;
            try
            {
                bool isDamaging = SkillTreeManager.Instance.getLevel("Whirlpool") >= 2;
                foreach (Enemy item in colliding)
                {
                    if (item == null || !item.canTarget()) { continue; }
                    item.KnockBack(transform.position, retracting: true, force, Timer);
                    if(isDamaging){
                        item.Hitted(10 + 1 * merges, 8, false, false);

                    }
                }
                
                
                CheckForOverlapAndStack();

            }
            catch (Exception e)
            {
                Debug.Log(e.StackTrace);
            }

        }
        if (lt <= 0)
        {
            GetComponent<Animator>().Play("EndWhirl");
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
       
        lt = 2f;
        force = Whirpool.Instance.force;
        size = 0.2232152f;
        Vector2 scale = new Vector2(size, size);
        transform.localScale = scale;

        foreach (Transform child in transform.GetChild(0))
        {
            child.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        }

        merges = 0;

        if (EnemySpawner.Instance.isOnAugments) { UnPool(); }
        CheckForOverlapAndStack();
    }
    public bool CheckedMax = false;
    public void Stack(WhirlAOE whirl)
    {
        if (lt <= 0) { return; }

        bool thisBetter = whirl.merges <= merges;
        if (!thisBetter)
        {
            whirl.Stack(this);
            return;
        }
        merges++;
        lt += 2f;
        lt = Mathf.Clamp(lt, 0f, 20f);
        size += whirl.size * 0.1f;
        size = Mathf.Clamp(size, 0.2232152f, 2f);
        Vector2 scale = new Vector2(size, size);
        transform.localScale = scale;
        whirl.UnPool();

        if (size >= 2f && Character.Instance.isCharacter("Whirlpool") && !CheckedMax)
        {
            CheckedMax = true;
            StartCoroutine(FadeInOutMegaWhirlPool());

        }
    }
   
    public IEnumerator FadeInOutMegaWhirlPool()
    {

        lt = 20f;
        SpriteRenderer spriteRenderer = transform.GetChild(1).GetComponent<SpriteRenderer>();
        transform.GetChild(1).GetComponent<Animator>().SetBool("MegaWhirlPoolOn", true);
        float time = 0f;
        while (time < 2.5f)
        {
            float a = time * .4f;
            spriteRenderer.color = new Color(1, 1, 1, a);
            time += Time.deltaTime;
            yield return null;

        }
        spriteRenderer.color = new Color(1, 1, 1, 1);
        

        yield return new WaitForSeconds(10f);
        colliding.ForEach(x => x.Health = 0);
        while (time < 5f)
        {
            float a = (5f - time) * .4f;
            spriteRenderer.color = new Color(1, 1, 1, a);
            time += Time.deltaTime;
            yield return null;
        }

        spriteRenderer.color = new Color(1, 1, 1, 0);
        lt = 0f;
    }
    public override string getReference()
    {
        return "WhirlPool";
    }
    public override void Define(float[] args)
    {
        transform.position = new Vector2(args[0], args[1]);

    }

    private void CheckForOverlapAndStack()
    {
        CapsuleCollider2D capsule = GetComponent<CapsuleCollider2D>();
        Collider2D[] overlaps = Physics2D.OverlapCapsuleAll(
            capsule.bounds.center,
            capsule.size,
            capsule.direction,
            0f // angle
        );
        
        foreach (Collider2D col in overlaps)
        {
            if (col == capsule) continue;

            WhirlAOE otherWhirlpool = col.GetComponent<WhirlAOE>();
            if (otherWhirlpool != null)
            {

                otherWhirlpool.Stack(this);

                return;
            }
            else if (SkillTreeManager.Instance.getLevel("Whirlpool") >= 1 && col.tag == "AlliedObjects")
            {
                IAlliedObject item = col.GetComponent<IAlliedObject>();
                if (item == null) { continue; }
                item.KnockBack(transform.position, retracting: true, force, Timer);
            
            }
        }
    }
    public override void UnPool()
    {
        merges = 0;
        transform.GetChild(1).GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        transform.GetChild(1).GetComponent<Animator>().SetBool("MegaWhirlPoolOn", false);
        size = 0.2232152f;
        CheckedMax = false;
        base.UnPool();
    }
    
    

}
