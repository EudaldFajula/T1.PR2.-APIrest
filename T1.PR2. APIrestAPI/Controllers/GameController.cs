using T1.PR2._APIrestAPI.Context;
using T1.PR2._APIrestAPI.DTO;
using T1.PR2._APIrestAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace T1.PR2._APIrestAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameController : ControllerBase
    {
        private readonly AppDbContext _context;
        public GameController(AppDbContext context) { _context = context; }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetGameDTO>>> GetAllGames()
        {
            var games = await _context.Games
                 .Include(g => g.VotingUsers)
                 .ToListAsync();

            if (games.Count == 0) { return NotFound("No games found"); }
            
            var gameDTO = games.Select(g => new GetGameDTO
            {
                Id = g.Id,
                Title = g.Title,
                Description = g.Description,
                DeveloperTeam = g.DeveloperTeam,
                ImgUrl = g.ImgUrl,
                VotingUsers = g.VotingUsers.Select(u => u.Name).ToList()
            }).ToList();

            return Ok(gameDTO);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetGameDTO>> GetById(int id)
        {
            var game = await _context.Games
                .Include(g => g.VotingUsers)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (game == null) { return NotFound($"Game {id} not found"); }

            var gameDTO = new GetGameDTO
            {
                Id = game.Id,
                Title = game.Title,
                Description = game.Description,
                DeveloperTeam = game.DeveloperTeam,
                ImgUrl = game.ImgUrl,
                VotingUsers = game.VotingUsers.Select(u => u.Name).ToList()
            };

            return Ok(gameDTO);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<Game>> AddGame(GetGameDTO gameDTO)
        {
            try
            {
                if (gameDTO == null) return BadRequest("Game data is required");

                var game = new Game
                {
                    Title = gameDTO.Title,
                    Description = gameDTO.Description,
                    DeveloperTeam = gameDTO.DeveloperTeam,
                    ImgUrl = gameDTO.ImgUrl,
                };

                _context.Games.Add(game);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetById), new { id = game.Id }, game);
            }
            catch (DbUpdateException)
            {
                return BadRequest("Error while adding the game");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("delete/{id}")]
        public async Task<ActionResult<Game>> DeleteGame(int id)
        {
            try
            {
                var game = await _context.Games.FindAsync(id);
                _context.Games.Remove(game);
                await _context.SaveChangesAsync();
                return Ok(game);
            }
            catch (DbUpdateException)
            {
                return BadRequest($"The delete failed");
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult<Game>> UpdateGame(GetGameDTO gameDTO, int id)
        {
            var games = await _context.Games.FindAsync(id);
            var updateGame = new Game { Title = gameDTO.Title, Description = gameDTO.Description, DeveloperTeam = gameDTO.DeveloperTeam };
            try
            {
                _context.Games.Update(updateGame);
                await _context.SaveChangesAsync();
                return Ok(updateGame);
            }catch (DbUpdateException)
            {
                return BadRequest("The update failed");
            }
        }
        [Authorize]
        [HttpPost("vote")]
        public async Task<ActionResult> Vote(int gameId, string name)
        {
            try
            {
                var game = await _context.Games.FindAsync(gameId);
                var user = await _context.Users.OfType<User>().FirstOrDefaultAsync(u => u.Name == name);

                var userHasVoted = game.VotingUsers.Any(u => u.Name == name);

                if (userHasVoted)
                {
                    user.VotedGames.Remove(game);
                    game.VotingUsers.Remove(user);
                    await _context.SaveChangesAsync();
                    return Ok($"Vote removed");
                }
                else
                {
                    user.VotedGames.Add(game);
                    game.VotingUsers.Add(user);
                    await _context.SaveChangesAsync();
                    return Ok($"Game voted");
                }
            }
            catch (DbUpdateException)
            {
                return BadRequest("Error while voting");
            }
        }
    }
}
