using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{

    [SerializeField] private GameObject blockPrefab;
    private Vector3 blockposition;
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(blockPrefab, blockposition, Quaternion.identity);
    }

    // IEnumerator StartBlockGeneration()
    // {

    // }


    void CreateBlocks()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
