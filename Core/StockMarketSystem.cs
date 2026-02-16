using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace CalRemix.Core
{
    public class StockMarketSystem : ModSystem
    {
        public static string[] StockList =
        {
            "CAL",
            "THR",
            "SLR",
            "SPR",
            "RMX",
            "WTG",
            "GZM",
            "ARS"
        };

        public static Dictionary<string, int> Stocks; // key is stock name, value is stock price

        public override void Load()
        {
            Stocks = new Dictionary<string, int>()
            {
                { "CAL", Item.buyPrice(gold: 2) },
                { "THR", Item.buyPrice(silver: 75) },
            };
        }

        public override void Unload()
        {
            Stocks = null;
        }

        public override void SaveWorldData(TagCompound tag)
        {
            foreach (var (name, value) in Stocks) tag["stockPrice" + name] = value;
        }

        public override void LoadWorldData(TagCompound tag)
        {
            foreach (var (name, value) in Stocks) Stocks[name] = tag.GetInt("stockPrice" + name);
        }

        public override void PostUpdateTime()
        {
            if (Main.hardMode && Main.dayTime) // trading is open
            {
                if (Main.time % 300 == 0) // condition to update stocks
                {
                    foreach (var (name, value) in Stocks)
                    {
                        if (Main.eclipse) Stocks[name] = (int)(value * (Main.rand.Next(980, 1005) / 1000.0));
                        else Stocks[name] = (int)(value * (Main.rand.Next(995, 1010) / 1000.0));
                    }
                }
            }
        }

        public static string GetStockPrices() // debug
        {
            string ret = "";
            foreach (var (name, value) in Stocks) ret += name + ": " + value.ToString() + "\n";
            return ret;
        }
    }
}
