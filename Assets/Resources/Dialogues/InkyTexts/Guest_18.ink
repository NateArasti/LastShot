-> Guest_18

=== Guest_18 ===
GUEST_MAN: Продаешь опиум?
THOMAS: Ты читать умеешь? На вывеске вроде бы "Салун" написано.
GUEST_MAN: Я тебе не в библиотеку пришёл, умник.
GUEST_MAN: Так продаёшь или нет?
* THOMAS: На выход. -> END
* THOMAS: Здесь только алкоголь.
  GUEST_MAN: Чёрт, да не сдалось мне твоё пойло!!!
   ** THOMAS: На выход. -> END
   ** THOMAS: Или заказывай выпивку, или иди, шныряйся по улице, пока сам тебя не выкинул... 
   GUEST_MAN: Ладно, ладно. Есть что-то поострее?
   GUEST_MAN: Желательно <alc_BLOODYMARY>Кровавую Мери</alc>. 
   -> Order
   
=== Order ===
#check
* THOMAS: Найдется. Одну минуту.
  #order
  THOMAS: Твой заказ. 
  -> Grade
* THOMAS: Из крепкого могу предложить <alc_ANYTHING></alc>.
  #guestchoice
  ** GUEST_MAN: Да плевать уже... Наливай.
     #order
     THOMAS: Твой заказ. 
     -> Grade
  ** GUEST_MAN: Нафиг надо, я вообще сюда не за этим приходил...
     THOMAS: Твое дело.
     #order
     THOMAS: Твой заказ. 
     -> Grade
* THOMAS: Не могу, давай лучше <alc_ANYTHING></alc> налью? 
  #guestchoice
  ** GUEST_MAN: Да плевать уже... Наливай.
     #order
     THOMAS: Твой заказ. 
     -> Grade
  ** GUEST_MAN: Нафиг надо, я вообще сюда не за этим приходил...
     -> END

=== Grade ===
#grade
    * GUEST_MAN: Ого. Это превзошло мои ожидания.
    * GUEST_MAN: Сойдёт...
    * GUEST_MAN: Дерьмо твоя выпивка... Я больше сюда не вернусь.
- -> END
