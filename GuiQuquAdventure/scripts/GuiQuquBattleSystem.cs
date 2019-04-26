using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GuiQuquAdventure
{
    public class GuiQuquBattleSystem : MonoBehaviour
    {
        public static GuiQuquBattleSystem instance;

        public enum ActorTyp
        {
            /// <summary>
            /// 左边选手
            /// </summary>
            LeftPlayer,
            /// <summary>
            /// 右边选手
            /// </summary>
            RightPlayer,
            /// <summary>
            /// 押左边选手的观众
            /// </summary>
            LeftObserver,
            /// <summary>
            /// 押右边选手的观众
            /// </summary>
            RightObserver,
            /// <summary>
            /// 围观的吃瓜群众
            /// </summary>
            OtherObserver = 9,
        }
        public PlayerData leftPlayer;
        public PlayerData rightPlayer;
        public int itemId;
        public ActorTyp actorTyp;
        public long playId = 0;

        private int ququBattlePart = 0;

        private int ququBattleTurn = 0;

        public GameObject setBattleSpeedHolder;

        public Toggle[] setBattleSpeedToggle;

        public GameObject ququBattleWindow;

        public Transform actorBack;

        public Transform enemyBack;

        public Button chooseActorButton;

        public GameObject[] ququBattleBack;

        private bool showQuquBattleWindow = false;

        public bool surrender = false;

        /// <summary>
        /// 敌人id
        /// </summary>
        public int ______ququBattleEnemyId = 0;

        /// <summary>
        /// 敌人战斗强度
        /// </summary>
        public int ______ququBattleId = 0;

        public int bodyActorId = 0;

        public ActorFace actorFace;

        public ActorFace enemyFace;

        public Text actorNameText;

        public Text enemyNameText;

        public Text[] battleStateText;

        public Transform[] battleValue;

        public Transform[] actorQuqu;

        public Transform[] enemyQuqu;

        public GameObject startBattleWindow;

        public Button startBattleButton;

        public Button loseBattleButton;

        public Text[] uiText;

        public Image actorBodyImage;

        public Text actorBodyNameText;

        public Image enemyBodyImage;

        public Text enemyBodyNameText;

        public Image resourceButtonImage;

        private int enemyBodyTyp;

        private int enemyBodyId;

        private int ______enemyBodySize;

        private int enemyBodyWorth;

        private int showQuquIndex = 0;

        public GameObject[] hideQuquImage;

        private int[] actorQuquId;

        private int[] enemyQuquId;

        private int[] actorQuquHp;

        private int[] enemyQuquHp;

        private int[] actorQuquSp;

        private int[] enemyQuquSp;

        public Text[] actorQuquHpText;

        public Text[] enemyQuquHpText;

        public Image[] actorQuquIcon;

        public Image[] enemyQuquIcon;

        public Text[] actorBattleQuquNameText;

        public Text[] actorBattleQuquPower1Text;

        public Text[] actorBattleQuquPower2Text;

        public Text[] actorBattleQuquPower3Text;

        public Text[] actorBattleQuquStrengthText;

        public Text[] actorBattleQuquMagicText;

        public Image[] actorBattleQuquStrengthBar;

        public Image[] actorBattleQuquMagicBar;

        public Text[] enemyBattleQuquNameText;

        public Text[] enemyBattleQuquPower1Text;

        public Text[] enemyBattleQuquPower2Text;

        public Text[] enemyBattleQuquPower3Text;

        public Text[] enemyBattleQuquStrengthText;

        public Text[] enemyBattleQuquMagicText;

        public Image[] enemyBattleQuquStrengthBar;

        public Image[] enemyBattleQuquMagicBar;

        public Text[] actorQuquName;

        public Text[] enemyQuquName;

        private int actorBodyTyp;

        private int actorBodyId;

        private int actorBodySize;

        private bool chooseBodyIsShow = false;

        public RectTransform chooseBodyWindow;

        public Text needResourceText;

        public Button useResourceButton;

        public GameObject acotrWindow;

        public Button useActorButton;

        public Transform actorHolder;

        public GameObject actorIcon;

        private int setActorBodyId = 0;

        private int choseItemIndex;

        public bool itemWindowIsShow = false;

        public GameObject itemWindow;

        public Button useItemButton;

        public GameObject itemMask;

        public Transform itemHolder;

        private int getItemTyp;

        private int winTurn = 0;

        private const float startDelay = 1f;

        public Button[] nextButton;

        public GameObject[] nextButtonMask;

        private int battleEndTyp = 0;

        public Text[] actorQuquDamageText;

        public Text[] enemyQuquDamageText;

        public GameObject battleEndWindow;

        public GameObject closeBattleButton;

        public Image battleEndTypImage;

        public Text battleEndBodyName;

        public Text battleEndBodyText;

        public QuquPlace[] actorQuquCall;

        public QuquPlace[] enemyQuquCall;

        private int[] actorQuquCallTime;

        private int[] enemyQuquCallTime;

        private const int maxCallSpeed = 600;


        public void SetQuquBattleSpeed(int value)
        {
            Time.timeScale = value;
        }

        private void SetTweener()
        {
            Tweener t = itemWindow.GetComponent<RectTransform>().DOLocalMoveX(0f, 0.2f).SetEase(Ease.OutBack)
                .SetUpdate(isIndependentUpdate: true);
            t.SetAutoKill(autoKillOnCompletion: false);
            t.Pause();
            Tweener t2 = chooseBodyWindow.DOSizeDelta(new Vector2(280f, 120f), 0.2f).SetUpdate(isIndependentUpdate: true);
            t2.SetAutoKill(autoKillOnCompletion: false);
            t2.Pause();
            Tweener t3 = acotrWindow.GetComponent<RectTransform>().DOLocalMoveX(0f, 0.2f).SetEase(Ease.OutBack)
                .SetUpdate(isIndependentUpdate: true);
            t3.SetAutoKill(autoKillOnCompletion: false);
            t3.Pause();
        }

        /// <summary>
        /// 显示战斗窗口 |
        /// </summary>
        public void ShowQuquBattleWindow()
        {
            GuiRandom.InitSeed((int)playId);// 初始化随机种子

            Main.Logger.Log("ShowQuquBattleWindow 显示战斗窗口 |" + Time.time);
            if (!showQuquBattleWindow)
            {
                showQuquBattleWindow = true;
                for (int i = 0; i < setBattleSpeedToggle.Length; i++)
                {
                    setBattleSpeedToggle[i].isOn = false; // 关闭所有加速开关
                }
                setBattleSpeedToggle[0].isOn = true; // 打开一倍加速
                Time.timeScale = 1f;
                setBattleSpeedHolder.SetActive(false); // 隐藏速度开关选项
                TipsWindow.instance.showTipsTime = -100;
                if (StorySystem.instance.itemWindowIsShow)
                {
                    StorySystem.instance.CloseItemWindow();
                }
                if (StorySystem.instance.toStoryIsShow)
                {
                    StorySystem.instance.ClossToStoryMenu();
                }
                UIMove.instance.CloseGUI();
                Main.Logger.Log("0.25s |" + Time.time);
                Invoke("QuquBattleWindowOpend", 0.25f);
            }
        }

        /// <summary>
        /// 战斗窗口打开 ||
        /// </summary>
        public void QuquBattleWindowOpend()
        {
            Main.Logger.Log("QuquBattleWindowOpend 战斗窗口打开 ||" + Time.time);
            showQuquBattleWindow = false;
            TipsWindow.instance.showTipsTime = -100;
            YesOrNoWindow.instance.ShwoWindowMask(ququBattleWindow.transform, true);
            battleEndWindow.transform.localScale = new Vector3(5f, 5f, 1f);
            battleEndWindow.SetActive(false);
            closeBattleButton.SetActive(false);
            ququBattleWindow.SetActive(true);
            SetQuquBattle();
            actorBack.transform.localPosition = new Vector3(-1400f, 0f, 0f);
            enemyBack.transform.localPosition = new Vector3(1400f, 0f, 0f);
            ququBattleWindow.transform.localPosition = new Vector3(0f, 1200f, 0f);
            DateFile.instance.SystemAudioPlay(4, 1, 1f);
            Main.Logger.Log("QuquBattleWindowOpend 战斗窗口打开!!!!!! ||" + Time.time);
            try
            {
                ququBattleWindow.transform.DOLocalMoveY(0f, 0.2f).SetUpdate(true).OnComplete(SetActorAnimation);
            }
            catch (System.Exception e)
            {
                Main.Logger.Log(" ERROR :当前异常的信息: " + e.Message);
                Main.Logger.Log(" ERROR :导致错误的应用程序或对象的名称: " + e.Source);
                Main.Logger.Log(" ERROR :堆栈上的即时框架字符串: " + e.StackTrace);
                throw;
            }
        }

        private void SetActorAnimation()
        {
            actorBack.DOLocalMoveX(-960f, 0.2f).SetDelay(0.1f).SetUpdate(isIndependentUpdate: true)
                .OnComplete(SetAnimationDone);
            enemyBack.DOLocalMoveX(960f, 0.2f).SetDelay(0.1f).SetUpdate(isIndependentUpdate: true);
        }

        private void SetAnimationDone()
        {
            ququBattlePart = 1;
            loseBattleButton.interactable = true;
        }

        public void CloseQuquBattleWindowButton()
        {
            YesOrNoWindow.instance.SetYesOrNoWindow(516, DateFile.instance.massageDate[8002][4].Split('|')[0], DateFile.instance.massageDate[8002][4].Split('|')[1] + ((DateFile.instance.ParseInt(DateFile.instance.cricketBattleDate[______ququBattleId][9]) > 0) ? (DateFile.instance.massageDate[8002][4].Split('|')[2] + DateFile.instance.cricketBattleDate[______ququBattleId][9]) : "") + DateFile.instance.massageDate[8002][4].Split('|')[3]);
        }

        public void CloseQuquBattleWindow()
        {
            battleEndWindow.SetActive(value: false);
            if (surrender)
            {
                int num = DateFile.instance.ParseInt(DateFile.instance.cricketBattleDate[______ququBattleId][9]);
                UIDate.instance.ChangeResource(DateFile.instance.MianActorID(), 6, -num, canShow: false);
                string[] array = DateFile.instance.cricketBattleDate[______ququBattleId][7].Split('&');
                int num2 = DateFile.instance.ParseInt(array[0]);
                if (num2 != 0)
                {
                    if (array.Length > 1)
                    {
                        int num3 = DateFile.instance.ParseInt(array[1]);
                        if (num3 == 1)
                        {
                            DateFile.instance.SetEvent(new int[4]
                            {
                            0,
                            ______ququBattleEnemyId,
                            num2,
                            ______ququBattleEnemyId
                            }, addToFirst: true);
                        }
                    }
                    else
                    {
                        DateFile.instance.SetEvent(new int[3]
                        {
                        0,
                        -1,
                        num2
                        }, addToFirst: true);
                    }
                }
            }
            setBattleSpeedHolder.SetActive(value: false);
            Time.timeScale = 1f;
            ququBattlePart = 0;
            ququBattleWindow.transform.DOLocalMoveY(1200f, 0.3f).SetUpdate(isIndependentUpdate: true).OnComplete(CloseBattleWindowDone);
        }

        private void CloseBattleWindowDone()
        {
            bodyActorId = 0;
            ququBattleWindow.SetActive(value: false);
            YesOrNoWindow.instance.ShwoWindowMask(ququBattleWindow.transform, on: false);
            UIMove.instance.ShowGUI();
            DateFile.instance.UpdatePlaceBgm(DateFile.instance.mianPartId, DateFile.instance.mianPlaceId);
        }

        /// <summary>
        /// 设置战斗 |||
        /// </summary>
        private void SetQuquBattle()
        {
            Main.Logger.Log("SetQuquBattle 设置战斗 |||" + Time.time);
            ququBattleTurn = 0;
            surrender = false;
            actorQuquCallTime = new int[3]
            {
            GuiRandom.Range(0, 300),
            GuiRandom.Range(0, 300),
            GuiRandom.Range(0, 300)
            };
            enemyQuquCallTime = new int[3]
            {
            GuiRandom.Range(0, 300),
            GuiRandom.Range(0, 300),
            GuiRandom.Range(0, 300)
            };
            //int num0 = DateFile.instance.MianActorID();
            //int num2 = DateFile.instance.ParseInt(DateFile.instance.cricketBattleDate[______ququBattleId][1]);
            //int num3 = (num2 == 0) ? ______ququBattleEnemyId : num2;
            startBattleWindow.SetActive(value: true);
            for (int i = 0; i < 3; i++)
            {
                Component[] componentsInChildren = ququBattleBack[i].GetComponentsInChildren<Component>();
                Component[] array = componentsInChildren;
                foreach (Component component in array)
                {
                    if (component is Graphic)
                    {
                        (component as Graphic).CrossFadeAlpha(0f, 0f, ignoreTimeScale: true);
                    }
                }
            }
            startBattleButton.interactable = false;
            loseBattleButton.interactable = false;
            Main.Logger.Log("AAA");
            for (int k = 0; k < battleValue.Length; k++)
            {
                Main.Logger.Log("AAA"+k);
                hideQuquImage[k].transform.localScale = new Vector3(1f, 1f, 1f);
                nextButton[k].gameObject.SetActive(value: false);
                nextButtonMask[k].GetComponent<Image>().color = new Color(0f, 0f, 0f, 0f);
                battleStateText[k].text = "";
                battleStateText[k].transform.localScale = new Vector3(1f, 1f, 1f);
                battleValue[k].localPosition = new Vector3(0f, 100f, 0f);
                actorQuqu[k].localPosition = new Vector3(0f, 0f, 0f);
                enemyQuqu[k].localPosition = new Vector3(0f, 0f, 0f);
                actorQuquIcon[k].GetComponent<Button>().interactable = true;
                actorQuquDamageText[k].text = "";
                enemyQuquDamageText[k].text = "";
            }
            Main.Logger.Log("AAA222");
            //chooseActorButton.interactable = (DateFile.instance.ParseInt(DateFile.instance.cricketBattleDate[______ququBattleId][8]) == 1);
            chooseActorButton.interactable = (true);
            actorBodyImage.GetComponent<Button>().interactable = true;

            //actorFace.SetActorFace(num0);
            //enemyFace.SetActorFace(num3);
            Main.Logger.Log("AAA444");
            if (actorFace == null)
            {
                Main.Logger.Log("AAA*&@%^@%^@&*");
            }
            Tools.UpdateFace(actorFace, leftPlayer.age, leftPlayer.gender, leftPlayer.actorGenderChange, leftPlayer.faceDate, leftPlayer.faceColor, leftPlayer.clotheId, true);
            Main.Logger.Log("AAA!!!");
            actorNameText.text = leftPlayer.name; //  DateFile.instance.GetActorName(num0);
            Main.Logger.Log("AAA@@@");

            bool show_enemy = null != rightPlayer;
            Main.Logger.Log("AAA###"+ show_enemy);
            if (show_enemy)
            {
                Tools.UpdateFace(enemyFace, rightPlayer.age, rightPlayer.gender, rightPlayer.actorGenderChange, rightPlayer.faceDate, rightPlayer.faceColor, rightPlayer.clotheId, true);
                enemyNameText.text = rightPlayer.name; //  DateFile.instance.GetActorName(num3);
            }
            else
            {
                enemyNameText.text = "";
            }
            Main.Logger.Log("AAA&&&");
            if (enemyFace.gameObject.activeSelf != show_enemy)
            {
                enemyFace.gameObject.SetActive(show_enemy);
            }
            Main.Logger.Log("AAA666");
            //enemyBodyTyp = DateFile.instance.ParseInt(DateFile.instance.cricketBattleDate[______ququBattleId][3]);
            //enemyBodySize = 1;
            //int num4 = DateFile.instance.ParseInt(DateFile.instance.cricketBattleDate[______ququBattleId][5]);
            #region/ 下面这整段都是设置敌人赌注的
            //if (num2 == 0)
            //{
            //    switch (enemyBodyTyp)
            //    {
            //        case 0:
            //            SetResourceBody(num3, num4);
            //            break;
            //        case 1:
            //            {
            //                enemyBodyId = 0;
            //                List<int> list = new List<int>();
            //                List<int> list2 = new List<int>(DateFile.instance.actorItemsDate[num3].Keys);
            //                for (int l = 0; l < list2.Count; l++)
            //                {
            //                    int num5 = list2[l];
            //                    if (DateFile.instance.ParseInt(DateFile.instance.GetItemDate(num5, 3)) == 1 && DateFile.instance.ParseInt(DateFile.instance.GetItemDate(num5, 8)) <= num4)
            //                    {
            //                        list.Add(num5);
            //                    }
            //                }
            //                if (list.Count > 0)
            //                {
            //                    enemyBodyId = list[GuiRandom.Range(0, list.Count)];
            //                }
            //                if (enemyBodyId != 0)
            //                {
            //                    enemyBodyImage.tag = "ActorItem";
            //                    enemyBodyImage.name = "EnemyItemIcon," + enemyBodyId;
            //                    enemyBodyImage.GetComponent<Image>().sprite = GetSprites.instance.itemSprites[DateFile.instance.ParseInt(DateFile.instance.GetItemDate(enemyBodyId, 98))];
            //                    resourceButtonImage.sprite = GetSprites.instance.makeResourceIcon[DateFile.instance.ParseInt(DateFile.instance.resourceDate[5][98])];
            //                    enemyBodyNameText.text = DateFile.instance.SetColoer(20001 + DateFile.instance.ParseInt(DateFile.instance.GetItemDate(enemyBodyId, 8)), DateFile.instance.GetItemDate(enemyBodyId, 0));
            //                    enemyBodyWorth = DateFile.instance.ParseInt(DateFile.instance.GetItemDate(enemyBodyId, 904));
            //                }
            //                else
            //                {
            //                    SetResourceBody(num3, num4);
            //                }
            //                break;
            //            }
            //    }
            //}
            //else
            //{
            //    enemyBodyId = DateFile.instance.ParseInt(DateFile.instance.cricketBattleDate[______ququBattleId][4]);
            //    switch (enemyBodyTyp)
            //    {
            //        case 0:
            //            enemyBodySize = num4 + GuiRandom.Range(-(num4 * 30 / 100), num4 * 30 / 100 + 1);
            //            enemyBodyImage.tag = "ResourceIcon";
            //            enemyBodyImage.name = "ResourceIcon," + enemyBodyId;
            //            enemyBodyImage.sprite = GetSprites.instance.attSprites[DateFile.instance.ParseInt(DateFile.instance.resourceDate[enemyBodyId][98])];
            //            resourceButtonImage.sprite = GetSprites.instance.makeResourceIcon[DateFile.instance.ParseInt(DateFile.instance.resourceDate[enemyBodyId][98])];
            //            enemyBodyNameText.text = DateFile.instance.resourceDate[enemyBodyId][1] + DateFile.instance.SetColoer((enemyBodyId == 6) ? 20007 : ((enemyBodyId == 5) ? 20008 : 20003), "×" + enemyBodySize);
            //            enemyBodyWorth = enemyBodySize;
            //            break;
            //        case 1:
            //            if (DateFile.instance.ParseInt(DateFile.instance.GetItemDate(enemyBodyId, 6)) == 0)
            //            {
            //                enemyBodyId = DateFile.instance.MakeNewItem(enemyBodyId, -4);
            //                if (DateFile.instance.ParseInt(DateFile.instance.GetItemDate(enemyBodyId, 2001)) == 1)
            //                {
            //                    string[] array2 = DateFile.instance.cricketBattleDate[______ququBattleId][5].Split('|');
            //                    enemyBodySize = ((array2.Length > 1) ? GuiRandom.Range(DateFile.instance.ParseInt(array2[0]), DateFile.instance.ParseInt(array2[1])) : DateFile.instance.ParseInt(array2[0]));
            //                    List<int> list3 = new List<int>(DateFile.instance.cricketDate.Keys);
            //                    int num6 = enemyBodySize;
            //                    int num7 = enemyBodySize;
            //                    int num8 = 0;
            //                    int num9 = 0;
            //                    int partId = 0;
            //                    if (num6 >= 8)
            //                    {
            //                        num7 = 0;
            //                        List<int> list4 = new List<int>();
            //                        for (int m = 0; m < list3.Count; m++)
            //                        {
            //                            int num10 = list3[m];
            //                            if (DateFile.instance.ParseInt(DateFile.instance.cricketDate[num10][1]) == num6)
            //                            {
            //                                for (int n = 0; n < DateFile.instance.ParseInt(DateFile.instance.cricketDate[num10][6]); n++)
            //                                {
            //                                    list4.Add(num10);
            //                                }
            //                            }
            //                        }
            //                        num8 = list4[GuiRandom.Range(0, list4.Count)];
            //                    }
            //                    else if (num6 >= 7)
            //                    {
            //                        num6 = Mathf.Clamp(num6 - GuiRandom.Range(0, num6 / 2), 1, 6);
            //                    }
            //                    else if (GuiRandom.Range(0, 100) < 60)
            //                    {
            //                        num7 = Mathf.Clamp(num7 - GuiRandom.Range(0, num7 / 2), 1, 6);
            //                    }
            //                    else
            //                    {
            //                        num6 = Mathf.Clamp(num6 - GuiRandom.Range(0, num6 / 2), 1, 6);
            //                    }
            //                    if (num8 == 0)
            //                    {
            //                        List<int> list5 = new List<int>();
            //                        for (int num11 = 0; num11 < list3.Count; num11++)
            //                        {
            //                            int num12 = list3[num11];
            //                            if (DateFile.instance.ParseInt(DateFile.instance.cricketDate[num12][3]) != 0 && DateFile.instance.ParseInt(DateFile.instance.cricketDate[num12][1]) == num6)
            //                            {
            //                                for (int num13 = 0; num13 < DateFile.instance.ParseInt(DateFile.instance.cricketDate[num12][6]); num13++)
            //                                {
            //                                    list5.Add(num12);
            //                                }
            //                            }
            //                        }
            //                        num9 = list5[GuiRandom.Range(0, list5.Count)];
            //                    }
            //                    if (num7 > 0)
            //                    {
            //                        List<int> list6 = new List<int>();
            //                        for (int num14 = 0; num14 < list3.Count; num14++)
            //                        {
            //                            int num15 = list3[num14];
            //                            if (DateFile.instance.ParseInt(DateFile.instance.cricketDate[num15][4]) == 1 && DateFile.instance.ParseInt(DateFile.instance.cricketDate[num15][1]) == num7)
            //                            {
            //                                for (int num16 = 0; num16 < DateFile.instance.ParseInt(DateFile.instance.cricketDate[num15][6]); num16++)
            //                                {
            //                                    list6.Add(num15);
            //                                }
            //                            }
            //                        }
            //                        partId = list6[GuiRandom.Range(0, list6.Count)];
            //                    }
            //                    GetQuquWindow.instance.MakeQuqu(enemyBodyId, (num8 > 0) ? num8 : num9, partId);
            //                }
            //            }
            //            enemyBodyImage.tag = "ActorItem";
            //            enemyBodyImage.name = "EnemyItemIcon," + enemyBodyId;
            //            enemyBodyImage.GetComponent<Image>().sprite = GetSprites.instance.itemSprites[DateFile.instance.ParseInt(DateFile.instance.GetItemDate(enemyBodyId, 98))];
            //            resourceButtonImage.sprite = GetSprites.instance.makeResourceIcon[DateFile.instance.ParseInt(DateFile.instance.resourceDate[5][98])];
            //            enemyBodyNameText.text = DateFile.instance.SetColoer(20001 + DateFile.instance.ParseInt(DateFile.instance.GetItemDate(enemyBodyId, 8)), DateFile.instance.GetItemDate(enemyBodyId, 0));
            //            enemyBodyWorth = DateFile.instance.ParseInt(DateFile.instance.GetItemDate(enemyBodyId, 904));
            //            break;
            //        case 2:
            //            if (bodyActorId == 0)
            //            {
            //                bodyActorId = DateFile.instance.MakeNewActor(DateFile.instance.ParseInt(DateFile.instance.cricketBattleDate[______ququBattleId][4]), true, -99);
            //            }
            //            enemyBodyImage.tag = "ShopBootyActor";
            //            enemyBodyImage.name = $"ActorIcon,{bodyActorId},{GuiRandom.Range(0, ActorMenu.instance.GetActorTalk(bodyActorId).Count)},{-999}";
            //            enemyBodyImage.GetComponent<Image>().sprite = GetSprites.instance.attSprites[0];
            //            enemyBodyNameText.text = DateFile.instance.SetColoer(10003, DateFile.instance.GetActorName(bodyActorId));
            //            enemyBodyWorth = DateFile.instance.GetActorWorth(bodyActorId);
            //            break;
            //    }
            //}
            #endregion

            actorBodyTyp = -1;
            setActorBodyId = 0;
            Main.Logger.Log("AAA777");
            UpdateActorBet(leftPlayer);
            UpdateActorBet(rightPlayer, 1);
            Main.Logger.Log("AAA999");
            actorQuquHp = new int[3];
            enemyQuquHp = new int[3];
            actorQuquSp = new int[3];
            enemyQuquSp = new int[3];
            MakeQuqu();
            //showQuquIndex = GuiRandom.Range(0, 3);
            //for (int num17 = 0; num17 < actorQuquHpText.Length; num17++)
            //{
            //    UpdateActorQuquValue(num17);
            //    UpdateEnemyQuquValue(num17);
            //    UpdateQuquHp(num17);
            //}
        }

        #region 设置敌人资源赌注
        ///// <summary>
        ///// 设置敌人资源赌注
        ///// </summary>
        ///// <param name="enemyId"></param>
        ///// <param name="baseBodySize"></param>
        //private void SetResourceBody(int enemyId, int baseBodySize)
        //{
        //    int[] array = ActorMenu.instance.ActorResource(enemyId);
        //    int num = 5;
        //    int num2 = array[num];
        //    for (int i = 0; i < 6; i++)
        //    {
        //        if (array[i] > num2)
        //        {
        //            num = i;
        //            num2 = array[i];
        //        }
        //    }
        //    enemyBodyId = num;
        //    ______enemyBodySize = num2 * baseBodySize / 100;
        //    enemyBodyImage.tag = "ResourceIcon";
        //    enemyBodyImage.name = "ResourceIcon," + enemyBodyId;
        //    enemyBodyImage.sprite = GetSprites.instance.makeResourceIcon[DateFile.instance.ParseInt(DateFile.instance.resourceDate[enemyBodyId][98])];
        //    resourceButtonImage.sprite = GetSprites.instance.makeResourceIcon[DateFile.instance.ParseInt(DateFile.instance.resourceDate[enemyBodyId][98])];
        //    enemyBodyNameText.text = DateFile.instance.resourceDate[enemyBodyId][1] + DateFile.instance.SetColoer((enemyBodyId == 6) ? 20007 : ((enemyBodyId == 5) ? 20008 : 20003), "×" + ______enemyBodySize);
        //    enemyBodyWorth = ______enemyBodySize;
        //}
        #endregion

        /// <summary>
        /// 制作蛐蛐
        /// </summary>
        private void MakeQuqu()
        {
            actorQuquId = new int[] { -99, -99, -99 };
            enemyQuquId = new int[] { -99, -99, -99 };




            // 以下是给敌人创建蛐蛐
            enemyQuquId = new int[3];
            int num = DateFile.instance.ParseInt(DateFile.instance.cricketBattleDate[______ququBattleId][2]); // 敌人等级 1 4 7 10 13 16 19 22 25 28
            int num2 = Mathf.Clamp(num / 3, 1, 9); // 1 - 9
            int[] array = new int[3] // 最大蛐蛐等级 蛐蛐的等级为敌人等级的三分之一
            {
            num2,
            num2,
            num2
            };
            int num3 = num % 3; // 
            bool flag = num > 27;
            if (!flag)
            {
                int num4 = GuiRandom.Range(0, 3);
                array[num4] = Mathf.Clamp(array[num4] + num3, 1, 9); // 敌人等级不大于27时 随机一个蛐蛐等级增加
            }
            List<int> list = new List<int>(DateFile.instance.cricketDate.Keys);
            for (int i = 0; i < 3; i++)
            {
                int ququ_level = array[i]; // 蛐蛐等级
                int ququ_level2 = array[i]; // 蛐蛐等级
                int make_color_id = 0; // 最后要生成的蛐蛐id
                int make_color_id2 = 0; // 虫王蛐蛐id
                int partId = 0;
                string[] array2 = DateFile.instance.cricketBattleDate[______ququBattleId][21 + i].Split('|');
                if (array2.Length == 2) // 这个是虫王
                {
                    make_color_id2 = DateFile.instance.ParseInt(array2[0]);
                    partId = DateFile.instance.ParseInt(array2[1]);
                }
                else
                {
                    if (ququ_level >= 8) // 蛐蛐等级大于8
                    {
                        ququ_level2 = 0;
                        List<int> list2 = new List<int>();
                        for (int j = 0; j < list.Count; j++)
                        {
                            int ququ_id = list[j]; // 蛐蛐id
                            if (DateFile.instance.ParseInt(DateFile.instance.cricketDate[ququ_id][1]) == ququ_level) // 查找到等级相符的蛐蛐
                            {
                                int num10 = flag ? DateFile.instance.ParseInt(DateFile.instance.cricketDate[ququ_id][1006]) : DateFile.instance.ParseInt(DateFile.instance.cricketDate[ququ_id][6]);
                                // 抽中的权重
                                for (int k = 0; k < num10; k++)
                                {
                                    list2.Add(ququ_id);
                                }
                            }
                        }
                        make_color_id = list2[GuiRandom.Range(0, list2.Count)];
                    }
                    else if (ququ_level >= 7)
                    {
                        ququ_level = Mathf.Clamp(ququ_level - GuiRandom.Range(0, ququ_level / 2), 1, 6); // num5减去最多一半
                    }
                    else if (GuiRandom.Range(0, 100) < 75)
                    {
                        ququ_level2 = Mathf.Clamp(ququ_level2 - GuiRandom.Range(0, ququ_level2 / 2), 1, 6); // num6减去最多一半
                    }
                    else  // num5减去最多一半
                    {
                        ququ_level = Mathf.Clamp(ququ_level - GuiRandom.Range(0, ququ_level / 2), 1, 6);
                    }
                    if (make_color_id == 0)
                    {
                        List<int> list3 = new List<int>();
                        for (int l = 0; l < list.Count; l++)
                        {
                            int num11 = list[l];
                            if (DateFile.instance.ParseInt(DateFile.instance.cricketDate[num11][3]) != 0 && DateFile.instance.ParseInt(DateFile.instance.cricketDate[num11][1]) == ququ_level)
                            {
                                for (int m = 0; m < DateFile.instance.ParseInt(DateFile.instance.cricketDate[num11][6]); m++)
                                {
                                    list3.Add(num11);
                                }
                            }
                        }
                        make_color_id2 = list3[GuiRandom.Range(0, list3.Count)];
                    }
                    if (ququ_level2 > 0)
                    {
                        List<int> list4 = new List<int>();
                        for (int n = 0; n < list.Count; n++)
                        {
                            int num12 = list[n];
                            if (DateFile.instance.ParseInt(DateFile.instance.cricketDate[num12][4]) == 1 && DateFile.instance.ParseInt(DateFile.instance.cricketDate[num12][1]) == ququ_level2)
                            {
                                for (int num13 = 0; num13 < DateFile.instance.ParseInt(DateFile.instance.cricketDate[num12][6]); num13++)
                                {
                                    list4.Add(num12);
                                }
                            }
                        }
                        partId = list4[GuiRandom.Range(0, list4.Count)];
                    }
                }
                enemyQuquId[i] = DateFile.instance.MakeNewItem(10000, -(i + 1));
                GetQuquWindow.instance.MakeQuqu(enemyQuquId[i], (make_color_id > 0) ? make_color_id : make_color_id2, partId);
            }
        }

        private void UpdateActorQuquValue(int index)
        {
            int ququ_id = actorQuquId[index];
            if (ququ_id >= 0)
            {
                actorBattleQuquNameText[index].text = DateFile.instance.SetColoer(20001 + DateFile.instance.ParseInt(DateFile.instance.GetItemDate(ququ_id, 8)), DateFile.instance.GetItemDate(ququ_id, 0));
                actorBattleQuquPower1Text[index].text = GetQuquWindow.instance.GetQuquDate(ququ_id, 21).ToString();
                actorBattleQuquPower2Text[index].text = GetQuquWindow.instance.GetQuquDate(ququ_id, 22).ToString();
                actorBattleQuquPower3Text[index].text = GetQuquWindow.instance.GetQuquDate(ququ_id, 23).ToString();
                actorQuquName[index].text = actorBattleQuquNameText[index].text;
                actorQuquHp[index] = GetQuquWindow.instance.GetQuquDate(ququ_id, 11);
                actorQuquSp[index] = GetQuquWindow.instance.GetQuquDate(ququ_id, 12);
                int num = DateFile.instance.ParseInt(DateFile.instance.GetItemDate(ququ_id, 901));
                int num2 = DateFile.instance.ParseInt(DateFile.instance.GetItemDate(ququ_id, 902));
                actorQuquHpText[index].text = $"{ActorMenu.instance.Color3(num, num2)}{num}</color>/{num2}";
            }
            else
            {
                actorQuquName[index].text = "";
                actorQuquHpText[index].text = "";
            }
            actorQuquIcon[index].sprite = ((ququ_id < 0) ? GetSprites.instance.itemSprites[0] : DateFile.instance.GetCricketImage(ququ_id));
            actorQuquIcon[index].name = "ActorQuqu," + ququ_id;
        }

        private void UpdateEnemyQuquValue(int index)
        {
            enemyBattleQuquNameText[index].text = DateFile.instance.SetColoer(20001 + DateFile.instance.ParseInt(DateFile.instance.GetItemDate(enemyQuquId[index], 8)), DateFile.instance.GetItemDate(enemyQuquId[index], 0));
            enemyBattleQuquPower1Text[index].text = GetQuquWindow.instance.GetQuquDate(enemyQuquId[index], 21).ToString();
            enemyBattleQuquPower2Text[index].text = GetQuquWindow.instance.GetQuquDate(enemyQuquId[index], 22).ToString();
            enemyBattleQuquPower3Text[index].text = GetQuquWindow.instance.GetQuquDate(enemyQuquId[index], 23).ToString();
            if (index != showQuquIndex)
            {
                enemyQuquName[index].text = DateFile.instance.SetColoer(20002, DateFile.instance.massageDate[3][2]);
                hideQuquImage[index].SetActive(value: true);
            }
            else
            {
                enemyQuquName[index].text = enemyBattleQuquNameText[index].text;
                hideQuquImage[index].SetActive(value: false);
            }
            enemyQuquHp[index] = GetQuquWindow.instance.GetQuquDate(enemyQuquId[index], 11);
            enemyQuquSp[index] = GetQuquWindow.instance.GetQuquDate(enemyQuquId[index], 12);
            enemyQuquIcon[index].sprite = DateFile.instance.GetCricketImage(enemyQuquId[index]);
            enemyQuquIcon[index].name = "EnemyQuqu," + enemyQuquId[index];
            int num = DateFile.instance.ParseInt(DateFile.instance.GetItemDate(enemyQuquId[index], 901));
            int num2 = DateFile.instance.ParseInt(DateFile.instance.GetItemDate(enemyQuquId[index], 902));
            enemyQuquHpText[index].text = $"{ActorMenu.instance.Color3(num, num2)}{num}</color>/{num2}";
        }

        private void UpdateQuquHp(int index)
        {
            int ququDate = GetQuquWindow.instance.GetQuquDate(actorQuquId[index], 11);
            int ququDate2 = GetQuquWindow.instance.GetQuquDate(actorQuquId[index], 12);
            actorQuquHp[index] = Mathf.Min(actorQuquHp[index], ququDate);
            actorQuquSp[index] = Mathf.Min(actorQuquSp[index], ququDate2);
            actorBattleQuquStrengthText[index].text = DateFile.instance.SetColoer((actorQuquHp[index] < ququDate * 50 / 100) ? 20010 : 20003, actorQuquHp[index].ToString());
            actorBattleQuquMagicText[index].text = DateFile.instance.SetColoer((actorQuquSp[index] < ququDate2 * 50 / 100) ? 20010 : 20003, actorQuquSp[index].ToString());
            actorBattleQuquStrengthBar[index].fillAmount = (float)actorQuquHp[index] / (float)ququDate;
            actorBattleQuquMagicBar[index].fillAmount = (float)actorQuquSp[index] / (float)ququDate2;
            actorBattleQuquPower1Text[index].text = GetQuquWindow.instance.GetQuquDate(actorQuquId[index], 21).ToString();
            actorBattleQuquPower2Text[index].text = GetQuquWindow.instance.GetQuquDate(actorQuquId[index], 22).ToString();
            actorBattleQuquPower3Text[index].text = GetQuquWindow.instance.GetQuquDate(actorQuquId[index], 23).ToString();
            int num = DateFile.instance.ParseInt(DateFile.instance.GetItemDate(actorQuquId[index], 901));
            int num2 = DateFile.instance.ParseInt(DateFile.instance.GetItemDate(actorQuquId[index], 902));
            actorQuquHpText[index].text = $"{ActorMenu.instance.Color3(num, num2)}{num}</color>/{num2}";
            int ququDate3 = GetQuquWindow.instance.GetQuquDate(enemyQuquId[index], 11);
            int ququDate4 = GetQuquWindow.instance.GetQuquDate(enemyQuquId[index], 12);
            enemyQuquHp[index] = Mathf.Min(enemyQuquHp[index], ququDate3);
            enemyQuquSp[index] = Mathf.Min(enemyQuquSp[index], ququDate4);
            enemyBattleQuquStrengthText[index].text = DateFile.instance.SetColoer((enemyQuquHp[index] < ququDate3 * 50 / 100) ? 20010 : 20003, enemyQuquHp[index].ToString());
            enemyBattleQuquMagicText[index].text = DateFile.instance.SetColoer((enemyQuquSp[index] < ququDate4 * 50 / 100) ? 20010 : 20003, enemyQuquSp[index].ToString());
            enemyBattleQuquStrengthBar[index].fillAmount = (float)enemyQuquHp[index] / (float)ququDate3;
            enemyBattleQuquMagicBar[index].fillAmount = (float)enemyQuquSp[index] / (float)ququDate4;
            enemyBattleQuquPower1Text[index].text = GetQuquWindow.instance.GetQuquDate(enemyQuquId[index], 21).ToString();
            enemyBattleQuquPower2Text[index].text = GetQuquWindow.instance.GetQuquDate(enemyQuquId[index], 22).ToString();
            enemyBattleQuquPower3Text[index].text = GetQuquWindow.instance.GetQuquDate(enemyQuquId[index], 23).ToString();
            int num3 = DateFile.instance.ParseInt(DateFile.instance.GetItemDate(enemyQuquId[index], 901));
            int num4 = DateFile.instance.ParseInt(DateFile.instance.GetItemDate(enemyQuquId[index], 902));
            enemyQuquHpText[index].text = $"{ActorMenu.instance.Color3(num3, num4)}{num3}</color>/{num4}";
        }

        public void ShowChooseBodyTyp()
        {
            if (chooseBodyIsShow)
            {
                CloseChooseBodyTyp();
            }
            else
            {
                int num = (enemyBodyTyp == 0) ? enemyBodyId : 5;
                int num2 = enemyBodyWorth;
                int num3 = ActorMenu.instance.ActorResource(DateFile.instance.MianActorID())[num];
                if (num3 >= num2)
                {
                    useResourceButton.interactable = true;
                    Text text = needResourceText;
                    string str = DateFile.instance.resourceDate[num][1];
                    DateFile dateFile = DateFile.instance;
                    int coloer;
                    switch (num)
                    {
                        default:
                            coloer = 20003;
                            break;
                        case 5:
                            coloer = 20008;
                            break;
                        case 6:
                            coloer = 20007;
                            break;
                    }
                    text.text = str + dateFile.SetColoer(coloer, "×" + enemyBodyWorth);
                }
                else
                {
                    useResourceButton.interactable = false;
                    needResourceText.text = DateFile.instance.resourceDate[num][1] + DateFile.instance.SetColoer(20010, "×" + enemyBodyWorth);
                }
                chooseBodyWindow.DOPlayForward();
                chooseBodyIsShow = true;
            }
        }

        public void CloseChooseBodyTyp()
        {
            chooseBodyWindow.DOPlayBackwards();
            chooseBodyIsShow = false;
        }

        /// <summary>
        /// 设置赌注
        /// </summary>
        private void UpdateActorBet(PlayerData player = null, int idx = 0)
        {
            if (null == player && player.bet > 0) // 设置空格子
            {
                if (idx == 0) // 左侧
                {
                    actorBodyId = -98;
                    actorBodyImage.tag = "ActorItem";
                    actorBodyImage.name = "ActorItemIcon," + actorBodyId;
                    actorBodyImage.sprite = GetItemIcon(actorBodyId);
                    actorBodyNameText.text = DateFile.instance.massageDate[8001][5].Split('|')[0];
                }
                else // 右侧
                {
                    enemyBodyId = -98;
                    enemyBodyImage.tag = "ActorItem";
                    enemyBodyImage.name = "ActorItemIcon," + enemyBodyId;
                    enemyBodyImage.sprite = GetItemIcon(enemyBodyId);
                    enemyBodyNameText.text = DateFile.instance.massageDate[8001][5].Split('|')[0];
                }
            }
            else
            {
                int id = player.bet;
                actorBodyTyp = -1;
                #region 原本设置主角赌注的代码
                //switch (actorBodyTyp)
                //{
                //    case 0:
                //        actorBodyImage.tag = "ResourceIcon"; // 金钱
                //        actorBodyImage.name = "ResourceIcon," + actorBodyId;
                //        actorBodyImage.sprite = GetSprites.instance.makeResourceIcon[DateFile.instance.ParseInt(DateFile.instance.resourceDate[actorBodyId][98])];
                //        actorBodyNameText.text = DateFile.instance.resourceDate[actorBodyId][1] + DateFile.instance.SetColoer((actorBodyId == 6) ? 20007 : ((actorBodyId == 5) ? 20008 : 20003), "×" + actorBodySize);
                //        break;
                //    case 1:
                //        actorBodyImage.tag = "ActorItem"; // 物品
                //        actorBodyImage.name = "ActorItemIcon," + actorBodyId;
                //        actorBodyImage.GetComponent<Image>().sprite = GetSprites.instance.itemSprites[DateFile.instance.ParseInt(DateFile.instance.GetItemDate(actorBodyId, 98))];
                //        actorBodyNameText.text = DateFile.instance.SetColoer(20001 + DateFile.instance.ParseInt(DateFile.instance.GetItemDate(actorBodyId, 8)), DateFile.instance.GetItemDate(actorBodyId, 0));
                //        break;
                //    case 2:
                //        actorBodyImage.tag = "ShopBootyActor"; // 人
                //        actorBodyImage.name = $"ActorIcon,{actorBodyId},{GuiRandom.Range(0, ActorMenu.instance.GetActorTalk(actorBodyId).Count)},{-999}";
                //        actorBodyImage.GetComponent<Image>().sprite = GetSprites.instance.attSprites[0];
                //        actorBodyNameText.text = DateFile.instance.SetColoer(10003, DateFile.instance.GetActorName(actorBodyId));
                //        break;
                //    default:
                //        actorBodyId = -98;
                //        actorBodyImage.tag = "ActorItem";
                //        actorBodyImage.name = "ActorItemIcon," + actorBodyId;
                //        actorBodyImage.sprite = GetItemIcon(actorBodyId);
                //        actorBodyNameText.text = DateFile.instance.massageDate[8001][5].Split('|')[0];
                //        break;
                //}
                #endregion

                if (idx == 0) // 左侧
                {
                    actorBodyId = id;
                    actorBodyImage.tag = "ActorItem"; // 物品
                    actorBodyImage.name = "ActorItemIcon," + actorBodyId;
                    actorBodyImage.GetComponent<Image>().sprite = GetSprites.instance.itemSprites[DateFile.instance.ParseInt(DateFile.instance.GetItemDate(actorBodyId, 98))];
                    actorBodyNameText.text = DateFile.instance.SetColoer(20001 + DateFile.instance.ParseInt(DateFile.instance.GetItemDate(actorBodyId, 8)), DateFile.instance.GetItemDate(actorBodyId, 0));
                }
                else
                {
                    enemyBodyId = id;
                    enemyBodyImage.tag = "ActorItem"; // 物品
                    enemyBodyImage.name = "EnemyItemIcon," + enemyBodyId;
                    enemyBodyImage.GetComponent<Image>().sprite = GetSprites.instance.itemSprites[DateFile.instance.ParseInt(DateFile.instance.GetItemDate(enemyBodyId, 98))];
                    enemyBodyNameText.text = DateFile.instance.SetColoer(20001 + DateFile.instance.ParseInt(DateFile.instance.GetItemDate(enemyBodyId, 8)), DateFile.instance.GetItemDate(actorBodyId, 0));
                }
            }
        }

        public void SetUseResourceButton()
        {
            actorBodyTyp = 0;
            actorBodyId = ((enemyBodyTyp != 0) ? 5 : enemyBodyId);
            actorBodySize = enemyBodyWorth;
            UpdateActorBet();
            CloseChooseBodyTyp();
            UpdateCanStartBattle();
        }

        public void SetUseItemButton()
        {
            getItemTyp = 1;
            GetItem();
            itemWindow.transform.DOPlayForward();
            Component[] componentsInChildren = itemWindow.GetComponentsInChildren<Component>();
            Component[] array = componentsInChildren;
            foreach (Component component in array)
            {
                if (component is Graphic)
                {
                    (component as Graphic).CrossFadeAlpha(1f, 0.2f, ignoreTimeScale: true);
                }
            }
            useItemButton.interactable = false;
            useItemButton.gameObject.SetActive(value: true);
            itemMask.SetActive(value: true);
            itemWindowIsShow = true;
            CloseChooseBodyTyp();
        }

        public void SetUseActorButton()
        {
            GetActor();
            acotrWindow.transform.DOPlayForward();
            Component[] componentsInChildren = acotrWindow.GetComponentsInChildren<Component>();
            Component[] array = componentsInChildren;
            foreach (Component component in array)
            {
                if (component is Graphic)
                {
                    (component as Graphic).CrossFadeAlpha(1f, 0.2f, ignoreTimeScale: true);
                }
            }
            useActorButton.interactable = false;
            useActorButton.gameObject.SetActive(value: true);
            itemMask.SetActive(value: true);
            CloseChooseBodyTyp();
        }

        public void CloseActorWindow()
        {
            acotrWindow.transform.DOPlayBackwards();
            Component[] componentsInChildren = acotrWindow.GetComponentsInChildren<Component>();
            Component[] array = componentsInChildren;
            foreach (Component component in array)
            {
                if (component is Graphic)
                {
                    (component as Graphic).CrossFadeAlpha(0f, 0.2f, ignoreTimeScale: true);
                }
            }
            useActorButton.gameObject.SetActive(value: false);
            itemMask.SetActive(value: false);
        }

        private void GetActor()
        {
            RemoveActor();
            int num = DateFile.instance.MianActorID();
            List<int> list = new List<int>();
            Dictionary<int, int> dictionary = new Dictionary<int, int>();
            List<int> list2 = new List<int>(DateFile.instance.GetFamily(getPrisoner: true));
            foreach (int item in list2)
            {
                if (item != num)
                {
                    dictionary.Add(item, DateFile.instance.GetActorWorth(item));
                }
            }
            List<KeyValuePair<int, int>> list3 = new List<KeyValuePair<int, int>>(dictionary);
            list3.Sort((KeyValuePair<int, int> s1, KeyValuePair<int, int> s2) => s2.Value.CompareTo(s1.Value));
            list.Add(num);
            foreach (KeyValuePair<int, int> item2 in list3)
            {
                list.Add(item2.Key);
            }
            for (int i = 0; i < list.Count; i++)
            {
                int num2 = list[i];
                int actorWorth = DateFile.instance.GetActorWorth(num2);
                if (num2 == num || actorWorth >= enemyBodyWorth)
                {
                    GameObject gameObject = Object.Instantiate(actorIcon, Vector3.zero, Quaternion.identity);
                    gameObject.name = "Actor," + num2;
                    gameObject.transform.SetParent(actorHolder, worldPositionStays: false);
                    gameObject.GetComponent<Toggle>().group = actorHolder.GetComponent<ToggleGroup>();
                    if (DateFile.instance.acotrTeamDate.Contains(num2))
                    {
                        gameObject.transform.Find("IsInTeamIcon").gameObject.SetActive(value: true);
                    }
                    gameObject.transform.Find("IsInBuildingIcon").gameObject.SetActive(DateFile.instance.ActorIsWorking(num2) != null);
                    int actorFavor = DateFile.instance.GetActorFavor(false, DateFile.instance.MianActorID(), num2);
                    gameObject.transform.Find("ListActorFavorText").GetComponent<Text>().text = ((num2 == num || actorFavor == -1) ? DateFile.instance.SetColoer(20002, DateFile.instance.massageDate[303][2]) : ActorMenu.instance.Color5(actorFavor));
                    gameObject.transform.Find("ListActorNameText").GetComponent<Text>().text = DateFile.instance.GetActorName(num2);
                    gameObject.transform.Find("SkillLevelText").GetComponent<Text>().text = ((num2 == num) ? DateFile.instance.massageDate[303][2] : actorWorth.ToString());
                    Transform transform = gameObject.transform.Find("ListActorFaceHolder").Find("FaceMask").Find("MianActorFace");
                    transform.GetComponent<ActorFace>().SetActorFace(num2);
                }
            }
        }

        private void RemoveActor()
        {
            for (int i = 0; i < actorHolder.childCount; i++)
            {
                Object.Destroy(actorHolder.GetChild(i).gameObject);
            }
        }

        public void SetActorId(int id)
        {
            setActorBodyId = id;
            useActorButton.interactable = true;
        }

        public void SetActorBody()
        {
            actorBodyTyp = 2;
            actorBodyId = setActorBodyId;
            actorBodySize = 1;
            UpdateActorBet();
            CloseActorWindow();
            UpdateCanStartBattle();
        }

        public void ChooseActorQuqu(int index)
        {
            choseItemIndex = index;
            getItemTyp = 0;
            GetItem();
            itemWindow.transform.DOPlayForward();
            Component[] componentsInChildren = itemWindow.GetComponentsInChildren<Component>();
            Component[] array = componentsInChildren;
            foreach (Component component in array)
            {
                if (component is Graphic)
                {
                    (component as Graphic).CrossFadeAlpha(1f, 0.2f, ignoreTimeScale: true);
                }
            }
            useItemButton.interactable = false;
            useItemButton.gameObject.SetActive(value: true);
            itemMask.SetActive(value: true);
            itemWindowIsShow = true;
        }

        public void CloseItemWindow()
        {
            itemWindow.transform.DOPlayBackwards();
            Component[] componentsInChildren = itemWindow.GetComponentsInChildren<Component>();
            Component[] array = componentsInChildren;
            foreach (Component component in array)
            {
                if (component is Graphic)
                {
                    (component as Graphic).CrossFadeAlpha(0f, 0.2f, ignoreTimeScale: true);
                }
            }
            useItemButton.gameObject.SetActive(value: false);
            itemMask.SetActive(value: false);
            itemWindowIsShow = false;
        }

        private void GetItem()
        {
            RemoveItems();
            int num = DateFile.instance.MianActorID();
            List<int> list = new List<int>(ActorMenu.instance.GetActorItems(num).Keys);
            int num2 = DateFile.instance.ParseInt(DateFile.instance.GetActorDate(num, 312, addValue: false));
            if (num2 > 0)
            {
                list.Add(num2);
            }
            for (int i = 0; i < list.Count; i++)
            {
                int num3 = list[i];
                bool flag = false;
                if (getItemTyp == 0)
                {
                    for (int j = 0; j < actorQuquId.Length; j++)
                    {
                        if (actorQuquId[j] == num3)
                        {
                            flag = true;
                        }
                    }
                }
                if ((getItemTyp == 0 && !flag && DateFile.instance.ParseInt(DateFile.instance.GetItemDate(num3, 2001)) == 1 && DateFile.instance.ParseInt(DateFile.instance.GetItemDate(num3, 901)) > 0) || (getItemTyp == 1 && DateFile.instance.ParseInt(DateFile.instance.GetItemDate(num3, 906)) == 1 && DateFile.instance.ParseInt(DateFile.instance.GetItemDate(num3, 904)) >= enemyBodyWorth && (DateFile.instance.ParseInt(DateFile.instance.GetItemDate(num3, 6)) == 1 || (DateFile.instance.ParseInt(DateFile.instance.GetItemDate(num3, 6)) == 0 && DateFile.instance.ParseInt(DateFile.instance.GetItemDate(num3, 901)) > 0))))
                {
                    GameObject gameObject = Object.Instantiate(ActorMenu.instance.itemIconNoDrag, Vector3.zero, Quaternion.identity);
                    gameObject.name = "Item," + num3;
                    gameObject.transform.SetParent(itemHolder, worldPositionStays: false);
                    gameObject.GetComponent<Toggle>().group = itemHolder.GetComponent<ToggleGroup>();
                    Image component = gameObject.transform.Find("ItemBack").GetComponent<Image>();
                    component.sprite = GetSprites.instance.itemBackSprites[DateFile.instance.ParseInt(DateFile.instance.GetItemDate(num3, 4))];
                    component.color = ActorMenu.instance.LevelColor(DateFile.instance.ParseInt(DateFile.instance.GetItemDate(num3, 8)));
                    if (DateFile.instance.ParseInt(DateFile.instance.GetItemDate(num3, 6)) > 0)
                    {
                        gameObject.transform.Find("ItemNumberText").GetComponent<Text>().text = "×" + DateFile.instance.GetItemNumber(num, num3);
                    }
                    else
                    {
                        int num4 = DateFile.instance.ParseInt(DateFile.instance.GetItemDate(num3, 901));
                        int num5 = DateFile.instance.ParseInt(DateFile.instance.GetItemDate(num3, 902));
                        gameObject.transform.Find("ItemNumberText").GetComponent<Text>().text = $"{ActorMenu.instance.Color3(num4, num5)}{num4}</color>/{num5}";
                    }
                    GameObject gameObject2 = gameObject.transform.Find("ItemIcon").gameObject;
                    gameObject2.name = "ItemIcon," + num3;
                    gameObject2.GetComponent<Image>().sprite = GetSprites.instance.itemSprites[DateFile.instance.ParseInt(DateFile.instance.GetItemDate(num3, 98))];
                }
            }
        }

        private void RemoveItems()
        {
            for (int i = 0; i < itemHolder.childCount; i++)
            {
                Object.Destroy(itemHolder.GetChild(i).gameObject);
            }
        }

        public void SetItem()
        {
            if (getItemTyp == 0)
            {
                actorQuquId[choseItemIndex] = ActorMenu.instance.choseItemId;
                UpdateActorQuquValue(choseItemIndex);
                CloseItemWindow();
                UpdateCanStartBattle();
            }
            else
            {
                actorBodyTyp = 1;
                actorBodyId = ActorMenu.instance.choseItemId;
                actorBodySize = 1;
                UpdateActorBet();
                CloseItemWindow();
                UpdateCanStartBattle();
            }
        }

        public void RemoveItem()
        {
            actorQuquId[choseItemIndex] = -99;
            UpdateActorQuquValue(choseItemIndex);
            CloseItemWindow();
            UpdateCanStartBattle();
        }

        private Sprite GetItemIcon(int id)
        {
            if (id < 0)
            {
                return GetSprites.instance.itemSprites[0];
            }
            return GetSprites.instance.itemSprites[DateFile.instance.ParseInt(DateFile.instance.GetItemDate(id, 98))];
        }

        private void UpdateCanStartBattle()
        {
            bool flag = true;
            for (int i = 0; i < actorQuquId.Length; i++)
            {
                if (actorQuquId[i] < 0)
                {
                    flag = false;
                }
            }
            startBattleButton.interactable = (actorBodyTyp >= 0 && flag);
        }

        public void StartBattleButton()
        {
            int key = (actorBodyTyp == 2 && actorBodyId == DateFile.instance.MianActorID()) ? 3 : actorBodyTyp;
            YesOrNoWindow.instance.SetYesOrNoWindow(515, DateFile.instance.massageDate[8002][key].Split('|')[0], DateFile.instance.massageDate[8002][key].Split('|')[1]);
        }

        private bool ShowHideQuquImage(int index)
        {
            if (hideQuquImage[index].activeSelf)
            {
                hideQuquImage[index].GetComponent<RectTransform>().DOScale(new Vector3(2f, 2f, 1f), 0.6f).SetEase(Ease.InBack)
                    .OnComplete(delegate
                    {
                        enemyQuquName[index].text = enemyBattleQuquNameText[index].text;
                        hideQuquImage[index].SetActive(value: false);
                    });
                Component[] componentsInChildren = hideQuquImage[index].GetComponentsInChildren<Component>();
                Component[] array = componentsInChildren;
                foreach (Component component in array)
                {
                    if (component is Graphic)
                    {
                        (component as Graphic).CrossFadeAlpha(0f, 0.6f, ignoreTimeScale: false);
                    }
                }
                return true;
            }
            return false;
        }

        public void StartQuquBattle()
        {
            winTurn = 0;
            ququBattlePart = 2;
            startBattleWindow.SetActive(value: false);
            setBattleSpeedHolder.SetActive(value: true);
            for (int i = 0; i < 3; i++)
            {
                Component[] componentsInChildren = ququBattleBack[i].GetComponentsInChildren<Component>();
                Component[] array = componentsInChildren;
                foreach (Component component in array)
                {
                    if (component is Graphic)
                    {
                        (component as Graphic).CrossFadeAlpha(1f, 0.5f, ignoreTimeScale: true);
                    }
                }
            }
            for (int k = 0; k < battleValue.Length; k++)
            {
                nextButtonMask[k].GetComponent<Image>().DOColor(new Color(0f, 0f, 0f, 0.5f), 0.5f);
            }
            for (int l = 0; l < actorQuquIcon.Length; l++)
            {
                actorQuquIcon[l].GetComponent<Button>().interactable = false;
            }
            actorBodyImage.GetComponent<Button>().interactable = false;
            if (actorBodyTyp == 2 && actorBodyId != DateFile.instance.MianActorID())
            {
                DateFile.instance.ChangeFavor(actorBodyId, -DateFile.instance.ParseInt(DateFile.instance.GetActorDate(actorBodyId, 3, addValue: false)), updateActor: false, showMassage: false);
            }
            UpdateQuquHp(0);
            UpdateQuquHp(1);
            UpdateQuquHp(2);
            ShowStartBattleState();
        }

        public void ShowStartBattleState()
        {
            int stateIndex = 0;
            int num = DateFile.instance.ParseInt(DateFile.instance.GetItemDate(actorQuquId[ququBattleTurn], 8));
            int num2 = DateFile.instance.ParseInt(DateFile.instance.GetItemDate(enemyQuquId[ququBattleTurn], 8));
            int num3 = DateFile.instance.ParseInt(DateFile.instance.GetItemDate(actorQuquId[ququBattleTurn], 2002));
            int num4 = DateFile.instance.ParseInt(DateFile.instance.GetItemDate(enemyQuquId[ququBattleTurn], 2002));
            if ((num3 != 0 && num4 == 0) || (num > num2 && num - num2 >= 6 && GuiRandom.Range(0, 100) < (num - num2) * 10))
            {
                stateIndex = 1;
            }
            else if ((num4 != 0 && num3 == 0) || (num2 > num && num2 - num >= 6 && GuiRandom.Range(0, 100) < (num2 - num) * 10))
            {
                stateIndex = 2;
            }
            else if (num3 == 0 && num4 == 0)
            {
                stateIndex = 1 + GuiRandom.Range(0, 2);
            }
            StartCoroutine(StartBattle(ShowHideQuquImage(ququBattleTurn) ? 1 : 0, stateIndex));
        }

        private IEnumerator StartBattle(float waitTime, int stateIndex)
        {
            yield return new WaitForSeconds(waitTime);
            float _delay5 = 1f;
            SetBattleStateText(DateFile.instance.massageDate[8003][0].Split('|')[ququBattleTurn], _delay5, 2f);
            _delay5 += 1f;
            SetBattleStateText(DateFile.instance.massageDate[8003][1].Split('|')[ququBattleTurn], _delay5, 2f);
            _delay5 += 1f;
            actorQuqu[ququBattleTurn].DOLocalJump(new Vector3(280f, 0f, 0f), 20f, 1, 0.1f).SetDelay(_delay5).OnComplete(delegate
            {
                actorQuquName[ququBattleTurn].text = "";
                battleValue[ququBattleTurn].DOLocalMoveY(0f, 0.1f);
                actorQuqu[ququBattleTurn].DOLocalJump(new Vector3(320f, 0f, 0f), 10f, 1, 0.05f).SetDelay(0.2f).OnComplete(delegate
                {
                    SetBattleStateText(DateFile.instance.massageDate[8003][2].Split('|')[0], 0f, 2f);
                    if (stateIndex == 0 || stateIndex == 1)
                    {
                        actorQuqu[ququBattleTurn].GetComponent<QuquPlace>().UpdateBattleQuquCall(actorQuquId[ququBattleTurn]);
                        actorQuqu[ququBattleTurn].GetComponent<QuquPlace>().CallvolumeRest(0.8f, 0.2f);
                    }
                });
            });
            enemyQuqu[ququBattleTurn].DOLocalJump(new Vector3(-280f, 0f, 0f), 20f, 1, 0.1f).SetDelay(_delay5).OnComplete(delegate
            {
                enemyQuquName[ququBattleTurn].text = "";
                enemyQuqu[ququBattleTurn].DOLocalJump(new Vector3(-320f, 0f, 0f), 10f, 1, 0.05f).SetDelay(0.2f).OnComplete(delegate
                {
                    if (stateIndex == 0 || stateIndex == 2)
                    {
                        enemyQuqu[ququBattleTurn].GetComponent<QuquPlace>().UpdateBattleQuquCall(enemyQuquId[ququBattleTurn]);
                        enemyQuqu[ququBattleTurn].GetComponent<QuquPlace>().CallvolumeRest(0.8f, 0.2f);
                    }
                });
            });
            _delay5 += 2f;
            SetBattleStateText(DateFile.instance.massageDate[8003][3].Split('|')[stateIndex], _delay5, 2f);
            _delay5 += 1.5f;
            switch (stateIndex)
            {
                case 0:
                    SetBattleStateText(DateFile.instance.SetColoer(20008, DateFile.instance.massageDate[8003][2].Split('|')[1]), _delay5, 2f, 0);
                    break;
                case 1:
                    SetBattleStateText(DateFile.instance.SetColoer(20005, DateFile.instance.massageDate[8003][4].Split('|')[0]), _delay5, 3f, 1);
                    break;
                case 2:
                    SetBattleStateText(DateFile.instance.SetColoer(20010, DateFile.instance.massageDate[8003][4].Split('|')[1]), _delay5, 3f, 2);
                    break;
            }
        }

        private void SetBattleStateText(string text, float delay, float size, int endTyp = -1)
        {
            battleStateText[ququBattleTurn].transform.DOScale(new Vector3(size, size, 1f), 0.1f).SetDelay(delay).SetEase(Ease.OutBack)
                .OnStart(delegate
                {
                    battleStateText[ququBattleTurn].text = text;
                    battleStateText[ququBattleTurn].transform.localScale = new Vector3(0f, 0f, 1f);
                });
            battleStateText[ququBattleTurn].transform.DOScale(new Vector3(size / 2f, size / 2f, 1f), 0.4f).SetDelay(0.1f + delay).SetEase(Ease.OutBack);
            StartCoroutine(BattleState(endTyp, 0.5f + delay));
        }

        private IEnumerator BattleState(int endTyp, float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            if (endTyp == 0)
            {
                battleStateText[ququBattleTurn].transform.DOScale(new Vector3(0f, 0f, 1f), 0.1f).SetDelay(0.8f).OnComplete(delegate
                {
                    nextButtonMask[ququBattleTurn].GetComponent<Image>().DOColor(new Color(0f, 0f, 0f, 0f), 0.2f);
                    QuquBattleLoopStart(0.1f, 0.2f);
                });
            }
            else if (endTyp >= 1)
            {
                if (endTyp == 1)
                {
                    winTurn++;
                    actorQuqu[ququBattleTurn].GetComponent<QuquPlace>().UpdateBattleQuquCall(actorQuquId[ququBattleTurn]);
                    actorQuqu[ququBattleTurn].GetComponent<QuquPlace>().CallvolumeRest(1.6f, 0.6f);
                    AddQuquBattleMassage(win: true);
                }
                else
                {
                    enemyQuqu[ququBattleTurn].GetComponent<QuquPlace>().UpdateBattleQuquCall(enemyQuquId[ququBattleTurn]);
                    enemyQuqu[ququBattleTurn].GetComponent<QuquPlace>().CallvolumeRest(1.6f, 0.6f);
                    AddQuquBattleMassage(win: false);
                }
                battleEndTyp = endTyp;
                for (int i = 0; i < nextButton.Length; i++)
                {
                    nextButton[i].gameObject.SetActive(i == ququBattleTurn);
                }
            }
        }

        public void BattleNextPart()
        {
            nextButton[ququBattleTurn].gameObject.SetActive(value: false);
            if (battleEndTyp == 1)
            {
                if (winTurn >= 2)
                {
                    BattleEndWindow(0);
                    return;
                }
            }
            else if (ququBattleTurn == 1 && winTurn == 0)
            {
                BattleEndWindow(1);
                return;
            }
            if (ququBattleTurn < 2)
            {
                actorQuqu[ququBattleTurn].DOKill();
                enemyQuqu[ququBattleTurn].DOKill();
                ququBattleTurn++;
                ShowStartBattleState();
            }
            else if (winTurn >= 2)
            {
                BattleEndWindow(0);
            }
            else
            {
                BattleEndWindow(1);
            }
        }

        private bool QuquIsDead()
        {
            bool flag = actorQuquHp[ququBattleTurn] <= 0 || actorQuquSp[ququBattleTurn] <= 0 || DateFile.instance.ParseInt(DateFile.instance.GetItemDate(actorQuquId[ququBattleTurn], 901)) <= 0;
            bool flag2 = enemyQuquHp[ququBattleTurn] <= 0 || enemyQuquSp[ququBattleTurn] <= 0 || DateFile.instance.ParseInt(DateFile.instance.GetItemDate(enemyQuquId[ququBattleTurn], 901)) <= 0;
            return flag | flag2;
        }

        private void AddQuquBattleMassage(bool win)
        {
            int num = actorQuquId[ququBattleTurn];
            int id = enemyQuquId[ququBattleTurn];
            if (win)
            {
                int num2 = DateFile.instance.ParseInt(DateFile.instance.GetItemDate(num, 8));
                int num3 = DateFile.instance.ParseInt(DateFile.instance.GetItemDate(id, 8));
                if (num3 + 3 >= num2)
                {
                    DateFile.instance.itemsDate[num][2006] = (DateFile.instance.ParseInt(DateFile.instance.GetItemDate(num, 2006)) + 1).ToString();
                    DateFile.instance.AddActorScore(502, num3 * 100);
                    int num4 = DateFile.instance.ParseInt(DateFile.instance.GetItemDate(num, 2010));
                    int key = DateFile.instance.ParseInt(DateFile.instance.GetItemDate(num, 2011));
                    int num5 = (num4 != 0) ? ((DateFile.instance.ParseInt(DateFile.instance.cricketDate[num4][1]) >= DateFile.instance.ParseInt(DateFile.instance.cricketDate[key][1])) ? DateFile.instance.ParseInt(DateFile.instance.cricketDate[num4][1]) : DateFile.instance.ParseInt(DateFile.instance.cricketDate[key][1])) : 0;
                    if (num3 >= num5)
                    {
                        DateFile.instance.itemsDate[num][2010] = DateFile.instance.GetItemDate(id, 2002);
                        DateFile.instance.itemsDate[num][2011] = DateFile.instance.GetItemDate(id, 2003);
                    }
                }
            }
            else
            {
                DateFile.instance.itemsDate[num][2009] = (DateFile.instance.ParseInt(DateFile.instance.GetItemDate(num, 2009)) + 1).ToString();
            }
        }

        private void QuquBattleLoopStart(float jumpSpeed, float delay)
        {
            actorQuqu[ququBattleTurn].DOLocalJump(new Vector3(450f, 0f, 0f), 20f, 1, jumpSpeed).SetDelay(delay);
            enemyQuqu[ququBattleTurn].DOLocalJump(new Vector3(-450f, 0f, 0f), 20f, 1, jumpSpeed).SetDelay(delay).OnComplete(delegate
            {
                actorQuqu[ququBattleTurn].GetComponent<QuquPlace>().UpdateBattleQuquCall(actorQuquId[ququBattleTurn]);
                actorQuqu[ququBattleTurn].GetComponent<QuquPlace>().CallvolumeRest(0.8f, 0.2f);
                enemyQuqu[ququBattleTurn].GetComponent<QuquPlace>().UpdateBattleQuquCall(enemyQuquId[ququBattleTurn]);
                enemyQuqu[ququBattleTurn].GetComponent<QuquPlace>().CallvolumeRest(0.8f, 0.2f);
                int ququDate = GetQuquWindow.instance.GetQuquDate(actorQuquId[ququBattleTurn], 21);
                int ququDate2 = GetQuquWindow.instance.GetQuquDate(enemyQuquId[ququBattleTurn], 21);
                int num = 0;
                if (ququDate > ququDate2)
                {
                    ShowDamage(true, false, 2, ququDate, 0.6f, false, false, false, false, false, 0, 0);
                    num = ((GuiRandom.Range(0, 100) < 80) ? 1 : 2);
                }
                else if (ququDate2 > ququDate)
                {
                    ShowDamage(false, true, 2, ququDate2, 0.6f, false, false, false, false, false, 0, 0);
                    num = ((GuiRandom.Range(0, 100) >= 80) ? 1 : 2);
                }
                else
                {
                    num = 1 + GuiRandom.Range(0, 2);
                }
                if (!QuquIsDead())
                {
                    QuquAttack(1f, num, newTurn: false);
                }
            });
        }

        private void QuquAttack(float baseDelay, int attacker, bool newTurn)
        {
            actorQuqu[ququBattleTurn].DOLocalJump(new Vector3(330f, 0f, 0f), 20f, 1, (attacker == 1) ? 0.02f : 0.01f).SetDelay(1f + baseDelay).OnComplete(delegate
            {
                if (attacker == 1)
                {
                    QuquBaseAttacke(1, newTurn);
                }
            });
            enemyQuqu[ququBattleTurn].DOLocalJump(new Vector3(-330f, 0f, 0f), 20f, 1, (attacker == 1) ? 0.01f : 0.02f).SetDelay(1f + baseDelay).OnComplete(delegate
            {
                if (attacker == 2)
                {
                    QuquBaseAttacke(2, newTurn);
                }
            });
        }

        private void QuquBaseAttacke(int attacker, bool newTurn)
        {
            bool _cHit = false;
            bool _def = false;
            bool _reAttack = false;
            float num = 0.4f;
            if (attacker == 1)
            {
                int _actorQuquDamage = GetQuquWindow.instance.GetQuquDate(actorQuquId[ququBattleTurn], 23);
                if (GuiRandom.Range(0, 100) < GetQuquWindow.instance.GetQuquDate(actorQuquId[ququBattleTurn], 31))
                {
                    _cHit = true;
                    _actorQuquDamage += GetQuquWindow.instance.GetQuquDate(actorQuquId[ququBattleTurn], 32);
                }
                if (GuiRandom.Range(0, 100) < GetQuquWindow.instance.GetQuquDate(enemyQuquId[ququBattleTurn], 33))
                {
                    _def = true;
                    _actorQuquDamage = Mathf.Max(0, _actorQuquDamage - GetQuquWindow.instance.GetQuquDate(enemyQuquId[ququBattleTurn], 34));
                }
                if (GuiRandom.Range(0, 100) < GetQuquWindow.instance.GetQuquDate(enemyQuquId[ququBattleTurn], 35))
                {
                    num += 0.4f;
                    _reAttack = true;
                }
                actorQuqu[ququBattleTurn].DOLocalJump(new Vector3(605f, 0f, 0f), 20f, 1, 0.01f).SetDelay(num).OnComplete(delegate
                {
                    ShowDamage(true, false, 1, _actorQuquDamage, 0.1f, _cHit, _def, _reAttack, false, newTurn, 0, 22);
                });
            }
            else
            {
                int _enemyQuquDamage = GetQuquWindow.instance.GetQuquDate(enemyQuquId[ququBattleTurn], 23);
                if (GuiRandom.Range(0, 100) < GetQuquWindow.instance.GetQuquDate(enemyQuquId[ququBattleTurn], 31))
                {
                    _cHit = true;
                    _enemyQuquDamage += GetQuquWindow.instance.GetQuquDate(enemyQuquId[ququBattleTurn], 32);
                }
                if (GuiRandom.Range(0, 100) < GetQuquWindow.instance.GetQuquDate(actorQuquId[ququBattleTurn], 33))
                {
                    _def = true;
                    _enemyQuquDamage = Mathf.Max(0, _enemyQuquDamage - GetQuquWindow.instance.GetQuquDate(actorQuquId[ququBattleTurn], 34));
                }
                if (GuiRandom.Range(0, 100) < GetQuquWindow.instance.GetQuquDate(actorQuquId[ququBattleTurn], 35))
                {
                    num += 0.4f;
                    _reAttack = true;
                }
                enemyQuqu[ququBattleTurn].DOLocalJump(new Vector3(-605f, 0f, 0f), 20f, 1, 0.01f).SetDelay(num).OnComplete(delegate
                {
                    ShowDamage(false, true, 1, _enemyQuquDamage, 0.1f, _cHit, _def, _reAttack, false, newTurn, 0, 22);
                });
            }
        }

        private void ShowDamage(bool attacker, bool defer, int typ, int damage, float delay, bool cHit, bool def, bool reAttack, bool isReAttack, bool newTurn, int reAttackTutn, int reAttackTyp)
        {
            bool _cHit3 = false;
            bool _def3 = false;
            float size = cHit ? 2.4f : 1.8f;
            float num = delay;
            string text = "";
            Color textColor = new Color(1f, 1f, 1f, 1f);
            switch (typ)
            {
                case 1:
                    if (defer)
                    {
                        int num2 = 0;
                        if (cHit | isReAttack)
                        {
                            num2 = GetQuquWindow.instance.GetQuquDate(enemyQuquId[ququBattleTurn], 21);
                        }
                        if (def)
                        {
                            num2 = Mathf.Max(0, num2 - GetQuquWindow.instance.GetQuquDate(actorQuquId[ququBattleTurn], 34));
                            textColor = new Color(1f, 0.784313738f, 0f, 1f);
                        }
                        else if (cHit)
                        {
                            enemyQuqu[ququBattleTurn].GetComponent<QuquPlace>().UpdateBattleQuquCall(enemyQuquId[ququBattleTurn]);
                            enemyQuqu[ququBattleTurn].GetComponent<QuquPlace>().CallvolumeRest(0.8f, 0.2f);
                            int num3 = GetQuquWindow.instance.GetQuquDate(enemyQuquId[ququBattleTurn], 31) + GetQuquWindow.instance.GetQuquDate(enemyQuquId[ququBattleTurn], 36);
                            if (GuiRandom.Range(0, 100) < num3)
                            {
                                DateFile.instance.ChangeItemHp(DateFile.instance.MianActorID(), actorQuquId[ququBattleTurn], -1);
                                if (GuiRandom.Range(0, 100) < num3)
                                {
                                    GetQuquWindow.instance.QuquAddInjurys(actorQuquId[ququBattleTurn]);
                                }
                            }
                            textColor = new Color(1f, 0f, 0f, 1f);
                        }
                        actorQuquHp[ququBattleTurn] -= damage;
                        text = $"{DateFile.instance.massageDate[8004][0].Split('|')[0]}-{damage}";
                        if (num2 > 0)
                        {
                            actorQuquSp[ququBattleTurn] -= num2;
                            text += $"\n{DateFile.instance.massageDate[8004][0].Split('|')[1]}-{num2}";
                        }
                        if (QuquIsDead())
                        {
                            actorQuqu[ququBattleTurn].DOKill();
                            enemyQuqu[ququBattleTurn].DOKill();
                            battleStateText[ququBattleTurn].text = "";
                            nextButtonMask[ququBattleTurn].GetComponent<Image>().DOColor(new Color(0f, 0f, 0f, 0.5f), 1f).OnComplete(delegate
                            {
                                SetBattleStateText(DateFile.instance.SetColoer(20010, DateFile.instance.massageDate[8003][4].Split('|')[1]), 0.4f, 3f, 2);
                            });
                            Damage(defer, size, delay, text, textColor);
                            return;
                        }
                        if (def)
                        {
                            actorQuqu[ququBattleTurn].GetComponent<QuquPlace>().UpdateBattleQuquCall(actorQuquId[ququBattleTurn]);
                            actorQuqu[ququBattleTurn].GetComponent<QuquPlace>().CallvolumeRest(0.8f, 0.2f);
                            actorQuqu[ququBattleTurn].DOLocalJump(new Vector3(actorQuqu[ququBattleTurn].localPosition.x - 20f, 0f, 0f), 20f, 1, 0.01f).SetDelay(delay).OnComplete(delegate
                            {
                                actorQuqu[ququBattleTurn].DOLocalJump(new Vector3(actorQuqu[ququBattleTurn].localPosition.x + 20f, 0f, 0f), 20f, 1, 0.01f).SetDelay(0.4f);
                            });
                            num += 0.6f;
                        }
                        if (reAttack)
                        {
                            int _enemyQuquDamage = GetQuquWindow.instance.GetQuquDate(actorQuquId[ququBattleTurn], reAttackTyp);
                            if (GuiRandom.Range(0, 100) < GetQuquWindow.instance.GetQuquDate(actorQuquId[ququBattleTurn], 31))
                            {
                                bool _cHit = true;
                                _enemyQuquDamage += GetQuquWindow.instance.GetQuquDate(actorQuquId[ququBattleTurn], 32);
                            }
                            if (GuiRandom.Range(0, 100) < GetQuquWindow.instance.GetQuquDate(enemyQuquId[ququBattleTurn], 33))
                            {
                                bool _def = true;
                                _enemyQuquDamage = Mathf.Max(0, _enemyQuquDamage - GetQuquWindow.instance.GetQuquDate(enemyQuquId[ququBattleTurn], 34));
                            }
                            actorQuqu[ququBattleTurn].DOLocalJump(new Vector3(actorQuqu[ququBattleTurn].localPosition.x + 20f, 0f, 0f), 20f, 1, 0.01f).SetDelay(num + 0.6f).OnComplete(delegate
                            {
                                bool flag2 = false;
                                int num8 = GetQuquWindow.instance.GetQuquDate(enemyQuquId[ququBattleTurn], 35) - reAttackTutn * 5;
                                if (GuiRandom.Range(0, 100) < num8)
                                {
                                    flag2 = true;
                                }
                                GuiQuquBattleSystem ququBattleSystem2 = this;
                                bool attacker3 = attacker;
                                int damage3 = _enemyQuquDamage;
                                bool cHit3 = _cHit3;
                                bool def3 = _def3;
                                bool reAttack3 = flag2;
                                int num9 = reAttackTutn;
                                reAttackTutn = num9 + 1;
                                ququBattleSystem2.ShowDamage(attacker3, false, 1, damage3, 0.1f, cHit3, def3, reAttack3, true, false, num9, (reAttackTyp == 22) ? 23 : 22);
                                if (!QuquIsDead())
                                {
                                    actorQuqu[ququBattleTurn].DOLocalJump(new Vector3(actorQuqu[ququBattleTurn].localPosition.x - 20f, 0f, 0f), 20f, 1, 0.01f).SetDelay(0.4f);
                                }
                            });
                        }
                        else if (attacker)
                        {
                            actorQuqu[ququBattleTurn].DOLocalJump(new Vector3(330f, 0f, 0f), 20f, 1, 0.01f).SetDelay(num + 1.4f).OnComplete(delegate
                            {
                                if (newTurn)
                                {
                                    QuquBattleLoopStart(0.02f, 0.6f);
                                }
                                else
                                {
                                    QuquBaseAttacke(2, newTurn: true);
                                }
                            });
                        }
                        else
                        {
                            enemyQuqu[ququBattleTurn].DOLocalJump(new Vector3(-330f, 0f, 0f), 20f, 1, 0.01f).SetDelay(num + 1.4f).OnComplete(delegate
                            {
                                if (newTurn)
                                {
                                    QuquBattleLoopStart(0.02f, 0.6f);
                                }
                                else
                                {
                                    QuquBaseAttacke(1, newTurn: true);
                                }
                            });
                        }
                    }
                    else
                    {
                        int num4 = 0;
                        if (cHit | isReAttack)
                        {
                            num4 = GetQuquWindow.instance.GetQuquDate(actorQuquId[ququBattleTurn], 21);
                        }
                        if (def)
                        {
                            num4 = Mathf.Max(0, num4 - GetQuquWindow.instance.GetQuquDate(enemyQuquId[ququBattleTurn], 34));
                            textColor = new Color(1f, 0.784313738f, 0f, 1f);
                        }
                        else if (cHit)
                        {
                            actorQuqu[ququBattleTurn].GetComponent<QuquPlace>().UpdateBattleQuquCall(actorQuquId[ququBattleTurn]);
                            actorQuqu[ququBattleTurn].GetComponent<QuquPlace>().CallvolumeRest(0.8f, 0.2f);
                            int num5 = GetQuquWindow.instance.GetQuquDate(actorQuquId[ququBattleTurn], 31) + GetQuquWindow.instance.GetQuquDate(actorQuquId[ququBattleTurn], 36);
                            if (GuiRandom.Range(0, 100) < num5)
                            {
                                DateFile.instance.ChangeItemHp(0, enemyQuquId[ququBattleTurn], -1);
                                if (GuiRandom.Range(0, 100) < num5)
                                {
                                    GetQuquWindow.instance.QuquAddInjurys(enemyQuquId[ququBattleTurn]);
                                }
                            }
                            textColor = new Color(1f, 0f, 0f, 1f);
                        }
                        enemyQuquHp[ququBattleTurn] -= damage;
                        text = $"{DateFile.instance.massageDate[8004][0].Split('|')[0]}-{damage}";
                        if (num4 > 0)
                        {
                            enemyQuquSp[ququBattleTurn] -= num4;
                            text += $"\n{DateFile.instance.massageDate[8004][0].Split('|')[1]}-{num4}";
                        }
                        if (QuquIsDead())
                        {
                            actorQuqu[ququBattleTurn].DOKill();
                            enemyQuqu[ququBattleTurn].DOKill();
                            battleStateText[ququBattleTurn].text = "";
                            nextButtonMask[ququBattleTurn].GetComponent<Image>().DOColor(new Color(0f, 0f, 0f, 0.5f), 1f).OnComplete(delegate
                            {
                                SetBattleStateText(DateFile.instance.SetColoer(20005, DateFile.instance.massageDate[8003][4].Split('|')[0]), 0.4f, 3f, 1);
                            });
                            Damage(defer, size, delay, text, textColor);
                            return;
                        }
                        if (def)
                        {
                            enemyQuqu[ququBattleTurn].GetComponent<QuquPlace>().UpdateBattleQuquCall(enemyQuquId[ququBattleTurn]);
                            enemyQuqu[ququBattleTurn].GetComponent<QuquPlace>().CallvolumeRest(0.8f, 0.2f);
                            enemyQuqu[ququBattleTurn].DOLocalJump(new Vector3(enemyQuqu[ququBattleTurn].localPosition.x + 20f, 0f, 0f), 20f, 1, 0.01f).SetDelay(delay).OnComplete(delegate
                            {
                                enemyQuqu[ququBattleTurn].DOLocalJump(new Vector3(enemyQuqu[ququBattleTurn].localPosition.x - 20f, 0f, 0f), 20f, 1, 0.01f).SetDelay(0.4f);
                            });
                            num += 0.6f;
                        }
                        if (reAttack)
                        {
                            int _actorQuquDamage = GetQuquWindow.instance.GetQuquDate(enemyQuquId[ququBattleTurn], reAttackTyp);
                            if (GuiRandom.Range(0, 100) < GetQuquWindow.instance.GetQuquDate(enemyQuquId[ququBattleTurn], 31))
                            {
                                bool _cHit2 = true;
                                _actorQuquDamage += GetQuquWindow.instance.GetQuquDate(enemyQuquId[ququBattleTurn], 32);
                            }
                            if (GuiRandom.Range(0, 100) < GetQuquWindow.instance.GetQuquDate(actorQuquId[ququBattleTurn], 33))
                            {
                                bool _def2 = true;
                                _actorQuquDamage = Mathf.Max(0, _actorQuquDamage - GetQuquWindow.instance.GetQuquDate(actorQuquId[ququBattleTurn], 34));
                            }
                            enemyQuqu[ququBattleTurn].DOLocalJump(new Vector3(enemyQuqu[ququBattleTurn].localPosition.x - 20f, 0f, 0f), 20f, 1, 0.01f).SetDelay(num + 0.6f).OnComplete(delegate
                            {
                                bool flag = false;
                                int num6 = GetQuquWindow.instance.GetQuquDate(actorQuquId[ququBattleTurn], 35) - reAttackTutn * 5;
                                if (GuiRandom.Range(0, 100) < num6)
                                {
                                    flag = true;
                                }
                                GuiQuquBattleSystem ququBattleSystem = this;
                                bool attacker2 = attacker;
                                int damage2 = _actorQuquDamage;
                                bool cHit2 = _cHit3;
                                bool def2 = _def3;
                                bool reAttack2 = flag;
                                int num7 = reAttackTutn;
                                reAttackTutn = num7 + 1;
                                ququBattleSystem.ShowDamage(attacker2, true, 1, damage2, 0.1f, cHit2, def2, reAttack2, true, false, num7, (reAttackTyp == 22) ? 23 : 22);
                                if (!QuquIsDead())
                                {
                                    enemyQuqu[ququBattleTurn].DOLocalJump(new Vector3(enemyQuqu[ququBattleTurn].localPosition.x + 20f, 0f, 0f), 20f, 1, 0.01f).SetDelay(0.4f);
                                }
                            });
                        }
                        else if (attacker)
                        {
                            actorQuqu[ququBattleTurn].DOLocalJump(new Vector3(330f, 0f, 0f), 20f, 1, 0.01f).SetDelay(num + 1.4f).OnComplete(delegate
                            {
                                if (newTurn)
                                {
                                    QuquBattleLoopStart(0.02f, 0.6f);
                                }
                                else
                                {
                                    QuquBaseAttacke(2, newTurn: true);
                                }
                            });
                        }
                        else
                        {
                            enemyQuqu[ququBattleTurn].DOLocalJump(new Vector3(-330f, 0f, 0f), 20f, 1, 0.01f).SetDelay(num + 1.4f).OnComplete(delegate
                            {
                                if (newTurn)
                                {
                                    QuquBattleLoopStart(0.02f, 0.6f);
                                }
                                else
                                {
                                    QuquBaseAttacke(1, newTurn: true);
                                }
                            });
                        }
                    }
                    break;
                case 2:
                    if (defer)
                    {
                        actorQuquSp[ququBattleTurn] -= damage;
                        text = $"{DateFile.instance.massageDate[8004][0].Split('|')[1]}-{damage}";
                        if (QuquIsDead())
                        {
                            actorQuqu[ququBattleTurn].DOKill();
                            enemyQuqu[ququBattleTurn].DOKill();
                            battleStateText[ququBattleTurn].text = "";
                            nextButtonMask[ququBattleTurn].GetComponent<Image>().DOColor(new Color(0f, 0f, 0f, 0.5f), 1f).OnComplete(delegate
                            {
                                SetBattleStateText(DateFile.instance.SetColoer(20010, DateFile.instance.massageDate[8003][4].Split('|')[1]), 0.4f, 3f, 2);
                            });
                        }
                    }
                    else
                    {
                        enemyQuquSp[ququBattleTurn] -= damage;
                        text = $"{DateFile.instance.massageDate[8004][0].Split('|')[1]}-{damage}";
                        if (QuquIsDead())
                        {
                            actorQuqu[ququBattleTurn].DOKill();
                            enemyQuqu[ququBattleTurn].DOKill();
                            battleStateText[ququBattleTurn].text = "";
                            nextButtonMask[ququBattleTurn].GetComponent<Image>().DOColor(new Color(0f, 0f, 0f, 0.5f), 1f).OnComplete(delegate
                            {
                                SetBattleStateText(DateFile.instance.SetColoer(20005, DateFile.instance.massageDate[8003][4].Split('|')[0]), 0.4f, 3f, 1);
                            });
                        }
                    }
                    break;
            }
            Damage(defer, size, delay, text, textColor);
        }

        private void Damage(bool isActor, float size, float delay, string damageText, Color textColor)
        {
            float num = size / 2f - 0.2f;
            if (isActor)
            {
                actorQuquDamageText[ququBattleTurn].DOKill();
                actorQuquIcon[ququBattleTurn].transform.DOShakePosition(0.1f, 20f).SetDelay(0.01f + delay).OnComplete(RestBattlerPosition);
                actorQuquDamageText[ququBattleTurn].transform.DOScale(new Vector3(size, size, 1f), 0.1f).SetDelay(delay).SetEase(Ease.OutBack)
                    .OnStart(delegate
                    {
                        actorQuquDamageText[ququBattleTurn].color = textColor;
                        actorQuquDamageText[ququBattleTurn].transform.localScale = new Vector3(0f, 0f, 1f);
                        actorQuquDamageText[ququBattleTurn].transform.localPosition = new Vector3(0f, 0f, 0f);
                        actorQuquDamageText[ququBattleTurn].text = damageText;
                    });
                actorQuquDamageText[ququBattleTurn].transform.DOScale(new Vector3(num, num, 1f), 0.4f).SetDelay(0.1f + delay).SetEase(Ease.OutBack);
                actorQuquDamageText[ququBattleTurn].transform.DOLocalMoveY(60f, 1.4f).SetDelay(0.4f + delay);
                actorQuquDamageText[ququBattleTurn].DOFade(0f, 0.3f).SetDelay(1.5f + delay);
            }
            else
            {
                enemyQuquDamageText[ququBattleTurn].DOKill();
                enemyQuquIcon[ququBattleTurn].transform.DOShakePosition(0.1f, 20f).SetDelay(0.01f + delay).OnComplete(RestBattlerPosition);
                enemyQuquDamageText[ququBattleTurn].transform.DOScale(new Vector3(size, size, 1f), 0.1f).SetDelay(delay).SetEase(Ease.OutBack)
                    .OnStart(delegate
                    {
                        enemyQuquDamageText[ququBattleTurn].color = textColor;
                        enemyQuquDamageText[ququBattleTurn].transform.localScale = new Vector3(0f, 0f, 1f);
                        enemyQuquDamageText[ququBattleTurn].transform.localPosition = new Vector3(0f, 0f, 0f);
                        enemyQuquDamageText[ququBattleTurn].text = damageText;
                    });
                enemyQuquDamageText[ququBattleTurn].transform.DOScale(new Vector3(num, num, 1f), 0.4f).SetDelay(0.1f + delay).SetEase(Ease.OutBack);
                enemyQuquDamageText[ququBattleTurn].transform.DOLocalMoveY(60f, 1.4f).SetDelay(0.4f + delay);
                enemyQuquDamageText[ququBattleTurn].DOFade(0f, 0.3f).SetDelay(1.5f + delay);
            }
            UpdateQuquHp(ququBattleTurn);
        }

        private void RestBattlerPosition()
        {
            actorQuquIcon[ququBattleTurn].transform.localPosition = new Vector3(0f, 0f, 0f);
            enemyQuquIcon[ququBattleTurn].transform.localPosition = new Vector3(0f, 0f, 0f);
        }

        private void BattleEndWindow(int typ)
        {
            battleEndBodyText.text = "";
            battleEndBodyName.text = DateFile.instance.massageDate[8004][1].Split('|')[typ];
            battleEndTypImage.sprite = GetSprites.instance.battleEndTypImage[typ];
            battleEndWindow.SetActive(value: true);
            battleEndWindow.transform.DOScale(new Vector3(1f, 1f, 1f), 0.25f).SetEase(Ease.OutQuad);
            Component[] componentsInChildren = battleEndWindow.GetComponentsInChildren<Component>();
            Component[] array = componentsInChildren;
            foreach (Component component in array)
            {
                if (component is Graphic)
                {
                    (component as Graphic).CrossFadeAlpha(1f, 0.25f, ignoreTimeScale: true);
                }
            }
            StartCoroutine(QuquBattleWin(0.25f, typ));
        }

        private IEnumerator QuquBattleWin(float waitTime, int typ)
        {
            yield return new WaitForSeconds(waitTime);
            int _mianActorId = DateFile.instance.MianActorID();
            if (typ == 0)
            {
                battleEndBodyText.text = enemyBodyNameText.text;
                if (DateFile.instance.ParseInt(DateFile.instance.cricketBattleDate[______ququBattleId][1]) != 0)
                {
                    switch (enemyBodyTyp)
                    {
                        case 0:
                            UIDate.instance.ChangeResource(DateFile.instance.MianActorID(), enemyBodyId, ______enemyBodySize, canShow: false);
                            break;
                        case 1:
                            if (DateFile.instance.ParseInt(DateFile.instance.GetItemDate(enemyBodyId, 6)) == 0)
                            {
                                int _newItemId = DateFile.instance.MakeNewItem(DateFile.instance.ParseInt(DateFile.instance.GetItemDate(enemyBodyId, 999)));
                                List<int> _bodyDate = new List<int>(DateFile.instance.itemsDate[enemyBodyId].Keys);
                                for (int i = 0; i < _bodyDate.Count; i++)
                                {
                                    int _itemAttId = _bodyDate[i];
                                    DateFile.instance.itemsDate[_newItemId][_itemAttId] = DateFile.instance.itemsDate[enemyBodyId][_itemAttId];
                                }
                                DateFile.instance.GetItem(DateFile.instance.MianActorID(), _newItemId, 1, false, 0);
                            }
                            else
                            {
                                DateFile.instance.GetItem(DateFile.instance.MianActorID(), enemyBodyId, 1, true, 0);
                            }
                            break;
                        case 2:
                            if (bodyActorId != -99)
                            {
                                DateFile.instance.GetActor(new List<int>
                        {
                            bodyActorId
                        }, 4);
                            }
                            break;
                    }
                }
                else
                {
                    switch (enemyBodyTyp)
                    {
                        case 0:
                            UIDate.instance.ChangeResource(______ququBattleEnemyId, enemyBodyId, -______enemyBodySize, canShow: false);
                            UIDate.instance.ChangeResource(DateFile.instance.MianActorID(), enemyBodyId, ______enemyBodySize, canShow: false);
                            break;
                        case 1:
                            DateFile.instance.ChangeTwoActorItem(______ququBattleEnemyId, _mianActorId, enemyBodyId);
                            break;
                    }
                }
                string[] _eventDate2 = DateFile.instance.cricketBattleDate[______ququBattleId][6].Split('&');
                int _eventId2 = DateFile.instance.ParseInt(_eventDate2[0]);
                if (_eventId2 != 0)
                {
                    if (_eventDate2.Length > 1)
                    {
                        int num = DateFile.instance.ParseInt(_eventDate2[1]);
                        if (num == 1)
                        {
                            DateFile.instance.SetEvent(new int[4]
                            {
                            0,
                            ______ququBattleEnemyId,
                            _eventId2,
                            ______ququBattleEnemyId
                            }, addToFirst: true);
                        }
                    }
                    else
                    {
                        DateFile.instance.SetEvent(new int[3]
                        {
                        0,
                        -1,
                        _eventId2
                        }, addToFirst: true);
                    }
                }
            }
            else
            {
                battleEndBodyText.text = actorBodyNameText.text;
                if (DateFile.instance.ParseInt(DateFile.instance.cricketBattleDate[______ququBattleId][1]) != 0)
                {
                    switch (actorBodyTyp)
                    {
                        case 0:
                            UIDate.instance.ChangeResource(DateFile.instance.MianActorID(), actorBodyId, -actorBodySize, canShow: false);
                            break;
                        case 1:
                            DateFile.instance.LoseItem(DateFile.instance.MianActorID(), actorBodyId, 1, removeItem: true);
                            break;
                        case 2:
                            DateFile.instance.RemoveActor(new List<int>
                    {
                        actorBodyId
                    }, die: false);
                            break;
                    }
                }
                else
                {
                    switch (actorBodyTyp)
                    {
                        case 0:
                            UIDate.instance.ChangeResource(______ququBattleEnemyId, actorBodyId, actorBodySize, canShow: false);
                            UIDate.instance.ChangeResource(DateFile.instance.MianActorID(), actorBodyId, -actorBodySize, canShow: false);
                            break;
                        case 1:
                            DateFile.instance.ChangeTwoActorItem(_mianActorId, ______ququBattleEnemyId, actorBodyId);
                            break;
                    }
                }
                string[] _eventDate = DateFile.instance.cricketBattleDate[______ququBattleId][7].Split('&');
                int _eventId = DateFile.instance.ParseInt(_eventDate[0]);
                if (_eventId != 0)
                {
                    if (_eventDate.Length > 1)
                    {
                        int num2 = DateFile.instance.ParseInt(_eventDate[1]);
                        if (num2 == 1)
                        {
                            DateFile.instance.SetEvent(new int[4]
                            {
                            0,
                            ______ququBattleEnemyId,
                            _eventId,
                            ______ququBattleEnemyId
                            }, addToFirst: true);
                        }
                    }
                    else
                    {
                        DateFile.instance.SetEvent(new int[3]
                        {
                        0,
                        -1,
                        _eventId
                        }, addToFirst: true);
                    }
                }
            }
            closeBattleButton.SetActive(value: true);
        }

        private void UpdateBattleQuquCall()
        {
            for (int i = 0; i < actorQuquCall.Length; i++)
            {
                if (actorQuquId[i] >= 0)
                {
                    actorQuquCallTime[i] += 1 + GuiRandom.Range(0, 5);
                    if (actorQuquCallTime[i] >= 600 + DateFile.instance.ParseInt(DateFile.instance.GetItemDate(actorQuquId[i], 8)) * 100)
                    {
                        actorQuquCallTime[i] = GuiRandom.Range(0, 300);
                        actorQuquCall[i].UpdateBattleQuquCall(actorQuquId[i]);
                        actorQuquCall[i].CallvolumeRest(0.6f, 0.6f);
                    }
                }
                else
                {
                    actorQuquCallTime[i] = GuiRandom.Range(0, 300);
                }
            }
            for (int j = 0; j < enemyQuquCall.Length; j++)
            {
                enemyQuquCallTime[j] += 1 + GuiRandom.Range(0, 5);
                if (enemyQuquCallTime[j] >= 600 + DateFile.instance.ParseInt(DateFile.instance.GetItemDate(enemyQuquId[j], 8)) * 100)
                {
                    enemyQuquCallTime[j] = GuiRandom.Range(0, 300);
                    enemyQuquCall[j].UpdateBattleQuquCall(enemyQuquId[j]);
                    enemyQuquCall[j].CallvolumeRest(0.6f, 0.6f);
                }
            }
        }

        private void Awake()
        {
        }

        private void Start()
        {
            SetTweener();
            ququBattleWindow.SetActive(value: false);
            chooseBodyWindow.sizeDelta = new Vector2(280f, 0f);
            for (int i = 0; i < uiText.Length; i++)
            {
                uiText[i].text = DateFile.instance.massageDate[8001][3].Split('|')[i];
            }
            battleEndWindow.transform.localPosition = new Vector3(0f, 0f, 0f);
            battleEndWindow.transform.localScale = new Vector3(5f, 5f, 1f);
            Component[] componentsInChildren = battleEndWindow.GetComponentsInChildren<Component>();
            Component[] array = componentsInChildren;
            foreach (Component component in array)
            {
                if (component is Graphic)
                {
                    (component as Graphic).CrossFadeAlpha(0f, 0f, ignoreTimeScale: true);
                }
            }
            battleEndWindow.SetActive(value: false);
            closeBattleButton.SetActive(value: false);
        }

        private void Update()
        {
            if (ququBattlePart == 1)
            {
                UpdateBattleQuquCall();
            }
        }

        public void Init()
        {
            instance = this;
            Transform _transform = transform;

            setBattleSpeedHolder = _transform.Find("QuquBattleBack/QuquBattleHolder/SetQuquBattleSpeed").gameObject;
            setBattleSpeedToggle = new Toggle[] {
_transform.Find("QuquBattleBack/QuquBattleHolder/SetQuquBattleSpeed/x1").GetComponent<Toggle>(),
_transform.Find("QuquBattleBack/QuquBattleHolder/SetQuquBattleSpeed/x2").GetComponent<Toggle>(),
_transform.Find("QuquBattleBack/QuquBattleHolder/SetQuquBattleSpeed/x3").GetComponent<Toggle>(),
};
            ququBattleWindow = gameObject;
            actorBack = _transform.Find("BattleActorBack");
            enemyBack = _transform.Find("BattleEnemyBack");
            chooseActorButton = _transform.Find("BattleActorBack/ChooseBodyWindow/ChooseActorButton,654").GetComponent<Button>();
            ququBattleBack = new GameObject[] {
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle1/Back").gameObject,
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle2/Back").gameObject,
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle3/Back").gameObject,
};
            actorFace = _transform.Find("BattleActorBack/ActorFaceHolder/MianActorFace").GetComponent<ActorFace>();
            enemyFace = _transform.Find("BattleEnemyBack/EnemyFaceHolder/MianActorFace").GetComponent<ActorFace>();
            actorNameText = _transform.Find("BattleActorBack/ActorFaceHolder/ActorNameBack/ActorNameText").GetComponent<Text>();
            enemyNameText = _transform.Find("BattleEnemyBack/EnemyFaceHolder/EnemyNameBack/EnemyNameText").GetComponent<Text>();
            battleStateText = new Text[] {
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle1/BattleStateText1").GetComponent<Text>(),
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle2/BattleStateText2").GetComponent<Text>(),
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle3/BattleStateText3").GetComponent<Text>(),
};
            battleValue = new Transform[] {
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle1/ValueMask1/BattleValue1"),
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle2/ValueMask2/BattleValue2"),
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle3/ValueMask3/BattleValue3"),
};
            actorQuqu = new Transform[] {
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle1/ActorBattleQuqu1/ActorQuqu1"),
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle2/ActorBattleQuqu2/ActorQuqu2"),
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle3/ActorBattleQuqu3/ActorQuqu3"),
};
            enemyQuqu = new Transform[] {
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle1/EnemyBattleQuqu1/EnemyQuqu1"),
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle2/EnemyBattleQuqu2/EnemyQuqu2"),
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle3/EnemyBattleQuqu3/EnemyQuqu3"),
};
            startBattleWindow = _transform.Find("QuquBattleBack/StartQuquBattle").gameObject;
            startBattleButton = _transform.Find("QuquBattleBack/StartQuquBattle/StartQuquBattleButton,656").GetComponent<Button>();
            loseBattleButton = _transform.Find("QuquBattleBack/StartQuquBattle/CloseQuquBattleButton,655").GetComponent<Button>();
            uiText = new Text[] {
_transform.Find("BattleActorBack/ActorBattleBodyNameBack/ActorBattleBodyNameText").GetComponent<Text>(),
_transform.Find("BattleEnemyBack/EnemyBattleBodyNameBack/EnemyBattleBodyNameText").GetComponent<Text>(),
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle1/ActorBattleQuqu1/TurnText1").GetComponent<Text>(),
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle2/ActorBattleQuqu2/TurnText2").GetComponent<Text>(),
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle3/ActorBattleQuqu3/TurnText3").GetComponent<Text>(),
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle1/EnemyBattleQuqu1/TurnText1").GetComponent<Text>(),
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle2/EnemyBattleQuqu2/TurnText2").GetComponent<Text>(),
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle3/EnemyBattleQuqu3/TurnText3").GetComponent<Text>(),
};
            actorBodyImage = _transform.Find("BattleActorBack/ActorBattleBodyNameBack/BattleBodyBack/BodyItemBack/ActorBodyItem").GetComponent<Image>();
            actorBodyNameText = _transform.Find("BattleActorBack/ActorBattleBodyNameBack/BattleBodyBack/BodyItemBack/ItemNameBack/ItemNameText").GetComponent<Text>();
            enemyBodyImage = _transform.Find("BattleEnemyBack/EnemyBattleBodyNameBack/BattleBodyBack/BodyItemBack/EnemyBodyItem").GetComponent<Image>();
            enemyBodyNameText = _transform.Find("BattleEnemyBack/EnemyBattleBodyNameBack/BattleBodyBack/BodyItemBack/ItemNameBack/ItemNameText").GetComponent<Text>();
            resourceButtonImage = _transform.Find("BattleActorBack/ChooseBodyWindow/ChooseResourceButton,652").GetComponent<Image>();
            hideQuquImage = new GameObject[] {
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle1/EnemyBattleQuqu1/HideQuquBack1").gameObject,
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle2/EnemyBattleQuqu2/HideQuquBack2").gameObject,
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle3/EnemyBattleQuqu3/HideQuquBack3").gameObject,
};
            actorQuquHpText = new Text[]{
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle1/ActorBattleQuqu1/ActorQuqu1/ActorQuquHpText1").GetComponent<Text>(),
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle2/ActorBattleQuqu2/ActorQuqu2/ActorQuquHpText2").GetComponent<Text>(),
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle3/ActorBattleQuqu3/ActorQuqu3/ActorQuquHpText3").GetComponent<Text>(),
};
            enemyQuquHpText = new Text[]{
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle1/EnemyBattleQuqu1/EnemyQuqu1/EnemyQuquHpText1").GetComponent<Text>(),
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle2/EnemyBattleQuqu2/EnemyQuqu2/EnemyQuquHpText2").GetComponent<Text>(),
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle3/EnemyBattleQuqu3/EnemyQuqu3/EnemyQuquHpText3").GetComponent<Text>(),
};
            actorQuquIcon = new Image[]{
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle1/ActorBattleQuqu1/ActorQuqu1/ActorQuquItem1").GetComponent<Image>(),
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle2/ActorBattleQuqu2/ActorQuqu2/ActorQuquItem2").GetComponent<Image>(),
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle3/ActorBattleQuqu3/ActorQuqu3/ActorQuquItem3").GetComponent<Image>(),
};
            enemyQuquIcon = new Image[]{
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle1/EnemyBattleQuqu1/EnemyQuqu1/EnemyQuquItem1").GetComponent<Image>(),
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle2/EnemyBattleQuqu2/EnemyQuqu2/EnemyQuquItem2").GetComponent<Image>(),
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle3/EnemyBattleQuqu3/EnemyQuqu3/EnemyQuquItem3").GetComponent<Image>(),
};
            actorBattleQuquNameText = new Text[]{
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle1/ValueMask1/BattleValue1/ActorQuquValueBack1/ActorQuquNameBack1/ActorQuquNameText1").GetComponent<Text>(),
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle2/ValueMask2/BattleValue2/ActorQuquValueBack2/ActorQuquNameBack2/ActorQuquNameText2").GetComponent<Text>(),
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle3/ValueMask3/BattleValue3/ActorQuquValueBack3/ActorQuquNameBack3/ActorQuquNameText3").GetComponent<Text>(),
};
            actorBattleQuquPower1Text = new Text[]{
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle1/ValueMask1/BattleValue1/ActorQuquValueBack1/ActorQuquNameBack1/ActorValue1Icon1/ActorValue1Text1").GetComponent<Text>(),
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle2/ValueMask2/BattleValue2/ActorQuquValueBack2/ActorQuquNameBack2/ActorValue1Icon2/ActorValue1Text2").GetComponent<Text>(),
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle3/ValueMask3/BattleValue3/ActorQuquValueBack3/ActorQuquNameBack3/ActorValue1Icon3/ActorValue1Text3").GetComponent<Text>(),
};
            actorBattleQuquPower2Text = new Text[]{
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle1/ValueMask1/BattleValue1/ActorQuquValueBack1/ActorQuquNameBack1/ActorValue2Icon1/ActorValue2Text1").GetComponent<Text>(),
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle2/ValueMask2/BattleValue2/ActorQuquValueBack2/ActorQuquNameBack2/ActorValue2Icon2/ActorValue2Text2").GetComponent<Text>(),
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle3/ValueMask3/BattleValue3/ActorQuquValueBack3/ActorQuquNameBack3/ActorValue2Icon3/ActorValue2Text3").GetComponent<Text>(),
};
            actorBattleQuquPower3Text = new Text[]{
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle1/ValueMask1/BattleValue1/ActorQuquValueBack1/ActorQuquNameBack1/ActorValue3Icon1/ActorValue3Text1").GetComponent<Text>(),
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle2/ValueMask2/BattleValue2/ActorQuquValueBack2/ActorQuquNameBack2/ActorValue3Icon2/ActorValue3Text2").GetComponent<Text>(),
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle3/ValueMask3/BattleValue3/ActorQuquValueBack3/ActorQuquNameBack3/ActorValue3Icon3/ActorValue3Text3").GetComponent<Text>(),
};
            actorBattleQuquStrengthText = new Text[]{
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle1/ValueMask1/BattleValue1/ActorQuquValueBack1/ActorHpSpBar1/ActorHpBarBack1/ActorHpText1").GetComponent<Text>(),
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle2/ValueMask2/BattleValue2/ActorQuquValueBack2/ActorHpSpBar2/ActorHpBarBack2/ActorHpText2").GetComponent<Text>(),
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle3/ValueMask3/BattleValue3/ActorQuquValueBack3/ActorHpSpBar3/ActorHpBarBack3/ActorHpText3").GetComponent<Text>(),
};
            actorBattleQuquMagicText = new Text[]{
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle1/ValueMask1/BattleValue1/ActorQuquValueBack1/ActorHpSpBar1/ActorSpBarBack1/ActorSpText1").GetComponent<Text>(),
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle2/ValueMask2/BattleValue2/ActorQuquValueBack2/ActorHpSpBar2/ActorSpBarBack2/ActorSpText2").GetComponent<Text>(),
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle3/ValueMask3/BattleValue3/ActorQuquValueBack3/ActorHpSpBar3/ActorSpBarBack3/ActorSpText3").GetComponent<Text>(),
};
            actorBattleQuquStrengthBar = new Image[]{
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle1/ValueMask1/BattleValue1/ActorQuquValueBack1/ActorHpSpBar1/ActorHpBarBack1/ActorHpBar1").GetComponent<Image>(),
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle2/ValueMask2/BattleValue2/ActorQuquValueBack2/ActorHpSpBar2/ActorHpBarBack2/ActorHpBar2").GetComponent<Image>(),
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle3/ValueMask3/BattleValue3/ActorQuquValueBack3/ActorHpSpBar3/ActorHpBarBack3/ActorHpBar3").GetComponent<Image>(),
};
            actorBattleQuquMagicBar = new Image[]{
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle1/ValueMask1/BattleValue1/ActorQuquValueBack1/ActorHpSpBar1/ActorSpBarBack1/ActorSpBar1").GetComponent<Image>(),
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle2/ValueMask2/BattleValue2/ActorQuquValueBack2/ActorHpSpBar2/ActorSpBarBack2/ActorSpBar2").GetComponent<Image>(),
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle3/ValueMask3/BattleValue3/ActorQuquValueBack3/ActorHpSpBar3/ActorSpBarBack3/ActorSpBar3").GetComponent<Image>(),
};
            enemyBattleQuquNameText = new Text[]{
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle1/ValueMask1/BattleValue1/EnemyQuquValueBack1/EnemyQuquNameBack1/EnemyQuquNameText1").GetComponent<Text>(),
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle2/ValueMask2/BattleValue2/EnemyQuquValueBack2/EnemyQuquNameBack2/EnemyQuquNameText2").GetComponent<Text>(),
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle3/ValueMask3/BattleValue3/EnemyQuquValueBack3/EnemyQuquNameBack3/EnemyQuquNameText3").GetComponent<Text>(),
};
            enemyBattleQuquPower1Text = new Text[]{
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle1/ValueMask1/BattleValue1/EnemyQuquValueBack1/EnemyQuquNameBack1/EnemyValue1Icon1/EnemyValue1Text1").GetComponent<Text>(),
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle2/ValueMask2/BattleValue2/EnemyQuquValueBack2/EnemyQuquNameBack2/EnemyValue1Icon2/EnemyValue1Text2").GetComponent<Text>(),
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle3/ValueMask3/BattleValue3/EnemyQuquValueBack3/EnemyQuquNameBack3/EnemyValue1Icon3/EnemyValue1Text3").GetComponent<Text>(),
};
            enemyBattleQuquPower2Text = new Text[]{
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle1/ValueMask1/BattleValue1/EnemyQuquValueBack1/EnemyQuquNameBack1/EnemyValue2Icon1/EnemyValue2Text1").GetComponent<Text>(),
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle2/ValueMask2/BattleValue2/EnemyQuquValueBack2/EnemyQuquNameBack2/EnemyValue2Icon2/EnemyValue2Text2").GetComponent<Text>(),
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle3/ValueMask3/BattleValue3/EnemyQuquValueBack3/EnemyQuquNameBack3/EnemyValue2Icon3/EnemyValue2Text3").GetComponent<Text>(),
};
            enemyBattleQuquPower3Text = new Text[]{
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle1/ValueMask1/BattleValue1/EnemyQuquValueBack1/EnemyQuquNameBack1/EnemyValue3Icon1/EnemyValue3Text1").GetComponent<Text>(),
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle2/ValueMask2/BattleValue2/EnemyQuquValueBack2/EnemyQuquNameBack2/EnemyValue3Icon2/EnemyValue3Text2").GetComponent<Text>(),
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle3/ValueMask3/BattleValue3/EnemyQuquValueBack3/EnemyQuquNameBack3/EnemyValue3Icon3/EnemyValue3Text3").GetComponent<Text>(),
};
            enemyBattleQuquStrengthText = new Text[]{
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle1/ValueMask1/BattleValue1/EnemyQuquValueBack1/EnemyHpSpBar1/EnemyHpBarBack1/EnemyHpText1").GetComponent<Text>(),
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle2/ValueMask2/BattleValue2/EnemyQuquValueBack2/EnemyHpSpBar2/EnemyHpBarBack2/EnemyHpText2").GetComponent<Text>(),
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle3/ValueMask3/BattleValue3/EnemyQuquValueBack3/EnemyHpSpBar3/EnemyHpBarBack3/EnemyHpText3").GetComponent<Text>(),
};
            enemyBattleQuquMagicText = new Text[]{
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle1/ValueMask1/BattleValue1/EnemyQuquValueBack1/EnemyHpSpBar1/EnemySpBarBack1/EnemySpText1").GetComponent<Text>(),
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle2/ValueMask2/BattleValue2/EnemyQuquValueBack2/EnemyHpSpBar2/EnemySpBarBack2/EnemySpText2").GetComponent<Text>(),
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle3/ValueMask3/BattleValue3/EnemyQuquValueBack3/EnemyHpSpBar3/EnemySpBarBack3/EnemySpText3").GetComponent<Text>(),
};
            enemyBattleQuquStrengthBar = new Image[]{
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle1/ValueMask1/BattleValue1/EnemyQuquValueBack1/EnemyHpSpBar1/EnemyHpBarBack1/EnemyHpBar1").GetComponent<Image>(),
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle2/ValueMask2/BattleValue2/EnemyQuquValueBack2/EnemyHpSpBar2/EnemyHpBarBack2/EnemyHpBar2").GetComponent<Image>(),
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle3/ValueMask3/BattleValue3/EnemyQuquValueBack3/EnemyHpSpBar3/EnemyHpBarBack3/EnemyHpBar3").GetComponent<Image>(),
};
            enemyBattleQuquMagicBar = new Image[]{
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle1/ValueMask1/BattleValue1/EnemyQuquValueBack1/EnemyHpSpBar1/EnemySpBarBack1/EnemySpBar1").GetComponent<Image>(),
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle2/ValueMask2/BattleValue2/EnemyQuquValueBack2/EnemyHpSpBar2/EnemySpBarBack2/EnemySpBar2").GetComponent<Image>(),
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle3/ValueMask3/BattleValue3/EnemyQuquValueBack3/EnemyHpSpBar3/EnemySpBarBack3/EnemySpBar3").GetComponent<Image>(),
};
            actorQuquName = new Text[]{
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle1/ActorBattleQuqu1/ActorQuquNameBack1/ActorQuquNameText1").GetComponent<Text>(),
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle2/ActorBattleQuqu2/ActorQuquNameBack2/ActorQuquNameText2").GetComponent<Text>(),
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle3/ActorBattleQuqu3/ActorQuquNameBack3/ActorQuquNameText3").GetComponent<Text>(),
};
            enemyQuquName = new Text[]{
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle1/EnemyBattleQuqu1/EnemyQuquNameBack1/EnemyQuquNameText1").GetComponent<Text>(),
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle2/EnemyBattleQuqu2/EnemyQuquNameBack2/EnemyQuquNameText2").GetComponent<Text>(),
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle3/EnemyBattleQuqu3/EnemyQuquNameBack3/EnemyQuquNameText3").GetComponent<Text>(),
};
            chooseBodyWindow = _transform.Find("BattleActorBack/ChooseBodyWindow") as RectTransform;
            needResourceText = _transform.Find("BattleActorBack/ChooseBodyWindow/ChooseResourceButton,652/UseMoneyText").GetComponent<Text>();
            useResourceButton = _transform.Find("BattleActorBack/ChooseBodyWindow/ChooseResourceButton,652").GetComponent<Button>();
            acotrWindow = _transform.Find("ActorBack").gameObject;
            useActorButton = _transform.Find("ActorBack/UseActorButton,631").GetComponent<Button>();
            actorHolder = _transform.Find("ActorBack/ActorView/ActorViewport/ActorHolder") as RectTransform;
            actorIcon = Instantiate(QuquBattleSystem.instance.actorIcon);
            itemWindow = _transform.Find("ItemsBack").gameObject;
            useItemButton = _transform.Find("ItemsBack/ItemsView/UseItemButton,631").GetComponent<Button>();
            itemMask = _transform.Find("ChooseItemMask").gameObject;
            itemHolder = _transform.Find("ItemsBack/ItemsView/ItemsViewport/ItemsHolder") as RectTransform;
            nextButton = new Button[]{
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle1/NexButton,657").GetComponent<Button>(),
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle2/NexButton,657").GetComponent<Button>(),
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle3/NexButton,657").GetComponent<Button>(),
};
            nextButtonMask = new GameObject[]{
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle1/BattleEndMask1").gameObject,
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle2/BattleEndMask2").gameObject,
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle3/BattleEndMask3").gameObject,
};
            actorQuquDamageText = new Text[]{
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle1/ActorBattleQuqu1/ActorQuqu1/ActorQuquDamageText1").GetComponent<Text>(),
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle2/ActorBattleQuqu2/ActorQuqu2/ActorQuquDamageText2").GetComponent<Text>(),
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle3/ActorBattleQuqu3/ActorQuqu3/ActorQuquDamageText3").GetComponent<Text>(),
};
            enemyQuquDamageText = new Text[]{
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle1/EnemyBattleQuqu1/EnemyQuqu1/QuquDamageText1").GetComponent<Text>(),
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle2/EnemyBattleQuqu2/EnemyQuqu2/QuquDamageText2").GetComponent<Text>(),
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle3/EnemyBattleQuqu3/EnemyQuqu3/QuquDamageText3").GetComponent<Text>(),
};
            actorQuquCall = new QuquPlace[]{
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle1/ActorBattleQuqu1").GetComponent<QuquPlace>(),
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle2/ActorBattleQuqu2").GetComponent<QuquPlace>(),
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle3/ActorBattleQuqu3").GetComponent<QuquPlace>(),
};
            enemyQuquCall = new QuquPlace[]{
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle1/EnemyBattleQuqu1").GetComponent<QuquPlace>(),
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle2/EnemyBattleQuqu2").GetComponent<QuquPlace>(),
_transform.Find("QuquBattleBack/QuquBattleHolder/QuquBattle3/EnemyBattleQuqu3").GetComponent<QuquPlace>(),
};



            battleEndWindow = Instantiate(QuquBattleSystem.instance.battleEndWindow);
            Transform ququ_battle_end_window = battleEndWindow.transform;
            ququ_battle_end_window.SetParent(QuquBattleSystem.instance.battleEndWindow.transform.parent, false);
            closeBattleButton = ququ_battle_end_window.Find("QuquBattleEndBack/CloseWindowButton").gameObject;
            battleEndTypImage = ququ_battle_end_window.Find("QuquBattleEndBack/BattleEndTypImage").GetComponent<Image>();
            battleEndBodyName = ququ_battle_end_window.Find("QuquBattleEndBack/BattleEndBodyNameBack/BattleRatedNameText").GetComponent<Text>();
            battleEndBodyText = ququ_battle_end_window.Find("QuquBattleEndBack/BattleEndBodyNameBack/BattleEndBodyBack/BattleEndBodyText").GetComponent<Text>();


        }
    }


    public static class GuiRandom
    {
        static int idx;
        static System.Random random;
        public static void InitSeed(int seed)
        {
            Main.Logger.Log(" 种子: " + seed);
            idx = 0;
            random = new System.Random(seed);
        }
        public static int Range(int a, int b)
        {
            int num = random.Next(a, b);
            Main.Logger.Log(idx++ + " 随机数: " + num);
            return num;
        }
    }

}