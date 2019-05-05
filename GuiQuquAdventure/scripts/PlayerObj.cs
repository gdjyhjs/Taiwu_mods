using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GuiQuquAdventure
{
    

    public class PlayerObj : MonoBehaviour
    {

        PlayerData data;
        public bool show_player;
        Text text;
        Shadow shadow;
        int pos;
        //Transform tImage;
        //GameObject gImage;
        int[] last_image = new int[20];
#if !TAIWU_GAME
        ActorFace actorFace;
#endif
        readonly static string name_format_str = "{0}\n<size=22>{1}</size>";
        readonly static string[] ready_state_str = new string[]
        {
            "<color=red>未准备</color>",
            "<color=red>确认赌注</color>",
            "<color=red>一切就绪</color>",
            "<color=red>对战中</color>",
            "<color=red>设置中</color>",
        };
        readonly static string[] bet_state_str = new string[]
        {
            "<color=red>未押注</color>",
            "<color=red>未押注</color>",
            "<color=red>押{0}</color>",
            "<color=red>押{0}</color>",
            "<color=red>观战中</color>",
            "<color=red>设置中</color>",
        };


        private void Awake()
        {
            text = transform.Find("Name").GetComponent<Text>();
            shadow = text.GetComponent<Shadow>();
            //tImage = transform.Find("image");
            //gImage = tImage.gameObject;
#if !TAIWU_GAME
            GameObject go = GameObject.Instantiate(Tools.GetActorFaceSample());
            actorFace = go.GetComponent<ActorFace>();

            RectTransform tf = (RectTransform)go.transform;
            tf.SetParent(transform, false);
            tf.localScale = Vector3.one * 0.45f;
            tf.anchoredPosition = new Vector2(0, -80);
#endif
            Button btn = gameObject.GetComponent<Button>();
            btn.onClick.AddListener(OnClickChair);
        }



        void OnClickChair()
        {
            // Main.Logger.Log("点击座位" + (!show_player) + " " + pos);
            if (!show_player)
            {
                //QuquDesk.instance.no_operation = 0;
                //PlayerData.self.desk_idx = desk_id;
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

        //float size = 0.3f;
        //float x = 0;
        //float y = 0;
        //PlayerData p;
        //void OnGUI()
        //{
        //    if (null != p)
        //    {
        //        if (GUI.Button(new Rect(50, 250, 100, 50), "调整PlayerObj"))
        //        {
        //            GameObject go = actorFace.gameObject;
        //            RectTransform tf = (RectTransform)go.transform;
        //            tf.anchoredPosition = new Vector2(x, y);
        //            tf.localScale = Vector3.one * size;
        //        }
        //        float.TryParse(GUI.TextField(new Rect(150, 250, 100, 50), size.ToString()), out size);
        //        float.TryParse(GUI.TextField(new Rect(250, 250, 100, 50), x.ToString()), out x);
        //        float.TryParse(GUI.TextField(new Rect(350, 250, 100, 50), y.ToString()), out y);
        //    }
        //}


        public void SetData(PlayerData playerData,int i)
        {
            pos = i;
            this.data = playerData;
            show_player = playerData.ip != "0";
            if (!show_player) // 空位
            {
                text.text = "虚位以待";
                text.color = Color.gray;
                shadow.effectColor = Color.white;
                actorFace.UpdateFace(0, 0, 0, 0, new int[] { 1 }, null, 0);
            }
            else
            {
                if (playerData.observer==0) // 选手
                {
                    int ready = playerData.ready;
                    if (ready == -1)
                    {
                        ready = 3;
                    }
                    else if (ready == -2)
                    {
                        ready = 4;
                    }
                    text.text = string.Format(name_format_str, playerData.player_name, ready_state_str[ready% ready_state_str.Length]);
                }
                else // 观战者
                {
                    int observer = playerData.observer;
                    if (playerData.ready == -1)
                    {
                        observer = 4;
                    }
                    else if (playerData.ready == -2)
                    {
                        observer = 5;
                    }
                    string bet_player_name = bet_state_str[observer % bet_state_str.Length];
                    if (playerData.observer > 1 && playerData.ready != -1)
                    {
                        PlayerData sel = QuquDesk.instance.GetPlayer(playerData.observer - 2);
                        if (sel.ip != "0")
                        {
                            bet_player_name = string.Format(bet_player_name, sel.player_name);
                        }
                        else
                        {
                            bet_player_name = string.Format(bet_player_name, (playerData.observer == 2 ? "左" : "右") + "边选手");
                        }
                    }
                    text.text = string.Format(name_format_str, playerData.player_name, bet_player_name);
                }
                SetColor(playerData.ip);
            }
#if !TAIWU_GAME
            if(null!= playerData && playerData.ip != "0")
            {
                Tools.UpdateFace(last_image,actorFace, playerData.age, playerData.gender, playerData.actorGenderChange, playerData.faceDate, playerData.faceColor, playerData.clotheId, true);
            }
            else
            {
                Tools.UpdateFace(last_image, actorFace, 0, 0, 0, new int[] { 3102 }, new int[] { 3102 }, 0, true);
            }
#endif
        }

        void SetColor(string ip)
        {
            Color color = Tools.GetColor(ip);
            Color shadowColor = Tools.GetShadowColor(color);
            text.color = shadowColor;
            shadow.effectColor = color;
        }


    }
}