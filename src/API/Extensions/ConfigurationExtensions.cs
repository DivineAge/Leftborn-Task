namespace API.Extensions;

internal static class ConfigurationExtensions
{
    internal static void AddModuleConfiguration(this IConfigurationBuilder configurationBuilder, string[] modules)
    {
        foreach (string module in modules)
        {
            configurationBuilder.AddJsonFile($"module.{module}.json", false, true);
            configurationBuilder.AddJsonFile($"module.{module}.Development.json", true, true);
        }
    }
}