using System;
using System.Device.Location;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;
using BattleShip.Core;
using BattleShip.Web.Models;
using BattleShip.Web.Models.Game;

namespace BattleShip.Web.Controllers
{
    public class GameController : Controller
    {
        public JsonResult Create(string name, string myemail, string opponentemail)
        {

            var gameId = MvcApplication.GameHost.CreateGame(name, myemail, myemail, opponentemail, opponentemail);
            MvcApplication.GameHost.SetPlayerLocation(gameId, myemail, GetRandomCenter());
            MvcApplication.GameHost.SetPlayerLocation(gameId, opponentemail, GetRandomCenter());

            this.Session[MagicStrings.SessionGameId] = gameId;
            this.Session[MagicStrings.SessionPlayerEmail] = myemail;

            SendEmail(gameId.ToString(), opponentemail);
            SendEmail(gameId.ToString(), myemail);

            return this.Json(null, JsonRequestBehavior.AllowGet);
        }

        private void SendEmail(string gameId, string email)
        {
            var message = new MailMessage("sava.varadzhakov@bedegaming.com", email)
                              {
                                  Subject = "Join the magnificent game of BattleShips",
                                  Body = String.Format("http://battleship.dev/Game/JoinGame?gameId={0}&email={1}", gameId, email)
                              };
            var client =
                new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential("sava.varadzhakov@bedegaming.com", "battleship")
                };
            
            client.Send(message);
        }

        public ActionResult NewGame()
        {
            return this.View();
        }

        public ActionResult JoinGame(string gameId, string email)
        {
            this.Session[MagicStrings.SessionGameId] = Guid.Parse(gameId);
            this.Session[MagicStrings.SessionPlayerEmail] = email;

            return this.Index();
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Shoot(GeoCoordinates latLong)
        {
            var model = new ShootVModel()
                            {
                                InTheTarget = false
                            };
            return this.Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTargetZone()
        {
            Guid? gameId = this.Session[MagicStrings.SessionGameId] as Guid?;
            string email = this.Session[MagicStrings.SessionPlayerEmail].ToString();
            TargetZone targetZone = MvcApplication.GameHost.GetOpponentTargetZone(gameId.Value, email);
            var model = new GetTargetZoneVModel()
                            {
                                Latitude = targetZone.Center.Latitude,
                                Longitude = targetZone.Center.Longitude,
                                Radius = ConvertGeoDistanceToMeters(targetZone.Radius)
                            };
            return this.Json(model, JsonRequestBehavior.AllowGet);
        }

        public const double LatMax = 51.75;
        public const double LatMin = 51.25;
        public const double LongMax = 0.075;
        public const double LongMin = -0.5;

        private GeoCoordinate GetRandomCenter()
        {
            var rand = new Random();
            return new GeoCoordinate(
                LatMin + (LatMax - LatMin) * rand.NextDouble(),
                LongMin + (LongMax - LongMin) * rand.NextDouble()
                );

        }

        private double ConvertGeoDistanceToMeters(double geodistance)
        {
            var start = new GeoCoordinate(0.0, 0.0);
            var end = new GeoCoordinate(0.0, geodistance);
            return start.GetDistanceTo(end);
        }

    }
}

