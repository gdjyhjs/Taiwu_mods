using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GuiQuquAdventure
{
    public class QuquChat : MonoBehaviour
    {
        public static QuquChat instance;
        List<ChatItem> items;
        public GameObject prefab;
        ScrollRect scroll;

        private void Awake()
        {
            instance = this;
            items = new List<ChatItem>();
            prefab = transform.GetChild(0).gameObject;
            scroll = transform.parent.parent.GetComponent<ScrollRect>();
        }

        public void RestPos()
        {
            scroll.verticalNormalizedPosition = 0;
        }

        public void SetData(ChatData[] chatDatas)
        {
            float pos = scroll.verticalNormalizedPosition;
            for (int i = 0; i < chatDatas.Length; i++)
            {
                ChatItem item;
                if(i < items.Count)
                {
                    item = items[i];
                }
                else
                {
                    GameObject go = Instantiate<GameObject>(prefab);
                    go.transform.SetParent(transform, false);
                    go.SetActive(true);
                    item = go.AddComponent<ChatItem>();
                    items.Add(item);
                }
                item.SetData(chatDatas[i]);
            }
            for (int i = chatDatas.Length; i < items.Count; i++)
            {
                GameObject go = items[i].gameObject;
                if (go.activeSelf)
                {
                    go.SetActive(false);
                }
                else
                {
                    break;
                }
            }
            scroll.verticalNormalizedPosition = pos;
        }
    }

}