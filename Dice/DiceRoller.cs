using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameTools.Dice
{
    public class DiceRoller
    {
        public static Random randomClass = new Random((int)DateTime.Now.Ticks);

        public enum DiceType
        {
            D4 = 4,
            D6 = 6,
            D8 = 8,
            D10 = 10,
            D12 = 12,
            D20 = 20,
            D100 = 100
        }

        public static int Throw(DiceType diceType)
        {
            /*byte[] array = new byte[1];
            System.Security.Cryptography.RandomNumberGenerator.Create().GetNonZeroBytes(array);
            return (array[0] % (int)diceType) + 1;*/
            return randomClass.Next(1, (int)diceType + 1);
        }

        public static int Throw(int diceValue)
        {
            if (diceValue > 0)
            {
                /*byte[] array = new byte[1];
                System.Security.Cryptography.RandomNumberGenerator.Create().GetNonZeroBytes(array);
                return (array[0] % diceValue) + 1;*/
                return randomClass.Next(1, diceValue + 1);
            }
            else return 0;
        }
    }
}
