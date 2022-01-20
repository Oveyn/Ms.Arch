using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ms.Arch.Hw02.Api.Application.Models;

namespace Ms.Arch.Hw02.Api.Application.Infrastructure.Interfaces
{
    public interface IUserService
    {
        public Task<IReadOnlyList<UserModel>> Get(CancellationToken ct);
        public Task<UserModel> Get(long userId, CancellationToken ct);
        public Task<UserModel> Create(UserModel user, CancellationToken ct);
        public Task<UserModel> Update(UserModel user, CancellationToken ct);
        public Task Delete(long userId, CancellationToken ct);
    }
}