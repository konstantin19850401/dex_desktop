<?
// *************************************************************
// * Значения некоторых полей по умолчанию
// *************************************************************
$config_default_dbhost = 'localhost';
$config_default_dbport = 3306;
$config_default_dbuser = 'dex';
$config_default_dbpass = 'dex';

$config_default_dex_user = 'dex'; // Пользователь DEX по умолчанию

$config_default_session_timeout_sec = 600;                              // Тайм-аут сессии (в секундах)
$SESS_DIR = "sessions";                                                 // Каталог с сессиями

// *************************************************************
// * Конфигурация баз данных DEX, с которыми работает система
// *************************************************************
$config_db = array(

	// Конфигурация базы МегаФон
    'MEGA' => array(
        'title' => 'МегаФон',											// Наименование в списке БД клиента
        'engine' => 'mega efd',                                         // Ключи конфигурации (для обработчиков)
        'dbname' => 'dex_mega',											// Наименование базы данных на сервере
        'dbhost' => $config_default_dbhost,                             // Адрес сервера MySql
        'dbport' => $config_default_dbport,                             // Порт сервера MySql
        'dbuser' => $config_default_dbuser,                             // Пользователь БД
        'dbpass' => $config_default_dbpass,                             // Пароль БД
        'doctypes' => array(											// Список типов документов (модулей), хранящихся в базе
            'DEXPlugin.Document.Mega.EFD.Fiz'
        ),
        "dexuser" => $config_default_dex_user                           // Пользователь DEX, под которым DEXOL осуществляет вход в БД
    ),
	// Конфигурация базы Билайн СК
    'BEE_STS' => array(
        'title' => 'Билайн СТС',										// Наименование в списке БД клиента
        'engine' => 'beeline',                                         	// Ключи конфигурации (для обработчиков)
        'dbname' => 'dex_beeline_sts',									// Наименование базы данных на сервере
        'dbhost' => $config_default_dbhost,                             // Адрес сервера MySql
        'dbport' => $config_default_dbport,                             // Порт сервера MySql
        'dbuser' => $config_default_dbuser,                             // Пользователь БД
        'dbpass' => $config_default_dbpass,                             // Пароль БД
        'doctypes' => array(											// Список типов документов (модулей), хранящихся в базе
            'DEXPlugin.Document.Beeline.DOL2.Contract'
        ),
        "dexuser" => $config_default_dex_user                           // Пользователь DEX, под которым DEXOL осуществляет вход в БД
    ),
	// Конфигурация базы Билайн СК 2
    'BEE_STS2' => array(
        'title' => 'Билайн СТС 2',										// Наименование в списке БД клиента
        'engine' => 'beeline',                                         	// Ключи конфигурации (для обработчиков)
        'dbname' => 'dex_beeline_sts2',									// Наименование базы данных на сервере
        'dbhost' => $config_default_dbhost,                             // Адрес сервера MySql
        'dbport' => $config_default_dbport,                             // Порт сервера MySql
        'dbuser' => $config_default_dbuser,                             // Пользователь БД
        'dbpass' => $config_default_dbpass,                             // Пароль БД
        'doctypes' => array(											// Список типов документов (модулей), хранящихся в базе
            'DEXPlugin.Document.Beeline.DOL2.Contract'
        ),
        "dexuser" => $config_default_dex_user                           // Пользователь DEX, под которым DEXOL осуществляет вход в БД
    ),
	// Конфигурация базы Билайн СК БТС
    'BEE_STS_BTS' => array(
        'title' => 'Билайн СТС БТС',									// Наименование в списке БД клиента
        'engine' => 'beeline',                                         	// Ключи конфигурации (для обработчиков)
        'dbname' => 'dex_beeline_sts_bts',								// Наименование базы данных на сервере
        'dbhost' => $config_default_dbhost,                             // Адрес сервера MySql
        'dbport' => $config_default_dbport,                             // Порт сервера MySql
        'dbuser' => $config_default_dbuser,                             // Пользователь БД
        'dbpass' => $config_default_dbpass,                             // Пароль БД
        'doctypes' => array(											// Список типов документов (модулей), хранящихся в базе
            'DEXPlugin.Document.Beeline.DOL2.Contract'
        ),
        "dexuser" => $config_default_dex_user                           // Пользователь DEX, под которым DEXOL осуществляет вход в БД
    ),
	// Конфигурация базы Билайн КБР
    'BEE_KBR' => array(
        'title' => 'Билайн КБР',										// Наименование в списке БД клиента
        'engine' => 'beeline',                                         	// Ключи конфигурации (для обработчиков)
        'dbname' => 'dex_beeline_kbr',									// Наименование базы данных на сервере
        'dbhost' => $config_default_dbhost,                             // Адрес сервера MySql
        'dbport' => $config_default_dbport,                             // Порт сервера MySql
        'dbuser' => $config_default_dbuser,                             // Пользователь БД
        'dbpass' => $config_default_dbpass,                             // Пароль БД
        'doctypes' => array(											// Список типов документов (модулей), хранящихся в базе
            'DEXPlugin.Document.Beeline.DOL2.Contract'
        ),
        "dexuser" => $config_default_dex_user                           // Пользователь DEX, под которым DEXOL осуществляет вход в БД
    ),
	// Конфигурация базы Билайн КЧР
    'BEE_KCR' => array(
        'title' => 'Билайн КЧР',										// Наименование в списке БД клиента
        'engine' => 'beeline',                                         	// Ключи конфигурации (для обработчиков)
        'dbname' => 'dex_beeline_kcr',									// Наименование базы данных на сервере
        'dbhost' => $config_default_dbhost,                             // Адрес сервера MySql
        'dbport' => $config_default_dbport,                             // Порт сервера MySql
        'dbuser' => $config_default_dbuser,                             // Пользователь БД
        'dbpass' => $config_default_dbpass,                             // Пароль БД
        'doctypes' => array(											// Список типов документов (модулей), хранящихся в базе
            'DEXPlugin.Document.Beeline.DOL2.Contract'
        ),
        "dexuser" => $config_default_dex_user                           // Пользователь DEX, под которым DEXOL осуществляет вход в БД
    ),
	// Конфигурация базы Билайн КЧР 2
    'BEE_KCR2' => array(
        'title' => 'Билайн КЧР 2',										// Наименование в списке БД клиента
        'engine' => 'beeline',                                         	// Ключи конфигурации (для обработчиков)
        'dbname' => 'dex_beeline_kcr2',									// Наименование базы данных на сервере
        'dbhost' => $config_default_dbhost,                             // Адрес сервера MySql
        'dbport' => $config_default_dbport,                             // Порт сервера MySql
        'dbuser' => $config_default_dbuser,                             // Пользователь БД
        'dbpass' => $config_default_dbpass,                             // Пароль БД
        'doctypes' => array(											// Список типов документов (модулей), хранящихся в базе
            'DEXPlugin.Document.Beeline.DOL2.Contract'
        ),
        "dexuser" => $config_default_dex_user                           // Пользователь DEX, под которым DEXOL осуществляет вход в БД
    ),
	// Конфигурация базы Билайн КЧР Мобильный Офис
    'BEE_KCR_MOBI' => array(
        'title' => 'Билайн КЧР Мобильный Офис',							// Наименование в списке БД клиента
        'engine' => 'beeline',                                         	// Ключи конфигурации (для обработчиков)
        'dbname' => 'dex_beeline_kcr_mobi',								// Наименование базы данных на сервере
        'dbhost' => $config_default_dbhost,                             // Адрес сервера MySql
        'dbport' => $config_default_dbport,                             // Порт сервера MySql
        'dbuser' => $config_default_dbuser,                             // Пользователь БД
        'dbpass' => $config_default_dbpass,                             // Пароль БД
        'doctypes' => array(											// Список типов документов (модулей), хранящихся в базе
            'DEXPlugin.Document.Beeline.DOL2.Contract'
        ),
        "dexuser" => $config_default_dex_user                           // Пользователь DEX, под которым DEXOL осуществляет вход в БД
    ),
	// Конфигурация базы Билайн КЧР БМ
    'BEE_KCR_BM' => array(
        'title' => 'Билайн КЧР БМ',										// Наименование в списке БД клиента
        'engine' => 'beeline',                                         	// Ключи конфигурации (для обработчиков)
        'dbname' => 'dex_beeline_kcr_bm',									// Наименование базы данных на сервере
        'dbhost' => $config_default_dbhost,                             // Адрес сервера MySql
        'dbport' => $config_default_dbport,                             // Порт сервера MySql
        'dbuser' => $config_default_dbuser,                             // Пользователь БД
        'dbpass' => $config_default_dbpass,                             // Пароль БД
        'doctypes' => array(											// Список типов документов (модулей), хранящихся в базе
            'DEXPlugin.Document.Beeline.DOL2.Contract'
        ),
        "dexuser" => $config_default_dex_user                           // Пользователь DEX, под которым DEXOL осуществляет вход в БД
    ),
	// Конфигурация базы Билайн РД
    'BEE_RD' => array(
        'title' => 'Билайн РД',											// Наименование в списке БД клиента
        'engine' => 'beeline',                                         	// Ключи конфигурации (для обработчиков)
        'dbname' => 'dex_beeline_rd',									// Наименование базы данных на сервере
        'dbhost' => $config_default_dbhost,                             // Адрес сервера MySql
        'dbport' => $config_default_dbport,                             // Порт сервера MySql
        'dbuser' => $config_default_dbuser,                             // Пользователь БД
        'dbpass' => $config_default_dbpass,                             // Пароль БД
        'doctypes' => array(											// Список типов документов (модулей), хранящихся в базе
            'DEXPlugin.Document.Beeline.DOL2.Contract'
        ),
        "dexuser" => $config_default_dex_user                           // Пользователь DEX, под которым DEXOL осуществляет вход в БД
    ),
	'MTS_STS' => array(
        'title' => 'МТС СК',											// Наименование в списке БД клиента
        'engine' => 'mts',                                         		// Ключи конфигурации (для обработчиков)
        'dbname' => 'dex_mts_sts',										// Наименование базы данных на сервере
        'dbhost' => $config_default_dbhost,                             // Адрес сервера MySql
        'dbport' => $config_default_dbport,                             // Порт сервера MySql
        'dbuser' => $config_default_dbuser,                             // Пользователь БД
        'dbpass' => $config_default_dbpass,                             // Пароль БД
        'doctypes' => array(											// Список типов документов (модулей), хранящихся в базе
            'DEXPlugin.Document.MTS.Jeans'
        ),
        "dexuser" => $config_default_dex_user                           // Пользователь DEX, под которым DEXOL осуществляет вход в БД
	),
	'MTS_STS_BTS' => array(
        'title' => 'МТС СК БТС',										// Наименование в списке БД клиента
        'engine' => 'mts',                                         		// Ключи конфигурации (для обработчиков)
        'dbname' => 'dex_mts_sts_bts',									// Наименование базы данных на сервере
        'dbhost' => $config_default_dbhost,                             // Адрес сервера MySql
        'dbport' => $config_default_dbport,                             // Порт сервера MySql
        'dbuser' => $config_default_dbuser,                             // Пользователь БД
        'dbpass' => $config_default_dbpass,                             // Пароль БД
        'doctypes' => array(											// Список типов документов (модулей), хранящихся в базе
            'DEXPlugin.Document.MTS.Jeans'
        ),
        "dexuser" => $config_default_dex_user                           // Пользователь DEX, под которым DEXOL осуществляет вход в БД
	),
	'MTS_STS_062013' => array(
        'title' => 'МТС СК 06.2013',									// Наименование в списке БД клиента
        'engine' => 'mts',                                         		// Ключи конфигурации (для обработчиков)
        'dbname' => 'dex_mts_sts_062013',								// Наименование базы данных на сервере
        'dbhost' => $config_default_dbhost,                             // Адрес сервера MySql
        'dbport' => $config_default_dbport,                             // Порт сервера MySql
        'dbuser' => $config_default_dbuser,                             // Пользователь БД
        'dbpass' => $config_default_dbpass,                             // Пароль БД
        'doctypes' => array(											// Список типов документов (модулей), хранящихся в базе
            'DEXPlugin.Document.MTS.Jeans'
        ),
        "dexuser" => $config_default_dex_user                           // Пользователь DEX, под которым DEXOL осуществляет вход в БД
	),
	'MTS_KBR' => array(
        'title' => 'МТС КБР',											// Наименование в списке БД клиента
        'engine' => 'mts',                                         		// Ключи конфигурации (для обработчиков)
        'dbname' => 'dex_mts_kbr',										// Наименование базы данных на сервере
        'dbhost' => $config_default_dbhost,                             // Адрес сервера MySql
        'dbport' => $config_default_dbport,                             // Порт сервера MySql
        'dbuser' => $config_default_dbuser,                             // Пользователь БД
        'dbpass' => $config_default_dbpass,                             // Пароль БД
        'doctypes' => array(											// Список типов документов (модулей), хранящихся в базе
            'DEXPlugin.Document.MTS.Jeans'
        ),
        "dexuser" => $config_default_dex_user                           // Пользователь DEX, под которым DEXOL осуществляет вход в БД
	),
	'MTS_KCR' => array(
        'title' => 'МТС КЧР',											// Наименование в списке БД клиента
        'engine' => 'mts',                                         		// Ключи конфигурации (для обработчиков)
        'dbname' => 'dex_mts_kcr',										// Наименование базы данных на сервере
        'dbhost' => $config_default_dbhost,                             // Адрес сервера MySql
        'dbport' => $config_default_dbport,                             // Порт сервера MySql
        'dbuser' => $config_default_dbuser,                             // Пользователь БД
        'dbpass' => $config_default_dbpass,                             // Пароль БД
        'doctypes' => array(											// Список типов документов (модулей), хранящихся в базе
            'DEXPlugin.Document.MTS.Jeans'
        ),
        "dexuser" => $config_default_dex_user                           // Пользователь DEX, под которым DEXOL осуществляет вход в БД
	)
);
// *************************************************************
// * Конфигурация пользователей DEXOL
// *************************************************************
$config_users = array();

/*
// Конфигурация пользователя test
$config_users['test'] = array(
    'name' => 'Тестовый пользователь',		// Человеческое имя пользователя
    'password' => 'test',					// Пароль пользователя
    // Параметры пользователя, передаваемые в тулбокс.
    // Значения DefaultDocumentState (состояние документа по умолчанию):
    // 0 = "Черновик", 1 = "На подтверждение", 2 = "Подтверждён",
    // 3 = "На отправку", 4 = "Отправлен", 5 = "Возвращён"
    //
    'user_props' => '<Properties><DefaultDocumentState>1</DefaultDocumentState></Properties>',
	'profile_code' => 'test1',				// Код профиля отправки. Внимание! Должен совпадать с тем, который в списке профилей в DEX_MEGA
    'db' => array(							// Список БД, с которыми имеет право работать пользователь
        'MEGA' =>  0,						// Связка: Идентификатор БД -> ID отделения
    )
);
*/
?>