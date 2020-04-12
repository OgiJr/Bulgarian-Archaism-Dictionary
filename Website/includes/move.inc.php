<?php
	include_once('dbh.inc.php');

	if( isset($_GET['move']) )
	{
		$id = $_GET['move'];
		$sql= "INSERT INTO dictionary (dictionary.word, dictionary.definition)
          SELECT suggestions.word, suggestions.definition
          FROM suggestions
          WHERE id='$id'";
    $res= mysqli_query($conn, $sql) or die("Failed ".mysqli_error($conn));
		echo "<meta http-equiv='refresh' content='0;url=admin.php?login=authorised'>";
	}
?>
