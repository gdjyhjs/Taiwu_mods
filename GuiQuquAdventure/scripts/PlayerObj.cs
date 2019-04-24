using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GuiQuquAdventure
{
    

    public class PlayerObj : MonoBehaviour
    {

        PlayerData data;
        Text text;
        Shadow shadow;
        Transform tImage;
        GameObject gImage;
#if !TAIWU_GAME
        ActorFace actorFace;
#endif
        bool show = false;
        readonly static string name_format_str = "{0}\n<size=22>{1}</size>";
        readonly static string[] ready_state_str = new string[]
        {
            "<color=red>未准备</color>",
            "<color=red>确认赌注</color>",
            "<color=red>一切就绪</color>",
        };
        readonly static string[] bet_state_str = new string[]
        {
            "<color=red>未押注</color>",
            "<color=red>未押注</color>",
            "<color=red>押{0}</color>",
            "<color=red>押{0}</color>",
        };


        private void Awake()
        {
            text = transform.Find("Name").GetComponent<Text>();
            shadow = text.GetComponent<Shadow>();
            tImage = transform.Find("image");
            gImage = tImage.gameObject;
#if !TAIWU_GAME
            GameObject go = GameObject.Instantiate(Tools.GetActorFaceSample());
            actorFace = go.GetComponent<ActorFace>();

            RectTransform tf = (RectTransform)go.transform;
            tf.SetParent(tImage, false);
            tf.localScale = Vector3.one * 0.45f;
            tf.anchoredPosition = new Vector2(0, -80);
#endif
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


        public void SetData(PlayerData playerData)
        {
            this.data = playerData;

            if (playerData.ip == "0") // 空位
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
                    text.text = string.Format(name_format_str, playerData.name, ready_state_str[playerData.ready]);
                }
                else // 观战者
                {
                    string bet_player_name = bet_state_str[playerData.observer];
                    if (playerData.observer > 1)
                    {
                        PlayerData sel = QuquDesk.instance.GetPlayer(playerData.observer - 2);
                        if (sel.ip != "0")
                        {
                            bet_player_name = string.Format(bet_player_name, sel.name);
                        }
                        else
                        {
                            bet_player_name = string.Format(bet_player_name, (playerData.observer == 2 ? "左" : "右") + "边选手");
                        }
                    }
                    text.text = string.Format(name_format_str, playerData.name, bet_player_name);
                }
                SetColor(playerData.ip);
            }
#if !TAIWU_GAME
            if(null!= playerData && playerData.ip != "0")
            {
                Tools.UpdateFace(actorFace, playerData.age, playerData.gender, playerData.actorGenderChange, playerData.faceDate, playerData.faceColor, playerData.clotheId, true);
            }
            else
            {
                //Tools.UpdateFace(actorFace, playerData.age, playerData.gender, playerData.actorGenderChange, playerData.faceDate, playerData.faceColor, playerData.clotheId, false, true);
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