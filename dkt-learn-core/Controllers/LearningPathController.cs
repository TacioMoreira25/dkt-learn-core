using System.Security.Claims;
using dkt_learn_core.Services;
using dkt_learn_core.shared.Models.Models;
using dkt_learn_core.shared.Models.Dtos;
using DKT_Learn.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace dkt_learn_core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LearningPathController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public LearningPathController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<List<LearningPathResponseDto>>> GetAll()
        {
            var trilhas = await _context.LearningPaths
                .Include(lp => lp.Modules)
                .ThenInclude(m => m.Contents)
                .ToListAsync();
            
            return _mapper.Map<List<LearningPathResponseDto>>(trilhas);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<LearningPathResponseDto>> GetById(int id)
        {
            var trilha = await _context.LearningPaths
                .Include(lp => lp.Modules)
                .ThenInclude(m => m.Contents)
                .FirstOrDefaultAsync(lp => lp.Id == id);
            
            if (trilha == null) return NotFound();
            
            return _mapper.Map<LearningPathResponseDto>(trilha);
        }

        [Authorize]
        [HttpGet("{id}/modulos")]
        public async Task<ActionResult<List<ModuleResponseDto>>> GetModules(int id)
        {
            var trilha = await _context.LearningPaths
                .Include(lp => lp.Modules)
                .FirstOrDefaultAsync(lp => lp.Id == id);
            
            if (trilha == null) return NotFound();
            
            return _mapper.Map<List<ModuleResponseDto>>(trilha.Modules);
        }
    }
}