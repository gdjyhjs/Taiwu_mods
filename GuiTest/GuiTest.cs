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
using Random = GuiTest.Main.Random;
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




        static void Log(string str)
        {
            Logger.Log(str);
        }

        static string GetPath(Transform go, string s = "")
        {
            if (s == "")
            {
                s = go.name;
            }
            else
            {
                s = go.name + "/" + s;
            }
            if (go.parent)
            {
                return GetPath(go.parent, s);
            }
            else
            {
                return s;
            }
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

            if (GUILayout.Button("创建UI"))
            {
                if (!obj) // 创建UI
                {
                    //var f_isShowActorMenu = typeof(ActorMenu).GetField("isShowActorMenu", BindingFlags.NonPublic | BindingFlags.Instance);
                    //return (bool)f_isShowActorMenu.GetValue(ActorMenu.instance);
                    QuquBattleSystem self = QuquBattleSystem.instance;
                    LogObj("setBattleSpeedHolder", self.setBattleSpeedHolder);
                    LogList("setBattleSpeedToggle", self.setBattleSpeedToggle);
                    LogObj("ququBattleWindow", self.ququBattleWindow);
                    LogObj("actorBack", self.actorBack);
                    LogObj("enemyBack", self.enemyBack);
                    LogObj("chooseActorButton", self.chooseActorButton);
                    LogList("ququBattleBack", self.ququBattleBack);
                    LogObj("actorFace", self.actorFace);
                    LogObj("enemyFace", self.enemyFace);
                    LogObj("actorNameText", self.actorNameText);
                    LogObj("enemyNameText", self.enemyNameText);
                    LogList("battleStateText", self.battleStateText);
                    LogList("battleValue", self.battleValue);
                    LogList("actorQuqu", self.actorQuqu);
                    LogList("enemyQuqu", self.enemyQuqu);
                    LogObj("startBattleWindow", self.startBattleWindow);
                    LogObj("startBattleButton", self.startBattleButton);
                    LogObj("loseBattleButton", self.loseBattleButton);
                    LogList("uiText", self.uiText);
                    LogObj("actorBodyImage", self.actorBodyImage);
                    LogObj("actorBodyNameText", self.actorBodyNameText);
                    LogObj("enemyBodyImage", self.enemyBodyImage);
                    LogObj("enemyBodyNameText", self.enemyBodyNameText);
                    LogObj("resourceButtonImage", self.resourceButtonImage);
                    LogList("hideQuquImage", self.hideQuquImage);
                    LogList("actorQuquHpText", self.actorQuquHpText);
                    LogList("enemyQuquHpText", self.enemyQuquHpText);
                    LogList("actorQuquIcon", self.actorQuquIcon);
                    LogList("enemyQuquIcon", self.enemyQuquIcon);
                    LogList("actorBattleQuquNameText", self.actorBattleQuquNameText);
                    LogList("actorBattleQuquPower1Text", self.actorBattleQuquPower1Text);
                    LogList("actorBattleQuquPower2Text", self.actorBattleQuquPower2Text);
                    LogList("actorBattleQuquPower3Text", self.actorBattleQuquPower3Text);
                    LogList("actorBattleQuquStrengthText", self.actorBattleQuquStrengthText);
                    LogList("actorBattleQuquMagicText", self.actorBattleQuquMagicText);
                    LogList("actorBattleQuquStrengthBar", self.actorBattleQuquStrengthBar);
                    LogList("actorBattleQuquMagicBar", self.actorBattleQuquMagicBar);
                    LogList("enemyBattleQuquNameText", self.enemyBattleQuquNameText);
                    LogList("enemyBattleQuquPower1Text", self.enemyBattleQuquPower1Text);
                    LogList("enemyBattleQuquPower2Text", self.enemyBattleQuquPower2Text);
                    LogList("enemyBattleQuquPower3Text", self.enemyBattleQuquPower3Text);
                    LogList("enemyBattleQuquStrengthText", self.enemyBattleQuquStrengthText);
                    LogList("enemyBattleQuquMagicText", self.enemyBattleQuquMagicText);
                    LogList("enemyBattleQuquStrengthBar", self.enemyBattleQuquStrengthBar);
                    LogList("enemyBattleQuquMagicBar", self.enemyBattleQuquMagicBar);
                    LogList("actorQuquName", self.actorQuquName);
                    LogList("enemyQuquName", self.enemyQuquName);
                    LogObj("chooseBodyWindow", self.chooseBodyWindow);
                    LogObj("needResourceText", self.needResourceText);
                    LogObj("useResourceButton", self.useResourceButton);
                    LogObj("acotrWindow", self.acotrWindow);
                    LogObj("useActorButton", self.useActorButton);
                    LogObj("actorHolder", self.actorHolder);
                    LogObj("actorIcon", self.actorIcon);
                    LogObj("itemWindow", self.itemWindow);
                    LogObj("useItemButton", self.useItemButton);
                    LogObj("itemMask", self.itemMask);
                    LogObj("itemHolder", self.itemHolder);
                    LogList("nextButton", self.nextButton);
                    LogList("nextButtonMask", self.nextButtonMask);
                    LogList("actorQuquDamageText", self.actorQuquDamageText);
                    LogList("enemyQuquDamageText", self.enemyQuquDamageText);
                    LogObj("battleEndWindow", self.battleEndWindow);
                    LogObj("closeBattleButton", self.closeBattleButton);
                    LogObj("battleEndTypImage", self.battleEndTypImage);
                    LogObj("battleEndBodyName", self.battleEndBodyName);
                    LogObj("battleEndBodyText", self.battleEndBodyText);
                    LogList("actorQuquCall", self.actorQuquCall);
                    LogList("enemyQuquCall", self.enemyQuquCall);

                    obj = GameObject.Instantiate(QuquBattleSystem.instance.gameObject);
                    QuquBattleSystem tmp = obj.GetComponent<QuquBattleSystem>();
                    Object.Destroy(tmp);
                    QuquBattleSystem.instance = self;
                    GuiQuquBattleSystem gui = obj.AddComponent<GuiQuquBattleSystem>();
                    gui.Init();
                    Transform tf = obj.transform;
                    tf.SetParent(self.transform.parent, false);

                    Log(obj.ToString());
                    obj.SetActive(true);
                    Log(GuiQuquBattleSystem.instance.ToString());
                    //obj.SetActive(false);


                    //Random.InitSeed(111);
                    //int num = 123456;
                    //GuiQuquBattleSystem.instance.ququBattleEnemyId = num;
                    //GuiQuquBattleSystem.instance.ququBattleId = 1; // 此处是根据身份来确定蛐蛐战斗ID 那么大家都是太吾就不需要了 20是拿身份 Mathf.Clamp(Mathf.Abs(DateFile.instance.ParseInt(DateFile.instance.GetActorDate(num, 20, false))) + Random.Range(-1, 2), 1, 9);
                    ////bool flag = Random.Range(0, 100) < 20;
                    ////if (flag)
                    ////{
                    ////    GuiQuquBattleSystem.instance.ququBattleId += 10;
                    ////}
                    //GuiQuquBattleSystem.instance.ShowQuquBattleWindow();
                }
            }





        }




        public static class Random
        {
            static int idx;
            static System.Random random;
            public static void InitSeed(int seed)
            {
                idx = 0;
                random = new System.Random(seed);
            }
            public static int Range(int a, int b)
            {
                int num = random.Next(a, b);
                Log(idx + " 随机数: " + num);
                return num;
            }
        }

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


            //Vector2 scrollPosition;
            //public UIImage[] data;
            //int data_height;
            //int data_width;
            private void OnGUI()
            {
                //GUILayout.Label(title, GUILayout.Width(300));
                //GUILayout.Label("x:", GUILayout.Width(50));
                //int.TryParse(GUILayout.TextField(x.ToString(), GUILayout.Width(50)), out x);
                //GUILayout.Label("y:", GUILayout.Width(50));
                //int.TryParse(GUILayout.TextField(y.ToString(), GUILayout.Width(50)), out y);
                //GUILayout.Label("w:", GUILayout.Width(50));
                //int.TryParse(GUILayout.TextField(w.ToString(), GUILayout.Width(50)), out w);
                //GUILayout.Label("h:", GUILayout.Width(50));
                //int.TryParse(GUILayout.TextField(h.ToString(), GUILayout.Width(50)), out h);
                //draw_xywh = GUILayout.Toggle(draw_xywh, "开关", GUILayout.Width(50));
                //if (GUILayout.Button("测试", GUILayout.Width(50)))
                //{
                //    Main.Logger.Log("路径： "+mod_path);
                //    //从文件夹里加载包
                //    var my_ui_ab = AssetBundle.LoadFromFile(mod_path+@"\ui.assetbundle");
                //    if (my_ui_ab == null)
                //    {
                //        Main.Logger.Log("error : Failed to load AssetBundle!");
                //    }
                //    else
                //    {
                //        //从Bundle包中加载名字为：ququ_adventure 的资源，加载为 GameObject
                //        var prefab = my_ui_ab.LoadAsset<GameObject>("ququ_adventure");
                //        GameObject go = Instantiate(prefab);
                //        Canvas canvas = FindObjectOfType<Canvas>();
                //        go.transform.SetParent(canvas.transform, false);
                //        Text t = go.GetComponentInChildren<Text>();
                //        t.font = DateFile.instance.font;
                //        t.text = "太吾世界 蛐蛐 角斗场";
                //        // asset 包用完就删 节约内存
                //        my_ui_ab.Unload(true);
                //    }
                //}

                //if (null != this.data)
                //{

                //    int height = 0;
                //    height = 0;
                //    scrollPosition = GUI.BeginScrollView(new Rect(0, 0, Screen.width, Screen.height), scrollPosition, new Rect(0, 0, data_width, data_height), false, true);
                //    foreach (var item in this.data)
                //    {
                //        GUI.Label(new Rect(0, height, Screen.width, 40), item.ui_path);
                //        height += 40;
                //        Texture2D texture = item.sprite.texture;
                //        GUI.DrawTexture(new Rect(0, height, texture.width, texture.height), texture);
                //        height += texture.height;
                //    }
                //    GUI.EndScrollView();
                //}
                //else
                //{
                //    if (GUILayout.Button("图片", GUILayout.Width(50)))
                //    {
                //        int idx = 0;
                //        png_list = new List<UIImage>();
                //        Canvas canvas = FindObjectOfType<Canvas>();
                //        while (canvas)
                //        {
                //            Dictionary<Sprite, UIImage> data = new Dictionary<Sprite, UIImage>();
                //            Image[] images = canvas.transform.root.GetComponentsInChildren<Image>();
                //            foreach (var item in images)
                //            {
                //                Sprite sprite = item.sprite;
                //                if (sprite && !data.ContainsKey(sprite))
                //                {
                //                    string ui_path = "";
                //                    Transform tf = item.transform;
                //                    while (tf)
                //                    {
                //                        ui_path = "/" + tf.name + ui_path;
                //                        tf = tf.parent;
                //                    }
                //                    UIImage uIImage = new UIImage() { sprite = sprite, ui_path = ui_path };
                //                    Main.Logger.Log(idx++ + " 圖片 : " + sprite.name + " " + ui_path);
                //                    data_height += sprite.texture.height;
                //                    if (data_width < sprite.texture.width)
                //                        data_width = sprite.texture.width;
                //                    data.Add(item.sprite, uIImage);
                //                    png_list.Add(uIImage);
                //                }
                //            }
                //            Main.Logger.Log(canvas.name + " 圖片總數 : " + png_list.Count);
                //            var xx = canvas.gameObject.GetComponent<CanvasScaler>();
                //            if (xx)
                //            {
                //                Main.Logger.Log(xx.uiScaleMode.ToString());
                //                Main.Logger.Log(xx.referenceResolution .ToString());
                //                Main.Logger.Log(xx.screenMatchMode .ToString());
                //                Main.Logger.Log(xx.matchWidthOrHeight.ToString());
                //                Main.Logger.Log(xx.referencePixelsPerUnit.ToString());
                //            }
                //            else
                //            {
                //                Main.Logger.Log("木有 xx");
                //            }



                //            canvas.transform.root.gameObject.SetActive(false);
                //            canvas = FindObjectOfType<Canvas>();
                //        }
                //        this.data = png_list.ToArray();
                //        Main.Logger.Log(" 图片 : " + this.data.Length);
                //    }
                //}

                // 1280 720
                // 背包 265 295 505 290    
                // 装备 265 390 505 230
                // 同道 75 75 155 570
                // 背包 0.205 0.405 0.394 0.402
                // 装备 0.205 0.541 0.394 0.319
                // 同道 0.058 0.104 0.121 0.791

                //if (draw_xywh)
                //{
                //    GUI.Box(new Rect(x, y, w, h), "");
                //}

            }
        }

        //class UIImage
        //{
        //    public Sprite sprite;
        //    public string ui_path;
        //}
        //static void SaveAllImage(Transform tf, List<UIImage> allImage = null, string path = "")
        //{
        //    if(null == allImage)
        //    {
        //        allImage = new List<UIImage>();
        //    }
        //    string s = path;
        //    s += "/"+tf.name;
        //    Image img = tf.GetComponent<Image>();
        //    if (img&&img.sprite)
        //    {
        //        allImage.Add(new UIImage() { sprite = img.sprite, ui_path = path });
        //    }
        //    for (int i = 0; i < tf.childCount; i++)
        //    {
        //        Transform child = tf.GetChild(i);
        //        SaveAllImage(child, allImage, path);
        //    }
        //}
        //static void SaveTextureToFile(Sprite sprite, string fileName)
        //{
        //    try
        //    {
        //        string path = mod_path + "/" + fileName + ".png";
        //        Main.Logger.Log(texture + " 保存图片 " + path);
        //        var bytes = sprite.;
        //        var file = File.Open(mod_path + "/" + path + ".png", FileMode.Create);
        //        var binary = new BinaryWriter(file);
        //        binary.Write(bytes);
        //        file.Close();
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}


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

        //[HarmonyPatch(typeof(DropObject), "OnDrop")]
        //public static class DropObject_OnDrop_Patch
        //{
        //    public static void Postfix(PointerEventData eventData)
        //    {
        //        try
        //        {
        //            Main.Logger.Log("dropObjectTyp = " + DropUpdate.instance.updateId);
        //            Main.Logger.Log(eventData.ToString());

        //            int id = DateFile.instance.ParseInt(eventData.pointerEnter.gameObject.name.Split(',')[1]);
        //            Dictionary<int, string> data1;
        //            DateFile.instance.itemsDate.TryGetValue(id, out data1);
        //            Main.Logger.Log("打印物品 pointerEnter =============================== ");
        //            foreach (var item in data1)
        //            {
        //                Main.Logger.Log(item.Key + " " + item.Value);
        //            }

        //            int id2 = DateFile.instance.ParseInt(eventData.pointerDrag.gameObject.name.Split(',')[1]);
        //            Dictionary<int, string> data2;
        //            DateFile.instance.itemsDate.TryGetValue(id2, out data2);
        //            Main.Logger.Log("打印物品 pointerDrag =============================== ");
        //            foreach (var item in data2)
        //            {
        //                Main.Logger.Log(item.Key + " " + item.Value);
        //            }
        //            Main.Logger.Log("打印完毕  =============================== ");
        //        }
        //        catch
        //        {

        //        }
        //    }
        //}

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

        //[HarmonyPatch(typeof(ActorFace), "UpdateFace")]
        //public static class ActorFace_UpdateFace_patch
        //{
        //    public static bool Prefix(int actorId, int age, int gender, int actorGenderChange, int[] faceDate, int[] faceColor, int clotheIndex, bool life = false)
        //    {

        //        Main.Logger.Log("actorId=" + actorId + " age=" + age + " gender=" + gender + " actorGenderChange=" + actorGenderChange + " clotheIndex=" + clotheIndex + " life=" + life);
        //        Log(" faceDate_length=" + faceDate.Length + " faceColor_length=" + faceColor.Length);
        //        foreach (var item in faceDate)
        //        {
        //            Log(item.ToString());
        //        }
        //        foreach (var item in faceColor)
        //        {
        //            Log(item.ToString());
        //        }


        //        return true;
        //    }
        //}



        class GuiQuquBattleSystem : MonoBehaviour
        {
            public void Init()
            {

            }

            public void SetQuquBattleSpeed(int value)
            {
                Time.timeScale = (float)value;
            }

            private void SetTweener()
            {
                Tweener tweener = TweenSettingsExtensions.SetUpdate<Tweener>(TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOLocalMoveX(this.itemWindow.GetComponent<RectTransform>(), 0f, 0.2f, false), (Ease)27), true);
                TweenSettingsExtensions.SetAutoKill<Tweener>(tweener, false);
                TweenExtensions.Pause<Tweener>(tweener);
                Tweener tweener2 = TweenSettingsExtensions.SetUpdate<Tweener>(this.chooseBodyWindow.DOSizeDelta(new Vector2(280f, 120f), 0.2f, false), true);
                TweenSettingsExtensions.SetAutoKill<Tweener>(tweener2, false);
                TweenExtensions.Pause<Tweener>(tweener2);
                Tweener tweener3 = TweenSettingsExtensions.SetUpdate<Tweener>(TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOLocalMoveX(this.acotrWindow.GetComponent<RectTransform>(), 0f, 0.2f, false), (Ease)27), true);
                TweenSettingsExtensions.SetAutoKill<Tweener>(tweener3, false);
                TweenExtensions.Pause<Tweener>(tweener3);
            }

            // Token: 0x060007B5 RID: 1973 RVA: 0x000C4768 File Offset: 0x000C2968
            public void ShowQuquBattleWindow()
            {
                bool flag = !this.showQuquBattleWindow;
                if (flag)
                {
                    this.showQuquBattleWindow = true;
                    for (int i = 0; i < this.setBattleSpeedToggle.Length; i++)
                    {
                        this.setBattleSpeedToggle[i].isOn = false;
                    }
                    this.setBattleSpeedToggle[0].isOn = true;
                    Time.timeScale = 1f;
                    this.setBattleSpeedHolder.SetActive(false);
                    TipsWindow.instance.showTipsTime = -100;
                    bool flag2 = StorySystem.instance.itemWindowIsShow;
                    if (flag2)
                    {
                        StorySystem.instance.CloseItemWindow();
                    }
                    bool toStoryIsShow = StorySystem.instance.toStoryIsShow;
                    if (toStoryIsShow)
                    {
                        StorySystem.instance.ClossToStoryMenu();
                    }
                    UIMove.instance.CloseGUI();
                    base.Invoke("QuquBattleWindowOpend", 0.25f);
                }
            }

            // Token: 0x060007B6 RID: 1974 RVA: 0x000C4840 File Offset: 0x000C2A40
            private void QuquBattleWindowOpend()
            {
                this.showQuquBattleWindow = false;
                TipsWindow.instance.showTipsTime = -100;
                YesOrNoWindow.instance.ShwoWindowMask(this.ququBattleWindow.transform, true, 0.75f, 0.2f, false);
                this.battleEndWindow.transform.localScale = new Vector3(5f, 5f, 1f);
                this.battleEndWindow.SetActive(false);
                this.closeBattleButton.SetActive(false);
                this.SetQuquBattle();
                this.actorBack.transform.localPosition = new Vector3(-1400f, 0f, 0f);
                this.enemyBack.transform.localPosition = new Vector3(1400f, 0f, 0f);
                this.ququBattleWindow.SetActive(true);
                this.ququBattleWindow.transform.localPosition = new Vector3(0f, 1200f, 0f);
                TweenSettingsExtensions.OnComplete<Tweener>(TweenSettingsExtensions.SetUpdate<Tweener>(ShortcutExtensions.DOLocalMoveY(this.ququBattleWindow.transform, 0f, 0.2f, false), true), new TweenCallback(this.SetActorAnimation));
                DateFile.instance.SystemAudioPlay(4, 1, 1f);
            }

            // Token: 0x060007B7 RID: 1975 RVA: 0x000C498C File Offset: 0x000C2B8C
            private void SetActorAnimation()
            {
                TweenSettingsExtensions.OnComplete<Tweener>(TweenSettingsExtensions.SetUpdate<Tweener>(TweenSettingsExtensions.SetDelay<Tweener>(ShortcutExtensions.DOLocalMoveX(this.actorBack, -960f, 0.2f, false), 0.1f), true), new TweenCallback(this.SetAnimationDone));
                TweenSettingsExtensions.SetUpdate<Tweener>(TweenSettingsExtensions.SetDelay<Tweener>(ShortcutExtensions.DOLocalMoveX(this.enemyBack, 960f, 0.2f, false), 0.1f), true);
            }

            // Token: 0x060007B8 RID: 1976 RVA: 0x00005332 File Offset: 0x00003532
            private void SetAnimationDone()
            {
                this.ququBattlePart = 1;
                this.loseBattleButton.interactable = true;
            }

            // Token: 0x060007B9 RID: 1977 RVA: 0x000C49FC File Offset: 0x000C2BFC
            public void CloseQuquBattleWindowButton()
            {
                YesOrNoWindow.instance.SetYesOrNoWindow(516, DateFile.instance.massageDate[8002][4].Split(new char[]
                {
            '|'
                })[0], DateFile.instance.massageDate[8002][4].Split(new char[]
                {
            '|'
                })[1] + ((DateFile.instance.ParseInt(DateFile.instance.cricketBattleDate[this.ququBattleId][9]) > 0) ? (DateFile.instance.massageDate[8002][4].Split(new char[]
                {
            '|'
                })[2] + DateFile.instance.cricketBattleDate[this.ququBattleId][9]) : "") + DateFile.instance.massageDate[8002][4].Split(new char[]
                {
            '|'
                })[3], false, true);
            }

            // Token: 0x060007BA RID: 1978 RVA: 0x000C4B24 File Offset: 0x000C2D24
            public void CloseQuquBattleWindow()
            {
                this.battleEndWindow.SetActive(false);
                bool flag = this.surrender;
                if (flag)
                {
                    int num = DateFile.instance.ParseInt(DateFile.instance.cricketBattleDate[this.ququBattleId][9]);
                    UIDate.instance.ChangeResource(DateFile.instance.MianActorID(), 6, -num, false);
                    string[] array = DateFile.instance.cricketBattleDate[this.ququBattleId][7].Split(new char[]
                    {
                '&'
                    });
                    int num2 = DateFile.instance.ParseInt(array[0]);
                    bool flag2 = num2 != 0;
                    if (flag2)
                    {
                        bool flag3 = array.Length > 1;
                        if (flag3)
                        {
                            int num3 = DateFile.instance.ParseInt(array[1]);
                            if (num3 == 1)
                            {
                                DateFile.instance.SetEvent(new int[]
                                {
                            0,
                            this.ququBattleEnemyId,
                            num2,
                            this.ququBattleEnemyId
                                }, true, true);
                            }
                        }
                        else
                        {
                            DateFile.instance.SetEvent(new int[]
                            {
                        0,
                        -1,
                        num2
                            }, true, true);
                        }
                    }
                }
                this.setBattleSpeedHolder.SetActive(false);
                Time.timeScale = 1f;
                this.ququBattlePart = 0;
                TweenSettingsExtensions.OnComplete<Tweener>(TweenSettingsExtensions.SetUpdate<Tweener>(ShortcutExtensions.DOLocalMoveY(this.ququBattleWindow.transform, 1200f, 0.3f, false), true), new TweenCallback(this.CloseBattleWindowDone));
            }

            // Token: 0x060007BB RID: 1979 RVA: 0x000C4C94 File Offset: 0x000C2E94
            private void CloseBattleWindowDone()
            {
                this.bodyActorId = 0;
                this.ququBattleWindow.SetActive(false);
                YesOrNoWindow.instance.ShwoWindowMask(this.ququBattleWindow.transform, false, 0.75f, 0.2f, false);
                UIMove.instance.ShowGUI();
                DateFile.instance.UpdatePlaceBgm(DateFile.instance.mianPartId, DateFile.instance.mianPlaceId);
            }

            // Token: 0x060007BC RID: 1980 RVA: 0x000C4D04 File Offset: 0x000C2F04
            private void SetQuquBattle()
            {
                this.ququBattleTurn = 0;
                this.surrender = false;
                this.actorQuquCallTime = new int[]
                {
            Random.Range(0, 300),
            Random.Range(0, 300),
            Random.Range(0, 300)
                };
                this.enemyQuquCallTime = new int[]
                {
            Random.Range(0, 300),
            Random.Range(0, 300),
            Random.Range(0, 300)
                };
                int num = DateFile.instance.MianActorID();
                int num2 = DateFile.instance.ParseInt(DateFile.instance.cricketBattleDate[this.ququBattleId][1]);
                int num3 = (num2 == 0) ? this.ququBattleEnemyId : num2;
                this.startBattleWindow.SetActive(true);
                for (int i = 0; i < 3; i++)
                {
                    Component[] componentsInChildren = this.ququBattleBack[i].GetComponentsInChildren<Component>();
                    foreach (Component component in componentsInChildren)
                    {
                        bool flag = component is Graphic;
                        if (flag)
                        {
                            (component as Graphic).CrossFadeAlpha(0f, 0f, true);
                        }
                    }
                }
                this.startBattleButton.interactable = false;
                this.loseBattleButton.interactable = false;
                for (int k = 0; k < this.battleValue.Length; k++)
                {
                    this.hideQuquImage[k].transform.localScale = new Vector3(1f, 1f, 1f);
                    this.nextButton[k].gameObject.SetActive(false);
                    this.nextButtonMask[k].GetComponent<Image>().color = new Color(0f, 0f, 0f, 0f);
                    this.battleStateText[k].text = "";
                    this.battleStateText[k].transform.localScale = new Vector3(1f, 1f, 1f);
                    this.battleValue[k].localPosition = new Vector3(0f, 100f, 0f);
                    this.actorQuqu[k].localPosition = new Vector3(0f, 0f, 0f);
                    this.enemyQuqu[k].localPosition = new Vector3(0f, 0f, 0f);
                    this.actorQuquIcon[k].GetComponent<Button>().interactable = true;
                    this.actorQuquDamageText[k].text = "";
                    this.enemyQuquDamageText[k].text = "";
                }
                this.chooseActorButton.interactable = (DateFile.instance.ParseInt(DateFile.instance.cricketBattleDate[this.ququBattleId][8]) == 1);
                this.actorBodyImage.GetComponent<Button>().interactable = true;
                this.actorFace.SetActorFace(num, false);
                this.enemyFace.SetActorFace(num3, false);
                this.actorNameText.text = DateFile.instance.GetActorName(num, false, false);
                this.enemyNameText.text = DateFile.instance.GetActorName(num3, false, false);
                this.enemyBodyTyp = DateFile.instance.ParseInt(DateFile.instance.cricketBattleDate[this.ququBattleId][3]);
                this.enemyBodySize = 1;
                int num4 = DateFile.instance.ParseInt(DateFile.instance.cricketBattleDate[this.ququBattleId][5]);
                bool flag2 = num2 == 0;
                if (flag2)
                {
                    int num5 = this.enemyBodyTyp;
                    if (num5 != 0)
                    {
                        if (num5 == 1)
                        {
                            this.enemyBodyId = 0;
                            List<int> list = new List<int>();
                            List<int> list2 = new List<int>(DateFile.instance.actorItemsDate[num3].Keys);
                            for (int l = 0; l < list2.Count; l++)
                            {
                                int num6 = list2[l];
                                bool flag3 = DateFile.instance.ParseInt(DateFile.instance.GetItemDate(num6, 3, true)) == 1 && DateFile.instance.ParseInt(DateFile.instance.GetItemDate(num6, 8, true)) <= num4;
                                if (flag3)
                                {
                                    list.Add(num6);
                                }
                            }
                            bool flag4 = list.Count > 0;
                            if (flag4)
                            {
                                this.enemyBodyId = list[Random.Range(0, list.Count)];
                            }
                            bool flag5 = this.enemyBodyId != 0;
                            if (flag5)
                            {
                                this.enemyBodyImage.tag = "ActorItem";
                                this.enemyBodyImage.name = "EnemyItemIcon," + this.enemyBodyId;
                                this.enemyBodyImage.GetComponent<Image>().sprite = GetSprites.instance.itemSprites[DateFile.instance.ParseInt(DateFile.instance.GetItemDate(this.enemyBodyId, 98, true))];
                                this.resourceButtonImage.sprite = GetSprites.instance.makeResourceIcon[DateFile.instance.ParseInt(DateFile.instance.resourceDate[5][98])];
                                this.enemyBodyNameText.text = DateFile.instance.SetColoer(20001 + DateFile.instance.ParseInt(DateFile.instance.GetItemDate(this.enemyBodyId, 8, true)), DateFile.instance.GetItemDate(this.enemyBodyId, 0, true), false);
                                this.enemyBodyWorth = DateFile.instance.ParseInt(DateFile.instance.GetItemDate(this.enemyBodyId, 904, true));
                            }
                            else
                            {
                                this.SetResourceBody(num3, num4);
                            }
                        }
                    }
                    else
                    {
                        this.SetResourceBody(num3, num4);
                    }
                }
                else
                {
                    this.enemyBodyId = DateFile.instance.ParseInt(DateFile.instance.cricketBattleDate[this.ququBattleId][4]);
                    switch (this.enemyBodyTyp)
                    {
                        case 0:
                            this.enemyBodySize = num4 + Random.Range(-(num4 * 30 / 100), num4 * 30 / 100 + 1);
                            this.enemyBodyImage.tag = "ResourceIcon";
                            this.enemyBodyImage.name = "ResourceIcon," + this.enemyBodyId;
                            this.enemyBodyImage.sprite = GetSprites.instance.attSprites[DateFile.instance.ParseInt(DateFile.instance.resourceDate[this.enemyBodyId][98])];
                            this.resourceButtonImage.sprite = GetSprites.instance.makeResourceIcon[DateFile.instance.ParseInt(DateFile.instance.resourceDate[this.enemyBodyId][98])];
                            this.enemyBodyNameText.text = DateFile.instance.resourceDate[this.enemyBodyId][1] + DateFile.instance.SetColoer((this.enemyBodyId == 6) ? 20007 : ((this.enemyBodyId == 5) ? 20008 : 20003), "×" + this.enemyBodySize, false);
                            this.enemyBodyWorth = this.enemyBodySize;
                            break;
                        case 1:
                            {
                                bool flag6 = DateFile.instance.ParseInt(DateFile.instance.GetItemDate(this.enemyBodyId, 6, true)) == 0;
                                if (flag6)
                                {
                                    this.enemyBodyId = DateFile.instance.MakeNewItem(this.enemyBodyId, -4, 0, 50, 20);
                                    bool flag7 = DateFile.instance.ParseInt(DateFile.instance.GetItemDate(this.enemyBodyId, 2001, true)) == 1;
                                    if (flag7)
                                    {
                                        string[] array2 = DateFile.instance.cricketBattleDate[this.ququBattleId][5].Split(new char[]
                                        {
                            '|'
                                        });
                                        this.enemyBodySize = ((array2.Length > 1) ? Random.Range(DateFile.instance.ParseInt(array2[0]), DateFile.instance.ParseInt(array2[1])) : DateFile.instance.ParseInt(array2[0]));
                                        List<int> list3 = new List<int>(DateFile.instance.cricketDate.Keys);
                                        int num7 = this.enemyBodySize;
                                        int num8 = this.enemyBodySize;
                                        int num9 = 0;
                                        int num10 = 0;
                                        int partId = 0;
                                        bool flag8 = num7 >= 8;
                                        if (flag8)
                                        {
                                            num8 = 0;
                                            List<int> list4 = new List<int>();
                                            for (int m = 0; m < list3.Count; m++)
                                            {
                                                int num11 = list3[m];
                                                bool flag9 = DateFile.instance.ParseInt(DateFile.instance.cricketDate[num11][1]) == num7;
                                                if (flag9)
                                                {
                                                    for (int n = 0; n < DateFile.instance.ParseInt(DateFile.instance.cricketDate[num11][6]); n++)
                                                    {
                                                        list4.Add(num11);
                                                    }
                                                }
                                            }
                                            num9 = list4[Random.Range(0, list4.Count)];
                                        }
                                        else
                                        {
                                            bool flag10 = num7 >= 7;
                                            if (flag10)
                                            {
                                                num7 = Mathf.Clamp(num7 - Random.Range(0, num7 / 2), 1, 6);
                                            }
                                            else
                                            {
                                                bool flag11 = Random.Range(0, 100) < 60;
                                                if (flag11)
                                                {
                                                    num8 = Mathf.Clamp(num8 - Random.Range(0, num8 / 2), 1, 6);
                                                }
                                                else
                                                {
                                                    num7 = Mathf.Clamp(num7 - Random.Range(0, num7 / 2), 1, 6);
                                                }
                                            }
                                        }
                                        bool flag12 = num9 == 0;
                                        if (flag12)
                                        {
                                            List<int> list5 = new List<int>();
                                            for (int num12 = 0; num12 < list3.Count; num12++)
                                            {
                                                int num13 = list3[num12];
                                                bool flag13 = DateFile.instance.ParseInt(DateFile.instance.cricketDate[num13][3]) != 0 && DateFile.instance.ParseInt(DateFile.instance.cricketDate[num13][1]) == num7;
                                                if (flag13)
                                                {
                                                    for (int num14 = 0; num14 < DateFile.instance.ParseInt(DateFile.instance.cricketDate[num13][6]); num14++)
                                                    {
                                                        list5.Add(num13);
                                                    }
                                                }
                                            }
                                            num10 = list5[Random.Range(0, list5.Count)];
                                        }
                                        bool flag14 = num8 > 0;
                                        if (flag14)
                                        {
                                            List<int> list6 = new List<int>();
                                            for (int num15 = 0; num15 < list3.Count; num15++)
                                            {
                                                int num16 = list3[num15];
                                                bool flag15 = DateFile.instance.ParseInt(DateFile.instance.cricketDate[num16][4]) == 1 && DateFile.instance.ParseInt(DateFile.instance.cricketDate[num16][1]) == num8;
                                                if (flag15)
                                                {
                                                    for (int num17 = 0; num17 < DateFile.instance.ParseInt(DateFile.instance.cricketDate[num16][6]); num17++)
                                                    {
                                                        list6.Add(num16);
                                                    }
                                                }
                                            }
                                            partId = list6[Random.Range(0, list6.Count)];
                                        }
                                        GetQuquWindow.instance.MakeQuqu(this.enemyBodyId, (num9 > 0) ? num9 : num10, partId);
                                    }
                                }
                                this.enemyBodyImage.tag = "ActorItem";
                                this.enemyBodyImage.name = "EnemyItemIcon," + this.enemyBodyId;
                                this.enemyBodyImage.GetComponent<Image>().sprite = GetSprites.instance.itemSprites[DateFile.instance.ParseInt(DateFile.instance.GetItemDate(this.enemyBodyId, 98, true))];
                                this.resourceButtonImage.sprite = GetSprites.instance.makeResourceIcon[DateFile.instance.ParseInt(DateFile.instance.resourceDate[5][98])];
                                this.enemyBodyNameText.text = DateFile.instance.SetColoer(20001 + DateFile.instance.ParseInt(DateFile.instance.GetItemDate(this.enemyBodyId, 8, true)), DateFile.instance.GetItemDate(this.enemyBodyId, 0, true), false);
                                this.enemyBodyWorth = DateFile.instance.ParseInt(DateFile.instance.GetItemDate(this.enemyBodyId, 904, true));
                                break;
                            }
                        case 2:
                            {
                                bool flag16 = this.bodyActorId == 0;
                                if (flag16)
                                {
                                    this.bodyActorId = DateFile.instance.MakeNewActor(DateFile.instance.ParseInt(DateFile.instance.cricketBattleDate[this.ququBattleId][4]), true, -99, -1, -1, null, null, null, null, 20);
                                }
                                this.enemyBodyImage.tag = "ShopBootyActor";
                                this.enemyBodyImage.name = string.Format("ActorIcon,{0},{1},{2}", this.bodyActorId, Random.Range(0, ActorMenu.instance.GetActorTalk(this.bodyActorId).Count), -999);
                                this.enemyBodyImage.GetComponent<Image>().sprite = GetSprites.instance.attSprites[0];
                                this.enemyBodyNameText.text = DateFile.instance.SetColoer(10003, DateFile.instance.GetActorName(this.bodyActorId, false, false), false);
                                this.enemyBodyWorth = DateFile.instance.GetActorWorth(this.bodyActorId);
                                break;
                            }
                    }
                }
                this.actorBodyTyp = -1;
                this.setActorBodyId = 0;
                this.UpdateActorBody();
                this.actorQuquHp = new int[3];
                this.enemyQuquHp = new int[3];
                this.actorQuquSp = new int[3];
                this.enemyQuquSp = new int[3];
                this.MakeQuqu();
                this.showQuquIndex = Random.Range(0, 3);
                for (int num18 = 0; num18 < this.actorQuquHpText.Length; num18++)
                {
                    this.UpdateActorQuquValue(num18);
                    this.UpdateEnemyQuquValue(num18);
                    this.UpdateQuquHp(num18);
                }
            }

            // Token: 0x060007BD RID: 1981 RVA: 0x000C5BA4 File Offset: 0x000C3DA4
            private void SetResourceBody(int enemyId, int baseBodySize)
            {
                int[] array = ActorMenu.instance.ActorResource(enemyId);
                int num = 5;
                int num2 = array[num];
                for (int i = 0; i < 6; i++)
                {
                    bool flag = array[i] > num2;
                    if (flag)
                    {
                        num = i;
                        num2 = array[i];
                    }
                }
                this.enemyBodyId = num;
                this.enemyBodySize = num2 * baseBodySize / 100;
                this.enemyBodyImage.tag = "ResourceIcon";
                this.enemyBodyImage.name = "ResourceIcon," + this.enemyBodyId;
                this.enemyBodyImage.sprite = GetSprites.instance.makeResourceIcon[DateFile.instance.ParseInt(DateFile.instance.resourceDate[this.enemyBodyId][98])];
                this.resourceButtonImage.sprite = GetSprites.instance.makeResourceIcon[DateFile.instance.ParseInt(DateFile.instance.resourceDate[this.enemyBodyId][98])];
                this.enemyBodyNameText.text = DateFile.instance.resourceDate[this.enemyBodyId][1] + DateFile.instance.SetColoer((this.enemyBodyId == 6) ? 20007 : ((this.enemyBodyId == 5) ? 20008 : 20003), "×" + this.enemyBodySize, false);
                this.enemyBodyWorth = this.enemyBodySize;
            }

            // Token: 0x060007BE RID: 1982 RVA: 0x000C5D28 File Offset: 0x000C3F28
            private void MakeQuqu()
            {
                this.actorQuquId = new int[]
                {
            -99,
            -99,
            -99
                };
                this.enemyQuquId = new int[3];
                int num = DateFile.instance.ParseInt(DateFile.instance.cricketBattleDate[this.ququBattleId][2]);
                int num2 = Mathf.Clamp(num / 3, 1, 9);
                int[] array = new int[]
                {
            num2,
            num2,
            num2
                };
                int num3 = num % 3;
                bool flag = num > 27;
                bool flag2 = !flag;
                if (flag2)
                {
                    int num4 = Random.Range(0, 3);
                    array[num4] = Mathf.Clamp(array[num4] + num3, 1, 9);
                }
                List<int> list = new List<int>(DateFile.instance.cricketDate.Keys);
                for (int i = 0; i < 3; i++)
                {
                    int num5 = array[i];
                    int num6 = array[i];
                    int num7 = 0;
                    int num8 = 0;
                    int partId = 0;
                    string[] array2 = DateFile.instance.cricketBattleDate[this.ququBattleId][21 + i].Split(new char[]
                    {
                '|'
                    });
                    bool flag3 = array2.Length == 2;
                    if (flag3)
                    {
                        num8 = DateFile.instance.ParseInt(array2[0]);
                        partId = DateFile.instance.ParseInt(array2[1]);
                    }
                    else
                    {
                        bool flag4 = num5 >= 8;
                        if (flag4)
                        {
                            num6 = 0;
                            List<int> list2 = new List<int>();
                            for (int j = 0; j < list.Count; j++)
                            {
                                int num9 = list[j];
                                bool flag5 = DateFile.instance.ParseInt(DateFile.instance.cricketDate[num9][1]) == num5;
                                if (flag5)
                                {
                                    int num10 = flag ? DateFile.instance.ParseInt(DateFile.instance.cricketDate[num9][1006]) : DateFile.instance.ParseInt(DateFile.instance.cricketDate[num9][6]);
                                    for (int k = 0; k < num10; k++)
                                    {
                                        list2.Add(num9);
                                    }
                                }
                            }
                            num7 = list2[Random.Range(0, list2.Count)];
                        }
                        else
                        {
                            bool flag6 = num5 >= 7;
                            if (flag6)
                            {
                                num5 = Mathf.Clamp(num5 - Random.Range(0, num5 / 2), 1, 6);
                            }
                            else
                            {
                                bool flag7 = Random.Range(0, 100) < 75;
                                if (flag7)
                                {
                                    num6 = Mathf.Clamp(num6 - Random.Range(0, num6 / 2), 1, 6);
                                }
                                else
                                {
                                    num5 = Mathf.Clamp(num5 - Random.Range(0, num5 / 2), 1, 6);
                                }
                            }
                        }
                        bool flag8 = num7 == 0;
                        if (flag8)
                        {
                            List<int> list3 = new List<int>();
                            for (int l = 0; l < list.Count; l++)
                            {
                                int num11 = list[l];
                                bool flag9 = DateFile.instance.ParseInt(DateFile.instance.cricketDate[num11][3]) != 0 && DateFile.instance.ParseInt(DateFile.instance.cricketDate[num11][1]) == num5;
                                if (flag9)
                                {
                                    for (int m = 0; m < DateFile.instance.ParseInt(DateFile.instance.cricketDate[num11][6]); m++)
                                    {
                                        list3.Add(num11);
                                    }
                                }
                            }
                            num8 = list3[Random.Range(0, list3.Count)];
                        }
                        bool flag10 = num6 > 0;
                        if (flag10)
                        {
                            List<int> list4 = new List<int>();
                            for (int n = 0; n < list.Count; n++)
                            {
                                int num12 = list[n];
                                bool flag11 = DateFile.instance.ParseInt(DateFile.instance.cricketDate[num12][4]) == 1 && DateFile.instance.ParseInt(DateFile.instance.cricketDate[num12][1]) == num6;
                                if (flag11)
                                {
                                    for (int num13 = 0; num13 < DateFile.instance.ParseInt(DateFile.instance.cricketDate[num12][6]); num13++)
                                    {
                                        list4.Add(num12);
                                    }
                                }
                            }
                            partId = list4[Random.Range(0, list4.Count)];
                        }
                    }
                    this.enemyQuquId[i] = DateFile.instance.MakeNewItem(10000, -(i + 1), 0, 50, 20);
                    GetQuquWindow.instance.MakeQuqu(this.enemyQuquId[i], (num7 > 0) ? num7 : num8, partId);
                }
            }

            // Token: 0x060007BF RID: 1983 RVA: 0x000C6218 File Offset: 0x000C4418
            private void UpdateActorQuquValue(int index)
            {
                bool flag = this.actorQuquId[index] >= 0;
                if (flag)
                {
                    this.actorBattleQuquNameText[index].text = DateFile.instance.SetColoer(20001 + DateFile.instance.ParseInt(DateFile.instance.GetItemDate(this.actorQuquId[index], 8, true)), DateFile.instance.GetItemDate(this.actorQuquId[index], 0, true), false);
                    this.actorBattleQuquPower1Text[index].text = GetQuquWindow.instance.GetQuquDate(this.actorQuquId[index], 21, true).ToString();
                    this.actorBattleQuquPower2Text[index].text = GetQuquWindow.instance.GetQuquDate(this.actorQuquId[index], 22, true).ToString();
                    this.actorBattleQuquPower3Text[index].text = GetQuquWindow.instance.GetQuquDate(this.actorQuquId[index], 23, true).ToString();
                    this.actorQuquName[index].text = this.actorBattleQuquNameText[index].text;
                    this.actorQuquHp[index] = GetQuquWindow.instance.GetQuquDate(this.actorQuquId[index], 11, true);
                    this.actorQuquSp[index] = GetQuquWindow.instance.GetQuquDate(this.actorQuquId[index], 12, true);
                    int num = DateFile.instance.ParseInt(DateFile.instance.GetItemDate(this.actorQuquId[index], 901, true));
                    int num2 = DateFile.instance.ParseInt(DateFile.instance.GetItemDate(this.actorQuquId[index], 902, true));
                    this.actorQuquHpText[index].text = string.Format("{0}{1}</color>/{2}", ActorMenu.instance.Color3(num, num2), num, num2);
                }
                else
                {
                    this.actorQuquName[index].text = "";
                    this.actorQuquHpText[index].text = "";
                }
                this.actorQuquIcon[index].sprite = ((this.actorQuquId[index] < 0) ? GetSprites.instance.itemSprites[0] : DateFile.instance.GetCricketImage(this.actorQuquId[index]));
                this.actorQuquIcon[index].name = "ActorQuqu," + this.actorQuquId[index];
            }

            // Token: 0x060007C0 RID: 1984 RVA: 0x000C6464 File Offset: 0x000C4664
            private void UpdateEnemyQuquValue(int index)
            {
                this.enemyBattleQuquNameText[index].text = DateFile.instance.SetColoer(20001 + DateFile.instance.ParseInt(DateFile.instance.GetItemDate(this.enemyQuquId[index], 8, true)), DateFile.instance.GetItemDate(this.enemyQuquId[index], 0, true), false);
                this.enemyBattleQuquPower1Text[index].text = GetQuquWindow.instance.GetQuquDate(this.enemyQuquId[index], 21, true).ToString();
                this.enemyBattleQuquPower2Text[index].text = GetQuquWindow.instance.GetQuquDate(this.enemyQuquId[index], 22, true).ToString();
                this.enemyBattleQuquPower3Text[index].text = GetQuquWindow.instance.GetQuquDate(this.enemyQuquId[index], 23, true).ToString();
                bool flag = index != this.showQuquIndex;
                if (flag)
                {
                    this.enemyQuquName[index].text = DateFile.instance.SetColoer(20002, DateFile.instance.massageDate[3][2], false);
                    this.hideQuquImage[index].SetActive(true);
                }
                else
                {
                    this.enemyQuquName[index].text = this.enemyBattleQuquNameText[index].text;
                    this.hideQuquImage[index].SetActive(false);
                }
                this.enemyQuquHp[index] = GetQuquWindow.instance.GetQuquDate(this.enemyQuquId[index], 11, true);
                this.enemyQuquSp[index] = GetQuquWindow.instance.GetQuquDate(this.enemyQuquId[index], 12, true);
                this.enemyQuquIcon[index].sprite = DateFile.instance.GetCricketImage(this.enemyQuquId[index]);
                this.enemyQuquIcon[index].name = "EnemyQuqu," + this.enemyQuquId[index];
                int num = DateFile.instance.ParseInt(DateFile.instance.GetItemDate(this.enemyQuquId[index], 901, true));
                int num2 = DateFile.instance.ParseInt(DateFile.instance.GetItemDate(this.enemyQuquId[index], 902, true));
                this.enemyQuquHpText[index].text = string.Format("{0}{1}</color>/{2}", ActorMenu.instance.Color3(num, num2), num, num2);
            }

            // Token: 0x060007C1 RID: 1985 RVA: 0x000C66BC File Offset: 0x000C48BC
            private void UpdateQuquHp(int index)
            {
                int ququDate = GetQuquWindow.instance.GetQuquDate(this.actorQuquId[index], 11, true);
                int ququDate2 = GetQuquWindow.instance.GetQuquDate(this.actorQuquId[index], 12, true);
                this.actorQuquHp[index] = Mathf.Min(this.actorQuquHp[index], ququDate);
                this.actorQuquSp[index] = Mathf.Min(this.actorQuquSp[index], ququDate2);
                this.actorBattleQuquStrengthText[index].text = DateFile.instance.SetColoer((this.actorQuquHp[index] < ququDate * 50 / 100) ? 20010 : 20003, this.actorQuquHp[index].ToString(), false);
                this.actorBattleQuquMagicText[index].text = DateFile.instance.SetColoer((this.actorQuquSp[index] < ququDate2 * 50 / 100) ? 20010 : 20003, this.actorQuquSp[index].ToString(), false);
                this.actorBattleQuquStrengthBar[index].fillAmount = (float)this.actorQuquHp[index] / (float)ququDate;
                this.actorBattleQuquMagicBar[index].fillAmount = (float)this.actorQuquSp[index] / (float)ququDate2;
                this.actorBattleQuquPower1Text[index].text = GetQuquWindow.instance.GetQuquDate(this.actorQuquId[index], 21, true).ToString();
                this.actorBattleQuquPower2Text[index].text = GetQuquWindow.instance.GetQuquDate(this.actorQuquId[index], 22, true).ToString();
                this.actorBattleQuquPower3Text[index].text = GetQuquWindow.instance.GetQuquDate(this.actorQuquId[index], 23, true).ToString();
                int num = DateFile.instance.ParseInt(DateFile.instance.GetItemDate(this.actorQuquId[index], 901, true));
                int num2 = DateFile.instance.ParseInt(DateFile.instance.GetItemDate(this.actorQuquId[index], 902, true));
                this.actorQuquHpText[index].text = string.Format("{0}{1}</color>/{2}", ActorMenu.instance.Color3(num, num2), num, num2);
                int ququDate3 = GetQuquWindow.instance.GetQuquDate(this.enemyQuquId[index], 11, true);
                int ququDate4 = GetQuquWindow.instance.GetQuquDate(this.enemyQuquId[index], 12, true);
                this.enemyQuquHp[index] = Mathf.Min(this.enemyQuquHp[index], ququDate3);
                this.enemyQuquSp[index] = Mathf.Min(this.enemyQuquSp[index], ququDate4);
                this.enemyBattleQuquStrengthText[index].text = DateFile.instance.SetColoer((this.enemyQuquHp[index] < ququDate3 * 50 / 100) ? 20010 : 20003, this.enemyQuquHp[index].ToString(), false);
                this.enemyBattleQuquMagicText[index].text = DateFile.instance.SetColoer((this.enemyQuquSp[index] < ququDate4 * 50 / 100) ? 20010 : 20003, this.enemyQuquSp[index].ToString(), false);
                this.enemyBattleQuquStrengthBar[index].fillAmount = (float)this.enemyQuquHp[index] / (float)ququDate3;
                this.enemyBattleQuquMagicBar[index].fillAmount = (float)this.enemyQuquSp[index] / (float)ququDate4;
                this.enemyBattleQuquPower1Text[index].text = GetQuquWindow.instance.GetQuquDate(this.enemyQuquId[index], 21, true).ToString();
                this.enemyBattleQuquPower2Text[index].text = GetQuquWindow.instance.GetQuquDate(this.enemyQuquId[index], 22, true).ToString();
                this.enemyBattleQuquPower3Text[index].text = GetQuquWindow.instance.GetQuquDate(this.enemyQuquId[index], 23, true).ToString();
                int num3 = DateFile.instance.ParseInt(DateFile.instance.GetItemDate(this.enemyQuquId[index], 901, true));
                int num4 = DateFile.instance.ParseInt(DateFile.instance.GetItemDate(this.enemyQuquId[index], 902, true));
                this.enemyQuquHpText[index].text = string.Format("{0}{1}</color>/{2}", ActorMenu.instance.Color3(num3, num4), num3, num4);
            }

            // Token: 0x060007C2 RID: 1986 RVA: 0x000C6B00 File Offset: 0x000C4D00
            public void ShowChooseBodyTyp()
            {
                bool flag = this.chooseBodyIsShow;
                if (flag)
                {
                    this.CloseChooseBodyTyp();
                }
                else
                {
                    int num = (this.enemyBodyTyp == 0) ? this.enemyBodyId : 5;
                    int num2 = this.enemyBodyWorth;
                    int num3 = ActorMenu.instance.ActorResource(DateFile.instance.MianActorID())[num];
                    bool flag2 = num3 >= num2;
                    if (flag2)
                    {
                        this.useResourceButton.interactable = true;
                        this.needResourceText.text = DateFile.instance.resourceDate[num][1] + DateFile.instance.SetColoer((num == 6) ? 20007 : ((num == 5) ? 20008 : 20003), "×" + this.enemyBodyWorth, false);
                    }
                    else
                    {
                        this.useResourceButton.interactable = false;
                        this.needResourceText.text = DateFile.instance.resourceDate[num][1] + DateFile.instance.SetColoer(20010, "×" + this.enemyBodyWorth, false);
                    }
                    ShortcutExtensions.DOPlayForward(this.chooseBodyWindow);
                    this.chooseBodyIsShow = true;
                }
            }

            // Token: 0x060007C3 RID: 1987 RVA: 0x00005349 File Offset: 0x00003549
            public void CloseChooseBodyTyp()
            {
                ShortcutExtensions.DOPlayBackwards(this.chooseBodyWindow);
                this.chooseBodyIsShow = false;
            }

            // Token: 0x060007C4 RID: 1988 RVA: 0x000C6C44 File Offset: 0x000C4E44
            private void UpdateActorBody()
            {
                switch (this.actorBodyTyp)
                {
                    case 0:
                        this.actorBodyImage.tag = "ResourceIcon";
                        this.actorBodyImage.name = "ResourceIcon," + this.actorBodyId;
                        this.actorBodyImage.sprite = GetSprites.instance.makeResourceIcon[DateFile.instance.ParseInt(DateFile.instance.resourceDate[this.actorBodyId][98])];
                        this.actorBodyNameText.text = DateFile.instance.resourceDate[this.actorBodyId][1] + DateFile.instance.SetColoer((this.actorBodyId == 6) ? 20007 : ((this.actorBodyId == 5) ? 20008 : 20003), "×" + this.actorBodySize, false);
                        break;
                    case 1:
                        this.actorBodyImage.tag = "ActorItem";
                        this.actorBodyImage.name = "ActorItemIcon," + this.actorBodyId;
                        this.actorBodyImage.GetComponent<Image>().sprite = GetSprites.instance.itemSprites[DateFile.instance.ParseInt(DateFile.instance.GetItemDate(this.actorBodyId, 98, true))];
                        this.actorBodyNameText.text = DateFile.instance.SetColoer(20001 + DateFile.instance.ParseInt(DateFile.instance.GetItemDate(this.actorBodyId, 8, true)), DateFile.instance.GetItemDate(this.actorBodyId, 0, true), false);
                        break;
                    case 2:
                        this.actorBodyImage.tag = "ShopBootyActor";
                        this.actorBodyImage.name = string.Format("ActorIcon,{0},{1},{2}", this.actorBodyId, Random.Range(0, ActorMenu.instance.GetActorTalk(this.actorBodyId).Count), -999);
                        this.actorBodyImage.GetComponent<Image>().sprite = GetSprites.instance.attSprites[0];
                        this.actorBodyNameText.text = DateFile.instance.SetColoer(10003, DateFile.instance.GetActorName(this.actorBodyId, false, false), false);
                        break;
                    default:
                        this.actorBodyId = -98;
                        this.actorBodyImage.tag = "ActorItem";
                        this.actorBodyImage.name = "ActorItemIcon," + this.actorBodyId;
                        this.actorBodyImage.sprite = this.GetItemIcon(this.actorBodyId);
                        this.actorBodyNameText.text = DateFile.instance.massageDate[8001][5].Split(new char[]
                        {
                '|'
                        })[0];
                        break;
                }
            }

            // Token: 0x060007C5 RID: 1989 RVA: 0x000C6F48 File Offset: 0x000C5148
            public void SetUseResourceButton()
            {
                this.actorBodyTyp = 0;
                this.actorBodyId = ((this.enemyBodyTyp != 0) ? 5 : this.enemyBodyId);
                this.actorBodySize = this.enemyBodyWorth;
                this.UpdateActorBody();
                this.CloseChooseBodyTyp();
                this.UpdateCanStartBattle();
            }

            // Token: 0x060007C6 RID: 1990 RVA: 0x000C6F98 File Offset: 0x000C5198
            public void SetUseItemButton()
            {
                this.getItemTyp = 1;
                this.GetItem();
                ShortcutExtensions.DOPlayForward(this.itemWindow.transform);
                Component[] componentsInChildren = this.itemWindow.GetComponentsInChildren<Component>();
                foreach (Component component in componentsInChildren)
                {
                    bool flag = component is Graphic;
                    if (flag)
                    {
                        (component as Graphic).CrossFadeAlpha(1f, 0.2f, true);
                    }
                }
                this.useItemButton.interactable = false;
                this.useItemButton.gameObject.SetActive(true);
                this.itemMask.SetActive(true);
                this.itemWindowIsShow = true;
                this.CloseChooseBodyTyp();
            }

            // Token: 0x060007C7 RID: 1991 RVA: 0x000C704C File Offset: 0x000C524C
            public void SetUseActorButton()
            {
                this.GetActor();
                ShortcutExtensions.DOPlayForward(this.acotrWindow.transform);
                Component[] componentsInChildren = this.acotrWindow.GetComponentsInChildren<Component>();
                foreach (Component component in componentsInChildren)
                {
                    bool flag = component is Graphic;
                    if (flag)
                    {
                        (component as Graphic).CrossFadeAlpha(1f, 0.2f, true);
                    }
                }
                this.useActorButton.interactable = false;
                this.useActorButton.gameObject.SetActive(true);
                this.itemMask.SetActive(true);
                this.CloseChooseBodyTyp();
            }

            // Token: 0x060007C8 RID: 1992 RVA: 0x000C70F0 File Offset: 0x000C52F0
            public void CloseActorWindow()
            {
                ShortcutExtensions.DOPlayBackwards(this.acotrWindow.transform);
                Component[] componentsInChildren = this.acotrWindow.GetComponentsInChildren<Component>();
                foreach (Component component in componentsInChildren)
                {
                    bool flag = component is Graphic;
                    if (flag)
                    {
                        (component as Graphic).CrossFadeAlpha(0f, 0.2f, true);
                    }
                }
                this.useActorButton.gameObject.SetActive(false);
                this.itemMask.SetActive(false);
            }

            // Token: 0x060007C9 RID: 1993 RVA: 0x000C717C File Offset: 0x000C537C
            private void GetActor()
            {
                this.RemoveActor();
                int num = DateFile.instance.MianActorID();
                List<int> list = new List<int>();
                Dictionary<int, int> dictionary = new Dictionary<int, int>();
                List<int> list2 = new List<int>(DateFile.instance.GetFamily(true, true));
                foreach (int num2 in list2)
                {
                    bool flag = num2 != num;
                    if (flag)
                    {
                        dictionary.Add(num2, DateFile.instance.GetActorWorth(num2));
                    }
                }
                List<KeyValuePair<int, int>> list3 = new List<KeyValuePair<int, int>>(dictionary);
                list3.Sort((KeyValuePair<int, int> s1, KeyValuePair<int, int> s2) => s2.Value.CompareTo(s1.Value));
                list.Add(num);
                foreach (KeyValuePair<int, int> keyValuePair in list3)
                {
                    list.Add(keyValuePair.Key);
                }
                for (int i = 0; i < list.Count; i++)
                {
                    int num3 = list[i];
                    int actorWorth = DateFile.instance.GetActorWorth(num3);
                    bool flag2 = num3 == num || actorWorth >= this.enemyBodyWorth;
                    if (flag2)
                    {
                        GameObject gameObject = Object.Instantiate<GameObject>(this.actorIcon, Vector3.zero, Quaternion.identity);
                        gameObject.name = "Actor," + num3;
                        gameObject.transform.SetParent(this.actorHolder, false);
                        gameObject.GetComponent<Toggle>().group = this.actorHolder.GetComponent<ToggleGroup>();
                        bool flag3 = DateFile.instance.acotrTeamDate.Contains(num3);
                        if (flag3)
                        {
                            gameObject.transform.Find("IsInTeamIcon").gameObject.SetActive(true);
                        }
                        gameObject.transform.Find("IsInBuildingIcon").gameObject.SetActive(DateFile.instance.ActorIsWorking(num3) != null);
                        int actorFavor = DateFile.instance.GetActorFavor(false, DateFile.instance.MianActorID(), num3, false, false);
                        gameObject.transform.Find("ListActorFavorText").GetComponent<Text>().text = ((num3 == num || actorFavor == -1) ? DateFile.instance.SetColoer(20002, DateFile.instance.massageDate[303][2], false) : ActorMenu.instance.Color5(actorFavor, true, -1));
                        gameObject.transform.Find("ListActorNameText").GetComponent<Text>().text = DateFile.instance.GetActorName(num3, false, false);
                        gameObject.transform.Find("SkillLevelText").GetComponent<Text>().text = ((num3 == num) ? DateFile.instance.massageDate[303][2] : actorWorth.ToString());
                        Transform transform = gameObject.transform.Find("ListActorFaceHolder").Find("FaceMask").Find("MianActorFace");
                        transform.GetComponent<ActorFace>().SetActorFace(num3, false);
                    }
                }
            }

            // Token: 0x060007CA RID: 1994 RVA: 0x000C74DC File Offset: 0x000C56DC
            private void RemoveActor()
            {
                for (int i = 0; i < this.actorHolder.childCount; i++)
                {
                    Object.Destroy(this.actorHolder.GetChild(i).gameObject);
                }
            }

            // Token: 0x060007CB RID: 1995 RVA: 0x0000535F File Offset: 0x0000355F
            public void SetActorId(int id)
            {
                this.setActorBodyId = id;
                this.useActorButton.interactable = true;
            }

            // Token: 0x060007CC RID: 1996 RVA: 0x00005376 File Offset: 0x00003576
            public void SetActorBody()
            {
                this.actorBodyTyp = 2;
                this.actorBodyId = this.setActorBodyId;
                this.actorBodySize = 1;
                this.UpdateActorBody();
                this.CloseActorWindow();
                this.UpdateCanStartBattle();
            }

            // Token: 0x060007CD RID: 1997 RVA: 0x000C7520 File Offset: 0x000C5720
            public void ChooseActorQuqu(int index)
            {
                this.choseItemIndex = index;
                this.getItemTyp = 0;
                this.GetItem();
                ShortcutExtensions.DOPlayForward(this.itemWindow.transform);
                Component[] componentsInChildren = this.itemWindow.GetComponentsInChildren<Component>();
                foreach (Component component in componentsInChildren)
                {
                    bool flag = component is Graphic;
                    if (flag)
                    {
                        (component as Graphic).CrossFadeAlpha(1f, 0.2f, true);
                    }
                }
                this.useItemButton.interactable = false;
                this.useItemButton.gameObject.SetActive(true);
                this.itemMask.SetActive(true);
                this.itemWindowIsShow = true;
            }

            // Token: 0x060007CE RID: 1998 RVA: 0x000C75D4 File Offset: 0x000C57D4
            public void CloseItemWindow()
            {
                ShortcutExtensions.DOPlayBackwards(this.itemWindow.transform);
                Component[] componentsInChildren = this.itemWindow.GetComponentsInChildren<Component>();
                foreach (Component component in componentsInChildren)
                {
                    bool flag = component is Graphic;
                    if (flag)
                    {
                        (component as Graphic).CrossFadeAlpha(0f, 0.2f, true);
                    }
                }
                this.useItemButton.gameObject.SetActive(false);
                this.itemMask.SetActive(false);
                this.itemWindowIsShow = false;
            }

            // Token: 0x060007CF RID: 1999 RVA: 0x000C7664 File Offset: 0x000C5864
            private void GetItem()
            {
                this.RemoveItems();
                int num = DateFile.instance.MianActorID();
                List<int> list = new List<int>(ActorMenu.instance.GetActorItems(num, 0, false).Keys);
                int num2 = DateFile.instance.ParseInt(DateFile.instance.GetActorDate(num, 312, false));
                bool flag = num2 > 0;
                if (flag)
                {
                    list.Add(num2);
                }
                for (int i = 0; i < list.Count; i++)
                {
                    int num3 = list[i];
                    bool flag2 = false;
                    bool flag3 = this.getItemTyp == 0;
                    if (flag3)
                    {
                        for (int j = 0; j < this.actorQuquId.Length; j++)
                        {
                            bool flag4 = this.actorQuquId[j] == num3;
                            if (flag4)
                            {
                                flag2 = true;
                            }
                        }
                    }
                    bool flag5 = (this.getItemTyp == 0 && !flag2 && DateFile.instance.ParseInt(DateFile.instance.GetItemDate(num3, 2001, true)) == 1 && DateFile.instance.ParseInt(DateFile.instance.GetItemDate(num3, 901, true)) > 0) || (this.getItemTyp == 1 && DateFile.instance.ParseInt(DateFile.instance.GetItemDate(num3, 906, true)) == 1 && DateFile.instance.ParseInt(DateFile.instance.GetItemDate(num3, 904, true)) >= this.enemyBodyWorth && (DateFile.instance.ParseInt(DateFile.instance.GetItemDate(num3, 6, true)) == 1 || (DateFile.instance.ParseInt(DateFile.instance.GetItemDate(num3, 6, true)) == 0 && DateFile.instance.ParseInt(DateFile.instance.GetItemDate(num3, 901, true)) > 0)));
                    if (flag5)
                    {
                        GameObject gameObject = Object.Instantiate<GameObject>(ActorMenu.instance.itemIconNoDrag, Vector3.zero, Quaternion.identity);
                        gameObject.name = "Item," + num3;
                        gameObject.transform.SetParent(this.itemHolder, false);
                        gameObject.GetComponent<Toggle>().group = this.itemHolder.GetComponent<ToggleGroup>();
                        Image component = gameObject.transform.Find("ItemBack").GetComponent<Image>();
                        component.sprite = GetSprites.instance.itemBackSprites[DateFile.instance.ParseInt(DateFile.instance.GetItemDate(num3, 4, true))];
                        component.color = ActorMenu.instance.LevelColor(DateFile.instance.ParseInt(DateFile.instance.GetItemDate(num3, 8, true)));
                        bool flag6 = DateFile.instance.ParseInt(DateFile.instance.GetItemDate(num3, 6, true)) > 0;
                        if (flag6)
                        {
                            gameObject.transform.Find("ItemNumberText").GetComponent<Text>().text = "×" + DateFile.instance.GetItemNumber(num, num3);
                        }
                        else
                        {
                            int num4 = DateFile.instance.ParseInt(DateFile.instance.GetItemDate(num3, 901, true));
                            int num5 = DateFile.instance.ParseInt(DateFile.instance.GetItemDate(num3, 902, true));
                            gameObject.transform.Find("ItemNumberText").GetComponent<Text>().text = string.Format("{0}{1}</color>/{2}", ActorMenu.instance.Color3(num4, num5), num4, num5);
                        }
                        GameObject gameObject2 = gameObject.transform.Find("ItemIcon").gameObject;
                        gameObject2.name = "ItemIcon," + num3;
                        gameObject2.GetComponent<Image>().sprite = GetSprites.instance.itemSprites[DateFile.instance.ParseInt(DateFile.instance.GetItemDate(num3, 98, true))];
                    }
                }
            }

            // Token: 0x060007D0 RID: 2000 RVA: 0x000C7A5C File Offset: 0x000C5C5C
            private void RemoveItems()
            {
                for (int i = 0; i < this.itemHolder.childCount; i++)
                {
                    Object.Destroy(this.itemHolder.GetChild(i).gameObject);
                }
            }

            // Token: 0x060007D1 RID: 2001 RVA: 0x000C7AA0 File Offset: 0x000C5CA0
            public void SetItem()
            {
                bool flag = this.getItemTyp == 0;
                if (flag)
                {
                    this.actorQuquId[this.choseItemIndex] = ActorMenu.instance.choseItemId;
                    this.UpdateActorQuquValue(this.choseItemIndex);
                    this.CloseItemWindow();
                    this.UpdateCanStartBattle();
                }
                else
                {
                    this.actorBodyTyp = 1;
                    this.actorBodyId = ActorMenu.instance.choseItemId;
                    this.actorBodySize = 1;
                    this.UpdateActorBody();
                    this.CloseItemWindow();
                    this.UpdateCanStartBattle();
                }
            }

            // Token: 0x060007D2 RID: 2002 RVA: 0x000053A8 File Offset: 0x000035A8
            public void RemoveItem()
            {
                this.actorQuquId[this.choseItemIndex] = -99;
                this.UpdateActorQuquValue(this.choseItemIndex);
                this.CloseItemWindow();
                this.UpdateCanStartBattle();
            }

            // Token: 0x060007D3 RID: 2003 RVA: 0x000C7B28 File Offset: 0x000C5D28
            private Sprite GetItemIcon(int id)
            {
                bool flag = id < 0;
                Sprite result;
                if (flag)
                {
                    result = GetSprites.instance.itemSprites[0];
                }
                else
                {
                    Sprite sprite = GetSprites.instance.itemSprites[DateFile.instance.ParseInt(DateFile.instance.GetItemDate(id, 98, true))];
                    result = sprite;
                }
                return result;
            }

            // Token: 0x060007D4 RID: 2004 RVA: 0x000C7B78 File Offset: 0x000C5D78
            private void UpdateCanStartBattle()
            {
                bool flag = true;
                for (int i = 0; i < this.actorQuquId.Length; i++)
                {
                    bool flag2 = this.actorQuquId[i] < 0;
                    if (flag2)
                    {
                        flag = false;
                    }
                }
                this.startBattleButton.interactable = (this.actorBodyTyp >= 0 && flag);
            }

            // Token: 0x060007D5 RID: 2005 RVA: 0x000C7BD0 File Offset: 0x000C5DD0
            public void StartBattleButton()
            {
                int key = (this.actorBodyTyp == 2 && this.actorBodyId == DateFile.instance.MianActorID()) ? 3 : this.actorBodyTyp;
                YesOrNoWindow.instance.SetYesOrNoWindow(515, DateFile.instance.massageDate[8002][key].Split(new char[]
                {
            '|'
                })[0], DateFile.instance.massageDate[8002][key].Split(new char[]
                {
            '|'
                })[1], false, true);
            }

            // Token: 0x060007D6 RID: 2006 RVA: 0x000C7C70 File Offset: 0x000C5E70
            private bool ShowHideQuquImage(int index)
            {
                bool activeSelf = this.hideQuquImage[index].activeSelf;
                bool result;
                if (activeSelf)
                {
                    TweenSettingsExtensions.OnComplete<Tweener>(TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScale(this.hideQuquImage[index].GetComponent<RectTransform>(), new Vector3(2f, 2f, 1f), 0.6f), (Ease)26), delegate ()
                    {
                        this.enemyQuquName[index].text = this.enemyBattleQuquNameText[index].text;
                        this.hideQuquImage[index].SetActive(false);
                    });
                    Component[] componentsInChildren = this.hideQuquImage[index].GetComponentsInChildren<Component>();
                    foreach (Component component in componentsInChildren)
                    {
                        bool flag = component is Graphic;
                        if (flag)
                        {
                            (component as Graphic).CrossFadeAlpha(0f, 0.6f, false);
                        }
                    }
                    result = true;
                }
                else
                {
                    result = false;
                }
                return result;
            }

            // Token: 0x060007D7 RID: 2007 RVA: 0x000C7D5C File Offset: 0x000C5F5C
            public void StartQuquBattle()
            {
                this.winTurn = 0;
                this.ququBattlePart = 2;
                this.startBattleWindow.SetActive(false);
                this.setBattleSpeedHolder.SetActive(true);
                for (int i = 0; i < 3; i++)
                {
                    Component[] componentsInChildren = this.ququBattleBack[i].GetComponentsInChildren<Component>();
                    foreach (Component component in componentsInChildren)
                    {
                        bool flag = component is Graphic;
                        if (flag)
                        {
                            (component as Graphic).CrossFadeAlpha(1f, 0.5f, true);
                        }
                    }
                }
                for (int k = 0; k < this.battleValue.Length; k++)
                {
                    this.nextButtonMask[k].GetComponent<Image>().DOColor(new Color(0f, 0f, 0f, 0.5f), 0.5f);
                }
                for (int l = 0; l < this.actorQuquIcon.Length; l++)
                {
                    this.actorQuquIcon[l].GetComponent<Button>().interactable = false;
                }
                this.actorBodyImage.GetComponent<Button>().interactable = false;
                bool flag2 = this.actorBodyTyp == 2 && this.actorBodyId != DateFile.instance.MianActorID();
                if (flag2)
                {
                    DateFile.instance.ChangeFavor(this.actorBodyId, -DateFile.instance.ParseInt(DateFile.instance.GetActorDate(this.actorBodyId, 3, false)), false, false);
                }
                this.UpdateQuquHp(0);
                this.UpdateQuquHp(1);
                this.UpdateQuquHp(2);
                this.ShowStartBattleState();
            }

            // Token: 0x060007D8 RID: 2008 RVA: 0x000C7F08 File Offset: 0x000C6108
            public void ShowStartBattleState()
            {
                int stateIndex = 0;
                int num = DateFile.instance.ParseInt(DateFile.instance.GetItemDate(this.actorQuquId[this.ququBattleTurn], 8, true));
                int num2 = DateFile.instance.ParseInt(DateFile.instance.GetItemDate(this.enemyQuquId[this.ququBattleTurn], 8, true));
                int num3 = DateFile.instance.ParseInt(DateFile.instance.GetItemDate(this.actorQuquId[this.ququBattleTurn], 2002, true));
                int num4 = DateFile.instance.ParseInt(DateFile.instance.GetItemDate(this.enemyQuquId[this.ququBattleTurn], 2002, true));
                bool flag = (num3 != 0 && num4 == 0) || (num > num2 && num - num2 >= 6 && Random.Range(0, 100) < (num - num2) * 10);
                if (flag)
                {
                    stateIndex = 1;
                }
                else
                {
                    bool flag2 = (num4 != 0 && num3 == 0) || (num2 > num && num2 - num >= 6 && Random.Range(0, 100) < (num2 - num) * 10);
                    if (flag2)
                    {
                        stateIndex = 2;
                    }
                    else
                    {
                        bool flag3 = num3 == 0 && num4 == 0;
                        if (flag3)
                        {
                            stateIndex = 1 + Random.Range(0, 2);
                        }
                    }
                }
                base.StartCoroutine(this.StartBattle((float)(this.ShowHideQuquImage(this.ququBattleTurn) ? 1 : 0), stateIndex));
            }

            // Token: 0x060007D9 RID: 2009 RVA: 0x000053D5 File Offset: 0x000035D5
            private IEnumerator StartBattle(float waitTime, int stateIndex)
            {
                yield return new WaitForSeconds(waitTime);
                float _delay = 1f;
                this.SetBattleStateText(DateFile.instance.massageDate[8003][0].Split(new char[]
                {
            '|'
                })[this.ququBattleTurn], _delay, 2f, -1);
                _delay += 1f;
                this.SetBattleStateText(DateFile.instance.massageDate[8003][1].Split(new char[]
                {
            '|'
                })[this.ququBattleTurn], _delay, 2f, -1);
                _delay += 1f;

                //TweenCallback <> 9__2;
                //TweenSettingsExtensions.OnComplete<Sequence>(TweenSettingsExtensions.SetDelay<Sequence>(ShortcutExtensions.DOLocalJump(this.actorQuqu[this.ququBattleTurn], new Vector3(280f, 0f, 0f), 20f, 1, 0.1f, false), _delay), delegate ()
                //{
                //    this.actorQuquName[this.ququBattleTurn].text = "";
                //    ShortcutExtensions.DOLocalMoveY(this.battleValue[this.ququBattleTurn], 0f, 0.1f, false);
                //    Sequence sequence = TweenSettingsExtensions.SetDelay<Sequence>(ShortcutExtensions.DOLocalJump(this.actorQuqu[this.ququBattleTurn], new Vector3(320f, 0f, 0f), 10f, 1, 0.05f, false), 0.2f);
                //    TweenCallback tweenCallback;
                //    if ((tweenCallback = <> 9__2) == null)
                //    {
                //        tweenCallback = (<> 9__2 = delegate ()
                //        {
                //            this.SetBattleStateText(DateFile.instance.massageDate[8003][2].Split(new char[]
                //            {
                //        '|'
                //            })[0], 0f, 2f, -1);
                //            bool flag = stateIndex == 0 || stateIndex == 1;
                //            if (flag)
                //            {
                //                this.actorQuqu[this.ququBattleTurn].GetComponent<QuquPlace>().UpdateBattleQuquCall(this.actorQuquId[this.ququBattleTurn]);
                //                this.actorQuqu[this.ququBattleTurn].GetComponent<QuquPlace>().CallvolumeRest(0.8f, 0.2f);
                //            }
                //        });
                //    }
                //    TweenSettingsExtensions.OnComplete<Sequence>(sequence, tweenCallback);
                //});
                //TweenCallback <> 9__3;
                //TweenSettingsExtensions.OnComplete<Sequence>(TweenSettingsExtensions.SetDelay<Sequence>(ShortcutExtensions.DOLocalJump(this.enemyQuqu[this.ququBattleTurn], new Vector3(-280f, 0f, 0f), 20f, 1, 0.1f, false), _delay), delegate ()
                //{
                //    this.enemyQuquName[this.ququBattleTurn].text = "";
                //    Sequence sequence = TweenSettingsExtensions.SetDelay<Sequence>(ShortcutExtensions.DOLocalJump(this.enemyQuqu[this.ququBattleTurn], new Vector3(-320f, 0f, 0f), 10f, 1, 0.05f, false), 0.2f);
                //    TweenCallback tweenCallback;
                //    if ((tweenCallback = <> 9__3) == null)
                //    {
                //        tweenCallback = (<> 9__3 = delegate ()
                //        {
                //            bool flag = stateIndex == 0 || stateIndex == 2;
                //            if (flag)
                //            {
                //                this.enemyQuqu[this.ququBattleTurn].GetComponent<QuquPlace>().UpdateBattleQuquCall(this.enemyQuquId[this.ququBattleTurn]);
                //                this.enemyQuqu[this.ququBattleTurn].GetComponent<QuquPlace>().CallvolumeRest(0.8f, 0.2f);
                //            }
                //        });
                //    }
                //    TweenSettingsExtensions.OnComplete<Sequence>(sequence, tweenCallback);
                //});

                _delay += 2f;
                this.SetBattleStateText(DateFile.instance.massageDate[8003][3].Split(new char[]
                {
            '|'
                })[stateIndex], _delay, 2f, -1);
                _delay += 1.5f;
                switch (stateIndex)
                {
                    case 0:
                        this.SetBattleStateText(DateFile.instance.SetColoer(20008, DateFile.instance.massageDate[8003][2].Split(new char[]
                        {
                '|'
                        })[1], false), _delay, 2f, 0);
                        break;
                    case 1:
                        this.SetBattleStateText(DateFile.instance.SetColoer(20005, DateFile.instance.massageDate[8003][4].Split(new char[]
                        {
                '|'
                        })[0], false), _delay, 3f, 1);
                        break;
                    case 2:
                        this.SetBattleStateText(DateFile.instance.SetColoer(20010, DateFile.instance.massageDate[8003][4].Split(new char[]
                        {
                '|'
                        })[1], false), _delay, 3f, 2);
                        break;
                }
                yield break;
            }

            // Token: 0x060007DA RID: 2010 RVA: 0x000C8058 File Offset: 0x000C6258
            private void SetBattleStateText(string text, float delay, float size, int endTyp = -1)
            {
                TweenSettingsExtensions.OnStart<Tweener>(TweenSettingsExtensions.SetEase<Tweener>(TweenSettingsExtensions.SetDelay<Tweener>(ShortcutExtensions.DOScale(this.battleStateText[this.ququBattleTurn].transform, new Vector3(size, size, 1f), 0.1f), delay), (Ease)27), delegate ()
                {
                    this.battleStateText[this.ququBattleTurn].text = text;
                    this.battleStateText[this.ququBattleTurn].transform.localScale = new Vector3(0f, 0f, 1f);
                });
                TweenSettingsExtensions.SetEase<Tweener>(TweenSettingsExtensions.SetDelay<Tweener>(ShortcutExtensions.DOScale(this.battleStateText[this.ququBattleTurn].transform, new Vector3(size / 2f, size / 2f, 1f), 0.4f), 0.1f + delay), (Ease)27);
                base.StartCoroutine(this.BattleState(endTyp, 0.5f + delay));
            }

            // Token: 0x060007DB RID: 2011 RVA: 0x000053F2 File Offset: 0x000035F2
            private IEnumerator BattleState(int endTyp, float waitTime)
            {
                yield return new WaitForSeconds(waitTime);
                bool flag = endTyp == 0;
                if (flag)
                {
                    TweenSettingsExtensions.OnComplete<Tweener>(TweenSettingsExtensions.SetDelay<Tweener>(ShortcutExtensions.DOScale(this.battleStateText[this.ququBattleTurn].transform, new Vector3(0f, 0f, 1f), 0.1f), 0.8f), delegate ()
                    {
                        this.nextButtonMask[this.ququBattleTurn].GetComponent<Image>().DOColor(new Color(0f, 0f, 0f, 0f), 0.2f);
                        this.QuquBattleLoopStart(0.1f, 0.2f);
                    });
                }
                else
                {
                    bool flag2 = endTyp >= 1;
                    if (flag2)
                    {
                        bool flag3 = endTyp == 1;
                        if (flag3)
                        {
                            this.winTurn++;
                            this.actorQuqu[this.ququBattleTurn].GetComponent<QuquPlace>().UpdateBattleQuquCall(this.actorQuquId[this.ququBattleTurn]);
                            this.actorQuqu[this.ququBattleTurn].GetComponent<QuquPlace>().CallvolumeRest(1.6f, 0.6f);
                            this.AddQuquBattleMassage(true);
                        }
                        else
                        {
                            this.enemyQuqu[this.ququBattleTurn].GetComponent<QuquPlace>().UpdateBattleQuquCall(this.enemyQuquId[this.ququBattleTurn]);
                            this.enemyQuqu[this.ququBattleTurn].GetComponent<QuquPlace>().CallvolumeRest(1.6f, 0.6f);
                            this.AddQuquBattleMassage(false);
                        }
                        this.battleEndTyp = endTyp;
                        int num;
                        for (int i = 0; i < this.nextButton.Length; i = num + 1)
                        {
                            this.nextButton[i].gameObject.SetActive(i == this.ququBattleTurn);
                            num = i;
                        }
                    }
                }
                yield break;
            }

            // Token: 0x060007DC RID: 2012 RVA: 0x000C8120 File Offset: 0x000C6320
            public void BattleNextPart()
            {
                this.nextButton[this.ququBattleTurn].gameObject.SetActive(false);
                bool flag = this.battleEndTyp == 1;
                if (flag)
                {
                    bool flag2 = this.winTurn >= 2;
                    if (flag2)
                    {
                        this.BattleEndWindow(0);
                        return;
                    }
                }
                else
                {
                    bool flag3 = this.ququBattleTurn == 1 && this.winTurn == 0;
                    if (flag3)
                    {
                        this.BattleEndWindow(1);
                        return;
                    }
                }
                bool flag4 = this.ququBattleTurn < 2;
                if (flag4)
                {
                    ShortcutExtensions.DOKill(this.actorQuqu[this.ququBattleTurn], false);
                    ShortcutExtensions.DOKill(this.enemyQuqu[this.ququBattleTurn], false);
                    this.ququBattleTurn++;
                    this.ShowStartBattleState();
                }
                else
                {
                    bool flag5 = this.winTurn >= 2;
                    if (flag5)
                    {
                        this.BattleEndWindow(0);
                    }
                    else
                    {
                        this.BattleEndWindow(1);
                    }
                }
            }

            // Token: 0x060007DD RID: 2013 RVA: 0x000C8214 File Offset: 0x000C6414
            private bool QuquIsDead()
            {
                bool flag = this.actorQuquHp[this.ququBattleTurn] <= 0 || this.actorQuquSp[this.ququBattleTurn] <= 0 || DateFile.instance.ParseInt(DateFile.instance.GetItemDate(this.actorQuquId[this.ququBattleTurn], 901, true)) <= 0;
                bool flag2 = this.enemyQuquHp[this.ququBattleTurn] <= 0 || this.enemyQuquSp[this.ququBattleTurn] <= 0 || DateFile.instance.ParseInt(DateFile.instance.GetItemDate(this.enemyQuquId[this.ququBattleTurn], 901, true)) <= 0;
                return flag || flag2;
            }

            // Token: 0x060007DE RID: 2014 RVA: 0x000C82CC File Offset: 0x000C64CC
            private void AddQuquBattleMassage(bool win)
            {
                int num = this.actorQuquId[this.ququBattleTurn];
                int id = this.enemyQuquId[this.ququBattleTurn];
                if (win)
                {
                    int num2 = DateFile.instance.ParseInt(DateFile.instance.GetItemDate(num, 8, true));
                    int num3 = DateFile.instance.ParseInt(DateFile.instance.GetItemDate(id, 8, true));
                    bool flag = num3 + 3 >= num2;
                    if (flag)
                    {
                        DateFile.instance.itemsDate[num][2006] = (DateFile.instance.ParseInt(DateFile.instance.GetItemDate(num, 2006, true)) + 1).ToString();
                        DateFile.instance.AddActorScore(502, num3 * 100);
                        int num4 = DateFile.instance.ParseInt(DateFile.instance.GetItemDate(num, 2010, true));
                        int key = DateFile.instance.ParseInt(DateFile.instance.GetItemDate(num, 2011, true));
                        int num5 = (num4 == 0) ? 0 : ((DateFile.instance.ParseInt(DateFile.instance.cricketDate[num4][1]) >= DateFile.instance.ParseInt(DateFile.instance.cricketDate[key][1])) ? DateFile.instance.ParseInt(DateFile.instance.cricketDate[num4][1]) : DateFile.instance.ParseInt(DateFile.instance.cricketDate[key][1]));
                        bool flag2 = num3 >= num5;
                        if (flag2)
                        {
                            DateFile.instance.itemsDate[num][2010] = DateFile.instance.GetItemDate(id, 2002, true);
                            DateFile.instance.itemsDate[num][2011] = DateFile.instance.GetItemDate(id, 2003, true);
                        }
                    }
                }
                else
                {
                    DateFile.instance.itemsDate[num][2009] = (DateFile.instance.ParseInt(DateFile.instance.GetItemDate(num, 2009, true)) + 1).ToString();
                }
            }

            // Token: 0x060007DF RID: 2015 RVA: 0x000C8518 File Offset: 0x000C6718
            private void QuquBattleLoopStart(float jumpSpeed, float delay)
            {
                TweenSettingsExtensions.SetDelay<Sequence>(ShortcutExtensions.DOLocalJump(this.actorQuqu[this.ququBattleTurn], new Vector3(450f, 0f, 0f), 20f, 1, jumpSpeed, false), delay);
                TweenSettingsExtensions.OnComplete<Sequence>(TweenSettingsExtensions.SetDelay<Sequence>(ShortcutExtensions.DOLocalJump(this.enemyQuqu[this.ququBattleTurn], new Vector3(-450f, 0f, 0f), 20f, 1, jumpSpeed, false), delay), delegate ()
                {
                    this.actorQuqu[this.ququBattleTurn].GetComponent<QuquPlace>().UpdateBattleQuquCall(this.actorQuquId[this.ququBattleTurn]);
                    this.actorQuqu[this.ququBattleTurn].GetComponent<QuquPlace>().CallvolumeRest(0.8f, 0.2f);
                    this.enemyQuqu[this.ququBattleTurn].GetComponent<QuquPlace>().UpdateBattleQuquCall(this.enemyQuquId[this.ququBattleTurn]);
                    this.enemyQuqu[this.ququBattleTurn].GetComponent<QuquPlace>().CallvolumeRest(0.8f, 0.2f);
                    int ququDate = GetQuquWindow.instance.GetQuquDate(this.actorQuquId[this.ququBattleTurn], 21, true);
                    int ququDate2 = GetQuquWindow.instance.GetQuquDate(this.enemyQuquId[this.ququBattleTurn], 21, true);
                    bool flag = ququDate > ququDate2;
                    int attacker;
                    if (flag)
                    {
                        this.ShowDamage(true, false, 2, ququDate, 0.6f, false, false, false, false, false, 0, 0);
                        attacker = ((Random.Range(0, 100) < 80) ? 1 : 2);
                    }
                    else
                    {
                        bool flag2 = ququDate2 > ququDate;
                        if (flag2)
                        {
                            this.ShowDamage(false, true, 2, ququDate2, 0.6f, false, false, false, false, false, 0, 0);
                            attacker = ((Random.Range(0, 100) < 80) ? 2 : 1);
                        }
                        else
                        {
                            attacker = 1 + Random.Range(0, 2);
                        }
                    }
                    bool flag3 = !this.QuquIsDead();
                    if (flag3)
                    {
                        this.QuquAttack(1f, attacker, false);
                    }
                });
            }

            // Token: 0x060007E0 RID: 2016 RVA: 0x000C85A4 File Offset: 0x000C67A4
            private void QuquAttack(float baseDelay, int attacker, bool newTurn)
            {
                TweenSettingsExtensions.OnComplete<Sequence>(TweenSettingsExtensions.SetDelay<Sequence>(ShortcutExtensions.DOLocalJump(this.actorQuqu[this.ququBattleTurn], new Vector3(330f, 0f, 0f), 20f, 1, (attacker == 1) ? 0.02f : 0.01f, false), 1f + baseDelay), delegate ()
                {
                    bool flag = attacker == 1;
                    if (flag)
                    {
                        this.QuquBaseAttacke(1, newTurn);
                    }
                });
                TweenSettingsExtensions.OnComplete<Sequence>(TweenSettingsExtensions.SetDelay<Sequence>(ShortcutExtensions.DOLocalJump(this.enemyQuqu[this.ququBattleTurn], new Vector3(-330f, 0f, 0f), 20f, 1, (attacker == 1) ? 0.01f : 0.02f, false), 1f + baseDelay), delegate ()
                {
                    bool flag = attacker == 2;
                    if (flag)
                    {
                        this.QuquBaseAttacke(2, newTurn);
                    }
                });
            }

            // Token: 0x060007E1 RID: 2017 RVA: 0x000C8690 File Offset: 0x000C6890
            private void QuquBaseAttacke(int attacker, bool newTurn)
            {
                bool _cHit = false;
                bool _def = false;
                bool _reAttack = false;
                float num = 0.4f;
                bool flag = attacker == 1;
                if (flag)
                {
                    int _actorQuquDamage = GetQuquWindow.instance.GetQuquDate(this.actorQuquId[this.ququBattleTurn], 23, true);
                    bool flag2 = Random.Range(0, 100) < GetQuquWindow.instance.GetQuquDate(this.actorQuquId[this.ququBattleTurn], 31, true);
                    if (flag2)
                    {
                        _cHit = true;
                        _actorQuquDamage += GetQuquWindow.instance.GetQuquDate(this.actorQuquId[this.ququBattleTurn], 32, true);
                    }
                    bool flag3 = Random.Range(0, 100) < GetQuquWindow.instance.GetQuquDate(this.enemyQuquId[this.ququBattleTurn], 33, true);
                    if (flag3)
                    {
                        _def = true;
                        _actorQuquDamage = Mathf.Max(0, _actorQuquDamage - GetQuquWindow.instance.GetQuquDate(this.enemyQuquId[this.ququBattleTurn], 34, true));
                    }
                    bool flag4 = Random.Range(0, 100) < GetQuquWindow.instance.GetQuquDate(this.enemyQuquId[this.ququBattleTurn], 35, true);
                    if (flag4)
                    {
                        num += 0.4f;
                        _reAttack = true;
                    }
                    TweenSettingsExtensions.OnComplete<Sequence>(TweenSettingsExtensions.SetDelay<Sequence>(ShortcutExtensions.DOLocalJump(this.actorQuqu[this.ququBattleTurn], new Vector3(605f, 0f, 0f), 20f, 1, 0.01f, false), num), delegate ()
                    {
                        this.ShowDamage(true, false, 1, _actorQuquDamage, 0.1f, _cHit, _def, _reAttack, false, newTurn, 0, 22);
                    });
                }
                else
                {
                    int _enemyQuquDamage = GetQuquWindow.instance.GetQuquDate(this.enemyQuquId[this.ququBattleTurn], 23, true);
                    bool flag5 = Random.Range(0, 100) < GetQuquWindow.instance.GetQuquDate(this.enemyQuquId[this.ququBattleTurn], 31, true);
                    if (flag5)
                    {
                        _cHit = true;
                        _enemyQuquDamage += GetQuquWindow.instance.GetQuquDate(this.enemyQuquId[this.ququBattleTurn], 32, true);
                    }
                    bool flag6 = Random.Range(0, 100) < GetQuquWindow.instance.GetQuquDate(this.actorQuquId[this.ququBattleTurn], 33, true);
                    if (flag6)
                    {
                        _def = true;
                        _enemyQuquDamage = Mathf.Max(0, _enemyQuquDamage - GetQuquWindow.instance.GetQuquDate(this.actorQuquId[this.ququBattleTurn], 34, true));
                    }
                    bool flag7 = Random.Range(0, 100) < GetQuquWindow.instance.GetQuquDate(this.actorQuquId[this.ququBattleTurn], 35, true);
                    if (flag7)
                    {
                        num += 0.4f;
                        _reAttack = true;
                    }
                    TweenSettingsExtensions.OnComplete<Sequence>(TweenSettingsExtensions.SetDelay<Sequence>(ShortcutExtensions.DOLocalJump(this.enemyQuqu[this.ququBattleTurn], new Vector3(-605f, 0f, 0f), 20f, 1, 0.01f, false), num), delegate ()
                    {
                        this.ShowDamage(false, true, 1, _enemyQuquDamage, 0.1f, _cHit, _def, _reAttack, false, newTurn, 0, 22);
                    });
                }
            }

            // Token: 0x060007E2 RID: 2018 RVA: 0x000C89E4 File Offset: 0x000C6BE4
            private void ShowDamage(bool attacker, bool defer, int typ, int damage, float delay, bool cHit, bool def, bool reAttack, bool isReAttack, bool newTurn, int reAttackTutn, int reAttackTyp)
            {
                bool _cHit = false;
                bool _def = false;
                float size = cHit ? 2.4f : 1.8f;
                float num = delay;
                string text = "";
                Color textColor;
                textColor = new Color(1f, 1f, 1f, 1f);
                if (typ != 1)
                {
                    if (typ == 2)
                    {
                        if (defer)
                        {
                            this.actorQuquSp[this.ququBattleTurn] -= damage;
                            text = string.Format("{0}-{1}", DateFile.instance.massageDate[8004][0].Split(new char[]
                            {
                        '|'
                            })[1], damage);
                            bool flag = this.QuquIsDead();
                            if (flag)
                            {
                                ShortcutExtensions.DOKill(this.actorQuqu[this.ququBattleTurn], false);
                                ShortcutExtensions.DOKill(this.enemyQuqu[this.ququBattleTurn], false);
                                this.battleStateText[this.ququBattleTurn].text = "";
                                TweenSettingsExtensions.OnComplete<Tweener>(this.nextButtonMask[this.ququBattleTurn].GetComponent<Image>().DOColor(new Color(0f, 0f, 0f, 0.5f), 1f), delegate ()
                                {
                                    this.SetBattleStateText(DateFile.instance.SetColoer(20010, DateFile.instance.massageDate[8003][4].Split(new char[]
                                    {
                                '|'
                                    })[1], false), 0.4f, 3f, 2);
                                });
                            }
                        }
                        else
                        {
                            this.enemyQuquSp[this.ququBattleTurn] -= damage;
                            text = string.Format("{0}-{1}", DateFile.instance.massageDate[8004][0].Split(new char[]
                            {
                        '|'
                            })[1], damage);
                            bool flag2 = this.QuquIsDead();
                            if (flag2)
                            {
                                ShortcutExtensions.DOKill(this.actorQuqu[this.ququBattleTurn], false);
                                ShortcutExtensions.DOKill(this.enemyQuqu[this.ququBattleTurn], false);
                                this.battleStateText[this.ququBattleTurn].text = "";
                                TweenSettingsExtensions.OnComplete<Tweener>(this.nextButtonMask[this.ququBattleTurn].GetComponent<Image>().DOColor(new Color(0f, 0f, 0f, 0.5f), 1f), delegate ()
                                {
                                    this.SetBattleStateText(DateFile.instance.SetColoer(20005, DateFile.instance.massageDate[8003][4].Split(new char[]
                                    {
                                '|'
                                    })[0], false), 0.4f, 3f, 1);
                                });
                            }
                        }
                    }
                }
                else if (defer)
                {
                    int num2 = 0;
                    bool flag3 = cHit || isReAttack;
                    if (flag3)
                    {
                        num2 = GetQuquWindow.instance.GetQuquDate(this.enemyQuquId[this.ququBattleTurn], 21, true);
                    }
                    if (def)
                    {
                        num2 = Mathf.Max(0, num2 - GetQuquWindow.instance.GetQuquDate(this.actorQuquId[this.ququBattleTurn], 34, true));
                        textColor = new Color(1f, 0.784313738f, 0f, 1f);
                    }
                    else if (cHit)
                    {
                        this.enemyQuqu[this.ququBattleTurn].GetComponent<QuquPlace>().UpdateBattleQuquCall(this.enemyQuquId[this.ququBattleTurn]);
                        this.enemyQuqu[this.ququBattleTurn].GetComponent<QuquPlace>().CallvolumeRest(0.8f, 0.2f);
                        int num3 = GetQuquWindow.instance.GetQuquDate(this.enemyQuquId[this.ququBattleTurn], 31, true) + GetQuquWindow.instance.GetQuquDate(this.enemyQuquId[this.ququBattleTurn], 36, true);
                        bool flag4 = Random.Range(0, 100) < num3;
                        if (flag4)
                        {
                            DateFile.instance.ChangeItemHp(DateFile.instance.MianActorID(), this.actorQuquId[this.ququBattleTurn], -1, 0, true);
                            bool flag5 = Random.Range(0, 100) < num3;
                            if (flag5)
                            {
                                GetQuquWindow.instance.QuquAddInjurys(this.actorQuquId[this.ququBattleTurn]);
                            }
                        }
                        textColor = new Color(1f, 0f, 0f, 1f);
                    }
                    this.actorQuquHp[this.ququBattleTurn] -= damage;
                    text = string.Format("{0}-{1}", DateFile.instance.massageDate[8004][0].Split(new char[]
                    {
                '|'
                    })[0], damage);
                    bool flag6 = num2 > 0;
                    if (flag6)
                    {
                        this.actorQuquSp[this.ququBattleTurn] -= num2;
                        text += string.Format("\n{0}-{1}", DateFile.instance.massageDate[8004][0].Split(new char[]
                        {
                    '|'
                        })[1], num2);
                    }
                    bool flag7 = this.QuquIsDead();
                    if (flag7)
                    {
                        ShortcutExtensions.DOKill(this.actorQuqu[this.ququBattleTurn], false);
                        ShortcutExtensions.DOKill(this.enemyQuqu[this.ququBattleTurn], false);
                        this.battleStateText[this.ququBattleTurn].text = "";
                        TweenSettingsExtensions.OnComplete<Tweener>(this.nextButtonMask[this.ququBattleTurn].GetComponent<Image>().DOColor(new Color(0f, 0f, 0f, 0.5f), 1f), delegate ()
                        {
                            this.SetBattleStateText(DateFile.instance.SetColoer(20010, DateFile.instance.massageDate[8003][4].Split(new char[]
                            {
                        '|'
                            })[1], false), 0.4f, 3f, 2);
                        });
                        this.Damage(defer, size, delay, text, textColor);
                        return;
                    }
                    if (def)
                    {
                        this.actorQuqu[this.ququBattleTurn].GetComponent<QuquPlace>().UpdateBattleQuquCall(this.actorQuquId[this.ququBattleTurn]);
                        this.actorQuqu[this.ququBattleTurn].GetComponent<QuquPlace>().CallvolumeRest(0.8f, 0.2f);
                        TweenSettingsExtensions.OnComplete<Sequence>(TweenSettingsExtensions.SetDelay<Sequence>(ShortcutExtensions.DOLocalJump(this.actorQuqu[this.ququBattleTurn], new Vector3(this.actorQuqu[this.ququBattleTurn].localPosition.x - 20f, 0f, 0f), 20f, 1, 0.01f, false), delay), delegate ()
                        {
                            TweenSettingsExtensions.SetDelay<Sequence>(ShortcutExtensions.DOLocalJump(this.actorQuqu[this.ququBattleTurn], new Vector3(this.actorQuqu[this.ququBattleTurn].localPosition.x + 20f, 0f, 0f), 20f, 1, 0.01f, false), 0.4f);
                        });
                        num += 0.6f;
                    }
                    if (reAttack)
                    {
                        int _enemyQuquDamage = GetQuquWindow.instance.GetQuquDate(this.actorQuquId[this.ququBattleTurn], reAttackTyp, true);
                        bool flag8 = Random.Range(0, 100) < GetQuquWindow.instance.GetQuquDate(this.actorQuquId[this.ququBattleTurn], 31, true);
                        if (flag8)
                        {
                            _cHit = true;
                            _enemyQuquDamage += GetQuquWindow.instance.GetQuquDate(this.actorQuquId[this.ququBattleTurn], 32, true);
                        }
                        bool flag9 = Random.Range(0, 100) < GetQuquWindow.instance.GetQuquDate(this.enemyQuquId[this.ququBattleTurn], 33, true);
                        if (flag9)
                        {
                            _def = true;
                            _enemyQuquDamage = Mathf.Max(0, _enemyQuquDamage - GetQuquWindow.instance.GetQuquDate(this.enemyQuquId[this.ququBattleTurn], 34, true));
                        }
                        //    TweenSettingsExtensions.OnComplete<Sequence>(TweenSettingsExtensions.SetDelay<Sequence>(ShortcutExtensions.DOLocalJump(this.actorQuqu[this.ququBattleTurn], new Vector3(this.actorQuqu[this.ququBattleTurn].localPosition.x + 20f, 0f, 0f), 20f, 1, 0.01f, false), num + 0.6f), delegate ()
                        //    {
                        //        bool flag17 = false;
                        //        int num6 = GetQuquWindow.instance.GetQuquDate(this.enemyQuquId[this.ququBattleTurn], 35, true) - reAttackTutn * 5;
                        //        bool flag18 = Random.Range(0, 100) < num6;
                        //        if (flag18)
                        //        {
                        //            flag17 = true;
                        //        }
                        //        QuquBattleSystem <> 4__this = this;
                        //        bool attacker4 = attacker;
                        //        bool defer2 = false;
                        //        int typ2 = 1;
                        //        int enemyQuquDamage = _enemyQuquDamage;
                        //        float delay2 = 0.1f;
                        //        bool cHit2 = _cHit;
                        //        bool def2 = _def;
                        //        bool reAttack2 = flag17;
                        //        bool isReAttack2 = true;
                        //        bool newTurn2 = false;
                        //        int reAttackTutn2 = reAttackTutn;
                        //        reAttackTutn = reAttackTutn2 + 1;

                        //<> 4__this.ShowDamage(attacker4, defer2, typ2, enemyQuquDamage, delay2, cHit2, def2, reAttack2, isReAttack2, newTurn2, reAttackTutn2, (reAttackTyp == 22) ? 23 : 22);
                        //        bool flag19 = !this.QuquIsDead();
                        //        if (flag19)
                        //        {
                        //            TweenSettingsExtensions.SetDelay<Sequence>(ShortcutExtensions.DOLocalJump(this.actorQuqu[this.ququBattleTurn], new Vector3(this.actorQuqu[this.ququBattleTurn].localPosition.x - 20f, 0f, 0f), 20f, 1, 0.01f, false), 0.4f);
                        //        }
                        //    });
                    }
                    else
                    {
                        bool attacker2 = attacker;
                        if (attacker2)
                        {
                            TweenSettingsExtensions.OnComplete<Sequence>(TweenSettingsExtensions.SetDelay<Sequence>(ShortcutExtensions.DOLocalJump(this.actorQuqu[this.ququBattleTurn], new Vector3(330f, 0f, 0f), 20f, 1, 0.01f, false), num + 1.4f), delegate ()
                            {
                                bool newTurn2 = newTurn;
                                if (newTurn2)
                                {
                                    this.QuquBattleLoopStart(0.02f, 0.6f);
                                }
                                else
                                {
                                    this.QuquBaseAttacke(2, true);
                                }
                            });
                        }
                        else
                        {
                            TweenSettingsExtensions.OnComplete<Sequence>(TweenSettingsExtensions.SetDelay<Sequence>(ShortcutExtensions.DOLocalJump(this.enemyQuqu[this.ququBattleTurn], new Vector3(-330f, 0f, 0f), 20f, 1, 0.01f, false), num + 1.4f), delegate ()
                            {
                                bool newTurn2 = newTurn;
                                if (newTurn2)
                                {
                                    this.QuquBattleLoopStart(0.02f, 0.6f);
                                }
                                else
                                {
                                    this.QuquBaseAttacke(1, true);
                                }
                            });
                        }
                    }
                }
                else
                {
                    int num4 = 0;
                    bool flag10 = cHit || isReAttack;
                    if (flag10)
                    {
                        num4 = GetQuquWindow.instance.GetQuquDate(this.actorQuquId[this.ququBattleTurn], 21, true);
                    }
                    if (def)
                    {
                        num4 = Mathf.Max(0, num4 - GetQuquWindow.instance.GetQuquDate(this.enemyQuquId[this.ququBattleTurn], 34, true));
                        textColor = new Color(1f, 0.784313738f, 0f, 1f);
                    }
                    else if (cHit)
                    {
                        this.actorQuqu[this.ququBattleTurn].GetComponent<QuquPlace>().UpdateBattleQuquCall(this.actorQuquId[this.ququBattleTurn]);
                        this.actorQuqu[this.ququBattleTurn].GetComponent<QuquPlace>().CallvolumeRest(0.8f, 0.2f);
                        int num5 = GetQuquWindow.instance.GetQuquDate(this.actorQuquId[this.ququBattleTurn], 31, true) + GetQuquWindow.instance.GetQuquDate(this.actorQuquId[this.ququBattleTurn], 36, true);
                        bool flag11 = Random.Range(0, 100) < num5;
                        if (flag11)
                        {
                            DateFile.instance.ChangeItemHp(0, this.enemyQuquId[this.ququBattleTurn], -1, 0, true);
                            bool flag12 = Random.Range(0, 100) < num5;
                            if (flag12)
                            {
                                GetQuquWindow.instance.QuquAddInjurys(this.enemyQuquId[this.ququBattleTurn]);
                            }
                        }
                        textColor = new Color(1f, 0f, 0f, 1f);
                    }
                    this.enemyQuquHp[this.ququBattleTurn] -= damage;
                    text = string.Format("{0}-{1}", DateFile.instance.massageDate[8004][0].Split(new char[]
                    {
                '|'
                    })[0], damage);
                    bool flag13 = num4 > 0;
                    if (flag13)
                    {
                        this.enemyQuquSp[this.ququBattleTurn] -= num4;
                        text += string.Format("\n{0}-{1}", DateFile.instance.massageDate[8004][0].Split(new char[]
                        {
                    '|'
                        })[1], num4);
                    }
                    bool flag14 = this.QuquIsDead();
                    if (flag14)
                    {
                        ShortcutExtensions.DOKill(this.actorQuqu[this.ququBattleTurn], false);
                        ShortcutExtensions.DOKill(this.enemyQuqu[this.ququBattleTurn], false);
                        this.battleStateText[this.ququBattleTurn].text = "";
                        TweenSettingsExtensions.OnComplete<Tweener>(this.nextButtonMask[this.ququBattleTurn].GetComponent<Image>().DOColor(new Color(0f, 0f, 0f, 0.5f), 1f), delegate ()
                        {
                            this.SetBattleStateText(DateFile.instance.SetColoer(20005, DateFile.instance.massageDate[8003][4].Split(new char[]
                            {
                        '|'
                            })[0], false), 0.4f, 3f, 1);
                        });
                        this.Damage(defer, size, delay, text, textColor);
                        return;
                    }
                    if (def)
                    {
                        this.enemyQuqu[this.ququBattleTurn].GetComponent<QuquPlace>().UpdateBattleQuquCall(this.enemyQuquId[this.ququBattleTurn]);
                        this.enemyQuqu[this.ququBattleTurn].GetComponent<QuquPlace>().CallvolumeRest(0.8f, 0.2f);
                        TweenSettingsExtensions.OnComplete<Sequence>(TweenSettingsExtensions.SetDelay<Sequence>(ShortcutExtensions.DOLocalJump(this.enemyQuqu[this.ququBattleTurn], new Vector3(this.enemyQuqu[this.ququBattleTurn].localPosition.x + 20f, 0f, 0f), 20f, 1, 0.01f, false), delay), delegate ()
                        {
                            TweenSettingsExtensions.SetDelay<Sequence>(ShortcutExtensions.DOLocalJump(this.enemyQuqu[this.ququBattleTurn], new Vector3(this.enemyQuqu[this.ququBattleTurn].localPosition.x - 20f, 0f, 0f), 20f, 1, 0.01f, false), 0.4f);
                        });
                        num += 0.6f;
                    }
                    if (reAttack)
                    {
                        int _actorQuquDamage = GetQuquWindow.instance.GetQuquDate(this.enemyQuquId[this.ququBattleTurn], reAttackTyp, true);
                        bool flag15 = Random.Range(0, 100) < GetQuquWindow.instance.GetQuquDate(this.enemyQuquId[this.ququBattleTurn], 31, true);
                        if (flag15)
                        {
                            _cHit = true;
                            _actorQuquDamage += GetQuquWindow.instance.GetQuquDate(this.enemyQuquId[this.ququBattleTurn], 32, true);
                        }
                        bool flag16 = Random.Range(0, 100) < GetQuquWindow.instance.GetQuquDate(this.actorQuquId[this.ququBattleTurn], 33, true);
                        if (flag16)
                        {
                            _def = true;
                            _actorQuquDamage = Mathf.Max(0, _actorQuquDamage - GetQuquWindow.instance.GetQuquDate(this.actorQuquId[this.ququBattleTurn], 34, true));
                        }
                        //    TweenSettingsExtensions.OnComplete<Sequence>(TweenSettingsExtensions.SetDelay<Sequence>(ShortcutExtensions.DOLocalJump(this.enemyQuqu[this.ququBattleTurn], new Vector3(this.enemyQuqu[this.ququBattleTurn].localPosition.x - 20f, 0f, 0f), 20f, 1, 0.01f, false), num + 0.6f), delegate ()
                        //    {
                        //        bool flag17 = false;
                        //        int num6 = GetQuquWindow.instance.GetQuquDate(this.actorQuquId[this.ququBattleTurn], 35, true) - reAttackTutn * 5;
                        //        bool flag18 = Random.Range(0, 100) < num6;
                        //        if (flag18)
                        //        {
                        //            flag17 = true;
                        //        }
                        //        QuquBattleSystem <> 4__this = this;
                        //        bool attacker4 = attacker;
                        //        bool defer2 = true;
                        //        int typ2 = 1;
                        //        int actorQuquDamage = _actorQuquDamage;
                        //        float delay2 = 0.1f;
                        //        bool cHit2 = _cHit;
                        //        bool def2 = _def;
                        //        bool reAttack2 = flag17;
                        //        bool isReAttack2 = true;
                        //        bool newTurn2 = false;
                        //        int reAttackTutn2 = reAttackTutn;
                        //        reAttackTutn = reAttackTutn2 + 1;

                        //<> 4__this.ShowDamage(attacker4, defer2, typ2, actorQuquDamage, delay2, cHit2, def2, reAttack2, isReAttack2, newTurn2, reAttackTutn2, (reAttackTyp == 22) ? 23 : 22);
                        //        bool flag19 = !this.QuquIsDead();
                        //        if (flag19)
                        //        {
                        //            TweenSettingsExtensions.SetDelay<Sequence>(ShortcutExtensions.DOLocalJump(this.enemyQuqu[this.ququBattleTurn], new Vector3(this.enemyQuqu[this.ququBattleTurn].localPosition.x + 20f, 0f, 0f), 20f, 1, 0.01f, false), 0.4f);
                        //        }
                        //    });
                    }
                    else
                    {
                        bool attacker3 = attacker;
                        if (attacker3)
                        {
                            TweenSettingsExtensions.OnComplete<Sequence>(TweenSettingsExtensions.SetDelay<Sequence>(ShortcutExtensions.DOLocalJump(this.actorQuqu[this.ququBattleTurn], new Vector3(330f, 0f, 0f), 20f, 1, 0.01f, false), num + 1.4f), delegate ()
                            {
                                bool newTurn2 = newTurn;
                                if (newTurn2)
                                {
                                    this.QuquBattleLoopStart(0.02f, 0.6f);
                                }
                                else
                                {
                                    this.QuquBaseAttacke(2, true);
                                }
                            });
                        }
                        else
                        {
                            TweenSettingsExtensions.OnComplete<Sequence>(TweenSettingsExtensions.SetDelay<Sequence>(ShortcutExtensions.DOLocalJump(this.enemyQuqu[this.ququBattleTurn], new Vector3(-330f, 0f, 0f), 20f, 1, 0.01f, false), num + 1.4f), delegate ()
                            {
                                bool newTurn2 = newTurn;
                                if (newTurn2)
                                {
                                    this.QuquBattleLoopStart(0.02f, 0.6f);
                                }
                                else
                                {
                                    this.QuquBaseAttacke(1, true);
                                }
                            });
                        }
                    }
                }
                this.Damage(defer, size, delay, text, textColor);
            }

            // Token: 0x060007E3 RID: 2019 RVA: 0x000C9844 File Offset: 0x000C7A44
            private void Damage(bool isActor, float size, float delay, string damageText, Color textColor)
            {
                float num = size / 2f - 0.2f;
                if (isActor)
                {
                    ShortcutExtensions.DOKill(this.actorQuquDamageText[this.ququBattleTurn], false);
                    TweenSettingsExtensions.OnComplete<Tweener>(TweenSettingsExtensions.SetDelay<Tweener>(ShortcutExtensions.DOShakePosition(this.actorQuquIcon[this.ququBattleTurn].transform, 0.1f, 20f, 10, 90f, false, true), 0.01f + delay), new TweenCallback(this.RestBattlerPosition));
                    TweenSettingsExtensions.OnStart<Tweener>(TweenSettingsExtensions.SetEase<Tweener>(TweenSettingsExtensions.SetDelay<Tweener>(ShortcutExtensions.DOScale(this.actorQuquDamageText[this.ququBattleTurn].transform, new Vector3(size, size, 1f), 0.1f), delay), (Ease)27), delegate ()
                    {
                        this.actorQuquDamageText[this.ququBattleTurn].color = textColor;
                        this.actorQuquDamageText[this.ququBattleTurn].transform.localScale = new Vector3(0f, 0f, 1f);
                        this.actorQuquDamageText[this.ququBattleTurn].transform.localPosition = new Vector3(0f, 0f, 0f);
                        this.actorQuquDamageText[this.ququBattleTurn].text = damageText;
                    });
                    TweenSettingsExtensions.SetEase<Tweener>(TweenSettingsExtensions.SetDelay<Tweener>(ShortcutExtensions.DOScale(this.actorQuquDamageText[this.ququBattleTurn].transform, new Vector3(num, num, 1f), 0.4f), 0.1f + delay), (Ease)27);
                    TweenSettingsExtensions.SetDelay<Tweener>(ShortcutExtensions.DOLocalMoveY(this.actorQuquDamageText[this.ququBattleTurn].transform, 60f, 1.4f, false), 0.4f + delay);
                    TweenSettingsExtensions.SetDelay<Tweener>(this.actorQuquDamageText[this.ququBattleTurn].DOFade(0f, 0.3f), 1.5f + delay);
                }
                else
                {
                    ShortcutExtensions.DOKill(this.enemyQuquDamageText[this.ququBattleTurn], false);
                    TweenSettingsExtensions.OnComplete<Tweener>(TweenSettingsExtensions.SetDelay<Tweener>(ShortcutExtensions.DOShakePosition(this.enemyQuquIcon[this.ququBattleTurn].transform, 0.1f, 20f, 10, 90f, false, true), 0.01f + delay), new TweenCallback(this.RestBattlerPosition));
                    TweenSettingsExtensions.OnStart<Tweener>(TweenSettingsExtensions.SetEase<Tweener>(TweenSettingsExtensions.SetDelay<Tweener>(ShortcutExtensions.DOScale(this.enemyQuquDamageText[this.ququBattleTurn].transform, new Vector3(size, size, 1f), 0.1f), delay), (Ease)27), delegate ()
                    {
                        this.enemyQuquDamageText[this.ququBattleTurn].color = textColor;
                        this.enemyQuquDamageText[this.ququBattleTurn].transform.localScale = new Vector3(0f, 0f, 1f);
                        this.enemyQuquDamageText[this.ququBattleTurn].transform.localPosition = new Vector3(0f, 0f, 0f);
                        this.enemyQuquDamageText[this.ququBattleTurn].text = damageText;
                    });
                    TweenSettingsExtensions.SetEase<Tweener>(TweenSettingsExtensions.SetDelay<Tweener>(ShortcutExtensions.DOScale(this.enemyQuquDamageText[this.ququBattleTurn].transform, new Vector3(num, num, 1f), 0.4f), 0.1f + delay), (Ease)27);
                    TweenSettingsExtensions.SetDelay<Tweener>(ShortcutExtensions.DOLocalMoveY(this.enemyQuquDamageText[this.ququBattleTurn].transform, 60f, 1.4f, false), 0.4f + delay);
                    TweenSettingsExtensions.SetDelay<Tweener>(this.enemyQuquDamageText[this.ququBattleTurn].DOFade(0f, 0.3f), 1.5f + delay);
                }
                this.UpdateQuquHp(this.ququBattleTurn);
            }

            // Token: 0x060007E4 RID: 2020 RVA: 0x000C9B0C File Offset: 0x000C7D0C
            private void RestBattlerPosition()
            {
                this.actorQuquIcon[this.ququBattleTurn].transform.localPosition = new Vector3(0f, 0f, 0f);
                this.enemyQuquIcon[this.ququBattleTurn].transform.localPosition = new Vector3(0f, 0f, 0f);
            }

            // Token: 0x060007E5 RID: 2021 RVA: 0x000C9B74 File Offset: 0x000C7D74
            private void BattleEndWindow(int typ)
            {
                this.battleEndBodyText.text = "";
                this.battleEndBodyName.text = DateFile.instance.massageDate[8004][1].Split(new char[]
                {
            '|'
                })[typ];
                this.battleEndTypImage.sprite = GetSprites.instance.battleEndTypImage[typ];
                this.battleEndWindow.SetActive(true);
                TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScale(this.battleEndWindow.transform, new Vector3(1f, 1f, 1f), 0.25f), (Ease)6);
                Component[] componentsInChildren = this.battleEndWindow.GetComponentsInChildren<Component>();
                foreach (Component component in componentsInChildren)
                {
                    bool flag = component is Graphic;
                    if (flag)
                    {
                        (component as Graphic).CrossFadeAlpha(1f, 0.25f, true);
                    }
                }
                base.StartCoroutine(this.QuquBattleWin(0.25f, typ));
            }

            // Token: 0x060007E6 RID: 2022 RVA: 0x0000540F File Offset: 0x0000360F
            private IEnumerator QuquBattleWin(float waitTime, int typ)
            {
                yield return new WaitForSeconds(waitTime);
                int _mianActorId = DateFile.instance.MianActorID();
                bool flag = typ == 0;
                if (flag)
                {
                    this.battleEndBodyText.text = this.enemyBodyNameText.text;
                    int _baseEnemyId = DateFile.instance.ParseInt(DateFile.instance.cricketBattleDate[this.ququBattleId][1]);
                    bool flag2 = _baseEnemyId == 0;
                    if (flag2)
                    {
                        int num = this.enemyBodyTyp;
                        if (num != 0)
                        {
                            if (num == 1)
                            {
                                DateFile.instance.ChangeTwoActorItem(this.ququBattleEnemyId, _mianActorId, this.enemyBodyId, 1, -1, 0, 0);
                            }
                        }
                        else
                        {
                            UIDate.instance.ChangeResource(this.ququBattleEnemyId, this.enemyBodyId, -this.enemyBodySize, false);
                            UIDate.instance.ChangeResource(DateFile.instance.MianActorID(), this.enemyBodyId, this.enemyBodySize, false);
                        }
                    }
                    else
                    {
                        switch (this.enemyBodyTyp)
                        {
                            case 0:
                                UIDate.instance.ChangeResource(DateFile.instance.MianActorID(), this.enemyBodyId, this.enemyBodySize, false);
                                break;
                            case 1:
                                {
                                    bool flag3 = DateFile.instance.ParseInt(DateFile.instance.GetItemDate(this.enemyBodyId, 6, true)) == 0;
                                    if (flag3)
                                    {
                                        int _newItemId = DateFile.instance.MakeNewItem(DateFile.instance.ParseInt(DateFile.instance.GetItemDate(this.enemyBodyId, 999, true)), 0, 0, 50, 20);
                                        List<int> _bodyDate = new List<int>(DateFile.instance.itemsDate[this.enemyBodyId].Keys);
                                        int num2;
                                        for (int i = 0; i < _bodyDate.Count; i = num2 + 1)
                                        {
                                            int _itemAttId = _bodyDate[i];
                                            DateFile.instance.itemsDate[_newItemId][_itemAttId] = DateFile.instance.itemsDate[this.enemyBodyId][_itemAttId];
                                            num2 = i;
                                        }
                                        DateFile.instance.GetItem(DateFile.instance.MianActorID(), _newItemId, 1, false, 0, 0);
                                        _bodyDate = null;
                                    }
                                    else
                                    {
                                        DateFile.instance.GetItem(DateFile.instance.MianActorID(), this.enemyBodyId, 1, true, 0, 0);
                                    }
                                    break;
                                }
                            case 2:
                                {
                                    bool flag4 = this.bodyActorId == -99;
                                    if (!flag4)
                                    {
                                        DateFile.instance.GetActor(new List<int>
                        {
                            this.bodyActorId
                        }, 4);
                                    }
                                    break;
                                }
                        }
                    }
                    string[] _eventDate = DateFile.instance.cricketBattleDate[this.ququBattleId][6].Split(new char[]
                    {
                '&'
                    });
                    int _eventId = DateFile.instance.ParseInt(_eventDate[0]);
                    bool flag5 = _eventId != 0;
                    if (flag5)
                    {
                        bool flag6 = _eventDate.Length > 1;
                        if (flag6)
                        {
                            int num3 = DateFile.instance.ParseInt(_eventDate[1]);
                            if (num3 == 1)
                            {
                                DateFile.instance.SetEvent(new int[]
                                {
                            0,
                            this.ququBattleEnemyId,
                            _eventId,
                            this.ququBattleEnemyId
                                }, true, true);
                            }
                        }
                        else
                        {
                            DateFile.instance.SetEvent(new int[]
                            {
                        0,
                        -1,
                        _eventId
                            }, true, true);
                        }
                    }
                    _eventDate = null;
                }
                else
                {
                    this.battleEndBodyText.text = this.actorBodyNameText.text;
                    int _baseEnemyId2 = DateFile.instance.ParseInt(DateFile.instance.cricketBattleDate[this.ququBattleId][1]);
                    bool flag7 = _baseEnemyId2 == 0;
                    if (flag7)
                    {
                        int num4 = this.actorBodyTyp;
                        if (num4 != 0)
                        {
                            if (num4 == 1)
                            {
                                DateFile.instance.ChangeTwoActorItem(_mianActorId, this.ququBattleEnemyId, this.actorBodyId, 1, -1, 0, 0);
                            }
                        }
                        else
                        {
                            UIDate.instance.ChangeResource(this.ququBattleEnemyId, this.actorBodyId, this.actorBodySize, false);
                            UIDate.instance.ChangeResource(DateFile.instance.MianActorID(), this.actorBodyId, -this.actorBodySize, false);
                        }
                    }
                    else
                    {
                        switch (this.actorBodyTyp)
                        {
                            case 0:
                                UIDate.instance.ChangeResource(DateFile.instance.MianActorID(), this.actorBodyId, -this.actorBodySize, false);
                                break;
                            case 1:
                                DateFile.instance.LoseItem(DateFile.instance.MianActorID(), this.actorBodyId, 1, true, true);
                                break;
                            case 2:
                                DateFile.instance.RemoveActor(new List<int>
                    {
                        this.actorBodyId
                    }, false, true);
                                break;
                        }
                    }
                    string[] _eventDate2 = DateFile.instance.cricketBattleDate[this.ququBattleId][7].Split(new char[]
                    {
                '&'
                    });
                    int _eventId2 = DateFile.instance.ParseInt(_eventDate2[0]);
                    bool flag8 = _eventId2 != 0;
                    if (flag8)
                    {
                        bool flag9 = _eventDate2.Length > 1;
                        if (flag9)
                        {
                            int num5 = DateFile.instance.ParseInt(_eventDate2[1]);
                            if (num5 == 1)
                            {
                                DateFile.instance.SetEvent(new int[]
                                {
                            0,
                            this.ququBattleEnemyId,
                            _eventId2,
                            this.ququBattleEnemyId
                                }, true, true);
                            }
                        }
                        else
                        {
                            DateFile.instance.SetEvent(new int[]
                            {
                        0,
                        -1,
                        _eventId2
                            }, true, true);
                        }
                    }
                    _eventDate2 = null;
                }
                this.closeBattleButton.SetActive(true);
                yield break;
            }

            // Token: 0x060007E7 RID: 2023 RVA: 0x000C9C80 File Offset: 0x000C7E80
            private void UpdateBattleQuquCall()
            {
                for (int i = 0; i < this.actorQuquCall.Length; i++)
                {
                    bool flag = this.actorQuquId[i] >= 0;
                    if (flag)
                    {
                        this.actorQuquCallTime[i] += 1 + Random.Range(0, 5);
                        bool flag2 = this.actorQuquCallTime[i] >= 600 + DateFile.instance.ParseInt(DateFile.instance.GetItemDate(this.actorQuquId[i], 8, true)) * 100;
                        if (flag2)
                        {
                            this.actorQuquCallTime[i] = Random.Range(0, 300);
                            this.actorQuquCall[i].UpdateBattleQuquCall(this.actorQuquId[i]);
                            this.actorQuquCall[i].CallvolumeRest(0.6f, 0.6f);
                        }
                    }
                    else
                    {
                        this.actorQuquCallTime[i] = Random.Range(0, 300);
                    }
                }
                for (int j = 0; j < this.enemyQuquCall.Length; j++)
                {
                    this.enemyQuquCallTime[j] += 1 + Random.Range(0, 5);
                    bool flag3 = this.enemyQuquCallTime[j] >= 600 + DateFile.instance.ParseInt(DateFile.instance.GetItemDate(this.enemyQuquId[j], 8, true)) * 100;
                    if (flag3)
                    {
                        this.enemyQuquCallTime[j] = Random.Range(0, 300);
                        this.enemyQuquCall[j].UpdateBattleQuquCall(this.enemyQuquId[j]);
                        this.enemyQuquCall[j].CallvolumeRest(0.6f, 0.6f);
                    }
                }
            }

            // Token: 0x060007E8 RID: 2024 RVA: 0x0000542C File Offset: 0x0000362C
            private void Awake()
            {
                GuiQuquBattleSystem.instance = this;
            }

            // Token: 0x060007E9 RID: 2025 RVA: 0x000C9E34 File Offset: 0x000C8034
            private void Start()
            {
                this.SetTweener();
                this.ququBattleWindow.SetActive(false);
                this.chooseBodyWindow.sizeDelta = new Vector2(280f, 0f);
                for (int i = 0; i < this.uiText.Length; i++)
                {
                    this.uiText[i].text = DateFile.instance.massageDate[8001][3].Split(new char[]
                    {
                '|'
                    })[i];
                }
                this.battleEndWindow.transform.localPosition = new Vector3(0f, 0f, 0f);
                this.battleEndWindow.transform.localScale = new Vector3(5f, 5f, 1f);
                Component[] componentsInChildren = this.battleEndWindow.GetComponentsInChildren<Component>();
                foreach (Component component in componentsInChildren)
                {
                    bool flag = component is Graphic;
                    if (flag)
                    {
                        (component as Graphic).CrossFadeAlpha(0f, 0f, true);
                    }
                }
                this.battleEndWindow.SetActive(false);
                this.closeBattleButton.SetActive(false);
            }

            // Token: 0x060007EA RID: 2026 RVA: 0x000C9F7C File Offset: 0x000C817C
            private void Update()
            {
                bool flag = this.ququBattlePart == 1;
                if (flag)
                {
                    this.UpdateBattleQuquCall();
                }
            }


            // Token: 0x04000BC1 RID: 3009
            public static GuiQuquBattleSystem instance;

            // Token: 0x04000BC2 RID: 3010
            private int ququBattlePart = 0;

            // Token: 0x04000BC3 RID: 3011
            private int ququBattleTurn = 0;

            // Token: 0x04000BC4 RID: 3012
            public GameObject setBattleSpeedHolder;

            // Token: 0x04000BC5 RID: 3013
            public Toggle[] setBattleSpeedToggle;

            // Token: 0x04000BC6 RID: 3014
            public GameObject ququBattleWindow;

            // Token: 0x04000BC7 RID: 3015
            public Transform actorBack;

            // Token: 0x04000BC8 RID: 3016
            public Transform enemyBack;

            // Token: 0x04000BC9 RID: 3017
            public Button chooseActorButton;

            // Token: 0x04000BCA RID: 3018
            public GameObject[] ququBattleBack;

            // Token: 0x04000BCB RID: 3019
            private bool showQuquBattleWindow = false;

            // Token: 0x04000BCC RID: 3020
            public bool surrender = false;

            // Token: 0x04000BCD RID: 3021
            public int ququBattleEnemyId = 0;

            // Token: 0x04000BCE RID: 3022
            public int ququBattleId = 0;

            // Token: 0x04000BCF RID: 3023
            public int bodyActorId = 0;

            // Token: 0x04000BD0 RID: 3024
            public ActorFace actorFace;

            // Token: 0x04000BD1 RID: 3025
            public ActorFace enemyFace;

            // Token: 0x04000BD2 RID: 3026
            public Text actorNameText;

            // Token: 0x04000BD3 RID: 3027
            public Text enemyNameText;

            // Token: 0x04000BD4 RID: 3028
            public Text[] battleStateText;

            // Token: 0x04000BD5 RID: 3029
            public Transform[] battleValue;

            // Token: 0x04000BD6 RID: 3030
            public Transform[] actorQuqu;

            // Token: 0x04000BD7 RID: 3031
            public Transform[] enemyQuqu;

            // Token: 0x04000BD8 RID: 3032
            public GameObject startBattleWindow;

            // Token: 0x04000BD9 RID: 3033
            public Button startBattleButton;

            // Token: 0x04000BDA RID: 3034
            public Button loseBattleButton;

            // Token: 0x04000BDB RID: 3035
            public Text[] uiText;

            // Token: 0x04000BDC RID: 3036
            public Image actorBodyImage;

            // Token: 0x04000BDD RID: 3037
            public Text actorBodyNameText;

            // Token: 0x04000BDE RID: 3038
            public Image enemyBodyImage;

            // Token: 0x04000BDF RID: 3039
            public Text enemyBodyNameText;

            // Token: 0x04000BE0 RID: 3040
            public Image resourceButtonImage;

            // Token: 0x04000BE1 RID: 3041
            private int enemyBodyTyp;

            // Token: 0x04000BE2 RID: 3042
            private int enemyBodyId;

            // Token: 0x04000BE3 RID: 3043
            private int enemyBodySize;

            // Token: 0x04000BE4 RID: 3044
            private int enemyBodyWorth;

            // Token: 0x04000BE5 RID: 3045
            private int showQuquIndex = 0;

            // Token: 0x04000BE6 RID: 3046
            public GameObject[] hideQuquImage;

            // Token: 0x04000BE7 RID: 3047
            private int[] actorQuquId;

            // Token: 0x04000BE8 RID: 3048
            private int[] enemyQuquId;

            // Token: 0x04000BE9 RID: 3049
            private int[] actorQuquHp;

            // Token: 0x04000BEA RID: 3050
            private int[] enemyQuquHp;

            // Token: 0x04000BEB RID: 3051
            private int[] actorQuquSp;

            // Token: 0x04000BEC RID: 3052
            private int[] enemyQuquSp;

            // Token: 0x04000BED RID: 3053
            public Text[] actorQuquHpText;

            // Token: 0x04000BEE RID: 3054
            public Text[] enemyQuquHpText;

            // Token: 0x04000BEF RID: 3055
            public Image[] actorQuquIcon;

            // Token: 0x04000BF0 RID: 3056
            public Image[] enemyQuquIcon;

            // Token: 0x04000BF1 RID: 3057
            public Text[] actorBattleQuquNameText;

            // Token: 0x04000BF2 RID: 3058
            public Text[] actorBattleQuquPower1Text;

            // Token: 0x04000BF3 RID: 3059
            public Text[] actorBattleQuquPower2Text;

            // Token: 0x04000BF4 RID: 3060
            public Text[] actorBattleQuquPower3Text;

            // Token: 0x04000BF5 RID: 3061
            public Text[] actorBattleQuquStrengthText;

            // Token: 0x04000BF6 RID: 3062
            public Text[] actorBattleQuquMagicText;

            // Token: 0x04000BF7 RID: 3063
            public Image[] actorBattleQuquStrengthBar;

            // Token: 0x04000BF8 RID: 3064
            public Image[] actorBattleQuquMagicBar;

            // Token: 0x04000BF9 RID: 3065
            public Text[] enemyBattleQuquNameText;

            // Token: 0x04000BFA RID: 3066
            public Text[] enemyBattleQuquPower1Text;

            // Token: 0x04000BFB RID: 3067
            public Text[] enemyBattleQuquPower2Text;

            // Token: 0x04000BFC RID: 3068
            public Text[] enemyBattleQuquPower3Text;

            // Token: 0x04000BFD RID: 3069
            public Text[] enemyBattleQuquStrengthText;

            // Token: 0x04000BFE RID: 3070
            public Text[] enemyBattleQuquMagicText;

            // Token: 0x04000BFF RID: 3071
            public Image[] enemyBattleQuquStrengthBar;

            // Token: 0x04000C00 RID: 3072
            public Image[] enemyBattleQuquMagicBar;

            // Token: 0x04000C01 RID: 3073
            public Text[] actorQuquName;

            // Token: 0x04000C02 RID: 3074
            public Text[] enemyQuquName;

            // Token: 0x04000C03 RID: 3075
            private int actorBodyTyp;

            // Token: 0x04000C04 RID: 3076
            private int actorBodyId;

            // Token: 0x04000C05 RID: 3077
            private int actorBodySize;

            // Token: 0x04000C06 RID: 3078
            private bool chooseBodyIsShow = false;

            // Token: 0x04000C07 RID: 3079
            public RectTransform chooseBodyWindow;

            // Token: 0x04000C08 RID: 3080
            public Text needResourceText;

            // Token: 0x04000C09 RID: 3081
            public Button useResourceButton;

            // Token: 0x04000C0A RID: 3082
            public GameObject acotrWindow;

            // Token: 0x04000C0B RID: 3083
            public Button useActorButton;

            // Token: 0x04000C0C RID: 3084
            public Transform actorHolder;

            // Token: 0x04000C0D RID: 3085
            public GameObject actorIcon;

            // Token: 0x04000C0E RID: 3086
            private int setActorBodyId = 0;

            // Token: 0x04000C0F RID: 3087
            private int choseItemIndex;

            // Token: 0x04000C10 RID: 3088
            public bool itemWindowIsShow = false;

            // Token: 0x04000C11 RID: 3089
            public GameObject itemWindow;

            // Token: 0x04000C12 RID: 3090
            public Button useItemButton;

            // Token: 0x04000C13 RID: 3091
            public GameObject itemMask;

            // Token: 0x04000C14 RID: 3092
            public Transform itemHolder;

            // Token: 0x04000C15 RID: 3093
            private int getItemTyp;

            // Token: 0x04000C16 RID: 3094
            private int winTurn = 0;

            // Token: 0x04000C17 RID: 3095
            private const float startDelay = 1f;

            // Token: 0x04000C18 RID: 3096
            public Button[] nextButton;

            // Token: 0x04000C19 RID: 3097
            public GameObject[] nextButtonMask;

            // Token: 0x04000C1A RID: 3098
            private int battleEndTyp = 0;

            // Token: 0x04000C1B RID: 3099
            public Text[] actorQuquDamageText;

            // Token: 0x04000C1C RID: 3100
            public Text[] enemyQuquDamageText;

            // Token: 0x04000C1D RID: 3101
            public GameObject battleEndWindow;

            // Token: 0x04000C1E RID: 3102
            public GameObject closeBattleButton;

            // Token: 0x04000C1F RID: 3103
            public Image battleEndTypImage;

            // Token: 0x04000C20 RID: 3104
            public Text battleEndBodyName;

            // Token: 0x04000C21 RID: 3105
            public Text battleEndBodyText;

            // Token: 0x04000C22 RID: 3106
            public QuquPlace[] actorQuquCall;

            // Token: 0x04000C23 RID: 3107
            public QuquPlace[] enemyQuquCall;

            // Token: 0x04000C24 RID: 3108
            private int[] actorQuquCallTime;

            // Token: 0x04000C25 RID: 3109
            private int[] enemyQuquCallTime;

            // Token: 0x04000C26 RID: 3110
            private const int maxCallSpeed = 600;
        }

    }
}