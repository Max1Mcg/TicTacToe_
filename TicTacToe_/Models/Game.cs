using System.ComponentModel.DataAnnotations.Schema;

namespace TicTacToe_.Models
{
    public class Game
    {
        public int Id { get; set; }
        public string status { get; set; } = "not started";
        public string field { get; set; } = ".........";
        public int turn { get; set; }
        public Player? p1 { get; set; }
        public Player? p2 { get; set; }
    }
}
