<?php
/*
// Тест mysql_escape_string
$s = "# " . chr(0) . " # " . chr(13) . " # " . chr(10) . " # " . '\\' . " # " . '"' . " # " . "'" . " # ";
echo $s . "\n";

echo mysql_escape_string($s );
*/

// Тест json
// http://192.168.2.50/dexol/test.php?json={"key":[["\"v1",2],[3,"v4"]]}


/*
echo "<" . $_GET['json'] . "><br/>\n";
$t = array();
$t["test"] = array(
    array(1, "v2"),
    array("v3", 4)
);

echo json_encode($t) . "<br />\n";

print_r(json_decode($_GET['json']));
*/

//phpinfo();

//echo gzcompress("Мама мыла раму Мама мыла раму Мама мыла раму Мама мыла раму Мама мыла раму ");
//echo md5("testestest");

//echo md5('AAAFrAAAzo8Gjyniaw3lR06cHC.VNIeI');

//echo 'test ''1''';
//echo shell_exec('sudo tail -n100 /var/log/apache2/dexol-error.log | dexserver');
?>
