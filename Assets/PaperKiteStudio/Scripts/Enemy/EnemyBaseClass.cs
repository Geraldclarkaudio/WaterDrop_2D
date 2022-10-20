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
        protected List<Transform> wayPoints;

        [SerializeField]
        protected int currentTarget;
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
        protected float timeRemaining;

        [SerializeField]
        protected enum State
        {
            Patrolling,
            Hit,
            Chasing,
            Attacking,
            Dying,
        }

        [SerializeField]
        protected State state;

        private RigidBodyMovement player;

        public virtual void Start()
        {
            player = GameObject.Find("Player").GetComponent<RigidBodyMovement>();
            _renderer = GetComponentInChildren<SpriteRenderer>();
            anim = GetComponentInChildren<Animator>();
        }

        public virtual void Update()
        {
            switch (state)
            {
                case State.Patrolling:
                    Patrol();
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

            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
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
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

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

        public virtual void Patrol()
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            hit = false;

            if (distanceToPlayer <= 10 && distanceToPlayer > 3)
            {
                anim.SetBool("Chasing", true);
                anim.SetBool("Patrolling", false);
                state = State.Chasing;
            }

            if (wayPoints.Count > 0) // are there waypoints?
            {
                if (wayPoints[currentTarget] != null)// does the current target exist?
                {
                    transform.position = Vector3.MoveTowards(transform.position, wayPoints[currentTarget].position, speed * Time.deltaTime);
                    float distance = Vector3.Distance(transform.position, wayPoints[currentTarget].position); // distance between target and enemy

                    if (distance < 1.0f && targetReached == false)
                    {
                        if(currentTarget == 0 || currentTarget == wayPoints.Count - 1)
                        {
                            targetReached = true;
 
                            StartCoroutine(Idle());
                        }
                    }  
                }

                if (currentTarget == 0)
                {
                    _renderer.flipX = true;
                }
                else if (currentTarget == 1)
                {
                    _renderer.flipX = false;
                }
            }
        }

        IEnumerator Idle() // target reached is false, so resume the if statement in Patroling. 
        {
            anim.SetBool("Patrolling", false);
            yield return new WaitForSeconds(2.0f);
            anim.SetBool("Patrolling", true);

            if (reversing == true) // if im at waypoint 1 and ready to go. 
            {
                currentTarget--;
                _renderer.flipX = true;

                if (currentTarget == 0) // there are no more waypoints to decrement. 
                {
                    reversing = false;
                    currentTarget = 0; // set to zero
                }
            }

            else if (reversing == false)
            {
                _renderer.flipX = false;
                currentTarget++;

                if (currentTarget == wayPoints.Count)  //if at the end of the waypoint list, reverse. 
                {
                    //made it to the end. reverse
                    reversing = true;
                    currentTarget--;
                }
            }

            targetReached = false;
        }
    

        public virtual void Chasing()
        {
            hit = false;
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

            if (distanceToPlayer > 10)
            {
                anim.SetBool("Chasing", false);
                anim.SetBool("Patrolling", true);
                state = State.Patrolling;
            }

            if(distanceToPlayer < 10 && distanceToPlayer > 3 && targetReached ==false)
            {                
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