using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace streams
{
	public class BitmapEditor : IDisposable
	{
		private readonly Bitmap Bmp;
		private readonly BitmapData Data;
		private readonly byte[] RgbValues;
		private readonly IntPtr ImgPtr;
	    private readonly int BytesWidth;

		public BitmapEditor(Bitmap bmp)
		{
			Bmp = bmp;
			Data = Bmp.LockBits(
				new Rectangle (0, 0, bmp.Width, bmp.Height), 
				ImageLockMode.ReadWrite, 
				bmp.PixelFormat
			);

			ImgPtr = Data.Scan0;
		    BytesWidth = bmp.Width * 3;
			RgbValues = new byte[Math.Abs(Data.Stride) * Bmp.Height];
			Marshal.Copy(ImgPtr, RgbValues, 0, Math.Abs(Data.Stride) * Bmp.Height);
		}

		public void SetPixel(int x, int y, byte r, byte g, byte b)
		{
			var place = x * 3 + y * BytesWidth;
			RgbValues [place] = b;
			RgbValues [place + 1] = g;
			RgbValues [place + 2] = r;
		}

		public Color GetPixel(int x, int y)
		{
			var place = x * 3 + y * BytesWidth;
			return Color.FromArgb(
				RgbValues [place + 2],
				RgbValues [place + 1],
				RgbValues [place]
			);
		}

		public void Dispose()
		{
			Marshal.Copy(RgbValues, 0, ImgPtr, Math.Abs(Data.Stride) * Bmp.Height);
			Bmp.UnlockBits(Data);
		}
	}
}
