<?php

if(isset($_POST['login-submit'])) {

    require 'includes/dbh.inc.php';

    $mailuid = $_POST['mailuid'];
    $password = $_POST['pwd'];

    if (empty($mailuid) || empty($password)) {

        header("Location: ../adminlog.php?error=emptyfields");
        exit();

    } else {

      if ($mailuid != "archaism_app" || $password != "DictionaryofArchaism123") {

        header("Location: ../adminlog.php?error=badcredentials");
        exit();

      } else {

         session_start();
         $_SESSION['logged']=true;
         header("Location: ../includes/admin.php?login=success");
      }

    }

}

?>
