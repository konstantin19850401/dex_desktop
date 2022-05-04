<?php
    function lib_session_create($login) {
        global $SESS_DIR, $config_default_session_timeout_sec;

        $sess_hash = md5($login . date("YmdHisU"));
        $exptime = time() + $config_default_session_timeout_sec;
        $exptext = '<? $session_time_expire["' . $sess_hash . '"] = array("user" => "' . $login .'", "exptime" => ' . $exptime . '); ?>';
        file_put_contents($SESS_DIR . "/" . $sess_hash, $exptext);

        return $sess_hash;
    }

    function lib_session_ping($sid) {
        global $SESS_DIR, $config_default_session_timeout_sec, $session_time_expire;

        if (file_exists($SESS_DIR . "/" . $sid)) {
            $exptime = time() + $config_default_session_timeout_sec;
            $exptext = '<? $session_time_expire["' . $sid . '"] = array("user" => "' . $session_time_expire[$sid]["user"] .'", "exptime" => ' . $exptime . '); ?>';
            @unlink($SESS_DIR . "/" . $sid);
            file_put_contents($SESS_DIR . "/" . $sid, $exptext);
            return true;
        }
        return false;
    }

    function lib_session_logout($sid) {
        global $SESS_DIR;

        if (file_exists($SESS_DIR . "/" . $sid)) {
            @unlink($SESS_DIR . "/" . $sid);
        }

        return true;
    }

    // Инициализация массива сессий
    $session_time_expire = array();

    $d = dir($SESS_DIR);
    while(false !== ($entry = $d->read())) {
        if ($entry != "." && $entry != ".." && $entry != "...") {
            require_once $SESS_DIR . "/" . $entry;
        }
    }
    $d->close();

    // Удаление устаревших сессий
    foreach($session_time_expire as $k => $v) {
        if ($v['exptime'] < time()) {
            // Грохнем сессию
            @unlink($SESS_DIR . "/" . $k);
            $session_time_expire[$k] = false;
        }
    }
?>
