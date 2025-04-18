using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UI;

public class FrogJump : MonoBehaviour
{
    public GameObject Camera;
    public float YLIM;

    public float sideSpeed;
    public float mov;
    Rigidbody2D rb;


    public float PlatformDistance;
    public List<FrogJumpLog> Platforms;
    public List<GameObject> Flys;
    public List<GameObject> Spikes;
    public List<GameObject> SpikesRed;
    public GameObject PlatformPrefab;
    public GameObject FlyPrefab;
    public GameObject SpikePrefab;
    public GameObject SpikeRedPrefab;
    public Transform PlatformParent;

    public Button Left;
    public Button Right;
    public float MinimumPercentageDistanceReduced = 0.2f;
    
    public float LimitToTP = 9f;


    public Gradient GradientSky;
    public float maxSky= 1000;
    public float minSky = 0;

    Animator anim;
    SpriteRenderer sr;


    public GameObject FrogCanvas;
    public DynamicText EmberText;
    public DynamicText ScoreText;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        SpawnPlatforms();
        SetupGameData();

        
    }
    int buttonMove = 0;
    public void Move(int dir)
    {
        buttonMove += dir;
    }
    bool started = false;

    int score;
    int embersgained;
    int flycatched;

    private void SpawnPlatforms()
    {
        Platforms = new List<FrogJumpLog>();
        Flys = new List<GameObject>();
        Spikes = new List<GameObject>();
        for (int i = 0; i < 20; i++)
        {
            GameObject log = Instantiate(PlatformPrefab, PlatformParent);
            log.transform.position = new Vector3(UnityEngine.Random.Range(-8,8), i * UnityEngine.Random.Range(0.2f,.5f) * PlatformDistance, 0);
            
            Platforms.Add(log.GetComponent<FrogJumpLog>());

            GameObject fly = Instantiate(FlyPrefab, PlatformParent);
            fly.transform.position = new Vector3(UnityEngine.Random.Range(-8,8), i * UnityEngine.Random.Range(0.2f,.5f) * PlatformDistance * 5f, 0);
            Flys.Add(fly);

            GameObject spike = Instantiate(SpikePrefab, PlatformParent);
            spike.transform.position = new Vector3(UnityEngine.Random.Range(-5,5), i * UnityEngine.Random.Range(0.2f,.5f) * PlatformDistance * 15f + 500f, 0);
            Spikes.Add(spike);

            GameObject spikered = Instantiate(SpikeRedPrefab, PlatformParent);
            spikered.transform.position = new Vector3(UnityEngine.Random.Range(-2,2), i * UnityEngine.Random.Range(0.2f,.5f) * PlatformDistance * 15f + 1250f, 0);
            Spikes.Add(spikered);

        }
        
    }


    
    void LateUpdate()
    {
        if(!started){
            if(Camera.transform.position.y > 0){
                Camera.transform.position = Vector3.Lerp(Camera.transform.position, new Vector3(0, 0, Camera.transform.position.z), Time.deltaTime * 5f);
                float p1 = Mathf.Clamp(transform.localPosition.y / maxSky,0f,1f);
                Camera.GetComponent<Camera>().backgroundColor = GradientSky.Evaluate(p1);
            }
            return;
        }
        if(transform.position.y > Camera.transform.position.y + YLIM){
            Camera.transform.position = new Vector3(Camera.transform.position.x, transform.position.y, Camera.transform.position.z);
        }
        float p = Mathf.Clamp(score / maxSky,0f,1f);
        Camera.GetComponent<Camera>().backgroundColor = GradientSky.Evaluate(p);
        
    }
    
    private void Update() {
        if(!started){

            return;
        }
        mov = (Input.GetAxis("Horizontal") + buttonMove) * sideSpeed;
        CheckPlatforms();

        MinimumPercentageDistanceReduced = Math.Clamp(transform.position.y * 0.001f + 0.2f,0.2f,1);

        anim.SetBool("Up", rb.velocity.y > 0f);
        if((!sr.flipX && mov>0) || (sr.flipX && mov < 0)){sr.flipX = !sr.flipX;}
        
        if(mov > 0 && transform.position.x > LimitToTP){
            transform.position = new Vector3(-LimitToTP, transform.position.y, transform.position.z);
        }else if(mov < 0 && transform.position.x < -LimitToTP){
            transform.position = new Vector3(LimitToTP, transform.position.y, transform.position.z);
        }


        score = Math.Max(score, (int)Math.Max(0,(int)transform.position.y));
        embersgained = score + flycatched*50;
        EmberText.SetTextDirect("<sprite name=\"Ember\"> " + embersgained);
        ScoreText.SetTextDirect(score+"m");

        if(transform.position.y < Camera.transform.position.y - 10f){
            GameOver();
        }
        
    }

    private void GameOver()
    {
        embersgained = 0;
        
        flycatched = 0;

        started = false;
        transform.localPosition = new Vector2(-6.3f, -168.9f); 
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        rb.velocity = Vector2.zero;
        anim.Rebind();
        anim.Update(0f);
        FrogCanvas.GetComponent<Animator>().Play("GameOver");
        GameVariables.SetVariable("FrogJumpHighscore", score > GameVariables.GetVariable("FrogJumpHighscore") ? score : GameVariables.GetVariable("FrogJumpHighscore"));
        score = 0;
        SkillTreeManager.Instance.AddEmbers(embersgained);

        EmberText.SetTextDirect("<sprite name=\"Ember\"> " + SkillTreeManager.Instance.PlayerData.embers);
        ScoreText.SetText("Personal Best: {0}m", new string[]{GameVariables.GetVariable("FrogJumpHighscore").ToString()});

        for (int i = 0; i < 20; i++)
            {
                Platforms[i].transform.position = new Vector3(UnityEngine.Random.Range(-8,8), i * UnityEngine.Random.Range(0.2f,.5f) * PlatformDistance, 0);
                Platforms[i].VirtualStart(this);
                Flys[i].transform.position = new Vector3(UnityEngine.Random.Range(-8,8), i * UnityEngine.Random.Range(0.2f,1f) * PlatformDistance * 5f, 0);

            }

    }

    private void FixedUpdate() {
        Vector2 vel = rb.velocity;
        vel.x = mov;
        rb.velocity = vel;

    }
    public bool invicible;
    private void CheckPlatforms(){
        foreach(FrogJumpLog log in Platforms){
            if(log.transform.position.y < Camera.transform.position.y - 5.5f || log.repos){
                log.repos = false;
                FrogJumpLog Highest = Platforms.Where(p => !p.isRotten).OrderByDescending(p => p.transform.position.y).FirstOrDefault();
                log.transform.position = new Vector3(UnityEngine.Random.Range(-8f,8f), Highest.transform.position.y + UnityEngine.Random.Range(MinimumPercentageDistanceReduced,1f) * PlatformDistance, 0);

                log.VirtualStart(this);
            }
        }
        foreach(GameObject fly in Flys){
            if(fly.transform.position.y < Camera.transform.position.y - 5.5f){
                GameObject Highest = Flys.OrderByDescending(p => p.transform.position.y).FirstOrDefault();
                fly.transform.position = new Vector3(UnityEngine.Random.Range(-8f,8f), Highest.transform.position.y + UnityEngine.Random.Range(.2f,1f) * PlatformDistance, 0);
                fly.GetComponent<FrogJumpFly>().Reset();
            }
        }

        foreach(GameObject spike in Spikes.Where(s=>!s.GetComponent<FrogJumpSpike>().extraMoving)){
            if(spike.transform.position.y < Camera.transform.position.y - 5.5f){
                GameObject Highest = Spikes.Where(s=>!s.GetComponent<FrogJumpSpike>().extraMoving).OrderByDescending(p => p.transform.position.y).FirstOrDefault();
                spike.transform.position = new Vector3(UnityEngine.Random.Range(-4f,4f), Highest.transform.position.y + UnityEngine.Random.Range(.2f,1f) * PlatformDistance * 15f, 0);
                spike.GetComponent<FrogJumpSpike>().Reset();
            }
        }
        foreach(GameObject spike in Spikes.Where(s=>s.GetComponent<FrogJumpSpike>().extraMoving)){
            if(spike.transform.position.y < Camera.transform.position.y - 5.5f){
                GameObject Highest = Spikes.Where(s=>s.GetComponent<FrogJumpSpike>().extraMoving).OrderByDescending(p => p.transform.position.y).FirstOrDefault();
                spike.transform.position = new Vector3(UnityEngine.Random.Range(-2f,2f), Highest.transform.position.y + UnityEngine.Random.Range(.2f,1f) * PlatformDistance * 15f, 0);
                spike.GetComponent<FrogJumpSpike>().Reset();
            }
        }
       
    }
    
    public void StartGame(){
        if(started){return;}
        FrogCanvas.GetComponent<Animator>().Play("Intro");
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.constraints &= ~RigidbodyConstraints2D.FreezePosition;
        
        rb.velocity = new Vector2(0, 10);
        anim.Play("Jump");
        started = true;
        
        
    }
    

    private void SetupGameData()
    {
        EmberText.SetTextDirect("<sprite name=\"Ember\"> " + SkillTreeManager.Instance?.PlayerData.embers);
        int pb = GameVariables.GetVariable("FrogJumpHighscore");
        if(pb == -1){
            ScoreText.SetTextDirect("");
        }else{
            ScoreText.SetText("Personal Best: {0}m", new string[]{pb.ToString()});
        }
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Nature"){
            flycatched++;
            RespawnFly(other.gameObject);
        }else if(other.tag == "Prop" && !invicible){
            GameOver();
        }
    }
    private void RespawnFly(GameObject fly){
        GameObject Highest = Flys.OrderByDescending(p => p.transform.position.y).FirstOrDefault();
        fly.transform.position = new Vector3(UnityEngine.Random.Range(-8,8), Highest.transform.position.y + UnityEngine.Random.Range(.2f,1f) * PlatformDistance * 5f, 0);
        fly.GetComponent<FrogJumpFly>().Reset();
    }


    
}
