using System.Collections.Generic;
using System.IO;
using Astral.Humans;
using Astral.Weapons;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Astral.Editor
{
    public class WeaponEdit : EditorWindow
    {
        private const string Path = "Assets/Resources/Weapons/";

        public GameObject humanPrefab;
        public Weapon selectedWeapon;
        public GameObject parentGameObject;
        public string weaponName;
        public int baseFirePower;
        public float baseFireRate;
        public int baseFireRange;
        public Transform bulletPointTransform;
        public Vector3 bulletPoint;

        public GameObject bulletPrefab;

        public int minHumanCount;
        public int maxHumanCount;
        public Color humanColor;


        public int humanCount;
        public List<GameObject> humanList;

        private readonly GUIStyle _style = new GUIStyle();
        private bool _editWeapon,_editable;

        [MenuItem("Astral/Weapon/Edit Weapon")]
        public static void ShowWindow()
        {
            var window = GetWindow<WeaponEdit>("Weapon Edit");
            window.minSize = new Vector2(700, 700);
            window.maxSize = new Vector2(800, 800);
        }

        void GUIStyle()
        {

            _style.fontStyle = FontStyle.Bold;
            _style.normal.textColor = Color.white;
            _style.fontSize = 15;
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Select Weapon");
            selectedWeapon =
                EditorGUILayout.ObjectField(selectedWeapon, typeof(Weapon), true) as Weapon;
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Select Parent Object");
            parentGameObject =
                EditorGUILayout.ObjectField(parentGameObject, typeof(GameObject), true) as GameObject;
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Select Human Prefab");
            humanPrefab =
                EditorGUILayout.ObjectField(humanPrefab, typeof(GameObject), true) as GameObject;
            EditorGUILayout.EndHorizontal();
            if (selectedWeapon)
            {
                if (GUILayout.Button("Play & Edit Weapon"))
                {
                    if (!EditorApplication.isPlaying)
                    {
                        EditorApplication.EnterPlaymode();
                    }
                    else
                    {
                        ListWeaponProperties();
                    }
                }
            }

            if (_editable)
            {
                DrawGUI();
            }

        }

         void ListWeaponProperties()
         {
            _editable = true;
            weaponName = selectedWeapon.weaponName;
            baseFirePower = selectedWeapon.baseFirePower;
            baseFireRate = selectedWeapon.baseFireRate;
            baseFireRange = selectedWeapon.baseFireRange;
            bulletPrefab = selectedWeapon.bulletPrefab;
            minHumanCount = selectedWeapon.minHumanCount;
            maxHumanCount = selectedWeapon.maxHumanCount;
            bulletPoint = selectedWeapon.bulletPoint;
            for (int i = 0; i < selectedWeapon.humanList.Count; i++)
            {
                if (parentGameObject != null)
                {
                    var createHuman = Instantiate(humanPrefab, parentGameObject.transform);
                    createHuman.transform.localPosition = selectedWeapon.humanList[i].humanPositions;
                    createHuman.transform.localRotation = Quaternion.Euler(selectedWeapon.humanList[i].humanRotation);
                    var humanAnim = createHuman.GetComponent<Animator>();
                    var humanRenderer = createHuman.GetComponentInChildren<Renderer>();
                    humanRenderer.material.color = selectedWeapon.humanList[i].humanColor;
                    humanAnim.Play(selectedWeapon.humanList[i].humanPose,0);
                }
            }
        }
        
        void DrawGUI()
        {
            GUIStyle();
            EditorGUILayout.Space(20);
            EditorGUILayout.LabelField("Weapon Settings", _style);
            EditorGUILayout.BeginVertical();
            weaponName = EditorGUILayout.TextField("Weapon Name", weaponName);
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("Base Fire Power");
            baseFirePower = EditorGUILayout.IntSlider(baseFirePower, 1, 100);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Base Fire Rate");
            baseFireRate = EditorGUILayout.IntSlider((int)baseFireRate, 1, 100);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Base Fire Range");
            baseFireRange = EditorGUILayout.IntSlider(baseFireRange, 1, 100);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Min Human Count");
            minHumanCount = EditorGUILayout.IntSlider(minHumanCount, 0, 100);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Max Human Count");
            maxHumanCount = EditorGUILayout.IntSlider(maxHumanCount, 1, 100);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Bullet Prefab");
            bulletPrefab = EditorGUILayout.ObjectField(bulletPrefab, typeof(GameObject), true) as GameObject;
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Human Color");
            humanColor = EditorGUILayout.ColorField(humanColor);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(5);
            EditorGUILayout.LabelField("Human List", _style);
            if (parentGameObject != null)
            {
                humanCount = parentGameObject.transform.childCount;
                humanList = new List<GameObject>();
                for (int i = 0; i < humanCount; i++)
                {
                    humanList.Add(null);
                    humanList[i] =
                        (GameObject)EditorGUILayout.ObjectField(parentGameObject.transform.GetChild(i).gameObject,
                            typeof(GameObject));
                }
            }
            EditorGUILayout.BeginHorizontal();
            bulletPoint = EditorGUILayout.Vector3Field("Bullet Spawn Point",bulletPoint);
            EditorGUILayout.EndHorizontal();
            if (GUILayout.Button("Update Weapon"))
            {
                UpdateWeapon();
            }

            EditorGUILayout.EndVertical();
        }

        private void UpdateWeapon()
        {
            if (File.Exists(Path + weaponName + ".asset"))
            {
                Weapon asset = CreateInstance<Weapon>();
                asset.weaponName = weaponName;
                asset.baseFirePower = baseFirePower;
                asset.baseFireRate = baseFireRate;
                asset.baseFireRange = baseFireRange;
                asset.bulletPrefab = bulletPrefab;
                asset.minHumanCount = minHumanCount;
                asset.maxHumanCount = maxHumanCount;
                asset.bulletPoint = bulletPoint;
                asset.humanList = new List<Human>();
                Debug.Log(humanCount + " " + humanList.Count);
                for (int i = 0; i < humanCount; i++)
                {
                    var getAnimator = humanList[i].GetComponent<Animator>().GetCurrentAnimatorClipInfo(0);
                    asset.humanList.Add(new Human(
                        getAnimator[0].clip.name,
                        humanList[i].transform.localPosition,
                        humanList[i].transform.localRotation.eulerAngles,
                        humanList[i].transform.localScale,
                        humanColor));
                }

                AssetDatabase.CreateAsset(asset, Path + weaponName + ".asset");
                AssetDatabase.SaveAssets();
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = asset;
            }
            else
            {
                Debug.LogWarning("this '" + weaponName + "' file not exists!");
                var window = GetWindow<WeaponCreate>("Weapon Editor");
                window.Show();
            }
        }
    }
}