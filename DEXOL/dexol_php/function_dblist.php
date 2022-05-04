<?php
/**
 * Функция: dblist
 * Параметры: нет
 * Описание: Возвращает список баз данных, доступных пользователю.
 * Вид списка:
 * []{
 * "db_name", 
 * "db_title",
 * "unit" { "uid", "foreign_id", "title", "documentstate", "data" },
 * "doctypes"[]
 * }
 */


    if (lib_session_ping($sid)) {
        $user = $config_users[$session_time_expire[$sid]['user']];
        $_reply = $RESULT_OK;

        if ($user) {
            $dblist = array();
            foreach($user['db'] as $k => $uid) {

                $cur_db = $config_db[$k];
                if ($dbl = mysql_connect($cur_db['dbhost'], $cur_db['dbuser'], $cur_db['dbpass'])) {
					if ($result = mysql_db_query($cur_db['dbname'], "select * from `units` where uid = " . $uid)) {
                        if ($row = mysql_fetch_assoc($result)) {
                            if ($row['status'] == 1) {
                                $dblist[] = array(
                                    "db_name" => $k,
                                    "db_title" => $cur_db['title'],
                                    "unit" => array(
                                        "uid" => $uid,
                                        "foreign_id" => $row['foreign_id'],
                                        "title" => $row['title'],
                                        "documentstate" => $row['documentstate'],
                                        "data" => $row['data']
                                    ),
                                    "doctypes" => $cur_db['doctypes']
                                    );
                            }
                        }
					}
                }
            }
			
            $_reply['dblist'] = $dblist;
        }
    } else {
        $_reply = $RESULT_NEED_AUTH;
    }
    
?>
