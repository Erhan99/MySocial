using MySocial.Application.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySocial.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        public IEnumerable<UserDTO> GetAll();
    }
}
