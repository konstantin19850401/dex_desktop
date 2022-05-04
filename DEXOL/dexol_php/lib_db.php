<?php

function lib_db_getquery($sql) {
    global $dblink, $_reply, $RESULT_BAD_REQUEST, $RESULT_OK;
    $_reply = $RESULT_BAD_REQUEST;
    $result = mysql_query($sql, $dblink);
    if ($result) {
        $_reply = $RESULT_OK;
        $fields = array();
        for($i = 0; $i < mysql_num_fields($result); $i++) {
            $field = mysql_fetch_field($result, $i);
            $fields[] = array("name" => $field->name, "type" => $field->type, "max_length" => $field->max_length);
        }

        $data = array();
        while($row = mysql_fetch_row($result)) {
            $data[] = $row;
        }

        $_reply["fields"] = $fields;
        $_reply["data"] = $data;
		$_reply["mysql_insert_id"] = mysql_insert_id();
    } else {
    	$_reply["error"] = mysql_error($dblink);
    }
}

function lib_db_table_records_count($tablename, $condition = false) {
    global $dblink, $_reply, $RESULT_BAD_REQUEST, $RESULT_OK;
    $_reply = $RESULT_BAD_REQUEST;
    $sql = "select count(*) as rcount from `" . mysql_escape_string($tablename) . "`";
    if ($condition) $sql .= " where " . $condition;
    $result = @mysql_query($sql, $dblink);
    if ($result) {
        if ($r = @mysql_fetch_assoc($result)) {
            $_reply = $RESULT_OK;
            $_reply["count"] = $r["rcount"];
        }
    }
}

?>
