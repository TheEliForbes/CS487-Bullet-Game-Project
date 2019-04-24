using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleGame.Movements
{
    class MoveDownLeft : Movement
    {
        public MoveDownLeft(double newSeconds) : base(newSeconds)
        {
        }

        //velocity: (-1,1)
        public override Vector2 getNewPosition(Vector2 position, Vector2 velocity)
        {
            Movement down = new MoveDown(seconds);
            Movement left = new MoveLeft(seconds);
            Vector2 temp = down.getNewPosition(position, velocity);
            return left.getNewPosition(temp, velocity);
        }
    }
}
