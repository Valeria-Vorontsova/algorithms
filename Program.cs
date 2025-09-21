using System.Diagnostics;
using System.Globalization;

namespace algorithms
{
    internal class Program
    {
        static List<int> GenerateLogSpaceNs(int maxN, int points)
        {
            var ns = new List<int>();
            for (int i = 0; i < points; i++)
            {
                double t = (double)i / Math.Max(1, points - 1);
                double val = Math.Exp(Math.Log(1) + t * Math.Log(maxN));
                int n = Math.Max(1, (int)Math.Round(val));
                ns.Add(n);
            }
            return ns.Distinct().OrderBy(x => x).ToList();
        }

        static void Main(string[] args)
        {
            var culture = CultureInfo.InvariantCulture;
            int trials = 5;

            // Параметры эксперимента (изменяемо)
            int maxN = 10000;
            int points = 600; // кол-во различных n
            int perRunSeconds = 1; // время замера для одного n в рамках одного прохода
            bool loopNs = true;
            TimeSpan totalDuration = TimeSpan.FromMinutes(10); // длительность одного trial (~10 минут)
            string outCsv = "results_sum_function_various_n1.csv";

            // чтобы данные отличались между запусками/trial
            var rnd = new Random();
            IFunction func = new SumFunction();

            var ns = GenerateLogSpaceNs(maxN, points);

            Console.WriteLine($"Function: {func.Name}");
            Console.WriteLine($"Ns (count {ns.Count}): {string.Join(", ", ns)}");
            Console.WriteLine($"Per trial time: {totalDuration.TotalMinutes} min, per-run {perRunSeconds}s, loopNs={loopNs}");
            Console.WriteLine($"Results -> {Path.GetFullPath(outCsv)}");

            using (var sw = new StreamWriter(outCsv, append: false))
            {
                // Добавили колонку 'trial'
                sw.WriteLine("timestamp,processor_count,trial,n,cycle_index,avg_ms_per_call,iterations,total_ms,func_name");

                for (int trial = 1; trial <= trials; trial++)
                {
                    Console.WriteLine($"\n=== Starting trial {trial} of {trials} ===");
                    var swTrial = Stopwatch.StartNew();
                    int cycleIndex = 0;

                    while (swTrial.Elapsed < totalDuration)
                    {
                        cycleIndex++;
                        foreach (var n in ns)
                        {
                            if (swTrial.Elapsed >= totalDuration) break;

                            // генерируем новый вектор для каждого замера (будет отличаться)
                            double[] v = VectorGenerator.GenerateRandomVector(n, rnd);

                            Console.WriteLine($"Trial {trial}  Cycle {cycleIndex}, n={n}  target {perRunSeconds}s  trial elapsed {swTrial.Elapsed.TotalSeconds:F0}s");

                            var (avgMs, iterations, totalMs) = BenchmarkRunner.Run(func, v, TimeSpan.FromSeconds(perRunSeconds), 100);

                            // записываем trial в CSV
                            string line = $"{DateTime.Now},{Environment.ProcessorCount},{trial},{n},{cycleIndex},{avgMs.ToString("F9", culture)},{iterations},{totalMs.ToString("F1", culture)},{func.Name}";
                            sw.WriteLine(line);
                            sw.Flush();

                            Console.WriteLine($" -> Trial {trial}, n={n} avg {avgMs:F9} ms/call, it={iterations}, total {totalMs / 1000.0:F2}s");
                        }

                        if (!loopNs) break; // если нужно только одно прохождение списка
                    }

                    swTrial.Stop();
                    Console.WriteLine($"=== Trial {trial} finished. Elapsed {swTrial.Elapsed.TotalSeconds:F1}s ===");
                    // Небольшая пауза между trial-ами, можно убрать
                    System.Threading.Thread.Sleep(1000);
                }
            }

            Console.WriteLine("All trials finished. CSV saved.");
        }
    }
}
