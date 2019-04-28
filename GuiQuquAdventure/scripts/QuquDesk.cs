using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GuiQuquAdventure
{

    public class QuquDesk : MonoBehaviour
    {

        readonly static string[] ready_state_str = new string[]
        {
            "确认",
            "准备",
            "取消",
        };


        public static QuquDesk instance;
        DeskData data;
        PlayerObj[] players;
        Text tLeftBtn;
        Text tRightBtn;
        Button bLeftBtn;
        Button bRightBtn;
        GameObject leftBtn;
        GameObject rightBtn;
        int my_idx = 0;
        PlayerData self;

        private void Awake()
        {
            instance = this;
            self = PlayerData.self;


            Transform player_root = transform.GetChild(0);
            players = new PlayerObj[player_root.childCount];
            for (int i = 0; i < players.Length; i++)
            {
                Transform child = player_root.GetChild(i);
                players[i] = child.gameObject.AddComponent<PlayerObj>();
            }
            tLeftBtn = transform.Find("LeftButton/Text").GetComponent<Text>();
            tRightBtn = transform.Find("RightButton/Text").GetComponent<Text>();
            bLeftBtn = transform.Find("LeftButton").GetComponent<Button>();
            bRightBtn = transform.Find("RightButton").GetComponent<Button>();
            bLeftBtn.onClick.AddListener(delegate { OnClickReady(0); });
            bRightBtn.onClick.AddListener(delegate { OnClickReady(1); });
            leftBtn = bLeftBtn.gameObject;
            rightBtn = bRightBtn.gameObject;
        }

        public void SetData(DeskData data)
        {
            this.data = data;
            // 玩家
            PlayerData[] playerDats = data.player_data;
            for (int i = 0; i < playerDats.Length; i++)
            {
                if (i < players.Length)
                {
                    PlayerData player = playerDats[i];
                    players[i].SetData(player);
                    if(playerDats[i].ip == self.ip)
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
                    }
                }
            }

            if(self.observer == 0) // 参赛选手
            {
                GameObject showBtn;
                GameObject hideBtn;
                Text tBtn;
                if(my_idx == 0)  // 在左边
                {
                    showBtn = leftBtn;
                    hideBtn = rightBtn;
                    tBtn = tLeftBtn;
                }
                else // 在右边
                {
                    showBtn = rightBtn;
                    hideBtn = leftBtn;
                    tBtn = tRightBtn;
                }
                hideBtn.SetActive(false);
                showBtn.SetActive(true);
                tBtn.text = ready_state_str[self.ready];
            }
            else // 观众
            {
                if (self.observer == 2) // 押1号选手
                {
                    leftBtn.SetActive(true);
                    tLeftBtn.text = "取消押注";
                }
                else
                {
                    leftBtn.SetActive(true);
                    PlayerData sel = GetPlayer(0);
                    string bet_player_name = "押他";
                    tLeftBtn.text = bet_player_name;
                }
                if (self.observer == 3) // 押1号选手
                {
                    rightBtn.SetActive(true);
                    tRightBtn.text = "取消押注";
                }
                else
                {
                    rightBtn.SetActive(true);
                    PlayerData sel = GetPlayer(1);
                    string bet_player_name = "押他";
                    tRightBtn.text = bet_player_name;
                }
            }


        }

        void OnClickReady(int idx)
        {
            if (self.observer == 0) // 参赛选手
            {
                if(idx == my_idx)
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
                if(self.observer == 2 + idx) // 押注
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
    }
}