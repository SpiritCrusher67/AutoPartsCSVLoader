<h3>Описание</h3>
<p>Приложение после запуска мониторит входящие сообщения. Добавляет данные из csv файла в БД на SQL Server<p/>
<h3>Конфигурация</h3>
<p>Для конфигурации приложения используется файл appsettings.json</p>
<ul>
<li>Список Email адресов поставщиков конфигурируется в секции "SuppliersEmails"
<li>Конфигурации сопоставления столбцов находятся в секции "SuppliersColumnsConfiguration"
<li>Аутентификационные данные находятся в "MailAuthenticationData"
<li>Данные для подключения к почтовому серверу в "MailConfiguration"
<li>Интервал проверок конфигурируется в "InboxCheckingIntervalInSeconds"
<ul/>
