using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleCards.Data;
using BattleCards.Data.Models;
using BattleCards.Services;
using BattleCards.ViewModels.CardModels;
using MyWebServer.Controllers;
using MyWebServer.Http;

namespace BattleCards.Controllers
{
    public class CardsController : Controller
    {
        private readonly IValidator validator;
        private readonly BattleCardsDbContext data;

        public CardsController(
            IValidator validator,
            BattleCardsDbContext data)
        {
            this.validator = validator;
            this.data = data;
        }

        [Authorize]
        public HttpResponse Add()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public HttpResponse Add(CardAddFormModel model)
        {
            var modelErrors = validator.ValidateCard(model);

            if (modelErrors.Any())
            {
                return Error(modelErrors);
            }

            var card = new Card()
            {
                Name = model.Name,
                ImageUrl = model.Image,
                Keyword = model.Keyword,
                Attack = model.Attack,
                Health = model.Health,
                Description = model.Description

            };

            this.data.Cards.Add(card);

            this.data.SaveChanges();

            var usercards = new UserCard
            {
                CardId = card.Id,
                UserId = this.User.Id
            };


            this.data.UsersCards.Add(usercards);

            this.data.SaveChanges();

            return Redirect("/Cards/All");
        }
        [Authorize]
        public HttpResponse Collection()
        {
            var cardsQuery = data.UsersCards.Where(uc => uc.UserId == User.Id).AsQueryable();

            var cardsList = cardsQuery
                .Select(uc => new CardsViewModel()
                {
                    CardId = uc.CardId,
                    Name = uc.Card.Name,
                    ImageUrl = uc.Card.ImageUrl,
                    Keyword = uc.Card.Keyword,
                    Attack = uc.Card.Attack,
                    Health = uc.Card.Health,
                    Description = uc.Card.Description
                })
                .ToList();

            return View(cardsList);
        }

        [Authorize]
        public HttpResponse All()
        {
            var cardsQuery = data.Cards.AsQueryable();

            var cardsList = cardsQuery
                .Select(c => new CardsViewModel()
                {
                    CardId = c.Id,
                    Name = c.Name,
                    ImageUrl = c.ImageUrl,
                    Keyword = c.Keyword,
                    Attack = c.Attack,
                    Health = c.Health,
                    Description = c.Description
                })
                .ToList();

            return View(cardsList);
        }

        [Authorize]
        public HttpResponse AddToCollection(string cardId)
        {

            if (data.UsersCards.Any(uc => uc.UserId == User.Id && uc.CardId == cardId))
            {
                return Redirect("/Cards/All ");
            }

            var card = new UserCard()
            {
                UserId = User.Id,
                CardId = cardId
            };
            data.UsersCards.Add(card);

            data.SaveChanges();

            return Redirect("/Cards/All");
        }

        [Authorize]
        public HttpResponse RemoveFromCollection(string cardId)
        {
            var userCard = this.data.UsersCards.FirstOrDefault(uc => uc.UserId == User.Id && uc.CardId == cardId);
            if (userCard == null)
            {
                return Error("You do not own the card!");
            }

            data.UsersCards.Remove(userCard);
            data.SaveChanges();

            return Redirect("/Cards/Collection");
        }
    }
}
