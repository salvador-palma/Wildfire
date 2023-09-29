using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Deck : MonoBehaviour
{
    public static Deck Instance {get; private set;}
    [SerializeField] List<Augment> augments;
    [SerializeField] GameObject[] Slots;
    [SerializeField] GameObject CanvasDeck;
    [SerializeField] private Augment[] currentAugments;
    private void Start() {
        Instance = this;
        currentAugments = new Augment[3];
        FillDeck();
    }

    void FillDeck(){
        
        augments.Add(new Augment("Target Practice", "Increase your accuracy by 15%", new UnityAction(()=> Flamey.Instance.addAccuracy(15))));
        augments.Add(new Augment("Steady Aim", "Increase your accuracy by 35%", new UnityAction(()=> Flamey.Instance.addAccuracy(35))));
        augments.Add(new Augment("Eagle Eye", "Double your current Accuracy", new UnityAction(()=> Flamey.Instance.multAccuracy(2))));

        augments.Add(new Augment("Swifty Flames", "Increase your attack speed by 50%", new UnityAction(()=> Flamey.Instance.multAttackSpeed(1.5f))));
        augments.Add(new Augment("Fire Dance", "Increase your attack speed by 125%", new UnityAction(()=> Flamey.Instance.multAttackSpeed(2.25f))));
        augments.Add(new Augment("Flamethrower", "Increase your attack speed by 200%", new UnityAction(()=> Flamey.Instance.multAttackSpeed(3f))));

        augments.Add(new Augment("Quick Shot", "Gain x1.25 Bullet Speed", new UnityAction(()=> Flamey.Instance.multBulletSpeed(1.25f))));
        augments.Add(new Augment("Fire-Express", "Gain x1.5 Bullet Speed", new UnityAction(()=> Flamey.Instance.multBulletSpeed(1.5f))));
        augments.Add(new Augment("HiperDrive", "Gain x2 Bullet Speed", new UnityAction(()=> Flamey.Instance.multBulletSpeed(2f))));

        augments.Add(new Augment("Warm Soup", "Heal 50% and gain +100 Max HP", new UnityAction(()=> Flamey.Instance.addHealth(100,0.5f))));
        augments.Add(new Augment("Sunfire Cape", "Heal 75% and gain +250 Max HP", new UnityAction(()=> Flamey.Instance.addHealth(250, 0.75f))));
        augments.Add(new Augment("Absolute Unit", "Heal to full HP and gain +500 Max HP", new UnityAction(()=> Flamey.Instance.addHealth(500,1f))));
    }

    Augment pickFromDeck(){
        if(augments.Count == 0){
            FillDeck();
        }
        Augment aug = augments[UnityEngine.Random.Range(0, augments.Count)];
        augments.Remove(aug);
        return aug;
    }
    void ChangeSlots(){
        for (int i = 0; i < 3; i++)
        {
            ChangeSingular(pickFromDeck(), Slots[i], i);
        }
    }
    void ChangeSingular(Augment augment, GameObject slot, int i){
        slot.transform.Find("Title").GetComponent<TextMeshProUGUI>().text = augment.Title;
        slot.transform.Find("Description").GetComponent<TextMeshProUGUI>().text = augment.Description;
        currentAugments[i] = augment;
        //slot.GetComponent<Button>().onClick.AddListener(augment.action);
    }

    public void PickedAugment(int i){
        CanvasDeck.SetActive(false);
        currentAugments[i].action();
        EnemySpawner.Instance.newRound();
    }

    public void StartAugments(){
        CanvasDeck.SetActive(true);
        ChangeSlots();
    }




}
