using System;
using System.Collections.Generic;
using System.IO;
using Astral.Humans;
using Astral.Weapons;
using UnityEditor;
using UnityEngine;


namespace Astral.Editor
{
    
    public class WeaponCreate : EditorWindow
    {
        private const string Path = "Assets/Resources/Weapons/"; 
        public GameObject parentGameObject;
        public string weaponName;
        public int baseFirePower;
        public int baseFireRate;
        public int baseFireRange;
        public Vector3 bulletPoint;

        
        public GameObject bulletPrefab;
        
        public int minHumanCount;
        public int maxHumanCount;
        public Color humanColor;


        public int humanCount;
        public List<GameObject> humanList;

        private readonly GUIStyle _style = new GUIStyle();
            
        [MenuItem("Astral/Weapon/Create Weapon")]
        public static void ShowWindow()
        {
           var window = GetWindow<WeaponCreate>("Weapon Create");
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

            DrawGUI();
            
            if (GUILayout.Button("Create Weapon"))
            {
                CreateMyAsset();
            }
        }

        void DrawGUI()
        {
            GUIStyle();
            EditorGUILayout.Space(5);
            EditorGUILayout.LabelField("Parent Settings", _style);
            EditorGUILayout.Space(5);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Select Parent Object");
            parentGameObject = EditorGUILayout.ObjectField(Selection.activeObject, typeof(GameObject), true) as GameObject;
            EditorGUILayout.EndHorizontal();
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
            baseFireRate = EditorGUILayout.IntSlider(baseFireRate, 1, 100);
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
                    humanList[i] = (GameObject)EditorGUILayout.ObjectField(parentGameObject.transform.GetChild(i).gameObject, typeof(GameObject));
                }
            }
            EditorGUILayout.BeginHorizontal();
            bulletPoint = EditorGUILayout.Vector3Field("Bullet Spawn Point",bulletPoint);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
        }

        private void CreateMyAsset()
        {
            if (!File.Exists(Path+weaponName+".asset"))
            {
                Weapon asset = CreateInstance<Weapon>();
                asset.weaponName = weaponName;
                asset.baseFirePower = baseFirePower;
                asset.baseFireRate = baseFireRate;
                asset.baseFireRange = baseFireRange;
                asset.bulletPrefab = bulletPrefab;
                asset.minHumanCount = minHumanCount;
                asset.maxHumanCount = maxHumanCount;
                asset.humanList = new List<Human>();
                asset.bulletPoint = bulletPoint;
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

                AssetDatabase.CreateAsset(asset, Path+weaponName+".asset");
                AssetDatabase.SaveAssets();
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = asset;
            }
            else
            {
                Debug.LogWarning("this '"+weaponName+ "' file exists!");
            }

        }
    
    }
}
