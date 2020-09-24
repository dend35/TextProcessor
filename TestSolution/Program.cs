using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TestSolution.Context;
using TestSolution.Helper;
using TestSolution.Helper.Interface;

namespace TestSolution
{
    class Program
    {
        private static bool AppIsRun = true;
        
        static async Task Main(string[] args)
        {
            await using IWordHelper wordHelper = new WordHelper(new WordContext());
            if (args.Length != 0)
            {
                foreach (var arg in args)
                {
                    switch (arg)
                    {
                        case "update":
                            await wordHelper.CreateDictionary("input.txt");
                            break;
                        case "create":
                            await wordHelper.Clear();
                            await wordHelper.CreateDictionary("input.txt");
                            break;
                        case "delete":
                            await wordHelper.Clear();
                            break;
                    }
                }
            }
            else
            {
                Console.WriteLine("Для получения помощи введите /help");
                await GUI(wordHelper);
                Console.WriteLine("Exit");
            }
        }

        static async Task GUI(IWordHelper wordHelper)
        {
            while (AppIsRun)
            {
                Console.WriteLine("Введите префикс: ");
                var input = Console.ReadLine();
                if (input != null && input.Contains('/'))
                    switch (input)
                    {
                        case "/":
                        case "/help":
                            Console.WriteLine("Список доступных команд:");
                            Console.WriteLine("/ или /help\t откроет этот список");
                            Console.WriteLine("/exit\tВыход из приложения");
                            Console.WriteLine("/clear\tОчистка бд");
                            Console.WriteLine("/create \"filename\"\tДобавление в словарь слов из файла");
                            break;
                        case "/exit":
                            AppIsRun = false;
                            break;
                        case "/clear":
                            await wordHelper.Clear();
                            break;
                        default:
                            if (input.Contains("/create "))
                            {
                                var filename = input.Substring(8);
                                if (File.Exists(filename))
                                {
                                    var (success, count) = await wordHelper.CreateDictionary(filename);
                                    if(success)
                                        Console.WriteLine($"База успешно обновлена {count} новых слов добавлено");
                                }
                                else
                                {
                                    Console.WriteLine("Файл не существует");
                                }
                                
                            }
                            break;
                        
                    }
                else
                {
                    var (query, error) = wordHelper.GetByPrefix(input);
                    if (query == null)
                    {
                        Console.WriteLine(error);
                    }
                    else
                    {
                        foreach (var word in query!.OrderByDescending(i=>i.Frequency).ThenBy(i=>i.Text).Select(i=>i.Text))
                        {
                            Console.WriteLine("> " + word); 
                        }
                    }
                }
            }
        }
    }
}