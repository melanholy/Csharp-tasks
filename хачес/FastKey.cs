namespace hashes
{
	public class FastKey
	{
		public int X { get; private set; }
		public int Y { get; private set; }
        int Z { get; set; }

		public FastKey(int x, int y)
		{
			X = x;
			Y = y;
		}

        public override bool Equals(object obj)
        {
            var key = obj as FastKey;
            return X == key.X && Y == key.Y;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return X * 2 + Y * 3;
            }
        }
	}
}