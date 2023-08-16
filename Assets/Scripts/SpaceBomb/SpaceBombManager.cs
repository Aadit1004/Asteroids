using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceBombManager : MonoBehaviour
{

    [Range(1, 2)] public int maxBombs;
    private int currentBombs;
    private List<GameObject> lofBomb = new List<GameObject>();

    void Start()
    {
        currentBombs = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
