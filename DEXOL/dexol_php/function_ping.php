<?
/**
 * Функция: ping
 * Параметры: нет
 * Описание: Продлевает сессию на $config_default_session_timeout_sec от 
 * текущего момента.
 */

    // $sid, $session_time_expire, $_reply

    if (lib_session_ping($sid)) {
        $_reply = $RESULT_OK;
        if ($dblink) $_reply["db"] = "1";
    } else {
        $_reply = $RESULT_NEED_AUTH;
    }

?>
