using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using dkt_learn_core.shared.Models.Models;
using DKT_Learn.Data;
using dkt_learn_core.shared.Models.Dtos;

namespace dkt_learn_core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModuleController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ModuleController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [Authorize]
        [HttpGet("{id}/conteudos")]
        public async Task<ActionResult<List<ContentResponseDto>>> GetContents(int id)
        {
            var modulo = await _context.Modules
                .Include(m => m.Contents)
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (modulo == null) return NotFound();
            
            return _mapper.Map<List<ContentResponseDto>>(modulo.Contents);
        }
    }
}
