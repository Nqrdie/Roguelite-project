using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : Enemy
{
    private void Start()
    {
        
    }
    protected override IEnumerator Attacking()
    {
        // Shoots player
        if (Vector3.Distance(enemyTransform.position, playerTransform.position) > attackRange)
        {
            //SwitchStates(States.chasing);
        }
        yield return new WaitForEndOfFrame();
        // Switched to chasing when too far away from player, and switches to running when too close
    }
}
