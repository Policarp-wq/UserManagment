# UserManagment
Проект реализует REST API для регистрации, аутентификации и администрирования пользователей с поддержкой JWT-авторизации

**Основные возможности**
-	Регистрация и аутентификация пользователей (JWT)
-	CRUD-операции над пользователями (создание, обновление, удаление, восстановление)
-	Разделение ролей (администратор/пользователь)
-	Валидация данных (логин, пароль, имя)
-	Хранение данных в базе данных SQLite (in-memory)
  
**Структура**
-	Controllers — обработка HTTP-запросов
-	Services — бизнес-логика и работа с текущим пользователем
-	Repositories — доступ к данным пользователей
-	Utility — вспомогательные классы
-	Models/ApiContracts — модели данных и DTO

Также реализован механизм глобальной обработки исключений для удобства возвращения кодов с ошибками
Валидация происходит автоматически внутри эндпоинтов. Логика валидации описана в самих классах. Все временные значения указаны в UTC

В планах: вынести валдиацию роли в атрибут в сервисе, проверку на то, что пользователь неактивен, можно сделать быстрее, если кешировать таких пользователей в условном Redis
