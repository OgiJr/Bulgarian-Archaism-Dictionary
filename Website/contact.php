<?php
    require "header.php";
?>

        <div class="user">
            <header class="user__header">
                <img src="https://s3-us-west-2.amazonaws.com/s.cdpn.io/3219/logo.svg" alt="" />
            </header>

            <form class="form" action="includes/contactform.inc.php" method="post">
                <div class="form__group">
                    <input type="text" name="name" placeholder="Име" class="form__input" />
                </div>

                <div class="form__group">
                    <input type="text" name="mail" placeholder="E-mail" class="form__input" />
                </div>

                <div class="form__group">
                    <input type="text" name="Subject" placeholder="Предмет" class="form__input" />
                </div>

                <div class="form__group">
                    <textarea cols="40" type="message" name="message" placeholder="Съобщение" class="form__input"></textarea>
                </div>
                <br>
                <button class="btnsignup" type="sumbit" name="submit">Изпратете</button>
            </form>
        </div>

<?php
    require "footer.php";
?>
