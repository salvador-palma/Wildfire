using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Adamastor : Boss
{
    public Enemy Boat;
    public Animator WaterAnimator;
    public GameObject WaterPrefab;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        NextPhase();
    }

    public void NextPhase()
    {
        Phase++;
        switch (Phase)
        {
            // Phase 1: Commands Boats and Throws Them
            case 0:
                StartCoroutine(WaitForPhase(Phase1()));
                break;


            // Phase 2: Same attack but with Whirlpool
            case 1:

                StartCoroutine(WaitForPhase(Phase2()));
                break;

            // Phase 3: Summons Clouds/Thunder/Rain Spawn Radius Decreases; Clears Sky Eventually and Repeats
            case 2:
                StartCoroutine(WaitForPhase(Phase3()));
                break;
        }
    }
    float timerWhirl = 1f;
    float timerThunder = 10f;
    public override void UpdateEnemy()
    {
        if (Phase == 1)
        {
            if (timerWhirl <= 0f)
            {
                EnemySpawner.Instance.PresentEnemies.ForEach(e => { if (e != null && e.canTarget()) { e.KnockBack(Vector2.zero, retracting: true, 0.4f, time: 1f, angleMissStep: -65); } });
                timerWhirl = 1f;
            }
            else
            {
                timerWhirl -= Time.deltaTime;
            }

        }
        if (Phase == 2)
        {
            if (timerThunder <= 0f)
            {
                WaterAnimator.Play("Thunder");
                timerThunder = 25f;
            }
            else
            {
                timerThunder -= Time.deltaTime;
            }
        }
        
        
        base.UpdateEnemy();
    }
    private IEnumerator Phase1()
    {
        GameObject waterGO = Instantiate(WaterPrefab);
        WaterAnimator = waterGO.GetComponent<Animator>();

        yield return new WaitForSeconds(2f);

        while (Health > MaxHealth * 2f / 3f)
        {
            GetComponent<Animator>().Play("Roar");
            yield return new WaitForSeconds(2f);
            Roar(5);
            yield return new WaitUntil(() => boats.All(b => b == null) || Health <= MaxHealth * 2f / 3f);
        }
        
    }
    private IEnumerator Phase2()
    {
        WhirlPool(true);
        yield return new WaitForSeconds(2f);
        while (Health > MaxHealth / 3f)
        {
            GetComponent<Animator>().Play("Roar");
            yield return new WaitForSeconds(2f);
            Roar(7);

            yield return new WaitUntil(() => boats.All(b => b == null) || Health <=  MaxHealth / 3f);
        }
    }

    private IEnumerator Phase3()
    {


        while (Health > 0)
        {
            Storm(true);
            yield return new WaitForSeconds(2f);
            if (Health <= 0 || this == null) break;
            GetComponent<Animator>().Play("Roar");
            yield return new WaitForSeconds(2f);
            if (Health <= 0 || this == null) break;
            Roar(9);
            yield return new WaitUntil(() => boats.All(b => b == null) || Health <= 0);
            if (Health <= 0 || this == null) break;
            Storm(false);
            yield return new WaitForSeconds(10f + (EnemySpawner.Instance.RoundTotalSpentTime/30f));
            if (Health <= 0 || this == null) break;
            GetComponent<Animator>().Play("Roar");
            yield return new WaitForSeconds(2f);
            if (Health <= 0 || this == null) break;
            Roar(9);
            yield return new WaitUntil(() => boats.All(b => b == null) || Health <= 0);
        }
        Storm(false);
        
        
        
    }
    public override void Die(bool onKill = true)
    {

        SteamLeaderboardManager.UnlockAchievment("ADAMASTOR_BEATEN");

        WaterAnimator.Play("WaterAdamastorExit");
        boats.ForEach(b => {if (b != null) { b.Health = 0; } });

        if (Whirpool.Instance != null && GameVariables.hasQuest(48) )
        {
            GameUI.Instance.CompleteQuestIfHasAndQueueDialogue(48, "Juno", 3);
        }


        base.Die(onKill);
    }
    private void Storm(bool on)
    {
        WaterAnimator.Play(on ? "CloudsOn" : "CloudsOff");
    }
    private void WhirlPool(bool on)
    {
        Debug.Log("Whirlpool " + (on ? "on" : "off"));
        WaterAnimator.Play(on ? "WaterAdamastorWhirlOn" : "WaterAdamastorWhirlOff");
    }
    List<Boat> boats = new List<Boat>();
    public void Roar(int n)
    {
        boats = new List<Boat>();
        for (int i = 0; i < n; i++)
        {
            Enemy en = Instantiate(Boat);
            en.transform.position = EnemySpawner.Instance.getPointAngle(360f / n * i);
            en.CheckFlip();
            boats.Add(en as Boat);
        }
    }

    public IEnumerator WaitForPhase(IEnumerator phaseCor)
    {
        Debug.Log("Starting Phase " + Phase);
        Exception error = null;

        IEnumerator SafeWrapper()
        {
            while (true)
            {
                object current;
                try
                {
                    if (!phaseCor.MoveNext())
                        yield break;
                    current = phaseCor.Current;
                }
                catch (Exception ex)
                {
                    error = ex;
                    Debug.LogError("Error in Adamastor phase " + Phase + ": " + ex.Message);
                    yield break;
                }

                yield return current;
            }
        }
        try
        {
            yield return SafeWrapper();
        }
        finally
        {
            Debug.Log("Phase " + Phase + (error == null ? " complete" : " ended with error") + ", moving to next phase");
            NextPhase();
        }

    }
}
