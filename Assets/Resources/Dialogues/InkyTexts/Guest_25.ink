-> Guest_25

=== Guest_25 ===
THOMAS: Добро пожаловать!
GUEST_MAN: Давно я у вас не был...
THOMAS: Мы всегда рады возвращению наших гостей, даже столь запоздалых.
GUEST_MAN: С моего последнего визита только хозяин поменялся... Вам бы интерьер обновить хоть немного...
THOMAS: И чего же этому месту не хватает?
GUEST_MAN: Света... Тут нужно больше света — темно как в дыре...
THOMAS: Хмм... Я подумаю над этим...
GUEST_MAN: Можно мне <alc_JINBOUTIQUE>Джин Бутик</alc>?
-> Order

=== Order ===
#check
* THOMAS: Будет сделано.
  #order
  THOMAS: Возьмите. 
  -> Grade
* THOMAS: Может вы попробуете <alc_ANYTHING></alc>?
  #guestchoice:
  **GUEST_MAN: Наливай.
     #order
     THOMAS: Возьмите. 
     -> Grade
  **GUEST_MAN: Нет, спасибо...
    THOMAS: Как знаете.
    #order
    THOMAS: Возьмите. 
    -> Grade
* THOMAS: Извини, закончилось ингридиенты. 
  GUEST_MAN: Ну... Если есть, можно что-нибудь разливное, эля, там, или пива, например...
  THOMAS: Могу я вам предложить <alc_ANYTHING></alc>.
  #guestchoice:
  **GUEST_MAN: Наливай.
     #order
     THOMAS: Возьмите. 
     -> Grade
  **GUEST_MAN: Нет, спасибо... Пожалуй я пойду отсюда... -> DONE

=== Grade
#grade
    * GUEST_MAN: Всё круто, спасибо, мужик. 
    * GUEST_MAN: Неплохая выпивка у тебя, мужик. 
    * GUEST_MAN: Ха, время идёт, а ничего в этом месте не меняется. Думаю, я не скоро сюда вернусь...
- -> END