using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace ns_SnakeGame
{
    public enum Direction { UP, DOWN, LEFT, RIGHT }
    internal class Snake : IEnumerable<Point>
    {
        readonly LinkedList<Point> bodyCoords;
        readonly Size? fieldSize;
        readonly int startLength;
        event Action<Snake> MoveHeadEvent;
        public LinkedListNode<Point> HeadNode { get => bodyCoords.First; }
        public LinkedListNode<Point> TailNode { get => bodyCoords.Last; }
        public Color Color { get; set; }
        public Snake(Color color, [Range(0, int.MaxValue)] int startLength, [Optional] Size fieldSize)
        {
            this.Color = color;
            this.fieldSize = fieldSize;
            this.startLength = startLength;
            bodyCoords = [];
            this.Reset();
        }
        public void Reset()
        {
            bodyCoords.Clear();
            Point startPoint = this.fieldSize.HasValue ? new(new Random().Next(startLength, this.fieldSize.Value.Height - startLength), new Random().Next(startLength, this.fieldSize.Value.Width - startLength)) : new(1, 1);
            switch (new Random().Next(4))
            {
                case 0:
                    for (int i = 0; i < this.startLength; i++)
                    {
                        bodyCoords.AddLast(new Point(startPoint.X, startPoint.Y + 1));
                    }
                    MoveHeadEvent = MoveHeadRight;
                    break;
                case 1:

                    for (int i = 0; i < this.startLength; i++)
                    {
                        bodyCoords.AddLast(new Point(startPoint.X, startPoint.Y - 1));
                    }
                    MoveHeadEvent = MoveHeadLeft;
                    break;
                case 2:

                    for (int i = 0; i < this.startLength; i++)
                    {
                        bodyCoords.AddLast(new Point(startPoint.X + 1, startPoint.Y));
                    }
                    MoveHeadEvent = MoveHeadUp;
                    break;
                case 3:

                    for (int i = 0; i < this.startLength; i++)
                    {
                        bodyCoords.AddLast(new Point(startPoint.X - 1, startPoint.Y));
                    }
                    MoveHeadEvent = MoveHeadDown;
                    break;
            }
        }
        static readonly Action<Snake> MoveHeadUp = delegate (Snake ths)
        {
            Point headCoords = new(ths.HeadNode.Value.X, ths.HeadNode.Value.Y);
            headCoords.Y -= 1;
            if (ths.fieldSize != null && headCoords.Y < 0)
            {
                headCoords.Y = ths.fieldSize.Value.Height - 1;
            }
            ths.bodyCoords.AddFirst(headCoords);
        };
        static readonly Action<Snake> MoveHeadDown = delegate (Snake ths)
        {
            Point headCoords = new(ths.HeadNode.Value.X, ths.HeadNode.Value.Y);
            headCoords.Y += 1;
            if (headCoords.Y >= ths.fieldSize?.Height)
            {
                headCoords.Y = 0;
            }
            ths.bodyCoords.AddFirst(headCoords);
        };
        static readonly Action<Snake> MoveHeadLeft = delegate (Snake ths)
        {
            Point headCoords = new(ths.HeadNode.Value.X, ths.HeadNode.Value.Y);
            headCoords.X -= 1;
            if (ths.fieldSize != null && headCoords.X < 0)
            {
                headCoords.X = ths.fieldSize.Value.Width - 1;
            }
            ths.bodyCoords.AddFirst(headCoords);
        };
        static readonly Action<Snake> MoveHeadRight = delegate (Snake ths)
        {
            Point headCoords = new(ths.HeadNode.Value.X, ths.HeadNode.Value.Y);
            headCoords.X += 1;
            if (headCoords.X >= ths.fieldSize?.Width)
            {
                headCoords.X = 0;
            }
            ths.bodyCoords.AddFirst(headCoords);
        };
        public void MoveHead()
        {
            MoveHeadEvent.Invoke(this);
        }
        public Point MoveTail()
        {
            Point last = TailNode.Value;
            bodyCoords.RemoveLast();
            return last;
        }
        public void Grow()
        {
            bodyCoords.AddLast(TailNode.Value);
        }
        public void SetDirection(Direction direction)
        {
            switch (direction)
            {
                case Direction.RIGHT:
                    if (HeadNode.Value.Y != HeadNode.Next?.Value.Y)
                    {
                        MoveHeadEvent = MoveHeadRight;
                    }
                    break;
                case Direction.LEFT:
                    if (HeadNode.Value.Y != HeadNode.Next?.Value.Y)
                    {
                        MoveHeadEvent = MoveHeadLeft;
                    }
                    break;
                case Direction.DOWN:
                    if (HeadNode.Value.X != HeadNode.Next?.Value.X)
                    {
                        MoveHeadEvent = MoveHeadDown;
                    }
                    break;
                case Direction.UP:
                    if (HeadNode.Value.X != HeadNode.Next?.Value.X)
                    {
                        MoveHeadEvent = MoveHeadUp;
                    }
                    break;
            }
        }
        public IEnumerator<Point> GetEnumerator()
        {
            return ((IEnumerable<Point>)bodyCoords).GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)bodyCoords).GetEnumerator();
        }
    }
}