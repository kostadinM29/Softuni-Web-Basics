using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleCards.ViewModels.CardModels;
using BattleCards.ViewModels.UserModels;

namespace BattleCards.Services
{
    public interface IValidator
    {
        ICollection<string> ValidateUser(UserRegisterModel model);

        ICollection<string> ValidateCard(CardAddFormModel model);
    }
}
