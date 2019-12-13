using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DouShouQi
{
    [Serializable]
    public class Piece
    {
        // Animal Pieces
        private static string[] LIST_HEWAN =
        {
            "rat",
            "cat",
            "wolf",
            "dog",
            "leopard",
            "tiger",
            "lion",
            "elephant"
        };

        /// <summary>
        /// NAME = NAMA HEWAN
        /// STRENGTH = STR HEWAN
        /// PLAYER = MILIK SIAPA
        /// IS ALIVE = MASIH HIDUP? \\*INI MUNGKIN AKAN DIDELETE*\\
        /// POSITION = POSISI HEWAN [0] == X, [1] == Y
        /// </summary>
       
        public string name;
        public int strength, player;
        public bool isAlive;
        public int[] position;

        // Constructor
        public Piece(int strength, int player)
        {
            this.name = LIST_HEWAN[strength];
            this.strength = strength;
            this.player = player;
            this.isAlive = true;
        }

        public override string ToString()
        {
            return $"{this.name} @({this.position[0]},{this.position[1]}) - Player {this.player}";
        }

    }
}
