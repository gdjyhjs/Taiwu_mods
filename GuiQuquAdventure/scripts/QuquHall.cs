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
        Image iBet;
        Text tBet;
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


        void Awake()
        {
            instance = this;

            PlayerData.self = new PlayerData();
            self = PlayerData.self;
            self.level = -1;
            self.desk_idx = -1;

            dataFile = gameObject.AddComponent<DataFile>();

            all = transform.Find("all").gameObject;

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
            iBet = transform.Find("all/bet").GetComponent<Image>();
            tBet = transform.Find("all/bet/Text").GetComponent<Text>();
            iQuqus = new Image[3];
            tQuqus = new Text[3];
            for (int i = 0; i < 3; i++)
            {
                Transform child = transform.Find("all/ququ" + (i + 1));
                iQuqus[i] = child.GetComponent<Image>();
                tQuqus[i] = child.GetComponentInChildren<Text>();
                Button btn = child.GetComponent<Button>();
                btn.onClick.AddListener(delegate { OnClickQuqu(i); });
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
            GuiQuquBattleSystem.instance.itemId = -1; // 即将获得或者失去的物品
            GuiQuquBattleSystem.instance.actorTyp =  GuiQuquBattleSystem.ActorTyp.LeftObserver;
            GuiQuquBattleSystem.instance.playId = -1;
            GuiQuquBattleSystem.instance.leftPlayer = PlayerData.self;
            GuiQuquBattleSystem.instance.rightPlayer = null;
            OpenQuquBattle();
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

        void OnDeskData(string error, long time_stamp, DeskData deskData, int battle_flag)
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

                    if (battle_flag != 0) // 触发战斗
                    {
                        Debug.Log("触发战斗");
                        self.ready = 0; // 准备战斗恢复为未准备
                        if(self.observer > 1)
                        {
                            self.observer = 1; // 押注观战恢复为普通观战
                        }
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

        void PlayBattle(int battleFlag)
        {
            BattleData[]  battleDatas = DataFile.instance.hall_data.room_data[self.level].desk_data[self.desk_idx].battle_data;
            if(battleDatas != null && battleDatas.Length > 0)
            {
                BattleData battleData = battleDatas[battleDatas.Length - 1]; // 拿到要播放的战斗数据
                if (battleData.player_data != null && battleData.player_data.Length > 1 && GuiQuquBattleSystem.instance.playId != battleData.time_stamp)
                {
                    GuiQuquBattleSystem.instance.itemId = battleFlag / 10; // 即将获得或者失去的物品
                    GuiQuquBattleSystem.instance.actorTyp = (GuiQuquBattleSystem.ActorTyp)(battleFlag % 10);
                    GuiQuquBattleSystem.instance.playId = battleData.time_stamp;
                    GuiQuquBattleSystem.instance.leftPlayer = battleData.player_data[0];
                    GuiQuquBattleSystem.instance.rightPlayer = battleData.player_data[1];
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

        void OnGUI()
        {
            if(GUI.Button(new Rect(20, 20, 50, 25), "test"))
            {

                // 初始化测试数据
                int battleFlag = 7707090;
                BattleData battleData = new BattleData();
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
                    data.ququ = new int[] { 20 + i, 1 + i, 18 + i };
                    data.time_stamp = 1556171873270;
                    data.ready = 0;
                    data.level = -1;
                    data.desk_idx = 0;
                    data.bet = 0;
                    data.SetImage("22,2,0,8,5,21,0,16,41,21,12,18,8,2,9,3,9,5,6,0,4,1");
                }


                if (battleData.player_data != null && battleData.player_data.Length > 1 && GuiQuquBattleSystem.instance.playId != battleData.time_stamp)
                {
                    GuiQuquBattleSystem.instance.itemId = battleFlag / 10; // 即将获得或者失去的物品
                    GuiQuquBattleSystem.instance.actorTyp = (GuiQuquBattleSystem.ActorTyp)(battleFlag % 10);
                    GuiQuquBattleSystem.instance.playId = battleData.time_stamp;
                    GuiQuquBattleSystem.instance.leftPlayer = battleData.player_data[0];
                    GuiQuquBattleSystem.instance.rightPlayer = battleData.player_data[1];
                    OpenQuquBattle();
                }
            }
        }
    }
}