# Добро на страницу проекта KShop!

**KShop** - пет-проект с функционалом витрины, корзины и создания заказов.

Основной целью проекта является практика микросервисного подхода в реализации **backend-а** с решением соответствующих проблем.

Репозитории backend и frontend объеденены только для удобства представления.

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

## Развертка окружения в Docker
Для развёртки используется docker-compose, располагаемый в папке `docker/`.
Для работы Docker рекомендуется выделить 3gb RAM.
```
docker-compose -p kshop-env -f docker-compose.yml up -d
```
## Развертка сервисов
#### В Docker
Занимает продолжительное время, что связано с выкачкой образов и nuget-пакетов.
```
docker-compose -p kshop-svc -f docker-compose-services.yml up -d
```
#### Локально
Локальное развертывание заключается в одновременном запуске WebApi-проектов. 
В Visual Studio это настраивается через `Set Startup Projects` во всплывающем меню солюшена.

 - KShop.Carts.WebApi 
 - KShop.Identities.WebApi 
 - KShop.Orders.WebApi
 - KShop.Payments.WebApi 
 - KShop.Products.WebApi 
 - KShop.Shipments.WebApi

## Запуск клиента
`npm start` в папке проекта /frontend/kshop-frontend
