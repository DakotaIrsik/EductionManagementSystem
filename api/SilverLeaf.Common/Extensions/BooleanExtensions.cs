using System;

namespace SilverLeaf.Common.Extensions
{
    public static class BooleanExtensions
    {
        public static bool? GenerateRandomNullableBool()
        {
            var random = new Random();
            var randomGenerator = random.Next(0, 2);
            bool? response = null;

            switch (randomGenerator)
            {
                case 1:
                    response = true;
                    break;
                case 2:
                    response = false;
                    break;
            }
            return response;
        }
    }
}
