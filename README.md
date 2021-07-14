# Добро на страницу проекта KShop!

**KShop** - пет-проект интернет-магазина, с фичами работы витриной, корзиной и заказами.

Основной целью проекта является использование микросервисного подхода в реализации **backend-а** с решением соответствующих проблем.

Проект не отполирован и не претендует на сборник best-practice, тем не менее, планируется пробовать и добавлять полезные/интересные решения и подходы.


## Состав репозитория

В репозитории находится backend и frontend.

 - `docker/`
	 - Файлы для разворачивания проекта в Docker
 - `docs/`
	 - Документные файлы, содержат схемы и  текстовые заметки
 - `frondend/`
	 - Single-App клиент написанный на typescript+react
 - `scripts/`
	 - Bash-скрипты 
 - `src/`
	 - Backend код на .net 5


# Запуск проекта

### Развертка окружения в Docker
Для развёртки используется docker-compose, располагаемый в папке `docker/`.
Для работы Docker рекомендуется выделить 3gb RAM.
```
docker-compose -f docker-compose.yml up -d
```
### Развертка сервисов в Docker
Занимает продолжительное время, что связано с выкачкой образов и nuget-пакетов.
```
docker-compose -f docker-compose-services.yml up -d
```
### Запуск сервисов локально
Используется Visual Studio 2019.
Локальное развертывание заключается в одновременном запуске WebApi-проектов через пункт меню `Set Startup Projects` в солюшене.

 - KShop.Carts.WebApi 
 - KShop.Identities.WebApi 
 - KShop.Orders.WebApi
 - KShop.Payments.WebApi 
 - KShop.Products.WebApi 
 - KShop.Shipments.WebApi

### Запуск клиента
`npm start` в папке проекта /frontend/kshop-frontend
