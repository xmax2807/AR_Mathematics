using System;
using System.Collections;
using System;

namespace Assets.Code.Scripts.Utilities.ExtensionMethods
{
	public static class DateTimeExtensionMethods
	{

		private static Random gen = new Random();
		public static DateTime RandomDayWithinAYear()
		{
			int thisYear = DateTime.Today.Year;
			DateTime start = new DateTime(thisYear, 1, 1);
			DateTime end = new DateTime(thisYear, 12, 31);
			int range = (end - start).Days;
			return start.AddDays(gen.Next(range));
		}
	}
}