<?

/**
 * Функция: tablerecordscount
 * Параметры:
 * table: Наименование таблицы
 * condition: (опционально) секция where запроса
 * db: идентификатор БД, из которой следует сделать выборку.
 * Описание: Возвращает количество записей в таблице table.
 * Если дано условие condition, к запросу добавляется "where (condition)"
 */

    // $sid, $session_time_expire, $_reply

    if (lib_session_ping($sid)) {
        if ($_GET['condition']) {
            lib_db_table_records_count($_GET['table'], $_GET['condition']);
        } else {
            lib_db_table_records_count($_GET['table'], false);
        }
    } else {
        $_reply = $RESULT_NEED_AUTH;
    }


?>
