using System;

namespace Mazes
{
	public static class MazeTasks
	{
		public static void MoveOutFromEmptyMaze(Robot robot, int width, int height)
		{
            for (int i = 0; i < height-3; i++)
            {
                robot.MoveTo(Direction.Down);
                if (i != height-4)robot.MoveTo(Direction.Right);
            }
		}

        public static void MoveOutFromDiagonalMaze(Robot robot, int width, int height)
        {
            int xOfEnd = width - 2, yOfEnd = height - 2;
            while (robot.Pos.X != xOfEnd || robot.Pos.Y != yOfEnd)
            {
                if (height > width) {
                    robot.MoveTo(Direction.Down);
                    robot.MoveTo(Direction.Down);
                    if (robot.Pos.Y != yOfEnd) robot.MoveTo(Direction.Right);
                } else {
                    robot.MoveTo(Direction.Right);
                    robot.MoveTo(Direction.Right);
                    robot.MoveTo(Direction.Right);
                    if (robot.Pos.Y != yOfEnd) robot.MoveTo(Direction.Down);
                }
            }
        }

		public static void MoveOutFromSnakeMaze(Robot robot, int width, int height)
		{
            SomeMoves(Direction.Down, robot, width, height-2, 0);
		}

        public static void MoveOutFromPyramidMaze(Robot robot, int width, int height)
        {
            SomeMoves(Direction.Up, robot, width, 1, 2);
        }

        public static void SomeMoves(Direction dir, Robot robot, int width, int tb, int s)
        {
            int side = 1, rightSide = width - 2, leftSide = 1;
            while (true)
            {
                if (robot.Pos.X == leftSide && robot.Pos.Y == tb) break;
                if (side == -1) robot.MoveTo(Direction.Left);
                else robot.MoveTo(Direction.Right);
                if (robot.Pos.X == leftSide && robot.Pos.Y != tb || robot.Pos.X == rightSide)
                {
                    robot.MoveTo(dir);
                    robot.MoveTo(dir);
                    if (robot.Pos.X == rightSide) leftSide += s;
                    else rightSide -= s;
                    side = -side;
                }
            }
        }
	}
}