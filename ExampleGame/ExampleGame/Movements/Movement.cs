using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleGame.Movements
{
    public abstract class Movement
    {
        public Double seconds;
        public Movement(Double newSeconds)
        {
            seconds = newSeconds;
        }
        public abstract Vector2 getNewPosition(Vector2 position, Vector2 velocity);
    }
}
