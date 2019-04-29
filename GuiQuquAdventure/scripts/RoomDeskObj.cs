using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GuiQuquAdventure
{
    public class RoomDeskObj : MonoBehaviour
    {
        readonly string des_format_str = "{0}{1}号桌\n<size=22><color=red>{2}\n{3}</color></size>";
        readonly string[] typ_name = new string[2]
        {
            "对赌桌",
            "自赌桌",
        };
        RoomChair[] chairs;
        Text des;
        int desk_id;
        int typ;
        private void Awake()
        {
            des = transform.Find("Text").GetComponent<Text>();
            Button btn = gameObject.AddComponent<Button>();
            btn.onClick.AddListener(OnClickDesk);
            btn.targetGraphic = gameObject.GetComponent<Image>();
            int count = transform.childCount - 1;
            if (RoomChair.si < RoomChair.mi)
                Main.Logger.Log("椅子数量"+ count);
            chairs = new RoomChair[count];
            for (int i = 0; i < count; i++)
            {
                chairs[i] = transform.GetChild(i).gameObject.AddComponent<RoomChair>();
            }
        }

        private void OnClickDesk()
        {
            int observer = -1;
            for (int i = 0; i < chairs.Length; i++)
            {
                var item = chairs[i];
                if (!item.show_player)
                {
                    observer = i < 2 ? 0 : 1;
                    break;
                }
            }
            if (observer != -1)
            {
                QuquDesk.instance.no_operation = 0;
                PlayerData.self.desk_idx = desk_id;
                PlayerData.self.ready = 0;
                PlayerData.self.observer = observer;
                DataFile.instance.hall_data.room_data[PlayerData.self.level].desk_data[desk_id].chat_data = new ChatData[0]; // 清空要进入的房间的聊天记录
                DataFile.instance.hall_data.room_data[PlayerData.self.level].desk_data[desk_id].battle_data = new BattleData[0]; // 清空要进入的房间的聊天记录
                QuquHall.instance.GetData();
            }
            else
            {
                YesOrNoWindow.instance.SetYesOrNoWindow(-1, "注意!", "这张桌子已经坐满人了！", false, true);
            }
        }

        public void SetDataForMark(int desk_idx, int mark)
        {
            this.desk_id = desk_idx;
            if (desk_id < RoomChair.mi)
                Main.Logger.Log("赌桌idx=" + desk_idx);
            for (int i = 0; i < chairs.Length; i++)
            {
                if (desk_id < RoomChair.mi)
                    Main.Logger.Log("赌桌位置=" + i);
                int bit = 1 << i;
                chairs[i].SetPlayerShow((mark & bit) == bit, desk_idx, i);
            }
            typ = (mark & (1 << chairs.Length)) > 0 ? 1 : 0;
            string des = "";
            int desk_level = desk_idx / 10;
            if (desk_id < RoomChair.mi)
                Main.Logger.Log("赌桌等级=" + desk_level);
            switch (PlayerData.self.bet_typ)
            {
                case 0:
                    des = $"每次需要{DeskData.GetRoomNeedResource(PlayerData.self.bet_id, PlayerData.self.bet_id, desk_level)}{DateFile.instance.resourceDate[PlayerData.self.bet_id][1]}!";
                    break;
                case 1:
                    des = $"物品价值至少{DeskData.GetRoomNeedResource(7, PlayerData.self.bet_id, desk_level)}!";
                    break;
                case 2:
                    des = $"人物身价至少{DeskData.GetRoomNeedResource(8, PlayerData.self.bet_id, desk_level)}!";
                    break;
                default:
                    des = $"物品价值至少{DeskData.GetRoomNeedResource(7, PlayerData.self.bet_id, desk_level)}!";
                    break;
            }

            this.des.text = string.Format(des_format_str, GetDeskLevelName(desk_level), desk_idx % 10 + 1, typ_name[typ], des);
        }

        public static string GetDeskLevelName(int desk_level)
        {
            if(desk_level < 1 || desk_level > 9)
            {
                return DateFile.instance.SetColoer(20001, "自由");
            }
            else
            {
                return DateFile.instance.SetColoer(20001 + desk_level, DateFile.instance.massageDate[8001][2].Split('|')[desk_level - 1]);
            }

        }

    }
}