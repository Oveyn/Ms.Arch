using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Options;
using Ms.Arch.Hw02.Api.Application.Infrastructure.Interfaces;
using Ms.Arch.Hw02.Api.Application.Models;
using Ms.Arch.Hw02.Api.Persistence.ModelsTvp;
using Npgsql;

namespace Ms.Arch.Hw02.Api.Persistence
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;

        public UserRepository(IOptions<PostgresDbConnect> postgresDbConnect)
        {
            _connectionString = postgresDbConnect.Value.ConnectionString;
        }

        public async Task<IReadOnlyList<UserModel>> Get(CancellationToken ct)
        {
            var getQuery = @"
                select id, firstname, lastname, phone, email
                from account.account;
                ";

            await using var con = GetConnection();

            var userTvp = await con.QueryAsync<UserTvp>(
                new CommandDefinition(getQuery, cancellationToken: ct));

            var userModels = userTvp.Select(user => user.ConvertToUserModel()).ToList();

            return userModels;
        }

        public async Task<UserModel> Get(long Id, CancellationToken ct)
        {
            var getQuery = @"
                select id, firstname, lastname, phone, email
                from account.account
                where id = @id;
                ";

            await using var con = GetConnection();
            var parameters = new DynamicParameters();
            parameters.Add("@id", Id);

            var userTvp = await con.QueryFirstOrDefaultAsync<UserTvp>(
                new CommandDefinition(getQuery, parameters, cancellationToken: ct));

            return userTvp?.ConvertToUserModel();
        }

        public async Task<long> Create(UserModel user, CancellationToken ct)
        {
            var getQuery = @"
                insert into account.account (firstname, lastname, phone, email)
                values(@firstname, @lastname, @phone, @email)
                returning id;
            ";

            await using var con = GetConnection();
            var parameters = new DynamicParameters();
            parameters.Add("@firstname", user.FirstName);
            parameters.Add("@lastname", user.LastName);
            parameters.Add("@phone", user.Phone);
            parameters.Add("@email", user.Email);

            var createdUserId = await con.QuerySingleAsync<long>(
                new CommandDefinition(getQuery, parameters, cancellationToken: ct));

            return createdUserId;
        }

        public async Task Update(UserModel user, CancellationToken ct)
        {
            var getQuery = @"
                update account.account set (firstname, lastname, phone, email) =
                (@firstname, @lastname, @phone, @email)
                where id = @id;
            ";

            await using var con = GetConnection();
            var parameters = new DynamicParameters();
            parameters.Add("@id", user.Id);
            parameters.Add("@firstname", user.FirstName);
            parameters.Add("@lastname", user.LastName);
            parameters.Add("@phone", user.Phone);
            parameters.Add("@email", user.Email);

            await con.ExecuteAsync(
                new CommandDefinition(getQuery, parameters, cancellationToken: ct));
        }

        public async Task Delete(long userId, CancellationToken ct)
        {
            var getQuery = @"
                delete from account.account
                where id = @id;
            ";

            await using var con = GetConnection();
            var parameters = new DynamicParameters();
            parameters.Add("@id", userId);

            await con.ExecuteAsync(
                new CommandDefinition(getQuery, parameters, cancellationToken: ct));
        }

        private NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(_connectionString);
        }
    }
}