namespace Trivial.Functional
{
    public record Cons;
    public record Cons<T, T2>(T Left, T2 Right) : Cons;

    public static partial class Functions
    {
        public static Cons<T, T2> Cons<T, T2>(T Left, T2 Right) => 
            new Cons<T, T2>(Left, Right);
    }
}