<?php
    require "header.php";
    include 'includes/dbh.inc.php';
?>


<?php
$servername = "localhost:3306";
$dBUsername = "archaism_app";
$dBPassword = "DictionaryOfArchaism123";
$dBName = "archaism_dictionary";

$conn = mysqli_connect($servername, $dBUsername, $dBPassword, $dBName);

$sqlQuery = "SELECT * FROM dictionary";
$qry = $conn->query($sqlQuery);

echo "<table id='myTable'><tr><th>id</th><th>word</th><th>definition</th></tr>";
while($row = $qry->fetch_assoc()){
echo "<tr><td>".$row["id"]."</td><td>".$row["word"]."</td><td>".$row["definition"]."</td><td></tr>";
}

echo "</table>";

?>

<?php
    require "footer.php";
?>
