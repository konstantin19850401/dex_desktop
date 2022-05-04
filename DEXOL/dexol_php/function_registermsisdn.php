<?php
/**
 * Params: msisdn, icc, owner_id, date_sold
 * Return: journal[]
 */

    if (lib_session_ping($sid)) {
        if ($dblink) {
			$_reply = $RESULT_BAD_REQUEST;
            $_reply['l'] = __FILE__ . " : " . __LINE__;

            $sql = "select * from `um_data` where msisdn = '" . mysql_escape_string($_GET['msisdn']) . "'";

			$_l = array();

            if ($row = @mysql_fetch_assoc(@mysql_query($sql, $dblink))) {
            	$_l[] = "Запись обнаружена в справочнике SIM-карт";

            	$remark = $row['comment'];

            	if ($_GET['owner_id'] != $row['owner_id']) {
            		$_l[] = "Отделение-владелец карты отличается от отделения, заключившего договор";
					$remark .= (strlen($remark) > 0 ? ";" : "") . "Предыдущий владелец: " . $row['owner_id'];
            	}

				if ($_GET['icc'] != $row['icc']) {
					$_l[] = "ICC карты отличается от предыдущего значения";
					$remark .= (strlen($remark) > 0 ? ";" : "") . "Предыдущий ICC: " . $row['icc'];
				}

				$sql = "update `um_data` set status = 2, owner_id = " . $_GET['owner_id'] . ", date_sold = '" .
						mysql_escape_string($_GET['date_sold']) . "', icc = '" . mysql_escape_string($_GET['icc']) .
						"', comment = '" . mysql_escape_string($remark) . "' where msisdn = '" .
						mysql_escape_string($_GET['msisdn']) . "'";


				$_reply = $RESULT_OK;
				if (mysql_query($sql, $dblink)) {
					$_l[] = "Сведения о SIM-карте изменены успешно";
				} else {
					$_l[] = "Запрос на изменение сведений о SIM-карте вернул ошибку:";
					$_l[] = mysql_error();
					$_l[] = "sql = " . $sql;
				}
            } else {
            	$_l[] = "Запись не обнаружена в справочнике SIM-карт";

				$sql =	"insert into `um_data` (status, msisdn, icc, date_in, owner_id, date_own, date_sold, " .
						"region_id, party_id, plan_id) values (2, '" . mysql_escape_string($_GET['msisdn']) .
						"', '" . mysql_escape_string($_GET['icc']) . "', '" . mysql_escape_string($_GET['date_sold']) .
						"', " . $_GET['owner_id'] . ",  '" . mysql_escape_string($_GET['date_sold']) . "', '" .
						mysql_escape_string($_GET['date_sold']) . "', '-', 1, '-')";

				$_reply = $RESULT_OK;
				if (mysql_query($sql, $dblink)) {
					$_l[] = "Сведения о SIM-карте добавлены успешно";
				} else {
					$_l[] = "Запрос на добавление сведений о SIM-карте вернул ошибку";
					$_l[] = mysql_error();
					$_l[] = "sql = " . $sql;
				}
            }

			$_reply['journal'] = $_l;
        }
    } else {
        $_reply = $RESULT_NEED_AUTH;
    }

?>
