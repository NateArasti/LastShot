-> Guest_50

=== Guest_50 ===
GUEST_WOMAN: Эй, хозяин, как жизнь?
THOMAS: Доброго дня. Не жалуюсь.
THOMAS: Что будете пить?
GUEST_WOMAN: Даже не знаю... А что у вас есть?
THOMAS: Всё, что вашей душе угодно. Проще услышать ваши предпочтения.
GUEST_WOMAN: Настроения вообще нет — только упиться. 
GUEST_WOMAN: Плесни мне в стакан чего-то крепкого.
-> Order

=== Order ===
 THOMAS: Понимаю твой настрой. Могу предложить <alc_ANYTHING></alc>.
  #guestchoice
  * GUEST_WOMAN: Да что угодно.
     #order
     THOMAS: Получай заказ. 
     -> Grade
  * GUEST_WOMAN: Нет, не хочется этого...
    THOMAS: <alc_ANYTHING></alc> для таких случев в самый раз.
    #guestchoice
    **GUEST_WOMAN: А плевать. Тащи.
      #order
      THOMAS: Получай заказ. 
      -> Grade
    **GUEST_WOMAN: Да ну, что-то дажае пить расхотелось... -> END

=== Grade ===
#grade
    * GUEST_WOMAN: Лучше не стало, но хотя бы было вкусно. 
    * GUEST_WOMAN: Даже алкоголь не помогает...
    * GUEST_WOMAN: Не думала, что может стать еще хуже...
- -> END
