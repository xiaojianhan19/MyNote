using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNote
{
    class UltilityCalc
    {
    }

    public enum DateMode
    {
        Normal, BusyDay, WeekendDay,
    }
    public class UltilityDate
    {
        public static DateMode mode = DateMode.Normal;

        public static DateTime GetNextDate(DateTime curDt)
        {
            if (mode == DateMode.BusyDay || mode == DateMode.WeekendDay)
            {
                DateTime t = curDt.AddDays(1);
                while (t.DayOfWeek == DayOfWeek.Saturday || t.DayOfWeek == DayOfWeek.Sunday)
                {
                    t.AddDays(1);
                }
                return t;
            }
            else
                return curDt.AddDays(1);
        }
        public static DateTime Today()
        {
            return Convert.ToDateTime(DateTime.Today.ToShortDateString());
        }

    }

    public class UltilityConvert
    {
        public static double ToDoble(string s)
        {
            try
            {
                return Convert.ToDouble(s);
            }
            catch
            {
                return 0.0;
            }
        }

        public static int ToInt(string s)
        {
            try
            {
                return Convert.ToInt32(s);
            }
            catch
            {
                return 0;
            }
        }

        public static DateTime ToDate(string curDate)
        {
            DateTime date = Convert.ToDateTime(DateTime.Today.ToShortDateString());

            try
            {
                if (!String.IsNullOrEmpty(curDate))
                {
                    try
                    {
                        date = Convert.ToDateTime(curDate);
                    }
                    catch
                    {
                        if (curDate.Length == 4)
                        {
                            date = new DateTime(DateTime.Today.Year, Convert.ToInt32(curDate.Substring(0, 2)), Convert.ToInt32(curDate.Substring(2, 2)));
                        }
                        else if (curDate.Length == 8)
                        {
                            date = new DateTime(Convert.ToInt32(curDate.Substring(0, 4)), Convert.ToInt32(curDate.Substring(4, 2)), Convert.ToInt32(curDate.Substring(6, 2)));
                        }
                    }
                }
            }
            catch { }

            return date;
        }

        public static DateTime ToDateMin(string curDate)
        {
            DateTime date = Convert.ToDateTime(DateTime.MinValue.ToShortDateString());

            try
            {
                if (!String.IsNullOrEmpty(curDate))
                {
                    try
                    {
                        date = Convert.ToDateTime(curDate);
                    }
                    catch
                    {
                        if (curDate.Length == 4)
                        {
                            date = new DateTime(DateTime.Today.Year, Convert.ToInt32(curDate.Substring(0, 2)), Convert.ToInt32(curDate.Substring(2, 2)));
                        }
                        else if (curDate.Length == 8)
                        {
                            date = new DateTime(Convert.ToInt32(curDate.Substring(0, 4)), Convert.ToInt32(curDate.Substring(4, 2)), Convert.ToInt32(curDate.Substring(6, 2)));
                        }
                    }
                }
            }
            catch { }

            return date;
        }

    }

    class UltilityEntity
    {
        public static void CopyValue(object origin, object target)
        {
            System.Reflection.PropertyInfo[] properties = (target.GetType()).GetProperties();
            System.Reflection.FieldInfo[] fields = (origin.GetType()).GetFields();
            for (int i = 0; i < fields.Length; i++)
            {
                for (int j = 0; j < properties.Length; j++)
                {
                    if (fields[i].Name == properties[j].Name && properties[j].CanWrite)
                    {
                        properties[j].SetValue(target, fields[i].GetValue(origin), null);
                    }
                }
            }
        }
    }
    enum Day { Sun, Mon, Tue, Wed, Thu, Fri, Sat };
}
