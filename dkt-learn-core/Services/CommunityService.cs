using System.Security.Claims;
using dkt_learn_core.shared.Models.Dtos;
using dkt_learn_core.shared.Models.Models;
using DKT_Learn.Data;
using Microsoft.EntityFrameworkCore;

namespace dkt_learn_core.Services;

public class CommunityService : ICommunityService
{
    private readonly AppDbContext context;
        
    public CommunityService(AppDbContext context)
    {
        this.context = context;
    }
    
    public async Task<Post?> CreatePostAsync(string Username, string Titulo, string Conteudo, Guid userId)
    {
        var post = new Post
        {
            Id = Guid.NewGuid(),
            Username = Username,
            Titulo = Titulo,
            Conteudo = Conteudo,
            CriadoEm = DateTime.UtcNow,
            UserId = userId
        };
        context.Posts.Add(post);
        await context.SaveChangesAsync();
        return post;
    }
    public async Task<IEnumerable<PostDto>> GetPostsAsync()
    {
        var posts = await context.Posts
            .Include(p => p.User)
            .Include(p => p.Replies)
            .Include(p => p.Likes)
            .ThenInclude(l => l.User)
            .ToListAsync();

        var response = posts.Select(p => new PostDto
        {
            Username = p.User.Username,
            Titulo = p.Titulo,
            Conteudo = p.Conteudo,
            CriadoEm = p.CriadoEm,
            Replies = p.Replies.Select(r => new ReplyDto
            {
                Username = r.Username,
                Conteudo = r.Conteudo,
                CriadoEm = r.CriadoEm
            }).ToList(),
            Likes = p.Likes.Select(l => l.User.Username).ToList() 
        }).ToList();

        return response;
    }
    public async Task<Post?> UpdatePostAsync(Guid postId, Guid userId, string titulo, string conteudo)
    {
        var post = await context.Posts.FindAsync(postId);
        if (post == null)
            return null;
        if (post.UserId != userId)
            throw new UnauthorizedAccessException("Você só pode editar seus próprios posts.");
        
        post.Titulo = titulo;
        post.Conteudo = conteudo;
        await context.SaveChangesAsync();
        return post;
    }
    
    public async Task<ReplyDto?> CreateReplyAsync(Guid postId, CreateReplyDto request, Guid userId, 
        string username)
    {
        var post = await context.Posts.FindAsync(postId);
        if (post == null) 
            return null;

        var reply = new Reply
        {
            Conteudo = request.Conteudo,
            CriadoEm  = DateTime.UtcNow,
            PostId = postId,
            UserId = userId,
            Username = username 
        };

        context.Replies.Add(reply);
        await context.SaveChangesAsync();

        return new ReplyDto
        {
            Conteudo = reply.Conteudo,
            CriadoEm = reply.CriadoEm,
            Username = reply.Username
        };
    }
    public async Task<Post?> DeletePostAsync(Guid id, Guid userId)
    {
        var post = await context.Posts
            .Include(p => p.Replies)
            .Include(p => p.Likes)
            .FirstOrDefaultAsync(p => p.Id == id);
        if (post == null)
            return null;

        if (post.UserId != userId)
            throw new UnauthorizedAccessException("Você só pode deletar seus próprios posts.");

        context.Replies.RemoveRange(post.Replies);
        context.Likes.RemoveRange(post.Likes);
        context.Posts.Remove(post);

        await context.SaveChangesAsync();
        return post;
    }
    public async Task<Like?> LikePostAsync(Guid postId, Guid userId)
    {
        var user = await context.Users.FindAsync(userId);
        if (user == null)
            return null;

        var alreadyLiked = await context.Likes.AnyAsync(l => l.PostId == postId && l.UserId == userId);
        if (alreadyLiked)
            throw new UnauthorizedAccessException("Você já curtiu esse post.");

        var like = new Like
        {
            PostId = postId,
            UserId = userId
        };

        context.Likes.Add(like);
        await context.SaveChangesAsync();

        return like;
    }

}
    