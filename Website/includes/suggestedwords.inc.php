<?php

if (isset($_POST['word-submit'])) {

    require 'dbh.inc.php';

    $word = $_POST['word'];
    $definition = $_POST['definition'];

    $sql = "SELECT word FROM suggestions WHERE id=?";
          $stmt = mysqli_stmt_init($conn);

          if (!mysqli_stmt_prepare($stmt, $sql)) {

              header("Location: ../words.php?error=sqlerror");
              exit();

          } else {

              mysqli_stmt_bind_param($stmt, "s", $word);
              mysqli_stmt_execute($stmt);
              mysqli_stmt_store_result($stmt);

              $sql = "INSERT INTO suggestions (word, definition) VALUES (?, ?)";
              $stmt = mysqli_stmt_init($conn);

              if (!mysqli_stmt_prepare($stmt, $sql)) {

                  header("Location: ../words.php?error=sqlerror");
                  exit();

              } else {


                  mysqli_stmt_bind_param($stmt, "ss", $word, $definition);
                  mysqli_stmt_execute($stmt);


                  header("Location: ../words.php?submission=success");
                  exit();


                  }
              }

		$result = "Test";
		file_put_contents('/dictionaryTest.txt', $result);

      mysqli_stmt_close($stmt);
      mysqli_close($conn);
    }

    ?>
