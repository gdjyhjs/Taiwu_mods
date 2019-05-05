using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GuiQuquAdventure
{
    public class RoomChair : MonoBehaviour
    {
       public bool show_player;
        int desk_id;
        int pos;
        //Transform tImage;
        //GameObject gImage;
        int[] last_image = new int[20];
#if !TAIWU_GAME
        ActorFace actorFace;
#endif
        private void Awake()
        {
            Button btn = GetComponent<Button>();
            btn.onClick.AddListener(OnClickChair);

            //tImage = transform.Find("image");
            //gImage = tImage.gameObject;
#if !TAIWU_GAME
            GameObject go = GameObject.Instantiate(Tools.GetActorFaceSample());
            actorFace = go.GetComponent<ActorFace>();
            Tools.UpdateFace(last_image, actorFace, 0, 0, 0, new int[] { 3102 }, new int[] { 3102 }, 0, true);

            RectTransform tf = (RectTransform)go.transform;
            tf.SetParent(transform, false);
            tf.localScale = Vector3.one * 0.25f;
            tf.anchoredPosition = new Vector2(0, -14);
#endif
        }


        public void SetPlayerShow(bool show, int desk_id, int pos)
        {
            this.desk_id = desk_id;
            this.show_player = show;
            this.pos = pos;
            if (show)
            {
                PlayerData playerData = null;
                PlayerData[] list = DataFile.instance.hall_data.room_data[PlayerData.self.level].player_data;
                for (int i = 0; i < list.Length; i++)
                {
                    if(list[i].desk_idx == desk_id && list[i].level == PlayerData.self.level && list[i].desk_pos == pos)
                    {
                        playerData = list[i];
                        break;
                    }
                }
#if !TAIWU_GAME
                if (null != playerData && playerData.ip != "0")
                {
                    Tools.UpdateFace(last_image,actorFace, playerData.age, playerData.gender, playerData.actorGenderChange, playerData.faceDate, playerData.faceColor, playerData.clotheId, true);
                }
                else
                {
                    Tools.UpdateFace(last_image, actorFace, 0, 0, 0, new int[] { 3102 }, new int[] { 3102 }, 0, true);
                }
#endif
            }
            else
            {
#if !TAIWU_GAME
                // 3102是侠士 3101是外道
                actorFace.UpdateFace(0, 0, 0, 0, new int[] { 3102 }, null, 0);
#endif

            }
        }

        void OnClickChair()
        {
            if (!show_player)
            {
                //QuquDesk.instance.no_operation = 0;
                PlayerData.self.desk_idx = desk_id;
                PlayerData.self.ready = 0;
                PlayerData.self.observer = pos < 2 ? 0 : 1;
                //DataFile.instance.hall_data.room_data[PlayerData.self.level].desk_data[desk_id].chat_data = new List<ChatData>(); // 清空要进入的房间的聊天记录
                QuquHall.instance.GetData(desk_pos: pos);
            }
            else
            {
                YesOrNoWindow.instance.SetYesOrNoWindow(-1, "注意!", "这个位置已经有人了！", false, true);
            }
        }
    }

}