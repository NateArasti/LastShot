-> Guest_5

===Guest_5===
GUEST_MAN: Дружище, налей мне <alc_ROM>рома</alc>. -> order

===order
#check
* THOMAS: Сейчас будет.
  #order
  THOMAS: Ваш заказ готов.
  -> grade
*THOMAS: Не желаете попробовать <alc_ANYTHING></alc>?
  #guestchoice
  **GUEST_MAN: Удиви.
  #order
  THOMAS: Держите.
  -> grade
  **GUEST_MAN: Не. Я только из-за рома пришел.
   THOMAS: Хорошо.
   #order
   THOMAS: Держите.
   -> grade
* THOMAS: Извините, он закончился. Может <alc_ANYTHING></alc>?
    THOMAS: Извините, он закончился. Может <alc_ANYTHING></alc>?
  #guestchoice
  **GUEST_MAN: Ну давай попробуем.
  #order
  THOMAS: Лови.
  -> grade
  **GUEST_MAN: Серьёзно? У вас нет <alc_ROM>рома</alc>? Боже, что за заведение...
   THOMAS: Мои извинения. -> END

===grade
#grade 
*GUEST_MAN: Давно я не пил такого...
-> story
*GUEST_MAN: А в памяти все казалось вкуснее...
-> story
*GUEST_MAN: Ты уверен, что это алкоголь?!
-> story

 ===story
GUEST_MAN: До войны у меня была своя плантация в Луизиане...
GUEST_MAN: Мне постоянно поставляли ром с Кубы. Чистейший кубинскй ром...
THOMAS: Вы участвовали в войне?
GUEST_MAN: Я воевал за то, что по праву было моё!!! Чертовы северяне!
GUEST_MAN: Наверное, вы и не застали этой войны здесь, на Западе?
* THOMAS: Я зарабатывал на юге во время войны...
    GUEST_MAN: Наш человек!
    GUEST_MAN: Проклятые северяне забрали всё, что моя семья строила многие десятки лет!
    GUEST_MAN: В нашем владениии останавливался сам генерал-губернатор Луизианы!
    GUEST_MAN: А какие балы мы проводили! Эх... Потеряно счастливое время...
* THOMAS: Ну, война тоже меня коснулась...
    GUEST_MAN: А ты на чьей стороне?
    THOMAS: Ну... Я много лет держал бар в Новом Орлеане...
    GUEST_MAN: Наш человек! 
    GUEST_MAN: У меня забрали всё, что моя семья строила многие десятки лет!
    GUEST_MAN: В нашем владениии останавливался сам генерал-губернатор Луизианы!
    GUEST_MAN: А какие балы мы проводили! Эх... Потеряно счастливое время...
* THOMAS: Тут своих проблем хватает, и без войны
    GUEST_MAN: Какие у вас проблемы то? Одна пустошь!
    THOMAS: Именно. Пустошь.
    Томас: Здесь, где не ступала нога правительства, творится полное беззаконие...
    GUEST_MAN: Думаете, это вы страдаете от беззакония? Да к чёрту этот закон!
    GUEST_MAN: У меня забрали всё, что моя семья строила многие десятки лет!
    GUEST_MAN: В нашем владениии останавливался сам генерал-губернатор Луизианы!
    GUEST_MAN: А какие балы мы проводили! Эх... Потеряно счастливое время...
- THOMAS: Что занесло вас в эти края?
GUEST_MAN: А что мне мне делать на юге? Я потерял, всё что имел...
-> END
