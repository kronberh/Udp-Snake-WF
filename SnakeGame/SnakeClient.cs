using System.Net;
using System.Runtime.InteropServices;
using Timer = System.Timers.Timer;

namespace ns_SnakeGame
{
    internal record SnakeClient(string name, Snake Snake, Timer Timer, [Optional] IPEndPoint Controller)
    {
        readonly string name = name;
        readonly Snake snake = Snake;
        readonly Timer timer = Timer;
        readonly IPEndPoint controller = Controller;
        public string Nickname { get => name; }
        public Snake Snake { get => snake; }
        public Timer Timer { get => timer; }
        public IPEndPoint? Controller { get => controller; }
    }
}
