-> Guest_26

=== Guest_26 ===
GUEST_MAN: Мужик, не знаешь, почему шерифа в участке нет?
THOMAS: Без понятия, может делами занят в городе.
GUEST_MAN: Ха! Да какие у него могут быть тут дела? Тишь одна...
THOMAS: Ну... Может поэтому и тишь, что он этим занят.
GUEST_MAN: Ладно, подожду здесь...
GUEST_MAN: Коктейль какой-нибудь хочу... <alc_GODFATHER>Крестного отца</alc> нальёшь? 
-> Order

=== Order ===
#check
* THOMAS: Налью.
#order
  THOMAS: Держи. 
  -> Grade
* THOMAS: Может какой другой коктейль? Например <alc_ANYTHING></alc>.
  #guestchoice:
  ** GUEST_MAN: Похер, давай.
     #order
     THOMAS: Держи. 
     -> Grade
  ** GUEST_MAN: Не, мне такого не надо.
    THOMAS: Хорошо.
    #order
    THOMAS: Держи. 
    -> Grade
* THOMAS: Отца нет, может лучше <alc_ANYTHING></alc>?
  #guestchoice:
  ** GUEST_MAN: Похер, давай.
     #order
     THOMAS: Держи. 
     -> Grade
  ** GUEST_MAN: Похуй, возле участка шерифа подожду... -> END

=== Grade
#grade
    * GUEST_MAN: Мужик, спасибо... Буду почаще к тебе заходить...
    * GUEST_MAN: Сойдет... Удачной торговли...
    * GUEST_MAN: Хрень какая-то... Если бы не шериф, не зашел бы сюда...
- -> END