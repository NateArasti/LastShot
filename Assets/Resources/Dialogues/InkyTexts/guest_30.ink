-> Guest_30

=== Guest_30 ===
GUEST_MAN: И где его искать бля.
GUEST_MAN: Плесни мне <alc_LIGHTBEER>светлого пива</alc>.  
-> order

===order
#check
*THOMAS: Принято.
 #order
 THOMAS: А вот и ваш напиток. 
 -> grade
*THOMAS: У меня тут новый рецепт. Не хочешь попробовать <alc_ANYTHING></alc>?
  #guestchoice
  **GUEST_MAN: Терять нечего. Тащи.
  #order
  THOMAS: А вот и ваш напиток. 
  -> grade
  **GUEST_MAN: Мне только коньяк сможет помочь.
   THOMAS: Как скажете.
   #order
   THOMAS: А вот и ваш напиток. 
   -> grade
*THOMAS: Сейчас не в наличии. Попробуй <alc_ANYTHING></alc>.
THOMAS: Сейчас не в наличии. Попробуй <alc_ANYTHING></alc>.
  #guestchoice
  **GUEST_MAN: Терять нечего. Тащи.
  #order
  THOMAS: А вот и ваш напиток. 
  -> grade
  **GUEST_MAN: Воздержусь. -> END

===grade
#grade
 *GUEST_MAN: Да... Так намного лучше 
 -> story
 *GUEST_MAN: Для разогрева, сойдет. 
 -> story
 *GUEST_MAN: На вкус как помои. 
 -> story
 
 ===story
 GUEST_MAN: Слушай, к вам шериф не заходил сегодня?
 THOMAS: Не было пока. Зачем он тебе?
 GUEST_MAN: Да, кто-то коня моего оседлал и умчался. Я только пыли и успел глотнуть. Вот шериф и понадобился.
 GUEST_MAN: Сам понимаешь какое это преступение. Будто жену мою обесчестили. Найду, яйца ублюдку на голову натяну.
 THOMAS: Действительно, ужасное поступок. Я сообщу шерифу, если он появится.
 GUEST_MAN: Спасибо, мужик. Но пойду еще раз загляну к нему. Может все таки окажется на месте.
 THOMAS: Могу пожелать лишь удачи.
-> END