using System;
using System.Collections;
using System.Collections.Generic;

namespace DEXExtendLib
{
    public interface IDEXPluginDocument : IDEXPluginInfo, IDEXPluginStartup
    {
        /// <summary>
        /// Возвращает список полей документа вида поле=описание
        /// </summary>
        /// <param name="toolbox"></param>
        /// <returns></returns>
        Dictionary<string, string> GetDocumentFields(Object toolbox);

        /// <summary>
        /// Возвращает поля документа, зарактеризующие лицо, заключившее договор.
        /// </summary>
        /// <param name="toolbox"></param>
        /// <param name="document"></param>
        /// <returns></returns>
        StringList GetPeopleData(Object toolbox, IDEXDocumentData document);

        /// <summary>
        /// Возвращает "человеческое" значение указанного поля. Нужно в основном для перечислений.
        /// </summary>
        /// <param name="toolbox"></param>
        /// <param name="fieldname">Наименование поля</param>
        /// <param name="value">Значение поля</param>
        /// <returns>Преобразованное значение поля</returns>
        string GetFieldValueText(Object toolbox, string fieldname, string value);

        /// <summary>
        /// Возвращает список полей документа, требующих проверку на дублирование.
        /// </summary>
        /// <param name="toolbox"></param>
        /// <returns></returns>
        ArrayList GetDocumentCriticals(Object toolbox);

        /// <summary>
        /// Проверяет значения полей документа на правильность заполнения
        /// </summary>
        /// <param name="toolbox"></param>
        /// <param name="document"></param>
        /// <returns></returns>
        ArrayList ValidateDocument(Object toolbox, IDEXDocumentData document);

        bool NewDocument(Object toolbox, IDEXDocumentData document);
        bool CloneDocument(Object toolbox, IDEXDocumentData source, IDEXDocumentData document);
        bool EditDocument(Object toolbox, IDEXDocumentData source, IDEXDocumentData document, IDEXDocumentData changes, bool ReadOnly);
        bool DeleteDocument(Object toolbox, IDEXDocumentData source, bool batch);
    }
}
