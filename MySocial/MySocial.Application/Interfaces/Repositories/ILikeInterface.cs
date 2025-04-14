using MySocial.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySocial.Application.Interfaces.Repositories
{
    public interface ILikeInterface
    {
        bool AddLike(int postId, string userId);
        void RemoveLike(int postId, string userId);
        IEnumerable<Like> GetLikes(int postId);
    }
}
