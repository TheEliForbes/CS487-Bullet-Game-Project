using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace ExampleGame.Movements
{
    public class MoveLeft : Movement
    {
        public MoveLeft(double newSeconds) : base(newSeconds)
        {
        }

        public override Vector2 getNewPosition(Vector2 position, Vector2 velocity)
        {
            Vector2 ret = position;
            ret.X += velocity.X;
            ret.Y = position.Y;

            return ret;
        }
    }
}
