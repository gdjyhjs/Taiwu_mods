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

            RectTransform tf = (RectTransform)go.transform;
            tf.SetParent(tImage, false);
            tf.localScale = Vector3.one * 0.1f;
            tf.anchoredPosition = new Vector2(0, -14);
#endif
        }


        float size = 0.3f;
        float x = 0;
        float y = 0;
        PlayerData p;
        void OnGUI()
        {
            if (null != p && p.ip != "0")
            {
                if (GUI.Button(new Rect(50, 150, 100, 50), "调整RoomChair"))
                {
                    GameObject go = actorFace.gameObject;
                    RectTransform tf = (RectTransform)go.transform;
                    tf.anchoredPosition = new Vector2(x, y);
                    tf.localScale = Vector3.one * size;
                }
                float.TryParse(GUI.TextField(new Rect(150, 150, 100, 50), size.ToString()), out size);
                float.TryParse(GUI.TextField(new Rect(250, 150, 100, 50), x.ToString()), out x);
                float.TryParse(GUI.TextField(new Rect(350, 150, 100, 50), y.ToString()), out y);
            }
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
                    if(list[i].desk_idx == desk_id)
                    {
                        playerData = list[i];
                        break;
                    }
                }
#if !TAIWU_GAME
                p = playerData;
                if (null != playerData && playerData.ip != "0")
                {
                        Tools.UpdateFace(actorFace, playerData.age, playerData.gender, playerData.actorGenderChange, playerData.faceDate, playerData.faceColor, playerData.clotheId, true);
                }
                else
                {
                    //Tools.UpdateFace(actorFace, playerData.age, playerData.gender, playerData.actorGenderChange, playerData.faceDate, playerData.faceColor, playerData.clotheId, false, true);
                }
#endif
            }
            else
            {
#if !TAIWU_GAME
                actorFace.UpdateFace(0, 0, 0, 0, new int[] { 0 }, null, 0);
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