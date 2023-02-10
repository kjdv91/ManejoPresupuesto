using Dapper;
using ManejoPresupuesto.Models;
using Microsoft.Data.SqlClient;

namespace ManejoPresupuesto.Services
{
    public class RepositoryAccountType: IRepositoryAccountType
    {   private readonly string connectionString;
        
        public RepositoryAccountType(IConfiguration configuration) {
            this.connectionString = configuration.GetConnectionString("DefaultConnection");


        }

        public async Task<IEnumerable<AccountType>> GetUserId (int userId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<AccountType>(
                @"
                SELECT AccountTypeId, Nombre, Orden FROM AccountType
                WHERE UserId = @UserId", new {userId});
        }
        

        public async Task Create (AccountType accountType)
        {
            using var connection = new SqlConnection(connectionString);
            //llamo a sp
            var id = await connection.QuerySingleAsync<int>("AccountTypes_Insert", 
                new {userId = accountType.UserId,
                    name = accountType.Name}, commandType: System.Data.CommandType.StoredProcedure);
                
            
            accountType.AccountTypeId = id;
        }

       

        public async Task Update (AccountType accountType)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"UPDATE AccountType
                    SET Nombre = @Name WHERE AccountTypeId = @AccountTypeId", accountType);
        }


        public async Task Delete(int id)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"DELETE AccountType 
                               WHERE AccountTypeId = @AccountTypeId", new { id });
        }



        public async Task <AccountType> GetAccountId(int id, int userId)
        {
            using var connection = new SqlConnection(connectionString);
            
            return await connection.QueryFirstOrDefaultAsync<AccountType>(@"SELECT
                    Nombre, Orden  FROM AccountType 
                    WHERE AccountTypeId = @AccountTypeId AND UserId = @UserId",
                    new {id, userId});

        }


        public async Task<bool> Exits(string name, int userId)
        {
            using var connection = new SqlConnection(connectionString);

            var exists = await connection.QueryFirstOrDefaultAsync<int>(
                @"SELECT 1 
                FROM AccountType 
                WHERE Nombre = @Name AND UserId = @UserId;",
                new { name, userId });

            return exists == 1;
        }

    }
}
