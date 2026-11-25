using DbContext;
using Models.DTOs;

namespace DbRepos;

public class AuthRepo
{
    private readonly MainDbContext _dbContext;
    private Encryptions _encryptions;

    public AuthRepo(MainDbContext dbContext, Encryptions encryptions)
    {
        _dbContext = dbContext;
        _encryptions = encryptions;
    }
    public async Task<LoginResponse> LoginUserAsync(LoginRequest usrCreds)
    {
        using (var cmd1 = _dbContext.Database.GetDbConnection().CreateCommand())
        {
            //Notice how I use the efc Command to call sp as I do not return any dataset, only output parameters
            //Notice also how I encrypt the password, no coms to database with open password
            cmd1.CommandType = CommandType.StoredProcedure;

            // Create parameters based on database provider
            DbParameter userNameParam, userPasswordParam, userIdParam, userNameOutParam, userRoleParam;
            var connection = _dbContext.Database.GetDbConnection();

            if (connection is MySqlConnection)
            {
                cmd1.CommandText = "gstusr_spLogin";

                // MySQL parameters
                userNameParam = new MySqlParameter("UserNameOrEmail", usrCreds.UserNameOrEmail);
                userPasswordParam = new MySqlParameter("UserPassword", _encryptions.EncryptPasswordToBase64(usrCreds.Password));
                userIdParam = new MySqlParameter("UserId", MySqlDbType.Guid) { Direction = ParameterDirection.Output };
                userNameOutParam = new MySqlParameter("UserName", MySqlDbType.VarChar, 100) { Direction = ParameterDirection.Output };
                userRoleParam = new MySqlParameter("UserRole", MySqlDbType.VarChar, 100) { Direction = ParameterDirection.Output };
            }
            else if (connection is NpgsqlConnection)
            {
                cmd1.CommandText = "SELECT userid, username, userrole FROM gstusr.\"spLogin\"(@usernameoremail, @userpassword)";
                cmd1.CommandType = CommandType.Text;

                // PostgreSQL parameters
                userNameParam = new NpgsqlParameter("usernameoremail", usrCreds.UserNameOrEmail);
                userPasswordParam = new NpgsqlParameter("userpassword", _encryptions.EncryptPasswordToBase64(usrCreds.Password));
                userIdParam = new NpgsqlParameter("userid", NpgsqlTypes.NpgsqlDbType.Uuid) { Direction = ParameterDirection.Output };
                userNameOutParam = new NpgsqlParameter("username", NpgsqlTypes.NpgsqlDbType.Varchar, 100) { Direction = ParameterDirection.Output };
                userRoleParam = new NpgsqlParameter("userrole", NpgsqlTypes.NpgsqlDbType.Varchar, 100) { Direction = ParameterDirection.Output };
            }
            else
            {
                cmd1.CommandText = "gstusr.spLogin";

                // SQL Server parameters (default)
                userNameParam = new SqlParameter("UserNameOrEmail", usrCreds.UserNameOrEmail);
                userPasswordParam = new SqlParameter("UserPassword", _encryptions.EncryptPasswordToBase64(usrCreds.Password));
                userIdParam = new SqlParameter("UserId", SqlDbType.UniqueIdentifier) { Direction = ParameterDirection.Output };
                userNameOutParam = new SqlParameter("UserName", SqlDbType.VarChar, 100) { Direction = ParameterDirection.Output };
                userRoleParam = new SqlParameter("UserRole", SqlDbType.VarChar, 100) { Direction = ParameterDirection.Output };
            }

            cmd1.Parameters.Add(userNameParam);
            cmd1.Parameters.Add(userPasswordParam);
            int _usrIdIdx = cmd1.Parameters.Add(userIdParam);
            int _usrIdx = cmd1.Parameters.Add(userNameOutParam);
            int _roleIdx = cmd1.Parameters.Add(userRoleParam);

            if (connection.State != ConnectionState.Open)
                await _dbContext.Database.OpenConnectionAsync();
            await cmd1.ExecuteScalarAsync();

            var info = new LoginResponse
            {
                //Notice the soft cast conversion 'as' it will be null if cast cannot be made
                UserId = cmd1.Parameters[_usrIdIdx].Value as Guid?,
                UserName = cmd1.Parameters[_usrIdx].Value as string,
                UserRole = cmd1.Parameters[_roleIdx].Value as string
            };

            return info;
        }
    }


    public async Task<RegisterResponse> RegisterUserAsync(RegisterRequest request)
    {
        using (var cmd = _dbContext.Database.GetDbConnection().CreateCommand())
        {
            cmd.CommandType = CommandType.StoredProcedure;

            DbParameter userNameParam, emailParam, passwordParam;
            DbParameter firstNameParam, lastNameParam, userIdOutParam;

            var connection = _dbContext.Database.GetDbConnection();

            if (connection is MySqlConnection)
            {
                cmd.CommandText = "gstusr_spRegister";

                userNameParam = new MySqlParameter("UserName", request.UserName);
                emailParam = new MySqlParameter("Email", request.Email);
                passwordParam = new MySqlParameter(
                    "Password",
                    _encryptions.EncryptPasswordToBase64(request.Password)
                );
                firstNameParam = new MySqlParameter("FirstName", request.FirstName);
                lastNameParam = new MySqlParameter("LastName", request.LastName);

                userIdOutParam = new MySqlParameter("UserId", MySqlDbType.Guid)
                {
                    Direction = ParameterDirection.Output
                };
            }
            else if (connection is NpgsqlConnection)
            {
                cmd.CommandText = "SELECT gstusr.\"spRegister\"(@username, @email, @password, @firstname, @lastname)";

                cmd.CommandType = CommandType.Text;

                userNameParam = new NpgsqlParameter("username", request.UserName);
                emailParam = new NpgsqlParameter("email", request.Email);
                passwordParam = new NpgsqlParameter(
                    "password",
                    _encryptions.EncryptPasswordToBase64(request.Password)
                );
                firstNameParam = new NpgsqlParameter("firstname", request.FirstName);
                lastNameParam = new NpgsqlParameter("lastname", request.LastName);

                userIdOutParam = new NpgsqlParameter("userid", NpgsqlTypes.NpgsqlDbType.Uuid)
                {
                    Direction = ParameterDirection.Output
                };
            }
            else
            {
                // SQL Server
                cmd.CommandText = "gstusr.spRegister";

                userNameParam = new SqlParameter("UserName", request.UserName);
                emailParam = new SqlParameter("Email", request.Email);
                passwordParam = new SqlParameter("Password", _encryptions.EncryptPasswordToBase64(request.Password));
                firstNameParam = new SqlParameter("FirstName", request.FirstName);
                lastNameParam = new SqlParameter("LastName", request.LastName);

                userIdOutParam = new SqlParameter("UserId", SqlDbType.UniqueIdentifier)
                {
                    Direction = ParameterDirection.Output
                };
            }

            cmd.Parameters.Add(userNameParam);
            cmd.Parameters.Add(emailParam);
            cmd.Parameters.Add(passwordParam);
            cmd.Parameters.Add(firstNameParam);
            cmd.Parameters.Add(lastNameParam);

            int userIdIdx = cmd.Parameters.Add(userIdOutParam);

            if (connection.State != ConnectionState.Open)
                await _dbContext.Database.OpenConnectionAsync();

            await cmd.ExecuteScalarAsync();

            return  new RegisterResponse { Success = true };
            
        }
    }
}