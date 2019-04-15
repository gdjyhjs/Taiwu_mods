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

namespace GuQuquAdventure
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
        static GameObject obj;

        public static bool Load(UnityModManager.ModEntry modEntry)
        {
            #region 基础设置
            settings = Settings.Load<Settings>(modEntry);
            Logger = modEntry.Logger;
            modEntry.OnToggle = OnToggle;
            modEntry.OnGUI = OnGUI;
            modEntry.OnSaveGUI = OnSaveGUI;
            #endregion


            if (obj == null)
            {
                   
                ////从文件夹里加载包
                //var my_ui_ab = UnityEngine.AssetBundle.LoadFromFile("F:/ui.assetbundle");
                //if (my_ui_ab == null)
                //{
                //    return true;
                //}
                ////从Bundle包中加载名字为：ququ_adventure 的资源，加载为 GameObject
                //var prefab = my_ui_ab.LoadAsset<GameObject>("ququ_adventure");
                //GameObject go = GameObject.Instantiate(prefab);

                //// 用完就删 asset 包用完就删 节约内存
                //my_ui_ab.Unload(false);
            }
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
    }
}