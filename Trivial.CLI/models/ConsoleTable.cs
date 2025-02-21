namespace Trivial.CLI.models;

public class ConsoleTable
{
    private List<ConsoleRow> m_Rows = new();
    private List<int> m_ColumnWidths = new();
    private int m_WidthBuffer = 8;

    public int MaxColumnWidth { get; set; } = 40;
    public List<string> Headers { get; set; } = new();
    public char Separator { get; set; } = '-';
    public ConsoleTable(params List<string> Headers)
    {
        this.Headers = Headers;
    }

    public void AddRow(ConsoleRow Row) =>
        m_Rows.Add(Row);

    public void AddRow(params List<object> Content) =>
        m_Rows.Add(new ConsoleRow(Content.Select(O => O?.ToString() ?? "").ToList()));

    public void Print()
    {
        _CalculateWidths();
        var t_Printables = new List<string>{
            _GenerateHeader(),
            _GenerateSeparator(),
            _GenerateContent()
        };

        t_Printables.ForEach(P => Console.WriteLine(P));
    }

    private void _CalculateWidths()
    {
        for(int i = 0; i < Headers.Count; i++)
        {
            var t_RowContents = m_Rows
                .SelectMany(R => R.Content.Skip(i).Take(1)).ToList();

            var t_MaxWidth = t_RowContents.Count == 0 ? Headers[i].Length : t_RowContents.Max(C => C.Length);

            m_ColumnWidths.Add(Math.Max(Headers[i].Length, Math.Min(t_MaxWidth, MaxColumnWidth)) + m_WidthBuffer);
        }
    }

    private string _GenerateSeparator() => 
        string.Join("", Enumerable.Range(0, m_ColumnWidths.Sum()).Select(_ => Separator));

    private string _GenerateHeader()
    {
        var t_HeaderStr = "";
        Headers.ForEach((S, I) => {
            t_HeaderStr += S.Length < m_ColumnWidths[I] ? S.PadRight(m_ColumnWidths[I]) : S[..m_ColumnWidths[I]];
        });
        return t_HeaderStr;
    }

    private string _GenerateContent()
    {
        var t_ContentStr = new List<string>();
        m_Rows.ForEach(R => {
            var t_Content = "";
            R.Content.ForEach((C, I) => {
                t_Content += C.Length < m_ColumnWidths[I] ? C.PadRight(m_ColumnWidths[I]) : C[..m_ColumnWidths[I]];
            });
            t_ContentStr.Add(t_Content);
        });
        return string.Join("\n", t_ContentStr);
    }
}

public record struct ConsoleRow(List<string> Content);