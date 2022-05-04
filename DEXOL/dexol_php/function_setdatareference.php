<?php
    if (lib_session_ping($sid)) {
        lib_toolbox_set_data_reference($_GET['tablename'], $_GET['key'], $_GET['doincrement']);
    } else {
        $_reply = $RESULT_NEED_AUTH;
    }
?>
