using System;
using System.Collections.Generic;
using System.Linq;
using Astral.Obstacles;
using Astral.Utility;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Astral.Weapons
{
    public class WeaponBehaviour : MonoBehaviour
    {
        public GameObject humanPrefab; 
        public List<Weapon> weaponList;
        public List<GameObject> humanList;
        [SerializeField] private LayerMask hitLayer;

        private Sequence _sequence;
        [SerializeField] Weapon _activeWeapon;
        private RaycastHit _hit;
        private float fireTime;
        private GameManager _manager;
        private GameObject _aloneHuman;
        private static readonly int Run = Animator.StringToHash("Run");
        private Animator _animator;
        [SerializeField] private Transform bulletPoint;

        private void OnEnable()
        {
            EventManager.onUpgradeWeapon += IncreaseHuman;
            EventManager.onDropHuman += DecreaseHuman;
            EventManager.onFinishDropHuman += DecreaseAllHuman;
        }

        private void OnDisable()
        {
            EventManager.onUpgradeWeapon -= IncreaseHuman;
            EventManager.onDropHuman -= DecreaseHuman;
            EventManager.onFinishDropHuman -= DecreaseAllHuman;

        }

        private void Start()
        {
            _manager = FindObjectOfType<GameManager>();
            CheckHuman();
        }

        private void CheckHuman()
        {
            if (_manager.humanCount.value<2)
            {
                _aloneHuman = Instantiate(humanPrefab,transform);
                _animator = _aloneHuman.GetComponent<Animator>();
                _aloneHuman.transform.position = transform.position;
                _aloneHuman.tag = "Gun";
                humanList.Add(_aloneHuman);

            }
            else
            {
                IncreaseHuman(transform.position,_manager.humanCount.value);
                UpdateWeapon();
            }
        }

        private void Update()
        {
            if (_manager.humanCount.value<2)
            {
                _animator
                    .SetBool(Run,_manager.state == State.Started);
            }

            if (_activeWeapon != null)
            {
                if (Physics.SphereCast(transform.position,.1f,transform.forward,out _hit,_activeWeapon.baseFireRange+_manager.range.value,hitLayer))
                {
                    fireTime -= Time.deltaTime;
                    if (fireTime <= 0)
                    {
                        SpawnBullet();
                        fireTime = _activeWeapon.baseFireRate;
                    }


                }
            }

        }

        private void SpawnBullet()
        {
            var bulletObj = Instantiate(_activeWeapon.bulletPrefab);
            bulletObj.transform.position = bulletPoint.position;
            var bullet = bulletObj.GetComponent<Bullet>();
            bullet.damage = _activeWeapon.baseFirePower + _manager.damage.value;
        }
        


        private void IncreaseHuman(Vector3 createdPos,int count = 1)
        {
            

            if (humanList.Count < weaponList[^1].maxHumanCount)
            {
                if (count> Mathf.Abs(humanList.Count - weaponList[^1].maxHumanCount)) count = Mathf.Abs(humanList.Count - weaponList[^1].maxHumanCount);

                    for (int i = 0; i < count; i++)
                    {
                        var newHuman = Instantiate(humanPrefab, transform);
                        newHuman.transform.position = createdPos;
                        newHuman.tag = "Gun";
                        humanList.Add(newHuman);
                    }
                    UpdateWeapon();

            }

        }

        private void DecreaseHuman(int count = 1)
       {

           if (humanList.Count > count)
           {
               for (int i = 0; i < count; i++)
               {
                   var randHuman = Random.Range(0, humanList.Count);
                   var foundHuman = humanList[randHuman];
                   humanList.Remove(foundHuman);
                   foundHuman.transform.parent = null;
                   foundHuman.AddComponent<Rigidbody>().AddForce(Vector3.up*3,ForceMode.Impulse);
                   foundHuman.tag = "Dropped";
               }
               UpdateWeapon();
           }
           else
           {
               _activeWeapon = null;
               DecreaseAllHuman(false);
           }
       }

        private void DecreaseAllHuman(bool isFinish)
        {
            _activeWeapon = null;
            foreach (var t in humanList)
            {
                t.transform.parent = null;
                var randomPos = Random.Range(-3, 3);
                t.AddComponent<Rigidbody>().AddForce(new Vector3(randomPos,randomPos,randomPos),ForceMode.Impulse);
            }

            if (isFinish) EventManager.OnWin();
            else EventManager.OnLose();
                
            humanList.Clear();
        }
       
        private void UpdateWeapon()
        {
            _activeWeapon =
                weaponList.Find(x => humanList.Count >= x.minHumanCount && humanList.Count <= x.maxHumanCount);
            if (_activeWeapon)
            {
                bulletPoint.localPosition = _activeWeapon.bulletPoint;
                for (var i = 0; i < humanList.Count; i++)
                {
                    humanList[i].transform.parent = transform; 
                    _sequence = DOTween.Sequence()
                        .Join(humanList[i].transform.DOLocalJump(_activeWeapon.humanList[i].humanPositions,.1f,1,.4f))
                        .Join(humanList[i].transform.DORotate(_activeWeapon.humanList[i].humanRotation, .4f))
                        .Join(humanList[i].transform.DOScale(_activeWeapon.humanList[i].humanScale, .4f));
                    var humanAnim = humanList[i].GetComponent<Animator>();
                    var humanRenderer = humanList[i].GetComponentInChildren<Renderer>();
                    humanRenderer.material.color = _activeWeapon.humanList[i].humanColor;
                    humanAnim.Play(_activeWeapon.humanList[i].humanPose,0);
                }
            }
        }

    }
}
