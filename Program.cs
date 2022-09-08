using System;
using System.IO;
using NLog.Web;

namespace SleepData
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = Directory.GetCurrentDirectory() + "\\nlog.config";

            // create instance of Logger
            var logger = NLog.Web.NLogBuilder.ConfigureNLog(path).GetCurrentClassLogger();
            logger.Info("Program started");

            // ask for input
            Console.WriteLine("Enter 1 to create data file.");
            Console.WriteLine("Enter 2 to parse data.");
            Console.WriteLine("Enter anything else to quit.");
            // input response
            string resp = Console.ReadLine();

            if (resp == "1")
            {
                // create data file

                // ask a question
                // input the response (convert to int)

                bool isValid = false;
                int weeks = 0;
                while (!isValid)
                {
                    Console.WriteLine("How many weeks of data is needed?");

                    if (int.TryParse(Console.ReadLine(), out weeks))
                    {
                        isValid = true;
                    }
                    else
                    {
                        logger.Error("Invalid input");
                    }
                }

                // determine start and end date
                DateTime today = DateTime.Now;
                // we want full weeks sunday - saturday
                DateTime dataEndDate = today.AddDays(-(int)today.DayOfWeek);
                // subtract # of weeks from endDate to get startDate
                DateTime dataDate = dataEndDate.AddDays(-(weeks * 7));
                
                // random number generator
                Random rnd = new Random();

                 // create file
                StreamWriter sw = new StreamWriter("data.txt");

                // loop for the desired # of weeks
                while (dataDate < dataEndDate)
                {
                    // 7 days in a week
                    int[] hours = new int[7];
                    for (int i = 0; i < hours.Length; i++)
                    {
                        // generate random number of hours slept between 4-12 (inclusive)
                        hours[i] = rnd.Next(4, 13);
                    }
                    // M/d/yyyy,#|#|#|#|#|#|#
                    //Console.WriteLine($"{dataDate:M/d/yy},{string.Join("|", hours)}");
                    sw.WriteLine($"{dataDate:M/d/yyyy},{string.Join("|", hours)}");
                    // add 1 week to date
                    dataDate = dataDate.AddDays(7);
                }
                sw.Close();
            }
            else if (resp == "2")
            {
                StreamReader sr = new StreamReader("data.txt");
                while (!sr.EndOfStream)
                {
                    string[] line = sr.ReadLine().Split(',');
                    DateTime date = DateTime.Parse(line[0]);
                    string[] hours = line[1].Split('|');
                    
                    Console.WriteLine($"Week of {date:MMM} {date:dd}, {date:yyyy}");
                }

                sr.Close();
            }

            logger.Info("Program ended");
        }
    }
}