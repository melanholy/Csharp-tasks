using System;
using System.Linq;

namespace Recognizer
{
	class Tasks
	{
		public static double[,] Grayscale(byte[, ,] original)
		{
            var result = new double[original.GetLength(0), original.GetLength(1)];
            for (int x = 0; x < original.GetLength(0); x++)
                for (int y = 0; y < original.GetLength(1); y++)
                    result[x, y] = original[x, y, 0] * 0.299 / 256 + original[x, y, 1] * 0.587 / 256 + original[x, y, 2] * 0.114 / 256;
            return result;
		}

		public static void ClearNoise(byte[, ,] original)
		{
            for (int x = 0; x < original.GetLength(0); x++)
                for (int y = 0; y < original.GetLength(1); y++)
                    if (original[x, y, 0]==original[x, y, 1] && original[x, y, 1]==original[x, y, 2] && (original[x, y, 2]==0 || original[x, y, 2]==255))
                    {
                        if (x == 0 && y == 0) GetColor(1, 0, 1, 0, original, x, y);
                        else if (x == 0) GetColor(1, 0, 1, 1, original, x, y);
                        else if (y == 0) GetColor(1, 1, 1, 0, original, x, y);
                        else if (x == original.GetLength(0) - 1 && y == original.GetLength(1) - 1) GetColor(0, 1, 0, 1, original, x, y);
                        else if (x == original.GetLength(0) - 1) GetColor(0, 1, 1, 1, original, x, y);
                        else if (y == original.GetLength(1) - 1) GetColor(1, 1, 0, 1, original, x, y);
                        else GetColor(1, 1, 1, 1, original, x, y);
                    }
		}

        static void GetColor(int dx1, int dx2, int dy1, int dy2, byte[, ,] original, int x, int y)
        {
            for (var k = 0; k < 3; k++)
            {
                int i = 0, o = 0;
                if (original[x + dx1, y, k] != 0 && original[x + dx1, y, k] != 255)
                {
                    o = original[x + dx1, y, k];
                    i++;
                }
                if (original[x - dx2, y, k] != 0 && original[x - dx2, y, k] != 255)
                {
                    o += original[x - dx2, y, k];
                    i++;
                }
                if (original[x, y + dy1, k] != 0 && original[x, y + dy1, k] != 255)
                {
                    o += original[x, y + dy1, k];
                    i++;
                }
                if (original[x, y - dy2, k] != 0)
                {
                    o += original[x, y - dy2, k];
                    i++;
                }
                if (i!=0) original[x, y, k] = (byte)(o/i);
            }
        }

		public static void ThresholdFiltering(double[,] original)
		{
            var i = 0;
            var values = new double[original.Length];
            for (int x = 0; x < original.GetLength(0); x++)
                for (int y = 0; y < original.GetLength(1); y++)
                {
                    values[i++] = original[x, y];
                }
            var threshold = values.OrderBy(x => x).ToArray()[values.Length * 9 / 10];
            for (int x = 0; x < original.GetLength(0); x++)
                for (int y = 0; y < original.GetLength(1); y++)
                    if (original[x, y] >= threshold) original[x, y] = 1.0;
                    else original[x,y]=0.0;
		}

		public static double[,] SobelFiltering(double[,] greyscale)
        {
            int width = greyscale.GetLength(0), height = greyscale.GetLength(1);
            var result = new double[width, height];
            var size = 21;
            int c = (int)(size / 2.0), b = (int)Math.Ceiling(size / 2.0);
            for (int x = size - b; x < width - b; x++)
                for (int y = size - b; y < height - b; y++)
                {
                    double gx = 0.0, gy = 0.0, multiplier = 0.0;
                    for (var i = -c; i <= c; i++)
                        for (var j = -c; j <= c; j++)
                        {
                            if (Math.Abs(j) != c)
                            {
                                multiplier = Math.Pow(2, Math.Abs(b - Math.Abs(i))) * Math.Abs(b - Math.Abs(j));
                                if (size == 3) multiplier /= 2;
                            }
                            else multiplier = Math.Pow(2, Math.Abs(b - Math.Abs(i)) - 1);
                            if (i > 0) gx += greyscale[x + j, y + i] * multiplier;
                            if (i < 0) gx -= greyscale[x + j, y + i] * multiplier;
                            if (Math.Abs(i) != c)
                            {
                                multiplier = Math.Pow(2, Math.Abs(b - Math.Abs(j))) * Math.Abs(b - Math.Abs(i));
                                if (size == 3) multiplier /= 2;
                            }
                            else multiplier = Math.Pow(2, Math.Abs(b - Math.Abs(j)) - 1);
                            if (j < 0) gy += greyscale[x + j, y + i] * multiplier;
                            if (j > 0) gy -= greyscale[x + j, y + i] * multiplier;
                        }
                    result[x, y] = Math.Sqrt(gx * gx + gy * gy);
                }
            return result;
        }

		/* Задача #5 (Бонусная, без баллов): 
		Реализуйте или используйте готовый алгоритм Хафа для поиска аналитических координат прямых на изображений
		http://ru.wikipedia.org/wiki/Преобразование_Хафа
		*/
		public static Line[] HoughAlgorithm(double[,] original)
		{
			var width = original.GetLength(0);
			var height = original.GetLength(1);
			return new Line[] { new Line(0, 0, width, height), new Line(0, height, width, 0) };
		}
	}
}
