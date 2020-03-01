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

        <div class="user">
            <header class="user__header">
                <img src="https://s3-us-west-2.amazonaws.com/s.cdpn.io/3219/logo.svg" alt="" />
            </header>

            <br>

            <h3>
										Преди да предадете дума, моля проверете дали вече я има в нашия речник; Дали дефиницията, която сте дали е актуална и я сравнете с някой източник. Ще погледнем Вашата дума и ще преценим дали е подходяща. Очаквайте тя да бъде качена към речника ни до 48 часа. Благодаря Ви.
									</h3>

            <form class="form" action="includes/suggestedwords.inc.php" method="post">
                <div class="form__group">
                    <input type="text" name="word" placeholder="Дума" class="form__input" />
                </div>
                <br>
                <div class="form__group">
                    <textarea cols="40" type="text" name="definition" placeholder="Дефиниция" class="form__input"></textarea>
                </div>
                <br>
                <button class="btnsignup" type="sumbit" name="word-submit">Изпратете</button>
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