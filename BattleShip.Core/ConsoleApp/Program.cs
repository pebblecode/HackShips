using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    using System.Device.Location;

    using BattleShip.Core;

    public class Program
    {
        public static void Main()
        {
            var p1 = new Player("A@b.com", "A");
            var p2 = new Player("B@b.com", "B");

            const double playerTargetZoneRadius = 2.0;
            const double shotBlastRadius = 1.0;

            var game = new Game("Test", p1, p2, playerTargetZoneRadius, shotBlastRadius);

            p1.UpdateLocation(new GeoCoordinate(0.0, 0.0), playerTargetZoneRadius);
            p2.UpdateLocation(new GeoCoordinate(5.0, 5.0), playerTargetZoneRadius);

            var result = game.TakeShot(p2, p1.Location);
            Console.WriteLine(result);
            
            result = game.TakeShot(p2, p1.Location);
            Console.WriteLine(result);

            result = game.TakeShot(p1, p1.Location);
            Console.WriteLine(result);

            Console.ReadKey();
        }
    }
}
