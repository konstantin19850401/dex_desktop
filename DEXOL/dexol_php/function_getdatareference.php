<?php
    if (lib_session_ping($sid)) {
        lib_toolbox_get_data_reference($_GET['tablename'], $_GET['key']);
    } else {
        $_reply = $RESULT_NEED_AUTH;
    }
?>
