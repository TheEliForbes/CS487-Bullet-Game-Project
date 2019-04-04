using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleGame.Movements
{
    class MoveUpLeft : Movement
    {
        public MoveUpLeft(double newSeconds) : base(newSeconds)
        {
        }

        public override Vector2 getNewPosition(Vector2 position, Vector2 velocity)
        {
            Vector2 ret = position;
            ret.Y -= velocity.Y;
            ret.X -= velocity.Y;

            return ret;

            //TODO - Fix this, it only moves up
            //Movement up = new MoveUp(seconds);
            //Movement left = new MoveLeft(seconds);
            //Vector2 temp = left.getNewPosition(position, velocity);
            //return up.getNewPosition(temp, velocity);
        }
    }
}
