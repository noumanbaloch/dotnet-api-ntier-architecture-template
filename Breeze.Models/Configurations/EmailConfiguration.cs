namespace Breeze.Models.Configurations;
public class EmailConfiguration
{
    public string Server { get; set; } = string.Empty;
    public int Port { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool EnableSsl { get; set; }
    public bool UseDefaultCredentials { get; set; }
    public string From { get; set; } = string.Empty;
    public string BrandName { get; set; } = string.Empty;
    public string TemplateDirectory { get; set; } = string.Empty;
}
