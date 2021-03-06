# Handling Managers Reports

## Общее описание задачи

Разработать приложение для преобразования информации о продажах товаров за день из CSV файла стандартного формата. Запущенное приложение отслеживает изменения в папке и, при появлении в ней новых файлов, выполняет их разбор и загрузку информации из них в базу данных. Файлы загружаются на сервер не менее одного раза в день. Имя файла состоит из фамилии менеджера и даты в следующем формате: SecondName_DDMMYYYY.csv, например Ivanov_19112012.csv

CSV файл имеет следующую структуру

Дата, Клиент, Товар, Сумма

Данные из этих столбцов должны быть загружены в базу. Структуру БД продумать и реализовать самостоятельно.

## Требования

- наличие клиентов двух видов: консольное приложение и служба Windows. В каждый момент времени может работать только один клиент.
- должна присутствовать возможность обработки двух и более файлов одновременно. Для этого можно использовать стандартные средства C# для работы с потоками (Threads, TPL).
- Реализовать механизм конкурентного доступа к БД (SELECT, INSERT, UPDATE) при обработке нескольких файлов одновременно.
- работа с базой данных должна происходить только с использованием Entity Framework, прямая работа с БД не допускается.
- модульность и N-уровневая архитектура приложения. Решение должно содержать сборки для: слоя доступа к данным, слоя бизнес-логики и слоев клиентов; реализация клиентов не должна приводить к изменению в предшествующих слоях.
- классы должны быть реализованы с использованием ООП.
- yправление ресурсами и использование интерфейса IDisposable (файлы, объекты контекстов и т.д. использование конфигурации AppConfig для хранения настроек приложения/службы

## Дополнительные требования

- использование исключений