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

            GUILayout.Label("color:");
            int.TryParse(GUILayout.TextField(qu_color.ToString()), out qu_color);
            GUILayout.Label("partId:");
            int.TryParse(GUILayout.TextField(partId.ToString()), out partId);
            if (GUILayout.Button("测试"))
            {
                foreach (var dic in DateFile.instance.cricketBattleDate)
                {
                    Log(dic.Key.ToString());
                    foreach (var item in dic.Value)
                    {
                        Log(" === " + dic.Key.ToString() + ":" + dic.Value);
                    }
                }
                try
                {
                    int item_id = DateFile.instance.MakeNewItem(10000, -10);
                    GetQuquWindow.instance.MakeQuqu(item_id, qu_color, partId);
                    DateFile.instance.GetItem(DateFile.instance.MianActorID(), item_id, 1, false);
                }
                catch (Exception e)
                {
                    Log(e.Message);
                }
            }
        }
        static int qu_color = 0;
        static int partId = 0;

        #endregion




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


        [HarmonyPatch(typeof(DateFile), "MakeNewItem")]
        public static class MakeNewItem
        {
            public static int Prefix(int id, int temporaryId = 0, int bookObbs = 0, int buffObbs = 50, int sBuffObbs = 20)
            {
                DateFile self = DateFile.instance;
                Log("id=" + id + " temporaryId=" + temporaryId + " bookObbs=" + bookObbs + " buffObbs=" + buffObbs + " sBuffObbs=" + sBuffObbs);
                foreach (var item in self.presetitemDate[id])
                {
                    Log(item.Key + ":" + item.Value);
                }

                int num = 0;
                Dictionary<int, string> value = new Dictionary<int, string>();
                if (temporaryId < 0)
                {
                    num = temporaryId;
                    self.itemsDate.Remove(temporaryId);
                    self.itemsChangeDate.Remove(temporaryId);
                }
                else
                {
                    self.newItemId++;
                    num = self.newItemId;
                }
                Log("new item_id = " + num);

                self.itemsDate.Add(num, value);
                Log("999 id = " + id);
                self.itemsDate[num][999] = id.ToString();
                Log("presetitemDate 31 = " + self.presetitemDate[id][31]);
                bool flag = self.ParseInt(self.presetitemDate[id][31]) > 0;
                Log("presetitemDate 5 = " + self.presetitemDate[id][5]);
                int num2 = self.ParseInt(self.presetitemDate[id][5]);
                bool flag2 = num2 != 36 && num2 != 42;
                Log("presetitemDate 6 = " + self.presetitemDate[id][6]);
                int num3 = ((num2 == 34 || num2 == 35) && self.ParseInt(self.presetitemDate[id][6]) != 1) ? 20 : 0;
                Log("presetitemDate 1 = " + self.presetitemDate[id][1]);
                int num4 = (flag2 && !flag) ? self.ParseInt(self.presetitemDate[id][1]) : 0;
                Log("presetitemDate 902 = " + self.presetitemDate[id][902]);
                int num5 = self.ParseInt(self.presetitemDate[id][902]);
                if (num5 > 0)
                {
                    if (flag2)
                    {
                        num5 = 1 + num5 * UnityEngine.Random.Range(50, 101) / 100;
                    }
                    self.itemsDate[num][902] = num5.ToString();
                    self.itemsDate[num][901] = num5.ToString();
                }
                else if (num5 < 0)
                {
                    self.itemsDate[num][902] = Mathf.Abs(num5).ToString();
                    self.itemsDate[num][901] = Mathf.Abs(num5).ToString();
                }
                Log("switch num4 装备类型 = " + num4);
                switch (num4) // 
                {
                    case 1:
                        {
                            if (self.ParseInt(self.presetitemDate[id][606]) > 0)
                            {
                                string text = "";
                                List<string> list2 = new List<string>(self.presetitemDate[id][7].Split('|'));
                                int count = list2.Count;
                                for (int i = 0; i < count; i++)
                                {
                                    int index = UnityEngine.Random.Range(0, list2.Count);
                                    text += ((i == 0) ? list2[index] : ("|" + list2[index]));
                                    list2.Remove(list2[index]);
                                }
                                self.itemsDate[num][7] = text;
                            }
                            if (UnityEngine.Random.Range(0, 100) < buffObbs)
                            {
                                int num6 = UnityEngine.Random.Range(0, 3) + 1;
                                if (buffObbs == 999)
                                {
                                    num6 *= 100;
                                }
                                self.itemsDate[num][505] = num6.ToString();
                            }
                            int num7 = self.ParseInt(self.presetitemDate[id][504]);
                            if (num7 != 0 && UnityEngine.Random.Range(0, 100) < sBuffObbs)
                            {
                                List<int> list3 = new List<int>
                {
                    2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 401, 402, 403, 404, 407
                };
                                if (num7 < 0)
                                {
                                    list3.Add(Mathf.Abs(num7));
                                }
                                int num8 = self.ParseInt(self.presetitemDate[id][506]);
                                if (num8 != 0)
                                {
                                    list3.Add(209 + num8);
                                }
                                ChangeNewItemSPower(id, num, list3[UnityEngine.Random.Range(0, list3.Count)]);
                            }
                            break;
                        }
                    case 2:
                        if (UnityEngine.Random.Range(0, 100) < buffObbs)
                        {
                            self.itemsDate[num][51361 + UnityEngine.Random.Range(0, 6)] = (Mathf.Max(self.ParseInt(self.presetitemDate[id][8]) / 2, 1) * ((buffObbs == 999) ? 10 : 5)).ToString();
                        }
                        if (UnityEngine.Random.Range(0, 100) < sBuffObbs)
                        {
                            List<int> list4 = new List<int>
                {
                    401, 402, 405, 406, 407
                };
                            ChangeNewItemSPower(id, num, list4[UnityEngine.Random.Range(0, list4.Count)]);
                        }
                        break;
                    case 3:
                        num3 = ((buffObbs == 999) ? 10 : 5);
                        if (UnityEngine.Random.Range(0, 100) < sBuffObbs)
                        {
                            List<int> list = new List<int>
                {
                    407
                };
                            ChangeNewItemSPower(id, num, list[UnityEngine.Random.Range(0, list.Count)]);
                        }
                        break;
                }
                if (num3 != 0 && UnityEngine.Random.Range(0, 100) < buffObbs)
                {
                    if (UnityEngine.Random.Range(0, 100) < 50)
                    {
                        self.itemsDate[num][50501 + UnityEngine.Random.Range(0, 16)] = (Mathf.Max(self.ParseInt(self.presetitemDate[id][8]) / 2, 1) * num3).ToString();
                    }
                    else
                    {
                        self.itemsDate[num][50601 + UnityEngine.Random.Range(0, 14)] = (Mathf.Max(self.ParseInt(self.presetitemDate[id][8]) / 2, 1) * num3).ToString();
                    }
                }
                if (flag)
                {
                    self.SetBookPage(num, bookObbs);
                }
                return num;
            }
            static void  ChangeNewItemSPower(int baseItemId, int itemId, int powerId)
            {
                DateFile self = DateFile.instance;
                int num = self.ParseInt(self.itemPowerDate[powerId][902]);
                int num2 = self.ParseInt(self.itemPowerDate[powerId][501]);
                int num3 = self.ParseInt(self.itemPowerDate[powerId][601]);
                int num4 = self.ParseInt(self.itemPowerDate[powerId][603]);
                int num5 = self.ParseInt(self.itemPowerDate[powerId][904]);
                int num6 = self.ParseInt(self.itemPowerDate[powerId][905]);
                if (num != 0)
                {
                    int num7 = Mathf.Max(self.ParseInt(self.GetItemDate(itemId, 902)) * (100 + num) / 100, 1);
                    self.itemsDate[itemId][901] = num7.ToString();
                    self.itemsDate[itemId][902] = num7.ToString();
                }
                if (num2 != 0)
                {
                    self.itemsDate[itemId][501] = Mathf.Max(self.ParseInt(self.presetitemDate[baseItemId][501]) * (100 + num2) / 100, 1).ToString();
                }
                if (num3 != 0)
                {
                    self.itemsDate[itemId][601] = Mathf.Max(self.ParseInt(self.presetitemDate[baseItemId][601]) * (100 + num3) / 100, 1).ToString();
                }
                if (num4 != 0)
                {
                    self.itemsDate[itemId][603] = Mathf.Max(self.ParseInt(self.presetitemDate[baseItemId][603]) * (100 + num4) / 100, 1).ToString();
                }
                if (num5 != 0)
                {
                    self.itemsDate[itemId][904] = Mathf.Max(self.ParseInt(self.presetitemDate[baseItemId][904]) * (100 + num5) / 100, 1).ToString();
                }
                if (num6 != 0)
                {
                    self.itemsDate[itemId][905] = Mathf.Max(self.ParseInt(self.presetitemDate[baseItemId][905]) * (100 + num6) / 100, 1).ToString();
                }
                self.itemsDate[itemId][504] = powerId.ToString();
            }
        }


    }
}