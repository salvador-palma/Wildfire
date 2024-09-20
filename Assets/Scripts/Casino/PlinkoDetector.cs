using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlinkoDetector : MonoBehaviour
{
    public int PascalEntry;
    
    [SerializeField] Plinko plinko;
    private void Start() {
        GetComponentInChildren<TextMeshProUGUI>().text = "x"+plinko.getMultiplier(PascalEntry);
    }
    private void OnTriggerEnter2D(Collider2D other) {
        plinko.CheckEntry(PascalEntry);
        Destroy(other.gameObject);
    }
}
