<?
//phpinfo();

require_once "config.php";
require_once "dexol_db.php";
require_once "const.php";
require_once "lib_session.php";
require_once "lib_db.php";
require_once "lib_toolbox.php";

$_reply = $RESULT_BAD_REQUEST;

$supress_output = false; // true = не выводит json

if (isset($_GET['sid']))
{
    $_reply = $RESULT_NEED_AUTH;
    $_reply['l'] = __FILE__ . " : " . __LINE__;
    $sid = $_GET['sid'];
    if ($session_time_expire[$sid]['exptime'] && $session_time_expire[$sid]['exptime'] > time()) {
        // Файл с сессией есть и она не истекла
        $_reply = $RESULT_BAD_REQUEST;
        $_reply['l'] = __FILE__ . " : " . __LINE__;

        // Подключим БД
        if (isset($_GET['db'])) {
            $db = $_GET['db'];
            if ($db && $config_db[$db]) {
                $_reply = $RESULT_ACCESS_DENIED;
                $_reply['l'] = __FILE__ . " : " . __LINE__;
                $user = $config_users[$session_time_expire[$sid]['user']];
                if ($user["db"][$db]) {
                    $_reply = $RESULT_INTERNAL_ERROR;
                    $_reply['l'] = __FILE__ . " : " . __LINE__;
                    $_cfg = $config_db[$db];
                    $dblink = mysql_connect($_cfg['dbhost'] . ':' . $_cfg['dbport'], $_cfg['dbuser'], $_cfg['dbpass']);
                    if ($dblink && mysql_select_db($_cfg['dbname'], $dblink)) {
                        mysql_set_charset('utf8', $dblink);
                        $_reply = $RESULT_OK;
                        $_reply['l'] = __FILE__ . " : " . __LINE__;
                    }
                }
            }
        }
        // Если БД не подключена - значит, плохой параметр в БД

        if ($_GET['f']) {
            // Факт затребования функции подтвержден
            $function_file = "function_" . $_GET['f'] . ".php";
            if (file_exists($function_file)) {
                // Указанная функция присутствует
                @require_once $function_file;
            } else {
                $_reply = $RESULT_NO_SUCH_FUNCTION;
                $_reply['l'] = __FILE__ . " : " . __LINE__;
            }
        } else {
            $_reply = $RESULT_BAD_REQUEST;
            $_reply['l'] = __FILE__ . " : " . __LINE__;
        }
    }

} else {
    // Проверим, есть ли login и md5(pass)
    if (isset($_GET['login']) && isset($_GET['pass']) && isset($config_users[$_GET['login']])) {

        $user = $config_users[$_GET['login']];
        if ($user && $user['password'] && (md5($user['password']) == $_GET['pass'] || $user['password'] == $_GET['pass'])) {
            // Данные пользователя совпадают. Создадим ему сессию.
            $sess_hash = lib_session_create($_GET['login'], $user['password']);

            // Вернём сессию пользователю
            $_reply = array("result" => 0, "sid" => $sess_hash, "user_name" => $user["name"], "profile_code" => $user['profile_code'], "user_props" => $user["user_props"]);
        } else {
            // В системе нет такого пользователя, либо пароль указан неверно
            $_reply = $RESULT_NO_SUCH_USERPASS;
            $_reply['l'] = __FILE__ . " : " . __LINE__;
        }
    } else {
        $_reply = $RESULT_NEED_AUTH;
        $_reply['l'] = __FILE__ . " : " . __LINE__;
    }
}



if (!$supress_output) { // Если вывод не запрещен

	$gzpack =  (isset($_GET['gzip']) && $_GET['gzip'] == "y");
    if (!$gzpack) {
        $outs = json_encode($_reply);
    } else {
        $outs = gzencode(json_encode($_reply));
    }

    header("Content-length: " . strlen($outs));
    echo $outs;
}

?>