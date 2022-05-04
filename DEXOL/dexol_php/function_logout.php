<?php
/**
 * Функция: logout
 * Параметры: нет
 * Описание: Уничтожает сессию.
 */

    // $sid, $session_time_expire, $_reply

    lib_session_logout($sid);
    $_reply = $RESULT_OK;
?>
