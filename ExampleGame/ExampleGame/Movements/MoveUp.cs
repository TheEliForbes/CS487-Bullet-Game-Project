using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace ExampleGame.Movements
{
    class MoveUp : Movement
    {
        public MoveUp(double newSeconds) : base(newSeconds)
        {
        }

        public override Vector2 getNewPosition(Vector2 position, Vector2 velocity)
        {
            Vector2 ret = position;
            ret.Y += velocity.Y;
            ret.X = position.X;

            return ret;
        }
    }
}
