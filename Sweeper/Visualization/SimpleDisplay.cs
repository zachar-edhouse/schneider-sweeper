using Schneider.Sweeper.Engine;
using Schneider.Sweeper.Map;
using System.Text;

namespace Schneider.Sweeper.Visualization
{
    internal class SimpleDisplay : IDisplay
    {
        private readonly IOutput output;
        private readonly IMap map;
        private readonly IPlayer player;
        private readonly ILifecycle lifecycle;

        public SimpleDisplay(IOutput output, IMap map, IPlayer player, ILifecycle lifecycle)
        {
            this.output = output;
            this.map = map;
            this.player = player;
            this.lifecycle = lifecycle;
        }

        public void Draw()
        {
            switch (lifecycle.CurrentState)
            {
                case LifecycleState.Initializing:
                    DrawInitScreen();
                    break;
                case LifecycleState.InGame:
                    DrawInGameScreen();
                    break;
                case LifecycleState.Death:
                case LifecycleState.Success:
                    DrawInGameScreen();
                    DrawExitInfo();
                    break;
            }
        }

        private void DrawInitScreen()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine();
            sb.AppendLine("MINESWEEPER WALKING GAME");
            sb.AppendLine();
            sb.AppendLine("Welcome to the game. The board looks just like the famous Minesweeper board. " +
                "The goal is to get from one side of the board to the other while avoiding hidden mines and minimizing moves count.");
            sb.AppendLine();
            sb.AppendLine("Control:");
            sb.AppendLine("  Arrows for movement");
            sb.AppendLine("  Esc for exit");
            sb.AppendLine();
            sb.AppendLine("Now press any key to start the game...");

            output.Clear();
            output.Write(sb.ToString());
        }

        private void DrawInGameScreen()
        {
            StringBuilder sb = new StringBuilder();
            bool revert = false;
            output.Clear();

            for (uint y = 0; y < map.Height; ++y)
            {
                for (uint x = 0; x < map.Width; ++x)
                {
                    if (player.X == x && player.Y == y)
                    {
                        revert = true;
                        output.Write(sb.ToString());
                        sb.Clear();
                        output.EnableHighlighting();
                    }
                    var value = map[x, y];
                    if (value == FieldValue.Empty)
                        sb.Append('.');
                    else if ((int)value > 0 && (int)value < 9)
                        sb.Append((int)value);
                    else if (value == FieldValue.Hidden)
                        sb.Append((char)1);
                    else if (value == FieldValue.Mine)
                        sb.Append('X');

                    if (revert)
                    {
                        revert = false;
                        output.Write(sb.ToString());
                        sb.Clear();
                        output.DisableHighlighting();
                    }
                }
                sb.AppendLine();
            }

            sb.AppendLine();
            sb.AppendLine("Player:");
            sb.AppendLine($"    Position: [{player.X}, {player.Y}]");
            sb.AppendLine($"    Life count: {player.LifeCount}");
            sb.AppendLine($"    Move count: {player.MoveCount}");

            output.WriteLine(sb.ToString());
        }

        private void DrawExitInfo()
        {
            output.WriteLine();
            if (lifecycle.CurrentState == LifecycleState.Death)
            {
                output.WriteLine("GAME OVER.");
                output.WriteLine("Press any key to exit.");
            }
            else if (lifecycle.CurrentState == LifecycleState.Success)
            {
                output.WriteLine($"GAME FINISHED. Your score is {player.MoveCount}.");
                output.WriteLine("Press any key to exit.");
            }
        }
    }
}
