using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace func_rocket
{
	public class Tasks
	{
		public static IEnumerable<GameSpace> GetGameSpaces()
		{
            yield return new GameSpace("zero-gravity", new Rocket(new Vector(50, 700), Vector.Zero, -1.2 * Math.PI), new Vector(800, 100), балбес => Vector.Zero, x => 110000 / x);
            yield return new GameSpace("heavy-gravity", new Rocket(new Vector(50, 700), Vector.Zero, 1.2 * Math.PI), new Vector(800, 100), балбес => new Vector(0, 0.9), x => 1 / x);
            var дыра = new Vector(800, 100);
            yield return new GameSpace("white-hole", new Rocket(new Vector(800, 700), Vector.Zero, 1.2 * Math.PI), дыра, балбес =>
            {
                var направление = (балбес - дыра);
                var модуль = (50 * направление.Length) / ((направление.Length + 1) * (направление.Length + 1));
                var чтото = модуль / направление.Length;
                var результат = new Vector(направление.X * чтото, направление.Y * чтото);
                return результат;
            }, x => 1/x);
            yield return new GameSpace("anomaly", new Rocket(new Vector(50, 700), Vector.Zero, 1.2 * Math.PI), new Vector(800, 100), балбес => 
            {
                var угол = Math.PI * DateTime.Now.Millisecond / 500.0;
                return new Vector(Math.Cos(угол), Math.Sin(угол));
            }, x => 1 / x);
            var rand = new Random();
            yield return new GameSpace("pulse", new Rocket(new Vector(800, 700), Vector.Zero, 1.2 * Math.PI), дыра, балбес =>
            {
                var угол = Math.PI * DateTime.Now.Millisecond / 500.0;
                var направление = (балбес - дыра);
                var модуль = (50 * направление.Length) / ((направление.Length + 1) * (направление.Length + 1));
                var милисекунды = DateTime.Now.Millisecond - 100;
                if (милисекунды < 450) милисекунды = 900 - милисекунды;
                var коэффициент = модуль * (милисекунды - rand.Next(4)) / 100 / направление.Length;
                var результат = new Vector(направление.X * коэффициент * Math.Cos(угол), направление.Y * коэффициент * Math.Sin(угол));
                return результат;
            }, x => 1 / x);
		}

		public static Turn ControlRocket(Rocket балбес, Vector дыра)
		{
            var векторќтЅалбеса ƒыре = дыра - балбес.Location;
            var угол¬ектораЅалбесƒыра = Math.Asin(векторќтЅалбеса ƒыре.Y/векторќтЅалбеса ƒыре.Length);
            if (векторќтЅалбеса ƒыре.X < 0 && векторќтЅалбеса ƒыре.Y > 0) угол¬ектораЅалбесƒыра += Math.PI;
            else if (векторќтЅалбеса ƒыре.X < 0 && векторќтЅалбеса ƒыре.Y < 0) угол¬ектораЅалбесƒыра -= Math.PI;
            if (угол¬ектораЅалбесƒыра < балбес.Direction % (2*Math.PI)) return Turn.Left;
            else return Turn.Right;
		}
	}
}