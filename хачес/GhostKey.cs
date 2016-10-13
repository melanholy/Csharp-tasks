using System;

namespace hashes
{
	public class GhostKey
	{
		public string Name { get; private set; }

		public GhostKey(string name)
		{
			if (name == null) throw new ArgumentNullException("name");
			Name = name;
		}

		public void DoSomething()
		{
            var result = "";
            for (var i = Name.Length - 1; i >= 0; i--)
                result += Name[i];
                Name = result;
		}

        public override bool Equals(object obj)
        {
            var key = obj as GhostKey;
            return Name == key.Name;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return Name[0];
            }
        }
	}
}
