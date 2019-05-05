using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.Diagnostics;

namespace GuiQuquAdventure // 聊天室
{
    public class DataFile : MonoBehaviour
    {
        private string[] err_list = new string[]
        {
            "位置已被占用", // 0
            "请更新MOD版本，网站：www.yellowshange.com", // 1
        };
        /// <summary>
        /// 保存使用的蛐蛐出战次数
        /// </summary>
        public static readonly string ModSaveQuquCountKey = "1556454951";
        /// <summary>
        /// 保存战绩 胜负场次
        /// </summary>
        public static readonly string ModSaveWinRecordKey = "1556454952";
        Dictionary<string, int> ququCountData;
        public int[] winCountData;
        public void AddQuquCount(int[] color,int[] part)
        {
            for (int i = 0; i < color.Length; i++)
            {
                string ququ = $"{color[i]}#{part[i]}";
                if (ququCountData.ContainsKey(ququ))
                {
                    ququCountData[ququ]++;
                }
                else
                {
                    ququCountData.Add(ququ, 1);
                }
            }
            List<string> ss = new List<string>();
            foreach (var item in ququCountData)
            {
                ss.Add(item.Key + "," + item.Value);
            }
            PlayerPrefs.SetString(ModSaveQuquCountKey, string.Join("｜", ss));
        }
        public void AddWinRecord(bool isWin)
        {
            winCountData[isWin ? 0 : 1]++;
            PlayerPrefs.SetString(ModSaveWinRecordKey, $"{winCountData[0]},{winCountData[1]}");
        }

        public string GetLoveQuqu()
        {
            string ququ = "";
            int max = 0;
            foreach (var item in ququCountData)
            {
                if(item.Value > max)
                {
                    max = item.Value;
                    ququ = item.Key;
                }
            }
            return ququ;
        }

        public HallData hall_data;
        public static DataFile instance;
        private string url;

        private void Awake()
        {
            hall_data = new HallData();
            instance = this;
            url = @"http://www.yellowshange.com/taiwu_server/cricket_battle/server2.php";

            // 初始化蛐蛐出战次数
            ququCountData = new Dictionary<string, int>();
            string ququ_count_str = PlayerPrefs.GetString(ModSaveQuquCountKey, "");
            string[] ququ_count_list = ququ_count_str.Split('｜');
            foreach (string item in ququ_count_list)
            {
                string[] ququ_count = item.Split(',');
                if(ququ_count.Length > 1)
                {
                    ququCountData.Add(ququ_count[0], int.Parse(ququ_count[1]));
                }
            }

            // 初始化胜负场次数
            winCountData = new int[2];
            string win_record_str = PlayerPrefs.GetString(ModSaveWinRecordKey,"");
            string[] win_count_list = win_record_str.Split(',');
            if(win_count_list.Length > 1)
            {
                winCountData[0] = int.Parse(win_count_list[0]);
                winCountData[1] = int.Parse(win_count_list[1]);
            }
        }




        // 进入大厅 请求服务器大厅数据 10001
        public void GetHallData(Action<string, long, RoomData[]> fun, string name, string image)
        {
            try
            {
                StartCoroutine(HttpGetHallData(fun, name, image));
            }
            catch (Exception e)
            {
                StackTrace st = new StackTrace(true);
                string err = "-10001:http2:" + e.Message + "\n" + st.ToString();
                DebugError(e, err);
                fun(err, 1, null);
            }
        }

        IEnumerator HttpGetHallData(Action<string, long, RoomData[]> fun, string name, string image)
        {
            int msg = 10001; // 协议号
            WWWForm form = new WWWForm();
            form.AddField("msg", msg);
            form.AddField("name", name);
            form.AddField("image", image);
            using (var getData = UnityWebRequest.Post(url, form))
            {
                yield return getData.SendWebRequest();
                if (getData.isNetworkError || getData.isHttpError)
                {
                    string err = msg + ":http:" + getData.error;
                    fun(err, 0, null);
                }
                else
                {
                    // // // Main.Logger.Log(getData.downloadHandler.text);
                    int pos = 0;
                    try
                    {
                        // // // // Main.Logger.Log(getData.downloadHandler.text);
                        string[] data = getData.downloadHandler.text.Split('|');
                        long time_stamp = long.Parse(data[pos++]); // 第一个数据是时间戳
                                                                   // // // // // Main.Logger.Log("/ 第一个数据是时间戳");
                        PlayerData.self.ip = data[pos++];
                        int errId = int.Parse(data[pos++]); // 错误码
                                                            // // // // // Main.Logger.Log("/ 错误码");
                        if (errId > -1)
                        {
                            string err = err_list[errId];
                            fun(err, time_stamp, null);
                        }
                        else
                        {
                            PlayerData.online_count = int.Parse(data[pos++]); // 在线人数

                            int count = int.Parse(data[pos++]); // 房间个数
                                                                // // // // // Main.Logger.Log("/ 第二个数据是房间个数");

                            // 保存数据到本地
                            if (null == hall_data.room_data || hall_data.room_data.Length != count)
                            {
                                hall_data.room_data = new RoomData[count];
                            }
                            for (int i = 0; i < count; i++) // 循环获取每个房间的等级和人数
                            {
                                RoomData room_data = hall_data.room_data[i];
                                if (null == room_data)
                                {
                                    room_data = new RoomData();
                                    hall_data.room_data[i] = room_data;
                                }
                                room_data.level = int.Parse(data[pos++]); // 房间等级
                                                                          // // // // // Main.Logger.Log("/ 房间等级");
                                room_data.people_num = int.Parse(data[pos++]); // 房间人数
                                                                               // // // // // Main.Logger.Log("/ 房间人数");
                            }
                            if (pos < data.Length)
                            {
                                // // // Main.Logger.Log("!!!" + data[pos]);
                                YesOrNoWindow.instance.SetYesOrNoWindow(-1, "服务器消息!", data[pos], false, true);
                            }

                            fun(null, time_stamp, hall_data.room_data);
                        }
                    }
                    catch (Exception e)
                    {
                        StackTrace st = new StackTrace(true);
                        string err = pos + "-" + msg + ":http2:" + e.Message + "\n" + st.ToString();
                        if (getData.downloadHandler.text != null)
                        {
                            err += "\n httptext " + getData.downloadHandler.text;
                        }
                        if (getData.error != null)
                        {
                            err += "\n httperror " + getData.error;
                        }
                        DebugError(e, err);
                        fun(err, 1, null);
                    }
                }
            }
        }

        // 进入房间 请求服务器房间数据 10002
        public void GetRoomData(Action<string, long, RoomData> fun, string name, int room_idx, long last_chat_time_stamp, string image, string chat_content = null, string chat_param = null)
        {
            try
            {
                StartCoroutine(HttpGetRoomData(fun, name, room_idx, last_chat_time_stamp, image, chat_content, chat_param));
            }
            catch (Exception e)
            {
                StackTrace st = new StackTrace(true);
                string err = "-10001:http2:" + e.Message + "\n" + st.ToString();
                DebugError(e, err);
                fun(err, 1, null);
            }
        }
        IEnumerator HttpGetRoomData(Action<string, long, RoomData> fun, string name, int room_idx, long last_chat_time_stamp, string image, string chat_content, string chat_param)
        {
            int msg = 10002; // 协议号
            WWWForm form = new WWWForm();
            form.AddField("msg", msg);
            form.AddField("room_idx", room_idx);
            form.AddField("name", name);
            form.AddField("lcts", last_chat_time_stamp.ToString());
            form.AddField("image", image);
            if (null != chat_content && null != chat_param)
            {
                form.AddField("chat_content", chat_content);
                form.AddField("chat_param", chat_param);
            }
            using (var getData = UnityWebRequest.Post(url, form))
            {
                yield return getData.SendWebRequest();
                if (getData.isNetworkError || getData.isHttpError)
                {
                    string err = msg + ":http:" + getData.error;
                    fun(err, 0, null);
                }
                else
                {
                    // // // Main.Logger.Log(getData.downloadHandler.text);
                        int pos = 0;
                    try
                    {
                        // // // // Main.Logger.Log(getData.downloadHandler.text);
                        string[] data = getData.downloadHandler.text.Split('|');
                        long time_stamp = long.Parse(data[pos++]); // 时间戳
                                                                   // // // // // Main.Logger.Log("/ 时间戳");
                        PlayerData.self.ip = data[pos++];
                        int errId = int.Parse(data[pos++]); // 错误码
                                                            // // // // // Main.Logger.Log("/ 错误码");
                        if (errId > -1)
                        {
                            string err = err_list[errId];
                            fun(err, time_stamp, null);
                        }
                        else
                        {
                            PlayerData.online_count = int.Parse(data[pos++]); // 在线人数

                            int idx = int.Parse(data[pos++]); // 房间索引
                                                              // // // // // Main.Logger.Log("/ 房间索引");
                            int count = int.Parse(data[pos++]); // 房间个数
                                                                // // // // // Main.Logger.Log("/ 房间个数");

                            // 保存数据到本地
                            if (null == hall_data.room_data || hall_data.room_data.Length != count)
                            {
                                hall_data.room_data = new RoomData[count];
                            }
                            RoomData room_data = hall_data.room_data[idx];
                            if (null == room_data)
                            {
                                room_data = new RoomData();
                                hall_data.room_data[idx] = room_data;
                            }
                            room_data.level = int.Parse(data[pos++]); // 房间等级
                                                                      // // // // // Main.Logger.Log("/ 房间等级");
                            room_data.people_num = int.Parse(data[pos++]); // 房间人数
                                                                           // // // // // Main.Logger.Log("/ 房间人数");
                            int desk_count = int.Parse(data[pos++]); // 房间的桌子数量
                                                                     // // // // // Main.Logger.Log("/ 房间的桌子数量");
                            if (null == room_data.desk_mark || room_data.desk_mark.Length != desk_count)
                            {
                                room_data.desk_mark = new int[desk_count];
                            }
                            if (null == room_data.desk_data || room_data.desk_data.Length != desk_count)
                            {
                                room_data.desk_data = new DeskData[desk_count];
                            }
                            for (int i = 0; i < desk_count; i++)
                            {
                                int mark = int.Parse(data[pos++]); // 每个桌子人数标识 1和2表示是否有对战者 4 8 16 32 64 128 256 516 1024 2048 分别表示是否存在的最多10个观战  4096 表示房间类型
                                                                   // // // // // Main.Logger.Log("/ 每个桌子人数标识 1和2表示是否有对战者 4 8 16 32 64 128 256 516 1024 2048 分别表示是否存在的最多10个观战  4096 表示房间类型");
                                room_data.desk_mark[i] = mark; // 1
                                DeskData desk_data = room_data.desk_data[i];
                                if (null == desk_data)
                                {
                                    desk_data = new DeskData();
                                    room_data.desk_data[i] = desk_data;
                                }
                                desk_data.idx = i; // 桌子序号
                                desk_data.typ = mark & (1 << 12); // 桌子类型
                            }
                            int player_count = room_data.people_num;
                            if (null == room_data.player_data || room_data.player_data.Length != player_count)
                            {
                                room_data.player_data = new PlayerData[player_count];
                            }
                            for (int i = 0; i < player_count; i++)
                            {
                                PlayerData player_data = room_data.player_data[i];
                                if (null == player_data)
                                {
                                    player_data = new PlayerData();
                                    room_data.player_data[i] = player_data;
                                }
                                player_data.name = data[pos++]; // 玩家名字
                                                                // // // // // Main.Logger.Log("/ 玩家名字");
                                player_data.ip = data[pos++]; // ip地址
                                                              // // // // // Main.Logger.Log("/ ip地址");
                                player_data.level = int.Parse(data[pos++]); // 所在房间
                                player_data.desk_idx = int.Parse(data[pos++]); // 所在桌子
                                player_data.observer = int.Parse(data[pos++]); // 是否观战
                                player_data.desk_pos = int.Parse(data[pos++]); // 所在桌子位置
                                // // // // // Main.Logger.Log("/ 所在桌子");
                                player_data.SetImage(data[pos++]); // 设置形象
                                                                   // // // // // Main.Logger.Log("/ 设置形象");
                            }
                            int chat_count = int.Parse(data[pos++]); // 聊天记录数量
                                                                     // // // // // Main.Logger.Log("/ 聊天记录数量");
                            for (int i = 0; i < chat_count; i++)
                            {
                                long ts = long.Parse(data[pos++]); // 发言时间
                                                                   // // // // // Main.Logger.Log("/ 发言时间");
                                string n = data[pos++]; // 发言者名字
                                                        // // // // // Main.Logger.Log("/ 发言者名字");
                                string p = data[pos++]; // 发言者ip
                                                        // // // // // Main.Logger.Log("/ 发言者ip");
                                string content = data[pos++]; // 发言内容
                                                              // // // // // Main.Logger.Log("/ 发言内容");
                                string param = data[pos++]; // 发言参数
                                                            // // // // // Main.Logger.Log("/ 发言参数");
                                room_data.chat_data.Add(new ChatData() { time_stamp = ts, name = n, ip = p, content = content, param = param });
                            }
                            if (pos < data.Length)
                            {
                                // // // Main.Logger.Log("!!!" + data[pos]);
                                YesOrNoWindow.instance.SetYesOrNoWindow(-1, "服务器消息!", data[pos], false, true);
                            }
                            fun(null, time_stamp, room_data);
                        }
                    }
                    catch (Exception e)
                    {
                        StackTrace st = new StackTrace(true);
                        string err = pos + "-" + msg + ":http2:" + e.Message + "\n" + st.ToString();
                        if (getData.downloadHandler.text != null)
                        {
                            err += "\n httptext " + getData.downloadHandler.text;
                        }
                        if (getData.error != null)
                        {
                            err += "\n httperror " + getData.error;
                        }
                        DebugError(e, err);
                        fun(err, 1, null);
                    }
                }
            }
        }



        /// <summary>
        /// 请求服务器桌子数据 10003
        /// </summary>
        /// <param name="fun">回调方法</param>
        /// <param name="name">玩家名字</param>
        /// <param name="room_idx">房间索引</param>
        /// <param name="desk_idx">桌子索引</param>
        /// <param name="ready">准备状态</param>
        /// <param name="observer">是否观战</param>
        /// <param name="last_chat_time_stamp">当前聊天数据的最后一次发言时间</param>
        /// <param name="last_battle_time_stamp">当前战斗数据的最后一次战斗时间</param>
        /// <param name="bet">押注物品id</param>
        public void GetDeskData(Action<string, long, DeskData, string> fun, string name, int room_idx, int desk_idx, int ready, int observer, long last_chat_time_stamp, long last_battle_time_stamp, string bet, string[] ququ, string image,int desk_pos, string chat_content = null, string chat_param = null)
        {
            try
            {
                StartCoroutine(HttpGetDeskData(fun, name, room_idx, desk_idx, ready, observer, last_chat_time_stamp, last_battle_time_stamp, bet, ququ, image, desk_pos, chat_content, chat_param));
            }
            catch (Exception e)
            {
                StackTrace st = new StackTrace(true);
                string err = "-10001:http2:" + e.Message + "\n" + st.ToString();
                DebugError(e, err);
                fun(err, 0, null, "0");
            }
        }

        IEnumerator HttpGetDeskData(Action<string, long, DeskData, string> fun, string name, int room_idx, int desk_idx, int ready, int observer, long last_chat_time_stamp, long last_battle_time_stamp, string bet, string[] ququ, string image,int desk_pos, string chat_content, string chat_param)
        {
            int msg = 10003; // 协议号
            WWWForm form = new WWWForm();
            // // // // Main.Logger.Log("!!!");
            form.AddField("msg", msg);
            form.AddField("name", name);
            // // // // Main.Logger.Log("!!!");
            // form.AddField("room_idx", room_idx);
            form.AddField("desk_idx", room_idx * 100 + desk_idx);
            form.AddField("ready", ready);
            // // // // Main.Logger.Log("!!!222");
            form.AddField("observer", observer);
            form.AddField("lcts", last_chat_time_stamp.ToString());
            // // // // Main.Logger.Log("!!!222");
            form.AddField("lbts", last_battle_time_stamp.ToString());
            // // // // Main.Logger.Log("!!!222433334");
            form.AddField("bet", bet);
            // // // // Main.Logger.Log("!!!2224333345454");
            form.AddField("ququ", string.Join(",", ququ));
            // // // // Main.Logger.Log("!!!22243333454547777");
            form.AddField("image", image);
            form.AddField("desk_pos", desk_pos);
            // // // // Main.Logger.Log("!!!2224333345454777557");
            if (null != chat_content && null != chat_param)
            {
                form.AddField("chat_content", chat_content);
                form.AddField("chat_param", chat_param);
            }
            using (var getData = UnityWebRequest.Post(url, form))
            {
                yield return getData.SendWebRequest();
                if (getData.isNetworkError || getData.isHttpError)
                {
                    string err = msg + ":http:" + getData.error;
                    fun(err, 0, null, "0");
                }
                else
                {

                    // // // Main.Logger.Log("!!!！！！");
                    // // // Main.Logger.Log(getData.downloadHandler.text);
                        int pos = 0;
                    try
                    {
                        // // // Main.Logger.Log(getData.downloadHandler.text);
                        string[] data = getData.downloadHandler.text.Split('|');
                        // // // // Main.Logger.Log("/ 哈哈 "+ data[pos]+pos);
                        // // // // Main.Logger.Log(" !! "+long.Parse(data[pos]));
                        long time_stamp = long.Parse(data[pos++]); // 时间戳
                                                                   // // // // Main.Logger.Log("/ 时间戳"+time_stamp);
                        PlayerData.self.ip = data[pos++];
                        int errId = int.Parse(data[pos++]); // 错误码
                                                            // // // // Main.Logger.Log("/ 错误码"+errId);
                        if (errId > -1)
                        {
                            string err = err_list[errId];
                            fun(err, time_stamp, null, "0");
                        }
                        else
                        {
                            PlayerData.online_count = int.Parse(data[pos++]); // 在线人数

                            int idx = int.Parse(data[pos++]); // 房间索引
                                                              // // // // Main.Logger.Log("/ 房间索引"+idx);
                            int count = int.Parse(data[pos++]); //房间个数
                                                                // // // // Main.Logger.Log("/ 房间个数"+count);

                            // 保存数据到本地
                            if (null == hall_data.room_data || hall_data.room_data.Length != count)
                            {
                                hall_data.room_data = new RoomData[count];
                            }
                            RoomData room_data = hall_data.room_data[idx];
                            if (null == room_data)
                            {
                                room_data = new RoomData();
                                hall_data.room_data[idx] = room_data;
                            }
                            int idx2 = int.Parse(data[pos++]) % 100; // 桌子索引
                                                                     // // // // Main.Logger.Log("/ 桌子索引"+idx2);
                            int count2 = int.Parse(data[pos++]); // 桌子数量
                                                                 // // // // Main.Logger.Log("/ 桌子数量"+count2);
                            if (null == room_data.desk_data || room_data.desk_data.Length != count2)
                            {
                                room_data.desk_data = new DeskData[count2];
                            }
                            DeskData desk_data = room_data.desk_data[idx2];
                            if (null == desk_data)
                            {
                                desk_data = new DeskData();
                                desk_data.idx = idx2;
                                room_data.desk_data[idx2] = desk_data;
                            }
                            int typ = int.Parse(data[pos++]); // 桌子类型
                                                              // // // // Main.Logger.Log("/ 桌子类型"+typ);
                            desk_data.typ = typ;

                            int player_count = int.Parse(data[pos++]); // 玩家数量
                                                                       // // // // Main.Logger.Log("/ 玩家数量"+player_count);
                            if (null == desk_data.player_data || desk_data.player_data.Length == player_count)
                            {
                                desk_data.player_data = new PlayerData[player_count];
                            }
                            for (int i = 0; i < player_count; i++)
                            {
                                PlayerData player_data = desk_data.player_data[i];
                                if (null == player_data)
                                {
                                    player_data = new PlayerData();
                                    desk_data.player_data[i] = player_data;
                                }
                                player_data.name = data[pos++]; // 玩家名字
                                                                // // // // Main.Logger.Log("/ 玩家名字" + player_data.name);
                                player_data.ip = data[pos++]; // ip地址
                                                              // // // // Main.Logger.Log("/ ip地址"+ player_data.ip);
                                player_data.observer = int.Parse(data[pos++]); // 0非游客 1普通游客 2押0号注游客 3押1号注游客
                                                                               // // // // Main.Logger.Log("/ 0非游客 1普通游客 2押0号注游客 3押1号注游客"+ player_data.observer);
                                player_data.ququ = new string[] { data[pos++], data[pos++], data[pos++] }; //[3] 出战蛐蛐
                                                                                                           // // // // Main.Logger.Log("/[3] 出战蛐蛐"+ player_data.ququ);
                                player_data.time_stamp = long.Parse(data[pos++]); // 心跳时间
                                                                                  // // // // Main.Logger.Log("/ 心跳时间"+time_stamp);
                                player_data.ready = int.Parse(data[pos++]); // 准备 0是未准备 1是确认赌注 2是准备好了
                                                                            // // // // Main.Logger.Log("/ 准备 0是未准备 1是确认赌注 2是准备好了"+ player_data.ready);
                                player_data.level = idx;
                                player_data.desk_idx = idx2;
                                player_data.bet = data[pos++]; // 赌注
                                                               // // // // Main.Logger.Log("/ 赌注"+ player_data.bet);
                                player_data.SetImage(data[pos++]); // 设置玩家形象
                                                                   // // // // Main.Logger.Log("/ 设置玩家形象"+ player_data.GetImage());
                            }

                            int battle_count = int.Parse(data[pos++]); // 战斗数量
                                                                       // // // // Main.Logger.Log("/ 战斗数量"+battle_count);
                            if (battle_count > 0)
                            {
                                // // // // Main.Logger.Log(getData.downloadHandler.text);
                                // // // // Main.Logger.Log("读取创建战斗数据");
                                long battle_time_stamp = long.Parse(data[pos++]); // 战斗时间
                                                                                  // // // // Main.Logger.Log("/ 战斗时间"+battle_time_stamp);
                                PlayerData player_data1 = new PlayerData();
                                player_data1.name = data[pos++]; // 玩家名字
                                                                 // // // // Main.Logger.Log("/ 玩家名字"+ player_data1.name);
                                player_data1.ip = data[pos++]; // ip地址
                                                               // // // // Main.Logger.Log("/ ip地址" + player_data1.ip);
                                player_data1.observer = int.Parse(data[pos++]); // 0非游客 1普通游客 2押注游客
                                                                                // // // // Main.Logger.Log("/ 0非游客 1普通游客 2押注游客" + player_data1.observer);
                                player_data1.ququ = new string[] { data[pos++], data[pos++], data[pos++] }; //[3] 出战蛐蛐
                                                                                                            // // // // Main.Logger.Log("/[3] 出战蛐蛐" + player_data1.ququ);
                                player_data1.time_stamp = long.Parse(data[pos++]); // 心跳时间
                                                                                   // // // // Main.Logger.Log("/ 心跳时间"+ player_data1.time_stamp);
                                player_data1.ready = int.Parse(data[pos++]); // 准备 0是未准备 1是确认赌注 2是准备好了
                                                                             // // // // Main.Logger.Log("/ 准备 0是未准备 1是确认赌注 2是准备好了" + player_data1.ready);
                                player_data1.level = idx;
                                player_data1.desk_idx = idx2;
                                player_data1.bet = data[pos++]; // 赌注
                                                                // // // // Main.Logger.Log("/ 赌注"+ player_data1.bet);
                                player_data1.SetImage(data[pos++]); // 形象
                                                                    // // // // Main.Logger.Log("/ 形象" + player_data1.GetImage());
                                PlayerData player_data2 = new PlayerData();
                                player_data2.name = data[pos++]; // 玩家名字
                                                                 // // // // Main.Logger.Log("/ 玩家名字" + player_data2.name);
                                player_data2.ip = data[pos++]; // ip地址
                                                               // // // // Main.Logger.Log("/ ip地址"+ player_data2.ip);
                                player_data2.observer = int.Parse(data[pos++]); // 0非游客 1普通游客 2押注游客
                                                                                // // // // Main.Logger.Log("/ 0非游客 1普通游客 2押注游客"+ player_data2.observer);
                                player_data2.ququ = new string[] { data[pos++], data[pos++], data[pos++] }; //[3] 出战蛐蛐
                                                                                                            // // // // Main.Logger.Log("/[3] 出战蛐蛐" + player_data2.ququ);
                                player_data2.time_stamp = long.Parse(data[pos++]); // 心跳时间
                                                                                   // // // // Main.Logger.Log("/ 心跳时间" + player_data2.time_stamp);
                                player_data2.ready = int.Parse(data[pos++]); // 准备 0是未准备 1是确认赌注 2是准备好了
                                                                             // // // // Main.Logger.Log("/ 准备 0是未准备 1是确认赌注 2是准备好了" + player_data2.ready);
                                player_data2.level = idx;
                                player_data2.desk_idx = idx2;
                                player_data2.bet = data[pos++]; // 赌注
                                                                // // // // Main.Logger.Log("/ 赌注"+ player_data2.bet);
                                player_data2.SetImage(data[pos++]); // 形象
                                                                    // // // // Main.Logger.Log("/ 形象" + player_data2.GetImage());
                                BattleData battle_data = new BattleData() { time_stamp = battle_time_stamp, player_data = new PlayerData[2] { player_data1, player_data2 } };
                                //BattleData.battleDatas.Add(battle_data);
                                // // // // Main.Logger.Log("当前战斗总数" + BattleData.battleDatas.Count);
                            }
                            int chat_count = int.Parse(data[pos++]); // 聊天数量
                                                                     // // // // Main.Logger.Log("/ 聊天数量"+chat_count);
                            for (int i = 0; i < chat_count; i++)
                            {
                                long ts = long.Parse(data[pos++]); // 发言时间
                                                                   // // // // Main.Logger.Log("/ 发言时间"+ts);
                                string n = data[pos++]; // 发言者名字
                                                        // // // // Main.Logger.Log("/ 发言者名字"+n);
                                string p = data[pos++]; // 发言者ip
                                                        // // // // Main.Logger.Log("/ 发言者ip"+p);
                                string content = data[pos++]; // 发言内容
                                                              // // // // Main.Logger.Log("/ 发言内容"+content);
                                string param = data[pos++]; // 发言参数
                                                            // // // // Main.Logger.Log("/ 发言参数"+param);

                                ChatData chatData = new ChatData() { time_stamp = ts, name = n, ip = p, content = content, param = param };
                                desk_data.chat_data.Add(chatData);
                                if (GuiQuquBattleSystem.instance.showQuquBattleWindow || GuiQuquBattleSystem.instance.gameObject.activeSelf || GuiQuquBattleSystem.instance.chatDatas != null)
                                {
                                    // // Main.Logger.Log("加入一條聊天彈幕");
                                    GuiQuquBattleSystem.instance.chatDatas.Enqueue(chatData);
                                }
                                else
                                {
                                    // // Main.Logger.Log("加入一條聊天");
                                }
                            }

                            string battle_flag = data[pos++]; // 触发战斗
                                                              // // // // Main.Logger.Log("/ 触发战斗"+battle_flag);
                            if (pos < data.Length)
                            {
                                // // // Main.Logger.Log("!!!" + data[pos]);
                                YesOrNoWindow.instance.SetYesOrNoWindow(-1, "服务器消息!", data[pos], false, true);
                            }

                            fun(null, time_stamp, desk_data, battle_flag);
                        }
                    }
                    catch (Exception e)
                    {
                        StackTrace st = new StackTrace(true);
                        string err = pos + "-" + msg + ":http2:" + e.Message + "\n" + st.ToString();
                        if (getData.downloadHandler.text != null)
                        {
                            err += "\n httptext " + getData.downloadHandler.text;
                        }
                        if (getData.error != null)
                        {
                            err += "\n httperror " + getData.error;
                        }
                        DebugError(e, err);
                        fun(err, 1, null, "0");
                    }
                }
            }
        }

        void DebugError(Exception e, string err)
        {
            string s = "error:" + err ;
            if (e.Source != null)
            {
                s += "\n Source:" + e.Source;
            }
            if (e.StackTrace != null)
            {
                s += "\n StackTrace:" + e.StackTrace;
            }
            // // Main.Logger.Log(s);
            UnityEngine.Debug.Log(s);
        }
    }

    #region 大厅数据汇总
    public class HallData // 大厅数据
    {
        public RoomData[] room_data; //[11] 各个房间的数据
    }

    public class RoomData // 房间数据
    {
        public int level; // 房间等级 0级赌注无限制 1级需要1品物品作为赌注 2级需要2品物品作为赌注
        public int people_num; // 人数
        public int[] desk_mark; //[100] 每个桌子人数标识 1和2表示是否有对战者 4 8 16 32 64 128 256 516 1024 2048 分别表示是否存在的最多10个观战  4096 表示房间类型
        public DeskData[] desk_data; //[100] 房间的桌子
        public PlayerData[] player_data; //[300] 玩家数据
        public List<ChatData> chat_data = new List<ChatData>() ; //[50] 聊天记录

    }


    public class DeskData // 桌子数据
    {
        public int idx; // 桌子索引 0 - 99
        public int typ; // 类型 0是玩家对赌 胜利者获得对方的赌注 1是系统对赌 胜利者获得系统赠予的一份和自己赌注一样的物品
        public int people_num; // 人数
        public PlayerData[] player_data; //[12] 玩家数据 0是房主 1是挑战者 其他是游客
        public List<ChatData> chat_data = new List<ChatData>(); //[50] 聊天记录

        public int need_level { get { return idx / 10; } }

        readonly static int[,] resource_worth = new int[9, 10]
        {
            {
                5000,500,1000,2500,5000,9000,15000,25000,40000,80000, // 赌桌对资源:粮食的要求
            },
            {
                5000,500,1000,2500,5000,9000,15000,25000,40000,80000,
            },
            {
                5000,500,1000,2500,5000,9000,15000,25000,40000,80000,
            },
            {
                5000,500,1000,2500,5000,9000,15000,25000,40000,80000,
            },
            {
                5000,500,1000,2500,5000,9000,15000,25000,40000,80000,
            },
            {
                5000,500,1000,2500,5000,9000,15000,25000,40000,80000,
            },
            {
                2500,250,500,1250,2500,4500,7500,12500,20000,40000, // 赌桌对资源:威望的要求
            },
            {
                0,100,300,450,900,1250,1800,2450,3600,6400, // 赌桌对物品价值的要求
            },
            {
                0,200,350,550,800,1100,1400,2000,3000,5000, // 赌桌对人物身价的要求
            },
        };

        public static int GetRoomNeedResource(int bet_typ, int bet_id, int level)
        {
            // // // Main.Logger.Log("获取房间需要的资源 bet_typ=" + bet_typ + " level=" + level);
            if(level<0|| level > 9)
            {
                level = 0;
            }
            // // // Main.Logger.Log("获取房间需要的资源 bet_typ=" + bet_typ + " level=" + level+" !!="+ resource_worth[bet_typ, level]);
            return resource_worth[bet_typ, level];
        }
    }

    public class PlayerData // 玩家数据
    {
        public static int online_count = -0; // 在线人数
        public static PlayerData self;
        public static int client_bet = -98; // 客户端押注的物品id
        public static int[] client_ids = new int[] { -98, -98, -98 }; // 客户端玩家出战的蛐蛐
        private string m_name;
        /// <summary>
        ///  玩家名字
        /// </summary>
        public string name
        {
            get
            {
                return m_name;
            }
            set
            {
                m_name = value;
                string[] ss = value.Split('｜');
                if (ss.Length > 0)
                {
                    player_name = ss[0];
                }
                if (ss.Length > 1)
                {
                    string[] record_list = ss[1].Split( '/');
                    if (record_list.Length > 1)
                    {
                        win = int.Parse(record_list[0]);
                        lose = int.Parse(record_list[1]);
                    }
                }
                if (ss.Length > 2)
                {
                    love_ququ = ss[2];
                }
            }
        }
        private string m_ip;
        public string ip { get
            {
                return m_ip;
            }
            set
            {
                m_ip = value;
                string[] ss = value.Split('.');
                if (ss.Length > 3)
                {
                    int.TryParse(ss[0], out r);
                    int.TryParse(ss[1], out g);
                    int.TryParse(ss[2], out b);
                    int.TryParse(ss[3], out a);
                }
            }
        } // ip地址
        public int observer; // -1是空 0非游客 1普通游客 2押注左边选手的游客 3押注右边选手的游客
        public string[] ququ; //[3] 出战蛐蛐
        public long time_stamp; // 心跳时间
        public int ready; // 准备 0是未准备 1是确认赌注 2是准备好了
        public int level; // 所在房间
        public int desk_idx; // 所在桌子
        public int desk_pos; // 所在桌子位置 在大厅中使用
        public string bet; // 赌注数据
        public int bet_id; // 赌注id
        public int bet_typ; // 赌注类型
        public string love_ququ = "无"; // 最爱的蛐蛐
        public string player_name = "太吾传人"; // 玩家名字
        public int win, lose;
        public int r, g, b, a;

        public PlayerData()
        {
            faceDate = new int[] { 5, 21, 0, 16, 41, 21, 12, 18 };
            faceColor = new int[] { 2, 9, 3, 9, 5, 6, 0, 4, 28 };
            ququ = new string[3] { "0", "0", "0" };
            bet_id = -98;
            bet_typ = -1;
            bet = "0";
        }

        public static void Sort(PlayerData[] players)
        {
            for (int i = 0; i < players.Length - 1; i++)
            {
                for (int j = i + 1; j < players.Length; j++)
                {
                    PlayerData p1 = players[i];
                    PlayerData p2 = players[j];
                    int i1 = p1.level * 100000000 + p1.r * 1000 + p1.g * 100 + p1.b * 10 + p1.a;
                    int i2 = p2.level * 100000000 + p2.r * 1000 + p2.g * 100 + p2.b * 10 + p2.a;
                    if (p1.level == self.level)
                    {
                        i1 += 10000000;
                    }
                    if (p2.level == self.level)
                    {
                        i2 += 10000000;
                    }
                    if (p1 == self)
                    {
                        i1 += 1000000000;
                    }
                    if (p2 == self)
                    {
                        i2 += 1000000000;
                    }
                    if (i2 > i1)
                    {
                        players[j] = p1;
                        players[i] = p2;
                    }
                }
            }
        }

        public string GetLoveQuquName()
        {
            string ququ_name = "无";
            string[] ss = love_ququ.Split('#');
            if (ss.Length > 1)
            {
                if (int.TryParse(ss[0], out int color) && int.TryParse(ss[1], out int partid))
                {
                    var cricketDate = DateFile.instance.cricketDate;
                    ququ_name = ((partid <= 0) ? cricketDate[color][0] : ((int.Parse(cricketDate[color][2]) >= int.Parse(cricketDate[partid][2])) ? (cricketDate[color][0].Split('|')[0] + cricketDate[partid][0]) : (cricketDate[partid][0] + cricketDate[color][0].Split('|')[1])));
                }
            }

            return $"{player_name} 胜{win} 负{lose} 最爱:{ququ_name}";
        }

        /// <summary>
        /// 确认赌注是否适合该房间
        /// </summary>
        /// <returns></returns>
        public static bool ChechBet(bool show_tips)
        {
            // // Main.Logger.Log("确认赌注！" + show_tips);
            //int dayTime = DateFile.instance.dayTime;
            //if (dayTime < 3)
            //{
            //    if (show_tips)
            //    {
            //        YesOrNoWindow.instance.SetYesOrNoWindow(-1, "力竭", "这个月行动力已用完，下个月再送吧！", false, true);
            //    }
            //    return false;
            //}
            int desk_level = self.desk_idx / 10;
            try
            {
                switch (PlayerData.self.bet_typ)
                {
                    case 0:// 资源
                        // // Main.Logger.Log("资源！");
                        int need = DeskData.GetRoomNeedResource(self.bet_id, self.bet_id, desk_level);
                        int[] array = ActorMenu.instance.ActorResource(DateFile.instance.MianActorID());
                        int has = array[self.bet_typ];
                        if (need > has)
                        {
                            if (show_tips)
                            {
                                string name = DateFile.instance.resourceDate[self.bet_id][1];
                                YesOrNoWindow.instance.SetYesOrNoWindow(-1, "赌注资源不足!", $"当前赌桌每场对赌需要{need}的{name},您当前拥有{has}的{name},不足以参加对赌,您可以更换赌注或者选择旁观!", false, true);
                            }
                            goto cantInfo;
                        }
                        break;
                    case 1:// 物品
                        // // Main.Logger.Log("物品！");
                        int need_worth = DeskData.GetRoomNeedResource(7, self.bet_id, desk_level);
                        int item_worth = int.Parse(DateFile.instance.GetItemDate(PlayerData.client_bet, 904)); // 物品价值
                        if (need_worth > item_worth)
                        {
                            if (show_tips)
                            {
                                YesOrNoWindow.instance.SetYesOrNoWindow(-1, "赌注物品太烂!", $"当前赌桌每场对赌需要价值{need_worth}以上的赌注,您当前赌注只值{item_worth},不足以参加对赌,您可以更换赌注或者选择旁观!", false, true);
                            }
                            goto cantInfo;
                        }
                        break;
                    case 2:// 人物
                        // // Main.Logger.Log("人物！");
                        int room_worth = DeskData.GetRoomNeedResource(8, self.bet_id, desk_level);
                        int you_worth = int.Parse(DateFile.instance.GetItemDate(PlayerData.client_bet, 904)); // 物品价值
                        if (room_worth > you_worth)
                        {
                            if (show_tips)
                            {
                                YesOrNoWindow.instance.SetYesOrNoWindow(-1, "赌注身价太低!", $"当前赌桌每场对赌需要身价{room_worth}以上的赌注,您当前赌注只值{you_worth},不足以参加对赌,您可以更换赌注或者选择旁观!", false, true);
                            }
                            goto cantInfo;
                        }
                        break;
                    default:
                        YesOrNoWindow.instance.SetYesOrNoWindow(-1, "请先设置蛐蛐和赌注!", "进入对战位置（左右位置）需要先设置好出战蛐蛐和赌注，您可以点击蛐蛐和赌注图标进入设置界面，或者可以选择旁观!", false, true);
                        goto cantInfo;
                    cantInfo:
                        self.ready = 0;
                        return false;
                }
            }
            catch (Exception e)
            {
                // // Main.Logger.Log("ChechBet error " + e.Message);
                YesOrNoWindow.instance.SetYesOrNoWindow(-1, "请先设置蛐蛐和赌注!", "进入对战位置（左右位置）需要先设置好出战蛐蛐和赌注，您可以点击蛐蛐和赌注图标进入设置界面，或者可以选择旁观!", false, true);
                self.ready = 0;
                return false;
            }
            return true;
        }

        /// <summary>
        /// 确认出战蛐蛐是否可以进入该等级房间
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public static bool CheckQuqu(int level,bool show_tips = false)
        {
            for (int i = 0; i < client_ids.Length; i++)
            {
                int item_id = client_ids[i];
                // // Main.Logger.Log("item_id=" + item_id + " level=" + level);
                if (item_id > 0)
                {
                    if (level > 0)
                    {
                        int lv = int.Parse(DateFile.instance.GetItemDate(item_id, 8));
                        // // Main.Logger.Log("level=" + level + " lv=" + lv + " " + (lv > level));
                        if (lv > level)
                        {
                            if (show_tips)
                                YesOrNoWindow.instance.SetYesOrNoWindow(-1, "出战蛐蛐太强!", $"当前房间不允许出战蛐蛐超过{DateFile.instance.massageDate[8001][1].Split('|')[self.level - 1]},您可以更换出战蛐蛐或者选择旁观!", false, true);
                            return false;
                        }
                    }
                    else
                    {
                        int color = int.Parse(DateFile.instance.GetItemDate(item_id, 2002));
                        // // Main.Logger.Log("id=" + color);
                        if (color == 21 || color == 19 || color == 18)
                        {
                            if (show_tips)
                                YesOrNoWindow.instance.SetYesOrNoWindow(-1, "出战蛐蛐太强!", $"当前房间为三禁房，不能使用天蓝青、三段锦、八败,您可以更换出战蛐蛐或者选择旁观!", false, true);
                            return false;
                        }
                    }
                }
                else
                {
                    if (show_tips)
                        YesOrNoWindow.instance.SetYesOrNoWindow(-1, "请先设置蛐蛐和赌注!", "进入对战位置（左右位置）需要先设置好出战蛐蛐和赌注，您可以点击蛐蛐和赌注图标进入设置界面，或者可以选择旁观!", false, true);
                    return false;
                }
            }
            return true;
        }

        public void SetBet()
        {
            client_bet = bet_id;
            //0：赌注类型 1：物品原id  2：物品数据 3：物品变化
            string typ0 = bet_typ.ToString();
            string id1 = null;
            string data2;
            string data3;
            string[] ss;
            switch (bet_typ)
            {
                case 0: // 资源
                    id1 = bet_id.ToString();
                    ss = new string[] { typ0, id1 };
                    break;
                case 1: // 物品
                    // // // Main.Logger.Log("赌注id：" + bet_id);
                    if (DateFile.instance.itemsDate.ContainsKey(bet_id)) // itemsDate是记录一些常状变化的参数
                    {
                        Dictionary<int, string> item = DateFile.instance.itemsDate[bet_id];
                        string[] map = new string[item.Count * 2];
                        int pos = 0;
                        foreach (var da in item)
                        {
                            // // // Main.Logger.Log("itemsDate " + da.Key + ":" + da.Value);
                            map[pos++] = da.Key.ToString();
                            map[pos++] = da.Value.Replace('|', '*');
                        }
                        data2 = string.Join("#", map);
                        id1 = item[999];
                    }
                    else
                    {
                        data2 = "";
                    }
                    if (DateFile.instance.itemsChangeDate.ContainsKey(bet_id)) // itemsChangeDate记录装备的精制数据
                    {
                        Dictionary<int, int> item = DateFile.instance.itemsChangeDate[bet_id];
                        string[] map = new string[item.Count * 2];
                        int pos = 0;
                        foreach (var da in item)
                        {
                            // // // Main.Logger.Log("itemsChangeDate " + da.Key + ":" + da.Value);
                            map[pos++] = da.Key.ToString();
                            map[pos++] = da.Value.ToString();
                        }
                        data3 = string.Join("#", map);
                    }
                    else
                    {
                        data3 = "";
                    }
                    if (id1 == null)
                        id1 = bet_id.ToString();
                    ss = new string[] { typ0, id1, data2, data3 };
                    break;

                case 2: // 人物W
                    id1 = bet_id.ToString();
                    ss = new string[] { typ0, id1 };
                    break;
                default:
                    ss = new string[] { typ0 };
                    break;
            }
            bet = string.Join("｜", ss);
            // // // Main.Logger.Log("保存赌注 " + bet);
        }

        public void SetBetIdAndTyp(int p)
        {
            string[] ss = bet.Split('｜');
            if (ss.Length < 2)
            {
                bet_id = -98;
                bet_typ = -1;
            }
            else
            {
                //0：赌注类型 1：物品原id 2：物品数据
                bet_typ = int.Parse(ss[0]);
                int id = int.Parse(ss[1]);
                switch (bet_typ)
                {
                    case 0: // 资源
                        // // Debug.Log("资源");
                        bet_id = id;
                        break;
                    case 1: // 物品
                        // // Debug.Log("物品");
                        if (DateFile.instance.presetitemDate.ContainsKey(id))
                        {
                            if (DateFile.instance.presetitemDate[id][6] == "0")
                            {
                                int item_id = DateFile.instance.MakeNewItem(id, -(1111 * (p + 1)));
                                string[] data = ss[2].Split('#');
                                Dictionary<int, string> item = DateFile.instance.itemsDate[item_id];
                                for (int i = 0; i < data.Length; i += 2)
                                {
                                    // // // Main.Logger.Log("data" + i);
                                    if((i+1)< data.Length)
                                    {
                                        // // // Main.Logger.Log(data[i] + "  data:  " + data[i + 1]);
                                        int key = int.Parse(data[i]);
                                        string value = data[i + 1].Replace('*', '|');
                                        if (item.ContainsKey(key))
                                            item[key] = value;
                                        else
                                            item.Add(key, value);
                                    }
                                }
                                // // // Main.Logger.Log("ss length " + ss.Length);
                                if (ss.Length > 2)
                                {
                                    string[] data2 = ss[3].Split('#');
                                    if (data2.Length > 0)
                                    {
                                        Dictionary<int, int> pairs;
                                        if (DateFile.instance.itemsChangeDate.ContainsKey(item_id))
                                        {
                                            pairs = DateFile.instance.itemsChangeDate[item_id];
                                        }
                                        else
                                        {
                                            pairs = new Dictionary<int, int>();
                                            DateFile.instance.itemsChangeDate.Add(item_id, pairs);
                                        }
                                        for (int i = 0; i < data2.Length; i += 2)
                                        {
                                            // // // Main.Logger.Log("data2" + i);
                                            if ((i + 1) < data2.Length)
                                            {
                                                // // // Main.Logger.Log(data2[i] + "  data2:  " + data2[i + 1]);
                                                int key = int.Parse(data2[i]);
                                                int value = int.Parse(data2[i + 1]);
                                                if (pairs.ContainsKey(key))
                                                    pairs[key] = value;
                                                else
                                                    pairs.Add(key, value);
                                            }
                                        }
                                    }
                                }
                                bet_id = item_id;
                            }
                            else
                            {
                                bet_id = id;
                            }
                        }
                        else
                        {
                            goto default;
                        }
                        break;
                    case 2: // 人物
                        break;
                    default:
                        bet_id = id;
                        break;
                }
            }
        }
        public static void GetBattleAttr(string bet, int p, out int betId, out int betTyp,out GuiQuquBattleSystem.ActorTyp actorTyp, out int deskTyp,out int deskLevel)
        {
            string[] ss = bet.Split('｜');
            //0：赌注类型 1：物品原id 2：物品数据
            int.TryParse(ss[0], out betTyp);
            int.TryParse(ss[1], out int  id);
            int.TryParse(ss[ss.Length - 1], out deskLevel);
            int.TryParse(ss[ss.Length - 2], out deskTyp);
            if(int.TryParse(ss[ss.Length - 3], out int iactorTyp))
            {
                actorTyp = (GuiQuquBattleSystem.ActorTyp)iactorTyp;
            }
            else
            {
                actorTyp = GuiQuquBattleSystem.ActorTyp.OtherObserver;
            }
            switch (betTyp)
            {
                case 0: // 资源
                    betId = id;
                    break;
                case 1: // 物品
                    if (DateFile.instance.presetitemDate.ContainsKey(id))
                    {
                        if (DateFile.instance.presetitemDate[id][6] == "0")
                        {
                            int item_id = DateFile.instance.MakeNewItem(id, -(1111 * (p + 1)));
                            string[] data = ss[2].Split('#');
                            Dictionary<int, string> item = DateFile.instance.itemsDate[item_id];
                            for (int i = 0; i < data.Length; i += 2)
                            {
                                int key = int.Parse(data[i]);
                                string value = data[i + 1].Replace('*', '|');
                                if (item.ContainsKey(key))
                                    item[key] = value;
                                else
                                    item.Add(key, value);
                            }
                            betId = item_id;
                            if (ss.Length > 2)
                            {
                                string[] data2 = ss[3].Split('#');
                                if (data2.Length > 0)
                                {
                                    Dictionary<int, int> pairs;
                                    if (DateFile.instance.itemsChangeDate.ContainsKey(item_id))
                                    {
                                        pairs = DateFile.instance.itemsChangeDate[item_id];
                                    }
                                    else
                                    {
                                        pairs = new Dictionary<int, int>();
                                        DateFile.instance.itemsChangeDate.Add(item_id, pairs);
                                    }
                                    for (int i = 0; i < data2.Length; i += 2)
                                    {
                                        // // // Main.Logger.Log("data2" + i);
                                        if ((i + 1) < data2.Length)
                                        {
                                            // // // Main.Logger.Log(data2[i] + "  data2:  " + data2[i + 1]);
                                            int key = int.Parse(data2[i]);
                                            int value = int.Parse(data2[i + 1]);
                                            if (pairs.ContainsKey(key))
                                                pairs[key] = value;
                                            else
                                                pairs.Add(key, value);
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            betId = id;
                        }
                    }
                    else
                    {
                        goto default;
                    }
                    break;
                case 2: // 人物
                    betId = id;
                    break;
                default:
                    betId = id;
                    break;
            }
        }
        public static void SetBattleQuqu(int itemId, int idx)
        {
            client_ids[idx] = itemId;
            string str_ququ;
            // // // Main.Logger.Log("蛐蛐 物品id：" + itemId);
            if (DateFile.instance.itemsDate.ContainsKey(itemId)) // itemsDate是记录一些常状变化的参数
            {
                Dictionary<int, string> item = DateFile.instance.itemsDate[itemId];
                string[] map = new string[item.Count * 2];
                int pos = 0;
                foreach (var da in item)
                {
                    // // // Main.Logger.Log("itemsDate " + da.Key + ":" + da.Value);
                    map[pos++] = da.Key.ToString();
                    map[pos++] = da.Value.Replace('|', '*');
                }
                str_ququ = string.Join("#", map);
            }
            else
            {
                str_ququ = "";
            }
            self.ququ[idx] = str_ququ;
        }

        public int GetBattleQuquId(int idx, int p,int replay)
        {

            string s = ququ[idx];
            string[] data = s.Split('#');
            int ququId = 10000;
            if (s.Length > 1)
            {
                int item_id = DateFile.instance.MakeNewItem(10000, (p - idx) * replay);
                Dictionary<int, string> item = DateFile.instance.itemsDate[item_id];
                for (int i = 0; i < data.Length; i += 2)
                {
                    int key = int.Parse(data[i]);
                    string value = data[i + 1].Replace('*', '|');
                    if (item.ContainsKey(key))
                        item[key] = value;
                    else
                        item.Add(key, value);
                }
                ququId = item_id;
            }
            else
            {
                ququId = 0;
            }

            return ququId;
        }

        //public int GetQuquColor(int idx)
        //{
        //    string s = ququ[idx];
        //    if (s == "0")
        //    {
        //        return 0;
        //    }
        //    string[] ss = s.Split('#');
        //    return int.Parse(ss[0]);
        //}
        //public int GetQuquPartId(int idx)
        //{
        //    string s = ququ[idx];
        //    if (s == "0")
        //    {
        //        return 0;
        //    }
        //    string[] ss = s.Split('#');
        //    return int.Parse(ss[1]);
        //}
        //public string GetQuquInjurys(int idx)
        //{
        //    string s = ququ[idx];
        //    if (s == "0")
        //    {
        //        return "";
        //    }
        //    string[] ss = s.Split('#');
        //    return ss[2].Replace('*', '|');
        //}


        #region 人物形象
        public int age; // 年龄 茄子代号11
        public int gender; // 性别 茄子代号14
        public int actorGenderChange; // 男生女相 1是 其他不是 茄子代号17
        public int[] faceDate; // 使用的部位 茄子代号995|分割
        public int[] faceColor; // 部位的颜色 茄子代号996|分割
        public int clotheId; // 穿着衣服ID 茄子代号305

        // 获取人物形象标识
        public string GetImage()
        {
            string flag = "";
            string[] data = new string[faceDate.Length + faceColor.Length + 6];
            int pos = 0;
            data[pos++] = age.ToString();
            data[pos++] = gender.ToString();
            data[pos++] = actorGenderChange.ToString();
            data[pos++] = faceDate.Length.ToString();
            for (int i = 0; i < faceDate.Length; i++)
            {
                data[pos++] = faceDate[i].ToString();
            }
            data[pos++] = faceColor.Length.ToString();
            for (int i = 0; i < faceColor.Length; i++)
            {
                data[pos++] = faceColor[i].ToString();
            }
            data[pos++] = clotheId.ToString();
            flag = string.Join(",", data);
            return flag;
        }

        // 设置人物形象标识
        public void SetImage(string flag)
        {
            string[] data = flag.Split(',');
            int pos = 0;
            age = int.Parse(data[pos++]);
            gender = int.Parse(data[pos++]);
            actorGenderChange = int.Parse(data[pos++]);
            faceDate = new int[int.Parse(data[pos++])];
            for (int i = 0; i < faceDate.Length; i++)
            {
                faceDate[i] = int.Parse(data[pos++]);
            }
            faceColor = new int[int.Parse(data[pos++])];
            for (int i = 0; i < faceColor.Length; i++)
            {
                faceColor[i] = int.Parse(data[pos++]);
            }
            clotheId = int.Parse(data[pos++]);
        }
        #endregion
    }

    public class BattleData : SortTimeStamp // 对战数据
    {
        public static List<BattleData> battleDatas;
        public BattleData()
        {
            battleDatas.Add(this);
            // // // Main.Logger.Log("新增战斗 当前数量"+ battleDatas.Count);
        }
        public PlayerData[] player_data; //[2] 玩家数据
        public string battleFlag;
    }

    public class ChatData : SortTimeStamp // 聊天数据
    {
        private string m_name;
        // 玩家名字
        public string name
        {
            get { return m_name; }
            set { m_name = value.Split('｜')[0]; }
        }
        public string ip; // 玩家ip地址
        public string content; // 聊天内容
        public string param; // 参数
    }

    public abstract class SortTimeStamp : IComparable<SortTimeStamp>
    {
        public long time_stamp;
        public virtual int CompareTo(SortTimeStamp other)
        {
            return other.time_stamp < this.time_stamp ? 1 : 0;
        }
    }
    #endregion

}