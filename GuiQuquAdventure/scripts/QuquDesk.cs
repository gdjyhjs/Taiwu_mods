using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GuiQuquAdventure
{

    public class QuquDesk : MonoBehaviour
    {

        //readonly static string[] ready_state_str = new string[]
        //{
        //    "确认",
        //    "准备",
        //    "取消",
        //};


        public static QuquDesk instance;
        /// <summary>
        /// 无操作时间
        /// </summary>
        public float no_operation = 0;
        //float max_no_operation = 300;
        //int last_t;
        DeskData data;
        PlayerObj[] players;
        Image tLeftImage;
        Image tRightImage;
        Button bLeftBtn;
        Button bRightBtn;
        GameObject leftBtn;
        GameObject rightBtn;
        Text tBattleDeskName;
        int my_idx = -1;
        PlayerData self;
        string[] loves;

        QuquPlace[] leftQuquCall = new QuquPlace[3];
        QuquPlace[] rightQuquCall = new QuquPlace[3];
        Text[] leftQuquName = new Text[3];
        Text[] rightQuquName = new Text[3];
        int[] leftQuquCallTime = new int[3];
        int[] rightQuquCallTime = new int[3];
        int[] leftQuquId = new int[3];
        int[] rightQuquId = new int[3];
        private void Awake()
        {
            instance = this;
            self = PlayerData.self;

            tBattleDeskName = transform.Find("Text").GetComponent<Text>();

            Transform player_root = transform.GetChild(0);
            players = new PlayerObj[player_root.childCount];
            for (int i = 0; i < players.Length; i++)
            {
                Transform child = player_root.GetChild(i);
                players[i] = child.gameObject.AddComponent<PlayerObj>();
            }
            Text tLeftBtn = transform.Find("LeftButton/Text").GetComponent<Text>();
            tLeftBtn.text = "";
            Text tRightBtn = transform.Find("RightButton/Text").GetComponent<Text>();
            tRightBtn.text = "";
            bLeftBtn = transform.Find("LeftButton").GetComponent<Button>();
            bRightBtn = transform.Find("RightButton").GetComponent<Button>();
            bLeftBtn.onClick.AddListener(delegate { OnClickReady(0); });
            bRightBtn.onClick.AddListener(delegate { OnClickReady(1); });
            leftBtn = bLeftBtn.gameObject;
            rightBtn = bRightBtn.gameObject;
            tLeftImage = bLeftBtn.GetComponent<Image>();
            tRightImage = bRightBtn.GetComponent<Image>();


            string[] ss = DateFile.instance.massageDate[8001][3].Split('|');
            for (int i = 0; i < 3; i++)
            {
                GameObject ququ_sample = QuquBattleSystem.instance.transform.Find($"QuquBattleBack/QuquBattleHolder/QuquBattle{i + 1}/EnemyBattleQuqu{i + 1}").gameObject;

                GameObject ququ = Instantiate(ququ_sample);
                leftQuquCall[i] = ququ.GetComponent<QuquPlace>();
                RectTransform tf_ququ = (RectTransform)ququ.transform;
                tf_ququ.SetParent(transform, false);
                tf_ququ.anchoredPosition = new Vector2(-850 - 575, -20 - 80 * i);
                tf_ququ.localScale = new Vector3(.45f, .45f, .45f);
                leftQuquName[i] = tf_ququ.Find($"EnemyQuquNameBack{i + 1}/EnemyQuquNameText{i + 1}").GetComponent<Text>();
                Graphic[] gs = ququ.GetComponentsInChildren<Graphic>();
                foreach (var item in gs)
                {
                    item.raycastTarget = false;
                }
                Text[] ts = ququ.GetComponentsInChildren<Text>();
                foreach (var item in ts)
                {
                    if (item.text != ss[i + 2])
                        item.text = "";
                    item.transform.localScale = Vector3.one * 2;
                }

                GameObject ququ2 = Instantiate(ququ_sample);
                rightQuquCall[i] = ququ2.GetComponent<QuquPlace>();
                RectTransform tf_ququ2 = (RectTransform)ququ2.transform;
                tf_ququ2.SetParent(transform, false);
                tf_ququ2.anchoredPosition = new Vector2(-850, -20 - 80 * i);
                tf_ququ2.localScale = new Vector3(.45f, .45f, .45f);
                rightQuquName[i] = tf_ququ2.Find($"EnemyQuquNameBack{i + 1}/EnemyQuquNameText{i + 1}").GetComponent<Text>();
                Graphic[] gs2 = ququ2.GetComponentsInChildren<Graphic>();
                foreach (var item in gs2)
                {
                    item.raycastTarget = false;
                }
                Text[] ts2 = ququ2.GetComponentsInChildren<Text>();
                foreach (var item in ts2)
                {
                    if (item.text != ss[i + 2])
                        item.text = "";
                    item.transform.localScale = Vector3.one * 2;
                }
            }
        }

        public void SetData(DeskData data)
        {
            this.data = data;
            loves = new string[] { "", "" };
            // 玩家
            PlayerData[] playerDats = data.player_data;
            for (int i = 0; i < playerDats.Length; i++)
            {
                if (i < players.Length)
                {
                    PlayerData player = playerDats[i];
                    players[i].SetData(player, i);

                    if (player.ip == self.ip)
                    {
                        my_idx = i;
                    }
                    if (i < 2)
                    {
                        string bet = player.bet;
                        player.SetBetIdAndTyp(i + 1);
                        int bet_typ = player.bet_typ;
                        int bet_id = player.bet_id;
                        QuquHall.instance.SetBetUI(i, bet_typ, bet_id);
                        if(player.ip != "0")
                        {
                            loves[i] = player.GetLoveQuquName();
                        }

                        if(player.ip != "0")
                        {
                            int[] callTime = i == 0 ? leftQuquCallTime : rightQuquCallTime;
                            callTime = new int[3]
                                    {
                            Random.Range(0, 300),
                            Random.Range(0, 300),
                            Random.Range(0, 300)
                                    };
                            int[] ququId = i == 0 ? leftQuquId : rightQuquId;
                            for (int k = 0; k < 3; k++)
                            {
                                ququId[k] = player.GetBattleQuquId(k, -10, 1);
                            }
                        }
                        else
                        {
                            int[] ququId = i == 0 ? leftQuquId : rightQuquId;
                            for (int k = 0; k < 3; k++)
                            {
                                ququId[k] = 0;
                            }
                        }
                    }
                }
            }

            if (self.observer == 0) // 参赛选手
            {
                GameObject showBtn;
                GameObject hideBtn;
                Image tImg;
                if (my_idx == 0)  // 在左边
                {
                    showBtn = leftBtn;
                    hideBtn = rightBtn;
                    tImg = tLeftImage;
                }
                else // 在右边
                {
                    showBtn = rightBtn;
                    hideBtn = leftBtn;
                    tImg = tRightImage;
                }
                hideBtn.SetActive(false);
                showBtn.SetActive(true);
                tImg.sprite = self.ready == 2 ? QuquHall.instance.sprite_x : QuquHall.instance.sprite_o;
                tImg.transform.localScale = self.ready == 0 ? new Vector3(.5f, .5f, .5f) : Vector3.one;
                //tBtn.text = ready_state_str[self.ready];
            }
            else // 观众
            {
                if (self.observer == 2) // 押1号选手
                {
                    leftBtn.SetActive(true);
                    //tLeftBtn.text = "取消押注";
                    tLeftImage.sprite =QuquHall.instance.sprite_x;
                }
                else
                {
                    leftBtn.SetActive(true);
                    PlayerData sel = GetPlayer(0);
                    //string bet_player_name = "押他";
                    //tLeftBtn.text = bet_player_name;
                    tLeftImage.sprite = QuquHall.instance.sprite_o;
                }
                if (self.observer == 3) // 押1号选手
                {
                    rightBtn.SetActive(true);
                    //tRightBtn.text = "取消押注";
                    tRightImage.sprite = QuquHall.instance.sprite_x;
                }
                else
                {
                    rightBtn.SetActive(true);
                    PlayerData sel = GetPlayer(1);
                    //string bet_player_name = "押他";
                    //tRightBtn.text = bet_player_name;
                    tRightImage.sprite = QuquHall.instance.sprite_o;
                }
                tLeftImage.transform.localScale = Vector3.one;
                tRightImage.transform.localScale = Vector3.one;
            }

            UpdateDeskDes(true);
        }

        void OnClickReady(int idx)
        {
            no_operation = 0;
            if (self.observer == 0) // 参赛选手
            {
                if (idx == my_idx)
                {
                    if (self.ready == 0) // 确认赌注
                    {
                        self.ready = 1;
                    }
                    else if (self.ready == 1) // 准备开始
                    {
                        self.ready = 2;
                    }
                    else if (self.ready == 2) // 取消准备
                    {
                        self.ready = 1;
                    }
                    QuquHall.instance.GetData();
                }
            }
            else
            {
                if (self.observer == 2 + idx) // 押注
                {
                    self.observer = 1;
                }
                else
                {
                    self.observer = 2 + idx; // 取消押注
                }
                QuquHall.instance.GetData();
            }
        }

        public PlayerData GetPlayer(int idx)
        {
            return data.player_data[idx];
        }

        void OnEnabled()
        {
            no_operation = 0;
        }

        void Update()
        {
            //if (!GuiQuquBattleSystem.instance.gameObject.activeSelf && self.observer == 0 && self.ready != 2 && players[(my_idx + 1) % 2].show_player)
            //{
            //    no_operation += Time.deltaTime;
            //    if (no_operation > max_no_operation)
            //    {
            //        no_operation = 0;
            //        OnClickReady(my_idx);
            //        UpdateDeskDes();
            //    }
            //}
            //else
            //{
            //    no_operation = 0;
            //    UpdateDeskDes();
            //}


            //ququCall
            //ququCallTime
            //callQuquIdx

            for (int i = 0; i < 3; i++)
            {
                if (leftQuquId[i] != 0)
                {
                    leftQuquCallTime[i] += 1 + Random.Range(0, 5);
                    if (leftQuquCallTime[i] >= 1600 + DateFile.instance.ParseInt(DateFile.instance.GetItemDate(leftQuquId[i], 8)) * 300)
                    {
                        leftQuquCallTime[i] = Random.Range(0, 300);
                        leftQuquCall[i].UpdateBattleQuquCall(leftQuquId[i]);
                        leftQuquCall[i].CallvolumeRest(0.6f, 0.6f);
                    }
                    leftQuquName[i].text = "有只蛐蛐";
                }
                else
                {
                    leftQuquCallTime[i] = Random.Range(0, 300);
                    leftQuquName[i].text = "没有蛐蛐";
                }

                if (rightQuquId[i] != 0)
                {
                    rightQuquCallTime[i] += 1 + Random.Range(0, 5);
                    if (rightQuquCallTime[i] >= 1600 + DateFile.instance.ParseInt(DateFile.instance.GetItemDate(rightQuquId[i], 8)) * 300)
                    {
                        rightQuquCallTime[i] = Random.Range(0, 300);
                        rightQuquCall[i].UpdateBattleQuquCall(rightQuquId[i]);
                        rightQuquCall[i].CallvolumeRest(0.6f, 0.6f);
                    }
                    rightQuquName[i].text = "有只蛐蛐";
                }
                else
                {
                    rightQuquCallTime[i] = Random.Range(0, 300);
                    rightQuquName[i].text = "没有蛐蛐";
                }
            }

        }

        public void UpdateDeskDes(bool force = false)
        {
            //int t = (int)(max_no_operation - no_operation);
            //if (force || last_t != t)
            //{
            //    last_t = t;
            int desk_level = self.desk_idx / 10;
            string desk_des = $"\n\n\n\n{RoomObj.GetRoomLevelName(self.level)}蛐蛐房>{RoomDeskObj.GetDeskLevelName(desk_level)}{(self.desk_idx % 10 + 1)}号桌<size=22><color=red>\n\n{loves[0]}\n\n{loves[1]}</color></size>";
            //    if (QuquDesk.instance.no_operation > 0)
            //    {
            //        switch (self.ready)
            //        {
            //            case 0:
            //                desk_des = $"{desk_des}\n请在{t}秒内确认!";
            //                break;

            //            case 1:
            //                desk_des = $"{desk_des}\n请在{t}秒内准备!";
            //                break;
            //        }
            //    }
            tBattleDeskName.text = desk_des;
            //}
        }
    }
}