-> Guest_31

=== Guest_31 ===
GUEST_MAN: Хэй, хозяин, как жизнь?
THOMAS: Работаем, процветаем.
GUEST_MAN: Прекрасно, дружище... Позволь внести вклад в ваше процветание.
THOMAS: Что я могу вам предложить? 
GUEST_MAN: Просто налей мне <alc_DARKBEER>пива темного</alc>.
-> Order
 
=== Order ===
#check
*THOMAS: Как скажете.
  #order
  THOMAS: Ваш заказ. 
  -> Grade
*THOMAS: Классика. Может хотите чего нового? Например <alc_ANYTHING></alc>.
  #guestchoice
  ** GUEST_MAN: А хорошо звучит! Наливай.
     #order
     THOMAS: Прошу, ваш заказ. 
     -> Grade  
  ** GUEST_MAN: Я не просил чего-то изысканного... Простого темного пива.
     THOMAS: Предложить стоило.
     #order
     THOMAS: Прошу, ваш заказ. 
     -> Grade
*THOMAS: Прости, друг, не могу налить... Могу я предложить тебе <alc_ANYTHING></alc>?
THOMAS: Прости, друг, не могу налить... Могу я предложить тебе <alc_ANYTHING></alc>?
  GUEST_MAN: Как так? Салун и нет пива...
  THOMAS: Не забывай, что мы в глуши живём... Не так уж и легко наладить тут бизнес.
  #guestchoice
  ** GUEST_MAN: Ладно. Верю тебе, наливай.
     #order
     THOMAS: Прошу, ваш заказ... 
     -> Grade  
  ** GUEST_MAN: Все конечно понимаю, но чтоб простецкого пива не было...
     GUEST_MAN: Ладно, хрен с тобой... Удачи в бизнесе! -> END

=== Grade ===
#grade
* GUEST_MAN: Отменный вкус! Спасибо, дружище. 
* GUEST_MAN: Неплохо-неплохо.
* GUEST_MAN: Ожидания не оправдались...
- -> END

