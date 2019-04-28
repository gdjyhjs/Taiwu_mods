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
        float auto_send_interval = 1;
        DataFile dataFile;
        GameObject all;
        GameObject hall;
        GameObject room;
        GameObject desk;
        RoomObj[] rooms;
        RoomDeskObj[] desks;
        Text tCloseBtn;
        Button bCloseBtn;
        Text tCloaseBtn;
        Transform tfBet;
        Image iBet;
        Text tBet;
        Transform[] tfQuqu;
        Image[] iQuqus;
        Text[] tQuqus;
        Text tBattleDeskName;
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

        Image actorBodyImage;
        Text actorBodyNameText;
        Text[] actorQuquName;
        Text[] actorQuquHpText;
        Image[] actorQuquIcon;


        void Awake()
        {
            instance = this;

            PlayerData.self = new PlayerData();
            self = PlayerData.self;
            self.level = -1;
            self.desk_idx = -1;

            dataFile = gameObject.AddComponent<DataFile>();

            all = transform.Find("all").gameObject;
            Image bg = all.GetComponent<Image>();
            Color tmp_c = bg.color;
            tmp_c.a = 1; // 背景不透明
            bg.color = tmp_c;

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

            tCloseBtn = battle_desk.Find("RightButton/Text").GetComponent<Text>();
            bCloseBtn = transform.Find("all/CloseButton").GetComponent<Button>();
            tCloaseBtn = transform.Find("all/CloseButton/Text").GetComponent<Text>();
            tfBet = transform.Find("all/bet");
            iBet = transform.Find("all/bet").GetComponent<Image>();
            iBet.enabled = false;
            tBet = transform.Find("all/bet/Text").GetComponent<Text>();
            tBet.enabled = false;
            iQuqus = new Image[3];
            tQuqus = new Text[3];
            tfQuqu = new Transform[3];
            for (int i = 0; i < 3; i++)
            {
                Transform child = transform.Find("all/ququ" + (i + 1));
                tfQuqu[i] = child;
                iQuqus[i] = child.GetComponent<Image>();
                iQuqus[i].enabled = false;
                tQuqus[i] = child.GetComponentInChildren<Text>();
                tQuqus[i].enabled = false;
                //Button btn = child.GetComponent<Button>();
                //btn.onClick.AddListener(delegate { OnClickQuqu(i); });
            }
            Button bBet = iBet.GetComponent<Button>();
            bBet.onClick.AddListener(OnClickBet);
            bCloseBtn.onClick.AddListener(OnClickClose);

            tBattleDeskName = battle_desk.Find("Text").GetComponent<Text>();
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
            all.SetActive(false);

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

        void OnClickQuqu(int idx)
        {
            SetQuquAndBet();
        }

        void OnClickBet()
        {
            SetQuquAndBet();
        }

        void SetQuquAndBet()
        {
            GuiQuquBattleSystem.instance.actorTyp =  GuiQuquBattleSystem.ActorTyp.LeftObserver;
            GuiQuquBattleSystem.instance.playId = -1;
            GuiQuquBattleSystem.instance.leftPlayer = PlayerData.self;
            GuiQuquBattleSystem.instance.rightPlayer = null;
            OpenQuquBattle();
        }

        void InitBetAndQuqu()
        {
            Main.Logger.Log("初始化赌注和蛐蛐");
            GameObject bet_sample = GuiQuquBattleSystem.instance.transform.Find("BattleActorBack/ActorBattleBodyNameBack/BattleBodyBack/BodyItemBack").gameObject;
            GameObject bet = Instantiate(bet_sample);
            Transform tf_bet = bet.transform;
            actorBodyImage = tf_bet.Find("ActorBodyItem").GetComponent<Image>();
            actorBodyNameText = tf_bet.Find("ItemNameBack/ItemNameText").GetComponent<Text>();
            tf_bet.SetParent(tfBet, false);
            ((RectTransform)tf_bet).anchoredPosition = new Vector2(-1100, 20);
            Vector2[] pos = new Vector2[]
            {
                new Vector2(-200,-10),
                new Vector2(-100,-10),
                new Vector2(0,-10),
            };
            actorQuquName = new Text[3];
            actorQuquHpText = new Text[3];
            actorQuquIcon = new Image[3];
            for (int i = 0; i < 3; i++)
            {
                GameObject ququ_sample = GuiQuquBattleSystem.instance.transform.Find($"QuquBattleBack/QuquBattleHolder/QuquBattle{i + 1}/ActorBattleQuqu{i + 1}").gameObject;
                GameObject ququ = Instantiate(ququ_sample);
                Transform tf_ququ = ququ.transform;
                actorQuquName[i] = tf_ququ.Find($"ActorQuquNameBack{i + 1}/ActorQuquNameText{i + 1}").GetComponent<Text>();
                actorQuquHpText[i] = tf_ququ.Find($"ActorQuqu{i + 1}/ActorQuquHpText{i + 1}").GetComponent<Text>();
                actorQuquIcon[i] = tf_ququ.Find($"ActorQuqu{i + 1}/ActorQuquItem{i + 1}").GetComponent<Image>();
                tf_ququ.SetParent(tfQuqu[i], false);


            }

            UpdateBetAndQuqu();

        }


        public void UpdateBetAndQuqu()
        {
            int bet_typ = PlayerData.self.bet_typ;
            int bet_id = PlayerData.self.bet_id;
            switch (bet_typ)
            {
                case 0:
                    actorBodyImage.tag = "ResourceIcon"; // 金钱
                    actorBodyImage.name = "ResourceIcon," + bet_id;
                    actorBodyImage.sprite = GetSprites.instance.makeResourceIcon[DateFile.instance.ParseInt(DateFile.instance.resourceDate[bet_id][98])];
                    actorBodyNameText.text = DateFile.instance.resourceDate[bet_id][1];
                    break;
                case 1:
                    actorBodyImage.tag = "ActorItem"; // 物品
                    actorBodyImage.name = "ActorItemIcon," + bet_id;
                    actorBodyImage.GetComponent<Image>().sprite = GetSprites.instance.itemSprites[DateFile.instance.ParseInt(DateFile.instance.GetItemDate(bet_id, 98))];
                    actorBodyNameText.text = DateFile.instance.SetColoer(20001 + DateFile.instance.ParseInt(DateFile.instance.GetItemDate(bet_id, 8)), DateFile.instance.GetItemDate(bet_id, 0));
                    break;
                case 2:
                    actorBodyImage.tag = "ShopBootyActor"; // 人
                    actorBodyImage.name = $"ActorIcon,{bet_id},{GuiRandom.Range(0, ActorMenu.instance.GetActorTalk(bet_id).Count)},{-999}";
                    actorBodyImage.GetComponent<Image>().sprite = GetSprites.instance.attSprites[0];
                    actorBodyNameText.text = DateFile.instance.SetColoer(10003, DateFile.instance.GetActorName(bet_id));
                    break;
                default:
                    bet_id = -98;
                    actorBodyImage.tag = "ActorItem";
                    actorBodyImage.name = "ActorItemIcon," + bet_id;
                    actorBodyImage.sprite = GuiQuquBattleSystem.instance.GetItemIcon(bet_id);
                    actorBodyNameText.text = DateFile.instance.massageDate[8001][5].Split('|')[0];
                    break;
            }

            for (int i = 0; i < 3; i++)
            {
                int ququ_id = PlayerData.client_ids[i]; if (ququ_id >= 0)
                {
                    actorQuquName[i].text = DateFile.instance.SetColoer(20001 + DateFile.instance.ParseInt(DateFile.instance.GetItemDate(ququ_id, 8)), DateFile.instance.GetItemDate(ququ_id, 0));
                    int num = DateFile.instance.ParseInt(DateFile.instance.GetItemDate(ququ_id, 901));
                    int num2 = DateFile.instance.ParseInt(DateFile.instance.GetItemDate(ququ_id, 902));
                    actorQuquHpText[i].text = $"{ActorMenu.instance.Color3(num, num2)}{num}</color>/{num2}";
                }
                else
                {
                    actorQuquName[i].text = "";
                    actorQuquHpText[i].text = "";
                }
                actorQuquIcon[i].sprite = ((ququ_id < 0) ? GetSprites.instance.itemSprites[0] : DateFile.instance.GetCricketImage(ququ_id));
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
                all.SetActive(false);
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
            Debug.Log(self.level + " 离开 " + self.desk_idx);
        }

        void OnClickSendChat()
        {
            string content = inputField.text;
            if (string.IsNullOrEmpty(content) && string.IsNullOrWhiteSpace(content))
            {
                Debug.Log("要先输入想说的话！");
            }
            else if (next_chat_time > Time.time)
            {
                Debug.Log("说话太快了！");
            }
            else
            {
                next_chat_time = Time.time + chat_time;
                inputField.text = "";
                content.Replace('|', '｜');
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
                all.SetActive(!all.activeSelf);
            }
        }

        public void GetData(string chat_content = null,string chat_param = null)
        {
            SetSelfData();
            next_auto_send_time = Time.time + auto_send_interval * 2;
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
                dataFile.GetDeskData(OnDeskData, self.name, self.level, self.desk_idx, self.ready, self.observer, self.time_stamp, 0, self.bet, self.ququ, self.GetImage(), chat_content, chat_param);
            }
        }

        void OnHallData(string error, long time_stamp, RoomData[] roomdata)
        {
            next_auto_send_time = Time.time + auto_send_interval*2;
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
                        tCloaseBtn.text = "关闭";
                        tTitle.text = self.name + " 大厅";
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
            }
            else
            {
                Debug.LogError(error);
            }
        }

        void OnRoomData(string error, long time_stamp, RoomData roomdata)
        {
            next_auto_send_time = Time.time + auto_send_interval;
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
                        tCloaseBtn.text = "返回";
                        tTitle.text = self.name + " 大厅>" + RoomObj.level_name[self.level];
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
            }
            else
            {
                Debug.LogError(error);
            }
        }

        void OnDeskData(string error, long time_stamp, DeskData deskData, string battle_flag)
        {
            next_auto_send_time = Time.time + auto_send_interval /2;
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
                        tCloaseBtn.text = "退出";
                        tBattleDeskName.text = RoomObj.level_name[self.level] + ">"+(self.desk_idx + 1) + "号桌";
                        tTitle.text = self.name + " 大厅>" + RoomObj.level_name[self.level] + ">" + (self.desk_idx + 1) + "号桌";
                    }

                    if (battle_flag != "0") // 触发战斗
                    {
                        Debug.Log("触发战斗");
                        self.ready = 0; // 准备战斗恢复为未准备
                        if(self.observer > 1)
                        {
                            self.observer = 1; // 押注观战恢复为普通观战
                        }
                        PlayBattle(battle_flag);
                    }

                    // 聊天
                    ChatData[] chatDatas = deskData.chat_data;
                    ququChat.SetData(chatDatas);

                    // 对战桌
                    ququDesk.SetData(deskData);
                }else if (!all.activeSelf)
                {
                    tPeopleNum.text = "返回比赛";
                }
            }
            else
            {
                self.desk_idx = -1;
                Debug.LogError(error);
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

        string s1 = "";
        string s2 = "";
        string s3 = "true";
        void OnGUI()
        {
            s1 = GUI.TextField(new Rect(10, 60, 40, 125), s1);
            s2 = GUI.TextField(new Rect(10, 110, 40, 125), s2);
            if (GUI.Button(new Rect(10, 10, 40, 25), "对战"))
            {

                // 初始化测试数据
                int battleFlag = 7707090;
                BattleData battleData = new BattleData();
                battleData.battleFlag = s2;
                battleData.time_stamp = 111;
                PlayerData[] playerDatas = new PlayerData[2];
                battleData.player_data = playerDatas;
                for (int i = 0; i < playerDatas.Length; i++)
                {
                    PlayerData data = new PlayerData();
                    playerDatas[i] = data;
                    data.name = "晴华" + i;
                    data.ip = "113.66.219.135";
                    data.observer = 0;
                    data.ququ = new string[] { "101#3001#0｜12", "101#3001#0｜12", "101#3001#0｜12" };
                    data.time_stamp = 1556171873270;
                    data.ready = 0;
                    data.level = -1;
                    data.desk_idx = 0;
                    data.bet = s1;
                    data.SetImage("22,2,0,8,5,21,0,16,41,21,12,18,8,2,9,3,9,5,6,0,4,1");
                }


                if (battleData.player_data != null && battleData.player_data.Length > 1 && GuiQuquBattleSystem.instance.playId != battleData.time_stamp)
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
        }
    }
}