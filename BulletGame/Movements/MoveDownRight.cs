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

        //velocity: (-1,1)
        public override Vector2 getNewPosition(Vector2 position, Vector2 velocity)
        {
            Movement down = new MoveDown(seconds);
            Movement right = new MoveRight(seconds);
            Vector2 temp = right.getNewPosition(position, velocity);
            return down.getNewPosition(temp, velocity);
        }
    }
}
