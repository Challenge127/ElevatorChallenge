using System;
namespace ElevatorChallenge
{
    public static class InputManager
    {
        public static int ReadInt(int? lowerBound = null, int? upperBound = null)
        {

            int input = 0;

            bool inputValid = false;

            while (inputValid == false)
            {
                int convertedInput;

                string outputMessage = "";

                if (lowerBound != null)
                    outputMessage += "Min: " + (lowerBound);

                if (upperBound != null)
                    outputMessage += " Max: " + (upperBound);

                Console.WriteLine(outputMessage);
                string rawInput = Console.ReadLine();

                inputValid = int.TryParse(rawInput, out convertedInput);

                if (lowerBound != null && convertedInput < lowerBound)
                    inputValid = false;
                if (upperBound != null && convertedInput > upperBound)
                    inputValid = false;

                if (inputValid == true)
                    input = convertedInput;
                else
                    Console.WriteLine("Please input a whole number using numeric characters");
            }
            return input;
        }
    }
}

