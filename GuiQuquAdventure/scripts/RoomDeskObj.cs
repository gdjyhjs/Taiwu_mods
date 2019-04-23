using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GuiQuquAdventure
{
    public class RoomDeskObj : MonoBehaviour
    {
        readonly string des_format_str = "{0}号桌\n<size=22><color=red>{1}</color></size>";
        readonly string[] typ_name = new string[2]
        {
            "对赌桌\n胜利获得对方赌注物品",
            "自赌桌\n胜利获得自己赌注物品",
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
            for (int i = 0; i < chairs.Length; i++)
            {
                int bit = 1 << i;
                chairs[i].SetPlayerShow((mark & bit) == bit, desk_idx, i);
            }
            typ = (mark & (1 << chairs.Length)) > 0 ? 1 : 0;
            des.text = string.Format(des_format_str, (desk_idx + 1), typ_name[typ]);
        }

    }
}