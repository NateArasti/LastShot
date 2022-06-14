-> Guest_35

=== Guest_35 ===
THOMAS: Добый день.
GUEST_WOMAN: Вы такой приветливый.
THOMAS: Просто в хорошем расположении духа... Что я могу предложить прекрасной леди?
GUEST_WOMAN: Я бы хотела попробовать <alc_VIVI>Ви-ви</alc>...
-> Order
 
=== Order ===
#check
* THOMAS: Для вас все самое лучшее.
  GUEST_WOMAN: Потрясающе...
  #order
  THOMAS: А вот и заказ. 
  -> Grade
* THOMAS: Прекрасный выбор. Но я бы вам предложил попробовать <alc_ANYTHING></alc>.
  #guestchoice
  ** GUEST_WOMAN: Заинтриговал. Доверюсь твоему предложению.
     #order
     THOMAS: А вот и заказ.  
     -> Grade 
  ** GUEST_WOMAN: Я, пожалуй, откажусь.
     THOMAS: Многое теряете.
     #order
     THOMAS: А вот и заказ.  
     -> Grade
* THOMAS: Не хотел бы вас расстраивать, но не могу приготовить. Вы можете попробовать <alc_ANYTHING></alc>.
  THOMAS: Обещаю, вы не разочаруетесь...
  #guestchoice
  ** GUEST_WOMAN: Заинтриговал. Доверюсь твоему предложению.
     #order
     THOMAS: А вот и заказ.  
     -> Grade 
  ** GUEST_WOMAN: Я, пожалуй, откажусь. Не стоило приходить. -> END

=== Grade ===
#grade
* GUEST_WOMAN: А вы и вправду прекрасный бармен.
* GUEST_WOMAN: Довольно специфичный вкус.
* GUEST_WOMAN: Какое разочарование.
- -> END