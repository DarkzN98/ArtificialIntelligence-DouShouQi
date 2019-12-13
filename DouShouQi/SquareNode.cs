using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace DouShouQi
{
    [Serializable]
    public class SquareNode
    {
        // SQUARE NODE
        public Piece animal;
        public bool isTrap, isDen, isWater;
        public int denOwner = -1;
        public int trapOwner = -1;

        // CONSTRUCTOR
        public SquareNode()
        {
            animal = null;
            isTrap = false;
            isDen = false;
            isWater = false;
        }

        public void deleteAnimal()
        {
            this.animal = null;
        }

        public void removeAnimal()
        {
            if(animal != null)
            {
                animal.isAlive = false;
            }
        }
        public object DeepClone()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                if (this.GetType().IsSerializable)
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(stream, this);
                    stream.Position = 0;
                    return formatter.Deserialize(stream);
                }
                return null;
            }
        }

        public override string ToString()
        {
            return $"Animal: {this.animal}\nisTrap: {isTrap}\nisDen: {isDen}\nisWater: {isWater}\ntrapOwner:{trapOwner}\ndenOwner:{denOwner}";
        }

    }
}
