<?php
    require "header.php";
?>

  <?php

    echo '
    <div class="user">
    <header class="user__header">
    <img src="https://s3-us-west-2.amazonaws.com/s.cdpn.io/3219/logo.svg" alt="" />
    </header>
    <form class="form" action="admin.inc.php" method="post">
    <div class="form__group">
    <input class="form__input" type="text" name="mailuid" placeholder="Име/E-mail..">
    </div>
    <div class="form__group">
    <input class="form__input" type="password" name="pwd" placeholder="Парола..">
    </div>
    <br>
    <button class="btnsignup" type="submit" name="login-submit">Влез</button>
    </form>
    </div>';

 ?>

<?php
    require "footer.php";
?>
