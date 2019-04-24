using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GuiQuquAdventure
{
    public class QuquPlayers : MonoBehaviour
    {
        public static QuquPlayers instance;
        List<PlayerItem> items;
        GameObject prefab;
        ScrollRect scroll;

        private void Awake()
        {
            instance = this;
            items = new List<PlayerItem>();
            prefab = transform.GetChild(0).gameObject;
            scroll = transform.parent.parent.GetComponent<ScrollRect>();
        }

        public void RestPos()
        {
            scroll.verticalNormalizedPosition = 0;
        }

        public void SetData(PlayerData[] chatDatas)
        {
            float pos = scroll.verticalNormalizedPosition;
            for (int i = 0; i < chatDatas.Length; i++)
            {
                PlayerItem item;
                if (i < items.Count)
                {
                    item = items[i];
                }
                else
                {
                    GameObject go = Instantiate<GameObject>(prefab);
                    go.transform.SetParent(transform, false);
                    item = go.AddComponent<PlayerItem>();
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