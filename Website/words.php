<?php
  require 'header.php';
 ?>

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
  

    <?php
    require "footer.php";
?>
