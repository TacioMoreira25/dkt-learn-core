using System.Security.Claims;
using dkt_learn_core.Services;
using dkt_learn_core.shared.Models.Dtos;
using dkt_learn_core.shared.Models.Models;
using DKT_Learn.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dkt_learn_core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommunityController : ControllerBase
    {
        private readonly AppDbContext context;
        private readonly ICommunityService service;
        
        public CommunityController(AppDbContext context, ICommunityService service)
        {
            this.context = context;
            this.service = service;
        }
        
        [Authorize]
        [HttpGet("posts")]
        public async Task<IActionResult> GetAllPosts()
        {
            var posts = await service.GetPostsAsync();
            return Ok(posts);
        }
        
        [Authorize]
        [HttpPost("posts")]
        public async Task<IActionResult> CreatePost(PostResponseDto request)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var Username = User.FindFirstValue(ClaimTypes.Name);
            var post = await service.CreatePostAsync(Username,request.Titulo,request.Conteudo, userId);
            return Ok(post);
        }
        
        [Authorize]
        [HttpPut("posts/{id}")]
        public async Task<IActionResult> UpdatePost(Guid id, PostResponseDto request)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var post = await service.UpdatePostAsync(id, userId, request.Titulo, request.Conteudo);;
            return Ok(post);
        }
        
        [Authorize]
        [HttpDelete("posts/{id}")]
        public async Task<IActionResult> DeletePost(Guid id)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var post = await service.DeletePostAsync(id, userId);
            return Ok("Post deletando com sucesso!");
        }
        
        [Authorize]
        [HttpPost("posts/{postId}/replies")]
        public async Task<IActionResult> ReplyToPost(Guid postId, CreateReplyDto request)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var username = User.FindFirstValue(ClaimTypes.Name);
            var reply = await service.CreateReplyAsync(postId, request, userId, username);
            if (reply == null)
                return NotFound();
            return Ok(reply);
        }
        
        [Authorize]
        [HttpPost("posts/{id}/like")]
        public async Task<IActionResult> LikePost(Guid id)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var like = await service.LikePostAsync(id, userId);
            
            return Ok("Post curtido com sucesso.");
        }
    }
}
