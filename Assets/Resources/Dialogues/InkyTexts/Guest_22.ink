-> Guest_22

=== Guest_22 ===
THOMAS: Добро пожаловать.
GUEST_MAN: И тебе не болеть, сынок...
GUEST_MAN: Можешь мне что-нибудь налить до следующего рейса?
THOMAS: Рейса? Вы с железной дороги что-ли?
GUEST_MAN: Ну да, а по форме не понятно? Парниш... У меня был тяжелый рейс, и не менее тяжелый предстоит назад...
GUEST_MAN: Просто предложи мне что-нибудь выпить, и мы чудесно проведём время...
THOMAS: Не знал, что машинистам разрешается выпивать перед рейсом.
GUEST_MAN: Ах-ха-ха-ха...
GUEST_MAN: А кто сказал, что разрешается? 
GUEST_MAN: Мы просто никому об этом не скажем...
THOMAS: Ха! Как скажете...
THOMAS: Что вы желаете? 
GUEST_MAN: У тебя есть <alc_WHISKEY>виски</alc>?
-> Order

=== Order ===
#check
*THOMAS: Для наших машинистов виски всегда найдётся.
  GUEST_MAN: Отлично, сынок...
  #order
  THOMAS: Держите. 
  -> Grade
*THOMAS: Могу я предложить вам что-нибудь на свой вкус? Может вам понравится <alc_ANYTHING></alc>?
  #guestchoice
  **GUEST_MAN: Валяй, сынок...
    #order
    THOMAS: Держите. 
    -> Grade
  **GUEST_MAN: Да не, к черту!
    GUEST_MAN: Раздраженный машинист — залог нескольких сбитых коров.
    THOMAS: Извините, сейчас налью вам виски.
    #order
    THOMAS: Держите. 
    -> Grade
*THOMAS: Нет, к сожалению, нет...
  THOMAS: Могу я предложить вам что-нибудь на свой вкус? Может вам понравится <alc_ANYTHING></alc>?
  THOMAS: Могу я предложить вам что-нибудь на свой вкус? Может вам понравится <alc_ANYTHING></alc>?
  #guestchoice
  **GUEST_MAN: Валяй, сынок...
    #order
    THOMAS: Держите. 
    -> Grade
  **GUEST_MAN: Да не, к черту!
    GUEST_MAN: Раздраженный машинист — залог нескольких сбитых коров.
    GUEST_MAN: Всего доброго! -> END

=== Grade
#grade
*GUEST_MAN: Чёрт возьми! С таким прекрасным настроением можно хоть до Восточного побережья поезд гнать...
*GUEST_MAN: А неплохо! Очень даже не плохо... Паровоз, поди, уже запривили, и я готов к рейсу... 
*GUEST_MAN: Зря только время потратил... 

- -> END