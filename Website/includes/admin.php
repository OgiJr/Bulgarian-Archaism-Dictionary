<?php
  require 'admin_header.php';

/* page left in case of bug in main: adminlog.php */

session_start();

if(isset($_SESSION['logged']) && (($_GET["login"] == "success") || $_GET["login"] == "authorised")) {

      $servername = "localhost:3306";
      $dBUsername = "admin";
      $dBPassword = "Trajanovisa4";
      $dBName = "archqetw_archaism_dictionary";

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
