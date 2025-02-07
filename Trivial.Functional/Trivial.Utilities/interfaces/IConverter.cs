namespace Trivial.Utilities.interfaces;

public interface IConverter<TFrom, TTo>
{
    Type GetFromType() { return typeof(TFrom); }
    Type GetToType() { return typeof(TTo); }
    
    TTo Convert(TFrom From);
}