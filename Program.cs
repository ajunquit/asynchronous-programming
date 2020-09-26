using asynchronous_programming_async_await.Cases;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;

namespace asynchronous_programming_async_await
{
    class Program
    {
        static Metrics _metrics;
        static string _pathFileMetrics = @"C:\test-asynchronous-metrics\metrics.json";
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            ReadMetrics();
            await DoProgramAsync();
        }

        private static void ReadMetrics()
        {
            if (!File.Exists(_pathFileMetrics))
            {
                using (FileStream fs = File.Create(_pathFileMetrics))
                {
                    fs.Close();
                }
                _metrics = new Metrics();
                WriteMetrics();
            }
            JObject jsonObject = JObject.Parse(File.ReadAllText(_pathFileMetrics));
            _metrics = JsonConvert.DeserializeObject<Metrics>(jsonObject.ToString());
        }

        private static bool WriteMetrics()
        {
            // serialize JSON to a string and then write string to a file
            File.WriteAllText(_pathFileMetrics, JsonConvert.SerializeObject(_metrics, Formatting.Indented));
            return true;
        }

        private static async System.Threading.Tasks.Task DoProgramAsync()
        {
            string option = "-1";
            do
            {
                Console.Clear();
                ShowMenu();
                option = Console.ReadLine();
                Console.Clear();
                int myInt;
                bool isNumerical = int.TryParse(option, out myInt);
                if (isNumerical)
                {
                    double timeExecute = await PlayOptionAsync(Int32.Parse(option));
                    if (Int32.Parse(option) >= 1 && Int32.Parse(option) <= 6)
                    {
                        SaveOptionTimeOnMetrics(Int32.Parse(option), timeExecute);
                        WriteMetrics();
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Input is not numeric.");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                Console.ReadLine();
            } while (option != "0");
        }

        private static void SaveOptionTimeOnMetrics(int option, double time)
        {
            switch (option)
            {
                case 1:
                    _metrics.NonAsynchronous.Add(time);
                    break;
                case 2:
                    _metrics.NonBlocking.Add(time);
                    break;
                case 3:
                    _metrics.StartTasksConcurrently.Add(time);
                    break;
                case 4:
                    _metrics.StartTasksConcurrentlyButMoveOrder.Add(time);
                    break;
                case 5:
                    _metrics.CompositionWithTasks.Add(time);
                    break;
                case 6:
                    _metrics.AwaitTasksEfficiently.Add(time);
                    break;
            }
        }

        private static async System.Threading.Tasks.Task<double> PlayOptionAsync(int option)
        {
            var init = DateTime.Now;
            Console.WriteLine("Init {0}", init);
            switch (option)
            {
                case 1:
                    NonAsynchronous.Play();
                    break;
                case 2:
                    await NonBlocking.PlayAsync();
                    break;
                case 3:
                    await StartTasksConcurrently.PlayAsync();
                    break;
                case 4:
                    await StartTasksConcurrentlyButMoveOrder.PlayAsync();
                    break;
                case 5:
                    await CompositionWithTasks.PlayAsync();
                    break;
                case 6:
                    await AwaitTasksEfficiently.PlayAsync();
                    break;
                case 99:
                    ViewMetrics();
                    break;
                case 88:
                    ViewStadistics();
                    break;
                case 0:
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Thanks for use.");
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid option.");
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
            }
            var end = DateTime.Now;
            Console.WriteLine("End {0}", end);
            TimeSpan ts = end - init;
            double totalSeconds = ts.TotalSeconds;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Total seconds {0}", option == 99 ? 0 : totalSeconds);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Finish. Press any key to continue.");
            Console.ForegroundColor = ConsoleColor.White;
            return option == 99 ? 0 : totalSeconds;
        }

        private static void ViewStadistics()
        {
            Console.WriteLine("********************************************");
            Console.WriteLine("Stadistics - Minimum, Maximum and Average");
            Console.WriteLine("********************************************");
            Console.WriteLine("NonAsynchronous: \t\t\t Min {0} \t Max {1} \t Average {2} \t\t Attemps {3}", _metrics.NonAsynchronous.Min(), _metrics.NonAsynchronous.Max(), Math.Round(_metrics.NonAsynchronous.Average(),7), _metrics.NonAsynchronous.Count);
            Console.WriteLine("NonBlocking: \t\t\t\t Min {0} \t Max {1} \t Average {2} \t\t Attemps {3}", _metrics.NonBlocking.Min(), _metrics.NonBlocking.Max(), Math.Round(_metrics.NonBlocking.Average(),7), _metrics.NonBlocking.Count);
            Console.WriteLine("StartTasksConcurrently: \t\t Min {0} \t\t Max {1} \t Average {2} \t\t Attemps {3}", _metrics.StartTasksConcurrently.Min(), _metrics.StartTasksConcurrently.Max(), Math.Round(_metrics.StartTasksConcurrently.Average(),7), _metrics.StartTasksConcurrently.Count);
            Console.WriteLine("StartTasksConcurrentlyButMoveOrder: \t Min {0} \t\t Max {1} \t\t Average {2} \t\t Attemps {3}", _metrics.StartTasksConcurrentlyButMoveOrder.Min(), _metrics.StartTasksConcurrentlyButMoveOrder.Max(), Math.Round(_metrics.StartTasksConcurrentlyButMoveOrder.Average(),7), _metrics.StartTasksConcurrentlyButMoveOrder.Count);
            Console.WriteLine("CompositionWithTasks: \t\t\t Min {0} \t\t Max {1} \t\t Average {2} \t\t Attemps {3}", _metrics.CompositionWithTasks.Min(), _metrics.CompositionWithTasks.Max(), Math.Round(_metrics.CompositionWithTasks.Average(),7), _metrics.CompositionWithTasks.Count);
            Console.WriteLine("AwaitTasksEfficiently: \t\t\t Min {0} \t\t Max {1} \t\t Average {2} \t\t Attemps {3}", _metrics.AwaitTasksEfficiently.Min(), _metrics.AwaitTasksEfficiently.Max(), Math.Round(_metrics.AwaitTasksEfficiently.Average(),7), _metrics.AwaitTasksEfficiently.Count);
        }

        private static void ViewMetrics()
        {
            if (_metrics != null)
            {
                Console.WriteLine("Data {0}", JObject.Parse(File.ReadAllText(_pathFileMetrics).ToString()));
            }
        }

        private static void ShowMenu()
        {
            Console.WriteLine("MENU");
            Console.WriteLine("1.- Non asynchronous. Attempt {0}", _metrics.NonAsynchronous.Count);
            Console.WriteLine("2.- Don't block, await instead. Attempt {0}", _metrics.NonBlocking.Count);
            Console.WriteLine("3.- Start tasks concurrently. Attempt {0}", _metrics.StartTasksConcurrently.Count);
            Console.WriteLine("4.- Start tasks concurrently move order. Attempt {0}", _metrics.StartTasksConcurrentlyButMoveOrder.Count);
            Console.WriteLine("5.- Composition with tasks. Attempt {0}", _metrics.CompositionWithTasks.Count);
            Console.WriteLine("6.- Await tasks efficiently. Attempt {0}", _metrics.AwaitTasksEfficiently.Count);
            Console.WriteLine("99.- View metrics from file");
            Console.WriteLine("88.- View Stadistics");
            Console.WriteLine("0.- Exit ");
            Console.WriteLine("Select any option >> ");
        }
    }
}
