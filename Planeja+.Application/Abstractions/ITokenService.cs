namespace Planeja_.Application.Abstractions;

public interface ITokenService
{
    (string token, DateTime expiresAt) GenerateAccessToken(Guid userId, string email);
}
