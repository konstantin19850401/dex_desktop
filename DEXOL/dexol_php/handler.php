<?php 
	require_once "settings.php";
	$db = mysql_connect($settings['db_host'],$settings['db_user'],$settings['db_pass']) or die('cant connect db');
	mysql_select_db($settings['db_name']) or die('cant select db');
	mysql_query("SET NAMES cp1251");
	$login = htmlspecialchars($_POST['login']);
	$name = htmlspecialchars($_POST['name']);
	$region = htmlspecialchars($_POST['region']);
	$profile_code = "ofis";
	$password = htmlspecialchars($_POST['profile_code']);
	$db_list = htmlspecialchars($_POST['db_list']);
	$user_props = "<Properties><DefaultDocumentState>1</DefaultDocumentState></Properties>";
	$login = "user".$login;
	$name = $name."-".$region;

	$query = "INSERT INTO `users` (`login`, `name`, `password`, `user_props`, `profile_code`, `db_list`) VALUES ('".$login."','".$name."','".$password."', '".$user_props."', '".$profile_code."', '".$db_list."')";
	$result = mysql_query($query) or die ("<p>Request failed</p>");	
	if ($result) {
		echo 'The operation is completed successfully';
	} else {
		echo 'The operation will fail';
	}
	
?>