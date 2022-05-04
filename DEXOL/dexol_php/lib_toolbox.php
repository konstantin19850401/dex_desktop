<?php

function lib_toolbox_set_data_reference($tablename, $key, $_doincrement) {
    global $dblink, $_reply, $RESULT_OK;

    if ($dblink) {
        $_reply = $RESULT_OK;

        $doincrement = ($_doincrement == "true" || $_doincrement == "1" || $_doincrement == 1);

        $sql = "select id, refcount from `datarefs` where reftable = '" .
            mysql_escape_string($tablename) . "' and refkey = '" .
            mysql_escape_string($key) . "'";

        $result = mysql_query($sql, $dblink);

        $refupdated = false;

        if ($result) {
            $row = mysql_fetch_assoc($result);
            if ($row) {
                $refcount = $row["refcount"];
                if ($doincrement) $refcount++;
                else $refcount--;

                if ($refcount < 1) {
                    $sql = "delete from `datarefs` where id = " . $row["id"];
                } else {
                    $sql = "update `datarefs` set refcount = " . $refcount . " where id = " . $row["id"];
                }

                mysql_query($sql, $dblink);
                $refupdated = true;
            } else {
                if ($doincrement) $refcount = 1;
            }
        }

        if (!$refupdated && $refcount > 0) {
            $sql = "insert into `datarefs` (reftable, refkey, refcount) values ('" .
                mysql_escape_string($tablename) . "', '" . mysql_escape_string($key) .
                "', 1)";
            mysql_query($sql, $dblink);
        }
    }
}

function lib_toolbox_get_data_reference($tablename, $key) {
    global $dblink, $_reply, $RESULT_OK;

    if ($dblink) {
        $_reply = $RESULT_OK;
        $_reply["refcount"] = 0;

        $sql = "select id, refcount from `datarefs` where reftable = '" .
            mysql_escape_string($tablename) . "' and refkey = '" .
            mysql_escape_string($key) . "'";

        $result = mysql_query($sql, $dblink);
        if ($result) {
            $row = mysql_fetch_assoc($result);
            if ($row) $_reply["refcount"] = $row["refcount"];
        }
    }
}

function lib_toolbox_clear_data_reference($tablename, $key) {
    global $dblink, $_reply, $RESULT_OK;

    if ($dblink) {
        $_reply = $RESULT_OK;
        $sql = "delete from `datarefs` where reftable = '" .
            mysql_escape_string($tablename) . "' and refkey = '" .
            mysql_escape_string($key) . "'";

        mysql_query($sql, $dblink);
    }
}

function lib_toolbox_set_document_criticals($signature, $fields, $reset) {
    global $dblink;

    if (true == $reset) {
        $sql = "delete from `criticals` where signature = '" .
                mysql_escape_string($signature) . "'";
        mysql_query($sql, $dblink);

    }

    if ($fields) {
        foreach($fields as $k => $v) {
            $sql = "insert into `criticals` (signature, cname, cvalue) values ('" .
                    mysql_escape_string($signature) . "', '" .
                    mysql_escape_string($k) . "', '" .
                    mysql_escape_string($v) . "')";

            mysql_query($sql, $dblink);
        }
    }
}

?>
