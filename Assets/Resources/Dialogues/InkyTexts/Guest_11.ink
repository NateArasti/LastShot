-> Guest_11

=== Guest_11 ===
GUEST_MAN: Хэй, хозяин! Налей мне что-нибудь выпить..
THOMAS: Что будешь? 
THOMAS: Виски? Ром? Текила?
GUEST_MAN: Ты за кого меня принимаешь? Я простой работящий человек.
THOMAS: И?
GUEST_MAN: Пиво нальёшь? Или, хотя, может у тебя есть эль? 
THOMAS: Сейчас посмотрим...
-> Order

=== Order ===
THOMAS: <alc_ANYTHING></alc> будешь?
THOMAS: <alc_ANYTHING></alc> будешь?
  #guestchoice
  * GUEST_MAN: Наливай!
     #order
     THOMAS: Держи. 
     -> Grade
  * GUEST_MAN: Я похож на богатого джентельмена по твоему? Не неси чепухи!
    THOMAS: В таком случае, может <alc_ANYTHING></alc>?
    THOMAS: В таком случае, может <alc_ANYTHING></alc>?
    #guestchoice
    **GUEST_MAN: Ну это уже получше.
      #order
      THOMAS: Держи. 
      -> Grade
    **GUEST_MAN: Нахер я сюда вообще зашел!? Такие цены... -> END

=== Grade ===
#grade
    * GUEST_MAN: Вы такой душевный хозяин...
      THOMAS: Наслаждайся выпивкой.
    * GUEST_MAN: Здесь вполне неплохо... Может буду чаще сюда заходить.
    * GUEST_MAN: Скотина! Всем нормально подаешь, а мне какие-т помои!? Ты ещё об этом пожалеешь...
- -> END 