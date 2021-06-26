using System;
using System.Collections.Generic;
using System.Text;

namespace BattleCards.Data.Models
{
    public class UserCard
    {
        public string UserId { get; set; }

        public User User { get; set; }

        public string CardId { get; set; }

        public Card Card { get; set; }
    }
}
