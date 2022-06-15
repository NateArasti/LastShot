-> Guest_43

=== Guest_43 ===
GUEST_MAN: Уважаемый, ты заправляешь этим заведением?
THOMAS: Я. Какие-то проблемы?
GUEST_MAN: Нет, нет. Просто не видел тебя раньше. 
GUEST_MAN: Плесни мне чего крепкого, рома там, или виски, например.
-> Order

=== Order ===
 THOMAS: <alc_ANYTHING></alc> сойдёт?
 THOMAS: <alc_ANYTHING></alc> сойдёт?
  #guestchoice
  * GUEST_MAN: Да, вполне.
     #order
     THOMAS:  Лови. 
     -> Grade
  * GUEST_MAN: Не, мне такого не надо. Получше не найдется?
    THOMAS: Ну может <alc_ANYTHING></alc>?
    THOMAS: Ну может <alc_ANYTHING></alc>?
    #guestchoice
    **GUEST_MAN: Вот. Сразу бы так!
      #order
      THOMAS: Лови. 
      -> Grade
    **GUEST_MAN: Да к черту. Даже пить перехотелось... -> END

=== Grade ===
#grade
    * GUEST_MAN: Охх... Сразу легче на душе.
    * GUEST_MAN: Ты тут на замене? Как будто первый день алкашку разливаешь.
    * GUEST_MAN: Ну и пойло. Как будто первый день алкашку разливаешь.
- -> END 