using GamesApi.Data;
using GamesApi.Dtos;
using GamesApi.Models;
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

        [HttpGet("/games/{pageNumber:int}/{pageSize:int}")]
        public async Task<ActionResult<PagedDto>> GetGames(int pageNumber, int pageSize)   
        {
            try
            {
                var count = await _gamesDataContext.Games.CountAsync();
                if (pageSize * pageNumber > count)
                {
                    return StatusCode(400, new ResultDto<string>("Página não existente"));
                }

                var games = await _gamesDataContext.Games.OrderBy(x=>x.Name).Skip(pageSize*pageNumber).Take(pageSize).ToListAsync();
                var remainingPages = (count / pageSize + (count % pageSize != 0 ? 1 : 0)) - (pageNumber + 1);

                return Ok(new PagedDto(pageNumber,count,remainingPages,games,new List<string>()));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultDto<string>("Erro interno"));
            }
        }
        [HttpGet("/games/{id:int}")]
        public async Task<ActionResult<ResultDto<Game>>> GetGamesById([FromRoute]int id)
        {
            try
            {
                var game = await _gamesDataContext.Games.FirstOrDefaultAsync(x => x.Id==id);
                if(game == null)
                {
                    return NotFound(new ResultDto<Game>("Para o Id informado, não foi possível encontrar nenhum Jogo"));
                }
                return Ok(new ResultDto<Game>(game));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultDto<string>("Erro interno"));
            }
        }

        [HttpPost("/games")]
        public async Task<ActionResult<ResultDto<Game>>> PostGame([FromBody]GamePostDto model)
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
                return Ok(new ResultDto<Game>(game));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultDto<string>("Erro interno"));
            }
        }
        [HttpPut("/games/{id:int}")]
        public async Task<ActionResult<ResultDto<Game>>> PutAsync([FromRoute] int id, [FromBody] GamePostDto model)
        {
            try
            {
                var game = await _gamesDataContext.Games.FirstOrDefaultAsync(x => x.Id == id);
                if (game == null)
                {
                    return NotFound(new ResultDto<Game>("Para o Id informado, não foi possível encontrar nenhum Jogo"));
                }
                game.Name = model.Name;
                game.Description = model.Description;
                game.Developer = model.Developer;
                game.LaunchDate = model.LaunchDate;
                _gamesDataContext.Update(game);
                await _gamesDataContext.SaveChangesAsync();
                return Ok(new ResultDto<Game>(game));
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new ResultDto<Game>("Erro no Banco de dados"));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultDto<Game>("Erro interno"));
            }
        }
        [HttpDelete("/games/{id:int}")]
        public async Task<ActionResult<ResultDto<Game>>> DeleteAsync([FromRoute]int id)
        {
            try
            {
                var game = await _gamesDataContext.Games.FirstOrDefaultAsync(y => y.Id == id);
                if (game == null)
                {
                    return NotFound(new ResultDto<Game>("Para o Id informado, não foi possível encontrar nenhum Jogo"));
                }
                _gamesDataContext.Games.Remove(game);
                await _gamesDataContext.SaveChangesAsync();
                return Ok(new ResultDto<Game>(game));
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new ResultDto<Game>("Erro no Banco de dados"));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultDto<Game>("Erro interno"));
            }
        }
    }
}
