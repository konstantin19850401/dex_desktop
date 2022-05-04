<?php
    if (lib_session_ping($sid)) {
        if ($_GET['date']) {
            $jdocdate = $_GET['date'];
            $unit_id = $config_users[$session_time_expire[$sid]["user"]]["db"][$db];
            
            $sql = "select id, `journal`.`status`, signature, jdocdate, " .
                   "substring(jdocdate, 1, 8) as vdocdate, docid, digest, data, journal " .
                    "from `journal` where unitid = " . $unit_id . " and jdocdate >= '" .
                    $jdocdate . "000000000' and jdocdate <= '" . $jdocdate . "235959999'";

            lib_db_getquery($sql);
            $_reply['dbg']['sql'] = $sql;
        }
    } else {
        $_reply = $RESULT_NEED_AUTH;
    }

?>
