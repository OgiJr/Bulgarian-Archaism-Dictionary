<?php
  require 'header.php';

    if(isset($_SESSION['logged'])) {

      $servername = "localhost:3306";
      $dBUsername = "archaism_app";
      $dBPassword = "DictionaryOfArchaism123";
      $dBName = "archaism_dictionary";

      $conn = mysqli_connect($servername, $dBUsername, $dBPassword, $dBName);

      $sqlQuery = "SELECT * FROM suggestions";
      $qry = $conn->query($sqlQuery);

      echo "<table id='myTable'><tr><th>id</th><th>word</th><th>definition</th></tr>";
      while($row = $qry->fetch_assoc()){
      echo "<tr><td>".$row["id"]."</td><td>".$row["word"]."</td><td>".$row["definition"]."</td><td><a href='delete.inc.php?del=$row[id]'>изтрии</a></td><td><a href='move.inc.php?move=$row[id]'>приеми</a></td></tr>";

      }

      echo "</table>";


    } else {
      echo "<h1>Нямате достъп!</h1>";
    }


      require "footer.php";
  ?>
