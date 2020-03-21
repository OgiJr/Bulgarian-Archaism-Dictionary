<?php
$sql = "SELECT * FROM `dictionary`";

$dsn = "mysql:dbname=archaism_dictionary;host:localhost:3306;charset=utf8";
$conn = new PDO($dsn, "archaism_app", "DictionaryOfArchaism123");

$stmt = $conn->prepare($sql);
$stmt->execute();
$result = $stmt->fetchAll();

$pre = '{
	"Property1": [
		{
			"type": "header",
			"version": "4.8.3",
			"comment": "Export to JSON plugin for PHPMyAdmin"
		},
		{
			"type": "database",
			"name": "archaism_dictionary"
		},
		{
			"type": "table",
			"name": "dictionary",
			"database": "archaism_dictionary",
			"data":';
$during = json_encode($result, JSON_UNESCAPED_UNICODE);
$after = "}]}";

echo $pre;
echo $during;
echo $after;
?>
