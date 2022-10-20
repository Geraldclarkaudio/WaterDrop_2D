using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ParticleSystemJobs;

namespace PaperKiteStudio.DroppysWaterTrials
{
    public class RigidBodyMovement : MonoBehaviour
    {
        private Rigidbody2D rb;

        [SerializeField]
        private float _speed = 5.0f;
        [SerializeField]
        private float _gravity = 1.0f;
        [SerializeField]
        private float _jumpHeight = 15.0f;
        [SerializeField]
        private float _yVelocity;
        [SerializeField]
        private bool isGrounded;
        [SerializeField]
        private bool canDoubleJump = false;

        private Animator _anim;
        private SpriteRenderer _renderer;

        private SpecialAttackHandler specialAttack;

        private int playerLyer = 8;
        private int enemyLayer = 9;

        [Header("SpecialEffects")]
        [SerializeField]
        private GameObject jumpEffect;
        [SerializeField]
        private ParticleSystem landEffect;
 

        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            _anim = GetComponentInChildren<Animator>();
            _renderer = GetComponentInChildren<SpriteRenderer>();
            specialAttack = GetComponentInChildren<SpecialAttackHandler>();
            
            Physics2D.IgnoreLayerCollision(playerLyer, enemyLayer);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if(other.collider.CompareTag("Ground"))
            {
                isGrounded = true;
                landEffect.Play();
            }
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if (other.collider.CompareTag("Ground"))
            {
                isGrounded = false;
                Instantiate(jumpEffect, transform.position, Quaternion.identity);
            }
        }

        private void FixedUpdate()
        {
            float horizontalInput = Input.GetAxisRaw("Horizontal");
            Vector3 direction = new Vector3(horizontalInput, 0, 0);
            Vector3 velocity = direction * _speed;

            if (isGrounded == true)
            {
                _anim.SetBool("Grounded", true);

                if (Input.GetKeyDown(KeyCode.Space)) //JUMP
                {
                    _yVelocity = _jumpHeight;
                    canDoubleJump = true;
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Space)) // double jump
                {
                    if (canDoubleJump == true)
                    {
                        _yVelocity += _jumpHeight;
                        canDoubleJump = false;
                    }
                }
                _anim.SetBool("Grounded", false);
                //apply gravity
                _yVelocity -= _gravity;
            }

            //rb.velocityvelocity.y = _yVelocity;
            rb.velocity = new Vector3(rb.velocity.x, _yVelocity, 0);
            transform.Translate(velocity * Time.deltaTime);
            _anim.SetFloat("Walking", Mathf.Abs(horizontalInput));

            if(horizontalInput > 0)
            {
                Flip(true);
                specialAttack.Flip(true);
            }
            else if(horizontalInput < 0)
            {
                Flip(false);
                specialAttack.Flip(false);
            }
        }

        void Flip(bool faceRight)
        {
            if(faceRight == true)
            {
                _renderer.flipX = false;
            }
            else if(faceRight == false)
            {
                _renderer.flipX = true;
            }
        }
    }    
}