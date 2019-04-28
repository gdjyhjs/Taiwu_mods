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
        int typ;
        private void Awake()
        {
            des = transform.Find("Text").GetComponent<Text>();
            int count = transform.childCount - 1;
            chairs = new RoomChair[count];
            for (int i = 0; i < count; i++)
            {
                chairs[i] = transform.GetChild(i).gameObject.AddComponent<RoomChair>();
            }
        }

        public void SetDataForMark(int desk_idx, int mark)
        {
            Main.Logger.Log("赌桌idx=" + desk_idx);
            for (int i = 0; i < chairs.Length; i++)
            {
                int bit = 1 << i;
                chairs[i].SetPlayerShow((mark & bit) == bit, desk_idx, i);
            }
            typ = (mark & (1 << chairs.Length)) > 0 ? 1 : 0;
            string des = "";
            int desk_level = desk_idx / 10;
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