using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PaperKiteStudio.DroppysWaterTrials
{
    public class SpecialAttackHandler : MonoBehaviour
    {
        [SerializeField]
        private Animator _anim;
        [SerializeField]
        private Animator _anim2;
        [SerializeField]
        private SpriteRenderer _renderer;

        [SerializeField]
        private GameObject projectile;

        public bool flipped = false;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (flipped == true)
                {
                    _anim.SetTrigger("Attack1Flipped");
                }
                else if (flipped == false)
                {
                    _anim.SetTrigger("Attack1");

                }
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                if (flipped == true)
                {
                    _anim.SetTrigger("Attack2Flipped");
                }
                else if (flipped == false)
                {
                    _anim.SetTrigger("Attack2");
                }
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                _anim.SetTrigger("Attack3");
                _anim2.SetTrigger("Attack3");
            }

            if (Input.GetKey(KeyCode.R))
            {
                if(flipped == false)
                {
                    _anim.SetBool("Attack4Holding", true);
                    _anim2.SetBool("Attack4Holding", true);
                }
                else if(flipped == true)
                {
                    _anim.SetBool("Attack4Holding_Flipped", true);
                    _anim2.SetBool("Attack4Holding_Flipped", true);
                }
                
            }

            else if (Input.GetKeyUp(KeyCode.R))
            {
                if(flipped == false)
                {
                    _anim.SetBool("Attack4Holding", false);
                    _anim2.SetBool("Attack4Holding", false);
                }
                else if(flipped == true)
                {
                    _anim.SetBool("Attack4Holding_Flipped", false);
                    _anim2.SetBool("Attack4Holding_Flipped", false);
                }
                
                Instantiate(projectile, transform.position, Quaternion.identity);
            }
        }

        public void Flip(bool faceRight)
        {
            if (faceRight == true)
            {
                _renderer.flipX = false;
                flipped = false;
            }
            else if (faceRight == false)
            {
                _renderer.flipX = true;
                flipped = true;
            }
        }

    }
}
