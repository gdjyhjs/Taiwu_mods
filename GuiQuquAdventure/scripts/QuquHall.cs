using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GuiQuquAdventure
{
    public class QuquHall : MonoBehaviour
    {
        public static QuquHall instance;
        float next_auto_send_time = 0;
        float auto_send_interval = .5f;
        DataFile dataFile;
        GameObject all;
        GameObject hall;
        GameObject room;
        GameObject desk;
        RoomObj[] rooms;
        RoomDeskObj[] desks;
        Text tTitle;
        GameObject chat;
        QuquChat ququChat;
        InputField inputField;
        float next_chat_time;
        float chat_time = .5f;
        //string last_chat = "";
        GameObject people;
        QuquPlayers ququPlayers;
        QuquDesk ququDesk;
        Image mask;
        PlayerData self;
        Text tPeopleNum;

        Image[] actorBodyImage;
        Text[] actorBodyNameText;
        Text[] actorQuquName;
        Text[] actorQuquHpText;
        Image[] actorQuquIcon;

        GameObject go_close;
        GameObject[] go_ququs;
        GameObject[] go_bet;



        void Awake()
        {
            instance = this;

            BattleData.battleDatas = new List<BattleData>();
            PlayerData.self = new PlayerData();
            self = PlayerData.self;
            self.level = -1;
            self.desk_idx = -1;

            dataFile = gameObject.AddComponent<DataFile>();

            all = transform.Find("all").gameObject;
            Image bg = all.GetComponent<Image>();
            bg.color = new Color(1, 1, 1, 1);
            bg.sprite = QuquBattleSystem.instance.GetComponent<Image>().sprite;

            Transform room_parent = transform.Find("all/Hall");
            hall = room_parent.gameObject;
            rooms = new RoomObj[room_parent.childCount];
            for (int i = 0; i < rooms.Length; i++)
            {
                Transform child = room_parent.GetChild(i);
                rooms[i] = child.gameObject.AddComponent<RoomObj>();
                rooms[i].SetData(i, 0);
            }

            Transform desk_parent = transform.Find("all/Room");
            room = desk_parent.gameObject;
            Transform content = desk_parent.GetChild(0).GetChild(0);
            desks = new RoomDeskObj[content.childCount];
            for (int i = 0; i < desks.Length; i++)
            {
                Transform child = content.GetChild(i);
                desks[i] = child.gameObject.AddComponent<RoomDeskObj>();
            }

            Transform battle_desk = transform.Find("all/Desk");
            desk = battle_desk.gameObject;
            ququDesk = desk.AddComponent<QuquDesk>();


            tTitle = transform.Find("all/Text").GetComponent<Text>();

            Transform chat_parent = transform.Find("all/chat");
            chat = chat_parent.gameObject;
            ququChat = chat_parent.GetChild(0).GetChild(0).gameObject.AddComponent<QuquChat>();


            inputField = chat_parent.Find("InputField").GetComponent<InputField>();
            Button sendChatBtn = chat_parent.Find("InputField/SendButton").GetComponent<Button>();
            sendChatBtn.onClick.AddListener(OnClickSendChat);

            Transform people_parent = transform.Find("all/people");
            people = people_parent.gameObject;
            ququPlayers = people_parent.GetChild(0).GetChild(0).gameObject.AddComponent<QuquPlayers>();

            Main.Logger.Log("搬运聊天背景");
            Image chatBg = chat_parent.GetComponent<Image>();
            Image propleBg = people_parent.GetComponent<Image>();
            Image chatBgCp = QuquBattleSystem.instance.transform.Find("BattleActorBack").GetComponent<Image>();
            if (chatBgCp)
            {
                Main.Logger.Log("搬运到聊天背景");
                chatBg.color = chatBgCp.color;
                chatBg.sprite = chatBgCp.sprite;
                propleBg.color = chatBgCp.color;
                propleBg.sprite = chatBgCp.sprite;
            }
            Main.Logger.Log("搬运聊天背景完成 要搬运字体了");
            Text chatText = ququChat.prefab.GetComponentInChildren<Text>();
            Text peopleText = ququPlayers.prefab.GetComponentInChildren<Text>();
            Text cpText = QuquBattleSystem.instance.transform.Find("BattleActorBack/ActorBattleBodyNameBack/BattleBodyBack/BodyItemBack/ItemNameBack/ItemNameText").GetComponent<Text>();
            chatText.font = cpText.font;
            chatText.color = cpText.color;
            peopleText.font = cpText.font;
            chatText.color = cpText.color;
            Shadow shadow = cpText.GetComponent<Shadow>();
            if (shadow)
            {
                Main.Logger.Log("阴影");
                chatText.GetComponent<Shadow>().effectColor = shadow.effectColor;
                peopleText.GetComponent<Shadow>().effectColor = shadow.effectColor;
            }
            Main.Logger.Log("完毕");
            ScrollRect cpScroll = WorldMapSystem.instance.GetComponentInParent<ScrollRect>();
            Scrollbar scrollbar = cpScroll.verticalScrollbar;
            Image handBg = scrollbar.GetComponent<Image>();
            Image handImg = scrollbar.handleRect.GetComponent<Image>();
            Image chatHandBg = chat_parent.Find("Scrollbar Vertical").GetComponent<Image>();
            Image chatHandImg = chat_parent.Find("Scrollbar Vertical/Sliding Area/Handle").GetComponent<Image>();
            Image peopleHandBg = people_parent.Find("Scrollbar Vertical").GetComponent<Image>();
            Image peopleHandImg = people_parent.Find("Scrollbar Vertical/Sliding Area/Handle").GetComponent<Image>();
            chatHandBg.color = handBg.color;
            chatHandBg.sprite = handBg.sprite;
            chatHandImg.color = handImg.color;
            chatHandImg.sprite = handImg.sprite;
            peopleHandBg.color = handBg.color;
            peopleHandBg.sprite = handBg.sprite;
            peopleHandImg.color = handImg.color;
            peopleHandImg.sprite = handImg.sprite;
            Main.Logger.Log("滑动条搬运完毕");






            inputField.characterLimit = 100;

            mask = transform.Find("all/mask").GetComponent<Image>();
            tPeopleNum = transform.Find("people").GetComponent<Text>();

            tTitle.text = self.name+" 大厅";
            tPeopleNum.text = "";
            Button btnPeopleNum = tPeopleNum.GetComponent<Button>();
            btnPeopleNum.onClick.AddListener(delegate { all.SetActive(true); });

            SetSelfData();


            QuquBattleSystem temp = QuquBattleSystem.instance;
            GameObject obj = GameObject.Instantiate(QuquBattleSystem.instance.gameObject);
            QuquBattleSystem tmp = obj.GetComponent<QuquBattleSystem>();
            Object.DestroyImmediate(tmp);
            QuquBattleSystem.instance = temp;
            GuiQuquBattleSystem gui = obj.AddComponent<GuiQuquBattleSystem>();
            gui.Init();
            Transform tf = obj.transform;
            tf.SetParent(temp.transform.parent, false);
            tf.SetSiblingIndex(temp.transform.GetSiblingIndex() + 2);


            hall.SetActive(false);
            room.SetActive(false);
            desk.SetActive(false);
            chat.SetActive(false);
            people.SetActive(false);
            mask.enabled = false;
            //all.SetActive(false);
            SetHallActive(false);

            Text[] texts = transform.GetComponentsInChildren<Text>();
            foreach (var item in texts)
            {
                item.font = DateFile.instance.font;
            }
            InitBetAndQuqu();

            Invoke("SetQuquAndBet", 0.1f);
            Invoke("CloseQuquBattle", 0.4f);
        }

        void SetSelfData()
        {
            int actorId = DateFile.instance.MianActorID();
            self.name = DateFile.instance.GetActorName();
            self.age = DateFile.instance.ParseInt(DateFile.instance.GetActorDate(actorId, 11, addValue: false)); ; // 年龄 茄子代号11
            self.gender = DateFile.instance.ParseInt(DateFile.instance.GetActorDate(actorId, 14, addValue: false)); // 性别 茄子代号14
            self.actorGenderChange = DateFile.instance.ParseInt(DateFile.instance.GetActorDate(actorId, 17, addValue: false)); // 男生女相 1是 其他不是 茄子代号17
            string[] array = DateFile.instance.GetActorDate(actorId, 995, addValue: false).Split('|');
            string[] array2 = DateFile.instance.GetActorDate(actorId, 996, addValue: false).Split('|');
            int[] array3 = new int[array.Length];
            int[] array4 = new int[array2.Length];
            for (int i = 0; i < array.Length; i++)
            {
                array3[i] = DateFile.instance.ParseInt(array[i]);
            }
            for (int j = 0; j < array2.Length; j++)
            {
                array4[j] = DateFile.instance.ParseInt(array2[j]);
            }
            self.faceDate = array3; // 使用的部位 茄子代号995|分割
            self.faceColor = array4; // 部位的颜色 茄子代号996|分割
            int num = DateFile.instance.ParseInt(DateFile.instance.GetActorDate(actorId, 305, addValue: false));
            self.clotheId = (num > 0) ? DateFile.instance.ParseInt(DateFile.instance.GetItemDate(num, 15)) : (-1); // 穿着衣服ID 茄子代号305
        }

        void SetQuquAndBet()
        {
            if (self.desk_idx != -1)
            {
                YesOrNoWindow.instance.SetYesOrNoWindow(-1, "注意!", "对战中不能更换赌注和出战蛐蛐！", false, true);
                return;
            }

            GuiQuquBattleSystem.instance.actorTyp =  GuiQuquBattleSystem.ActorTyp.LeftObserver;
            GuiQuquBattleSystem.instance.playId = -1;
            GuiQuquBattleSystem.instance.leftPlayer = PlayerData.self;
            GuiQuquBattleSystem.instance.rightPlayer = null;
            OpenQuquBattle();
        }

        void InitBetAndQuqu()
        {
            actorBodyImage = new Image[3];
            actorBodyNameText = new Text[3];
            go_ququs = new GameObject[3];
            go_bet = new GameObject[3];
            actorQuquName = new Text[3];
            actorQuquHpText = new Text[3];
            actorQuquIcon = new Image[3];

            if (!DateFile.instance.massageDate.ContainsKey(1556454951))
            {
                DateFile.instance.massageDate.Add(1556454951, new Dictionary<int, string>() { { 0, "设置蛐蛐和押注" }, { 1, "选择符合要求的蛐蛐和押注作为赛注..." } });
                DateFile.instance.massageDate.Add(1556454952, new Dictionary<int, string>() { { 0, "隐藏蛐蛐竞技场" }, { 1, "隐藏蛐蛐竞技场，也可以使用快捷键F1打开或者隐藏..." } });
                DateFile.instance.massageDate.Add(1556454953, new Dictionary<int, string>() { { 0, "退出房间" }, { 1, "退出房间，返回到大厅中，将不计为在线人数..." } });
                DateFile.instance.massageDate.Add(1556454954, new Dictionary<int, string>() { { 0, "退出赌桌" }, { 1, "退出赌桌，可以选择其他赌桌进行游戏，或者设置赌注和出战蛐蛐..." } });
                DateFile.instance.massageDate.Add(1556454955, new Dictionary<int, string>() { { 0, "押注" }, { 1, "设置押注后，斗蛐蛐失败将失去此押注，胜利可获得对方的押注，不同等级的赌桌要求押注的价值不同..." } });
                DateFile.instance.massageDate.Add(1556454956, new Dictionary<int, string>() { { 0, "出战蛐蛐" }, { 1, "设置押注后，斗蛐蛐失败将失去此押注，胜利可获得对方的押注，不同等级的房间要求蛐蛐不能超过的等级不同..." } });
            }

            Main.Logger.Log("初始化赌注和蛐蛐");

            GameObject bet_sample = GuiQuquBattleSystem.instance.transform.Find("BattleActorBack/ActorBattleBodyNameBack/BattleBodyBack/BodyItemBack").gameObject;
            GameObject bet = Instantiate(bet_sample);
            go_bet[2] = bet;
            Transform tf_bet = bet.transform;
            actorBodyImage[2] = tf_bet.Find("ActorBodyItem").GetComponent<Image>();
            actorBodyNameText[2] = tf_bet.Find("ItemNameBack/ItemNameText").GetComponent<Text>();
            tf_bet.SetParent(transform.Find("all/set"), false);
            ((RectTransform)tf_bet).anchoredPosition = new Vector2(1530, -80);
            Main.Logger.Log("初始化赌注和蛐蛐");

            Vector2[] pos = new Vector2[]
            {
                new Vector2(25,-25),
                new Vector2(175,-75),
                new Vector2(325,175),

            };
            Main.Logger.Log("初始化赌注和蛐蛐");
            for (int i = 0; i < 3; i++)
            {
                Main.Logger.Log("初始化赌注和蛐蛐"+i);
                GameObject ququ_sample = GuiQuquBattleSystem.instance.transform.Find($"QuquBattleBack/QuquBattleHolder/QuquBattle{i + 1}/ActorBattleQuqu{i + 1}").gameObject;
                GameObject ququ = Instantiate(ququ_sample);
                go_ququs[i] = ququ;
                ququ.name = ququ_sample.name;
                Transform tf_ququ = ququ.transform;
                actorQuquName[i] = tf_ququ.Find($"ActorQuquNameBack{i + 1}/ActorQuquNameText{i + 1}").GetComponent<Text>();
                actorQuquHpText[i] = tf_ququ.Find($"ActorQuqu{i + 1}/ActorQuquHpText{i + 1}").GetComponent<Text>();
                actorQuquIcon[i] = tf_ququ.Find($"ActorQuqu{i + 1}/ActorQuquItem{i + 1}").GetComponent<Image>();
                tf_ququ.SetParent(transform.Find("all/ququ" + (i + 1)), false);
                ((RectTransform)tf_ququ).anchoredPosition = pos[i];
                Main.Logger.Log("初始化赌注和蛐蛐"+i);
                Text[] ts = ququ.GetComponentsInChildren<Text>();
                string[] ss = DateFile.instance.massageDate[8001][3].Split('|');
                foreach (var item in ts)
                {
                    if (item.text != ss[2] && item.text != ss[3] && item.text != ss[4])
                        item.text = "";
                }
                Main.Logger.Log("初始化赌注和蛐蛐"+i);
            }


            Main.Logger.Log("初始化赌注和蛐蛐");

            // 关闭按钮
            go_close = Instantiate(GuiQuquBattleSystem.instance.loseBattleButton.gameObject);
            //go_close.name = GuiQuquBattleSystem.instance.loseBattleButton.name;
            go_close.name = "1556454952,1556454952";
            go_close.SetActive(true);
            Transform tf_close = go_close.transform;
            go_close.GetComponent<Button>().onClick.AddListener(OnClickClose);
            tf_close.SetParent(transform.Find("all/close"), false);
            ((RectTransform)tf_close).anchoredPosition = new Vector2(20, -20);

            Main.Logger.Log("初始化赌注和蛐蛐");
            // 设置按钮
            GameObject go_set = Instantiate(GuiQuquBattleSystem.instance.startBattleButton.gameObject);
            //go_set.name = GuiQuquBattleSystem.instance.startBattleButton.name;
            go_set.name = "1556454951,1556454951";
            go_set.SetActive(true);
            Transform tf_set = go_set.transform;
            go_set.GetComponent<Button>().onClick.AddListener(SetQuquAndBet);
            tf_set.SetParent(transform.Find("all/set"), false);
            ((RectTransform)tf_set).anchoredPosition = new Vector2(1400, 20);

            Main.Logger.Log("初始化赌注和蛐蛐");

            GameObject left_bet = Instantiate(bet_sample);
            go_bet[0] = left_bet;
            Transform tf_left_bet = left_bet.transform;
            actorBodyImage[0] = tf_left_bet.Find("ActorBodyItem").GetComponent<Image>();
            actorBodyNameText[0] = tf_left_bet.Find("ItemNameBack/ItemNameText").GetComponent<Text>();
            tf_left_bet.SetParent(transform.Find("all/Desk/left_bet"), false);
            ((RectTransform)tf_left_bet).anchoredPosition = new Vector2(250, -100);

            Main.Logger.Log("初始化赌注和蛐蛐");

            GameObject right_bet = Instantiate(bet_sample);
            go_bet[1] = right_bet;
            Transform tf_right_bet = right_bet.transform;
            actorBodyImage[1] = tf_right_bet.Find("ActorBodyItem").GetComponent<Image>();
            actorBodyNameText[1] = tf_right_bet.Find("ItemNameBack/ItemNameText").GetComponent<Text>();
            tf_right_bet.SetParent(transform.Find("all/Desk/right_bet"), false);
            ((RectTransform)tf_right_bet).anchoredPosition = new Vector2(-250, -100);

            Main.Logger.Log("初始化赌注和蛐蛐");

            UpdateBetAndQuqu();
        }


        public void UpdateBetAndQuqu()
        {
            Main.Logger.Log("更新赌注和蛐蛐");

            int bet_typ = PlayerData.self.bet_typ;
            int bet_id = PlayerData.self.bet_id;
            Main.Logger.Log("更新赌注和蛐蛐");
            SetBetUI(2, bet_typ, bet_id);
            Main.Logger.Log("更新赌注和蛐蛐");

            for (int i = 0; i < 3; i++)
            {
                Main.Logger.Log("更新赌注和蛐蛐"+i);
                int ququ_id = PlayerData.client_ids[i];
                actorQuquIcon[i].name = "ActorQuqu," + ququ_id;
                if (ququ_id > 0)
                {
                    actorQuquName[i].tag = "ActorItem";
                    actorQuquName[i].text = DateFile.instance.SetColoer(20001 + DateFile.instance.ParseInt(DateFile.instance.GetItemDate(ququ_id, 8)), DateFile.instance.GetItemDate(ququ_id, 0));
                    int num = DateFile.instance.ParseInt(DateFile.instance.GetItemDate(ququ_id, 901));
                    int num2 = DateFile.instance.ParseInt(DateFile.instance.GetItemDate(ququ_id, 902));
                    actorQuquHpText[i].text = $"{ActorMenu.instance.Color3(num, num2)}{num}</color>/{num2}";
                }
                else
                {
                    actorQuquName[i].tag = go_close.tag;
                    actorQuquName[i].text = "无出战蛐蛐";
                    actorQuquHpText[i].text = "";
                    actorQuquIcon[i].name = "1556454956,1556454956";
                }
                actorQuquIcon[i].sprite = ((ququ_id < 0) ? GetSprites.instance.itemSprites[0] : DateFile.instance.GetCricketImage(ququ_id));

                Main.Logger.Log("更新赌注和蛐蛐" + i);
            }
        }

        public void SetBetUI(int idx,int bet_typ,int bet_id)
        {
            Main.Logger.Log("设置赌注ui");
            switch (bet_typ)
            {
                case 0:
                    actorBodyImage[idx].tag = "ResourceIcon"; // 金钱
                    actorBodyImage[idx].name = "ResourceIcon," + bet_id;
                    actorBodyImage[idx].sprite = GetSprites.instance.makeResourceIcon[DateFile.instance.ParseInt(DateFile.instance.resourceDate[bet_id][98])];
                    actorBodyNameText[idx].text = DateFile.instance.resourceDate[bet_id][1];
                    break;
                case 1:
                    actorBodyImage[idx].tag = "ActorItem"; // 物品
                    actorBodyImage[idx].name = "ActorItemIcon," + bet_id;
                    actorBodyImage[idx].GetComponent<Image>().sprite = GetSprites.instance.itemSprites[DateFile.instance.ParseInt(DateFile.instance.GetItemDate(bet_id, 98))];
                    actorBodyNameText[idx].text = DateFile.instance.SetColoer(20001 + DateFile.instance.ParseInt(DateFile.instance.GetItemDate(bet_id, 8)), DateFile.instance.GetItemDate(bet_id, 0));
                    break;
                case 2:
                    actorBodyImage[idx].tag = "ShopBootyActor"; // 人
                    actorBodyImage[idx].name = $"ActorIcon,{bet_id},{GuiRandom.Range(0, ActorMenu.instance.GetActorTalk(bet_id).Count)},{-999}";
                    actorBodyImage[idx].GetComponent<Image>().sprite = GetSprites.instance.attSprites[0];
                    actorBodyNameText[idx].text = DateFile.instance.SetColoer(10003, DateFile.instance.GetActorName(bet_id));
                    break;
                default:
                    bet_id = -98;
                    actorBodyImage[idx].tag = go_close.tag;
                    actorBodyImage[idx].name = "1556454955,1556454955";
                    actorBodyImage[idx].sprite = GuiQuquBattleSystem.instance.GetItemIcon(bet_id);
                    actorBodyNameText[idx].text = DateFile.instance.massageDate[8001][5].Split('|')[0];
                    break;
            }
        }

        void SetPeopleNum()
        {
            if(null!= DataFile.instance.hall_data)
            {
                RoomData[] roomDatas = DataFile.instance.hall_data.room_data;
                if (null != roomDatas)
                {
                    int num = 0;
                    for (int i = 0; i < roomDatas.Length; i++)
                    {
                        num += roomDatas[i].people_num;
                    }
                    tPeopleNum.text = num + "人在线";
                }
            }
        }

        void OnClickClose()
        {
            if (self.desk_idx == -1 && self.level == -1) // 在大厅中
            {
                SetHallActive(false);
            }
            else if (self.desk_idx == -1 && self.level != -1) // 在房间中
            {
                self.level = -1;
                GetData();
            }
            else if (self.desk_idx != -1) // 在桌子中
            {
                self.ready = 0;
                self.observer = 0;
                self.desk_idx = -1;
                GetData();
            }
            //Debug.Log(self.level + " 离开 " + self.desk_idx);
        }

        void OnClickSendChat()
        {
            string content = inputField.text;
            if (string.IsNullOrEmpty(content) && string.IsNullOrWhiteSpace(content))
            {
                YesOrNoWindow.instance.SetYesOrNoWindow(-1, "注意!", "要先输入想说的话！", false, true);
            }
            else if (next_chat_time > Time.time)
            {
                YesOrNoWindow.instance.SetYesOrNoWindow(-1, "注意!", "说话太快了！", false, true);
            }
            else
            {
                next_chat_time = Time.time + chat_time;
                inputField.text = "";
                content = content.Replace('|', '｜');
                GetData(content, "");
            }
        }

        void Update()
        {
            if (Time.time > next_auto_send_time)
            {
                GetData();
            }
            if (Input.GetKeyDown(KeyCode.KeypadEnter)|| Input.GetKeyDown(KeyCode.Return))
            {
                if (EventSystem.current.currentSelectedGameObject == inputField.gameObject)
                {
                    // 选中输入框则发送信息
                    OnClickSendChat();
                }
                inputField.ActivateInputField();
            }
            if (Input.GetKeyDown(KeyCode.F1))
            {
                SetHallActive(!all.activeSelf);
            }
        }

        public void GetData(string chat_content = null,string chat_param = null)
        {
            SetSelfData();
            next_auto_send_time = Time.time + auto_send_interval*10;
            if (self.desk_idx == -1 && self.level == -1) // 获取大厅数据
            {
                dataFile.GetHallData(OnHallData, self.name, self.GetImage());
            }
            else if(self.desk_idx == -1 && self.level != -1) // 获取房间数据
            {
                dataFile.GetRoomData(OnRoomData, self.name, self.level, self.time_stamp, self.GetImage(), chat_content, chat_param);
            }
            else if (self.desk_idx != -1) // 获取桌子数据
            {
                if (self.observer == 0)
                {
                    if (!PlayerData.CheckQuqu(self.level))
                    {
                        YesOrNoWindow.instance.SetYesOrNoWindow(-1, "出战蛐蛐太强!", $"当前房间不允许出战蛐蛐超过{DateFile.instance.massageDate[8001][1].Split('|')[self.level - 1]},您可以更换出战蛐蛐或者选择旁观!", false, true);
                        self.desk_idx = -1;
                        return;
                    }
                    if (!PlayerData.ChechBet(true))
                    {
                        return;
                    }
                }
                else if (self.observer == 2 || self.observer == 3)
                {
                    if(PlayerData.client_bet < 0)
                    {
                        YesOrNoWindow.instance.SetYesOrNoWindow(-1, "没有赌注!", "请先准备好赌注再押,不能空手套白狼噢!", false, true);
                        self.observer = 1;
                        return;
                    }
                }
                Main.Logger.Log("获取桌子数据 name=" + self.name + " level=" + self.level + " desk_idx=" + self.desk_idx + " ready=" + self.ready + " observer=" + self.observer + " time_stamp=" + self.time_stamp + " bet=" + self.bet + " ququ=" + self.ququ[0] + "," + self.ququ[1] + "," + self.ququ[2]);
                dataFile.GetDeskData(OnDeskData, self.name, self.level, self.desk_idx, self.ready, self.observer, self.time_stamp, self.time_stamp, self.bet, self.ququ, self.GetImage(), chat_content, chat_param);
            }
        }

        void OnHallData(string error, long time_stamp, RoomData[] roomdata)
        {
            next_auto_send_time = Time.time + auto_send_interval;
            if (null == error)
            {
                inputField.text = "";
                self.time_stamp = time_stamp;
                if (all.activeSelf && self.desk_idx == -1 && self.level == -1) // 在大厅中
                {
                    if (!hall.gameObject.activeSelf)
                    {
                        this.hall.SetActive(true);
                        this.room.SetActive(false);
                        this.desk.SetActive(false);
                        this.chat.SetActive(false);
                        this.people.SetActive(false);
                        tTitle.text = self.name + " 大厅";
                        go_close.name = "1556454952,1556454952";
                    }
                    for (int i = 0; i < roomdata.Length; i++)
                    {
                        if (rooms.Length > i)
                        {
                            RoomData room_data = roomdata[i];
                            RoomObj room = rooms[i];
                            room.SetData(room_data.level, room_data.people_num);
                        }
                    }
                }else if (!all.activeSelf)
                {
                    SetPeopleNum();
                }
                UpdateBetAndQuqu();
            }
            else
            {
                YesOrNoWindow.instance.SetYesOrNoWindow(-1, "出错!", error, false, true);
            }
        }

        void OnRoomData(string error, long time_stamp, RoomData roomdata)
        {
            next_auto_send_time = Time.time + auto_send_interval*2;
            if (null == error)
            {
                tPeopleNum.text = "";
                self.time_stamp = time_stamp;
                if (all.activeSelf && self.desk_idx == -1 && self.level != -1)
                {
                    if (!room.gameObject.activeSelf)
                    {
                        this.hall.SetActive(false);
                        this.room.SetActive(true);
                        this.desk.SetActive(false);
                        this.chat.SetActive(true);
                        this.people.SetActive(true);
                        tTitle.text = $"{self.name} 大厅>{RoomObj.GetRoomLevelName(self.level)}蛐蛐房>";
                        go_close.name = "1556454953,1556454953";
                    }
                    for (int i = 0; i < roomdata.desk_mark.Length; i++)
                    {
                        if (desks.Length > i)
                        {
                            int mark = roomdata.desk_mark[i];
                            RoomDeskObj desk = desks[i];
                            desk.SetDataForMark(i, mark);
                        }
                    }

                    ChatData[] chatDatas = roomdata.chat_data;
                    ququChat.SetData(chatDatas);

                    PlayerData[] playerDats = roomdata.player_data;
                    ququPlayers.SetData(playerDats);
                }else if (!all.activeSelf)
                {
                    tPeopleNum.text = "返回房间";
                }
                UpdateBetAndQuqu();
            }
            else
            {
                YesOrNoWindow.instance.SetYesOrNoWindow(-1, "出错!", error, false, true);
            }
        }

        void OnDeskData(string error, long time_stamp, DeskData deskData, string battle_flag)
        {
            next_auto_send_time = Time.time + auto_send_interval;
            if (null == error)
            {
                tPeopleNum.text = "";
                self.time_stamp = time_stamp;
                if (all.activeSelf && self.desk_idx != -1)
                {
                    if (!desk.gameObject.activeSelf)
                    {
                        this.hall.SetActive(false);
                        this.room.SetActive(false);
                        this.desk.SetActive(true);
                        this.chat.SetActive(true);
                        this.people.SetActive(false);
                        int desk_level = self.desk_idx / 10;
                        tTitle.text = $"{self.name} 大厅>{RoomObj.GetRoomLevelName(self.level)}蛐蛐房>{(self.desk_idx + 1)}{RoomDeskObj.GetDeskLevelName(desk_level)}{(self.desk_idx % 10 + 1)}号桌";
                        go_close.name = "1556454954,1556454954";
                        QuquDesk.instance.UpdateDeskDes(true);
                    }

                    // 聊天
                    ChatData[] chatDatas = deskData.chat_data;
                    ququChat.SetData(chatDatas);

                    // 对战桌
                    ququDesk.SetData(deskData);



                    if (!int.TryParse(battle_flag, out int tmp)) // 触发战斗
                    {
                        Main.Logger.Log("触发战斗");
                        self.ready = 0; // 准备战斗恢复为未准备
                        if (self.observer > 1)
                        {
                            self.observer = 1; // 押注观战恢复为普通观战
                        }
                        PlayBattle(battle_flag);
                    }
                    else
                    {
                        Main.Logger.Log("不触发战斗");
                    }


                }
                else if (!all.activeSelf)
                {
                    tPeopleNum.text = "返回赌桌";
                }
                UpdateBetAndQuqu();
                //OnLose();
            }
            else
            {
                self.desk_idx = -1;
                YesOrNoWindow.instance.SetYesOrNoWindow(-1, "出错!", error, false, true);
                GetData();
            }
        }

        void PlayBattle(string battleFlag)
        {
            BattleData[]  battleDatas = DataFile.instance.hall_data.room_data[self.level].desk_data[self.desk_idx].battle_data;
            if(battleDatas != null && battleDatas.Length > 0)
            {
                BattleData battleData = battleDatas[battleDatas.Length - 1]; // 拿到要播放的战斗数据
                if (battleData.player_data != null && battleData.player_data.Length > 1 && GuiQuquBattleSystem.instance.playId != battleData.time_stamp)
                {
                    string[] ss = battleFlag.Split('｜');
                    battleData.battleFlag = battleFlag;
                    GuiQuquBattleSystem.instance.observer_battleFlag = battleFlag;
                    GuiQuquBattleSystem.instance.actorTyp = (GuiQuquBattleSystem.ActorTyp)int.Parse(ss[ss.Length - 3]);
                    GuiQuquBattleSystem.instance.playId = battleData.time_stamp;
                    GuiQuquBattleSystem.instance.leftPlayer = battleData.player_data[0];
                    GuiQuquBattleSystem.instance.rightPlayer = battleData.player_data[1];
                    GuiQuquBattleSystem.instance.replay = false;
                    OpenQuquBattle();
                }
            }
        }

        void OpenQuquBattle()
        {
            //int num = 123456;
            //GuiQuquBattleSystem.instance.ququBattleEnemyId = num;
            //GuiQuquBattleSystem.instance.ququBattleId = 1; // 此处是根据身份来确定蛐蛐战斗ID 那么大家都是太吾就不需要了 20是拿身份 Mathf.Clamp(Mathf.Abs(DateFile.instance.ParseInt(DateFile.instance.GetActorDate(num, 20, false))) + Random.Range(-1, 2), 1, 9);
            //bool flag = Random.Range(0, 100) < 20;
            //if (flag)
            //{
            //    GuiQuquBattleSystem.instance.ququBattleId += 10;
            //}
            GuiQuquBattleSystem.instance.ShowQuquBattleWindow();
        }

        void CloseQuquBattle()
        {
            GuiQuquBattleSystem.instance.CloseQuquBattleWindow();
        }

        string s1 = "1｜650002｜999#650002#902#4#901#3#33#1*1*1*1*1*1*1*1*1*0｜";
        string s2 = "1｜650002｜999#650002#902#4#901#3#33#1*1*1*1*1*1*1*1*1*0｜｜9｜0｜1";
        string s3 = "true";
        string s4 = "111";
        string[] s = new string[] { "105#0#0*11｜203#3004#0｜232#3015#0", "232#3015#0｜105#0#0*11｜203#3004#0" };
        void OnGUI()
        {
            s1 = GUI.TextField(new Rect(10, 60, 140, 25), s1);
            s2 = GUI.TextField(new Rect(10, 110, 140, 25), s2);
            s3 = GUI.TextField(new Rect(10, 160, 40, 25), s3);
            s4 = GUI.TextField(new Rect(10, 210, 40, 25), s4);
            s[0] = GUI.TextField(new Rect(10, 260, 140, 25), s[0]);
            s[1] = GUI.TextField(new Rect(10, 310, 140, 25), s[1]);
            if (GUI.Button(new Rect(10, 10, 40, 25), "对战"))
            {

                // 初始化测试数据
                BattleData battleData = new BattleData();
                battleData.battleFlag = s2;
                battleData.time_stamp = long.Parse(s4);
                PlayerData[] playerDatas = new PlayerData[2];
                battleData.player_data = playerDatas;
                for (int i = 0; i < playerDatas.Length; i++)
                {
                    PlayerData data = new PlayerData();
                    playerDatas[i] = data;
                    data.name = "晴华" + i;
                    data.ip = "113.66.219.135";
                    data.observer = 0;
                    data.ququ = s[i].Split('｜');
                    data.time_stamp = 1556171873270;
                    data.ready = 0;
                    data.level = -1;
                    data.desk_idx = 0;
                    data.bet = s1;
                    data.SetImage("22,2,0,8,5,21,0,16,41,21,12,18,8,2,9,3,9,5,6,0,4,1");
                }


                if (battleData.player_data != null && battleData.player_data.Length > 1)
                {
                    string[] ss = battleData.battleFlag.Split('｜');
                    GuiQuquBattleSystem.instance.actorTyp = (GuiQuquBattleSystem.ActorTyp)int.Parse(ss[ss.Length - 3]);
                    GuiQuquBattleSystem.instance.observer_battleFlag = battleData.battleFlag;
                    GuiQuquBattleSystem.instance.playId = battleData.time_stamp;
                    GuiQuquBattleSystem.instance.leftPlayer = battleData.player_data[0];
                    GuiQuquBattleSystem.instance.rightPlayer = battleData.player_data[1];
                    GuiQuquBattleSystem.instance.replay = bool.Parse(s3);
                    OpenQuquBattle();
                }
            }

            if (GUI.Button(new Rect(60 + 60 * 0, 10, 40, 25), "设置"))
            {
                SetQuquAndBet();
            }
            if (GUI.Button(new Rect(60 + 60 * 1, 10, 40, 25), "返回"))
            {
                OnClickClose();
            }
        }

        public void OnLose()
        {
            bool has = false;
            List<int> list = new List<int>(ActorMenu.instance.GetActorItems(DateFile.instance.MianActorID()).Keys); // 获取玩家背包物品
            for (int i = 0; i < PlayerData.client_ids.Length; i++)
            {
                int id = PlayerData.client_ids[i];
                has = false;
                for (int j = 0; j < list.Count; j++)
                {
                    if (list[j] == id)
                    {
                        has = true;
                        break;
                    }
                }
                if (!has)
                {
                    PlayerData.client_ids[i] = -98;
                }
            }
            has = false;
            for (int k = 0; k < list.Count; k++)
            {
                int id = PlayerData.client_bet;
                if (list[k] == id)
                {
                    has = true;
                    break;
                }
            }
            if (!has)
            {
                PlayerData.client_bet = -98;
            }
            SetQuquAndBet();
            if (!PlayerData.ChechBet(false) && self.observer == 0)
            {
                YesOrNoWindow.instance.SetYesOrNoWindow(-1, "失去赌注或蛐蛐!", "由于您失去了赌注或蛐蛐!已将您踢出了比赛,请重新选择赌注参赛吧!", false, true);
                self.desk_idx = -1;
                GetData();
            }
        }

        void SetHallActive(bool show)
        {
            all.SetActive(show);
            if (!show)
                UIMove.instance.CloseGUI();
            else
                UIMove.instance.ShowGUI();
        }
    }
}