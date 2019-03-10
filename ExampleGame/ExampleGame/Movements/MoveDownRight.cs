using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleGame.Movements
{
    class MoveDownRight : Movement
    {
        public MoveDownRight(double newSeconds) : base(newSeconds)
        {
        }

        public override Vector2 getNewPosition(Vector2 position, Vector2 velocity)
        {
            Vector2 ret = position;
            ret.Y += velocity.Y;
            ret.X += velocity.Y;

            return ret;

            //TODO - Fix this, it only moves it down
            //Movement down = new MoveDown(seconds);
            //Movement right = new MoveRight(seconds);
            //Vector2 temp = right.getNewPosition(position, velocity);
            //return down.getNewPosition(temp, velocity);
        }
    }
}
