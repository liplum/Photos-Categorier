using System.Windows.Controls;

namespace PhotosCategorier.SubWindows.RenamePhotosWindows
{
    public interface IRule
    {
        string GenerateString();
        static string RuleDisplayedName { get; }
        Panel GetFuntionArea();

    }
}
