<?php
    if (lib_session_ping($sid)) {
        if ($dblink) {
            $_reply = $RESULT_BAD_REQUEST;
            $_reply['l'] = __FILE__ . " : " . __LINE__;
            if ($_GET['id']) {
                $_reply = $RESULT_INTERNAL_ERROR;
                $sql = "select * from `journal` where id = " . $_GET['id'];
                if ($result = mysql_query($sql, $dblink)) {
                    $_reply = $RESULT_NEGATIVE;
                    $_reply['message'] = "Нет записей для удаления";
                    if ($row = mysql_fetch_assoc($result)) {
                        if ($row['status'] < 2) {
                            $_user = $config_users[$session_time_expire[$sid]["user"]];
                            $_db = $config_db[$db];

                            if ($_user["db"][$db] == $row['unitid']) {
                                $sql = "delete from `journal` where id = " . $_GET['id'];
                                if (mysql_query($sql, $dblink)) {
                                    lib_toolbox_set_data_reference("units", $row["unitid"], 0);
                                    lib_toolbox_set_data_reference("users", $row["userid"], 0);

                                    lib_toolbox_set_document_criticals($row["signature"], false, true);
                                    $_reply = $RESULT_OK;
                                } else {
                                    $_reply['message'] = "Не удалось удалить документ";
                                }
                            } else {
                                $_reply['message'] = "Невозможно удалить документ, так как он принадлежит другому отделению.";
                            }
                        } else {
                            $_reply['message'] = "Для удаления этого документа необходимо обратиться в офис";
                        }
                    }
                }
            }
        }
    } else {
        $_reply = $RESULT_NEED_AUTH;
        $_reply['l'] = __FILE__ . " : " . __LINE__;
    }

?>
