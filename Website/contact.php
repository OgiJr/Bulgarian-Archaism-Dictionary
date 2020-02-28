<?php
    require "header.php";
?>


<main al>

  <div class="user">
    <header class="user__header">
      <img src="https://s3-us-west-2.amazonaws.com/s.cdpn.io/3219/logo.svg" alt="" />
    </header>

    <form class="form" action="contactform.php" method="post">
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
        <input type="message" name="message" placeholder="Съобщение" class="form__input" />
      </div>

      <button class="btnsignup" type="sumbit" name="submit">Изпрати</button>
    </form>
  </div>

</main>

    <p> “В българският език и литература се използват множество различни архаизми и диалекти. Те могат да затруднят учениците, които не ги разбират, но също така правят нашия език по-богат. Затова, те не могат да бъдат заменени, за да се улеснят произведенията.
<br>През 2019г. бяха сменени 6000 архаични думи в „Под игото“ на Иван Вазов, което доведе до множество отзиви на различни новинарски компании и индивидуални създатели в Интернет пространството в полза на премахването на тези промени. Това ни вдъхнови да създадем това приложение.”

<br>Един от аргументите срещу интеграцията на информационните технологии е необходимостта от много ресурси и средства. Но тук идват на помощ смартфоните на учениците - средство, което досега е използвано, като източник за разсейване.

<br>Смартфоните обаче могат да се превърнат в източник за знание и учение. Почти всеки ученик има достъп до телефон и Интернет, което му позволява да използва тази технология, която от своя страна не носи разход за държавата и МОН да екипира училищата с лаптопи, таблети или хромбуци.
      </p>

<?php
    require "footer.php";
?>
