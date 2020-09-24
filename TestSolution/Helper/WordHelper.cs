using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TestSolution.Context;
using TestSolution.Helper.Interface;
using TestSolution.Model;

namespace TestSolution.Helper
{
    public class WordHelper : IWordHelper
    {
        private readonly WordContext _wordContext;

        public WordHelper(WordContext wordContext)
        {
            _wordContext = wordContext;
        }

        public async Task<(bool, int)> CreateDictionary(string filename)
        {
            var words = Parse(filename).GroupBy(i => i.ToLower()).Select(i => new Word
            {
                Text = i.Key,
                Frequency = i.Count()
            }).ToList();
            var doubles = new List<string>();
            foreach (var word in words)
            {
                var foundWord = _wordContext.Words.FirstOrDefault(i => i.Text == word.Text);
                if (foundWord == null) continue;
                foundWord.Frequency += word.Frequency;
                doubles.Add(word.Text);
            }

            words = words.Where(i => !doubles.Contains(i.Text))
                .ToList(); //убираем дубли, можно было написать на строчку ниже
            await Add(words.ToArray()); //чтобы избежать излишнего преобразования
            return (true, words.Count); //но захотел фичу чтобы показывать колько новых слов будет добавлено
        }

        public async Task<bool> Add(params Word[] words)
        {
            await _wordContext.Words.AddRangeAsync(words);
            await Flush();
            return true;
        }

        public IQueryable<Word> GetAll()
        {
            return _wordContext.Words.AsQueryable();
        }

        public (IQueryable<Word>, string) GetByPrefix(string prefix)
        {
            return prefix.Length < 2
                ? (null, "Укажите более длинный префикс")
                : (_wordContext.Words.Where(i => i.Text.StartsWith(prefix)).OrderByDescending(i=>i.Frequency).ThenBy(i=>i.Text), "");
        }

        public async Task<bool> Clear()
        {
            _wordContext.Words.RemoveRange(_wordContext.Words);
            await Flush();
            return true;
        }

        private async Task Flush()
        {
            await _wordContext.SaveChangesAsync();
        }

        public async Task<bool> Recreate(params Word[] words)
        {
            return await Clear() && await Add(words);
        }

        public async ValueTask DisposeAsync()
        {
            await _wordContext.SaveChangesAsync();
        }

        public IEnumerable<string> Parse(string filename) // можно кончено было сделать через регулярки, но так не интересно
        {
            var allowedChars = AlphabetHelper.GetAllowedChars(
                (Constants.RuАlphabet, true),
                (Constants.EnАlphabet, true),
                (Constants.SpecАlphabet, false));

            foreach (var line in File.ReadAllLines(filename))
            {
                var formattedLine = new string(line.Select(i => allowedChars.Contains(i) ? i : " "[0]).ToArray());
                while (formattedLine.Contains("  ")) //2 пробела
                    formattedLine = formattedLine.Replace("  ", " ");
                var split = formattedLine.Split(" ");
                foreach (var t in split)
                {
                    var word = t;
                    if (word.Contains(" "))
                        word = word.Replace(" ", "");
                    if (!string.IsNullOrEmpty(word) && word.Length > 2)
                        yield return word;
                }
            }
        }
    }
}