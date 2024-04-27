using System.Net;
using System.Text.Json;
using System.Text;
using System.Net.Sockets;
using ns_Data;
using Timer = System.Timers.Timer;

namespace ns_SnakeGame
{
    internal class SnakeGame : IDisposable
    {
        readonly bool[,] collisionField;
        readonly Color foodColor;
        readonly Color[] fieldColors;
        readonly Color[,] field;
        readonly List<PackageData> packageData;
        readonly Action printFieldForHost;
        readonly Action deathMessageForHost;
        public readonly List<SnakeClient> Players;
        Point foodCoords;
        public UdpClient Host { get; }
        public int PlayerCount { get => Players.Count; }
        public Snake HostSnake { get => Players[0].Snake; }
        public SnakeGame(Action printFieldForHostFunc, Action deathMessageForHostFunc, Color[,] field, Color[] fieldColors, Color foodColor, UdpClient host, Color hostSnakeColor)
        {
            this.field = field;
            this.fieldColors = fieldColors;
            this.foodColor = foodColor;
            this.collisionField = new bool[field.GetLength(0), field.GetLength(1)];
            this.packageData = [];
            this.Players = [];
            for (int i = 0; i < field.GetLength(0); i++)
            {
                for (int j = 0; j < field.GetLength(1); j++)
                {
                    this.field[i, j] = FieldColor(i, j);
                }
            }
            this.Host = host;
            SnakeClient hostClient = new(DefaultSnake(hostSnakeColor), new Timer());
            this.AddPlayer((IPEndPoint)host.Client.LocalEndPoint, hostSnakeColor);
            // todo add snake start pos to packageData
            // todo add snake start collisions
            this.printFieldForHost = printFieldForHostFunc;
            printFieldForHost();
            this.deathMessageForHost = deathMessageForHostFunc;
            SpawnFood();
        }
        Snake DefaultSnake(Color color)
        {
            return new Snake(color, 2, new Size(collisionField.GetLength(0), collisionField.GetLength(1)));
        }
        void DieIfDead(SnakeClient snakeClient)
        {
            if (collisionField[snakeClient.Snake.HeadNode.Value.Y, snakeClient.Snake.HeadNode.Value.X])
            {
                Task.Run(() => DieForcefully(snakeClient));
                if (snakeClient == Players[0])
                {
                    Task.Run(() =>
                    {
                        deathMessageForHost();
                    });
                }
                else
                {
                    Host?.SendAsync(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(new CommunicationUnit("You died"))), snakeClient.Controller);
                }
            }
        }
        void DieForcefully(SnakeClient snakeClient)
        {
            if (!snakeClient.Timer.Enabled)
            {
                return;
            }
            snakeClient.Timer.Stop();
            snakeClient.Timer.Enabled = false;
            foreach (Point coord in snakeClient.Snake)
            {
                collisionField[coord.Y, coord.X] = false;
            }
            int fadeSteps = 4;
            for (int i = fadeSteps - 1; i >= 0; i--)
            {
                lock (packageData)
                {
                    foreach (Point coord in snakeClient.Snake)
                    {
                        Color deadBodyColor = Color.FromArgb
                        (
                            snakeClient.Snake.Color.R / fadeSteps * i + FieldColor(coord.X, coord.Y).R / fadeSteps * (fadeSteps - i),
                            snakeClient.Snake.Color.G / fadeSteps * i + FieldColor(coord.X, coord.Y).G / fadeSteps * (fadeSteps - i),
                            snakeClient.Snake.Color.B / fadeSteps * i + FieldColor(coord.X, coord.Y).B / fadeSteps * (fadeSteps - i)
                        );
                        packageData.Add(new(coord.X, coord.Y, deadBodyColor));
                    }
                }
                ChangeFieldAndPrint();
                Thread.Sleep(200);
            }
        }
        void SpawnFoodIfOneConsumedBy(SnakeClient snakeClient)
        {
            if (snakeClient.Snake.HeadNode.Value == foodCoords)
            {
                snakeClient.Snake.Grow();
                snakeClient.Timer.Interval *= 0.95;
                SpawnFood();
            }
        }
        void SpawnFood()
        {
            Random rand = new();
            int food_x, food_y;
            do
            {
                food_x = rand.Next(0, collisionField.GetLength(1));
                food_y = rand.Next(0, collisionField.GetLength(0));
            }
            while (collisionField[food_y, food_x]);
            foodCoords = new(food_x, food_y);
            lock (packageData)
            {
                packageData.Add(new(foodCoords.X, foodCoords.Y, foodColor));
            }
        }
        void ChangeFieldAndPrint()
        {
            CommunicationUnit message = new("Field data");
            lock (packageData)
            {
                foreach (PackageData package in packageData)
                {
                    field[package.X, package.Y] = package.Color;
                }
                message.Attachment = packageData.ToList();
            }
            string json = JsonSerializer.Serialize(message, PackageData.SerializerOptions);
            foreach (IPEndPoint player in Players.Skip(1).Select(x => x.Controller))
            {
                // todo make sends-recieves a separate tasks for async awaiting responses\try{
                Host?.SendAsync(Encoding.UTF8.GetBytes(json), player);
                // todo remove endpoint if no response
            }
            printFieldForHost();
            packageData.Clear();
        }
        public void AddPlayer(IPEndPoint controller, Color snakeColor)
        {
            SnakeClient player = new(DefaultSnake(snakeColor), new Timer(), controller);
            Players.Add(player);
            List<PackageData> currentFieldStateData = [];
            for (int i = 0; i < collisionField.GetLength(0); i++)
            {
                for (int j = 0; j < collisionField.GetLength(1); j++)
                {
                    currentFieldStateData.Add(new(i, j, field[i, j]));
                }
            }
            CommunicationUnit message = new("Field data") { Attachment = currentFieldStateData };
            string fieldJson = JsonSerializer.Serialize(message, PackageData.SerializerOptions);
            Host?.SendAsync(Encoding.UTF8.GetBytes(fieldJson), controller);
            player.Timer.Interval = 135;
            player.Timer.Elapsed += (sender, e) =>
            {
                Point last = player.Snake.MoveTail();
                collisionField[last.Y, last.X] = false;
                lock (packageData)
                {
                    packageData.Add(new(last.X, last.Y, FieldColor(last.X, last.Y)));
                }
            };
            player.Timer.Elapsed += (sender, e) =>
            {
                player.Snake.MoveHead();
                collisionField[player.Snake.HeadNode.Next.Value.Y, player.Snake.HeadNode.Next.Value.X] = true;
                lock (packageData)
                {
                    packageData.Add(new(player.Snake.HeadNode.Value.X, player.Snake.HeadNode.Value.Y, player.Snake.Color));
                }
            };
            player.Timer.Elapsed += (sender, e) => DieIfDead(player);
            player.Timer.Elapsed += (sender, e) => SpawnFoodIfOneConsumedBy(player);
            player.Timer.Elapsed += (sender, e) => ChangeFieldAndPrint();
            player.Timer.AutoReset = true;
            // todo expect OK acknowledgement
            AwakeSnake(player, false);
        }
        public void RemovePlayer(IPEndPoint controller)
        {
            SnakeClient? player = PlayerByRemote(controller);
            if (player != null) {
                DieForcefully(player);
                player.Timer.Close();
                Players.Remove(player);
            }
        }
        public static void AwakeSnake(SnakeClient client, bool revive)
        {
            if (client.Timer.Enabled)
            {
                return;
            }
            client.Timer.Enabled = true;
            client.Timer.Interval = 135;
            if (revive)
            {
                client.Snake.Reset();
            }
            client.Timer.Start();
        }
        public void AcceptSetSnakeDirectionSignal(IPEndPoint remote, Direction direction)
        {
            PlayerByRemote(remote)?.Snake.SetDirection(direction);
        }
        // todo Ping func for getting ping (for future interface)
        public SnakeClient? PlayerByRemote(IPEndPoint remote)
        {
            return Players.Find(x => x.Controller?.Address.ToString() == remote.Address.ToString() && x.Controller.Port == remote.Port);
        }
        Color FieldColor(int col, int row)
        {
            return fieldColors[(col * collisionField.GetLength(0) + row + col % fieldColors.Length) % fieldColors.Length];
        }
        public void Stop()
        {
            Stop(false);
        }
        public void Dispose()
        {
            Stop(true);
        }
        void Stop(bool disposing)
        {
            foreach (Timer timer in Players.Select(x => x.Timer))
            {
                timer.Stop();
                // todo send GAME STOPPED acknowledgement
                if (disposing)
                {
                    timer.Close();
                }
            }
        }
    }
}