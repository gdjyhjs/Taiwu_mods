﻿using System;
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
        int[] last_image = new int[20];
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


            RectTransform tf = (RectTransform)go.transform;
            tf.SetParent(tImage, false);
            tf.localScale = Vector3.one * 0.15f;
            tf.anchoredPosition = new Vector2(-5, -50);
#endif
        }

        //float size = 0.3f;
        //float x = 0;
        //float y = 0;
        //void OnGUI()
        //{
        //    if (GUI.Button(new Rect(50, 50, 100, 50), "调整PlayerItem"))
        //    {
        //        GameObject go = actorFace.gameObject;
        //        RectTransform tf = (RectTransform)go.transform;
        //        tf.anchoredPosition = new Vector2(x, y);
        //        tf.localScale = Vector3.one * size;
        //    }
        //    float.TryParse(GUI.TextField(new Rect(150, 50, 100, 50), size.ToString()), out size);
        //    float.TryParse(GUI.TextField(new Rect(250, 50, 100, 50), x.ToString()), out x);
        //    float.TryParse(GUI.TextField(new Rect(350, 50, 100, 50), y.ToString()), out y);
        //}

        public void SetData(PlayerData playerData)
        {
            if (!gameObject.activeSelf)
            {
                gameObject.SetActive(true);
            }
#if !TAIWU_GAME
            if (null != playerData && playerData.ip != "0")
            {
                Tools.UpdateFace(last_image, actorFace, playerData.age, playerData.gender, playerData.actorGenderChange, playerData.faceDate, playerData.faceColor, playerData.clotheId, true);
            }
#endif
            if (this.ip == playerData.ip && this.desk_idx == playerData.desk_idx && this.observer == playerData.observer)
            {
                return;
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
        }

        void SetColor(string ip)
        {
            Color color = Tools.GetColor(ip);
            Color shadowColor = Tools.GetShadowColor(color);
            //text.color = shadowColor;
            text.text = $"<color=#{(color.r*255).ToString("0x")}{(color.g * 255).ToString("0x")}{(color.b * 255).ToString("0x")}>✦</color>{text.text}" ;
            shadow.effectColor = color;
        }
    }
}