using Harmony12;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using UnityModManagerNet;
using System.Text.RegularExpressions;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Diagnostics;
using UnityEngine.EventSystems;
using Object = UnityEngine.Object;
using System.Collections;

namespace GuiQuquAdventure
{
    public class Settings : UnityModManager.ModSettings
    {
        public override void Save(UnityModManager.ModEntry modEntry)
        {
            UnityModManager.ModSettings.Save<Settings>(this, modEntry);
        }
    }
    public static class Main
    {
        public static bool onOpen = false;//
        public static bool enabled;
        public static Settings settings;
        public static UnityModManager.ModEntry.ModLogger Logger;
        public static GameObject obj;
        static string mod_path;

        public static bool Load(UnityModManager.ModEntry modEntry)
        {
            #region 基础设置
            settings = Settings.Load<Settings>(modEntry);
            Logger = modEntry.Logger;
            modEntry.OnToggle = OnToggle;
            modEntry.OnGUI = OnGUI;
            modEntry.OnSaveGUI = OnSaveGUI;
            mod_path = modEntry.Path;
            #endregion

            GameObject go = new GameObject();
            UnityEngine.GameObject.DontDestroyOnLoad(go);
            go.AddComponent<MaintainHall>();

            return true;
        }

        static void OnGUI(UnityModManager.ModEntry modEntry)
        {

        }

        public static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            enabled = value;
            return true;
        }
        static void OnSaveGUI(UnityModManager.ModEntry modEntry)
        {
            settings.Save(modEntry);
        }


        public static void LoadUI()
        {
            if (!obj)
            {
                //从文件夹里加载包
                var my_ui_ab = AssetBundle.LoadFromFile(mod_path + @"\assetbundle\guihall");
                if (my_ui_ab == null)
                {
                    return;
                }
                //从Bundle包中加载名字为：ququ_adventure 的资源，加载为 GameObject
                var prefab = my_ui_ab.LoadAsset<GameObject>("guihall");
                obj = Object.Instantiate(prefab);

                // 用完就删 asset 包用完就删 节约内存
                my_ui_ab.Unload(false);

                obj.transform.SetParent(QuquBattleSystem.instance.transform.parent, false);
                obj.transform.SetSiblingIndex(QuquBattleSystem.instance.transform.GetSiblingIndex() + 1);
                obj.AddComponent<QuquHall>();
            }
        }
    }

    class MaintainHall : MonoBehaviour
    {
        public static MaintainHall instance;
        void Start()
        {
            instance = this;
            StartCoroutine(MaintainWhile());
        }

        IEnumerator MaintainWhile()
        {
            yield return null;
            while (true)
            {
                yield return new WaitForSeconds(5);
                bool open = false;
                try
                {
                    if (Main.obj == null && DateFile.instance != null && ActorMenu.instance != null && Loading.instance != null && Loading.instance.gameObject.activeSelf)
                    {
                        string name = DateFile.instance.GetActorName();
                        if (null != name)
                            open = true;
                    }
                }
                catch (Exception e)
                {
                    open = false;
                    throw;
                }
                if (open)
                {
                    Main.LoadUI();
                }
            }
        }

        public void AddInvoke(Action fun, float time)
        {
            StartCoroutine(GoInvoke(fun, time));
        }

        private IEnumerator GoInvoke(Action fun, float time)
        {
            yield return new WaitForSeconds(time);
            fun();
        }

        void OnGUI()
        {
            if (GUI.Button(new Rect(20, 100, 50, 20), "设置"))
            {
                GuiQuquBattleSystem.instance.itemId = -1; // 即将获得或者失去的物品
                GuiQuquBattleSystem.instance.actorTyp = GuiQuquBattleSystem.ActorTyp.LeftObserver;
                GuiQuquBattleSystem.instance.playId = -1;
                GuiQuquBattleSystem.instance.leftPlayer = PlayerData.self;
                GuiQuquBattleSystem.instance.rightPlayer = null;
                GuiQuquBattleSystem.instance.ShowQuquBattleWindow();
            }
            if (GUI.Button(new Rect(20, 150, 50, 20), "关闭"))
            {
                GuiQuquBattleSystem.instance.CloseQuquBattleWindow();
            }
            if (GUI.Button(new Rect(20, 200, 50, 20), "赌人"))
            {
                QuquBattleSystem.instance.chooseActorButton.interactable = true;
            }
        }
    }
}