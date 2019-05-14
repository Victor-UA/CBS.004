using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CBS._004
{
    class Program
    {
        // Напишите программу, которая бы позволила вам по указанному адресу web-страницы
        // выбирать все

        // ссылки на другие страницы,
        // номера телефонов,
        // почтовые адреса

        // и сохраняла полученный результат в файл.


        private const string FILE_PATH = "ParsingResult.txt";
        private const string HREF_PATTERN = "<a[^>]* href=\"(?<link>(http([^ \"]*)))\"";                                             
        private const string PHONENUMBER_PATTERN = @"\(?\d{3}\)?-? *\d{3}-? *-?\d{4}";
        private const string EMAIL_PATTERN = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";

        static void Main(string[] args)
        {            
            string pageContent = DownloadPage("https://ru.wikipedia.org/wiki/%D0%93%D1%80%D0%B0%D0%BD%D0%B6");
            var parser = new Parser(pageContent);
            if (File.Exists(FILE_PATH))
            {
                try
                {
                    File.Delete(FILE_PATH);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
            var hRefCollection = parser.Parse2File("References:", HREF_PATTERN, FILE_PATH, new List<string>{"link"});
            var phoneNumberCollection = parser.Parse2File("Phone numbers:", PHONENUMBER_PATTERN, FILE_PATH);
            var emailCollection = parser.Parse2File("E-mails:", EMAIL_PATTERN, FILE_PATH);

            //Console.Write(pageContent);
            Console.WriteLine("Done");
            Console.ReadKey();
        }

        private static string DownloadPage(string url)
        {
            var client = new HttpClient();
            var response = client.GetAsync(url).Result;
            if (!response.IsSuccessStatusCode)
            {
                throw new ArgumentException("Cannot download page via this url");
            }
            return response.Content.ReadAsStringAsync().Result;
        }
    }
}
