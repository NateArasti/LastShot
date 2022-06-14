-> Guest_7

===Guest_7===
GUEST_MAN: Как же трещит башка... Налей мне <alc_BLOODYMARY>Кровавую Мери</alc>.
-> order1

 ===order1
#check
*THOMAS: Пять минут.
 #order
 THOMAS: Вот и ваш напиток.
 -> grade1
*THOMAS: Может хотите <alc_ANYTHING></alc>?
  #guestchoice
  **GUEST_MAN: Плевать. Наливай.
  #order
  THOMAS: Вот и ваш напиток.
  -> grade1
  **GUEST_MAN: Давай без креатива.
   THOMAS: Хорошо.
   #order
   THOMAS: Вот и ваш напиток.
   -> grade1
*THOMAS: Увы, нет на складе. Может <alc_ANYTHING></alc>?
  #guestchoice
  **GUEST_MAN: Плевать. Наливай.
  #order
  THOMAS: Вот и ваш напиток.
  -> grade1
  **GUEST_MAN: Испортился салун... -> END


===grade1
#grade
 *GUEST_MAN: Для меня самое то!
 -> story
 *GUEST_MAN: Для меня сойдет.
 -> story
 *GUEST_MAN: Ты уверен, что ты бармен?
 -> story
 
=== story
GUEST_MAN: А ты новенький в этом городе?
GUEST_MAN: Я не припомню тебя здесь раньше...
* THOMAS: Я полжизни здесь провёл.
    GUEST_MAN: Точно-точно! Это же тебе салун от Джона перешёл...
* THOMAS: Да так... Судьба занесла сюда.
    GUEST_MAN: Судьба просто так не заносит в наши края. Зачем приехал?
  ** Решил вернуться к тому, с чего начал.
    GUEST_MAN: Ладно... Не валяй дурака! Не хочешь - не отвечай.  
  ** Друг завещал мне салун этот. Вот, пришлось вернуться.
    GUEST_MAN: Точно-точно! Это же тебе салун от Джона перешёл.
    GUEST_MAN: Как я сразу не понял...
* THOMAS: Решил вернуться к тому, с чего начал.
    GUEST_MAN: Ладно... Не валяй дурака! Не хочешь - не отвечай.
- THOMAS: Чем ты здесь занимаешься?
GUEST_MAN: Шахта... Чертова шахта.
GUEST_MAN: Чем ещё в этом городе можно заниматься!?
GUEST_MAN: Налей-ка мне ещё чего.
-> order2

===order2
THOMAS: <alc_ANYTHING></alc> подойдет?
  #guestchoice
  *GUEST_MAN: Ладно, давай этого.
  #order
  THOMAS: Ваш заказ.
  -> grade2
  *GUEST_MAN: Покрече не найдется?
    THOMAS: <alc_ANYTHING></alc>?
    #guestchoice
    **GUEST_MAN: Ладно, давай этого...
      #order
      THOMAS: Ваш заказ.
      -> grade2
    **GUEST_MAN: Ой, не, что-то совсем не то. Думаю мне на сегодня хватит. -> END

===grade2
#grade
 *GUEST_MAN: На этот раз намного лучше.
 *GUEST_MAN: Более менее.
 *GUEST_MAN: Ты точно уверен, что ты бармен?
--> END
