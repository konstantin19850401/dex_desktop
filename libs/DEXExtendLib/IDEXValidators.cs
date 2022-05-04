using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DEXExtendLib
{
    public interface IDEXValidators
    {
        /// <summary>
        /// Проверка, не является ли дата более ранней, чем значение startperiod таблицы регистров.
        /// Если startperiod отсутствует или не является датой, проверка всегда возвращает true.
        /// </summary>
        /// <param name="src">Дата, которую необходимо проверить</param>
        /// <returns>Результат проверки</returns>
        bool CheckPeriodDate(DateTime src);
    }
}
