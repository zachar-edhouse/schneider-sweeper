namespace Schneider.Sweeper.Map
{
    internal class Map : IMap
    {
        private readonly FieldValue[,] privateData;
        private readonly FieldValue[,] publicData;

        public uint Width { get; }
        public uint Height { get; }

        public Map(FieldValue[,] data, uint initialX, uint initialY)
        {
            Width = (uint)data.GetLength(1);
            Height = (uint)data.GetLength(0);
            privateData = Initialize(data);
            publicData = new FieldValue[Height, Width];
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    publicData[y, x] = FieldValue.Hidden;
                }
            }

            Visit(initialX, initialY);
        }

        public FieldValue this[uint x, uint y]
        {
            get => At(x, y);
        }

        public FieldValue Visit(uint x, uint y)
        {
            publicData[y, x] = privateData[y, x];
            List<Tuple<uint, uint>> toVisit = new List<Tuple<uint, uint>>();
            if (privateData[y, x] == FieldValue.Empty)
            {
                for (uint j = (uint)Math.Max(0, (int)y - 1); j <= Math.Min(y + 1, Height - 1); ++j)
                {
                    for (uint i = (uint)Math.Max(0, (int)x - 1); i <= Math.Min(x + 1, Width - 1); ++i)
                    {
                        if (publicData[j, i] == FieldValue.Hidden)
                        {
                            toVisit.Add(new Tuple<uint, uint>(i, j));
                        }
                        publicData[j, i] = privateData[j, i];
                    }
                }
            }
            foreach (var coordinates in toVisit)
            {
                Visit(coordinates.Item1, coordinates.Item2);
            }

            return publicData[y, x];
        }

        private FieldValue At(uint x, uint y)
        {
            if (x > Width || y > Height)
            {
                throw new IndexOutOfRangeException($"The coordinates [{x}, {y}] are out of limits ({Width}, {Height}).");
            }

            return publicData[y, x];
        }

        private FieldValue[,] Initialize(FieldValue[,] loadedData)
        {
            FieldValue[,] result = new FieldValue[Height, Width];

            for (int y = 0; y < Height; ++y)
            {
                for (int x = 0; x < Width; ++x)
                {
                    FieldValue value = GetEightNeighbourhoodValue(loadedData, x, y);
                    result[y, x] = value;
                }
            }

            return result;
        }

        private FieldValue GetEightNeighbourhoodValue(FieldValue[,] data, int x, int y)
        {
            if (data[y, x] == FieldValue.Mine)
            {
                return FieldValue.Mine;
            }

            int mineCount = 0;
            for (int localY = Math.Max(0, y - 1); localY <= Math.Min(y + 1, Height - 1); ++localY)
            {
                for (int localX = Math.Max(0, x - 1); localX <= Math.Min(x + 1, Width - 1); ++localX)
                {
                    if (data[localY, localX] == FieldValue.Mine)
                    {
                        mineCount++;
                    }
                }
            }

            return (FieldValue)mineCount;
        }
    }
}
