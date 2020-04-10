<?php
    include 'includes/dbh.inc.php';
?>

<!DOCTYPE html>
<html lang="en" dir="ltr">

<head>
    <meta charset="utf-8">
    <link rel="stylesheet" type="text/css" href="main.css">
    <title></title>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/2.1.3/jquery.min.js">
    </script>
    <script src="https://code.jquery.com/jquery-2.2.4.min.js" integrity="sha256-BbhdlvQf/xTY9gja0Dq3HiwQF8LaCRTXxZKRutelT44=" crossorigin="anonymous"></script>
    <script src="https://code.jquery.com/ui/1.12.0/jquery-ui.min.js" integrity="sha256-eGE6blurk5sHj+rmkfsGYeKyZx3M4bG+ZlFyA7Kns7E=" crossorigin="anonymous"></script>

    <link rel="stylesheet" href="https://cdn.rawgit.com/olifolkerd/tabulator/master/tabulator.css">
    <script type="text/javascript" src="https://cdn.rawgit.com/olifolkerd/tabulator/master/tabulator.js"></script>
</head>

<body class="body2">

    <div class="wrapper2">
        <header class="header1">

            <div class="feature-content">
                <h1>Речник на българския архаизъм</h1>

                <!-- Added more buttons for header and space between them. !-->
                <a href="dictionary.php" class="theme-btn btn-animated">Речник</a> &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp
                <a href="words.php" class="theme-btn btn-animated">Добави дума</a> &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp
                <a href="about.php" class="theme-btn btn-animated">За нас</a> &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp
                <a href="contact.php" class="theme-btn btn-animated">Контакт</a> &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp
				<a href="https://play.google.com/store/apps/details?id=com.archaism.dictionary" class="theme-btn btn-animated">Приложение (Андроид) </a> &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp
				<a href="https://apps.apple.com/us/app/bulgarian-archaism-dictionary/id1506463405?ls=1" class="theme-btn btn-animated">Приложение (Апъл)</a> &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp
				<a href="https://chrome.google.com/webstore/detail/%D1%80%D0%B5%D1%87%D0%BD%D0%B8%D0%BA-%D0%BD%D0%B0-%D0%B0%D1%80%D1%85%D0%B0%D0%B8%D0%B7%D0%BC%D0%B0/mjepoiimoeebijccpgomhjlpioflpjhd?hl=bg" class="theme-btn btn-animated">Хром допълнение</a>
			</div>
        </header>

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
