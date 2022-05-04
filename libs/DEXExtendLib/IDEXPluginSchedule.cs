using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DEXExtendLib
{
    public interface IDEXPluginSchedule : IDEXPluginInfo
    {
        /// <summary>
        /// Вызывается в случае, если назначен запуск этой задачи
        /// </summary>
        void Schedule(Object toolbox);

        /// <summary>
        /// Выполняется раз в 10 секунд, если запуск задачи не назначен
        /// </summary>
        void Idle(Object toolbox);
    }
}
