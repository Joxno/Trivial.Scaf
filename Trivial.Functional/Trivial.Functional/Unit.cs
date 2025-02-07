namespace Trivial.Functional
{
    public record Unit;

    public static partial class Defaults
    {
        public static Unit Unit => new Unit();

        public static Unit ToUnit<T>(this T _) => Unit;
    }

}