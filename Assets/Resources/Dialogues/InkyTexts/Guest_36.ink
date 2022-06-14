-> Guest_36

=== Guest_36 ===
GUEST_WOMAN: Примите заказ.
THOMAS: Да, конечно, чего желаете?
GUEST_WOMAN: Одну <alc_MARGARITA>Маргариту</alc>, пожалуйста.
-> Order
 
=== Order ===
#check
* THOMAS: Будет сделано.
  #order
  THOMAS: Ваш заказ готов. 
  -> Grade
* THOMAS: Посмею вам предложить заместо нее <alc_ANYTHING></alc>.
  GUEST_WOMAN: Мне так хотелось именно Маргариты...
  THOMAS: Не отказывайтесь от моего предложения, думаю вам понравится.
  #guestchoice
  ** GUEST_WOMAN: Так и быть, вы меня убедили.
     #order
     THOMAS: Прошу, ваш напиток. 
     -> Grade  
  ** GUEST_WOMAN: Не сомневаваюсь. Но лучше все-таки Маргариту.
     THOMAS: Ваше желание - закон.
     #order
     THOMAS: Прошу, ваш напиток. 
     -> Grade
* THOMAS: К сожалению, я вынужден отказать в вам. Может что-то другое? Например, <alc_ANYTHING></alc>?
  GUEST_WOMAN: Мне так хотелось именно Маргариты...
  THOMAS: Не отказывайтесь от моего предложения, думаю вам понравится.
  #guestchoice
  ** GUEST_WOMAN: Так и быть, вы меня убедили.
     #order
     THOMAS: Прошу, ваш напиток. 
     -> Grade  
  ** GUEST_WOMAN: Не сомневаваюсь. Но лучше я пойду. -> END

=== Grade ===
#grade
* GUEST_WOMAN: Вы меня сильно удивили. Буду заходить почаще.
* GUEST_WOMAN: Все хорошо...Но-чего то не хватает.
* GUEST_WOMAN: Увы, вы довольно посредственно.
- -> END