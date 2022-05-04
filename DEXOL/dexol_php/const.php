<?
/**
 * Константы
 */

$RESULT_OK = array("result" => 0);
$RESULT_NEED_AUTH = array("result" => 1, "message" => "Необходима авторизация");
$RESULT_BAD_REQUEST = array("result" => 2, "message" => "Некорректный запрос");
$RESULT_NO_SUCH_USERPASS = array("result" => 3, "message" => "В системе нет такого пользователя, либо пароль указан неверно");
$RESULT_NO_SUCH_FUNCTION = array("result" => 4, "message" => "Затребованная функция отсутствует");
$RESULT_ACCESS_DENIED = array("result" => 5, "message" => "Нет доступа к затребованному элементу");
$RESULT_INTERNAL_ERROR = array("result" => 6, "message" => "Внутренняя ошибка сервера");
$RESULT_NEGATIVE = array("result" => 7, "message" => "Функция вернула ошибку");
?>