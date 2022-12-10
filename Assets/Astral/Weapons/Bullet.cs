using System;
using Astral.Obstacles;
using UnityEngine;

namespace Astral.Weapons
{
    public class Bullet : MonoBehaviour
    {
        public float speed;
        public int damage;

        private void Start()
        {
            Destroy(gameObject,1);
        }


        private void Update()
        {
            transform.position += transform.forward * (speed * Time.deltaTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.TryGetComponent<IDamageable>(out var hitObject))
            { 
                hitObject.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
    }
}