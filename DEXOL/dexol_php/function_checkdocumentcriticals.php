<?php
    if (lib_session_ping($sid)) {
        if ($dblink) {
            if ($_GET['fields'] && $_GET['signature']) {
                $fields = json_decode($_GET['fields']);
                if ($fields) {

                    $_reply = $RESULT_OK;
                    $ret = array();
                    foreach($fields as $k => $v) {
                        $sql = "select * from `criticals` where cname = '" .
                            mysql_escape_string($k) . "' and cvalue = '" .
                            mysql_escape_string($v) . "' and signature <> '" .
                            mysql_escape_string($_GET['signature']) . "'";

                        if ($result = mysql_query($sql, $dblink)) {
                            while($row = mysql_fetch_assoc($result)) {
                                $sql = "select digest from `journal` where signature='" .
                                        mysql_escape_string($row['signature']) . "'";
                                if ($result2 = mysql_query($sql, $dblink)) {
                                    while($row2 = mysql_fetch_assoc($result2)) {
                                        $ret[] = $k . ": " . $v . " (" . $row2['digest'] . ")";
                                    }
                                }
                            }
                        }
                    }
                    $_reply['data'] = $ret;
                } else {
                    $_reply = $RESULT_BAD_REQUEST;
                    $_reply['l'] = __FILE__ . " : " . __LINE__;
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
