<?php
    // params: signature, [fields], [reset]

    if (lib_session_ping($sid)) {
        if ($dblink) {
            if ($_GET['signature']) {

                $_reply = $RESULT_OK;

                if ($_GET['reset'] == "true" || $_GET['reset'] == "1" || $_GET['reset'] == 1) {
                    $sql = "delete from `criticals` where signature = '" .
                            mysql_escape_string($_GET['signature']) . "'";
                    mysql_query($sql, $dblink);
                }

                if ($_GET['fields']) {
                    $fields = json_decode($_GET['fields']);
                    if ($fields) {
                        foreach($fields as $k => $v) {
                            $sql = "insert into `criticals` (signature, cname, cvalue) values ('" .
                                    mysql_escape_string($_GET['signature']) . "', '" .
                                    mysql_escape_string($k) . "', '" .
                                    mysql_escape_string($v) . "')";

                            mysql_query($sql, $dblink);
                        }
                    }
                }
            } else {
                $_reply = $RESULT_BAD_REQUEST;
                $_reply['l'] = __FILE__ . " : " . __LINE__;
            }
        }
    } else {
        $_reply = $RESULT_NEED_AUTH;
        $_reply['l'] = __FILE__ . " : " . __LINE__;
    }
?>
