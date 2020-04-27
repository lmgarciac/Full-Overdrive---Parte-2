using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Events;

public class QTEState : State
{
    private GameObject instantiatedQTE;    
    private Vector3 positionQTE;
    private GameObject QTEprefab;

    public override void InitState() { //Preguntar a Pablo como resolver esto

        //QTEprefab = (GameObject)Resources.Load("Prefabs/Qte", typeof(GameObject));
        //instantiatedQTE = (GameObject)Instantiate(QTEprefab, positionQTE, Quaternion.identity);
    }

    public override void UpdateState(float delta) {
                
    }

    public override void ExitState() {
        if (instantiatedQTE != null) //
        {
            //Destroy(instantiatedQTE);
        }           
    }
}
