<?php
$servername = "localhost:3306";
$dBUsername = "archaism_app";
$dBPassword = "DictionaryOfArchaism123";
$dBName = "archaism_dictionary";

$con = new PDO("mysql:host=localhost:3306;dbname=archaism_dictionary", "archaism_app", "DictionaryOfArchaism123");

$word = $_GET['word'];

$sqlQuery = "SELECT word,definition FROM dictionary WHERE word = '$word' ";

$sth = $con->prepare($sqlQuery);

$sth -> setFetchMode(PDO:: FETCH_OBJ);
$sth -> execute();

if($row = $sth -> fetch())
{
echo $row -> word;
echo "<br>";
echo $row -> definition;
}

?>
