-> Guest_16

=== Guest_16 ===
GUEST_WOMAN: Срочно! Налей мне что-нибудь!
THOMAS: К чему такая спешка?
GUEST_WOMAN: Меня отсюда заберут через четверть часа!
THOMAS: Хорошо. Есть время на приготовление коктейля?
GUEST_WOMAN: Нет!
GUEST_WOMAN: Нет, просто плесни чего-нибудь...
THOMAS: Разливного?
GUEST_WOMAN: Я же говорю, у меня нет времени на бокал-другой, просто чего крепкого!
-> Order

=== Order ===
THOMAS: Я могу вам предложить... <alc_ANYTHING></alc>?
THOMAS: Я могу вам предложить... <alc_ANYTHING></alc>?
  #guestchoice
  * GUEST_WOMAN: Да, да, самое то.
     #order
     THOMAS: Держите, мисс.
     GUEST_WOMAN: Ха! Действительно, пока ещё мисс.
     THOMAS: Выходите замуж? Поздравляю...
     GUEST_WOMAN: Пусть идёт на хрен это свиное рыло!!! Я никогда ему не достанусь!!!
     THOMAS: Ммм... Ладно? 
     -> Grade
  * GUEST_WOMAN: Да ты издеваешься надо мной?
    THOMAS: <alc_ANYTHING></alc>?
    THOMAS: <alc_ANYTHING></alc>?
    #guestchoice
    **GUEST_WOMAN: Да, да, самое то.
      #order
      THOMAS: Держите, мисс.
      GUEST_WOMAN: Ха! Действительно, пока ещё мисс.
      THOMAS: Выходите замуж? Поздравляю...
      GUEST_WOMAN: Пусть идёт на хрен это свиное рыло! Я никогда ему не достанусь!
      THOMAS: Ммм... Ладно? 
      -> Grade
    **GUEST_WOMAN: Подожду-ка я лучше снаружи... Козёл. -> END
    
=== Grade ===
#grade
* GUEST_WOMAN: Кстати, хорошое у тебя пойло. 
-> story
* GUEST_WOMAN: Кстати, пойло неплохое, сойдёт. 
-> story
* GUEST_WOMAN: Как вы эту дрянь пьёте? Вообще же не берёт! 
-> story

===story
GUEST_WOMAN: Меня уже должна ждать повозка за дверью салуна. 
THOMAS: Куда едет повозка?
GUEST_WOMAN: На станцию.Поезд на восток... Бегу от этого мужлана.
GUEST_WOMAN: Мне бы не опоздать на станцию.
THOMAS: Если ты намерена сбегать, тебе следует меньше трепаться языком и меньше опракидывать стаканы.
THOMAS: Не самое лучшее решение сбегать пьяной.
GUEST_WOMAN: Ты прав. Мне, ещё понадобится трезвый ум.
GUEST_WOMAN: Спасибо, красавчик... Я сваливаю в новую жизнь! К чертям эту пустошь.
THOMAS: Удачи!
-> END