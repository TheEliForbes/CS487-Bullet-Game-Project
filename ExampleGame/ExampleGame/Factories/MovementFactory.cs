using ExampleGame.Movements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleGame.Factories
{
    class MovementFactory
    {
        public Movement makeMovement(string type, double duration)
        {
            type = type.ToLower();
            switch (type)
            {
                case "moveright":
                    return new MoveRight(duration);
                case "moveleft":
                    return new MoveLeft(duration);
                case "moveup":
                    return new MoveUp(duration);
                case "movedown":
                    return new MoveDown(duration);
                case "movedownleft":
                    return new MoveDownLeft(duration);
                case "movedownright":
                    return new MoveDownRight(duration);
                case "moveupleft":
                    return new MoveUpLeft(duration);
                case "moveupright":
                    return new MoveUpRight(duration);
            }
            return null;
        }
    }
}
