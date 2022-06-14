-> Guest_34

=== Guest_34 ===
THOMAS: Приветствую.
GUEST_MAN: Прекрасный день, чтобы напиться!
THOMAS: А вы серьезно настроены. Что я могу вам предложить? 
GUEST_MAN: Пожалуй <alc_CHIVAVA>Чиваву</alc>.
-> Order
 
=== Order ===
#check
* THOMAS: Интересный выбор. Подождите минуту.
  #order
  THOMAS: Получите и распишитесь. 
  -> Grade
* THOMAS: Чивава? Хороший выбор. Но может вы желаете попробовать <alc_ANYTHING></alc>?
  #guestchoice
  ** GUEST_MAN: Ладно, налей мне на пробу.
     #order
     THOMAS: Получите и распишитесь. 
     -> Grade  
  ** GUEST_MAN: Давай без сюрпризов. Просто налей ром.
     #order
     THOMAS: Получите и распишитесь. 
     -> Grade  
* THOMAS: Запасы на сегодня иссякли... Может <alc_ANYTHING></alc>?
  THOMAS: Запасы на сегодня иссякли... Может <alc_ANYTHING></alc>?
  GUEST_MAN: Похоже, моим планам сегодня не сбыться...
  #guestchoice
  ** GUEST_MAN: Ладно, налей мне на пробу.
     #order
     THOMAS: Получите и распишитесь. 
     -> Grade  
  ** GUEST_MAN: Всего доброго... -> END

=== Grade ===
#grade
* GUEST_MAN: Чертовски хорошо. Не даром зовешь себя барменом.
* GUEST_MAN: Для новичка сойдет.
* GUEST_MAN: И ты называешь себя барменом? Позорище.
- -> END