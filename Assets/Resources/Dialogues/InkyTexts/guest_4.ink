-> Guest_4

===Guest_4===
GUEST_WOMAN: Налей мне что-нибудь крепкого и покислее.
-> order1

===order1
THOMAS: <alc_ANYTHING></alc>?
  #guestchoice
  *GUEST_WOMAN: Давай...
   #order
   THOMAS: Вот, пожалуйста.
   -> grade1
  *GUEST_WOMAN: Какой-то отстой. Есть еще что?
    THOMAS: <alc_ANYTHING></alc>.
    #guestchoice
    **GUEST_WOMAN: Подойдет.
      #order
      THOMAS: Готово.
      -> grade1
    **GUEST_WOMAN: Что за хрень ты мне предлагаешь. -> END
    
===grade1
#grade
 *GUEST_WOMAN: А ничего так.
 -> story
 *GUEST_WOMAN: Ну и вкус.
 -> story
 *GUEST_WOMAN: Может у вас продукты просрочены? 
 -> story

===story
GUEST_WOMAN: Вот скажи мне, я красивая? 
* THOMAS: Вы прекрасны.
    GUEST_WOMAN: А вот козёл Боб так больше не считает!
    GUEST_WOMANGUEST_WOMAN: Что он нашёл в этой дуре, Хельге?
    GUEST_WOMAN: Она страшная, как стиральная доска!
    GUEST_WOMAN: У меня есть формы, у меня ум, я во всем лучше, чем она.
    GUEST_WOMAN: Разве не так?
    ** THOMAS: Никогда не видел Хельгу, но думаю, Боб полный дурак.
        GUEST_WOMAN: Да! Дурак!
        GUEST_WOMAN: Он ещё будет сожалеть, что потерял меня.
        GUEST_WOMAN: Дурнушка Хельга... Она... Её... у неё весь город побывал!
        GUEST_WOMAN: Она по популярности ничем не уступает этому салуну...
    ** THOMAS: Нет, это не так.
        GUEST_WOMAN: Вы нахал!!!
        GUEST_WOMAN: Грубая скотина!
        GUEST_WOMAN: Все вы, мужланы, одинаковые в этом городе!
        GUEST_WOMAN: ...уходит...
        -> END
    ** THOMAS: Вы выглядите глупо!
        GUEST_WOMAN: Глупым выглядишь ты!
        GUEST_WOMAN: Жалкий старик за стойкой бара в хлеву!
        GUEST_WOMAN: Все вы, мужланы, одинаковые в этом городе!
        GUEST_WOMAN: ...уходит...
        -> END
    -- Томас: Я смотрю, в этом городе вы нескучно живёте!
* THOMAS: Скромности у вас не занимать...
    GUEST_WOMAN: Скромность — удел слабых.
    GUEST_WOMAN: Во мне есть всё, что нужно настоящему мужчине.
    GUEST_WOMAN: У меня прекрасные формы, блестящий ум.
    GUEST_WOMAN: Только недоумок Боб не смог этого понять.
    GUEST_WOMAN: Ну, он ещё пожалеет, что связался с этой Хельгой — стиральной доской!
    ** Никогда не видел Хельгу, но думаю, Боб полный дурак.
        GUEST_WOMAN: Да! Дурак!
        GUEST_WOMAN: Он ещё будет сожалеть, что потерял меня.
        GUEST_WOMAN: Дурнушка Хельга... Она... Её... у неё весь город побывал!
        GUEST_WOMAN: Она по популярности ничем не уступает этому салуну...
    ** Нет, не пожалеет.
        GUEST_WOMAN: Вы нахал!!!
        GUEST_WOMAN: Грубая скотина!
        GUEST_WOMAN: Все вы, мужланы, одинаковые в этом городе!
        GUEST_WOMAN: ...уходит...
        -> END
    ** Вы выглядите глупо!
        GUEST_WOMAN: Глупым выглядишь ты!
        GUEST_WOMAN: Жалкий старик за стойкой бара в хлеву!
        GUEST_WOMAN: Все вы, мужланы, одинаковые в этом городе!
        GUEST_WOMAN: ...уходит...
        -> END
    -- Томас: Я смотрю, в этом городе вы нескучно живёте!
* THOMAS: С какой целью вы спрашиваете?
    GUEST_WOMAN: Я уверена в своём совершенстве.
    GUEST_WOMAN: Но не каждому мужчине дано это понять.
    GUEST_WOMAN: Особенно недоумку Бобу... Пусть и дальше развлекается со своей Хельгой...
    ** THOMAS: Никогда не видел Хельгу, но думаю, Боб полный дурак.
        GUEST_WOMAN: Да! Дурак!
        GUEST_WOMAN: Он ещё будет сожалеть, что потерял меня.
        GUEST_WOMAN: Дурнушка Хельга... Она... Её... у неё весь город побывал!
        GUEST_WOMAN: Она по популярности ничем не уступает этому салуну...
    ** THOMAS: Нет, это не так.
        GUEST_WOMAN: Вы нахал!!!
        GUEST_WOMAN: Грубая скотина!
        GUEST_WOMAN: Все вы, мужланы, одинаковые в этом городе!
        GUEST_WOMAN: ...уходит...
        -> END
    ** THOMAS: Вы выглядите глупо!
        GUEST_WOMAN: Глупым выглядишь ты!
        GUEST_WOMAN: Жалкий старик за стойкой бара в хлеву!
        GUEST_WOMAN: Все вы, мужланы, одинаковые в этом городе!
        GUEST_WOMAN: ...уходит...
        -> END
    -- THOMAS: Я смотрю, в этом городе вы нескучно живёте!
- THOMAS: Может быть, ваша судьба найдётся сегодня в этом зале?
GUEST_WOMAN: Не неси чушь! В этом сборище пьянчуг? 
GUEST_WOMAN: Налей мне еще чего-нибудь...
-> order2

===order2
THOMAS: Что думаете по поводу <alc_ANYTHING></alc>?
  #guestchoice
  *GUEST_WOMAN: Да мне всё равно, наливай уже...
  #order
  THOMAS: Ваш напиток.
  -> grade2
  *GUEST_WOMAN: Не хочу. Есть еще что?
    THOMAS: Вам может понравится <alc_ANYTHING></alc>.
    #guestchoice
    **GUEST_WOMAN: Уже лучше.
      #order
      THOMAS: Ваш напиток.
      -> grade2
    **GUEST_WOMAN: Да к черту. -> END
    
===grade2
#grade
 *GUEST_WOMAN: Этот намного лучше. 
 *GUEST_WOMAN: Боже...Сойдет. 
 *GUEST_WOMAN: Ты точно здесь работаешь?
--> END