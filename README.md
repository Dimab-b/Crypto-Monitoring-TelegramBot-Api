# Crypto Monitoring API & Telegram Bot
# Tech Stack

## Backend
- ASP.NET Core Web API
- C#
- REST API

## Architecture
- Onion Architecture
- CQRS
- MediatR

## Background Jobs
- Hangfire

## Cache
- Redis

## Resilience
- Polly

## External APIs
- Telegram Bot API
- Crypto API

## DevOps
- Docker
- Docker Compose

Це бекенд-сервіс для автоматизованого відстеження криптовалют. Система побудована з акцентом на модульність, масштабованість та стабільність фонових процесів, що дозволяє легко інтегрувати її у власні проєкти.

---

# Архітектура та стек

### Runtime
- .NET 10

### Архітектурний підхід
- Onion Architecture
- Чітке розділення на шари:
  - Domain (моделі)
  - Application (бізнес-логіка, команди)
  - Infrastructure (БД, зовнішні API)
  - API (контролери)

### CQRS & MediatR
Всі запити та команди обробляються через паттерн CQRS за допомогою MediatR. Це дозволяє розділити операції читання та запису, зробити код більш чистим, тестованим та простішим у підтримці.

### Background Processing
- Hangfire
- Планування та виконання повторюваних задач за CRON-виразами.

### Caching
- Redis
- Використовується як розподілений кеш для даних та як сховище стану Hangfire.

### HTTP Resilience
- Polly
- Retry Policy
- Circuit Breaker

### Communication
- Telegram Bot API

### Containerization
- Docker
- Docker Compose

---

# Суть роботи

Система отримує дані про вартість криптовалют, аналізує їх згідно із заданими параметрами та, у разі спрацювання тригера, формує повідомлення, яке через Telegram-бота відправляється в обраний чат.

Фонові процеси, що керуються Hangfire, забезпечують регулярне опитування ринку незалежно від HTTP-запитів до API.

---

# Запуск проєкту

Для запуску достатньо встановленого Docker.

## 1. Клонування репозиторію

```bash
git clone -b main https://github.com/Dimab-b/Crypto-Monitoring-TelegramBot-Api.git
cd Crypto-Monitoring-TelegramBot-Api
```

## 2. Створіть `.env`

На основі `.env.example` заповніть:

```
TelegramBot__Token=
ChatId=
CryptoApiSettings__CryptoApiKey=
```

## 3. Налаштуйте `appsettings.json`

Заповніть необхідні параметри конфігурації.

## 4. Запуск

```bash
docker-compose up -d --build
```

Після запуску Docker автоматично:

- створить мережу контейнерів;
- запустить API;
- запустить Redis.

---

# Доступні сервіси

### Swagger UI

```
http://localhost:5000/swagger
```

### Hangfire Dashboard

```
http://localhost:5000/hangfire
```

---

# Зупинка

```bash
docker-compose down
```

---

# Використання

Сервіс можна використовувати як автономний інструмент для автоматизації моніторингу криптовалют.

Для цього достатньо:

1. Створити власного Telegram-бота через **BotFather**.
2. Отримати:
   - Bot Token
   - Chat ID
3. Заповнити їх у `.env`.

Після запуску ви отримаєте повністю готовий сервіс моніторингу, який можна використовувати для власних задач.

Проєкт буде особливо корисним для трейдерів, які торгують криптовалютними ф'ючерсами та хочуть оперативно отримувати повідомлення про зміну ціни активів.

---

# Приклад роботи

## Telegram Bot

<p align="center">
    <img src="images/telegram-bot.png" width="500">
</p>

---

## Swagger UI

<p align="center">
    <img src="images/swagger.png" width="900">
</p>
