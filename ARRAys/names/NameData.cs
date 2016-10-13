using System;

namespace names
{
	public class NameData
	{
		/// <summary>Дата рождения</summary>
		public DateTime BirthDate;

		/// <summary>Имя</summary>
		public string Name;

		/// <summary>Отчество</summary>
		public string Patronymic;

		/// <summary>Фамилия</summary>
		public string Surename;

		public static NameData ParseFrom(string textLine)
		{
			string[] parts = textLine.Split('\t');
			return new NameData
				{
					BirthDate = DateTime.Parse(parts[0]),
					Surename = parts[1],
					Name = parts[2],
					Patronymic = parts[3],
				};
		}

		public override string ToString()
		{
			return string.Format("{0}\t{1}\t{2}\t{3}", BirthDate.ToString("dd.MM.yyyy"), Surename, Name, Patronymic);
		}
	}
}