using System.Net;
using System.Runtime.InteropServices;
using Timer = System.Timers.Timer;

namespace ns_SnakeGame
{
    internal record SnakeClient(Snake Snake, Timer Timer, [Optional] IPEndPoint Controller)
    {
        readonly Snake snake = Snake;
        readonly Timer timer = Timer;
        readonly IPEndPoint? controller = Controller;
        public Snake Snake { get => snake; }
        public Timer Timer { get => timer; }
        public IPEndPoint? Controller { get => controller; }
    }
}
