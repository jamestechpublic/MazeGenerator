using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MazeGenerator.AppLogic
{
    [Flags]
    internal enum WallState
    {
        LEFT = 1, // 0001
        RIGHT = 2, // 0010
        UP = 4, // 0100
        DOWN = 8, // 1000

        VISITED = 128 // 1000 0000
    }
    // WallState wallstate = WallState.LEFT | WallState.Right;    0011
    // wallstate |= WallState.DOWN;  0111 adds a state
    // wakkstate &= ~WallState.RIGHT; 0101 removes a state by using inverse of right

    internal struct Position
    {
        public int X;
        public int Y;
    }

    internal struct Neighbour
    {
        public Position Position;
        public WallState SharedWall;
    }

    internal class MazeGen
    {
        public WallState[,] Generate(int width, int height)
        {
            WallState[,] maze = new WallState[width, height];
            
            var allWalls = WallState.LEFT | WallState.RIGHT | WallState.DOWN | WallState.UP;
            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                    maze[i, j] = allWalls; // 1111

            // maze[i,j].HasFlag(WallState.Right);

            return ApplyRecursiveBackTracker(maze, width, height);
        }

        private WallState[,] ApplyRecursiveBackTracker(WallState[,] maze, int width, int height)
        {
            var rnd = new System.Random(/* seed */);
            var posStack = new Stack<Position>();
            var pos = new Position { X = rnd.Next(0,width), Y = rnd.Next(0,height) };
            
            maze[pos.X, pos.Y] |= WallState.VISITED; // 1000 1111
            posStack.Push(pos);

            while (posStack.Count > 0)
            {
                var current = posStack.Pop();
                var neighbours = GetUnvisitedNeighbours(pos, maze, width, height);

                if (neighbours.Count > 0)
                {
                    posStack.Push(current); // put back on stack

                    var rndIndex = rnd.Next(0, neighbours.Count); // random unvisited index
                    var randomNeighbour = neighbours[rndIndex]; // random unvisited neighbour
                    var npos = randomNeighbour.Position;
                    maze[current.X, current.Y] &= ~randomNeighbour.SharedWall; // remove wall from current cell
                    maze[npos.X, npos.Y] &= ~GetOppositeWall(randomNeighbour.SharedWall); // remove wall from neighbour
                    maze[npos.X, npos.Y] |= WallState.VISITED; // mark neighbour as visited
                    posStack.Push(npos); // put neighbour onto stack as new position
                }
            }

            return maze;
        }

        private WallState GetOppositeWall(WallState wall)
        {
            switch (wall)
            {
                case WallState.LEFT: return WallState.RIGHT;
                case WallState.RIGHT: return WallState.LEFT;
                case WallState.UP: return WallState.DOWN;
                case WallState.DOWN: return WallState.UP;
                default: return WallState.LEFT;
            }
        }

        private List<Neighbour> GetUnvisitedNeighbours(Position p, WallState[,] maze, int width, int height)
        {
            var list = new List<Neighbour>();

            if (p.X > 0) // LEFT
                if (!maze[p.X - 1, p.Y].HasFlag(WallState.VISITED))
                    list.Add(new Neighbour
                    {
                        Position = new Position { X = p.X - 1, Y = p.Y },
                        SharedWall = WallState.LEFT
                    });

            if (p.Y > 0) // DOWN
                if (!maze[p.X, p.Y - 1].HasFlag(WallState.VISITED))
                    list.Add(new Neighbour
                    {
                        Position = new Position { X = p.X, Y = p.Y - 1 },
                        SharedWall = WallState.DOWN
                    });

            if (p.Y < height - 1) // UP
                if (!maze[p.X, p.Y + 1].HasFlag(WallState.VISITED))
                    list.Add(new Neighbour
                    {
                        Position = new Position { X = p.X, Y = p.Y + 1 },
                        SharedWall = WallState.UP
                    });

            if (p.X < width - 1) // RIGHT
                if (!maze[p.X + 1, p.Y].HasFlag(WallState.VISITED))
                    list.Add(new Neighbour
                    {
                        Position = new Position { X = p.X + 1, Y = p.Y },
                        SharedWall = WallState.RIGHT
                    });

            return list;
        }
    }
}
