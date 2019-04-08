namespace CS.ActorConfig
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;

    public class ActorHelper
    {
        private const string ROOT_BONE = "Bip001";
        public static GameObject Instantiate(ActorConfigEditor actorEditor)
        {
            GameObject role = Instantiate(actorEditor.CharacterPath);
            GameObject avatargo = Instantiate(actorEditor.AvatarPath);
            Debug.Log("骨骼:" + actorEditor.CharacterPath + "\n蒙皮:" + actorEditor.AvatarPath);
            if (role != null && avatargo != null)
            {
                role.SetActive(false);
                Transform bip = role.transform.Find(ROOT_BONE);
                if (bip != null)
                    UnityEngine.Object.DestroyImmediate(bip.gameObject, true);

                while (avatargo.transform.childCount > 0)
                {
                    Transform trans = avatargo.transform.GetChild(0);
                    trans.parent = role.transform;
                }
                UnityEngine.Object.DestroyImmediate(avatargo, true);
                role.SetActive(true);
                role.transform.localScale = Vector3.one * actorEditor.ModelScale;
                role.tag = "Player";
            }
            var mc = role.AddComponent<MecanimControl>();
            return role;
        }

        public static GameObject Instantiate(string path)
        {
            return Instantiate(path, null);
        }

        public static GameObject Instantiate(string path, GameObject parent)
        {
            return Instantiate(path, parent, Vector3.zero, Vector3.zero, Vector3.one);
        }
        public static GameObject Instantiate(string path, GameObject parent, Vector3 pos, Vector3 eularangles, Vector3 scale)
        {
            GameObject obj = null;
            if (!string.IsNullOrEmpty(path))
            {
                var Prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                if (Prefab != null)
                {
                    obj = GameObject.Instantiate(Prefab) as GameObject;
                    if (parent != null)
                    {
                        obj.transform.parent = parent.transform;
                    }
                    obj.transform.localPosition = pos;
                    obj.transform.localEulerAngles = eularangles;
                    obj.transform.localScale = scale;
                }

            }

            return obj;
        }

        public static GameObject CreateGameObject(string name, string relPath = "/")
        {
            string path = relPath + name;
            GameObject go = GameObject.Find(path);
            if (go == null)
                go = new GameObject(name);
            GameObject parent = GameObject.Find(relPath);
            if (parent != null)
            {
                go.transform.parent = parent.transform;
                go.transform.localPosition = Vector3.zero;
                go.transform.localEulerAngles = Vector3.zero;
            }
            return go;
        }
    }
}


//if (actorEditor.Weapon != null)
//{
//    if (!actorEditor.Weapon.lpath.IsNullOrEmpty())
//    {

//        var bone = role.GetChildByName(PlayEffectItem.BoneNames[CinemaDirector.PlayEffectItem.EffectInstanceBindType.LeftWeapon]);
//        if (bone != null)
//        {
//            string path = ActionSettings.GetWeaponPath(actorEditor.Weapon.lpath);
//            if (!path.IsNullOrEmpty())
//            {
//                Debug.Log("load left weapon:" + path);
//                SkillEditorHelper.Instantiate(path, bone);
//            }

//        }
//    }
//    if (!actorEditor.Weapon.rpath.IsNullOrEmpty())
//    {
//        var bone = role.GetChildByName(PlayEffectItem.BoneNames[CinemaDirector.PlayEffectItem.EffectInstanceBindType.RightWeapon]);

//        if (bone != null)
//        {
//            string path = HomeConfig.GetWeaponPath(actorEditor.Weapon.rpath);
//            if (!path.IsNullOrEmpty())
//            {
//                Debug.Log("load right weapon:" + path);
//                SkillEditorHelper.Instantiate(path, bone);
//            }
//        }
//    }
//}