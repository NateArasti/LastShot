-> Guest_34

=== Guest_34 ===
GUEST_MAN: Вы обслуживаете?
THOMAS: Да, конечно...
-> Order
 
=== Order ===
THOMAS: Чего желаете?
GUEST_MAN: Плесни мне <alc_A_WHISKEY>виски местного разлива</alc>...
#check
* THOMAS: Это мы можем, один момент...#order
    THOMAS: Ваш заказ...
  -> Grade
* THOMAS: Увы, его нет в наличии... Никогда такого не было, и вот опять... Может попрубуете <alc_ANYTHING></alc>?
  GUEST_MAN:
  #guestchoice
  ** GUEST_MAN: Почему бы и нет? Наливай...#order
     THOMAS: Ваш заказ 
     -> Grade  
  ** GUEST_MAN: Постой, я сюда именно за виски пришел...
     THOMAS: Увы... Не могу ничего вам предложить...
     GUEST_MAN: Тогда до свидания...
     -> DONE

=== Grade ===
#grade
* GUEST_MAN: Спасибо, всё по высшему качеству...
* GUEST_MAN: Неплохо, спасибо...
* GUEST_MAN: Сойдёт... Всего доброго...
- -> DONE
