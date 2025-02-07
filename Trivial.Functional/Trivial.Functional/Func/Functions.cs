using System;

namespace Trivial.Functional
{
    public static partial class Functions
    {
        public static Func<T, T> Identity<T>() =>
            P => P;

        public static Func<string, int> ParseInt() =>
            Str => int.Parse(Str);

        public static Func<string, float> ParseFloat() =>
            Str => float.Parse(Str);

        public static Func<string, double> ParseDouble() =>
            Str => double.Parse(Str);

        public static Func<string, decimal> ParseDecimal() =>
            Str => decimal.Parse(Str);

        public static Func<string, Unit> WriteToConsole() =>
            Str => { Console.WriteLine(Str); return Defaults.Unit; };

        public static Func<object, string> ToString() =>
            P => P.ToString();

        public static Func<DateTime> TimeNow() => 
            () => DateTime.Now;

        public static Func<string, DateTime> ParseDateTime() =>
            Str => DateTime.Parse(Str);

        public static Func<string, IFormatProvider, DateTime> ParseDateTimeWithFormat() =>
            (Str, Format) => DateTime.Parse(Str, Format);

        public static Func<int> Counter(int StartValue) =>
            ((Func<Func<int>>)(() => { int t_I = StartValue; return () => t_I++; }))();

        public static T Call<T>(Func<T> Func) =>
            Func();
    }
}