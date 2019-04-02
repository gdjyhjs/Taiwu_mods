
using Harmony12;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GuiScroll
{
    public static class ActorMenuInjuryPatch
    {
        [HarmonyPatch(typeof(ActorMenu), "SetInjuryIcon")]
        public static class ActorMenu_SetInjuryIcon_Patch
        {
            public static void Postfix(int typ, int injuryId, int injuryPower)
            {
                if (!Main.enabled)
                    return;
                // Main.Logger.Log(typ + " 设置伤口图标 " + injuryId+" : "+ injuryPower);
                //ActorMenu _this = ActorMenu.instance;
                //GameObject gameObject = UnityEngine.Object.Instantiate(_this.injuryBack, Vector3.zero, Quaternion.identity);
                //gameObject.name = "injury," + injuryId;
                //gameObject.transform.Find("InjuryIcon").GetComponent<Image>().sprite = GetSprites.instance.injuryIcon[DateFile.instance.ParseInt(DateFile.instance.injuryDate[injuryId][98])];
                //gameObject.transform.SetParent(_this.actorInjuryHolder[Mathf.Min(typ, 7)], worldPositionStays: false);
                //int injuryTyp = (DateFile.instance.ParseInt(DateFile.instance.injuryDate[injuryId][1]) > 0) ? 1 : 2;
                //string value = Mathf.Max(1, DateFile.instance.ParseInt(DateFile.instance.injuryDate[injuryId][1]) * injuryPower / 100).ToString();
                //gameObject.transform.Find("HpSpText").GetComponent<Text>().text = injuryTyp == 1 ? DateFile.instance.SetColoer(20010, value) : DateFile.instance.SetColoer(20007, value);
                //Button btn = gameObject.GetComponent<Button>();
                //if(!btn)
                //    btn = gameObject.AddComponent<Button>();
                //var onclick = btn.onClick;
                //onclick.RemoveAllListeners();
                //onclick.AddListener(delegate { OnClickInjury(injuryId, injuryTyp); });
                //return false;


                int injuryTyp = (DateFile.instance.ParseInt(DateFile.instance.injuryDate[injuryId][1]) > 0) ? 1 : 2;
                Transform tf = ActorMenu.instance.actorInjuryHolder[Mathf.Min(typ, 7)].Find("injury," + injuryId);
                if (tf)
                {
                    GameObject gameObject = tf.gameObject;
                    Button btn = gameObject.GetComponent<Button>();
                    if (!btn)
                        btn = gameObject.AddComponent<Button>();
                    var onclick = btn.onClick;
                    onclick.RemoveAllListeners();
                    onclick.AddListener(delegate { OnClickInjury(injuryId, injuryTyp); });
                }
            }
        }
        /// <summary>
        /// 点击伤口 进行自动治疗
        /// </summary>
        /// <param name="injuryId">伤口id</param>
        /// <param name="typ">1是外伤 2是内伤</param>
        public static void OnClickInjury(int injury_id, int typ)
        {
            // Main.Logger.Log(injuryId + " 点击伤口 退出 " + typ);
            int actorId = ActorMenu.instance.acotrId;
            int mainActorId = DateFile.instance.MianActorID();
            int useActorId = actorId;
            //  如果不是主角并且按了Ctrl 使用太吾传人身上的药进行疗伤
            if (actorId!= mainActorId && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)))
            {
                useActorId = mainActorId;
                // Main.Logger.Log(actorId + " 从太吾传人包里拿药 " + useActorId);
            }
            bool is_use = false; // 是否有吃了药
            while (true)// 实现从背包获取疗伤药进行疗伤，直到伤口治愈或者没有疗伤药了
            {
                int injuryId = injury_id;
                if (injuryId == -1)
                {
                    foreach (var item in DateFile.instance.actorInjuryDate[actorId])
                    {
                        int ijId = item.Key;
                        if (DateFile.instance.injuryDate[ijId].ContainsKey(typ) && DateFile.instance.ParseInt(DateFile.instance.injuryDate[ijId][typ]) > 0)
                        {
                            injuryId = ijId;
                            break;
                        }
                    }
                }
                if (!DateFile.instance.actorInjuryDate[actorId].TryGetValue(injuryId, out  int value)) // 没有这个伤口id 退出
                {
                    // Main.Logger.Log(actorId+" 没有这个伤口id 退出 " + injuryId);
                    if (!is_use)
                    {
                        YesOrNoWindow.instance.SetYesOrNoWindow(-1, "你狠健康", "你都这么健康了怎么还想吃药！", false, true);
                    }
                    break;
                }
                int itemId = GetActorHealingMedicine(actorId, typ, injuryId, useActorId); // 获取可用疗伤药物品ID
                if (itemId == -1) // 没有可用疗伤药 退出
                {
                    if (!is_use)
                    {
                        YesOrNoWindow.instance.SetYesOrNoWindow(-1, "没药了", "包包里找不到可以用来治疗伤口的药了哦！", false, true);
                    }
                    // Main.Logger.Log(actorId + " 没有可用疗伤药 退出 " + injuryId);
                    break;
                }
                is_use = true;
                int cureValue = typ == 1 ? (Mathf.Abs(DateFile.instance.ParseInt(DateFile.instance.GetItemDate(itemId, 11)))) : (Mathf.Abs(DateFile.instance.ParseInt(DateFile.instance.GetItemDate(itemId, 12))));
                if ((cureValue * 3 < DateFile.instance.actorInjuryDate[actorId][injuryId]))// 判断是否能百分百发挥疗伤药药效
                {
                    cureValue /= 5;
                }
                DateFile.instance.RemoveInjury(actorId, injuryId, -cureValue);
                for (int m = 0; m < 6; m++)
                {
                    int num23 = DateFile.instance.ParseInt(DateFile.instance.GetItemDate(itemId, 71 + m));
                    if (num23 != 0)
                    {
                        ActorMenu.instance.ChangePoison(actorId, m, num23 * 10);
                    }
                }
                DateFile.instance.ChangeItemHp(useActorId, itemId, -1); // 消耗物品耐久度
            }
            if (is_use)
            {
                DateFile.instance.PlayeSE(8); // 音效
                WindowManage.instance.WindowSwitch(on: false);
                ActorMenu.instance.GetActorInjury(actorId, ActorMenu.instance.injuryTyp);
                if (DateFile.instance.battleStart)
                {
                    StartBattle.instance.UpdateActorHSQP();
                }
            }
        }
        /// <summary>
        /// 获得人物身上的疗伤药 优先寻找能百分百发挥药效的疗伤药
        /// </summary>
        /// <param name="actorId">人物ID</param>
        /// <param name="typ">1是外伤 2是内伤</param>
        /// <param name="injuryId">伤口id</param>
        /// <param name="useActorId">提供疗伤药的人物ID</param>
        /// <returns>疗伤药的物品id</returns>
        public static int GetActorHealingMedicine(int actorId,int typ, int injuryId,int useActorId)
        {
            // Main.Logger.Log(actorId + " 寻找疗伤药 " + typ);
            ActorMenu _this = ActorMenu.instance;
            List<int> itemSort = DateFile.instance.GetItemSort(new List<int>(_this.GetActorItems(useActorId).Keys));
            int result = -1;
            foreach (int itemId in itemSort)
            {
                int cureValue = typ == 1 ? (Mathf.Abs(DateFile.instance.ParseInt(DateFile.instance.GetItemDate(itemId, 11)))) : (Mathf.Abs(DateFile.instance.ParseInt(DateFile.instance.GetItemDate(itemId, 12))));
                // Main.Logger.Log(itemId + " 物品疗伤效果 " + cureValue);
                if (cureValue>0)//判断是否要的疗伤药
                {
                    if ((cureValue * 3 >= DateFile.instance.actorInjuryDate[actorId][injuryId]))// 判断是否能百分百发挥疗伤药药效
                    {
                        // Main.Logger.Log("获得能百分百发挥药效的疗伤药");
                        return itemId;
                    }
                    else if(result==-1)
                    {
                        // Main.Logger.Log("记录疗伤药");
                        result = itemId;
                    }
                }
            }
            return result;
        }
        // 增加健康
        public static void AddHealth()
        {
            int actorId = ActorMenu.instance.acotrId;
            int mainActorId = DateFile.instance.MianActorID();
            int useActorId = actorId;
            //  如果不是主角并且按了Ctrl 使用太吾传人身上的药进行疗伤
            if (actorId != mainActorId && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)))
            {
                useActorId = mainActorId;
                // Main.Logger.Log(actorId + " 从太吾传人包里拿药 " + useActorId);
            }
            bool is_use = false; // 是否有吃了药
            int old_hp = ActorMenu.instance.Health(ActorMenu.instance.acotrId);
            while (true)
            {
                int hp = ActorMenu.instance.Health(actorId);
                int maxHp = ActorMenu.instance.MaxHealth(actorId);
                // Main.Logger.Log("健康:" + hp + "/" + maxHp);
                if (hp >= maxHp)
                {
                    if (!is_use)
                    {
                        YesOrNoWindow.instance.SetYesOrNoWindow(-1, "你狠健康", "你都这么健康了怎么还想吃药！", false, true);
                    }
                    break;
                }
                int itemId = GetActorHealthMedicine(actorId, useActorId, maxHp - hp); // 获取可用疗伤药物品ID
                if (itemId == -1) // 没有可用疗伤药 退出
                {
                    if (!is_use)
                    {
                        YesOrNoWindow.instance.SetYesOrNoWindow(-1, "你太穷了", "包里没有可以吃的药哦！", false, true);
                    }
                    break;
                }
                is_use = true;
                ActorMenu.instance.ChangeHealth(actorId, DateFile.instance.ParseInt(DateFile.instance.GetItemDate(itemId, 13)));
                for (int num33 = 0; num33 < 6; num33++)
                {
                    int num34 = DateFile.instance.ParseInt(DateFile.instance.GetItemDate(itemId, 71 + num33));
                    if (num34 != 0)
                    {
                        ActorMenu.instance.ChangePoison(actorId, num33, num34 * 10);
                    }
                }
                DateFile.instance.ChangeItemHp(actorId, itemId, -1);

            }
            if (is_use)
            {
                int new_hp = ActorMenu.instance.Health(ActorMenu.instance.acotrId);
                YesOrNoWindow.instance.SetYesOrNoWindow(-1, "恢复健康", "健康恢复了" + (new_hp - old_hp) + "点！", false, true);

                DateFile.instance.PlayeSE(8); // 音效
                WindowManage.instance.WindowSwitch(on: false);

                ActorMenu.instance.UpdateItems(actorId, ActorMenu.instance.itemTyp);
            }
        }

        public static int GetActorHealthMedicine(int actorId, int useActorId, int lack)
        {
            ActorMenu _this = ActorMenu.instance;
            List<int> itemSort = DateFile.instance.GetItemSort(new List<int>(_this.GetActorItems(useActorId).Keys));
            int result = -1;
            // Main.Logger.Log("物品数量 " + itemSort.Count);
            foreach (int itemId in itemSort)
            {
                int cureValue = DateFile.instance.ParseInt(DateFile.instance.GetItemDate(itemId, 13));
                // Main.Logger.Log(DateFile.instance.GetItemDate(itemId, 0) +" : "+cureValue);
                if (cureValue > 0)//判断是否要的寿命药
                {
                    if (cureValue * 3 >= lack)// 判断是否能百分百发挥疗伤药药效
                    {
                        return itemId;
                    }
                    else if (result == -1)
                    {
                        result = itemId;
                    }
                }
            }
            // Main.Logger.Log("获取寿命药 "+ result);
            return result;
        }


        // 回复内息
        public static void AddMianQi()
        {
            int actorId = ActorMenu.instance.acotrId;
            int mainActorId = DateFile.instance.MianActorID();
            int useActorId = actorId;
            //  如果不是主角并且按了Ctrl 使用太吾传人身上的药进行疗伤
            if (actorId != mainActorId && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)))
            {
                useActorId = mainActorId;
                // Main.Logger.Log(actorId + " 从太吾传人包里拿药 " + useActorId);
            }
            bool is_use = false; // 是否有吃了药
            while (true)
            {
                int actorMianQi = DateFile.instance.GetActorMianQi(actorId);
                // Main.Logger.Log("健康:" + hp + "/" + maxHp);
                if (actorMianQi <= 0)
                {
                    if (!is_use)
                    {
                        YesOrNoWindow.instance.SetYesOrNoWindow(-1, "你狠健康", "你都这么健康了怎么还想吃药！", false, true);
                    }
                    break;
                }
                int itemId = GetActorMianQiMedicine(actorId, useActorId, actorMianQi); // 获取可用疗伤药物品ID
                if (itemId == -1) // 没有可用内息药 退出
                {
                    if (!is_use)
                    {
                        YesOrNoWindow.instance.SetYesOrNoWindow(-1, "你太穷了", "包里没有可以吃的药哦！", false, true);
                    }
                    break;
                }
                is_use = true;
                ActorMenu.instance.ChangeMianQi(actorId, DateFile.instance.ParseInt(DateFile.instance.GetItemDate(itemId, 39)));
                for (int num33 = 0; num33 < 6; num33++) // 毒性
                {
                    int num34 = DateFile.instance.ParseInt(DateFile.instance.GetItemDate(itemId, 71 + num33));
                    if (num34 != 0)
                    {
                        ActorMenu.instance.ChangePoison(actorId, num33, num34 * 10);
                    }
                }
                DateFile.instance.ChangeItemHp(actorId, itemId, -1);

            }
            if (is_use)
            {

                DateFile.instance.PlayeSE(8); // 音效
                WindowManage.instance.WindowSwitch(on: false);

                ActorMenu.instance.UpdateItems(actorId, ActorMenu.instance.itemTyp);
            }
        }

         // 获取内息药物
        public static int GetActorMianQiMedicine(int actorId, int useActorId, int actorMianQi)
        {
            ActorMenu _this = ActorMenu.instance;
            List<int> itemSort = DateFile.instance.GetItemSort(new List<int>(_this.GetActorItems(useActorId).Keys));
            int result = -1;
            // Main.Logger.Log("物品数量 " + itemSort.Count);
            foreach (int itemId in itemSort)
            {
                int cureValue = Mathf.Abs(DateFile.instance.ParseInt(DateFile.instance.GetItemDate(itemId, 39)));
                // Main.Logger.Log(DateFile.instance.GetItemDate(itemId, 0) +" : "+cureValue);
                if (cureValue > 0)//判断是否内息药
                {
                    if (actorMianQi > cureValue)// 判断是否能百分百发挥疗伤药药效
                    {
                        return itemId;
                    }
                    else if (result == -1)
                    {
                        result = itemId;
                    }
                }
            }
            // Main.Logger.Log("获取内息药 "+ result);
            return result;
        }


        // 回复中毒
        public static void AddPoison(int typ)
        {
            int actorId = ActorMenu.instance.acotrId;
            int mainActorId = DateFile.instance.MianActorID();
            int useActorId = actorId;
            //  如果不是主角并且按了Ctrl 使用太吾传人身上的药进行疗伤
            if (actorId != mainActorId && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)))
            {
                useActorId = mainActorId;
                // Main.Logger.Log(actorId + " 从太吾传人包里拿药 " + useActorId);
            }
            bool is_use = false; // 是否有吃了药
            while (true)
            {
                bool need = false;
                for (int i = 0; i < 6; i++)
                {
                    if(typ == -1 || typ == i)
                    {
                        int value = DateFile.instance.ParseInt(DateFile.instance.GetActorDate(actorId, 51 + i, addValue: false));
                        if (!need && value > 0)
                        {
                            need = true;
                            break;
                        }
                    }
                }

                // Main.Logger.Log("健康:" + hp + "/" + maxHp);
                if (!need)
                {
                    if (!is_use)
                    {
                        YesOrNoWindow.instance.SetYesOrNoWindow(-1, "你狠健康", "你都这么健康了怎么还想吃药！", false, true);
                    }
                    break;
                }
                int itemId = GetActorPoisonMedicine(actorId, useActorId, typ); // 获取可用疗伤药物品ID
                if (itemId == -1) // 没有可用药 退出
                {
                    if (!is_use)
                    {
                        YesOrNoWindow.instance.SetYesOrNoWindow(-1, "你太穷了", "包里没有可以吃的药哦！", false, true);
                    }
                    break;
                }
                is_use = true;
                
                for (int num33 = 0; num33 < 6; num33++) // 毒性
                {
                    int num4 = DateFile.instance.ParseInt(DateFile.instance.GetItemDate(itemId, 61 + num33));
                    if (num4 != 0)
                    {
                        ActorMenu.instance.ChangePoison(actorId, num33, num4 * 5);
                    }
                    int num34 = DateFile.instance.ParseInt(DateFile.instance.GetItemDate(itemId, 71 + num33));
                    if (num34 != 0)
                    {
                        ActorMenu.instance.ChangePoison(actorId, num33, num34 * 10);
                    }
                }
                DateFile.instance.ChangeItemHp(actorId, itemId, -1);

            }
            if (is_use)
            {

                DateFile.instance.PlayeSE(8); // 音效
                WindowManage.instance.WindowSwitch(on: false);

                ActorMenu.instance.UpdateItems(actorId, ActorMenu.instance.itemTyp);
            }
        }

        // 获取解毒药物
        public static int GetActorPoisonMedicine(int actorId, int useActorId, int typ)
        {
            // Main.Logger.Log("获取解毒药物"+ actorId+" "+ useActorId+" "+ typ);
            ActorMenu _this = ActorMenu.instance;
            List<int> itemSort = DateFile.instance.GetItemSort(new List<int>(_this.GetActorItems(useActorId).Keys));
            int result = -1;
            // Main.Logger.Log("物品数量 " + itemSort.Count);
            foreach (int itemId in itemSort)
            {
                for (int i = 0; i < 6; i++)
                {
                    // Main.Logger.Log(DateFile.instance.GetItemDate(itemId, 0));
                    if (typ==-1||typ == i)
                    {
                        int value = DateFile.instance.ParseInt(DateFile.instance.GetItemDate(itemId, 61 + i));
                        if (value < 0)
                        {
                            int num = DateFile.instance.ParseInt(DateFile.instance.GetActorDate(actorId, 51 + i, addValue: false));
                            if (!(value < 0 && num > Mathf.Abs(value) * 3))// 判断是否能百分百发挥疗伤药药效
                            {
                                return itemId;
                            }
                            else if (result == -1)
                            {
                                result = itemId;
                            }
                        }
                    }
                }
            }
            // Main.Logger.Log("获取解毒药 "+ result);
            return result;
        }

    }
}