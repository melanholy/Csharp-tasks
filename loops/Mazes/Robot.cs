using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Mazes
{
	public enum Direction
	{
		Up,
		Down,
		Left,
		Right
	}

	public class Robot
	{
		private readonly Maze maze;
		private readonly List<Point> path = new List<Point>();

		public Robot(Maze maze)
		{
			this.maze = maze;
			path.Add(maze.Robot);
		}

		public IEnumerable<Point> Path { get { return path; } }

		public Point Pos
		{
			get { return path.Last(); }
		}

		public void MoveTo(Direction dir)
		{
			int[] dxs = { 0, 0, -1, 1 };
			int[] dys = { -1, 1, 0, 0 };
			TryMoveTo(new Point(
				Pos.X + dxs[(int)dir], 
				Pos.Y + dys[(int)dir]));
		}

		private void TryMoveTo(Point destination)
		{
			if (path.Count > 1000 || maze.IsWall(destination))
				throw new Exception("Robot is broken!");
			else path.Add(destination);
		}
	}
}