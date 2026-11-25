


using DbRepos;
using Models.DTOs;

namespace Services;

public class AuthService : IAuthService
{
    private readonly AuthRepo _repo;
    public AuthService(AuthRepo repo)
    {
        _repo = repo;
    }

    public async Task<LoginResponse>LoginAsync(LoginRequest usrCreds)
    {
        try
        {
            var _usrSession = await _repo.LoginUserAsync(usrCreds);

            //Successful login. Create a JWT token
            _usrSession.Item.JwtToken = _jtwService.CreateJwtUserToken(_usrSession.Item);

            //For test only, decypt the JWT token and compare.
            var _tmpUserSession = _jtwService.DecodeToken(_usrSession.Item.JwtToken.EncryptedToken);

            return _usrSession;
        }
        catch
        {
            //if there was an error during login, simply pass it on.
            throw;
        }
    }
    public async Task<RegisterResponse> RegisterAsync(RegisterRequest request)
    {
        try
        {
            var _usrRegister = await _repo.RegisterUserAsync(request);

            return _usrRegister;
        }
        catch
        {
            throw;
        }
    }
}