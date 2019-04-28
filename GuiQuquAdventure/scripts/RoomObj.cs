using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GuiQuquAdventure
{
    public class RoomObj : MonoBehaviour
    {
        readonly static string name_format_str = "{0}蛐蛐房\n<size=22> ({1}人在线)</size>";

        private int level = -1;
        private int people_num = -1;
        private bool open = false;
        Text text;

        private void Awake()
        {
            text = gameObject.GetComponentInChildren<Text>();
            Button btn = gameObject.GetComponent<Button>();
            btn.onClick.AddListener(OnClickRoom);
        }

        void OnClickRoom()
        {
            DataFile.instance.hall_data.room_data[this.level].chat_data = new ChatData[0]; // 清空要进入的房间的聊天记录
            PlayerData.self.level = this.level;
            QuquHall.instance.GetData();
            if (QuquChat.instance)
            {
                QuquChat.instance.RestPos();
            }
        }

        private void OnEnable()
        {
            open = true;
            if (level != -1)
            {
                people_num = -1;
                HallData hall_data = DataFile.instance.hall_data;
                if (null != hall_data)
                {
                    RoomData[] roomDatas = hall_data.room_data;
                    if (null != roomDatas)
                    {
                        RoomData roomData = roomDatas[level];
                        SetData(roomData.level, roomData.people_num);
                    }
                }
            }
        }

        private void OnDisable()
        {
            open = false;
        }

        public void SetData(int level, int num)
        {
            if (open && text && (level != this.level || people_num != num) && level > -1 && num > -1)
            {
                text.text = string.Format(name_format_str, GetRoomLevelName(level), num);
            }
            this.level = level;
            this.people_num = num;
        }

        public static string GetRoomLevelName(int level)
        {
            if (level < 1 || level > 9)
            {
                return DateFile.instance.SetColoer(20001, "自由");
            }
            else
            {
                return DateFile.instance.SetColoer(20001 + level, DateFile.instance.massageDate[8001][1].Split('|')[level - 1]);
            }

        }
    }

}