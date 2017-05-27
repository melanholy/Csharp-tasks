using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace func_rocket
{
	public class Tasks
	{
		public static IEnumerable<GameSpace> GetGameSpaces()
		{
            yield return new GameSpace("zero-gravity", new Rocket(new Vector(50, 700), Vector.Zero, -1.2 * Math.PI), new Vector(800, 100), áàëáåñ => Vector.Zero, x => 110000 / x);
            yield return new GameSpace("heavy-gravity", new Rocket(new Vector(50, 700), Vector.Zero, 1.2 * Math.PI), new Vector(800, 100), áàëáåñ => new Vector(0, 0.9), x => 1 / x);
            var äûðà = new Vector(800, 100);
            yield return new GameSpace("white-hole", new Rocket(new Vector(800, 700), Vector.Zero, 1.2 * Math.PI), äûðà, áàëáåñ =>
            {
                var íàïðàâëåíèå = (áàëáåñ - äûðà);
                var ìîäóëü = (50 * íàïðàâëåíèå.Length) / ((íàïðàâëåíèå.Length + 1) * (íàïðàâëåíèå.Length + 1));
                var ÷òîòî = ìîäóëü / íàïðàâëåíèå.Length;
                var ðåçóëüòàò = new Vector(íàïðàâëåíèå.X * ÷òîòî, íàïðàâëåíèå.Y * ÷òîòî);
                return ðåçóëüòàò;
            }, x => 1/x);
            yield return new GameSpace("anomaly", new Rocket(new Vector(50, 700), Vector.Zero, 1.2 * Math.PI), new Vector(800, 100), áàëáåñ => 
            {
                var óãîë = Math.PI * DateTime.Now.Millisecond / 500.0;
                return new Vector(Math.Cos(óãîë), Math.Sin(óãîë));
            }, x => 1 / x);
            var rand = new Random();
            yield return new GameSpace("pulse", new Rocket(new Vector(800, 700), Vector.Zero, 1.2 * Math.PI), äûðà, áàëáåñ =>
            {
                var óãîë = Math.PI * DateTime.Now.Millisecond / 500.0;
                var íàïðàâëåíèå = (áàëáåñ - äûðà);
                var ìîäóëü = (50 * íàïðàâëåíèå.Length) / ((íàïðàâëåíèå.Length + 1) * (íàïðàâëåíèå.Length + 1));
                var ìèëèñåêóíäû = DateTime.Now.Millisecond - 100;
                if (ìèëèñåêóíäû < 450) ìèëèñåêóíäû = 900 - ìèëèñåêóíäû;
                var êîýôôèöèåíò = ìîäóëü * (ìèëèñåêóíäû - rand.Next(4)) / 100 / íàïðàâëåíèå.Length;
                var ðåçóëüòàò = new Vector(íàïðàâëåíèå.X * êîýôôèöèåíò * Math.Cos(óãîë), íàïðàâëåíèå.Y * êîýôôèöèåíò * Math.Sin(óãîë));
                return ðåçóëüòàò;
            }, x => 1 /x);
		}

		public static Turn ControlRocket(Rocket áàëáåñ, Vector äûðà)
		{
            var âåêòîðÎòÁàëáåñàÊÄûðå = äûðà - áàëáåñ.Location;
            var óãîëÂåêòîðàÁàëáåñÄûðà = Math.Asin(âåêòîðÎòÁàëáåñàÊÄûðå.Y/âåêòîðÎòÁàëáåñàÊÄûðå.Length);
            if (âåêòîðÎòÁàëáåñàÊÄûðå.X < 0 && âåêòîðÎòÁàëáåñàÊÄûðå.Y > 0) óãîëÂåêòîðàÁàëáåñÄûðà += Math.PI;
            else if (âåêòîðÎòÁàëáåñàÊÄûðå.X < 0 && âåêòîðÎòÁàëáåñàÊÄûðå.Y < 0) óãîëÂåêòîðàÁàëáåñÄûðà -= Math.PI;
            if (óãîëÂåêòîðàÁàëáåñÄûðà < áàëáåñ.Direction % (2*Math.PI)) return Turn.Left;
            else return Turn.Right;
		}
	}
}
