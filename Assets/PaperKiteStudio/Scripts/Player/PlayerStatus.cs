using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PaperKiteStudio.DroppysWaterTrials
{
    public class PlayerStatus : MonoBehaviour
    {
        [SerializeField]
        private int health;

        private void Start()
        {
            health = 100;
        }
        public void Damage(int damageAmount)
        {
            health -= damageAmount;
        }
    }
}