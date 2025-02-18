using System.CommandLine;
using System.CommandLine.Binding;

namespace Trivial.CLI.extensions;

public static class Cmd
{
    public static Command New(string Name, string Description, Action CmdAction) => 
        new Command(Name, Description).Tap(C => C.SetHandler(CmdAction));
    public static Command New<T1>(string Name, string Description, Action<T1> CmdAction, IValueDescriptor<T1> P1)
    {
        var t_Cmd = new Command(Name, Description);
        t_Cmd._AddParam(P1);
        t_Cmd.SetHandler(CmdAction, P1);

        return t_Cmd;
    }

    public static Command New<T1, T2>(string Name, string Description, Action<T1, T2> CmdAction, IValueDescriptor<T1> P1, IValueDescriptor<T2> P2)
    {
        var t_Cmd = new Command(Name, Description);
        t_Cmd._AddParam(P1);
        t_Cmd._AddParam(P2);
        t_Cmd.SetHandler(CmdAction, P1, P2);

        return t_Cmd;
    }

    public static Command New<T1, T2, T3>(string Name, string Description, Action<T1, T2, T3> CmdAction, IValueDescriptor<T1> P1, IValueDescriptor<T2> P2, IValueDescriptor<T3> P3)
    {
        var t_Cmd = new Command(Name, Description);
        t_Cmd._AddParam(P1);
        t_Cmd._AddParam(P2);
        t_Cmd._AddParam(P3);
        t_Cmd.SetHandler(CmdAction, P1, P2, P3);

        return t_Cmd;
    }

    public static Command New<T1, T2, T3, T4>(string Name, string Description, Action<T1, T2, T3, T4> CmdAction, IValueDescriptor<T1> P1, IValueDescriptor<T2> P2, IValueDescriptor<T3> P3, IValueDescriptor<T4> P4)
    {
        var t_Cmd = new Command(Name, Description);
        t_Cmd._AddParam(P1);
        t_Cmd._AddParam(P2);
        t_Cmd._AddParam(P3);
        t_Cmd._AddParam(P4);
        t_Cmd.SetHandler(CmdAction, P1, P2, P3, P4);

        return t_Cmd;
    }

    public static Command New<T1, T2, T3, T4, T5>(string Name, string Description, Action<T1, T2, T3, T4, T5> CmdAction, IValueDescriptor<T1> P1, IValueDescriptor<T2> P2, IValueDescriptor<T3> P3, IValueDescriptor<T4> P4, IValueDescriptor<T5> P5)
    {
        var t_Cmd = new Command(Name, Description);
        t_Cmd._AddParam(P1);
        t_Cmd._AddParam(P2);
        t_Cmd._AddParam(P3);
        t_Cmd._AddParam(P4);
        t_Cmd._AddParam(P5);
        t_Cmd.SetHandler(CmdAction, P1, P2, P3, P4, P5);

        return t_Cmd;
    }

    public static Command New<T1, T2, T3, T4, T5, T6>(string Name, string Description, Action<T1, T2, T3, T4, T5, T6> CmdAction, IValueDescriptor<T1> P1, IValueDescriptor<T2> P2, IValueDescriptor<T3> P3, IValueDescriptor<T4> P4, IValueDescriptor<T5> P5, IValueDescriptor<T6> P6)
    {
        var t_Cmd = new Command(Name, Description);
        t_Cmd._AddParam(P1);
        t_Cmd._AddParam(P2);
        t_Cmd._AddParam(P3);
        t_Cmd._AddParam(P4);
        t_Cmd._AddParam(P5);
        t_Cmd._AddParam(P6);
        t_Cmd.SetHandler(CmdAction, P1, P2, P3, P4, P5, P6);

        return t_Cmd;
    }

    public static Command New<T1, T2, T3, T4, T5, T6, T7>(string Name, string Description, Action<T1, T2, T3, T4, T5, T6, T7> CmdAction, IValueDescriptor<T1> P1, IValueDescriptor<T2> P2, IValueDescriptor<T3> P3, IValueDescriptor<T4> P4, IValueDescriptor<T5> P5, IValueDescriptor<T6> P6, IValueDescriptor<T7> P7)
    {
        var t_Cmd = new Command(Name, Description);
        t_Cmd._AddParam(P1);
        t_Cmd._AddParam(P2);
        t_Cmd._AddParam(P3);
        t_Cmd._AddParam(P4);
        t_Cmd._AddParam(P5);
        t_Cmd._AddParam(P6);
        t_Cmd._AddParam(P7);
        t_Cmd.SetHandler(CmdAction, P1, P2, P3, P4, P5, P6, P7);

        return t_Cmd;
    }

    public static Command NewSub(this Command Cmd, string Name, string Description) => 
        new Command(Name, Description);

    public static Command NewSub(this Command Cmd, string Name, string Description, Action CmdAction) => 
        new Command(Name, Description).Tap(C => C.SetHandler(CmdAction));

    public static Command NewSub<T1>(this Command Cmd, string Name, string Description, Action<T1> CmdAction, IValueDescriptor<T1> P1)
    {
        var t_Cmd = new Command(Name, Description);
        t_Cmd._AddParam(P1);
        t_Cmd.SetHandler(CmdAction, P1);
        Cmd.Add(t_Cmd);
        return t_Cmd;
    }

    public static Command NewSub<T1, T2>(this Command Cmd, string Name, string Description, Action<T1, T2> CmdAction, IValueDescriptor<T1> P1, IValueDescriptor<T2> P2)
    {
        var t_Cmd = new Command(Name, Description);
        t_Cmd._AddParam(P1);
        t_Cmd._AddParam(P2);
        t_Cmd.SetHandler(CmdAction, P1, P2);
        Cmd.Add(t_Cmd);
        return t_Cmd;
    }

    public static Command NewSub<T1, T2, T3>(this Command Cmd, string Name, string Description, Action<T1, T2, T3> CmdAction, IValueDescriptor<T1> P1, IValueDescriptor<T2> P2, IValueDescriptor<T3> P3)
    {
        var t_Cmd = new Command(Name, Description);
        t_Cmd._AddParam(P1);
        t_Cmd._AddParam(P2);
        t_Cmd._AddParam(P3);
        t_Cmd.SetHandler(CmdAction, P1, P2, P3);
        Cmd.Add(t_Cmd);
        return t_Cmd;
    }

    public static Command NewSub<T1, T2, T3, T4>(this Command Cmd, string Name, string Description, Action<T1, T2, T3, T4> CmdAction, IValueDescriptor<T1> P1, IValueDescriptor<T2> P2, IValueDescriptor<T3> P3, IValueDescriptor<T4> P4)
    {
        var t_Cmd = new Command(Name, Description);
        t_Cmd._AddParam(P1);
        t_Cmd._AddParam(P2);
        t_Cmd._AddParam(P3);
        t_Cmd._AddParam(P4);
        t_Cmd.SetHandler(CmdAction, P1, P2, P3, P4);
        Cmd.Add(t_Cmd);
        return t_Cmd;
    }

    public static Command NewSub<T1, T2, T3, T4, T5>(this Command Cmd, string Name, string Description, Action<T1, T2, T3, T4, T5> CmdAction, IValueDescriptor<T1> P1, IValueDescriptor<T2> P2, IValueDescriptor<T3> P3, IValueDescriptor<T4> P4, IValueDescriptor<T5> P5)
    {
        var t_Cmd = new Command(Name, Description);
        t_Cmd._AddParam(P1);
        t_Cmd._AddParam(P2);
        t_Cmd._AddParam(P3);
        t_Cmd._AddParam(P4);
        t_Cmd._AddParam(P5);
        t_Cmd.SetHandler(CmdAction, P1, P2, P3, P4, P5);
        Cmd.Add(t_Cmd);
        return t_Cmd;
    }

    public static Command NewSub<T1, T2, T3, T4, T5, T6>(this Command Cmd, string Name, string Description, Action<T1, T2, T3, T4, T5, T6> CmdAction, IValueDescriptor<T1> P1, IValueDescriptor<T2> P2, IValueDescriptor<T3> P3, IValueDescriptor<T4> P4, IValueDescriptor<T5> P5, IValueDescriptor<T6> P6)
    {
        var t_Cmd = new Command(Name, Description);
        t_Cmd._AddParam(P1);
        t_Cmd._AddParam(P2);
        t_Cmd._AddParam(P3);
        t_Cmd._AddParam(P4);
        t_Cmd._AddParam(P5);
        t_Cmd._AddParam(P6);
        t_Cmd.SetHandler(CmdAction, P1, P2, P3, P4, P5, P6);
        Cmd.Add(t_Cmd);
        return t_Cmd;
    }

    public static Command NewSub<T1, T2, T3, T4, T5, T6, T7>(this Command Cmd, string Name, string Description, Action<T1, T2, T3, T4, T5, T6, T7> CmdAction, IValueDescriptor<T1> P1, IValueDescriptor<T2> P2, IValueDescriptor<T3> P3, IValueDescriptor<T4> P4, IValueDescriptor<T5> P5, IValueDescriptor<T6> P6, IValueDescriptor<T7> P7)
    {
        var t_Cmd = new Command(Name, Description);
        t_Cmd._AddParam(P1);
        t_Cmd._AddParam(P2);
        t_Cmd._AddParam(P3);
        t_Cmd._AddParam(P4);
        t_Cmd._AddParam(P5);
        t_Cmd._AddParam(P6);
        t_Cmd._AddParam(P7);
        t_Cmd.SetHandler(CmdAction, P1, P2, P3, P4, P5, P6, P7);
        Cmd.Add(t_Cmd);
        return t_Cmd;
    }

    private static void _AddParam<T>(this Command Cmd, IValueDescriptor<T> Param) =>
        Param.GetType().SwitchOnValue(
            (typeof(Option), () => Cmd.Add((Option)Param)),
            (typeof(Argument), () => Cmd.Add((Argument)Param))
        );
}