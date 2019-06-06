/*
 * @Author: fasthro
 * @Date: 2019-06-06 20:49:32
 * @Description: 时间转换
 */

using System;
using UnityEngine;

namespace UnityScript
{
    public class TimeConvert
    {
        public const long TICKS_TO_SECOND = 10000000L;
        public static long sSeverTime = 0;
        public static float sServerStartTime = 0;

        public static long GetSeverTime()
        {
            return sSeverTime + (long)((Time.realtimeSinceStartup - sServerStartTime) * 1000f);
        }

        // 将unix时间转换为字符串日期时间
        public static string UNIXTimeToDateTimeString(long time)
        {
            DateTime date = UNIXTimeToDateTime(time);
            return date.ToString("yyyy/MM/dd hh:mm:ss");
        }

        // 将unix时间转换为字符串日期时间
        public static string UNIXTimeToDateString(long time)
        {
            DateTime date = UNIXTimeToDateTime(time);
            return date.ToString("yyyy-MM-dd");
        }

        // 将unix时间转换为c#时间
        public static DateTime UNIXTimeToDateTime(long time)
        {
            long timeL = time * 10000000 + (new DateTime(1970, 1, 1, 8, 0, 0).Ticks);

            DateTime date = new DateTime(timeL);
            return date;
        }

        // c#日期时间转换为unix时间
        public static long DateTimeToUNIXTime(DateTime dt)
        {
            long timeL = (dt.Ticks - (new DateTime(1970, 1, 1, 8, 0, 0).Ticks)) / 10000000L;
            return timeL;
        }

        // Show "22m 22.351s"
        public static string MilSecToString(long _milSec)
        {
            int inSec = (int)(_milSec / 1000);

            int minute = inSec / 60;
            int second = inSec % 60;
            int msecond = (int)(_milSec % 1000);

            string result = " ";
            if (minute > 0)
                result = minute + "m";

            if (second != 0 || msecond != 0)
                result = result + " " + second + "." + msecond + "s";

            return result;
        }

        // Show "22d 22h" or "22m 22s" or "22h 22m"
        public static string SecondToString(int second)
        {
            string dd = "d";
            string hh = "h";
            string mm = "m";
            string ss = "s";

            string str = string.Empty;
            if (second <= 0)
                return "0" + ss;

            int day = second / 86400;
            int hour = (second - day * 86400) / 3600;
            int minute = (second - day * 86400 - hour * 3600) / 60;
            second = second - day * 86400 - hour * 3600 - minute * 60;
            if (day > 0)
            {
                str = str + day + dd;
                if (hour > 0) str = str + " ";
            }
            if (hour > 0)
            {
                if (hour < 10)
                    str = str + " " + hour + hh;
                else
                    str = str + hour + hh;
                if (day == 0 && minute > 0) str = str + " ";
            }
            // max show two time segments 5d 20h
            if (day == 0)
            {
                if (minute > 0)
                {
                    if (minute < 10)
                        str = str + " " + minute + mm;
                    else
                        str = str + minute + mm;
                    if (hour == 0 && second > 0) str = str + " ";
                }
                // max show two time segments 20h 43m
                if (hour == 0)
                {
                    if (second > 0)
                    {
                        if (second < 10)
                            str = str + " " + second + ss;
                        else
                            str = str + second + ss;
                    }
                }
            }
            return str;
        }



        // Show "22:22:22"
        public static string SecondToString2(int second)
        {
            string str = string.Empty;
            int hour = second / 3600;
            int minute = (second - hour * 3600) / 60;
            second = second - hour * 3600 - minute * 60;
            if (hour != 0)
            {
                if (hour < 10)
                    str = str + "0" + hour + ":";
                else
                    str = str + hour + ":";
            }
            if (minute < 10)
                str = str + "0" + minute + ":";
            else
                str = str + minute + ":";
            if (second < 10)
                str = str + "0" + second;
            else
                str = str + second;
            return str;
        }
    }
}