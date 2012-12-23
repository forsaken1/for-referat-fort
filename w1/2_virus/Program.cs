using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ConsoleApplication1
{
    class Program
    {
        static string Modificate(int n, string virus, string str)
        {
            string result = "";
            for(int i = 0; i < n; i++)
                result += str[i];

            switch (virus)
            {
                case "+":
                    for (int i = n; i < str.Length - 1; i++)
                        result += (str[i - 1] + str[i + 1]) > 0 ? (char)(str[i - 1] + result[i - 1]) : (char)(32);
                    break;
                case "-":
                    for (int i = n; i < str.Length - 1; i++)
                        result += (str[i - 1] - str[i + 1]) > 0 ? (char)(str[i - 1] - result[i - 1]) : (char)(32);
                    break;
                case "*":
                    for (int i = n; i < str.Length - 1; i++)
                        result += (str[i - 1] * str[i + 1]) > 0 ? (char)(str[i - 1] * result[i - 1]) : (char)(32);
                    break;
                case "/":
                    for (int i = n; i < str.Length - 1; i++)
                        result += (str[i - 1] / str[i + 1]) > 0 ? (char)(str[i - 1] / result[i - 1]) : (char)(32);
                    break;
            }
            result += str[str.Length - 2]; //последний символ модифицируемого элемента - предпоследний символ исходной строки
            return result;
        }

        static string Remodificate(int n, string virus, string str)
        {
            string result = "";
            for(int i = 0; i < n; i++)
                result += str[i];

            switch(virus)
            {
                case "+":
                    for (int i = n; i < str.Length - 1; i++)
                        result += (str[i - 1] - result[i - 1]) > 0 ? (char)(str[i - 1] - result[i - 1]) : (char)(32);
                break;

                case "-":
                    for (int i = n; i < str.Length - 1; i++)
                        result += (str[i - 1] + result[i - 1]) > 0 ? (char)(str[i - 1] + result[i - 1]) : (char)(32);
                break;

                case "*":
                    for (int i = n; i < str.Length - 1; i++)
                        result += (str[i - 1] / result[i - 1]) > 0 ? (char)(str[i - 1] / result[i - 1]) : (char)(32);
                break;

                case "/":
                    for (int i = n; i < str.Length - 1; i++)
                        result += (str[i - 1] * result[i - 1]) > 0 ? (char)(str[i - 1] * result[i - 1]) : (char)(32);
                break;
            }
            return result;
        }

        static bool IsNumber(char ch)
        {
            return '0' <= ch && ch <= '9';
        }

        static bool IsLetter(char ch)
        {
            return 'A' <= ch && ch <= 'Z' || 'a' <= ch && ch <= 'z' || 'а' <= ch && ch <= 'я' || 'А' <= ch && ch <= 'Я';
        }

        static bool Checked(string str)
        {
            for (int i = 0; i < str.Length; i++)
                if (!IsNumber(str[i]) && !IsLetter(str[i]))
                    return false;
            return true;
        }

        static void Main(string[] args) //консольное приложение. файлы в директории bin/debug/..    
        {
            StreamReader sr = new StreamReader(File.Open("input.txt", FileMode.Open)); //открываем файл
            string[] s = sr.ReadLine().Split(' '); //парсим строку
            int n = int.Parse(s[0]);    //первая позиция в файле - n
            string str = s[1];          //вторая позиция - подпорченый вирусом текст
            string[] viruses = {"+", "-", "*", "/"}; //возможные варианты модификации (варианты логического сложения и умножения при модификации 
            //простых чисел (ASCII кодов символов) не имеет смысла реализовывать, ибо любое число кроме нуля будет являться true (если я вас правильно понял) )

            string rem = "#";
            for (int i = 0; i < viruses.Length && !Checked(rem); i++) 
                rem = Remodificate(n, viruses[i], str); //генерация всех возможных вариантов исходного файла и проверка на валидность

            if (Checked(rem)) //если есть валидный - выводим на консоль и выходим из программы
            {
                Console.WriteLine("Text after virus modificate: " + rem);
                StreamWriter sw = new StreamWriter(File.Open("output.txt", FileMode.Open)); //открываем файл для записи
                sw.Write(rem);
                sw.Close();
            }
            else //иначе - подразумаваем, что у нас есть исходный текст, портим его и выводим на консоль
            {
                sr = new StreamReader(File.Open("virus.txt", FileMode.Open)); //открываем файл
                string virus = sr.ReadLine(); //загружаем вирус

                sr = new StreamReader(File.Open("origin.txt", FileMode.Open)); //открываем файл
                string origin = sr.ReadLine(); //загружаем оригинальный текст

                Console.Write("Original text after virus modificate: '" + Modificate(n, virus, origin) + "'");  //"портим" исходный файл вирусом и выводим на консоль
            }
            Console.ReadLine(); //как я понял, подразумевалась возможность примерного восстановления, поэтому возможны несколько правильных вариантов выходного текста
        }
    }
}
