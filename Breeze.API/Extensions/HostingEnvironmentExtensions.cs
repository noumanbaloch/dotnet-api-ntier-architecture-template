using Breeze.Models.Constants;

namespace Breeze.API.Extensions;

public static class HostingEnvironmentExtensions
{
    public static bool IsQA(this IWebHostEnvironment env)
    {
        return env.EnvironmentName == EnvironmentNames.QA_ENV;
    }
}