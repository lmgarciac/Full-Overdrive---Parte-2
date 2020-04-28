using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Events;

public class Limit : MonoBehaviour
{

    private readonly ScoreEvent score = new ScoreEvent();
    // Start is called before the first frame update
    [SerializeField] private int player_ID;
    [SerializeField] private GameObject player;
    void Start()
    {

    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {

        if (player_ID == 1)
        {
            score.playerId=player_ID;
            score.playerPosition=player.transform.position;
            score.scoreData=1;
            EventController.TriggerEvent(score);
        }
        if (player_ID == 2)
        {
            score.playerId=player_ID;
            score.playerPosition=player.transform.position;
            score.scoreData=1;
            EventController.TriggerEvent(score);
        }
    }
}
