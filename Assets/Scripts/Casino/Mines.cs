using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Mines : MonoBehaviour
{
    [Header("References")]
    [SerializeField] TextMeshProUGUI MineAmountText;
    [SerializeField] TMP_InputField WageAmountText;
    [SerializeField] GameObject InGamePanel;
    [SerializeField] Transform grid;
    [SerializeField] TextMeshProUGUI MultiplierTxt;
    [SerializeField] TextMeshProUGUI CashOutTxt;

    [Header("Values")]
    [SerializeField] private int bet;
    [SerializeField] private int mineAmount;
    [SerializeField] private int[] mines;
    [SerializeField] private int clearedSlots;
    [SerializeField] private float multiplier;
    [SerializeField] private bool inGame;
    private void Start() {
        WageAmountText.text = "1000";

        Debug.Log("Winnging Probability: " + calculateWinningProbability(3,3));
        Debug.Log("Multiplier: " + calculateMultiplier(calculateWinningProbability(3,3)));
    }

    public float calculateWinningProbability(int spaces_cleared, int bombs){
        float winningProbability = 1f;
        for (int i = 0; i < spaces_cleared; i++)
        {
            winningProbability *= (25f-bombs-i)/(25f-i);
        }
        return winningProbability;
    }
    public float calculateMultiplier(float winningProbability){
        return Mathf.Round(.97f * 1f/winningProbability * 100f) * 0.01f;
    }

    public void StartGame(){
        if(inGame){return;}
        inGame=true;
        UpdateSetupUI(true);
        SetupGame();
        
    }

    private void SetupGame()
    {
        ClearPreviousGame();
        bet = int.Parse(WageAmountText.text);
        clearedSlots = 0;
        mines = new int[mineAmount];
        multiplier=0;
        MultiplierTxt.text = "x1";
        CashOutTxt.text = bet.ToString();

        for (int i = 0; i < mineAmount; i++)
        {
            int newValue = UnityEngine.Random.Range(0, 25);
            while(mines.Contains(newValue)){
                newValue = UnityEngine.Random.Range(0, 25);
            }
            mines[i] = newValue;
        }

    }

    private void ClearPreviousGame()
    {
        foreach (Transform pot in grid)
        {
            pot.GetComponent<Animator>().Play("Default");
        }
    }

    //====== SETUP GAME SECTION =======

    public void changeMineAmount(int dir){
        if(inGame){return;}
        mineAmount += dir;
        if(mineAmount<1){mineAmount=24;}
        if(mineAmount>24){mineAmount=1;}
        MineAmountText.text = mineAmount.ToString();
    }
    public void UpdateSetupUI(bool On){
        WageAmountText.interactable = !On;
        InGamePanel.SetActive(On);
    }

    //====== IN GAME SECTION =======

    public void Clicked(int ID){
        if(!inGame){return;}
        if(mines.Contains(ID)){
            grid.GetChild(ID).GetComponent<Animator>().Play("EaterPopUp");
            Debug.Log("Lost: -" + bet + " Embers");
            EndGame();
        }else{
            grid.GetChild(ID).GetComponent<Animator>().Play("FlowerPopUp");
            IncrementClearSlots();
        }
    }

    private void IncrementClearSlots()
    {
        clearedSlots++;
        multiplier = calculateMultiplier(calculateWinningProbability(clearedSlots,mineAmount));
        MultiplierTxt.text = multiplier.ToString();
        CashOutTxt.text = Math.Round(bet*multiplier).ToString();

    }
    public void Cashout(){
        Debug.Log("Won: +" + Math.Round(bet*multiplier) + " Embers");
        EndGame();
    }

    private void EndGame(){
        UpdateSetupUI(false);
        inGame = false;
    }
}
