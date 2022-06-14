-> Guest_21

=== Guest_21 ===
GUEST_WOMAN: Добрый вечер, нальёте?
THOMAS: Чего желаете?
GUEST_WOMAN: Я не выпивоха, но заметила, что тяготею ко всему кислому и цитрусам. Что посоветуете? 
-> order

===order
THOMAS: Я могу предложить вам <alc_ANYTHING></alc>.
 #guestchoice
 *GUEST_WOMAN: Да, будет прекрасно...
   #order
   THOMAS: Заказ готов. 
   -> Grade
 *GUEST_WOMAN: Может что-то более приятное для дамы?
   THOMAS: Эмм... <alc_ANYTHING></alc>?
   #guestchoice
    **GUEST_WOMAN: Так уже намного лучше.
      #order
      THOMAS: Заказ готов. 
      -> Grade
    **GUEST_WOMAN: За кого вы меня принимаете? -> END

=== Grade
#grade
    * GUEST_WOMAN: Это было потрясающе... Благодарю вас...
    * GUEST_WOMAN: Спасибо... Тут довольно неплохо...
    * GUEST_WOMAN: У меня были более высокие ожидания от этого места... Всего доброго... 
- -> END