using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ms.Arch.Hw02.Api.Application.Infrastructure.Interfaces;
using Ms.Arch.Hw02.Api.Application.Models;

namespace Ms.Arch.Hw02.Api.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IReadOnlyList<UserModel>> Get(CancellationToken ct)
        {
            var users = await _userRepository.Get(ct);
            return users;
        }

        public async Task<UserModel> Get(long userId, CancellationToken ct)
        {
            var user = await _userRepository.Get(userId, ct);
            return user;
        }

        public async Task<UserModel> Create(UserModel user, CancellationToken ct)
        {
            var userId = await _userRepository.Create(user, ct);
            var userModel = await _userRepository.Get(userId, ct);
            return userModel;
        }

        public async Task<UserModel> Update(UserModel user, CancellationToken ct)
        {
            await _userRepository.Update(user, ct);
            var userModel = await _userRepository.Get(user.Id, ct);
            return userModel;
        }

        public async Task Delete(long userId, CancellationToken ct)
        {
            await _userRepository.Delete(userId, ct);
        }
    }
}