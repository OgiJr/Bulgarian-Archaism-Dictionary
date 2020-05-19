<?php
    require 'header.php';
    include 'includes/dbh.inc.php';
?>


		<div class="user">

			<?php
			include("includes/search_word.php");

			$word = $_GET['word'];

			$sqlQuery = "SELECT word,definition FROM dictionary WHERE word = '$word' ";

			$sth = $con->prepare($sqlQuery);

			$sth -> setFetchMode(PDO:: FETCH_OBJ);
			$sth -> execute();

			if($row = $sth -> fetch())
			{
			$wordFinal = $row -> word;
			$defFinal = $row -> definition;

			echo "<h1> $wordFinal </h1> <h2> $defFinal </h2><br><br><br><br>";
			}

			?>

            <header class="user__header">
                <img src="https://s3-us-west-2.amazonaws.com/s.cdpn.io/3219/logo.svg" alt="" />
            </header>

            <form class="form" action="dictionary.php" method="SEARCH">
                <div class="form__group">
                    <input type="text" name="word" placeholder="Дума" class="form__input" />
                </div>
                <br>
                <input type ="submit" class="btnsignup" value = "Потърсете"></input>
            </form>

        </div>

        <div class="menu-icon">
            <img src="https://img.icons8.com/cotton/2x/menu.png" />
        </div>

        <div class="nav-bar">
            <ul>
                <li>
                    <a href="index.php">
            Начална страница
          </a>
                </li>
                <li>
                    <a href="words.php">
            Добави дума
          </a>
                </li>
                <li>
                    <a href="dictionary.php">
            Речник
          </a>
                </li>
				<li>
                    <a href="about.php">
            За нас
          </a>
                </li>
				<li>
					<a href="contact.php">
			Контакт
		  </a>
				</li>
                <li>
                    <a href="https://play.google.com/store/apps/details?id=com.archaism.dictionary">
            Приложение
          </a>
                </li>
            </ul>
        </div>
    </div>

    <?php
    require "footer.php";
?>
