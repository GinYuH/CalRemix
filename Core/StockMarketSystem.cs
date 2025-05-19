using System.Collections.Generic;
using System.Linq;
using CalamityMod;
using CalamityMod.CalPlayer;
using CalamityMod.Events;
using CalamityMod.Items.Armor.Demonshade;
using CalamityMod.World;
using CalRemix.Content.DifficultyModes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using static CalamityMod.CalamityUtils;

namespace CalRemix.Core
{
    public class StockMarketSystem : ModSystem
    {
        public static Dictionary<string, int> Stocks; // key is stock name, value is stock price
        public override void Load()
        {
            Stocks = new Dictionary<string, int>()
            {
                { "CAL", Item.sellPrice(gold: 2) },
                { "THR", Item.sellPrice(silver: 75) },
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
            if(Main.hardMode && Main.dayTime) // trading is open
            {
                if(Main.time % 300 == 0) // condition to update stocks
                {
                    foreach(var (name, value) in Stocks)
                    {
                        if (Main.eclipse) Stocks[name] = (int)(value * (Main.rand.Next(980, 1005) / 1000.0));
                        else Stocks[name] = (int)(value * (Main.rand.Next(995, 1010) / 1000.0));
                    }
                }
            }
        }

        public static void PrintStockPrices() // debug
        {
            foreach (var (name, value) in Stocks) Main.NewText(name + ": " + value.ToString());
        }
    }
}
