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
            privateData = InitializePrivateData(data);
            publicData = InitializePublicData();

            Visit(initialX, initialY);
        }

        public FieldValue this[uint x, uint y]
        {
            get 
            {
                if (x > Width || y > Height)
                {
                    throw new IndexOutOfRangeException($"The coordinates [{x}, {y}] are out of limits ({Width}, {Height}).");
                }

                return publicData[y, x];
            }
        }

        public FieldValue Visit(uint x, uint y)
        {
            publicData[y, x] = privateData[y, x];
            
            if (privateData[y, x] == FieldValue.Empty)
            {
                DoFor8Neighbourhood(x, y, (localX, localY) => { if (publicData[localY, localX] == FieldValue.Hidden) { Visit(localX, localY); } });
            }

            return publicData[y, x];
        }

        private FieldValue[,] InitializePrivateData(FieldValue[,] loadedData)
        {
            FieldValue[,] result = new FieldValue[Height, Width];

            for (uint y = 0; y < Height; ++y)
            {
                for (uint x = 0; x < Width; ++x)
                {
                    FieldValue value = Calculate8NeighbourhoodValue(loadedData, x, y);
                    result[y, x] = value;
                }
            }

            return result;
        }

        private FieldValue[,] InitializePublicData()
        {
            var data = new FieldValue[Height, Width];
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    data[y, x] = FieldValue.Hidden;
                }
            }
            return data;
        }

        private FieldValue Calculate8NeighbourhoodValue(FieldValue[,] data, uint x, uint y)
        {
            if (data[y, x] == FieldValue.Mine)
            {
                return FieldValue.Mine;
            }

            int mineCount = 0;
            DoFor8Neighbourhood(x, y, (localX, localY) => { if (data[localY, localX] == FieldValue.Mine) mineCount++; });

            return (FieldValue)mineCount;
        }

        private void DoFor8Neighbourhood(uint x, uint y, Action<uint, uint> action)
        {
            for (uint localY = Math.Max(1, y) - 1; localY <= Math.Min(y + 1, Height - 1); ++localY)
            {
                for (uint localX = Math.Max(1, x) - 1; localX <= Math.Min(x + 1, Width - 1); ++localX)
                {
                    if (localX == x && localY == y)
                    {
                        continue;
                    }
                    action(localX, localY);
                }
            }
        }

    }
}
