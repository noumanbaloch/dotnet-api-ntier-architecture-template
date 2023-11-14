namespace Breeze.Models.Configurations;
public class EmailConfiguration
{
    public required string Server { get; set; }
    public int Port { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
    public bool EnableSsl { get; set; }
    public bool UseDefaultCredentials { get; set; }
    public required string From { get; set; }
    public required string BrandName { get; set; }
    public required string TemplateDirectory { get; set; }
}
