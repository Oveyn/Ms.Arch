using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ms.Arch.Hw02.Api.Application.Models;

namespace Ms.Arch.Hw02.Api.Application.Infrastructure.Interfaces
{
    public interface IUserRepository
    {
        public Task<IReadOnlyList<UserModel>> Get(CancellationToken ct);
        public Task<UserModel> Get(long Id, CancellationToken ct);
        public Task<long> Create(UserModel user, CancellationToken ct);
        public Task Update(UserModel user, CancellationToken ct);
        public Task Delete(long user, CancellationToken ct);
    }
}