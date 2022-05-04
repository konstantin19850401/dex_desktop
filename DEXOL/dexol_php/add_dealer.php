<html>
	<head>
		<meta http-equiv="Content-Type" content="text/html; charset=windows-1251">
		<title>test</title>
	</head>
	<body>
		<script> 
			function hendler(elem) {
				if (elem.value != "") {
					console.log(elem.value);
					console.log(document.getElementById("number").value);
					if (elem.value == "КБР" && document.getElementById("number").value != "") {
						var number = document.getElementById("number").value;
						document.getElementById("db_list").value = "MEGA="+number+",BEE_KBR="+number+",MTS_KBR="+number; 
						document.getElementById("profile_code").value = "Ie8JG596";
					}
					else if (elem.value == "КЧР" && document.getElementById("number").value != "") {
						var number = document.getElementById("number").value;
						document.getElementById("db_list").value = "MEGA="+number+",BEE_KCR="+number+",MTS_KCR="+number+",BEE_KCR2="+number+",BEE_KCR_MOBI="+number;
						document.getElementById("profile_code").value = "wpZ75sF4";
					}
					else if (elem.value == "СК" && document.getElementById("number").value != ""){
						var number = document.getElementById("number").value;
						document.getElementById("db_list").value = "MEGA="+number+",BEE_STS="+number+",BEE_STS2="+number+",BEE_STS_BTS="+number+",MTS_STS_BTS="+number+",MTS_STS="+number+",MTS_STS_062013="+number;
					} else {
						document.getElementById("db_list").value = "";
						document.getElementById("profile_code").value = "";
					}
				}
			}
		</script>
		<? echo "Test page PHP"; ?><br/>DEXOL
		
		<form action="./handler.php" method="POST">
			Number: <input type="text" name="login" id="number"/><br />
			Name: <input type="text" name="name" /><br />
			<!--Region:  <input type="text" name="region" /><br />-->
			Region: <select onclick="hendler(this)" name="region">
						<option></option>
						<option>КБР</option>
						<option>КЧР</option>
						<option>СК</option>
					</select> <br />
			<!--User props: <input type="text" name="user_props" value="ofis"/><br />-->
			Profile code: <input type="text" name="profile_code" id="profile_code"/> <br />
			Db list: <input type="text" name="db_list" id="db_list"/ size="50" height="50"> <br />
			<input type="submit" value="GO" />
		</form>
		
		
	</body>
</html>