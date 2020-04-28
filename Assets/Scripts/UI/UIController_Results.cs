using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIController_Results : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI tx_scoreP1;
    [SerializeField] private TextMeshProUGUI tx_scoreP2;
    [SerializeField] private TextMeshProUGUI tx_winID;


    void Start()
    {
        tx_scoreP1.text = $"{UpdatedScores.p1score}";
        tx_scoreP2.text = $"{UpdatedScores.p2score}";
        tx_winID.text = $"P{UpdatedScores.winplayerID}";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
