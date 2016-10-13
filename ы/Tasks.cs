using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace func_rocket
{
	public class Tasks
	{
		public static IEnumerable<GameSpace> GetGameSpaces()
		{
            yield return new GameSpace("zero-gravity", new Rocket(new Vector(50, 700), Vector.Zero, -1.2 * Math.PI), new Vector(800, 100), ������ => Vector.Zero, x => 110000 / x);
            yield return new GameSpace("heavy-gravity", new Rocket(new Vector(50, 700), Vector.Zero, 1.2 * Math.PI), new Vector(800, 100), ������ => new Vector(0, 0.9), x => 1 / x);
            var ���� = new Vector(800, 100);
            yield return new GameSpace("white-hole", new Rocket(new Vector(800, 700), Vector.Zero, 1.2 * Math.PI), ����, ������ =>
            {
                var ����������� = (������ - ����);
                var ������ = (50 * �����������.Length) / ((�����������.Length + 1) * (�����������.Length + 1));
                var ����� = ������ / �����������.Length;
                var ��������� = new Vector(�����������.X * �����, �����������.Y * �����);
                return ���������;
            }, x => 1/x);
            yield return new GameSpace("anomaly", new Rocket(new Vector(50, 700), Vector.Zero, 1.2 * Math.PI), new Vector(800, 100), ������ => 
            {
                var ���� = Math.PI * DateTime.Now.Millisecond / 500.0;
                return new Vector(Math.Cos(����), Math.Sin(����));
            }, x => 1 / x);
            var rand = new Random();
            yield return new GameSpace("pulse", new Rocket(new Vector(800, 700), Vector.Zero, 1.2 * Math.PI), ����, ������ =>
            {
                var ���� = Math.PI * DateTime.Now.Millisecond / 500.0;
                var ����������� = (������ - ����);
                var ������ = (50 * �����������.Length) / ((�����������.Length + 1) * (�����������.Length + 1));
                var ����������� = DateTime.Now.Millisecond - 100;
                if (����������� < 450) ����������� = 900 - �����������;
                var ����������� = ������ * (����������� - rand.Next(4)) / 100 / �����������.Length;
                var ��������� = new Vector(�����������.X * ����������� * Math.Cos(����), �����������.Y * ����������� * Math.Sin(����));
                return ���������;
            }, x => 1 / x);
		}

		public static Turn ControlRocket(Rocket ������, Vector ����)
		{
            var �������������������� = ���� - ������.Location;
            var ��������������������� = Math.Asin(��������������������.Y/��������������������.Length);
            if (��������������������.X < 0 && ��������������������.Y > 0) ��������������������� += Math.PI;
            else if (��������������������.X < 0 && ��������������������.Y < 0) ��������������������� -= Math.PI;
            if (��������������������� < ������.Direction % (2*Math.PI)) return Turn.Left;
            else return Turn.Right;
		}
	}
}