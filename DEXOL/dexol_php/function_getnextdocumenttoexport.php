<?php
/**
 * Params: docid1 .. docidN
 */

    if (lib_session_ping($sid)) {
        if ($dblink) {
			$_reply = $RESULT_BAD_REQUEST;

            $_reply['l'] = __FILE__ . " : " . __LINE__;
            $sql = "select * from `journal` where status = 3";

			$request_valid = true;

			if (strpos($config_db[$db]['engine'], 'efd')) {
				if ($user['profile_code']) {
					$sql .= ' and data like \'%<ProfileCode>' . $user['profile_code'] . '</ProfileCode>%\'';
				} else {
					$request_valid = false;
					$_reply = $RESULT_INTERNAL_ERROR; //= array("result" => 6, "message" => "Внутренняя ошибка сервера");
					$_reply['message'] = "В конфигурации пользователя не найдено поле profile_code";
				}
			}

			if ($request_valid) {
				$wh = "";
				$i = 1;
				while($_GET["docid" . $i]) {
					$s = $_GET["docid" . $i];
					if (strlen($wh) > 0) $wh .= " or ";
					$wh .= "docid = '" . mysql_escape_string($s) . "'";
					$i++;
				}
				if (strlen($wh) > 0) {
					$sql .= " and (" . $wh . ") ";
				}
				$_reply['l'] = __FILE__ . " : " . __LINE__;

				$sql .= "order by jdocdate limit 0, 1";

				if ($result = mysql_query($sql, $dblink)) {
					$_reply = $RESULT_NEGATIVE;
					if ($row = mysql_fetch_assoc($result)) {
						$_reply = $RESULT_OK;
						$_reply["data"] = array(
							"status" => $row["status"],
							"digest" => $row["digest"],
							"signature" => $row["signature"],
							"jdocdate" => $row["jdocdate"],
							"unitid" => $row["unitid"],
							"text" => $row["data"],
							"journal" => $row["journal"]
						);
					}
				}
				$_reply["sql"] = $sql;
			}
        }
    } else {
        $_reply = $RESULT_NEED_AUTH;
    }

?>
