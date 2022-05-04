using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DEXExtendLib
{
    public interface IDEXScheduler
    {
        /// <summary>
        /// Добавление задания в очередь. Если задание уже имеется в очереди, 
        /// для него устанавливается новое время запуска.
        /// </summary>
        /// <param name="schedule">Объект задания</param>
        /// <param name="nextTime">Время запуска задания</param>
        void AddSchedule(IDEXPluginSchedule schedule, DateTime nextTime);

        /// <summary>
        /// Добавление задания в очередь на выполнение через указанное количество секунд 
        /// после текущего момента. Если задание уже имеется в очереди, для него
        /// устанавливается новое время запуска.
        /// </summary>
        /// <param name="schedule">Объект задания</param>
        /// <param name="SecondsAfterNow">Количество секунд, через которое задание должно быть запущено</param>
        void AddSchedule(IDEXPluginSchedule schedule, int SecondsAfterNow);

        /// <summary>
        /// Удаление задания из очереди.
        /// </summary>
        /// <param name="schedule">Объект задания</param>
        void RemoveSchedule(IDEXPluginSchedule schedule);
        
        /// <summary>
        /// Проверка, стоит ли задание в очереди.
        /// </summary>
        /// <param name="schedule">Объект задания</param>
        /// <returns>true, если задание отсутствует в очереди.</returns>
        bool IsScheduleIdle(IDEXPluginSchedule schedule);

        /// <summary>
        /// Получение времени запуска задания.
        /// </summary>
        /// <param name="schedule">Объект задания</param>
        /// <returns>Время следующего запуска задания или DateTime.MinValue, 
        /// если задание не запланировано.</returns>
        DateTime GetScheduleTime(IDEXPluginSchedule schedule);
    }
}
