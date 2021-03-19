using System;
using System.Collections.Generic;

namespace RocketAnt.Util
{
    public static class TaskNameGenerator
    {
        static string[] taskTypes = {
          "Floor plan copier",
          "System backup",
          "User notifier",
          "Data synchronizer"
        };

        public static string GenerateTaskName()
        {
            Random random = new Random();
            int index = random.Next(0, taskTypes.Length - 1);

            Random randomNumberGenerator = new Random();
            int randomTaskNumber = randomNumberGenerator.Next(100, 1000);

            return $"{taskTypes[index]} {randomTaskNumber}";
        }
    }
}