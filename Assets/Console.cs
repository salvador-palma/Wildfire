using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Console : MonoBehaviour
{
    private static Console Instance;
    int i = 0;
    [SerializeField] TextMeshProUGUI consoletxt;
    void Awake()
    {
        Instance = this;
    }

    public static void Log(string str){
        //Instance.LogPrivate(str);
    }

    protected void LogPrivate(string str){
        consoletxt.SetText(i++ + ": " + str + "\n" + consoletxt.text);
    }
}
