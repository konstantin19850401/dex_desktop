<?
if (file_exists("update/update.exe")) {
    $file = ("update/update.exe");
    header  ("Content-Type: application/octet-stream");
    header  ("Accept-Ranges: bytes");
    header  ("Content-Length: ".filesize ($file));
    header  ("Content-Disposition: attachment; filename=update.exe");
    readfile ($file);
    $supress_output = true;
}

?>
