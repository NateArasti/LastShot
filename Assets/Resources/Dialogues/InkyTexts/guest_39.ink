-> Guest_39
=== Guest_39 ===
GUEST_MAN: Д-добрый день.
THOMAS: Добрый.
GUEST_MAN: Мне, п-пожалуйста, <alc_WHISPER>Шёпот</alc>.  
-> order

===order
#check
*THOMAS: Хорошо.
 #order
 THOMAS: Для вас. 
 -> grade
*THOMAS: Может вам налить <alc_ANYTHING></alc>?
  #guestchoice
  **GUEST_MAN: Д-да, давайте. Мне лишь бы нервишки успокоить.
  #order
  THOMAS: Для вас. 
  -> grade
  **GUEST_MAN: Не д-думаю что мне это п-поможет.
   THOMAS: Хорошо.
   #order
   THOMAS: Для вас. 
   -> grade
*THOMAS: Нет уже ее. Можете попробовать <alc_ANYTHING></alc>.
THOMAS: Нет уже ее. Можете попробовать <alc_ANYTHING></alc>.
  #guestchoice
  **GUEST_MAN: Д-да, давайте. Мне лишь бы нервишки успокоить.
  #order
  THOMAS: Для вас. 
  -> grade
  **GUEST_MAN: Не д-думаю что мне это п-поможет.
   THOMAS: Тогда ничем не могу помочь. -> story

===grade
#grade
 *GUEST_MAN: Самое то. 
 -> story
 *GUEST_MAN: Хоть к-какое-то успокоительное. 
 -> story
 *GUEST_MAN: К-какой отвратный вкус...
 -> story
 
 ===story
 THOMAS: Что вы такой нервный то?
 GUEST_MAN: Да я собак боюсь. А за мной с самого утра у-увязалась одна.
 THOMAS: Так она на вас напала?
 GUEST_MAN: Нет. Выглядела мирной, б-белая вся, только пятно черное во лбу. Но детсую травму то не переубедить.
 THOMAS: Страхам нужно смотреть в глаза. Попробуйте ее погладить. Может и бояться перестанете.
 GUEST_MAN: Мудро звучит. Но я в-воздержусь. До встречи.
 THOMAS: Досвидания.
-> END