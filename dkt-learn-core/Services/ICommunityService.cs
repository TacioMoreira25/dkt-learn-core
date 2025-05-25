using dkt_learn_core.shared.Models.Dtos;
using dkt_learn_core.shared.Models.Models;

namespace dkt_learn_core.Services;

public interface ICommunityService
{
    Task<Post?> CreatePostAsync(string Username,string Titulo, string Conteudo, Guid userId);
    Task<IEnumerable<PostDto>> GetPostsAsync();
    Task<Post?> UpdatePostAsync(Guid postId, Guid userId, string titulo, string conteudo);
     Task<ReplyDto?> CreateReplyAsync(Guid postId, CreateReplyDto request, Guid userId, string username);
     Task<Post?> DeletePostAsync(Guid id, Guid userId);
     Task<bool> LikePostAsync(Guid postId, Guid userId);

}