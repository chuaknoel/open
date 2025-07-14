using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDamaged : MonoBehaviour
{
    public GameObject enemy;

    private Enemy enemyScript;

    private void Start()
    {
        enemyScript = enemy.GetComponent<Enemy>();    
    }

    private void Update()
    {
        
    }

    private void TestTakeDamaged()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
        }
    }
    
}
