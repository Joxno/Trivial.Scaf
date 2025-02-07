using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using Trivial.Utilities;

namespace Trivial.Functional.Trivial.Reflection;

public static partial class Meta
{
    private static Dictionary<string, Type> m_TypeCache = new Dictionary<string, Type>();
    private static List<Type> m_TypeCacheList = new List<Type>();
    static Meta()
    {
        _BuildTypeCache();
        AppDomain.CurrentDomain.DomainUnload += _HandleUnloading;
        AppDomain.CurrentDomain.AssemblyLoad += _HandleAssemblyLoad;
    }

    public static void LoadAllReferencedAssemblies()
    {
        var t_AssemblySet = AppDomain.CurrentDomain.GetAssemblies().Select(A => A.FullName).ToHashSet();
        foreach (var t_Assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            _LoadAssembly(t_Assembly, t_AssemblySet);
        }
    }

    private static void _LoadAssembly(Assembly Assembly, HashSet<string> LoadedAssemblies)
    {
        foreach (var t_AssemblyName in Assembly.GetReferencedAssemblies())
        {
            if (!LoadedAssemblies.Contains(t_AssemblyName.FullName))
            {
                LoadedAssemblies.Add(t_AssemblyName.FullName);
                _LoadAssembly(Assembly.Load(t_AssemblyName), LoadedAssemblies);
            }
        }
    }

    public static void RebuildTypeCache()
    {
        m_TypeCacheList.Clear();
        m_TypeCache.Clear();
        _BuildTypeCache();
    }

    private static void _BuildTypeCache()
    {
        AppDomain.CurrentDomain
            .GetAssemblies()
            .Where(A => !A.IsNull())
            .SelectMany(A => 
                Try.Invoke(() => {
                    var t_Types = new Type[] { };
                    try
                    {
                        t_Types = A.GetTypes();
                    }
                    catch(Exception _) {}

                    return t_Types;
                })
                .ThenOrElse(
                    T => T, 
                    E => ((ReflectionTypeLoadException)E).Types.Where(T => !T.IsNull()).ToArray()
                ).Value).ToList()
            .ForEach(T => {
                m_TypeCache.TryAdd(T.FullName, T);
                if(!m_TypeCacheList.Contains(T))
                    m_TypeCacheList.Add(T);
            });
    }

    private static void _HandleUnloading(object? S, EventArgs E)
    {
        m_TypeCacheList.Clear();
        m_TypeCache.Clear();
        AppDomain.CurrentDomain.DomainUnload -= _HandleUnloading;
        AppDomain.CurrentDomain.AssemblyLoad -= _HandleAssemblyLoad;
    }

    private static void _HandleAssemblyLoad(object? S, EventArgs E)
    {
        m_TypeCacheList.Clear();
        m_TypeCache.Clear();
        _BuildTypeCache();
    }

    public static Action<T1, T2> CreateFieldSetter<T1, T2>(FieldInfo Field)
    {
        var t_MethodName = $"{Field.ReflectedType.FullName}.set_field_{Field.Name}";
        var t_Setter = new DynamicMethod(
            t_MethodName,
            typeof(void),
            new Type[] { typeof(T1), typeof(T2) },
            typeof(Meta).Module,
            true
        );

        var t_Il = t_Setter.GetILGenerator();
        t_Il.Emit(OpCodes.Ldarg_0);
        t_Il.Emit(OpCodes.Ldarg_1);
        t_Il.Emit(OpCodes.Stfld, Field);
        t_Il.Emit(OpCodes.Ret);

        return (Action<T1, T2>)t_Setter.CreateDelegate(typeof(Action<T1, T2>));
    }

    public static Func<T1, T2> CreateFieldGetter<T1, T2>(FieldInfo Field)
    {
        var t_MethodName = $"{Field.ReflectedType.FullName}.get_field_{Field.Name}";
        var t_Getter = new DynamicMethod(
            t_MethodName,
            typeof(T2),
            new Type[] { typeof(T1) },
            typeof(Meta).Module,
            true
        );

        var t_Il = t_Getter.GetILGenerator();
        t_Il.Emit(OpCodes.Ldarg_0);
        t_Il.Emit(OpCodes.Ldfld, Field);
        t_Il.Emit(OpCodes.Ret);

        return (Func<T1, T2>)t_Getter.CreateDelegate(typeof(Func<T1, T2>));
    }

    public static Action CreateMethodFunnel(object SourceInstance, MethodInfo Source, object DestinationInstance, MethodInfo Destination)
    {
        var t_MethodName = $"{Source.ReflectedType.FullName}.method_funnel_{Source.Name}_{Destination.Name}";
        var t_Getter = new DynamicMethod(
            t_MethodName,
            typeof(void),
            new Type[] { typeof(object), typeof(object) },
            typeof(Meta).Module,
            true
        );

        var t_Il = t_Getter.GetILGenerator();
        t_Il.Emit(OpCodes.Ldarg_0);
        t_Il.Emit(OpCodes.Call, Source);
        t_Il.Emit(OpCodes.Pop);
        //t_IL.Emit(OpCodes.Ldarg_1);
        //t_IL.Emit(OpCodes.Call, Destination);
        t_Il.Emit(OpCodes.Ret);
        var t_Delegate = (Action<object, object>)t_Getter.CreateDelegate(typeof(Action<object, object>));

        return () => {
            t_Delegate(SourceInstance, DestinationInstance);
        };
    }

    public static Action<T> CreatePropertySetter<T>(PropertyInfo Property)
    {
        var t_Method = Property.ReflectedType.GetMethod($"set_{Property.Name}", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        return (Action<T>)t_Method.CreateDelegate(typeof(Action<T>));
    }

    public static Func<T> CreatePropertyGetter<T>(PropertyInfo Property)
    {
        var t_Method = Property.ReflectedType.GetMethod($"get_{Property.Name}", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        return (Func<T>)t_Method.CreateDelegate(typeof(Func<T>));
    }

    public static Result<Type[]> GetDelegateParameterTypes(Type D) =>
        D.ToResult()
            .Bind<Type>(D => D.BaseType == typeof(MulticastDelegate) ? D : new Exception("Not a delegate."))
            .Bind<MethodInfo>(D => D.GetMethod("Invoke") != null ? D.GetMethod("Invoke") : new Exception("Not a delegate."))
            .Bind<ParameterInfo[]>(D => D.GetParameters())
            .Bind<Type[]>(D => D.Select(X => X.ParameterType).ToArray());

    public static MethodInfo GetGenericMethod(Type T, string MethodName, int GenericParametersCount) =>
        T.GetMethods().First(X => X.Name == MethodName && X.GetGenericArguments().Length == GenericParametersCount);

    public static MethodInfo GetGenericMethod(Type T, string MethodName, int GenericParametersCount, BindingFlags Flags) =>
        T.GetMethods(Flags).First(X => X.Name == MethodName && X.GetGenericArguments().Length == GenericParametersCount);

    public static T1 ConvertAnyTypeGeneric<T1, T2>(T2 Value)
    {
        if(typeof(T2).IsAssignableTo(typeof(T1)))
            return (T1)(object)Value;

        return Convert.ChangeType(Value, typeof(T1)) is T1 t_Value ? t_Value : default;
    }

    public static MethodInfo SpecializeMethod(MethodInfo Method, Type[] GenericArguments) =>
        Method.MakeGenericMethod(GenericArguments);

    public static Type LookupType(string Name, bool IsGeneric = false, int GenericParamsCount = 1)
    {
        if(LookupTypeSimple(Name, IsGeneric, GenericParamsCount) is Type t_)
            return t_;

        return LookupTypeExhaustive(Name, IsGeneric, GenericParamsCount);
    }

    public static Type LookupTypeSimple(string Name, bool IsGeneric = false, int GenericParamsCount = 1) =>
        IsGeneric ? 
            (Type.GetType($"{Name}`{GenericParamsCount}") is Type t_ ? t_ : null) : 
            (System.Type.GetType(Name) is Type t_T2 ? t_T2 : null);

    public static Type LookupTypeExhaustive(string Name, bool IsGeneric = false, int GenericParamsCount = 1) =>
        Functions.Let(IsGeneric ? $"{Name}`{GenericParamsCount}" : Name,
            N => m_TypeCache.GetValueOrDefault(N) ?? throw new Exception($"Unable to find type: {N}"))();

    public static bool AssignableTo(Type Type, Type Base) =>
        Type.IsAssignableTo(Base);

    public static IEnumerable<Type> GetTypesAssignableTo(Type Base) =>
        m_TypeCacheList.Where(X => AssignableTo(X, Base));

    public static IEnumerable<PropertyInfo> GetPropertiesOfSubClass(Type Type) =>
        Type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Static);

    public static bool ImplementsInterface<T>(Type Type) =>
        Type.GetInterfaces().Any(I => I == typeof(T));
    public static bool ImplementsOpenGenericTypeInterface(Type Type, Type GenericInterface) =>
        Type.GetInterfaces().Any(I => I.IsGenericType && I.GetGenericTypeDefinition() == GenericInterface);

    public static bool IsGenericTypeOf(Type Type, Type GenericType) =>
        !Type.IsGenericType ? false : Type.GetGenericTypeDefinition() == GenericType;

    public static IEnumerable<Type> GetOpenGenericTypeArguments(Type Type, Type GenericInterface) =>
        Type.GetInterfaces().First(I => I.IsGenericType && I.GetGenericTypeDefinition() == GenericInterface).GetGenericArguments();

    public static IEnumerable<Type> GetGenericTypeArguments(Type Type) =>
        Type.GetGenericArguments();

    public static IEnumerable<object> GetItemsAndBox<T>(IEnumerable<T> Items) =>
        Items.Select(I => (object)I);

    public static IEnumerable<Type> GetTypesImplementingInterface<T>() =>
        GetTypesImplementingInterface<T>(m_TypeCacheList);

    public static IEnumerable<Type> GetTypesImplementingInterface<T>(IEnumerable<Type> Types) =>
        Types.Where(T => ImplementsInterface<T>(T));

    public static IEnumerable<Type> GetTypesImplementingOpenGenericInterface(Type GenericInterface) =>
        m_TypeCacheList
            .Where(T => ImplementsOpenGenericTypeInterface(T, GenericInterface));

    public static Type GetClosedGenericType(Type Type, Type GenericInterface) =>
        Type.GetInterfaces().First(I => I.IsGenericType && I.GetGenericTypeDefinition() == GenericInterface);

    public static Type MakeClosedGenericType(Type Type, params Type[] GenericArguments) =>
        Type.MakeGenericType(GenericArguments);

    public static bool ImplementsInterfaceMultipleTimes(Type Type, Type Interface) =>
        GetMultipleInterfaceImpl(Type, Interface).Count() > 1;
    public static IEnumerable<Type> GetMultipleInterfaceImpl(Type Type, Type Interface) =>
        Type.GetInterfaces().Where(I => (I.IsGenericType && I.GetGenericTypeDefinition() == Interface) || I == Interface);

    public static IEnumerable<FieldInfo> GetFieldsWithAttribute<T>(Type Type) where T : Attribute =>
        Type.GetFields(Meta.GetExhaustiveFlags()).Where(F => F.GetCustomAttributes(typeof(T), true).Any());

    public static List<PropertyInfo> GetPropertiesWithAttribute<T>(Type Type) where T : Attribute =>
        Type.GetProperties(Meta.GetExhaustiveFlags()).Where(P => P.GetCustomAttributes(typeof(T), true).Any()).ToList();

    public static Maybe<T> GetAttribute<T>(FieldInfo Field) where T : Attribute =>
        Field.GetCustomAttributes(typeof(T), true).FirstOrDefault() is T t_Attribute ? t_Attribute : null;

    public static Maybe<T> GetAttribute<T>(PropertyInfo Prop) where T : Attribute =>
        Prop.GetCustomAttributes(typeof(T), true).FirstOrDefault() is T t_Attribute ? t_Attribute : null;

    public static Maybe<(object, Type)> GetGenericAttribute(PropertyInfo Prop, Type Attribute) =>
        Meta.GetGenericAttributes(Prop, Attribute).FirstOrNone();

    public static IEnumerable<(object, Type)> GetGenericAttributes(PropertyInfo Prop, Type Attribute) =>
        Prop.GetCustomAttributes(true)
            .Where(T => T.GetType().IsGenericType && T.GetType().GetGenericTypeDefinition() == Attribute)
            .Select(O => (O, O.GetType()));

    public static IEnumerable<T> GetAttributes<T>(Type Type) where T : Attribute =>
        Type.GetCustomAttributes(typeof(T), true).Select(A => (T)A);

    public static Maybe<PropertyInfo> GetProperty(Type Type, string Name) =>
        Type.GetProperty(Name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

    public static BindingFlags GetExhaustiveFlags() =>
        BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

    public static Maybe<ConstructorInfo> GetParameterlessCtr(Type Type) =>
        Type.GetConstructor(Type.EmptyTypes);

    public static bool HasDefaultConstructor(Type Type) => 
        GetParameterlessCtr(Type).HasValue;

    public static List<ConstructorInfo> GetConstructors(Type Type) =>
        Type.GetConstructors().OrderBy(C => C.GetParameters().Length).ToList();

}