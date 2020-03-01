<?php
    require "header.php";
?>

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

    <?php
    require "footer.php";
?>
