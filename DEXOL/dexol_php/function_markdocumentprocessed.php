<?php
/*
 * Params: status, signature [, journal]
 */

    if (lib_session_ping($sid)) {
        if ($dblink) {
            $_reply = $RESULT_BAD_REQUEST;

            if ($_GET["status"] && $_GET["signature"]) {
                $sql = "update `journal` set status = " . $_GET["status"];
                if ($_GET["journal"]) {
                    $sql .= ", journal = '" . mysql_escape_string($_GET["journal"]) . "'";
                }
                $sql .= " where signature = '" . mysql_escape_string($_GET["signature"]) . "'";

                $_reply = $RESULT_NEGATIVE;
                if (mysql_query($sql, $dblink)) {
                    $_reply = $RESULT_OK;
                }
            }
        }
    } else {
        $_reply = $RESULT_NEED_AUTH;
    }

?>
