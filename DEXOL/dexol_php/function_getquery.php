<?php
/**
 * Функция: getquery
 * Параметры:
 * sql: Текст sql-запроса
 * db: идентификатор БД, из которой следует сделать выборку.
 * Описание: Возвращает массив массивов со значениями. Первый массив - наименования и типы столбцов.
 */

    // $sid, $session_time_expire, $_reply

    if (lib_session_ping($sid)) {
        lib_db_getquery($_GET['sql']);
    } else {
        $_reply = $RESULT_NEED_AUTH;
    }
    
?>
