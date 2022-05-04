<?php

function _log($msg) {
    /*
	$file_path = $_SERVER['DOCUMENT_ROOT'].'logs/dexol.log';

	$text = htmlspecialchars(date("c") . " " . $msg)."\r\n";
	$handle = fopen($file_path, "a");
	@flock ($handle, LOCK_EX);
	fwrite ($handle, $text);
	@flock ($handle, LOCK_UN);
	fclose($handle);
     * 
     */
}

?>