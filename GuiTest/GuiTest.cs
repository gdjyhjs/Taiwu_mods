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

        static string title = "鬼的测试";
        public static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            enabled = value;
            return true;
        }
        static void OnSaveGUI(UnityModManager.ModEntry modEntry)
        {
            settings.Save(modEntry);
        }



        static void OnGUI(UnityModManager.ModEntry modEntry)
        {


            //if (GUILayout.Button("测试"))
            //{
            //    ActorMenu.instance.listActorsHolder.root.gameObject.SetActive(!ActorMenu.instance.listActorsHolder.root.gameObject.activeSelf);
            //    Main.Logger.Log(ActorMenu.instance.listActorsHolder.root.ToString());
            //    Main.Logger.Log(ActorMenu.instance.listActorsHolder.ToString());
            //}

            //if (GUILayout.Button("打印"))
            //{
            //    GuiBaseUI.Main.LogAllChild(ActorMenu.instance.listActorsHolder.root);

            //}


        }

        class TestClick : MonoBehaviour
        {
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
                GUILayout.Label(title, GUILayout.Width(300));
                GUILayout.Label("x:", GUILayout.Width(50));
                int.TryParse(GUILayout.TextField(x.ToString(), GUILayout.Width(50)), out x);
                GUILayout.Label("y:", GUILayout.Width(50));
                int.TryParse(GUILayout.TextField(y.ToString(), GUILayout.Width(50)), out y);
                GUILayout.Label("w:", GUILayout.Width(50));
                int.TryParse(GUILayout.TextField(w.ToString(), GUILayout.Width(50)), out w);
                GUILayout.Label("h:", GUILayout.Width(50));
                int.TryParse(GUILayout.TextField(h.ToString(), GUILayout.Width(50)), out h);
                draw_xywh = GUILayout.Toggle(draw_xywh, "开关", GUILayout.Width(50));
                if (GUILayout.Button("测试", GUILayout.Width(50)))
                {
                    Main.Logger.Log("路径： "+mod_path);
                    //从文件夹里加载包
                    var my_ui_ab = AssetBundle.LoadFromFile(mod_path+@"\ui.assetbundle");
                    if (my_ui_ab == null)
                    {
                        Main.Logger.Log("error : Failed to load AssetBundle!");
                    }
                    else
                    {
                        //从Bundle包中加载名字为：ququ_adventure 的资源，加载为 GameObject
                        var prefab = my_ui_ab.LoadAsset<GameObject>("ququ_adventure");
                        GameObject go = Instantiate(prefab);
                        Canvas canvas = FindObjectOfType<Canvas>();
                        go.transform.SetParent(canvas.transform, false);
                        Text t = go.GetComponentInChildren<Text>();
                        t.font = DateFile.instance.font;
                        t.text = "太吾世界 蛐蛐 角斗场";
                        // asset 包用完就删 节约内存
                        my_ui_ab.Unload(true);
                    }
                }
                if (GUILayout.Button("图片", GUILayout.Width(50)))
                {
                    Canvas canvas = FindObjectOfType<Canvas>();
                    List<UIImage> allImage = new List<UIImage>();
                    SaveAllImage(canvas.transform, allImage);
                    for (int i = 0; i < allImage.Count; i++)
                    {
                        var item = allImage[i];
                        Sprite sprite = item.sprite;
                        var texture = sprite.texture;
                    }
                }

                // 1280 720
                // 背包 265 295 505 290    
                // 装备 265 390 505 230
                // 同道 75 75 155 570
                // 背包 0.205 0.405 0.394 0.402
                // 装备 0.205 0.541 0.394 0.319
                // 同道 0.058 0.104 0.121 0.791

                if (draw_xywh)
                {
                    GUI.Box(new Rect(x, y, w, h), "");
                }

            }
        }

        class UIImage
        {
            public Sprite sprite;
            public string ui_path;
        }
        static void SaveAllImage(Transform tf, List<UIImage> allImage = null, string path = "")
        {
            if(null == allImage)
            {
                allImage = new List<UIImage>();
            }
            string s = path;
            s += "/"+tf.name;
            Image img = tf.GetComponent<Image>();
            if (img&&img.sprite)
            {
                allImage.Add(new UIImage() { sprite = img.sprite, ui_path = path });
            }
            for (int i = 0; i < tf.childCount; i++)
            {
                Transform child = tf.GetChild(i);
                SaveAllImage(child, allImage, path);
            }
        }
        static void SaveTextureToFile(Texture2D texture, string fileName)
        {
            var bytes = texture.EncodeToPNG();
            var file = File.Open(mod_path + "/" + fileName+".png", FileMode.Create);
            var binary = new BinaryWriter(file);
            binary.Write(bytes);
            file.Close();
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

        //[HarmonyPatch(typeof(DateFile), "ChangeTwoActorItem")]
        //public static class DateFile_ChangeTwoActorItem_Patch
        //{
        //    public static void Postfix(int loseItemActorId, int getItemActorId, int itemId, int itemNumber = 1, int getTyp = -1, int partId = 0, int placeId = 0)
        //    {
        //        Main.Logger.Log("massageItemTyp=" + MassageWindow.instance.massageItemTyp);
        //        Main.Logger.Log(loseItemActorId + " " + getItemActorId + " " + itemId + " " + itemNumber + " " + getTyp + " " + partId + placeId);
        //        StackTrace st = new StackTrace(true);

        //        Main.Logger.Log(st.ToString());
        //    }
        //}

        //[HarmonyPatch(typeof(DateFile), "SetActorEquip")]
        //public static class DateFile_SetActorEquip_Patch
        //{
        //    public static void Postfix(int key, int equipIndex, int newEquipId)
        //    {
        //        Main.Logger.Log(key + "穿戴装备 " + equipIndex + " 新装备= " + newEquipId);
        //    }
        //}

        [HarmonyPatch(typeof(DropObject), "OnDrop")]
        public static class DropObject_OnDrop_Patch
        {
            public static void Postfix(PointerEventData eventData)
            {
                try
                {
                    Main.Logger.Log("dropObjectTyp = " + DropUpdate.instance.updateId);
                    Main.Logger.Log(eventData.ToString());

                    int id = DateFile.instance.ParseInt(eventData.pointerEnter.gameObject.name.Split(',')[1]);
                    Dictionary<int, string> data1;
                    DateFile.instance.itemsDate.TryGetValue(id, out data1);
                    Main.Logger.Log("打印物品 pointerEnter =============================== ");
                    foreach (var item in data1)
                    {
                        Main.Logger.Log(item.Key + " " + item.Value);
                    }

                    int id2 = DateFile.instance.ParseInt(eventData.pointerDrag.gameObject.name.Split(',')[1]);
                    Dictionary<int, string> data2;
                    DateFile.instance.itemsDate.TryGetValue(id2, out data2);
                    Main.Logger.Log("打印物品 pointerDrag =============================== ");
                    foreach (var item in data2)
                    {
                        Main.Logger.Log(item.Key + " " + item.Value);
                    }
                    Main.Logger.Log("打印完毕  =============================== ");
                }
                catch
                {

                }
            }
        }

        //[HarmonyPatch(typeof(DateFile), "DragObjectEnd")]
        //public static class DateFile_DragObjectEnd_Patch
        //{
        //    public static bool Prefix(int typ)
        //    {
        //        if (!Main.enabled)
        //            return true;

        //            Main.Logger.Log("DateFile_DragObjectEnd_Patch:typ=" + typ);

        //        return true;
        //    }
        //}

        //[HarmonyPatch(typeof(DragObject), "OnBeginDrag")]
        //public static class DragObject_OnBeginDrag_Patch
        //{
        //    public static bool Prefix(PointerEventData eventData)
        //    {
        //        if (!Main.enabled)
        //            return true;

        //        Main.Logger.Log("DateFile_DragObjectEnd_Patch:eventData=" + eventData);

        //        return true;
        //    }
        //}

        //[HarmonyPatch(typeof(DateFile), "ChangeTwoActorItem")]
        //public static class DateFile_ChangeTwoActorItem_Patch
        //{
        //    public static bool Prefix(int loseItemActorId, int getItemActorId, int itemId, int itemNumber = 1, int getTyp = -1, int partId = 0, int placeId = 0)
        //    {
        //        if (!Main.enabled)
        //            return true;

        //        Main.Logger.Log("loseItemActorId=" + loseItemActorId + " getItemActorId=" + getItemActorId + " itemId=" + itemId + " itemNumber=" + itemNumber + " getTyp=" + getTyp + " partId=" + partId + " placeId=" + placeId);

        //        return true;
        //    }
        //}

    }
}