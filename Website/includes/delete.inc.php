<?php
	include_once('dbh.inc.php');

	if( isset($_GET['del']) )
	{
		$id = $_GET['del'];
		$sql= "DELETE FROM suggestions WHERE id='$id'";
    $res= mysqli_query($conn, $sql) or die("Failed".mysqli_error());
		echo "<meta http-equiv='refresh' content='0;url=admin.php?login=authorised'>";
	}
?>
