﻿using MySocial.Application.DTOs.Comment;
using MySocial.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySocial.Application.Interfaces.Repositories
{
    public interface ICommentRepository
    {
        public void AddComment(Comment comment);
        public CommentDTO GetCommentById(int commentId);
        public IEnumerable<CommentDTO> GetCommentByPost(int postId);
        public void DeleteComment(int commentId);
        public void UpdateComment(int CommentId, string content);
    }
}
