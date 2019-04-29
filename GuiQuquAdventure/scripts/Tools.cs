using System.Collections.Generic;
using UnityEngine;

namespace GuiQuquAdventure
{
    public static class Tools
    {
        public static T GetChildComponent<T>(Transform parent, string path) where T : Component
        {
            return parent.Find(path).GetComponent<T>();
        }
        public static GameObject GetChildObj(Transform parent, string path)
        {
            return parent.Find(path).gameObject;
        }
        public static Transform GetChildTf(Transform parent, string path)
        {
            return parent.Find(path);
        }
        public static Color GetShadowColor(Color color)
        {
            int r = (int)color.r * 255;
            int g = (int)color.g * 255;
            int b = (int)color.b * 255;
            int max = Mathf.Max(Mathf.Max(r, g), b);
            int min = Mathf.Min(Mathf.Min(r, g), b);
            Color shadowColor = new Color((float)(max + min - r) / 255, (float)(max + min - g) / 255, (float)(max + min - b) / 255, 1);


            //Color shadowColor = new Color(1 -color.r, 1-color.g,1-color.b);

            return shadowColor;
        }


        public static Color GetColor(string ip, int typ = 0)
        {
            string[] ipc = ip.Split('.');
            int r = 0;
            int g = 0;
            int b = 0;
            if (ipc.Length > 0)
            {
                int.TryParse(ipc[0], out r);
            }
            if (ipc.Length > 1)
            {
                int.TryParse(ipc[1], out g);
            }
            if (ipc.Length > 2)
            {
                int.TryParse(ipc[2], out b);
            }
            Color color = new Color((float)r / 255, (float)g / 255, (float)b / 255, 1);
            return color;
        }
#if !TAIWU_GAME
        public static GameObject GetActorFaceSample()
        {
            return ActorMenu.instance.mianActorFace.gameObject;
        }
#endif

        public static void UpdateFace(int[] last_image, ActorFace actorFace, int age, int gender, int actorGenderChange, int[] faceDate, int[] faceColor, int clotheIndex, bool life = true)
        {
            if (!CheckImageChange(last_image, faceDate.Length, age, gender, actorGenderChange, faceDate, faceColor, clotheIndex))
                return;
            if (faceDate.Length == 1)
            {

                actorFace.ageImage.gameObject.SetActive(value: false);
                actorFace.nose.gameObject.SetActive(value: false);
                actorFace.faceOther.gameObject.SetActive(value: false);
                actorFace.eye.gameObject.SetActive(value: false);
                actorFace.eyePupil.gameObject.SetActive(value: false);
                actorFace.eyebrows.gameObject.SetActive(value: false);
                actorFace.mouth.gameObject.SetActive(value: false);
                actorFace.beard.gameObject.SetActive(value: false);
                actorFace.hair1.gameObject.SetActive(value: false);
                actorFace.hair2.gameObject.SetActive(value: false);
                actorFace.hairOther.gameObject.SetActive(value: false);
                actorFace.clothes.gameObject.SetActive(value: false);
                actorFace.clothesColor.gameObject.SetActive(value: false);
                actorFace.body.gameObject.SetActive(value: true);
                if (actorFace.smallSize)
                {
                    actorFace.body.sprite = Resources.Load<Sprite>("Graphics/ActorFaceSmall/NPCFace/NPCFace_" + faceDate[0].ToString());
                }
                else
                {
                    actorFace.body.sprite = Resources.Load<Sprite>("Graphics/ActorFace/NPCFace/NPCFace_" + faceDate[0].ToString());
                }
                actorFace.body.color = new Color(1f, 1f, 1f, 1f);
            }
            else
            {

                bool flag = life || false || false;
                if (!flag)
                {
                    actorFace.ageImage.gameObject.SetActive(value: false);
                    actorFace.nose.gameObject.SetActive(value: false);
                    actorFace.faceOther.gameObject.SetActive(value: false);
                    actorFace.eye.gameObject.SetActive(value: false);
                    actorFace.eyePupil.gameObject.SetActive(value: false);
                    actorFace.eyebrows.gameObject.SetActive(value: false);
                    actorFace.mouth.gameObject.SetActive(value: false);
                    actorFace.beard.gameObject.SetActive(value: false);
                    actorFace.hair1.gameObject.SetActive(value: false);
                    actorFace.hair2.gameObject.SetActive(value: false);
                    actorFace.hairOther.gameObject.SetActive(value: false);
                    actorFace.clothes.gameObject.SetActive(value: false);
                    actorFace.clothesColor.gameObject.SetActive(value: false);
                    actorFace.body.gameObject.SetActive(value: true);
                    if (actorFace.smallSize)
                    {
                        actorFace.body.sprite = Resources.Load<Sprite>("Graphics/ActorFaceSmall/NPCFace/NPCFace_Dead");
                    }
                    else
                    {
                        actorFace.body.sprite = Resources.Load<Sprite>("Graphics/ActorFace/NPCFace/NPCFace_Dead");
                    }
                    actorFace.body.color = new Color(1f, 1f, 1f, 1f);
                }
                else
                {
                    int num = (actorGenderChange == 0) ? (gender - 1) : ((gender == 1) ? 1 : 0);
                    int key = Mathf.Min(faceDate[0], DateFile.instance.ParseInt(GetSprites.instance.actorFaceDate[num][98]) - 1);
                    int key2 = Mathf.Min(faceDate[0], DateFile.instance.ParseInt(GetSprites.instance.actorFaceDate[num][99]) - 1);
                    actorFace.ageImage.gameObject.SetActive(flag);
                    actorFace.body.gameObject.SetActive(value: true);
                    actorFace.nose.gameObject.SetActive(flag);
                    actorFace.faceOther.gameObject.SetActive(flag);
                    actorFace.eye.gameObject.SetActive(flag);
                    actorFace.eyePupil.gameObject.SetActive(flag);
                    actorFace.eyebrows.gameObject.SetActive(flag);
                    actorFace.mouth.gameObject.SetActive(flag);
                    actorFace.beard.gameObject.SetActive(gender == 1 && num == 0 && age >= 20);
                    actorFace.hair1.gameObject.SetActive(flag || (!flag && faceDate[7] == 15));
                    actorFace.hair2.gameObject.SetActive(flag || (!flag && faceDate[7] == 15));
                    actorFace.hairOther.gameObject.SetActive(flag || (!flag && faceDate[7] == 15));
                    actorFace.clothes.gameObject.SetActive(value: true);
                    actorFace.clothesColor.gameObject.SetActive(value: true);
                    if (age <= 14)
                    {
                        Dictionary<int, Dictionary<int, Sprite[]>> dictionary = actorFace.smallSize ? GetSprites.instance.childFaceSmall : GetSprites.instance.childFace;
                        if (age <= 3)
                        {
                            actorFace.body.sprite = dictionary[num][key][2];
                            actorFace.nose.sprite = dictionary[num][key][0];
                            actorFace.eyePupil.sprite = dictionary[num][key][1];
                            actorFace.clothesColor.sprite = dictionary[num][key][3];
                            actorFace.body.color = new Color(1f, 1f, 1f);
                            actorFace.nose.color = DateFile.instance.faceColor[0][faceColor[0]];
                            actorFace.eyePupil.color = new Color(1f, 1f, 1f);
                            actorFace.clothesColor.color = DateFile.instance.faceColor[7][faceColor[7]];
                            actorFace.faceOther.gameObject.SetActive(value: false);
                            actorFace.eye.gameObject.SetActive(value: false);
                            actorFace.eyebrows.gameObject.SetActive(value: false);
                            actorFace.mouth.gameObject.SetActive(value: false);
                            actorFace.beard.gameObject.SetActive(value: false);
                            actorFace.hair1.gameObject.SetActive(value: false);
                            actorFace.hair2.gameObject.SetActive(value: false);
                            actorFace.hairOther.gameObject.SetActive(value: false);
                            actorFace.clothes.gameObject.SetActive(value: false);
                        }
                        else
                        {
                            actorFace.body.sprite = dictionary[num][key][4];
                            actorFace.nose.sprite = dictionary[num][key][5];
                            actorFace.eye.sprite = dictionary[num][key][6];
                            actorFace.eyePupil.sprite = dictionary[num][key][7];
                            actorFace.hair1.sprite = dictionary[num][key][9];
                            actorFace.clothesColor.sprite = dictionary[num][key][8];
                            actorFace.body.color = new Color(1f, 1f, 1f);
                            actorFace.nose.color = DateFile.instance.faceColor[0][faceColor[0]];
                            actorFace.eyePupil.color = DateFile.instance.faceColor[2][faceColor[2]];
                            actorFace.hair1.color = DateFile.instance.faceColor[6][faceColor[6]];
                            actorFace.clothesColor.color = DateFile.instance.faceColor[7][faceColor[7]];
                            actorFace.faceOther.gameObject.SetActive(value: false);
                            actorFace.eyebrows.gameObject.SetActive(value: false);
                            actorFace.mouth.gameObject.SetActive(value: false);
                            actorFace.beard.gameObject.SetActive(value: false);
                            actorFace.hair2.gameObject.SetActive(value: false);
                            actorFace.hairOther.gameObject.SetActive(value: false);
                            actorFace.clothes.gameObject.SetActive(value: false);
                        }
                    }
                    else
                    {
                        Dictionary<int, Dictionary<int, List<Sprite[]>>> dictionary2 = actorFace.smallSize ? GetSprites.instance.actorFaceSmall : GetSprites.instance.actorFace;
                        if (age >= 60)
                        {
                            actorFace.ageImage.sprite = dictionary2[num][key2][0][2];
                        }
                        else if (age >= 40)
                        {
                            actorFace.ageImage.sprite = dictionary2[num][key2][0][1];
                        }
                        else
                        {
                            actorFace.ageImage.sprite = dictionary2[num][key2][0][0];
                        }
                        actorFace.body.sprite = dictionary2[num][key2][1][(!flag) ? 1 : 0];
                        actorFace.nose.sprite = dictionary2[num][key2][2][faceDate[1]];
                        actorFace.faceOther.sprite = dictionary2[num][key2][3][faceDate[2]];
                        actorFace.eye.sprite = dictionary2[num][key2][4][faceDate[3]];
                        actorFace.eyePupil.sprite = dictionary2[num][key2][5][faceDate[3]];
                        actorFace.eyebrows.sprite = dictionary2[num][key2][6][faceDate[4]];
                        actorFace.mouth.sprite = dictionary2[num][key2][7][faceDate[5]];
                        if (actorFace.beard.gameObject.activeSelf)
                        {
                            actorFace.beard.sprite = dictionary2[0][key2][8][faceDate[6]];
                        }
                        actorFace.hair1.sprite = dictionary2[num][key2][9][faceDate[7]];
                        actorFace.hair2.sprite = dictionary2[num][key2][12][faceDate[7]];
                        actorFace.hairOther.sprite = dictionary2[num][key2][13][faceDate[7]];
                        if (clotheIndex != -1)
                        {
                            actorFace.clothes.sprite = dictionary2[num][key2][10][clotheIndex];
                            actorFace.clothesColor.sprite = dictionary2[num][key2][11][clotheIndex];
                        }
                        else
                        {
                            actorFace.clothes.sprite = dictionary2[num][key2][10][0];
                            actorFace.clothesColor.sprite = dictionary2[num][key2][11][0];
                        }
                        actorFace.body.color = (flag ? DateFile.instance.faceColor[0][faceColor[0]] : new Color(1f, 1f, 1f, 1f));
                        actorFace.nose.color = DateFile.instance.faceColor[0][faceColor[0]];
                        actorFace.eyebrows.color = DateFile.instance.faceColor[1][faceColor[1]];
                        actorFace.eyePupil.color = DateFile.instance.faceColor[2][faceColor[2]];
                        actorFace.mouth.color = DateFile.instance.faceColor[3][faceColor[3]];
                        if (age >= 70)
                        {
                            Color color = new Color(0.921568632f, 0.9490196f, 0.9647059f);
                            if (actorFace.beard.gameObject.activeSelf)
                            {
                                actorFace.beard.color = color;
                            }
                            actorFace.hair1.color = color;
                            actorFace.hair2.color = color;
                        }
                        else
                        {
                            if (actorFace.beard.gameObject.activeSelf)
                            {
                                actorFace.beard.color = DateFile.instance.faceColor[4][faceColor[4]];
                            }
                            actorFace.hair1.color = DateFile.instance.faceColor[6][faceColor[6]];
                            actorFace.hair2.color = DateFile.instance.faceColor[6][faceColor[6]];
                        }
                        actorFace.faceOther.color = DateFile.instance.faceColor[5][faceColor[5]];
                        actorFace.clothesColor.color = DateFile.instance.faceColor[7][faceColor[7]];
                        actorFace.hairOther.color = DateFile.instance.faceColor[7][faceColor[7]];
                    }
                }
            }
        }

        static bool CheckImageChange(int[] last_image,int length,int age, int gender, int actorGenderChange, int[] faceDate, int[] faceColor, int clotheIndex)
        {
            if (length == 1)
            {
                if(last_image[0]!=age||last_image[1]!= gender|| last_image[2]!= actorGenderChange|| last_image[19] != clotheIndex
                    || last_image[3]!= faceDate[0]
                    || last_image[11] != faceColor[0])
                {
                    last_image[0] = age;
                    last_image[1] = gender;
                    last_image[2] = actorGenderChange;
                    last_image[19] = clotheIndex;

                    last_image[3] = faceDate[0];

                    last_image[12] = faceColor[0];

                    return true;
                }
            }
            else
            {
                if (last_image[0] != age || last_image[1] != gender || last_image[2] != actorGenderChange || last_image[19] != clotheIndex

                    || last_image[3] != faceDate[0] || last_image[4] != faceDate[1] || last_image[5] != faceDate[2] || last_image[6] != faceDate[3]
                    || last_image[7] != faceDate[4] || last_image[8] != faceDate[5] || last_image[9] != faceDate[6] || last_image[10] != faceDate[7]

                    || last_image[11] != faceColor[0] || last_image[12] != faceColor[1] || last_image[13] != faceColor[2] || last_image[14] != faceColor[3]
                    || last_image[15] != faceColor[4] || last_image[16] != faceColor[5] || last_image[17] != faceColor[6] || last_image[18] != faceColor[7])
                {
                    last_image[0] = age;
                    last_image[1] = gender;
                    last_image[2] = actorGenderChange;
                    last_image[19] = clotheIndex;

                    last_image[3] = faceDate[0];
                    last_image[4] = faceDate[1];
                    last_image[5] = faceDate[2];
                    last_image[6] = faceDate[3];
                    last_image[7] = faceDate[4];
                    last_image[8] = faceDate[5];
                    last_image[9] = faceDate[6];
                    last_image[10] = faceDate[7];

                    last_image[11] = faceColor[0];
                    last_image[12] = faceColor[1];
                    last_image[13] = faceColor[2];
                    last_image[14] = faceColor[3];
                    last_image[15] = faceColor[4];
                    last_image[16] = faceColor[5];
                    last_image[17] = faceColor[6];
                    last_image[18] = faceColor[7];

                    return true;
                }
            }
            return false;
        }
    }
}