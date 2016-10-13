using System;
using System.Drawing;

namespace func_rocket
{
	public class GameSpace
	{
		public GameSpace(string name, Rocket rocket, Vector target, Func<Vector, Vector> gravity, Func<float, float> wall)
		{
			this.name = name;
			Rocket = rocket;
			Target = target;
			Gravity = gravity;
            Wall = wall;
		}

        public Func<float, float> Wall;
		private readonly string name;
		public Rocket Rocket;
		public Vector Target;

		public override string ToString()
		{
			return name;
		}

		public Func<Vector, Vector> Gravity { get; private set; }

		public void Move(Rectangle spaceRect, Turn turnRate)
		{
			Rocket.Direction += (int)turnRate * 0.08;
			var direction = new Vector(Math.Cos(Rocket.Direction), Math.Sin(Rocket.Direction));
			var force = direction + Gravity(Rocket.Location);
			Rocket.Velocity = Rocket.Velocity + force;
			if (Rocket.Velocity.Length > 20) Rocket.Velocity = Rocket.Velocity * (10 / Rocket.Velocity.Length);
            if (Math.Abs(Wall((float)(Rocket.Location + Rocket.Velocity * 0.5).X) - (Rocket.Location + Rocket.Velocity * 0.5).Y) < 7) 
                return;
			Rocket.Location = Rocket.Location + Rocket.Velocity*0.5;
            var locationX = Rocket.Location.X % spaceRect.Width;
            if (locationX < 0) locationX += spaceRect.Width;
            var locationY = Rocket.Location.Y % spaceRect.Height;
            if (locationY < 0) locationY += spaceRect.Height;
            Rocket.Location = new Vector(locationX, locationY);
		}
	}
}