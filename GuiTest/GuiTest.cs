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
using System.IO;
using DG.Tweening;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using System.Collections;


namespace GuiTest
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
        static Transform root;
        static int x, y, w, h;
        static bool draw_xywh;
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

            HarmonyInstance harmony = HarmonyInstance.Create(modEntry.Info.Id);
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            if (root == null)
            {
                GameObject go = new GameObject();
                go.AddComponent<TestClick>();
                GameObject.DontDestroyOnLoad(go);
            }

            w = Screen.width;
            h = Screen.height;

            return true;
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


        #region  打印UI

        static void Log(string str)
        {
            Logger.Log(str);
        }

        static string GetPath(Transform go, string s = "")
        {
            if (s == "")
                s = go.name;
            else
                s = go.name + "/" + s;
            if (go.parent)
                return GetPath(go.parent, s);
            else
                return s;
        }

        static void LogObj<T>(string name, T t) where T : Component
        {
            Log(name + ":" + t.ToString() + " path=" + GetPath(t.transform));
        }
        static void LogObj(string name, GameObject t)
        {
            Log(name + ":" + t.ToString() + " path=" + GetPath(t.transform));
        }
        static void LogList<T>(string name, T[] t) where T : Component
        {
            Log(name + " length=" + t.Length);
            for (int i = 0; i < t.Length; i++)
            {
                Log(i + ":" + t[i].ToString() + " path=" + GetPath(t[i].transform));
            }
        }
        static void LogList(string name, GameObject[] t)
        {
            Log(name + " length=" + t.Length);
            for (int i = 0; i < t.Length; i++)
            {
                Log(i + ":" + t[i].ToString() + " path=" + GetPath(t[i].transform));
            }
        }

        #endregion

        static GameObject obj;
        static void OnGUI(UnityModManager.ModEntry modEntry)
        {

            //if (GUILayout.Button("创建UI"))
            //{
            //    if (!obj) // 创建UI
            //    {
            //        //var f_isShowActorMenu = typeof(ActorMenu).GetField("isShowActorMenu", BindingFlags.NonPublic | BindingFlags.Instance);
            //        //return (bool)f_isShowActorMenu.GetValue(ActorMenu.instance);
            //        //QuquBattleSystem self = QuquBattleSystem.instance;
            //        //LogObj("setBattleSpeedHolder", self.setBattleSpeedHolder);
            //        //LogList("setBattleSpeedToggle", self.setBattleSpeedToggle);
            //    }
            //}

            //GUILayout.Label("color:");
            //int.TryParse(GUILayout.TextField(qu_color.ToString()), out qu_color);
            //GUILayout.Label("partId:");
            //int.TryParse(GUILayout.TextField(partId.ToString()), out partId);
            //if (GUILayout.Button("测试"))
            //{
            //    foreach (var dic in DateFile.instance.cricketBattleDate)
            //    {
            //        Log(dic.Key.ToString());
            //        foreach (var item in dic.Value)
            //        {
            //            Log(" === " + dic.Key.ToString() + ":" + dic.Value);
            //        }
            //    }
            //    try
            //    {
            //        int item_id = DateFile.instance.MakeNewItem(10000, idx--);
            //        GetQuquWindow.instance.MakeQuqu(item_id, qu_color, partId);
            //        DateFile.instance.GetItem(DateFile.instance.MianActorID(), item_id, 1, false);
            //    }
            //    catch (Exception e)
            //    {
            //        Log(e.Message);
            //    }
            //}
            if (GUILayout.Button("打印"))
            {
                Button[] btns = QuquBattleSystem.instance.GetComponentsInChildren<Button>();
                foreach (var item in btns)
                {
                    LogObj(item.name, item);
                }
            }
            if (GUILayout.Button("堵人"))
            {
                QuquBattleSystem.instance.chooseActorButton.interactable = !QuquBattleSystem.instance.chooseActorButton.interactable;
            }
        }
        static int qu_color = 0;
        static int partId = 0;
        static int idx = -10;




        class TestClick : MonoBehaviour
        {
            //List<UIImage> png_list;
            void Update()
            {
                if (Input.GetMouseButtonDown(0))
                {
                    GameObject go = ClickObject();
                    if (go)
                    {
                        string str = "点击";
                        Transform tf = go.transform;
                        while (tf)
                        {
                            str += "/" + tf;
                            tf = tf.parent;
                        }
                        Main.Logger.Log(str);
                    }
                }
            }
            public GameObject ClickObject()
            {
                PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
                eventDataCurrentPosition.position = new Vector2
                    (
                    Input.mousePosition.x, Input.mousePosition.y
                    );
                List<RaycastResult> results = new List<RaycastResult>();
                EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
                if (results.Count > 0)
                {
                    return results[0].gameObject;
                }
                else
                {
                    return null;
                }
            }
            private void OnGUI()
            {

            }
        }


        //[HarmonyPatch(typeof(ActorMenu), "CloseActorMenu")]
        //public static class ActorMenu_CloseActorMenu_Patch
        //{
        //    public static void Postfix()
        //    {
        //        Main.Logger.Log("关闭");
        //        StackTrace st = new StackTrace(true);
        //        Main.Logger.Log(st.ToString());
        //    }
        //}



        //[HarmonyPatch(typeof(QuquBattleSystem), "SetQuquBattleSpeed")]
        //public static class SetQuquBattleSpeed
        //{
        //    public static bool Prefix(int value)
        //    {
        //        Log("SetQuquBattleSpeed");
        //        Log(""+value);
        //        return true;
        //    }
        //}
        //[HarmonyPatch(typeof(QuquBattleSystem), "SetTweener")]
        //public static class SetTweener
        //{
        //    public static bool Prefix()
        //    {
        //        Log("SetTweener");
        //        return true;
        //    }
        //}
        //[HarmonyPatch(typeof(QuquBattleSystem), "ShowQuquBattleWindow")]
        //public static class ShowQuquBattleWindow
        //{
        //    public static bool Prefix()
        //    {
        //        Log("ShowQuquBattleWindow");
        //        return true;
        //    }
        //}
        //[HarmonyPatch(typeof(QuquBattleSystem), "QuquBattleWindowOpend")]
        //public static class QuquBattleWindowOpend
        //{
        //    public static bool Prefix()
        //    {
        //        Log("QuquBattleWindowOpend");
        //        return true;
        //    }
        //}
        //[HarmonyPatch(typeof(QuquBattleSystem), "SetActorAnimation")]
        //public static class SetActorAnimation
        //{
        //    public static bool Prefix()
        //    {
        //        Log("SetActorAnimation");
        //        return true;
        //    }
        //}
        //[HarmonyPatch(typeof(QuquBattleSystem), "SetAnimationDone")]
        //public static class SetAnimationDone
        //{
        //    public static bool Prefix()
        //    {
        //        Log("SetAnimationDone");
        //        return true;
        //    }
        //}
        //[HarmonyPatch(typeof(QuquBattleSystem), "CloseQuquBattleWindowButton")]
        //public static class CloseQuquBattleWindowButton
        //{
        //    public static bool Prefix()
        //    {
        //        Log("CloseQuquBattleWindowButton");
        //        return true;
        //    }
        //}
        //[HarmonyPatch(typeof(QuquBattleSystem), "CloseQuquBattleWindow")]
        //public static class CloseQuquBattleWindow
        //{
        //    public static bool Prefix()
        //    {
        //        Log("CloseQuquBattleWindow");
        //        return true;
        //    }
        //}
        //[HarmonyPatch(typeof(QuquBattleSystem), "CloseBattleWindowDone")]
        //public static class CloseBattleWindowDone
        //{
        //    public static bool Prefix()
        //    {
        //        Log("CloseBattleWindowDone");
        //        return true;
        //    }
        //}
        //[HarmonyPatch(typeof(QuquBattleSystem), "SetQuquBattle")]
        //public static class SetQuquBattle
        //{
        //    public static bool Prefix()
        //    {
        //        Log("SetQuquBattle");
        //        return true;
        //    }
        //}
        //[HarmonyPatch(typeof(QuquBattleSystem), "SetResourceBody")]
        //public static class SetResourceBody
        //{
        //    public static bool Prefix(int enemyId, int baseBodySize)
        //    {
        //        Log("SetResourceBody");
        //        Log(""+enemyId);
        //        Log(""+baseBodySize);
        //        return true;
        //    }
        //}
        //[HarmonyPatch(typeof(QuquBattleSystem), "MakeQuqu")]
        //public static class MakeQuqu
        //{
        //    public static bool Prefix()
        //    {
        //        Log("MakeQuqu");
        //        return true;
        //    }
        //}
        //[HarmonyPatch(typeof(QuquBattleSystem), "UpdateActorQuquValue")]
        //public static class UpdateActorQuquValue
        //{
        //    public static bool Prefix(int index)
        //    {
        //        Log("UpdateActorQuquValue");
        //        Log(""+index);
        //        return true;
        //    }
        //}
        //[HarmonyPatch(typeof(QuquBattleSystem), "UpdateEnemyQuquValue")]
        //public static class UpdateEnemyQuquValue
        //{
        //    public static bool Prefix(int index)
        //    {
        //        Log("UpdateEnemyQuquValue");
        //        Log(""+index);
        //        return true;
        //    }
        //}
        //[HarmonyPatch(typeof(QuquBattleSystem), "UpdateQuquHp")]
        //public static class UpdateQuquHp
        //{
        //    public static bool Prefix(int index)
        //    {
        //        Log("UpdateQuquHp");
        //        Log(""+index);
        //        return true;
        //    }
        //}
        //[HarmonyPatch(typeof(QuquBattleSystem), "ShowChooseBodyTyp")]
        //public static class ShowChooseBodyTyp
        //{
        //    public static bool Prefix()
        //    {
        //        Log("ShowChooseBodyTyp");
        //        return true;
        //    }
        //}
        //[HarmonyPatch(typeof(QuquBattleSystem), "CloseChooseBodyTyp")]
        //public static class CloseChooseBodyTyp
        //{
        //    public static bool Prefix()
        //    {
        //        Log("CloseChooseBodyTyp");
        //        return true;
        //    }
        //}
        //[HarmonyPatch(typeof(QuquBattleSystem), "UpdateActorBody")]
        //public static class UpdateActorBody
        //{
        //    public static bool Prefix()
        //    {
        //        Log("UpdateActorBody");
        //        return true;
        //    }
        //}
        //[HarmonyPatch(typeof(QuquBattleSystem), "SetUseResourceButton")]
        //public static class SetUseResourceButton
        //{
        //    public static bool Prefix()
        //    {
        //        Log("SetUseResourceButton");
        //        return true;
        //    }
        //}
        //[HarmonyPatch(typeof(QuquBattleSystem), "SetUseItemButton")]
        //public static class SetUseItemButton
        //{
        //    public static bool Prefix()
        //    {
        //        Log("SetUseItemButton");
        //        return true;
        //    }
        //}
        //[HarmonyPatch(typeof(QuquBattleSystem), "SetUseActorButton")]
        //public static class SetUseActorButton
        //{
        //    public static bool Prefix()
        //    {
        //        Log("SetUseActorButton");
        //        return true;
        //    }
        //}
        //[HarmonyPatch(typeof(QuquBattleSystem), "CloseActorWindow")]
        //public static class CloseActorWindow
        //{
        //    public static bool Prefix()
        //    {
        //        Log("CloseActorWindow");
        //        return true;
        //    }
        //}
        //[HarmonyPatch(typeof(QuquBattleSystem), "GetActor")]
        //public static class GetActor
        //{
        //    public static bool Prefix()
        //    {
        //        Log("GetActor");
        //        return true;
        //    }
        //}
        //[HarmonyPatch(typeof(QuquBattleSystem), "RemoveActor")]
        //public static class RemoveActor
        //{
        //    public static bool Prefix()
        //    {
        //        Log("RemoveActor");
        //        return true;
        //    }
        //}
        //[HarmonyPatch(typeof(QuquBattleSystem), "SetActorId")]
        //public static class SetActorId
        //{
        //    public static bool Prefix(int id)
        //    {
        //        Log("SetActorId");
        //        Log(""+id);
        //        return true;
        //    }
        //}
        //[HarmonyPatch(typeof(QuquBattleSystem), "SetActorBody")]
        //public static class SetActorBody
        //{
        //    public static bool Prefix()
        //    {
        //        Log("SetActorBody");
        //        return true;
        //    }
        //}
        //[HarmonyPatch(typeof(QuquBattleSystem), "ChooseActorQuqu")]
        //public static class ChooseActorQuqu
        //{
        //    public static bool Prefix(int index)
        //    {
        //        Log("ChooseActorQuqu");
        //        Log(""+index);
        //        return true;
        //    }
        //}
        //[HarmonyPatch(typeof(QuquBattleSystem), "CloseItemWindow")]
        //public static class CloseItemWindow
        //{
        //    public static bool Prefix()
        //    {
        //        Log("CloseItemWindow");
        //        return true;
        //    }
        //}
        //[HarmonyPatch(typeof(QuquBattleSystem), "GetItem")]
        //public static class GetItem
        //{
        //    public static bool Prefix()
        //    {
        //        Log("GetItem");
        //        return true;
        //    }
        //}
        //[HarmonyPatch(typeof(QuquBattleSystem), "RemoveItems")]
        //public static class RemoveItems
        //{
        //    public static bool Prefix()
        //    {
        //        Log("RemoveItems");
        //        return true;
        //    }
        //}
        //[HarmonyPatch(typeof(QuquBattleSystem), "SetItem")]
        //public static class SetItem
        //{
        //    public static bool Prefix()
        //    {
        //        Log("SetItem");
        //        return true;
        //    }
        //}
        //[HarmonyPatch(typeof(QuquBattleSystem), "RemoveItem")]
        //public static class RemoveItem
        //{
        //    public static bool Prefix()
        //    {
        //        Log("RemoveItem");
        //        return true;
        //    }
        //}
        //[HarmonyPatch(typeof(QuquBattleSystem), "GetItemIcon")]
        //public static class GetItemIcon
        //{
        //    public static bool Prefix(int id)
        //    {
        //        Log("GetItemIcon");
        //        Log(""+id);
        //        return true;
        //    }
        //}
        //[HarmonyPatch(typeof(QuquBattleSystem), "UpdateCanStartBattle")]
        //public static class UpdateCanStartBattle
        //{
        //    public static bool Prefix()
        //    {
        //        Log("UpdateCanStartBattle");
        //        return true;
        //    }
        //}
        //[HarmonyPatch(typeof(QuquBattleSystem), "StartBattleButton")]
        //public static class StartBattleButton
        //{
        //    public static bool Prefix()
        //    {
        //        Log("StartBattleButton");
        //        return true;
        //    }
        //}
        //[HarmonyPatch(typeof(QuquBattleSystem), "ShowHideQuquImage")]
        //public static class ShowHideQuquImage
        //{
        //    public static bool Prefix(int index)
        //    {
        //        Log("ShowHideQuquImage");
        //        Log(""+index);
        //        return true;
        //    }
        //}
        //[HarmonyPatch(typeof(QuquBattleSystem), "StartQuquBattle")]
        //public static class StartQuquBattle
        //{
        //    public static bool Prefix()
        //    {
        //        Log("StartQuquBattle");
        //        return true;
        //    }
        //}
        //[HarmonyPatch(typeof(QuquBattleSystem), "ShowStartBattleState")]
        //public static class ShowStartBattleState
        //{
        //    public static bool Prefix()
        //    {
        //        Log("ShowStartBattleState");
        //        return true;
        //    }
        //}
        //[HarmonyPatch(typeof(QuquBattleSystem), "StartBattle")]
        //public static class StartBattle
        //{
        //    public static bool Prefix(float waitTime, int stateIndex)
        //    {
        //        Log("StartBattle");
        //        Log(""+waitTime);
        //        Log(""+stateIndex);
        //        return true;
        //    }
        //}
        //[HarmonyPatch(typeof(QuquBattleSystem), "SetBattleStateText")]
        //public static class SetBattleStateText
        //{
        //    public static bool Prefix(string text, float delay, float size, int endTyp = -1)
        //    {
        //        Log("SetBattleStateText");
        //        Log(""+text);
        //        Log(""+delay);
        //        Log(""+size);
        //        Log(""+endTyp);
        //        return true;
        //    }
        //}
        //[HarmonyPatch(typeof(QuquBattleSystem), "BattleState")]
        //public static class BattleState
        //{
        //    public static bool Prefix(int endTyp, float waitTime)
        //    {
        //        Log("BattleState");
        //        Log(""+endTyp);
        //        Log(""+waitTime);
        //        return true;
        //    }
        //}
        //[HarmonyPatch(typeof(QuquBattleSystem), "BattleNextPart")]
        //public static class BattleNextPart
        //{
        //    public static bool Prefix()
        //    {
        //        Log("BattleNextPart");
        //        return true;
        //    }
        //}
        //[HarmonyPatch(typeof(QuquBattleSystem), "QuquIsDead")]
        //public static class QuquIsDead
        //{
        //    public static bool Prefix()
        //    {
        //        Log("QuquIsDead");
        //        return true;
        //    }
        //}
        //[HarmonyPatch(typeof(QuquBattleSystem), "AddQuquBattleMassage")]
        //public static class AddQuquBattleMassage
        //{
        //    public static bool Prefix(bool win)
        //    {
        //        Log("AddQuquBattleMassage");
        //        Log(""+win);
        //        return true;
        //    }
        //}
        //[HarmonyPatch(typeof(QuquBattleSystem), "QuquBattleLoopStart")]
        //public static class QuquBattleLoopStart
        //{
        //    public static bool Prefix(float jumpSpeed, float delay)
        //    {
        //        Log("QuquBattleLoopStart");
        //        Log(""+jumpSpeed);
        //        Log(""+delay);
        //        return true;
        //    }
        //}
        //[HarmonyPatch(typeof(QuquBattleSystem), "QuquAttack")]
        //public static class QuquAttack
        //{
        //    public static bool Prefix(float baseDelay, int attacker, bool newTurn)
        //    {
        //        Log("QuquAttack");
        //        Log(""+baseDelay);
        //        Log(""+attacker);
        //        Log(""+newTurn);
        //        return true;
        //    }
        //}
        //[HarmonyPatch(typeof(QuquBattleSystem), "QuquBaseAttacke")]
        //public static class QuquBaseAttacke
        //{
        //    public static bool Prefix(int attacker, bool newTurn)
        //    {
        //        Log("QuquBaseAttacke");
        //        Log(""+attacker);
        //        Log(""+newTurn);
        //        return true;
        //    }
        //}
        //[HarmonyPatch(typeof(QuquBattleSystem), "ShowDamage")]
        //public static class ShowDamage
        //{
        //    public static bool Prefix(bool attacker, bool defer, int typ, int damage, float delay, bool cHit, bool def, bool reAttack, bool isReAttack, bool newTurn, int reAttackTutn, int reAttackTyp)
        //    {
        //        Log("ShowDamage");
        //        Log(""+attacker);
        //        Log(""+defer);
        //        Log(""+typ);
        //        Log(""+damage);
        //        Log(""+delay);
        //        Log(""+cHit);
        //        Log(""+def);
        //        Log(""+reAttack);
        //        Log(""+isReAttack);
        //        Log(""+newTurn);
        //        Log(""+reAttackTutn);
        //        Log(""+reAttackTyp);
        //        return true;
        //    }
        //}
        //[HarmonyPatch(typeof(QuquBattleSystem), "Damage")]
        //public static class Damage
        //{
        //    public static bool Prefix(bool isActor, float size, float delay, string damageText, Color textColor)
        //    {
        //        Log("Damage");
        //        Log(""+isActor);
        //        Log(""+size);
        //        Log(""+delay);
        //        Log(""+damageText);
        //        Log(""+textColor);
        //        return true;
        //    }
        //}
        //[HarmonyPatch(typeof(QuquBattleSystem), "RestBattlerPosition")]
        //public static class RestBattlerPosition
        //{
        //    public static bool Prefix()
        //    {
        //        Log("RestBattlerPosition");
        //        return true;
        //    }
        //}
        //[HarmonyPatch(typeof(QuquBattleSystem), "BattleEndWindow")]
        //public static class BattleEndWindow
        //{
        //    public static bool Prefix(int typ)
        //    {
        //        Log("BattleEndWindow");
        //        Log(""+typ);
        //        return true;
        //    }
        //}
        //[HarmonyPatch(typeof(QuquBattleSystem), "QuquBattleWin")]
        //public static class QuquBattleWin
        //{
        //    public static bool Prefix(float waitTime, int typ)
        //    {
        //        Log("QuquBattleWin");
        //        Log(""+waitTime);
        //        Log(""+typ);
        //        return true;
        //    }
        //}
        //// [HarmonyPatch(typeof(QuquBattleSystem), "UpdateBattleQuquCall")]
        //// public static class UpdateBattleQuquCall
        //// {
        ////     public static bool Prefix()
        ////     {
        ////         Log("UpdateBattleQuquCall");
        ////         return true;
        ////     }
        //// }
        //[HarmonyPatch(typeof(QuquBattleSystem), "Awake")]
        //public static class Awake
        //{
        //    public static bool Prefix()
        //    {
        //        Log("Awake");
        //        return true;
        //    }
        //}
        //[HarmonyPatch(typeof(QuquBattleSystem), "Start")]
        //public static class Start
        //{
        //    public static bool Prefix()
        //    {
        //        Log("Start");
        //        return true;
        //    }
        //}
        //// [HarmonyPatch(typeof(QuquBattleSystem), "Update")]
        //// public static class Update
        //// {
        ////     public static bool Prefix()
        ////     {
        ////         Log("Update");
        ////         return true;
        ////     }
        //// }
    }
}