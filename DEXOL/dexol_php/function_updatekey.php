<?
if (file_exists("update/update.key") && file_exists("update/update.exe")) {
    $_reply = $RESULT_OK;
    $_reply['key'] = @file_get_contents("update/update.key");
} else {
    $_reply = $RESULT_INTERNAL_ERROR;
    $_reply['message'] = "Отсутствует файл обновления";
}
?>
