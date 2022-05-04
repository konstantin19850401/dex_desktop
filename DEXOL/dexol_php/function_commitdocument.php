<?php
	$mark = '1';
    if (lib_session_ping($sid)) {
		$mark .= '2';
        if ($dblink) {
			$mark .= '3';
    //  "document", document.ToString());
            $_reply = $RESULT_BAD_REQUEST;
            $_reply['l'] = __FILE__ . " : " . __LINE__;
            if ($_POST['document']) {
				$mark .= '4';
                $_reply['dbg'] = __LINE__;
                $doc = json_decode($_POST['document'], true);
                if ($doc) {
					$mark .= '5';
                    $_reply['dbg'] = __LINE__;
                    $_user = $config_users[$session_time_expire[$sid]["user"]];
                    $_db = $config_db[$db];
                    $_userid = mysql_escape_string($_db["dexuser"]);
                    $_unitid = $_user["db"][$db];

                    $_jdocdate = mysql_escape_string($doc["jdocdate"]);
                    $_docid = mysql_escape_string($doc["docid"]);
                    $_digest = mysql_escape_string($doc["digest"]);
                    $_data = mysql_escape_string($doc["data"]);
                    $_journal = mysql_escape_string($doc["journal"]);
                    $_signature = mysql_escape_string($doc["signature"]);

                    $_criticals = $doc["criticals"];
                    $_peopledata = $doc["peopledata"];
                    $nstatus = $doc["status"];

                    $_reply = $RESULT_OK;
                    $_reply['dbg'] = __LINE__;

                    $sql = false;

					$mark .= "6";

                    if ($doc["action"] == "new") {
                        // Новый документ
                        $sql = "insert into `journal` (locked, locktime, userid, status, " .
                                "signature, jdocdate, unitid, docid, digest, data, journal) " .
                                "values ('', '', '$_userid', $nstatus, '$_signature', " .
                                "'$_jdocdate', $_unitid, '$_docid', '$_digest', '$_data', " .
                                "'$_journal')";
                    }

                    if ($doc["action"] == "edit") {
/*
update `{0}` set locked=@locked, locktime=@locktime, userid=@userid, status=@status,
    signature=@signature, jdocdate=@jdocdate, unitid=@unitid, docid=@docid, digest=@digest,
    data=@data, journal=@journal where id=@id
 */
                        $sql = "update `journal` set locked = '', locktime = '', userid = '" .
                               $_userid . "', status = $nstatus, signature = '$_signature', " .
                               "jdocdate = '$_jdocdate', unitid = $_unitid, docid = '$_docid', " .
                               "digest = '$_digest', data = '$_data', journal = '$_journal' " .
                               "where id = " . $doc["id"];
                    }

					$mark .= "7";

                    $_reply["sql1"] = $sql;

                    if ($sql !== false) {
						$mark .= "8";
                        if (mysql_query($sql, $dblink)) {
                            // Установка DataReferences
							$mark .= "9";

                            if ($doc["action"] == "edit") {
                                $sql2 = "select unitid from `journal` where id = " . $doc["id"];
                                if ($r2 = @mysql_fetch_assoc(@mysql_query($sql2, $dblink))) {
                                    lib_toolbox_set_data_reference("units", $r2["unitid"], 0);
                                }
                            }

                            lib_toolbox_set_data_reference("units", $_unitid, 1);

                            if ($doc["action"] == "new") {
                                lib_toolbox_set_data_reference("users", $_userid, 1);
                            }

                            // Criticals
                            lib_toolbox_set_document_criticals($_signature, $_criticals, true);

                            //TODO Установка People Data
                            if ($doc['peopledata']) {
								$mark .= "a";
                                $pd = $doc['peopledata'];
                                if ($pd['FirstName'] && $pd['SecondName'] && $pd['LastName'] && $pd['Birth']) {
									$mark .= "b";
                                    $nhash = md5($pd['FirstName'] . $pd['SecondName'] . $pd['LastName'] . $pd['Birth']);

                                    $pddata = "";
                                    foreach($pd as $k => $v) $pddata .= $k . "=" . $v . "\n";

                                    $pddata = mysql_escape_string($pddata);

                                    $sql = "select * from `people` where phash = '" . mysql_escape_string($nhash) . "'";

                                    if ($row = @mysql_fetch_assoc(@mysql_query($sql, $dblink))) {
										$mark .= "c1";
                                        $sql = "update `people` set data = '" . $pddata . "' where phash = '" . mysql_escape_string($nhash) . "'";
                                    } else {
										$mark .= "c2";
                                        $sql = "insert into `people` (phash, firstname, secondname, lastname, birth, data) values ('" .
                                                mysql_escape_string($nhash) . "', '" . mysql_escape_string($pd['FirstName']) . "', '" .
                                                mysql_escape_string($pd['SecondName']) . "', '" . mysql_escape_string($pd['LastName']) . "', '" .
                                                mysql_escape_string($pd['Birth']) . "', '" . $pddata . "')";
                                    }

                                    $_reply["sql2"] = $sql;

                                    @mysql_query($sql, $dblink);
                                }
                            }
                        }
                    } else {
                        $_reply = $RESULT_BAD_REQUEST;
                        $_reply['l'] = __FILE__ . " : " . __LINE__;
                    }

                }
            }
        }
    } else {
        $_reply = $RESULT_NEED_AUTH;
        $_reply['l'] = __FILE__ . " : " . __LINE__;
    }

//	$_reply['mark'] = $mark;
?>
