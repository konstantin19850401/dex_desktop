<?

$dexol_dblink = mysql_connect(
	$config_default_dbhost . ':' . $config_default_dbport,
	$config_default_dbuser,
	$config_default_dbpass
	);

if ($dexol_dblink && mysql_select_db('dexol', $dexol_dblink)) {
	mysql_set_charset('utf8', $dexol_dblink);

	if ($result = mysql_query("select * from `users`", $dexol_dblink)) {		while ($row = mysql_fetch_assoc($result)) {
			$st = explode(",", $row['db_list']);
			$dbs = array();
			foreach($st as $s) {				$ss = explode("=", $s);
				if (count($ss) > 1)	$dbs[trim($ss[0])] = trim($ss[1]);			}

			$config_users[$row['login']] = array(
				'name' => $row['name'],
				'password' => $row['password'],
				'user_props' => $row['user_props'],
				'profile_code' => $row['profile_code'],
				'db' => $dbs
			);
		}
	}
}

?>