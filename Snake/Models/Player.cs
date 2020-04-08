using System.Collections.Generic;
using System.Windows.Shapes;
using Snake.Others;

namespace Snake.Models
{
    public class Player
    {
        public double X { get; private set; }
        public double Y { get; private set; }
        public LinkedList<Rectangle> SnakeQueue { get; set; }

        public Direction LastMove { get; set; } = Direction.Right;


        public void MovePlayer()
        {
            switch (LastMove)
            {
                case Direction.Left:
                    X -= SnakeQueue.First.Value.Width;
                    break;
                case Direction.Right:
                    X += SnakeQueue.First.Value.Width;
                    break;
                case Direction.Up:
                    Y -= SnakeQueue.First.Value.Height;
                    break;
                case Direction.Down:
                    Y += SnakeQueue.First.Value.Height;
                    break;
                default:
                    X += SnakeQueue.First.Value.Width;
                    break;
            }
        }
    }
}