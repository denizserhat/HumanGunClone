using System.Collections.Generic;
using Astral.Humans;
using UnityEngine;

namespace Astral.Weapons
{
    [CreateAssetMenu(menuName = "Astral/Weapons/Create Weapon", fileName = "Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [Header("Weapon Properties")]
        public string weaponName;
        public int baseFirePower;
        public float baseFireRate;
        public int baseFireRange;
        public Vector3 bulletPoint;
        
        [Header("Weapon Visuals")]
        public GameObject bulletPrefab;

        [Header("Humans")] 
        [Range(0, 100)] 
        public int minHumanCount;
        [Range(0, 100)] 
        public int maxHumanCount;
        public List<Human> humanList;
    }
}
