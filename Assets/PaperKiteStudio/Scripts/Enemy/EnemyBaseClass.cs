using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PaperKiteStudio.DroppysWaterTrials
{
    public abstract class EnemyBaseClass : MonoBehaviour
    {
        [SerializeField]
        protected int health;
        [SerializeField]
        protected float speed;
        [SerializeField]
        protected GameObject lootDrop;

        [SerializeField]
        protected Transform[] wayPoints;
        protected Vector3 currentTarget;
        [SerializeField]
        protected bool reversing;
        [SerializeField]
        protected bool targetReached;
        protected SpriteRenderer _renderer;
        protected Animator anim;

        [SerializeField]
        protected bool hit;
        [SerializeField]
        protected bool attacking;
        [SerializeField]
        protected GameObject hitBox;
        [SerializeField]
        protected float distanceToPlayer;

        [SerializeField]
        protected enum State
        {
            Patrolling,
            Idle,
            Hit,
            Chasing,
            Attacking,
            Dying,
        }

        [SerializeField]
        protected State state;

        private RigidBodyMovement player;

        public virtual void Init()
        {
            player = GameObject.Find("Player").GetComponent<RigidBodyMovement>();
            _renderer = GetComponentInChildren<SpriteRenderer>();
            anim = GetComponentInChildren<Animator>();
        }

        public virtual void Start()
        {
            Init();
        }

        public virtual void Update()
        {
            distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            switch (state)
            {
                case State.Patrolling:
                    WaypointNav();
                    break;
                case State.Hit:
                    Hit();
                    break;
                case State.Chasing:
                    Chasing();
                    break;
                case State.Attacking:
                    Attack();
                    break;
            }

            if (_renderer.flipX == true)
            {
                hitBox.transform.position = new Vector3(transform.position.x - 3, transform.position.y, transform.position.z);
            }
            if (_renderer.flipX == false)
            {
                hitBox.transform.position = new Vector3(transform.position.x + 3, transform.position.y, transform.position.z);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("PlayerAttack"))
            {
                anim.SetTrigger("Hurt");
                state = State.Hit;
            }
        }

        public virtual void Hit()
        {
            if (hit == false)
            {
                StartCoroutine(HitWait());
                hit = true;
            }
        }

        IEnumerator HitWait()
        {
            //float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

            //anim.SetBool("Patrolling", false);
            yield return new WaitForSeconds(Random.Range(1.0f, 1.5f));

            if (distanceToPlayer < 3)
            {
                hit = false;
            }

            if (distanceToPlayer > 3 && distanceToPlayer < 10)
            {
                anim.SetBool("Chasing", true);
                state = State.Chasing;
            }

            if (distanceToPlayer > 10)
            {
                state = State.Patrolling;
                anim.SetBool("Patrolling", true);
            }

        }

        public virtual void WaypointNav()
        {
            if (targetReached == false)
            {
                transform.position = Vector3.MoveTowards(transform.position, currentTarget, speed * Time.deltaTime); // move to current target
            }

            if (currentTarget == wayPoints[0].position) // flip the sprite
            {
                _renderer.flipX = true;
            }
            else
            {
                _renderer.flipX = false;
            }

            if(transform.position == wayPoints[0].position) // do this when target reached
            {
                targetReached = true;
                StartCoroutine(Idle());
            }
            else if(transform.position == wayPoints[1].position)
            {
                targetReached = true;
                StartCoroutine(Idle());
            }

            if(distanceToPlayer < 10 && distanceToPlayer > 3)
            {
                state = State.Chasing;
            }
        }

        IEnumerator Idle()
        {
            anim.SetBool("Idle", true);
            anim.SetBool("Patrolling", false);
            yield return new WaitForSeconds(Random.Range(2.0f, 4.5f)); //waits
            anim.SetBool("Patrolling", true); // patrolling anim
            anim.SetBool("Idle", false);
         
            
            if (transform.position == wayPoints[0].position)
            {
                currentTarget = wayPoints[1].position;//target changes
            }
            else if(transform.position == wayPoints[1].position)
            {
                currentTarget = wayPoints[0].position;//target changes

            }

            targetReached = false; // will move now
        }

        public virtual void Chasing()
        {
            hit = false;

            if (distanceToPlayer > 10)
            {
                anim.SetBool("Chasing", false);
                anim.SetBool("Patrolling", true);
                state = State.Patrolling;
            }

            if(distanceToPlayer < 10 && distanceToPlayer > 3 && targetReached ==false)
            {
                anim.SetBool("Chasing", true);
                anim.SetBool("Patrolling", false);
                if(player.transform.position.x > transform.position.x)
                {
                    _renderer.flipX = false;
                }
                else if(player.transform.position.x < transform.position.x)
                {
                    _renderer.flipX = true;
                }
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);

            }

            else if (distanceToPlayer <= 3)
            {
                targetReached = true;
                anim.SetBool("Chasing", false);

                state = State.Attacking;
            }
        }

        public virtual void Attack()
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            
            if(distanceToPlayer > 3 && attacking == false)
            {
                anim.SetBool("Chasing", true);
                targetReached = false;
                state = State.Chasing;
            }

            if(attacking == false)
            {
                StartCoroutine(AttackLoop());
                attacking = true;
            }
        }

        IEnumerator AttackLoop()
        {
            anim.SetBool("Patrolling", false);
            anim.SetBool("Chasing", false);
            anim.SetTrigger("Attack");
            yield return new WaitForSeconds(1.0f);
            attacking = false;
        }
    }
}