using System;
using System.Collections;
using System.Collections.Generic;
using GuiQuquAdventure;
using UnityEngine;
using UnityEngine.UI;

namespace GuiQuquAdventure
{

    public class PlayerItem : MonoBehaviour
    {
        public long time_stamp;
        RectTransform rtf;
        RectTransform rText;
        Text text;
        Shadow shadow;
        string ip;
        int desk_idx;
        int observer;
        Transform tImage;
#if !TAIWU_GAME
        ActorFace actorFace;
#endif
        private void Awake()
        {
            rText = (RectTransform)transform.Find("Text");
            shadow = rText.GetComponent<Shadow>();
            text = rText.GetComponent<Text>();
            rtf = (RectTransform)transform;
            tImage = transform.Find("image");
#if !TAIWU_GAME
            GameObject go = GameObject.Instantiate(Tools.GetActorFaceSample());
            actorFace = go.GetComponent<ActorFace>();
            go.transform.SetParent(tImage, false);
            go.transform.localScale = Vector3.one * .3f;
#endif
        }

        public void SetData(PlayerData playerData)
        {
            if(this.ip == playerData.ip && this.desk_idx == playerData.desk_idx && this.observer == playerData.observer)
            {
                return;
            }

            if (!gameObject.activeSelf)
            {
                gameObject.SetActive(true);
            }
            text.text = playerData.name ;
            if (playerData.desk_idx == -1)
            {
                text.text += "\n<size=30>(正在房间中闲逛)</size>";
            }
            else
            {
                text.text += string.Format("\n<size=25>(正在{0}号桌中{1})</size>", playerData.desk_idx % 100 + 1, playerData.observer == 0 ? "对战" : "围观");
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate(rText);
            rtf.sizeDelta = new Vector2(rtf.sizeDelta.x, rText.sizeDelta.y + 10);

            SetColor(playerData.ip);

            this.ip = playerData.ip;
            this.desk_idx = playerData.desk_idx;
            this.observer = playerData.observer;
#if !TAIWU_GAME
            if (playerData.ip != "0")
            {
                Tools.UpdateFace(actorFace, playerData.age, playerData.gender, playerData.actorGenderChange, playerData.faceDate, playerData.faceColor, playerData.clotheId, false);
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