namespace Common;

[System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S101:Types should be named in PascalCase", Justification = "<Pending>")]
public interface HasServiceProvider
{
    Eff<IServiceProvider> ServiceProvider { get; }
    Eff<T> RequiredService<T>() where T : notnull;
}