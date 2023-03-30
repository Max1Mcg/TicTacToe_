using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using TicTacToe_.Database;
using TicTacToe_.Models;
using System.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text;

namespace TicTacToe_.Controllers
{
    [ApiController]
    [Route("/api")]
    public class ContactController : ControllerBase
    {
        //private ContactRepository contactRepository;
        public ContactController()
        {
            //this.contactRepository = new ContactRepository();
        }
        [HttpGet]
        [Route("/Players")]
        public Player[] GetPlayers()
        {
            using (GameContext game = new GameContext())
            {
                return game.Players.ToArray();
            }
        }
        [HttpGet]
        [Route("/Games")]
        public Game[] GetGames()
        {
            using (GameContext game = new GameContext())
            {
                return game.Games.Include(g => g.p1).Include(g => g.p2).ToArray();
            }
        }
        [HttpGet]
        [Route("/NewPlayer")]
        public string NewPlayer(string name)
        {
            using (GameContext game = new GameContext())
            {
                var player = new Player { Name = name};
                game.Players.Add(player);
                game.SaveChanges();
                return $"add player {player}";
            }
        }
        [HttpGet]
        [Route("/NewGame")]
        public string NewGame(int player1, int player2)
        {
            using (GameContext game = new GameContext())
            {
                var player1_ = game.Players.FirstOrDefault(p => p.Id == player1);
                var player2_ = game.Players.FirstOrDefault(p => p.Id == player2);
                if (player1_ == null || player2_ == null || player1_ == player2_)
                    return "wrong id of some player";
                var newGame = new Game {p1 = player1_, p2 = player2_, turn = player1_.Id };
                game.Games.Add(newGame);
                game.SaveChanges();
                return $"add game {newGame}";
            }
        }
        [HttpGet]
        [Route("/Delete")]
        public string Delete()
        {
            using (GameContext game = new GameContext())
            {
                game.Database.EnsureDeleted();
                game.Database.EnsureCreated();
                game.SaveChanges(true);
                return "deleted";
            }
        }
        [HttpGet]
        [Route("/Game")]
        //place - number in range(1, 9)
        public string GameId(int id, int playerId, int place)
        {
            using (GameContext game = new GameContext())
            {
                //check game for exist
                if(game.Games.Where(i => i.Id == id).Count() == 0)
                    return "unknown game";
                var curGame = game.Games.FirstOrDefault(g => g.Id == id);
                if (curGame.turn != playerId)
                    return "wrong player trying do turn";
                if (curGame.field[place] != '.')
                    return "this cell already has value";
                //all good
                //CHECK FOR WIN
                if (curGame.status == "end")
                    return "game ended";
                if (curGame.field)//HERE, ДОБАВИТЬ ПРОВЕРКУ НА КОНЕЦ ИГРЫ
                {

                }
                if (curGame.status == "not started")
                    curGame.status = "started";
                curGame.turn = (curGame.p1.Id == playerId) ? curGame.p2.Id : curGame.p1.Id;
                var sb = new StringBuilder(curGame.field);
                if (curGame.field.Where(g => g == 'k').Count() > curGame.field.Where(g => g == 'o').Count())
                    sb[place] = 'o';
                else
                    sb[place] = 'k';
                curGame.field = sb.ToString();
                game.Games.FirstOrDefault(g => g.Id == id).turn = curGame.turn;
                game.Games.FirstOrDefault(g => g.Id == id).field = curGame.field;
                game.Games.FirstOrDefault(g => g.Id == id).status = curGame.status;
                game.SaveChanges();
                return "Turn was successful";
            }

        }
        [HttpGet]
        [Route("/Test")]
        public string Test()
        {
            var str = "del";
            JsonDocument.Parse(str);
            return ("deleted");
        }
    }
}
