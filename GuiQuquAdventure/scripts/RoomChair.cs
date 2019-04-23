using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GuiQuquAdventure
{
    public class RoomChair : MonoBehaviour
    {
        bool show_player;
        int desk_id;
        int pos;
        Transform tImage;
        GameObject gImage;
      #if !TAIWU_GAME
        ActorFace actorFace;
#endif
        private void Awake()
        {
            Button btn = GetComponent<Button>();
            btn.onClick.AddListener(OnClickChair);

            tImage = transform.Find("image");
            gImage = tImage.gameObject;
            #if !TAIWU_GAME
            GameObject go = GameObject.Instantiate(Tools.GetActorFaceSample());
            actorFace = go.GetComponent<ActorFace>();
            go.transform.SetParent(tImage, false);
            go.transform.localScale = Vector3.one * .5f;
#endif
        }

        public void SetPlayerShow(bool show, int desk_id, int pos)
        {
            this.desk_id = desk_id;
            this.show_player = show;
            this.pos = pos;
            gImage.SetActive(show);
            if (show)
            {
                PlayerData playerData = null;
                PlayerData[] list = DataFile.instance.hall_data.room_data[PlayerData.self.level].player_data;
                for (int i = 0; i < list.Length; i++)
                {
                    if(list[i].desk_idx == desk_id)
                    {
                        playerData = list[i];
                        break;
                    }
                }
#if !TAIWU_GAME
                if (null == playerData && playerData.ip != "0")
                {
                        Tools.UpdateFace(actorFace, playerData.age, playerData.gender, playerData.actorGenderChange, playerData.faceDate, playerData.faceColor, playerData.clotheId, false);
                }
                else
                {
                    Tools.UpdateFace(actorFace, playerData.age, playerData.gender, playerData.actorGenderChange, playerData.faceDate, playerData.faceColor, playerData.clotheId, false, true);
                }
#endif
            }
        }

        void OnClickChair()
        {
            PlayerData.self.desk_idx = desk_id;
            PlayerData.self.ready = 0;
            PlayerData.self.observer = pos < 2 ? 0 : 1;
            DataFile.instance.hall_data.room_data[PlayerData.self.level].desk_data[desk_id].chat_data = new ChatData[0]; // 清空要进入的房间的聊天记录
            DataFile.instance.hall_data.room_data[PlayerData.self.level].desk_data[desk_id].battle_data = new BattleData[0]; // 清空要进入的房间的聊天记录
            QuquHall.instance.GetData();
        }
    }

}