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
        readonly List<FieldDataUnit> packageData;
        readonly Action printFieldForHost;
        readonly Action<string> chatMessageForHost;
        readonly Action scoreMessageForHost;
        readonly Action deathMessageForHost;
        public List<SnakeClient> Players { get; }
        Point foodCoords;
        public UdpClient Host { get; }
        public int PlayerCount { get => Players.Count; }
        public Snake HostSnake { get => Players[0].Snake; }
        public SnakeGame(Action printFieldForHostFunc, Action<string> chatMessageForHostFunc, Action scoreMessageForHostFunc, Action deathMessageForHostFunc, Color[,] field, Color[] fieldColors, Color foodColor, UdpClient host, string hostName, Color hostSnakeColor)
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
            this.printFieldForHost = printFieldForHostFunc;
            this.chatMessageForHost = chatMessageForHostFunc;
            this.scoreMessageForHost = scoreMessageForHostFunc;
            this.deathMessageForHost = deathMessageForHostFunc;
            SpawnFood();
            Task.Run(() =>
            {
                this.AddPlayer((IPEndPoint)host.Client.LocalEndPoint, hostName, hostSnakeColor);
                printFieldForHost();
                scoreMessageForHost();
            });
        }
        void DieIfCollision(SnakeClient snakeClient)
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
                    Host.SendAsync(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(new CommunicationUnit("You died"))), snakeClient.Controller);
                }
                SendChatMessageToAll($"☠️ {snakeClient.Nickname} died.");
            }
        }
        public void GiveUp(SnakeClient snakeClient)
        {
            Task.Run(() => DieForcefully(snakeClient));
            SendChatMessageToAll($"☠️ {snakeClient.Nickname} gave up.");
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
            int fadeSteps = 5;
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
                Thread.Sleep(250);
            }
        }
        void SpawnFoodIfOneConsumedBy(SnakeClient snakeClient)
        {
            if (snakeClient.Snake.HeadNode.Value == foodCoords)
            {
                snakeClient.Snake.Grow();
                snakeClient.Timer.Interval *= 0.98;
                SpawnFood();
                if (snakeClient == Players[0])
                {
                    Task.Run(() =>
                    {
                        scoreMessageForHost();
                    });
                }
                else
                {
                    Host.SendAsync(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(new CommunicationUnit("New score") { Attachment = snakeClient.Snake.Count() })), snakeClient.Controller);
                }
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
                foreach (FieldDataUnit package in packageData)
                {
                    field[package.X, package.Y] = package.Color;
                }
                message.Attachment = packageData.ToList();
            }
            string json = JsonSerializer.Serialize(message, FieldDataUnit.SerializerOptions);
            foreach (IPEndPoint remote in Players.Skip(1).Select(x => x.Controller))
            {
                try
                {
                    Host.SendAsync(Encoding.UTF8.GetBytes(json), remote);
                }
                catch
                {
                    RemovePlayer(remote);
                }
            }
            printFieldForHost();
            packageData.Clear();
        }
        public void AddPlayer(IPEndPoint controller, string name, Color snakeColor)
        {
            SnakeClient player = new(name, DefaultSnake(snakeColor), new Timer(), controller);
            Players.Add(player);
            // todo add snake start pos to packageData
            // todo add snake start collisions
            List<FieldDataUnit> currentFieldStateData = [];
            for (int i = 0; i < collisionField.GetLength(0); i++)
            {
                for (int j = 0; j < collisionField.GetLength(1); j++)
                {
                    currentFieldStateData.Add(new(i, j, field[i, j]));
                }
            }
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
            player.Timer.Elapsed += (sender, e) => DieIfCollision(player);
            player.Timer.Elapsed += (sender, e) => SpawnFoodIfOneConsumedBy(player);
            player.Timer.Elapsed += (sender, e) => ChangeFieldAndPrint();
            player.Timer.AutoReset = true;
            AwakeSnake(player, false);
            if (Players.Count == 1)
            {
                CommunicationUnit message = new("Field data") { Attachment = currentFieldStateData };
                string fieldJson = JsonSerializer.Serialize(message, FieldDataUnit.SerializerOptions);
                Host.SendAsync(Encoding.UTF8.GetBytes(fieldJson), controller);
                chatMessageForHost($"➡️ {name} started the server.");
            }
            else
            {
                SendChatMessageToAll($"➡️ {name} joined.");
            }
        }
        public void RemovePlayer(IPEndPoint controller)
        {
            SnakeClient? player = PlayerByRemote(controller);
            if (player != null) {
                DieForcefully(player);
                player.Timer.Close();
                Players.Remove(player);
                SendChatMessageToAll($"⬅ {player.Nickname} left.");
            }
        }
        public void AwakeSnake(SnakeClient client, bool revive)
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
                SendChatMessageToAll($"✨ {client.Nickname} revived.");
            }
            client.Timer.Start();
            if (client == Players[0])
            {
                scoreMessageForHost();
            }
            else
            {
                Host.SendAsync(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(new CommunicationUnit("New score") { Attachment = client.Snake.Count() })), client.Controller);
            }
        }
        public void SendChatMessageToAll(string message)
        {
            CommunicationUnit messageUnit = new("Message") { Attachment = message };
            foreach (IPEndPoint r in Players.Skip(1).Select(x => x.Controller))
            {
                Host.SendAsync(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(messageUnit)), r);
            }
            chatMessageForHost(message);
        }
        public void SetSnakeDirection(IPEndPoint remote, Direction direction)
        {
            PlayerByRemote(remote)?.Snake.SetDirection(direction);
        }
        public SnakeClient? PlayerByRemote(IPEndPoint remote)
        {
            return Players.Find(x => x.Controller?.Address.ToString() == remote.Address.ToString() && x.Controller.Port == remote.Port);
        }
        Snake DefaultSnake(Color color)
        {
            return new Snake(color, 2, new Size(collisionField.GetLength(0), collisionField.GetLength(1)));
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
                if (disposing)
                {
                    timer.Close();
                }
            }
        }
    }
}