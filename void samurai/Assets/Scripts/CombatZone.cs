using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatZone : MonoBehaviour
{
    private List<EnemyController> enemiesInZone = new List<EnemyController>();

    public bool IsCombatActive => enemiesInZone.Count > 0;
    public bool IsCleared => enemiesInZone.Count == 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RegisterEnemy(EnemyController enemy)
    {
        if (!enemiesInZone.Contains(enemy))
            enemiesInZone.Add(enemy);
    }

    public void UnregisterEnemy(EnemyController enemy)
    {
        if (enemiesInZone.Contains(enemy))
            enemiesInZone.Remove(enemy);
    }
}
