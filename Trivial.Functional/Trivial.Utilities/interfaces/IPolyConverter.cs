namespace Trivial.Utilities.interfaces;

public interface IPolyConverter<TFrom, TTo>
{
    TTo Convert(TFrom From);
}