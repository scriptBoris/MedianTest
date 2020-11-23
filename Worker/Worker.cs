using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Worker.Algorithms;

namespace Worker
{
    public class Worker
    {
        private IAlgorithm[] algorithms = new IAlgorithm[]
        {
            new Sum(),
            new Multiply(),
            new Max(),
            new Min(),
        };

        public string FilePath;

        public Worker()
        {
        }

        public Worker(string filePath)
        {
            FilePath = filePath;
        }


        public async IAsyncEnumerable<float> ReadFile(CancellationToken? cancel = null)
        {
            if (string.IsNullOrWhiteSpace(FilePath))
                throw new FileNotFoundException($"Необходимо указать путь к файлу");

            if (!File.Exists(FilePath))
                throw new FileNotFoundException($"Не удалось найти {FilePath}");

            var stream = File.OpenText(FilePath);
            int lineCount = 0;
            string line;

            while ((line = await stream.ReadLineAsync()) != null)
            {
                if (cancel.HasValue && cancel.Value.IsCancellationRequested)
                {
                    stream.Dispose();
                    throw new OperationCanceledException();
                }

                lineCount++;
                float? res = CatchLine(line, lineCount);
                if (res.HasValue)
                    yield return res.Value;

                await Task.Delay(500);
            }

            stream.Dispose();
        }

        private float? CatchLine(string line, int lineNumber)
        {
            var splitLine = line.Split(' ');
            if (splitLine.Length < 2)
                return null;

            // Парсим тип
            int type;
            if (!int.TryParse(splitLine[0], out type))
                throw new Exception($"Начало строки должно быть числом (0-4) (Тип). Строка {lineNumber}");

            if (type < 0 || type > 4)
                throw new Exception($"Начало строки должно быть числом (0-4) (Тип). Строка {lineNumber}");


            // Парсим параметры
            var parameters = new List<int>();

            for (int i = 1; i < splitLine.Length; i++)
            {
                string param = splitLine[i];
                if (int.TryParse(param, out int parseParam))
                {
                    if (parseParam >= 0 && parseParam <= 255)
                        parameters.Add(parseParam);
                    else
                        throw new Exception($"Параметры строки должны быть числами (0-255). Строка {lineNumber}");
                }
                else
                {
                    throw new Exception($"Параметры строки должны быть числами (0-255). Строка {lineNumber}");
                }
            }

            return algorithms[type-1].Calculate(parameters.ToArray());
        }
    }
}
