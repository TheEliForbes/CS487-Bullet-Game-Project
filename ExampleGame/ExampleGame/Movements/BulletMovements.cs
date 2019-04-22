using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleGame.Movements
{
    class BulletMovements
    {
        private List<Movement> moves;
       
        public BulletMovements()
        {
            moves = new List<Movement>();
        }
        public void addMovement(Movement newmove)
        {
            moves.Add(newmove);
        }
        public bool popMovement()
        {
            if(moves.Count > 0)
            {
                moves.RemoveAt(0);
                return true;
            }
            return false;
        }
        public Movement GetMovement()
        {
            if(moves.Count > 0)
                return moves[0];
            return new MoveDown(5.0);
        }
        public bool isEmpty()
        {
            if(moves.Count > 0)
            {
                return false;
            }
            return true;
        }

    }
}
