using System.Collections.Generic;
using System.Text;
using HarmonyLib;
namespace PisCondensedCrafting
{
    [HarmonyPatch(typeof(PlayerCamera), "IngredientTextForRecipe")]
    public static class IngredientTextPatch
    {
        [HarmonyPostfix]
        public static void Postfix(ref string __result)
        {
            string[] blocks = __result.Split(
                new[] { "<color=#FFFFFF>" },
                System.StringSplitOptions.RemoveEmptyEntries
            );

            Dictionary<string, int> totalCounts = new Dictionary<string, int>();
            Dictionary<string, int> availableCounts = new Dictionary<string, int>();
            Dictionary<string, string> firstBlock = new Dictionary<string, string>();
            List<string> order = new List<string>();

            foreach (string block in blocks)
            {
                // Get the first line of this ingredient block.
                string firstLine = block.Split('\n')[0];

                // Determine whether the player has this particular item.
                bool available = firstLine.Contains("<sprite index=23>");

                // Remove the sprite tag to get the actual item name.
                string itemName = System.Text.RegularExpressions.Regex.Replace(
                    firstLine,
                    @"<sprite[^>]*>",
                    ""
                ).Trim();

                if (!totalCounts.ContainsKey(itemName))
                {
                    totalCounts[itemName] = 0;
                    availableCounts[itemName] = 0;
                    firstBlock[itemName] = block;
                    order.Add(itemName);
                }

                totalCounts[itemName]++;

                if (available)
                {
                    availableCounts[itemName]++;
                }
            }

            System.Text.StringBuilder result = new System.Text.StringBuilder();

            foreach (string itemName in order)
            {
                string block = firstBlock[itemName];

                int total = totalCounts[itemName];
                int available = availableCounts[itemName];
                string firstLine = block.Split('\n')[0];
                bool haveItem = firstLine.Contains("<sprite index=23>");
                bool notHaveItem = firstLine.Contains("<sprite index=24>");

                // Create the quantity text
                string quantityText;
                if (haveItem == false && notHaveItem == false)
                {
                    quantityText = "<color=#FFD200>(" + available + "/" + total + ")</color>";
                }
                else if (available >= total)
                {
                    quantityText = "<color=#00FF00>(" + available + "/" + total + ")</color>";
                }
                else
                {
                    quantityText = "<color=#FF0000>(" + available + "/" + total + ")</color>";
                }

            /*Put the quantity at the end of the item name
                before the newline*/
                int newlineIndex = block.IndexOf('\n');

                if (newlineIndex >= 0)
                {
                    block = block.Insert(newlineIndex, " " + quantityText);
                }
                else
                {
                    block += " " + quantityText;
                }

                result.Append("<color=#FFFFFF>");
                result.Append(block);
            }

            __result = result.ToString();
        }
    }
}