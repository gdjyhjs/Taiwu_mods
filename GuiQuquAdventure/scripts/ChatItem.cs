using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GuiQuquAdventure
{
    class ChatItem : MonoBehaviour
    {
        public long time_stamp;
        RectTransform rtf;
        RectTransform rText;
        Text text;
        Shadow shadow;
        private void Awake()
        {
            rText = (RectTransform)transform.Find("Text");
            shadow = rText.GetComponent<Shadow>();
            text = rText.GetComponent<Text>();
            rtf = (RectTransform)transform;
        }

        public void SetData(ChatData chatData)
        {
            if (chatData.time_stamp == time_stamp)
            {
                // 不需要修改
                return;
            }
            if (!gameObject.activeSelf)
            {
                gameObject.SetActive(true);
            }
            text.text = chatData.name + "：" + chatData.content;
            LayoutRebuilder.ForceRebuildLayoutImmediate(rText);
            rtf.sizeDelta = new Vector2(rtf.sizeDelta.x, rText.sizeDelta.y + 10);

            SetColor(chatData.ip);
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