using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace algorithms
{
    public static class BenchmarkRunner
    {
        // Выполняет быстрый прогон: прогрев + измерение в течение минимального времени (прогрев не учитывается в измерениях)
        public static (double avgMsPerCall, long iterations, double totalMs) Run(IFunction func, double[] v,
            TimeSpan minDuration, int minIterations = 100)
        {
            // Прогрев
            for (int i = 0; i < 200; i++) func.Evaluate(v);

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            var sw = Stopwatch.StartNew();
            long iterations = 0;
            TimeSpan lastReport = TimeSpan.Zero;
            TimeSpan reportInterval = TimeSpan.FromSeconds(2);

            while (sw.Elapsed < minDuration || iterations < minIterations)
            {
                func.Evaluate(v);
                iterations++;

                // небольшой локальный прогресс (чтобы отслеживать)
                if (sw.Elapsed - lastReport > reportInterval)
                {
                    Console.Write($"\r  local elapsed {sw.Elapsed.TotalSeconds:F0}s, it={iterations}   ");
                    lastReport = sw.Elapsed;
                }
            }
            sw.Stop();

            double totalMs = sw.Elapsed.TotalMilliseconds;
            double avgMs = totalMs / iterations;
            Console.WriteLine(); // завершение локальной строки
            return (avgMs, iterations, totalMs);
        }
    }
}
