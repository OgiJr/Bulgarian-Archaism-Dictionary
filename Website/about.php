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
                <a href="#" class="theme-btn btn-animated">Приложение</a>
            </div>
        </header>
        <br>
        <header class="user__header">
            <img src="https://s3-us-west-2.amazonaws.com/s.cdpn.io/3219/logo.svg" alt="" />
        </header>

        <br>
        <h2> В българският език и литература се използват множество различни архаизми и диалекти. Те могат да затруднят учениците, които не ги разбират, но също така правят нашия език по-богат. Затова, те не могат да бъдат заменени, за да се улеснят произведенията.
<br><br>През 2019г. бяха сменени 6000 архаични думи в „Под игото“ на Иван Вазов, което доведе до множество отзиви на различни новинарски компании и индивидуални създатели в Интернет пространството в полза на премахването на тези промени. Това ни вдъхнови да създадем това приложение.
<br><br>Един от аргументите срещу интеграцията на информационните технологии е необходимостта от много ресурси и средства. Но тук идват на помощ смартфоните на учениците - средство, което досега е използвано, като източник за разсейване.

<br><br>Смартфоните обаче могат да се превърнат в източник за знание и учение. Почти всеки ученик има достъп до телефон и Интернет, което му позволява да използва тази технология, която от своя страна не носи разход за държавата и МОН да екипира училищата с лаптопи, таблети или хромбуци.
<br><br>
</h2>

        <div class="user">
            <br>

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
                        <a href="contact.php">
            За нас
          </a>
                    </li>
                    <li>
                        <a href="index.php">
            Приложение
          </a>
                    </li>
                </ul>
            </div>
        </div>

        <?php
    require "footer.php";
?>
