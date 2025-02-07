namespace Trivial.Utilities;

public static class StringExtensions
{
    public static StringKey ToKey(this string Key) => new(Key);
}