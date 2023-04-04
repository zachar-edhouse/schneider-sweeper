﻿using Schneider.Sweeper.Map;

namespace Schneider.Sweeper.Engine
{
    internal class Player : IPlayer
    {
        private readonly IMap map;
        private uint endX;
        private readonly ILifecycle lifecycle;

        public uint X { get; private set; }
        public uint Y { get; private set; }

        public uint MoveCount { get; private set; }
        public uint LifeCount { get; private set; }


        public Player(IMap map, ILifecycle lifecycle, uint lifeCount, uint startY)
        {
            if (startY >= map.Height)
            {
                throw new IndexOutOfRangeException("The player cannot start out of the map.");
            }
            if (lifeCount == 0)
            {
                throw new ArgumentException("The number of lifes has to be non-zero.");
            }

            this.map = map;
            this.lifecycle = lifecycle;
            X = 0;
            Y = startY;
            endX = map.Width - 1;
            MoveCount = 0;
            LifeCount = lifeCount;
        }

        public void Move(Direction direction)
        {
            if (lifecycle.CurrentState != LifecycleState.InGame)
            {
                throw new InvalidOperationException("Cannot move when game not started yet.");
            }

            switch (direction)
            {
                case Direction.Left:
                    if (X <= 0)
                        return;
                    X -= 1;
                    break;
                case Direction.Right:
                    if (X >= (map.Width - 1))
                        return;
                    X += 1;
                    break;
                case Direction.Up:
                    if (Y <= 0)
                        return;
                    Y -= 1;
                    break;
                case Direction.Down:
                    if (Y >= (map.Height - 1))
                        return;
                    Y += 1;
                    break;
                default:
                    throw new InvalidOperationException($"Unsupported move direction: {direction}");
            }

            MoveCount++;
            var value = map.Visit(X, Y);
            CheckCollision(value);
            if (X == endX)
            {
                lifecycle.Succeed();
            }
        }

        private void CheckCollision(FieldValue value)
        {
            if (value == FieldValue.Mine)
            {
                LifeCount--;
                if (LifeCount == 0)
                {
                    lifecycle.Die();
                }
            }
        }
    }
}
