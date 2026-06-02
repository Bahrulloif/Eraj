# MaxShop — Контекст проекта

## Что это
ASP.NET Core 7 Web API — маркетплейс (интернет-магазин). Архитектура: Clean Architecture с тремя слоями.

## Структура проекта
```
Eraj/
├── Domain/          # Сущности (Entities), DTO, интерфейсы
├── Infrastructure/  # Реализация сервисов, EF Core, AutoMapper, миграции
└── WebApi/          # Controllers, Program.cs, конфигурация
```
Solution файл: `MaxShop.sln`

## Технологии
- .NET 7 (`net7.0`)
- PostgreSQL + Entity Framework Core (DbContext: `DataContext`)
- ASP.NET Core Identity (`ApplicationUser`, `Roles`)
- JWT аутентификация (Bearer token, без валидации Issuer/Audience)
- AutoMapper (`ServiceProfile`)
- Swagger (только в Development)

## Домены / модули
| Категория | Сущности |
|-----------|----------|
| Пользователи | `ApplicationUser`, `ProfileUser`, `Roles` |
| Каталог | `Catalog`, `Category`, `SubCategory` |
| Товары — Техника | `NoteBook`, `SmartPhone`, `Tablet`, `SpareAccessorKomp` |
| Товары — Транспорт | `Car`, `Motorbike`, `Truck`, `SpareAccessorTransp` |
| Недвижимость | `Apartment`, `CommercialRealEstate`, `Cottage` |
| Заказы | `Order`, `Cart`, `DeliveryAddress`, `Address` |
| Прочее | `Picture`, `RatingAndTop` |

## Паттерн сервисов
Каждый модуль имеет пару файлов:
- `IXxxService.cs` — интерфейс (в Infrastructure/Services/)
- `XxxService.cs` — реализация

Все сервисы регистрируются как `AddScoped` в `WebApi/ExtentionsMethods/AddServices/AddServices.cs`.

## Запуск
```bash
dotnet run --project WebApi
```
Swagger UI доступен по `/swagger` в режиме Development.

## База данных
- СУБД: PostgreSQL
- Строка подключения: `DefaultConnection` в `appsettings.json`
- Миграции: `Infrastructure/Migrations/`
- Применить миграции: `dotnet ef database update --project Infrastructure --startup-project WebApi`

## Аутентификация
- JWT Bearer токен
- Минимальные требования к паролю: длина 4 символа, без обязательных цифр/спецсимволов
- При старте приложения автоматически создаются роли и SuperAdmin (через `Seeds`)

## Git
- Remote: `git@github.com:Bahrulloif/Eraj.git`
- Ветка: `main`

## Роли пользователей
- Публичная регистрация даёт роль `User`
- Роли назначает `SuperAdmin` через `RoleService`
- `SuperAdmin` создаётся автоматически при старте: логин `SuperAdmin`, пароль `Maxshop123`
- Доступные роли: `SuperAdmin`, `Admin`, `Marketing`, `User`, `Businessman`, `Courier`

## Статус багов
Все известные баги исправлены в коммитах на ветке `main` (2026-06-02). Сервисы проверены:
`OrderService`, `CartService`, `AccountService`, `TruckService`, `TabletService`,
`MotorbikeService`, `CarService`, `SmartPhoneService`, `ProfileService`,
`SubCategoryService`, `DeliveryAddressService`, `CategoryService`, `CatalogService`, `NoteBookService`.