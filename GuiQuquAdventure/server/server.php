<?php
header("Content-Type:text/html; charset=utf-8");
ini_set('default_socket_timeout', -1);  //不超时
$msg = isset($_POST['msg'])? htmlspecialchars($_POST['msg']) : 0;
$name = isset($_POST['name'])? htmlspecialchars($_POST['name']) : 0;
$name = strtr($name, '|', '｜');

function msectime() {
	list($msec, $sec) = explode(' ', microtime());
	$msectime = (float)sprintf('%.0f', (floatval($msec) + floatval($sec)) * 1000);
	return $msectime;
}

function get_ip(){
	//strcasecmp 比较两个字符，不区分大小写。返回0，>0，<0。
    if(getenv('HTTP_CLIENT_IP') && strcasecmp(getenv('HTTP_CLIENT_IP'), 'unknown')) {
        $ip = getenv('HTTP_CLIENT_IP');
    } elseif(getenv('HTTP_X_FORWARDED_FOR') && strcasecmp(getenv('HTTP_X_FORWARDED_FOR'), 'unknown')) {
        $ip = getenv('HTTP_X_FORWARDED_FOR');
    } elseif(getenv('REMOTE_ADDR') && strcasecmp(getenv('REMOTE_ADDR'), 'unknown')) {
        $ip = getenv('REMOTE_ADDR');
    } elseif(isset($_SERVER['REMOTE_ADDR']) && $_SERVER['REMOTE_ADDR'] && strcasecmp($_SERVER['REMOTE_ADDR'], 'unknown')) {
        $ip = $_SERVER['REMOTE_ADDR'];
    }
    $res =  preg_match ( '/[\d\.]{7,15}/', $ip, $matches ) ? $matches [0] : '';
    return $res;
    //dump(phpinfo());//所有PHP配置信息
}
global $new_err;
global $user_time_stamp;
global $off_line_time;
global $user_ip;
global $redis;
global $key_player;
global $key_room;
global $key_desk;
global $key_chat;
$new_err = -1;
/*
$new_err_list = array(
'0' => '位置已被占用',
)
 */


$user_time_stamp = msectime(); // 时间 毫秒
$off_line_time = 30000; // 脱机时间
$battle_max_time = 1800000; // 战斗最多保存时间
$user_ip = get_ip().".".$name;
$redis = new Redis();
$redis->connect('127.0.0.1', '6379');
$redis->auth('密码'); //密码验证
$key_player = "cricket_battle_player";
$key_room = "cricket_battle_room";
$key_desk = "cricket_battle_desk";
$key_chat = "cricket_battle_chat";

if($redis->exists("last_time_stamp")){
	$last_time_stamp = $redis->get("last_time_stamp");
	if($user_time_stamp <= $last_time_stamp){
		$user_time_stamp = $last_time_stamp + 1;
	}
}
$redis->set("last_time_stamp", $user_time_stamp);



function GetChatData($name,$ip,$content,$param,$level,$desk_idx){
	$chat_data = array();
	$chat_data['time_stamp'] = $GLOBALS['user_time_stamp'];
	$chat_data['name'] = $name;
	$chat_data['ip'] = $ip;
	$chat_data['content'] = $content;
	$chat_data['param'] = $param;
	$chat_data['level'] = $level;
	$chat_data['desk_idx'] = $desk_idx;
	return $chat_data;
}

function GetBattleData($left_player, $right_player, $flag_list){
	$battle_data = array();
	$battle_data['time_stamp'] = $GLOBALS['user_time_stamp'];
	$battle_data['player_data'] = array($left_player,$right_player);
	$battle_data['observer'] = $flag_list;
	return $battle_data;
}

function GetPlayerData($level, $desk_idx){
	$player_data = array();
	$player_data['name'] = 0;
	$player_data['ip'] = 0;
	$player_data['observer'] = -1;
	$player_data['ququ'] = array(0,0,0);
	$player_data['time_stamp'] = 0;
	$player_data['ready'] = 0; // 准备 0是未准备 1是确认赌注 2是准备好了
	$player_data['level'] = $level;
	$player_data['desk_idx'] = $desk_idx;
	$player_data['bet'] = 0;
	$player_data['image'] = "0,0,0,0,0,0";
	return $player_data;
}

function GetDeskData($idx){
	$desk_data = array();
	$desk_data['idx'] = $idx;
	$desk_data['typ'] = 0;
	$desk_data['player_data'] = array();
	for ($j=0; $j < 12; $j++) { 
		$desk_data['player_data'][$j] = 0; // 这里存玩家ip
	}
	$desk_data['battle_data'] = 0;
	return $desk_data;
}

function GetHallData($typ){
	$need_rest = $typ == 123456;
	$redis = $GLOBALS['redis'];
	$key_player = $GLOBALS['key_player'];
	$key_room = $GLOBALS['key_room'];
	$key_desk = $GLOBALS['key_desk'];
	$key_chat = $GLOBALS['key_chat'];
	if ($need_rest){
		$player_data = array(); // 所有玩家数据 [ip] = 玩家
		$room_data = array(); // 所有房间数据 [level] = 房间
		$desk_data = array(); // 所有桌子数据 [desk_idx] = 房间
		$chat_data = array(); // 所有聊天数据 [time_stamp] = 聊天

		// 初始化房间和桌子数据
		for ($i=0; $i <10 ; $i++) { 
			$room_data[$i] = array('level'=> $i);
			for ($j=0; $j < 100; $j++) { 
				$idx = $i * 100 + $j ;
				$desk_data[$idx] = GetDeskData($idx);
			}
		}
		SaveData($key_player, $player_data);
		SaveData($key_room, $room_data);
		SaveData($key_desk, $desk_data);
		SaveData($key_chat, $chat_data);
		$hall_data = array('player_data' => $player_data,'room_data' => $room_data,'desk_data' => $desk_data,'chat_data' => $chat_data); // 大厅数据
	}else{
		$player_data = GetData($key_player);
		$room_data = GetData($key_room);
		$desk_data = GetData($key_desk);
		$chat_data = GetData($key_chat);
		$hall_data = array('player_data' => $player_data,'room_data' => $room_data,'desk_data' => $desk_data,'chat_data' => $chat_data); // 大厅数据
	}
	return $hall_data;
}

function SaveData($k, $data){
	$redis = $GLOBALS['redis'];
	if($redis->exists($k)){
		$redis->del($k);	
	}
	$redis->set($k, json_encode($data));
}

function GetData($k){
	$redis = $GLOBALS['redis'];
	$data_str = $redis->get($k);
	return json_decode($data_str, true);
}

function ClearOffLinePlayer($player_data){ // 清除脱机玩家
	$time_stamp = $GLOBALS['user_time_stamp'];
	$off_line_time = $GLOBALS['off_line_time'];
	foreach($player_data as $ip=>$data)
	{
		if(($data['time_stamp'] + $off_line_time) < $time_stamp){ // 脱机玩家
			unset($player_data[$ip]);
		}
	}
	// $player_datas = $hall_data['player_data'];
	// for ($i=count($player_datas) - 1; $i >=0 ; $i--) { 
	// 	$player_data = $player_datas[$i];
	// 	if(($player_data['time_stamp'] + $off_line_time) < $time_stamp){ // 脱机玩家
	// 		array_splice($hall_data['player_data'], $i, 1); // array_splice(数组，要删除第几个元素，删除几次)
	// 	}
	// }
	return $player_data;
}

function SetOnLinePlayer($hall_data,$name,$ip,$level,$desk_idx,$observer,$save_desk,$image){ // 设置在线玩家
	if(isset($hall_data['player_data'][$ip])){
		$player_data = $hall_data['player_data'][$ip];
	}else{
		$player_data = GetPlayerData(-1, -1);
		$player_data['ip'] = $ip;
	}

	if($desk_idx!=-1){
		// 关于玩家是否可以坐到桌子上
		$desk_data = $hall_data['desk_data'][$desk_idx];
		$star_idx = $observer == 0 ? 0:2;
		$end_idx = $observer == 0 ? 2:12;
		$add_pos = -1;

		// 将已离开座位的玩家ip置为0
		for ($i=0; $i < count($desk_data['player_data']); $i++) { 
			$desk_player_ip = $desk_data['player_data'][$i];
			if($desk_player_ip != 0 && isset($hall_data['player_data'][$desk_player_ip])){
				$player = $hall_data['player_data'][$desk_player_ip];	
			}else{
				$player = false;
			}

			if($desk_player_ip == 0 || !$player || $player['desk_idx'] != $desk_idx || $desk_player_ip == $ip){
				// 玩家已离开桌子或者换位置
				$desk_data['player_data'][$i] = 0;
			}

			if($add_pos == -1 && $desk_data['player_data'][$i] == 0){
				if($i >= $star_idx && $i <= $end_idx){
					$add_pos = $i;	
				}
			}

		}
		if($add_pos == -1){
			$level = intval($desk_idx/100);
			$desk_idx = -1;
			$GLOBALS['new_err'] = 0;
		}else{
			$desk_data['player_data'][$add_pos] = $ip;
			$hall_data['desk_data'][$desk_idx] = $desk_data; // 更新桌子数据	
			if($save_desk){
				SaveData($GLOBALS['key_desk'], $hall_data['desk_data']);
			}
		}
	}


	// 玩家的信息
	$player_data['name'] = $name;
	$player_data['level'] = $level;
	$player_data['desk_idx'] = $desk_idx;
	$player_data['observer'] = $observer;
	$player_data['image'] = $image; // 人物形象
	$player_data['time_stamp'] = $GLOBALS['user_time_stamp']; // 更新在线
	$hall_data['player_data'][$ip] = $player_data; // 更新玩家信息



	return $hall_data;
}












// 获取各个房间人数
function GetRoomPeopleNum($player_data){
	$array = array();
	foreach ($player_data as $key => $value) {
		$array[$value['level']] = (isset($array[$value['level']])?$array[$value['level']]:0) + 1;
	}
	return $array;
}

// 获取房间的人
function GetRoomProple($player_data, $room_idx){
	$array = array();
	foreach ($player_data as $key => $value) {
		if($value['level'] == $room_idx){
			array_push($array,$value);//添加元素
		}
	}
	return $array;
}

 // 每个桌子人数标识 1和2表示是否有对战者 4 8 16 32 64 128 256 516 1024 2048 分别表示是否存在的最多10个观战  4096 表示房间类型
function GetDeskMark($hall_data, $desk_idx){
	$result = 0;
	$desk_data = $hall_data['desk_data'][$desk_idx];
	$player_data = $hall_data['player_data'];
	$count = count($desk_data['player_data']);
	for ($i=0; $i < $count; $i++) { 
		if(isset($player_data[$desk_data['player_data'][$i]])){
			$player = $player_data[$desk_data['player_data'][$i]];
			if($player && $player['desk_idx'] == $desk_idx){
				$result |= 1 << $i;
			}	
		}
	}
	if($desk_data['typ'] == 1){
		$result |= 1 << $count;
	}
	return $result;
}

// 获取房间聊天记录
function GetRoomChatRecord($chat_data, $room_idx, $lcts){
	$array = array();
	foreach ($chat_data as $key => $value) {
		if($value['level'] == $room_idx && $value['time_stamp'] > $lcts){
			array_push($array,$value);//添加元素
		}
	}
	return $array;
}

// 获取一个桌子的聊天记录
function GetDeskChatRecord($chat_data, $desk_idx, $lcts){
	$array = array();
	foreach ($chat_data as $key => $value) {
		if($value['desk_idx'] == $desk_idx && $value['time_stamp'] > $lcts){
			array_push($array,$value);//添加元素
		}
	}
	return $array;
}

// 添加一个聊天记录
function AddChatRecord($hall_data, $value){
	$chat_data = $hall_data['chat_data'];
	array_push($chat_data,$value);//添加元素
	$max_count = 300; // 保存的最大聊天数量 超过时删除一半
	if(count($chat_data) > $max_count){
		$new_chat_data = array();
		for ($i=intval($max_count/2); $i < count($chat_data) ; $i++) { 
			array_push($new_chat_data,$chat_data[$i]);//添加元素
		}
		$hall_data['chat_data'] = $new_chat_data;
	}else{
		$hall_data['chat_data'] = $chat_data;
	}
	return $hall_data;
}


















if($msg < 10001 || $msg > 10003){

	$typ = isset($_POST['typ'])? htmlspecialchars($_POST['typ']) : 0;
	$hall_data = GetHallData($typ);
	$player_data = ClearOffLinePlayer($hall_data['player_data']); // 清除脱机玩家
	SaveData("cricket_battle_player", $player_data);
	echo "<br><pre>";print_r($hall_data);
	echo "<pre> <br>";


}elseif(10001 == $msg){ // 进入大厅 获取房间信息
	$image = isset($_POST['image'])? htmlspecialchars($_POST['image']) : "0,0,0,0,0,0";


	$hall_data = GetHallData(0);
	$hall_data['player_data'] = ClearOffLinePlayer($hall_data['player_data']);
	$hall_data = SetOnLinePlayer($hall_data,$name,$user_ip,-1,-1,-1,false,$image); // 更新在线
	SaveData($GLOBALS['key_player'], $hall_data['player_data']);

	$pos = 0;
	$data = array();
	$data[$pos++] = $user_time_stamp; // 时间戳
	$data[$pos++] = $user_ip; // ip
	$data[$pos++] = $new_err; // 错误码
	if( $new_err == -1 ){
		$people_num_list = GetRoomPeopleNum($hall_data['player_data']);
		$room_data = $hall_data['room_data'];
		$count = count($room_data);
		$data[$pos++] = $count; // 房间数量
		for ($i=0; $i < $count; $i++) { 
			$data[$pos++] = $room_data[$i]['level']; // 房间等级
			$data[$pos++] = isset($people_num_list[$i])?$people_num_list[$i]:0; // 房间人数
		}
	}
	echo implode('|', $data);

	// echo "<br><pre>";print_r($hall_data);echo "<pre> <br>";
}elseif(10002 == $msg){
	$room_idx = isset($_POST['room_idx'])? htmlspecialchars($_POST['room_idx']) : 0;
	$lcts = isset($_POST['lcts'])? htmlspecialchars($_POST['lcts']) : 0;
	$image = isset($_POST['image'])? htmlspecialchars($_POST['image']) : "0,0,0,0,0,0";

	$hall_data = GetHallData(0);
	$hall_data['player_data'] = ClearOffLinePlayer($hall_data['player_data']);
	$hall_data = SetOnLinePlayer($hall_data,$name,$user_ip,$room_idx,-1,-1,false,$image); // 更新在线
	SaveData($GLOBALS['key_player'], $hall_data['player_data']);

	// 聊天
	if(isset($_POST['chat_content'])){
		$chat_content = htmlspecialchars($_POST['chat_content']);
		$chat_content = strtr($chat_content, '|', '｜');
		$content_param = isset($_POST['content_param'])? htmlspecialchars($_POST['content_param']) : "";
		$chat_record = GetChatData($name,$user_ip,$chat_content,$content_param,$room_idx,-1);
		$hall_data = AddChatRecord($hall_data, $chat_record);
		SaveData($GLOBALS['key_chat'], $hall_data['chat_data']);
	}



	$pos = 0;
	$data = array();
	$data[$pos++] = $user_time_stamp; // 时间戳
	$data[$pos++] = $user_ip; // ip
	$data[$pos++] = $new_err; // 错误码
	if( $new_err == -1 ){
		$data[$pos++] = $room_idx; // 房间索引
		$data[$pos++] = 10; // 房间个数
		$data[$pos++] = $hall_data['room_data'][$room_idx]['level']; // 房间等级
		$people_list = GetRoomProple($hall_data['player_data'], $room_idx);
		$data[$pos++] = count($people_list); // 房间人数
		$data[$pos++] = 100; // 房间的桌子数量
		for ($desk_idx = $room_idx * 100; $desk_idx < $room_idx * 100 + 100; $desk_idx++) { 
			$data[$pos++] = GetDeskMark($hall_data, $desk_idx); // 每个桌子人数标识 1和2表示是否有对战者 4 8 16 32 64 128 256 516 1024 2048 观战  4096 表示房间类型
		}
		foreach ($people_list as $key => $value) {
			$data[$pos++] = $value['name']; // 玩家名字
			$data[$pos++] = $value['ip']; // ip地址
			$data[$pos++] = $value['desk_idx']; // 所在桌子
			$data[$pos++] = $value['image']; // 玩家形象
		}
		$chat_record_list = GetRoomChatRecord($hall_data['chat_data'], $room_idx, $lcts);
		$data[$pos++] = count($chat_record_list); // 聊天记录数量
		foreach ($chat_record_list as $key => $value) {
			$data[$pos++] = $value['time_stamp']; // 发言时间
			$data[$pos++] = $value['name']; // 发言者名字
			$data[$pos++] = $value['ip']; // 发言者ip
			$data[$pos++] = $value['content']; // 发言内容
			$data[$pos++] = $value['param']; // 发言参数
		}
	}
	echo implode('|', $data);

	// echo "<br><pre>";print_r($hall_data);echo "<pre> <br>";
}elseif(10003 == $msg){
	$desk_idx = isset($_POST['desk_idx'])? htmlspecialchars($_POST['desk_idx']) : 0;
	$ready = isset($_POST['ready'])? htmlspecialchars($_POST['ready']) : 0;
	$lcts = isset($_POST['lcts'])? htmlspecialchars($_POST['lcts']) : 0;
	$lbts = isset($_POST['lbts'])? htmlspecialchars($_POST['lbts']) : 0;
	$bet = isset($_POST['bet'])? htmlspecialchars($_POST['bet']) : 0;
	$ququ = isset($_POST['ququ'])? htmlspecialchars($_POST['ququ']) : "0,0,0";
	$observer = isset($_POST['observer'])? htmlspecialchars($_POST['observer']) : 0;
	$image = isset($_POST['image'])? htmlspecialchars($_POST['image']) : "0,0,0,0,0,0";

	$hall_data = GetHallData(0);
	$hall_data['player_data'] = ClearOffLinePlayer($hall_data['player_data']);
	$hall_data = SetOnLinePlayer($hall_data,$name,$user_ip,-1,$desk_idx,$observer,true,$image); // 更新在线
	if( $new_err == -1 ){
		// 聊天
		if(isset($_POST['chat_content'])){
			$chat_content = htmlspecialchars($_POST['chat_content']);
			$chat_content = strtr($chat_content, '|', '｜');
			$content_param = isset($_POST['content_param'])? htmlspecialchars($_POST['content_param']) : "";
			$chat_record = GetChatData($name,$user_ip,$chat_content,$content_param,-1,$desk_idx);
			$hall_data = AddChatRecord($hall_data, $chat_record);
			$hall_data['player_data'][$user_ip]['ready'] = $ready;
			$hall_data['player_data'][$user_ip]['bet'] = $bet;
			$hall_data['player_data'][$user_ip]['observer'] = $observer;
			$hall_data['player_data'][$user_ip]['ququ'] = explode(',',$ququ);
			SaveData($GLOBALS['key_chat'], $hall_data['chat_data']);
		}


		$desk_data = $hall_data['desk_data'][$desk_idx]; // 桌子数据
		if(isset($hall_data['player_data'][$desk_data['player_data'][0]]) && isset($hall_data['player_data'][$desk_data['player_data'][1]])){
			$left_player = $hall_data['player_data'][$desk_data['player_data'][0]];
			$right_player = $hall_data['player_data'][$desk_data['player_data'][1]];
			// 开始战斗
			if($left_player['ready'] == 2 && $right_player['ready'] == 2){ // 都准备好了
				$people_flag = array();
				foreach ($desk_data['player_data'] as $key => $flag_ip) { // 记录这场战斗的参与者
					if($flag_ip != 0){
						$player = $hall_data['player_data'][$flag_ip]; // 拿到这个玩家的数据

						$item_id = $player['bet']; // 赌注物品id

						$bet_flag = 9; // 押了0号还是1号   9是没有押注的普通观众

						if($left_player['ip'] == $flag_ip){ // 0号对战者
							$bet_flag = 0;
							if($desk_data['typ'] == 0){
								$item_id = $right_player['bet']; // 获得对方的押注
							}
						}elseif($right_player['ip'] == $flag_ip){ // 1号对战者
							$bet_flag = 1;
							if($desk_data['typ'] == 0){
								$item_id = $left_player['bet']; // 获得对方的押注
							}
						}elseif($player['observer'] == 2){ // 押了0号的观战者
							$bet_flag = 2;
						}elseif($player['observer'] == 3){ // 押了1号的观战者
							$bet_flag = 3;
						}
						$people_flag[$flag_ip] = $bet * 10 + $bet_flag;
						$hall_data['player_data'][$flag_ip]['ready'] = 0;
					}
				}
				$battle_data = GetBattleData($left_player, $right_player, $people_flag);
				$desk_data['battle_data'] = $battle_data; // 覆盖记录为最近一场战斗
				$hall_data['desk_data'][$desk_idx] = $desk_data;
				$hall_data['player_data'][$desk_data['player_data'][0]]['ready'] = 0;
				$hall_data['player_data'][$desk_data['player_data'][1]]['ready'] = 0;
			}
		}

		SaveData($GLOBALS['key_desk'], $hall_data['desk_data']); // 保存桌子战斗数据
		SaveData($GLOBALS['key_player'], $hall_data['player_data']); // 保存玩家准备数据
	}



	$pos = 0;
	$data = array();
	$data[$pos++] = $user_time_stamp; // 时间戳
	$data[$pos++] = $user_ip; // ip
	$data[$pos++] = $new_err; // 错误码
	if( $new_err == -1 ){
		$room_idx = intval($desk_idx/100);
		$data[$pos++] = $room_idx; // 房间索引
		$data[$pos++] = 10; // 房间个数
		$data[$pos++] = $desk_idx; // 桌子索引
		$data[$pos++] = 100; // 桌子数量
		$desk_data = $hall_data['desk_data'][$desk_idx]; // 桌子数据



		$data[$pos++] = $desk_data['typ']; // 桌子类型
		$player_count = count($desk_data['player_data']);
		$data[$pos++] = $player_count; // 玩家数量
		for ($i=0; $i < $player_count; $i++) { 
			$ip = $desk_data['player_data'][$i];
			if(isset($hall_data['player_data'][$ip])){
				$player_data = $hall_data['player_data'][$ip];
			}else{
				$player_data = GetPlayerData($room_idx, $desk_idx);
			}
            $data[$pos++] = $player_data['name']; // 玩家名字
            $data[$pos++] = $player_data['ip']; // ip地址
            $data[$pos++] = $player_data['observer']; // 0非游客 1普通游客 2押0号注游客 3押1号注游客
            $ququ = $player_data['ququ'];
            for ($j=0; $j < 3; $j++) { 
            	if(isset($ququ[$j])){
            		$data[$pos++] = $ququ[$j]; //[3] 出战蛐蛐
            	}else{
            		$data[$pos++] = -1000;; //[3] 没有蛐蛐蛐蛐	
            	}
            }
            $data[$pos++] = $player_data['time_stamp']; // 心跳时间
            $data[$pos++] = $player_data['ready']; // 准备 0是未准备 1是确认赌注 2是准备好了
            $data[$pos++] = $player_data['bet']; // 赌注
            $data[$pos++] = $player_data['image']; // 人物形象
		}
		$battle_data = $desk_data['battle_data'];
		$battle_count = 0;
		if($battle_data!=0 && $battle_data['time_stamp'] > $lbts){
			$battle_count = 1;
		}

		$data[$pos++] = $battle_count; // 战斗数量
		if($battle_count > 0){
			$data[$pos++] = $battle_data['time_stamp']; // 战斗时间
			for ($i=0; $i < 2; $i++) { 
				$player_data = $battle_data['player_data'][$i];
				$data[$pos++] = $player_data['name']; // 玩家名字
				$data[$pos++] = $player_data['ip']; // ip地址
				$data[$pos++] = 0; // 0非游客 1普通游客 2押注游客
				for ($j=0; $j < 3; $j++) { 
					$data[$pos++] = $player_data['ququ'][$j]; //[3] 出战蛐蛐
				}
				$data[$pos++] = $player_data['time_stamp']; // 心跳时间
				$data[$pos++] = $player_data['ready']; // 准备 0是未准备 1是确认赌注 2是准备好了
				$data[$pos++] = $player_data['bet']; // 赌注
				$data[$pos++] = $player_data['image']; // 形象
			}
		}


		$chat_record_list = GetDeskChatRecord($hall_data['chat_data'], $desk_idx, $lcts);
		$data[$pos++] = count($chat_record_list); // 聊天记录数量
		foreach ($chat_record_list as $key => $value) {
			$data[$pos++] = $value['time_stamp']; // 发言时间
			$data[$pos++] = $value['name']; // 发言者名字
			$data[$pos++] = $value['ip']; // 发言者ip
			$data[$pos++] = $value['content']; // 发言内容
			$data[$pos++] = $value['param']; // 发言参数
		}
		//判断是否需要播放战斗
		$observer_list = $hall_data['desk_data'][$desk_idx]['battle_data']['observer'];
		if(isset($observer_list[$user_ip])){ // 需要播放战斗
			$data[$pos++] = $observer_list[$user_ip]; // 播放战斗 赌注*10 + 参与者类型
			unset($hall_data['desk_data'][$desk_idx]['battle_data']['observer'][$user_ip]);
			SaveData($GLOBALS['key_desk'], $hall_data['desk_data']); // 保存桌子战斗数据
			if ($hall_data['player_data'][$user_ip]['ready'] != 0){
				$hall_data['player_data'][$user_ip]['ready'] =0;
				SaveData($GLOBALS['key_player'], $hall_data['player_data']); // 保存玩家准备数据	
			}
		}else{
			$data[$pos++] = 0; // 不播放战斗
		}
	}
	echo implode('|', $data);

	// echo "<br><pre>";print_r($hall_data);echo "<pre> <br>";



}

?>