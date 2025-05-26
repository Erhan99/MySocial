using MySocial.Application.DTOs.User;
using MySocial.Application.Interfaces.Repositories;
using MySocial.Infrastructure.Data;
using MySocial.Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySocial.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        public readonly MSDbContext _context;

        public UserRepository(MSDbContext context)
        {
            _context = context;
        }
        public IEnumerable<UserDTO> GetAll()
        {
            var users = _context.Users.Select(u => new UserDTO()
            {
                Id = u.Id,
                UserName = u.UserName,
                ProfilePictureUrl = u.ProfilePictureUrl
            }).ToList();
            if (users == null) return null;
            return users;
        }

        public UserDTO FindById(string id)
        {
            var user = _context.Users.Select(x => new UserDTO
            {
                Id = x.Id,
                UserName = x.UserName,
                ProfilePictureUrl = x.ProfilePictureUrl
            }).FirstOrDefault(x => x.Id == id);

            if (user == null) return null;

            return user;
        }
    }
}
