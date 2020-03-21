<!DOCTYPE html>
<html>

<head>
    <meta charset="utf-8">
</head>

<body>

<?php
$sql = "SELECT * FROM `dictionary`";

$dsn = "mysql:dbname=archaism_dictionary;host:localhost:3306;charset=utf8";
$conn = new PDO($dsn, "archaism_app", "DictionaryOfArchaism123");

$stmt = $conn->prepare($sql);
$stmt->execute();
$result = $stmt->fetchAll();

$pre = "{
	Property1: \n";
$during = json_encode($result, JSON_UNESCAPED_UNICODE);
$after = "\n }";

echo $pre;
echo $during;
echo $after;
?>

</body>
</html>
