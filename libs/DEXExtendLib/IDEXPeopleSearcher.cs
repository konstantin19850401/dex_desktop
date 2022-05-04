using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace DEXExtendLib
{
    public interface IDEXPeopleSearcher
    {
        /// <summary>
        /// Поиск людей с заданными именем, отчеством, фамилией и датой рождения.
        /// </summary>
        /// <param name="FirstName">Имя или null, если не нужно искать по имени</param>
        /// <param name="SecondName">Отчество или null, если не нужно искать по отчеству</param>
        /// <param name="LastName">Фамилия или null, если не нужно искать по фамилии</param>
        /// <param name="Birth">Дата рождения в формате YYYYMMDD или null, если не нужно искать по дате рождения</param>
        /// <returns>Список строк с хэшами найденных записей</returns>
        ArrayList getPeopleHash(string FirstName, string SecondName, string LastName, string Birth);

        /// <summary>
        /// Получение сведений о конкретном человеке по хэшу.
        /// </summary>
        /// <param name="Hash">Хэш данных о человеке</param>
        /// <returns>Объект StringList с данными или null, если данными с таким хэшом не найдено</returns>
        StringList getPeopleData(string Hash);

        /// <summary>
        /// Сохранение или обновление уже сохранённых сведений о человеке.
        /// </summary>
        /// <param name="Hash">Хэш данных или null, если необходимо добавить сведения о новом человеке.</param>
        /// <param name="PData">Объект StringList со сведениями</param>
        /// <returns>string с хэшом добавленного документа или null, если запись не добавлена</returns>
        string setPeopleData(StringList PData);

        /// <summary>
        /// Проверка паспортных данных записи сведений о человеке.
        /// </summary>
        /// <param name="PData">Объект StringList со сведениями</param>
        /// <returns>null, если нет ошибок или текст ошибки</returns>
        string[] checkPassport(StringList PData);

    }
}
