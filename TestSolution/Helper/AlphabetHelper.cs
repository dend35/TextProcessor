using System.Text;

namespace TestSolution.Helper
{
    public class AlphabetHelper
    {
        /// <summary>
        /// Получить массив допустимых символов в тексте
        /// </summary>
        /// <param name="alphabets">string - dictionary | bool - needToUp</param>
        /// <returns></returns>
        public static string GetAllowedChars(params (string, bool)[] alphabets)
        {
            var concatenated = new StringBuilder();
            foreach (var (alphabet, needToUp) in alphabets)
            {
                concatenated.Append(alphabet);
                if (needToUp)
                    concatenated.Append(alphabet.ToUpper());
            }

            return concatenated.ToString();
        }
    }
}