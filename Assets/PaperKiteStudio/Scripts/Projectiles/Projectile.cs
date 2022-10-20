using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PaperKiteStudio.DroppysWaterTrials
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField]
        private float _speed;

        private SpecialAttackHandler specialAttack;

        private SpriteRenderer _renderer;

        private void Start()
        {
            Destroy(this.gameObject, 3.0f);
            specialAttack = GameObject.Find("Player").GetComponent<SpecialAttackHandler>();
            _renderer = GetComponent<SpriteRenderer>();

            if(specialAttack.flipped == false)
            {
                _renderer.flipX = false;
            }

            else if(specialAttack.flipped == true)
            {
                _renderer.flipX = true;
            }
        }
        // Update is called once per frame
        void Update()
        {
            if(specialAttack.flipped == false)
            {
                transform.Translate(new Vector3(1, 0, 0) * _speed * Time.deltaTime);
            }
            else if(specialAttack.flipped == true)
            {
                transform.Translate(new Vector3(-1, 0, 0) * _speed * Time.deltaTime);

            }
        }
    }
}
