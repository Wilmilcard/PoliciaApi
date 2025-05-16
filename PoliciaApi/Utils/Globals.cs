namespace PoliciaApi.Utils
{
    public class Globals
    {
        public static DateTime SystemDate() => DateTime.UtcNow.AddHours(-5);

        public static string PathSystem(string[] paths)
        {
            var basePath = AppDomain.CurrentDomain.BaseDirectory;
            var solutionPath = Directory.GetParent(basePath).Parent.Parent.Parent.Parent.FullName;

            return Path.Combine(solutionPath, Path.Combine(paths));
        }
    }
}
