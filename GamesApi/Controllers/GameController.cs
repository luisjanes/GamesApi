using GamesApi.Data;
using GamesApi.Models;
using GamesApi.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GamesApi.Controllers
{
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly GamesDataContext _gamesDataContext;

        public GameController([FromServices]GamesDataContext gamesDataContext)
        {
            _gamesDataContext = gamesDataContext;
        }

        [HttpGet("/games")]
        public async Task<ActionResult<ResultViewModel<List<Game>>>> GetGames()   
        {
            try
            {
                var games = await _gamesDataContext.Games.OrderBy(x=>x.Name).ToListAsync();
                return Ok(new ResultViewModel<List<Game>>(games));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<string>("Erro interno"));
            }
        }
        [HttpGet("/games/{id:int}")]
        public async Task<ActionResult<ResultViewModel<Game>>> GetGamesById([FromRoute]int id)
        {
            try
            {
                var game = await _gamesDataContext.Games.FirstOrDefaultAsync(x => x.Id==id);
                if(game == null)
                {
                    return NotFound(new ResultViewModel<Game>("Para o Id informado, não foi possível encontrar nenhum Jogo"));
                }
                return Ok(new ResultViewModel<Game>(game));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<string>("Erro interno"));
            }
        }

        [HttpPost("/games")]
        public async Task<ActionResult<ResultViewModel<Game>>> PostGame([FromBody]GamePost model)
        {
            try
            {
                var game = new Game
                {
                    Id = 0,
                    Name = model.Name,
                    Description = model.Description,
                    Developer = model.Developer,
                    LaunchDate = model.LaunchDate,
                };
                await _gamesDataContext.Games.AddAsync(game);
                await _gamesDataContext.SaveChangesAsync();
                return Ok(new ResultViewModel<Game>(game));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<string>("Erro interno"));
            }
        }
        [HttpPut("/games/{id:int}")]
        public async Task<ActionResult<ResultViewModel<Game>>> PutAsync([FromRoute] int id, [FromBody] GamePost model)
        {
            try
            {
                var game = await _gamesDataContext.Games.FirstOrDefaultAsync(x => x.Id == id);
                if (game == null)
                {
                    return NotFound(new ResultViewModel<Game>("Para o Id informado, não foi possível encontrar nenhum Jogo"));
                }
                game.Name = model.Name;
                game.Description = model.Description;
                game.Developer = model.Developer;
                game.LaunchDate = model.LaunchDate;
                _gamesDataContext.Update(game);
                await _gamesDataContext.SaveChangesAsync();
                return Ok(new ResultViewModel<Game>(game));
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new ResultViewModel<Game>("Erro no Banco de dados"));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<Game>("Erro interno"));
            }
        }
        [HttpDelete("/games/{id:int}")]
        public async Task<ActionResult<ResultViewModel<Game>>> DeleteAsync([FromRoute]int id)
        {
            try
            {
                var game = await _gamesDataContext.Games.FirstOrDefaultAsync(y => y.Id == id);
                if (game == null)
                {
                    return NotFound(new ResultViewModel<Game>("Para o Id informado, não foi possível encontrar nenhum Jogo"));
                }
                _gamesDataContext.Games.Remove(game);
                await _gamesDataContext.SaveChangesAsync();
                return Ok(new ResultViewModel<Game>(game));
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new ResultViewModel<Game>("Erro no Banco de dados"));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<Game>("Erro interno"));
            }
        }
    }
}
