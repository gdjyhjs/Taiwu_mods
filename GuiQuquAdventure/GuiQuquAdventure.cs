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

        static string make_color = "234";
        static string make_part = "3002";
        static string make_message = "可以点击制作蛐蛐来免费领养蛐蛐！";
        static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("颜色", GUILayout.Width(50));
            make_color = GUILayout.TextField(make_color, GUILayout.Width(100));
            GUILayout.Space(50);
            GUILayout.Label("部位", GUILayout.Width(50));
            make_part = GUILayout.TextField(make_part, GUILayout.Width(100));
            GUILayout.Space(50);
            if (GUILayout.Button("制作蛐蛐", GUILayout.Width(100)))
            {
                if (int.TryParse(make_color, out int color) && int.TryParse(make_part, out int partId))
                {
                    if (DateFile.instance.cricketDate.ContainsKey(color)&& DateFile.instance.cricketDate.ContainsKey(partId))
                    {
                        int itemId = DateFile.instance.MakeNewItem(10000);
                        GetQuquWindow.instance.MakeQuqu(itemId, color, partId);
                        DateFile.instance.GetItem(DateFile.instance.MianActorID(), itemId, 1, false);
                    }
                    else
                    {
                        make_message = "不存在的颜色ID或部位ID！";
                        make_color = "234";
                        make_part = "3002";
                    }
                }
                else
                {
                    make_message = "输入的颜色ID或部位ID错误！";
                    make_color = "234";
                    make_part = "3002";
                }
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label(make_message);
            if (GUILayout.Button("消除！", GUILayout.Width(100)))
            {
                GameObject gui = GameObject.Find("GUI");
                if (gui)
                {
                    Transform WindowMask = gui.transform.Find("WindowMask");
                    if (WindowMask)
                    {
                        WindowMask.gameObject.SetActive(!WindowMask.gameObject.activeSelf);
                    }
                }
            }
            if (GUILayout.Button("卡战斗时点击关闭战斗"))
            {
                if (QuquHall.instance)
                    QuquHall.instance.CloseQuquBattle();
            }
            GUILayout.EndHorizontal();
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
                yield return new WaitForSeconds(1);
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
                    // Main.Logger.Log("error:"+e.Message);
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
        }
    }
}