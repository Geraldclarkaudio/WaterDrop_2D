using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PaperKiteStudio.DroppysWaterTrials
{
    public class Goblin : EnemyBaseClass
    {
        public override void Start()
        {
            base.Start();
            state = State.Patrolling;
        }
    }
}
