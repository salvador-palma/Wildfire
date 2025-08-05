using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeeSelectMenu : MonoBehaviour
{

    [SerializeField] Bee[] beeTypes;
    [SerializeField] Button[] beeButtons;
    [SerializeField] DynamicText TitleBeeTxt;
    [SerializeField] DynamicText DescriptionBeeTxt;

    void DisplayText(int type)
    {
        TitleBeeTxt.SetText("{0} Bee", new string[]{beeTypes[type].Type});
        DescriptionBeeTxt.SetText(beeTypes[type].Description);

    }

    public void Hovering(int type)
    {
        DisplayText(type);
    }
    public void Clicked(int type)
    {

        Array.ForEach(beeButtons, e => e.interactable = false);
        EnemySpawner.Instance.Paused = false;
        GetComponent<Animator>().Play("BeeMenuOff");
        Summoner.Instance.removeWorkerBee();
        Summoner.Instance.addBee(1, type+1, false);

        Deck.StartRoundEvent();
        EnemySpawner.Instance.newRound();

    }
    public void StartButtons()
    {
        TitleBeeTxt.SetTextDirect("");
        DescriptionBeeTxt.SetTextDirect("");
        EnemySpawner.Instance.Paused = true;
        GetComponent<Animator>().Play("BeeMenuOn");
        Array.ForEach(beeButtons, e => e.interactable = true);
    }
}
