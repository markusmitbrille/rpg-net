public class PackageEventArgs
{
    public IPackage Package { get; }

    public PackageEventArgs(IPackage package)
    {
        Package = package;
    }
}
