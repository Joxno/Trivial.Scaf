namespace Trivial.Utilities;

public struct StringKey
{
    private string m_Key = "";
    
    public StringKey(string Key) => m_Key = Key.ToUpper();

    public static implicit operator StringKey(string Value) =>
        new StringKey(Value);

    public static implicit operator string(StringKey Key) =>
        Key.m_Key.ToUpper();

    public static bool operator ==(StringKey T, string Value) =>
        T.Equals(Value);

    public static bool operator !=(StringKey T, string Value) =>
        !(T == Value);

    public override bool Equals(object O)
    {
        if(O is null || m_Key is null) return false;
        if(O is string t_S)
            return m_Key == t_S.ToUpper();

        return O is StringKey t_Key &&
               m_Key == t_Key.m_Key && t_Key != null && m_Key != null && t_Key.m_Key != null;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(m_Key);
    }

    public override string ToString() =>
        m_Key.ToUpper();

    public static bool Compare(string K1, string K2) =>
        K1.ToUpper() == K2.ToUpper();
}