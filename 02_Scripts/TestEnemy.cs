using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : BaseCreature
{
    // Start is called before the first frame update
    void Start()
    {
        stat = GetComponent<PlayerStat>();
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
