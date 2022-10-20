using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PaperKiteStudio.DroppysWaterTrials
{ 
    public class EnemyAttack : MonoBehaviour
    {
        private PlayerStatus player;
        [SerializeField]
        private int damageAmount;

        private void Start()
        {
            player = GameObject.Find("Player").GetComponent<PlayerStatus>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(other.CompareTag("Player"))
            {
                player.Damage(damageAmount);
            }
        }
    }
}
