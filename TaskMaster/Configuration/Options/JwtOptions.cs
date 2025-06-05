namespace Configuration.Options;

public class JwtOptions
{
    public int AccessTokenExpiryMinutes { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public string IssuerSecretKey { get; set; }
    public int RefreshTokenExpiryDays { get; set; }
}