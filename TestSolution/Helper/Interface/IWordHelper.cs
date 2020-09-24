using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestSolution.Model;

namespace TestSolution.Helper.Interface
{
    public interface IWordHelper : IAsyncDisposable
    {
        /// <summary>
        /// Получить все слова из таблицы
        /// </summary>
        /// <returns></returns>
        public IQueryable<Word> GetAll();

        /// <summary>
        /// Получить список слов по префиксу
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        (IQueryable<Word>, string) GetByPrefix(string prefix);
        
        /// <summary>
        /// Очистка бд
        /// </summary>
        /// <returns></returns>
        Task<bool> Clear();
        
        /// <summary>
        /// Пересоздание таблицы
        /// </summary>
        /// <param name="words"></param>
        /// <returns></returns>
        Task<bool> Recreate(params Word[] words);
        
        /// <summary>
        /// Добавить слова в таблицу
        /// </summary>
        /// <param name="words"></param>
        /// <returns></returns>
        Task<bool> Add(params Word[] words);

        /// <summary>
        /// Получить слова из файла
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        IEnumerable<string> Parse(string filename);

        /// <summary>
        /// Создание словаря из файла
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        Task<(bool, int)> CreateDictionary(string filename);
    }
}