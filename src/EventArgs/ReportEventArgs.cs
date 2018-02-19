public class ReportEventArgs
{
    public IReport Report { get; }

    public ReportEventArgs(IReport report)
    {
        Report = report;
    }
}
