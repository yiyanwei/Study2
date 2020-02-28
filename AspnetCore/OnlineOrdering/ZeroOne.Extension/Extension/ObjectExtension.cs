using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroOne.Extension
{
    public static class ObjectExtension
    {
        /// <summary>
        /// 不为空
        /// </summary>
        /// <param name="obj">判断的对象</param>
        /// <returns></returns>
        public static bool IsNotNullAndEmpty(this object obj)
        {
            if (obj == null)
            {
                return false;
            }
            else
            {
                if (obj.GetType() == typeof(Guid))
                {
                    return !obj.ToString().Equals(Guid.Empty.ToString());
                }
                if (string.IsNullOrEmpty(obj.ToString()))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public static DateTime? ConvertToDateTime(this object obj)
        {
            if (obj.IsNotNullAndEmpty())
            {
                string strDateTime = obj.ToString().Trim();
                try
                {
                    return Convert.ToDateTime(strDateTime);
                }
                catch
                {
                    try
                    {
                        int firstIndex = strDateTime.IndexOf(' ');
                        if (firstIndex != -1)
                        {
                            string strDate = strDateTime.Substring(0, firstIndex + 1);
                            string[] dateArr = strDate.Split(new char[] { '/', '\\', '.' });
                            #region 是否日期正常分割3部分
                            if (dateArr.Length == 3)
                            {
                                int year = Convert.ToInt32(dateArr[0]);
                                int month = Convert.ToInt32(dateArr[1]);
                                int day = Convert.ToInt32(dateArr[2]);
                                DateTime dateTime = new DateTime(year, month, day);
                                int lastIndex = strDateTime.LastIndexOf(' ');
                                #region 时分秒
                                if (lastIndex != -1 && strDateTime.Length > lastIndex + 1)
                                {
                                    string strTime = strDateTime.Substring(lastIndex + 1, strDateTime.Length - lastIndex - 1);
                                    string[] timeArr = strTime.Split(':');
                                    //如果只有时
                                    if (timeArr.Length == 1)
                                    {
                                        int hour;
                                        if (int.TryParse(timeArr[0], out hour))
                                        {
                                            dateTime = dateTime.AddHours(hour);
                                        }
                                    }
                                    //如果有时分
                                    else if (timeArr.Length == 2)
                                    {
                                        int hour;
                                        if (int.TryParse(timeArr[0], out hour))
                                        {
                                            dateTime = dateTime.AddHours(hour);
                                            int minute;
                                            if (int.TryParse(timeArr[1], out minute))
                                            {
                                                dateTime = dateTime.AddMinutes(minute);
                                            }
                                        }
                                    }
                                    //如果是时分秒
                                    else if (timeArr.Length == 3)
                                    {
                                        int hour;
                                        if (int.TryParse(timeArr[0], out hour))
                                        {
                                            dateTime = dateTime.AddHours(hour);
                                            int minute;
                                            if (int.TryParse(timeArr[1], out minute))
                                            {
                                                dateTime = dateTime.AddMinutes(minute);
                                                int second;
                                                if (int.TryParse(timeArr[2], out second))
                                                {
                                                    dateTime = dateTime.AddSeconds(second);
                                                }
                                            }
                                        }
                                    }
                                }
                                #endregion
                                return dateTime;
                            }
                            #endregion
                            #region 判断是否以汉字'年,月,日'分割
                            else
                            {
                                int yearIndex = strDateTime.IndexOf('年');
                                int monthIndex = strDateTime.IndexOf('月');
                                int dayIndex = strDateTime.IndexOf('日');
                                if (yearIndex != -1 && monthIndex != -1 && dayIndex != -1)
                                {
                                    int year = Convert.ToInt32(strDateTime.Substring(0, yearIndex));
                                    int month = Convert.ToInt32(strDateTime.Substring(yearIndex + 1, monthIndex));
                                    int day = Convert.ToInt32(strDateTime.Substring(monthIndex + 1, dayIndex));
                                    DateTime dateTime = new DateTime(year, month, day);

                                    int lastIndex = strDateTime.LastIndexOf(' ');
                                    #region 时分秒
                                    if (lastIndex != -1 && strDateTime.Length > lastIndex + 1)
                                    {
                                        string strTime = strDateTime.Substring(lastIndex + 1, strDateTime.Length - lastIndex - 1);
                                        string[] timeArr = strTime.Split(':');
                                        //如果只有时
                                        if (timeArr.Length == 1)
                                        {
                                            int hour;
                                            if (int.TryParse(timeArr[0], out hour))
                                            {
                                                dateTime = dateTime.AddHours(hour);
                                            }
                                        }
                                        //如果有时分
                                        else if (timeArr.Length == 2)
                                        {
                                            int hour;
                                            if (int.TryParse(timeArr[0], out hour))
                                            {
                                                dateTime = dateTime.AddHours(hour);
                                                int minute;
                                                if (int.TryParse(timeArr[1], out minute))
                                                {
                                                    dateTime = dateTime.AddMinutes(minute);
                                                }
                                            }
                                        }
                                        //如果是时分秒
                                        else if (timeArr.Length == 3)
                                        {
                                            int hour;
                                            if (int.TryParse(timeArr[0], out hour))
                                            {
                                                dateTime = dateTime.AddHours(hour);
                                                int minute;
                                                if (int.TryParse(timeArr[1], out minute))
                                                {
                                                    dateTime = dateTime.AddMinutes(minute);
                                                    int second;
                                                    if (int.TryParse(timeArr[2], out second))
                                                    {
                                                        dateTime = dateTime.AddSeconds(second);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    #endregion
                                    return dateTime;
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            string strDate = strDateTime.Substring(0, firstIndex + 1);
                            string[] dateArr = strDate.Split(new char[] { '/', '\\', '.' });
                            if (dateArr.Length == 3)
                            {
                                int year = Convert.ToInt32(dateArr[0]);
                                int month = Convert.ToInt32(dateArr[1]);
                                int day = Convert.ToInt32(dateArr[2]);
                                int lastIndex = strDateTime.LastIndexOf(' ');
                                DateTime dateTime = new DateTime(year, month, day);
                                return dateTime;
                            }
                            else
                            {
                                int yearIndex = strDateTime.IndexOf('年');
                                int monthIndex = strDateTime.IndexOf('月');
                                int dayIndex = strDateTime.IndexOf('日');
                                if (yearIndex != -1 && monthIndex != -1 && dayIndex != -1)
                                {
                                    int year = Convert.ToInt32(strDateTime.Substring(0, yearIndex));
                                    int month = Convert.ToInt32(strDateTime.Substring(yearIndex + 1, monthIndex));
                                    int day = Convert.ToInt32(strDateTime.Substring(monthIndex + 1, dayIndex));
                                    DateTime dateTime = new DateTime(year, month, day);
                                    return dateTime;
                                }
                            }
                        }
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 值转换成目标类型值
        /// </summary>
        /// <param name="obj">需转换的值</param>
        /// <param name="type">转换结果类型</param>
        /// <returns></returns>
        public static object ChangeDataType(this object obj, Type type)
        {
            if (obj.GetType() == type)
            {
                return obj;
            }

            if (type == typeof(string))
            {
                return obj.ToString();
            }
            else if (type == typeof(DateTime?))
            {
                return obj.ConvertToDateTime();
            }
            else if (type == typeof(DateTime))
            {
                DateTime temp;
                if (DateTime.TryParse(obj.ToString(), out temp))
                {
                    return temp;
                }
                return null;
            }
            else if (type == typeof(int?) || type == typeof(int))
            {
                int temp;
                if (int.TryParse(obj.ToString(), out temp))
                {
                    return temp;
                }
                return null;
            }
            else if (type == typeof(int?) || type == typeof(int))
            {
                int temp;
                if (int.TryParse(obj.ToString(), out temp))
                {
                    return temp;
                }
                return null;
            }
            else if (type == typeof(float?) || type == typeof(float))
            {
                float temp;
                if (float.TryParse(obj.ToString(), out temp))
                {
                    return temp;
                }
                return null;
            }
            else if (type == typeof(double?) || type == typeof(double))
            {
                double temp;
                if (double.TryParse(obj.ToString(), out temp))
                {
                    return temp;
                }
                return null;
            }
            else if (type == typeof(decimal?) || type == typeof(decimal))
            {
                decimal temp;
                if (decimal.TryParse(obj.ToString(), out temp))
                {
                    return temp;
                }
                return null;
            }
            return null;
        }

    }

}
