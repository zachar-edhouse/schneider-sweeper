namespace Schneider.Sweeper.Map
{
    internal static class MapHelper
    {
        private const uint DEFAULT_MAP_SIZE = 8;
        private const uint DEFAULT_MINES_COUNT = 10;

        /// <summary>
        /// Loads rectangular map from a file. The file contains 'image' of the map in the following format:
        ///  - ' ' represents space
        ///  - 'x' represents mine
        /// All lines must have the same length.
        /// </summary>
        /// <param name="filePath">Path to a file containing map in the valid format.</param>
        /// <returns>Memory representation of the data.</returns>
        /// <exception cref="FileNotFoundException">Thrown when the file is not present.</exception>
        /// <exception cref="InvalidDataException">Thrown when provided file doesn't contain valid data.</exception>
        public static FieldValue[,] LoadFromFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"Couln't find file '{filePath}'.", filePath);
            }

            List<string> lines = new List<string>();
            using (var f = File.OpenText(filePath))
            {
                while (!f.EndOfStream)
                {
                    lines.Add(f.ReadLine() ?? "");
                }
            }

            if (lines.Count == 0)
            {
                throw new InvalidDataException($"The file '{filePath}' doesn't contain any data.");
            }

            var lineLength = lines[0].Count();
            if (lines.Exists(s => s.Count() != lineLength))
            {
                throw new InvalidDataException($"The file '{filePath}' doesn't contain valid data. Lines do not have the same length.");
            }

            var finalData = new FieldValue[lines.Count(), lineLength];
            for (int i = 0; i < lines.Count; i++)
            {
                for (int j = 0; j < lineLength; j++)
                {
                    var ch = lines[i][j];
                    finalData[i, j] = CharToFieldValue(ch);
                }
            }

            return finalData;
        }

        /// <summary>
        /// Creates random map with the default size and the default mines count.
        /// </summary>
        /// <returns>Default map with random mines distribution.</returns>
        public static FieldValue[,] CreateRandom(uint mapWidth = DEFAULT_MAP_SIZE, uint mapHeight = DEFAULT_MAP_SIZE, uint minesCount = DEFAULT_MINES_COUNT)
        {
            var data = new FieldValue[mapHeight, mapWidth];
            var random = new Random(DateTime.Now.Millisecond);
            for (var i = 0; i < minesCount; ++i)
            {
                // do not care about the situation when random positions are generated with the same coordinates
                var x = random.Next((int)mapWidth);
                var y = random.Next((int)mapHeight);
                data[y, x] = FieldValue.Mine;
            }
            return data;
        }

        private static FieldValue CharToFieldValue(char ch)
        {
            switch (ch)
            {
                case ' ':
                    return FieldValue.Empty;
                case 'x':
                case 'X':
                    return FieldValue.Mine;
                default:
                    throw new InvalidDataException($"The provided file contains invalid character: {ch}");
            }
        }
    }
}
