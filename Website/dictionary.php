<?php

$servername = "localhost:3306";
$dBUsername = "archaism_app";
$dBPassword = "DictionaryOfArchaism123";
$dBName = "archaism_dictionary";

$conn = mysqli_connect($servername, $dBUsername, $dBPassword, $dBName);

if (!$conn) {
  die("Свързването е неуспешно: ".mysqli_connect_error());
}

$query = "SELECT * FROM dictionary"; //You don't need a ; like you do in SQL
$result = mysql_query($query);

echo "<table>"; // start a table tag in the HTML

while($row = mysql_fetch_array($result)){   //Creates a loop to loop through results
echo "<tr><td>" . $row['id'] . "</td><td>" . $row['word'] . "</td><td>" . $row['definition'] . "</td></tr>";  //$row['index'] the index here is a field name
}

echo "</table>"; //Close the table in HTML

mysql_close();

 ?>
