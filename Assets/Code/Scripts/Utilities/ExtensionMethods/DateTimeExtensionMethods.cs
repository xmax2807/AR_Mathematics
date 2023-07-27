using System;

namespace Project.Utils.ExtensionMethods
{
    public static class DateTimeExtensionMethods
	{

		private static Random gen = new Random();
		public static DateTime RandomDayWithinAYear()
		{
			int thisYear = DateTime.Today.Year;
			DateTime start = new(thisYear, 1, 1);
			DateTime end = new DateTime(thisYear, 12, 31);
			int range = (end - start).Days;
			return start.AddDays(gen.Next(range));
		}
	}
}