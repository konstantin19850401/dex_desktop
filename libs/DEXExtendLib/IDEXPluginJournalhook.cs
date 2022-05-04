using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DEXExtendLib
{
    public interface IDEXPluginJournalhook : IDEXPluginInfo
    {
        /// <summary>
        /// Инициализация списка видимости функций
        /// </summary>
        void InitReflist(object toolbox);

        /// <summary>
        /// Предоставляется информация, позволяющая определить видимость пунктов меню в зависимости от типа и статуса документа.
        /// </summary>
        /// <param name="DocType">Тип документа</param>
        /// <param name="DocStatus">Статус документа</param>
        void AddReferenceVisibility(object toolbox, string DocType, int DocStatus);

        /// <summary>
        /// Запрос списка видимых пунктов меню, полученного на основании вызова AddReferenceVisibility для каждого выделенного документа.
        /// Если нет ни одного видимого элемента - возвращает пустой список.
        /// </summary>
        /// <returns>Список "метка=название" видимых пунктов меню</returns>
        Dictionary<string, string> getVisibleFunctionsList(object toolbox);


        /// <summary>
        /// Список подфункций для указанной функции.
        /// </summary>
        /// <param name="FunctionName">Наименование функции</param>
        /// <returns>Список подфункций "метка=название" или null, если у функции нет подфункций.</returns>
        Dictionary<string, string> getVisibleSubFunctionsList(string FunctionName);

        /// <summary>
        /// Исполнение функции для документа.
        /// </summary>
        /// <param name="FunctionName">Наименование функции</param>
        /// <param name="SubFunctionName">Наименование подфункции или null, если нет</param>
        /// <param name="docId">Тип документа-источника</param>
        /// <param name="doc">Документ-источник</param>
        /// <param name="docChanged">true, если документ был изменен</param>
        /// <returns>true, если дальнейшее выполнение функции (до первого вызова InitReflist) необходимо прекратить.</returns>
        bool RunFunctionForDocument(string FunctionName, string SubFunctionName, string docId, IDEXDocumentData doc, out bool docChanged);
    }
}
