using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using CalRemix.Core.World;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ID;
using Terraria;
using CalamityMod.Items.Accessories;
using static Terraria.ModLoader.ModContent;
using CalamityMod.Items.Mounts;
using System.Collections.Generic;
using Terraria.GameContent;
using CalamityMod.Buffs.Mounts;
using CalRemix.UI;
using Terraria.UI;
using Terraria.DataStructures;
using CalRemix.Core.Retheme.Sneakers;
using System;
using Terraria.ModLoader.IO;
using Terraria.Localization;
using System.Text.RegularExpressions;
using System.Text;

namespace CalRemix.Core.Retheme
{
    public class SynonymSwap
    {
        public enum SynonymType : uint
        {
            BarIngot = 1 << 0,
            AxeAx = 1 << 1,
            PickaxePick = 1 << 2,
            SilkFabric = 1 << 3,
            GreavesLeggings = 1 << 4,
            WoodPlanks = 1 << 5,
            PotionElixir = 1 << 6,
            BottleJar = 1 << 7,
            PixieFairy = 1 << 8,
            SpecterSpectre = 1 << 9,
            DyePigment = 1 << 10,
            MagmaLava = 1 << 11,
            RogueRouge = 1 << 12,
            SummonerInvoker = 1 << 13,
            RangerHunter = 1 << 14,
            MageEnchanter = 1 << 15,
            MeleeBrawl = 1 << 16,
            MinionServant = 1 << 17,
            SoulSpirit = 1 << 18,
            StaffScepter = 1 << 19,
            MechRobot = 1 << 20,
            ChairSeat = 1 << 21,
            GemJewel = 1 << 22,
            BandBrace = 1 << 23,
            CrystalPrism = 1 << 24,
            StoneRock = 1 << 25,
            KnockbackPushback = 1 << 26,

            AllSwaps = ~0u,
        }

        public static uint SynonymToggles;
        private static Dictionary<string, string> ActiveSynonymReplacements = new Dictionary<string, string>();
        private static string RegexMatchUppercase;
        private static string RegexMatchLowercase;
        private static StringBuilder regexUppercaseBuilder = new StringBuilder(32 * 16);
        private static StringBuilder regexLowercaseBuilder = new StringBuilder(32 * 16);

        public static void Load()
        {
        }

        public static void LoadWorld(TagCompound tag)
        {
            if (tag.TryGet("SynonymsEnabled", out uint toggles) && toggles != 0)
                SynonymToggles = toggles;
            else
            {
                toggles = 0;
                for (int i = 0; i < 32; i++)
                {
                    uint position = (uint)(1 << i);
                    if (Main.rand.NextBool())
                        toggles |= position;
                }
                SynonymToggles = toggles;
            }
            CreateFlipList();
        }

        public static void SaveWorld(TagCompound tag)
        {
            tag["SynonymsEnabled"] = SynonymToggles;
        }

        public static void ClearWorld()
        {
            SynonymToggles = 0;
            ActiveSynonymReplacements.Clear();
            RegexMatchUppercase = "";
            RegexMatchLowercase = "";
            regexUppercaseBuilder.Clear();
            regexLowercaseBuilder.Clear();
        }

        #region Initializing regex
        private static void CreateFlipList()
        {
            ActiveSynonymReplacements.Clear();
            regexUppercaseBuilder.Clear();
            regexLowercaseBuilder.Clear();

            if ((SynonymToggles & (uint)SynonymType.BarIngot) != 0)
                RegisterFlip("Bar", "Ingot");
            if ((SynonymToggles & (uint)SynonymType.PickaxePick) != 0)
                RegisterFlip("Pickaxe", "Pick", false);
            if ((SynonymToggles & (uint)SynonymType.AxeAx) != 0)
            {
                if ((SynonymToggles & (uint)SynonymType.PickaxePick) == 0)
                    RegisterFlip("Pickaxe", "Pickax", false);
                RegisterFlip("Axe", "Ax", false);
            }
            if ((SynonymToggles & (uint)SynonymType.SilkFabric) != 0)
                RegisterFlip("Silk", "Fabric");
            if ((SynonymToggles & (uint)SynonymType.GreavesLeggings) != 0)
            {
                RegisterFlip("Greaves", "Leggings");
                RegisterFlip("Pants", "Trousers");
            }
            if ((SynonymToggles & (uint)SynonymType.WoodPlanks) != 0)
            {
                RegisterFlip("Wood", "Planks", canBeSuffix:true);
            }
            if ((SynonymToggles & (uint)SynonymType.PotionElixir) != 0)
            {
                RegisterFlip("Potion", "Elixir");
                RegisterFlip("Flask", "Brew");
            }
            if ((SynonymToggles & (uint)SynonymType.BottleJar) != 0)
            {
                RegisterFlip("Bottle", "Jar");
            }
            if ((SynonymToggles & (uint)SynonymType.PixieFairy) != 0)
            {
                RegisterFlip("Pixie", "Fairy");
            }
            if ((SynonymToggles & (uint)SynonymType.SpecterSpectre) != 0)
            {
                RegisterFlip("Specter", "Spectre");
            }
            if ((SynonymToggles & (uint)SynonymType.DyePigment) != 0)
            {
                RegisterFlip("Dye", "Pigment");
            }
            if ((SynonymToggles & (uint)SynonymType.MagmaLava) != 0)
            {
                RegisterFlip("Magma", "Lava");
            }
            if ((SynonymToggles & (uint)SynonymType.RogueRouge) != 0)
            {
                RegisterFlip("Rogue", "Rouge");
            }
            if ((SynonymToggles & (uint)SynonymType.SummonerInvoker) != 0)
            {
                RegisterFlip("Summoner", "Invoker");
                RegisterFlip("Summoning", "Invoking");
                RegisterFlip("Summon", "Invocation");
            }
            if ((SynonymToggles & (uint)SynonymType.RangerHunter) != 0)
            {
                RegisterFlip("Ranger", "Hunter");
                RegisterFlip("Ranged", "Hunt");
            }

            if ((SynonymToggles & (uint)SynonymType.MageEnchanter) != 0)
            {
                RegisterFlip("Mage", "Enchanter");
                RegisterFlip("Magic", "Enchantment");
            }

            if ((SynonymToggles & (uint)SynonymType.MeleeBrawl) != 0)
            {
                RegisterFlip("Warrior", "Brawler");
                RegisterFlip("Melee", "Brawl");
                RegisterFlip("True Melee", "Fisticuffs");
            }

            if ((SynonymToggles & (uint)SynonymType.MinionServant) != 0)
            {
                RegisterFlip("Minion", "Servant");
                RegisterFlip("Minions", "Servants");
            }

            if ((SynonymToggles & (uint)SynonymType.SoulSpirit) != 0)
            {
                RegisterFlip("Soul", "Spirit");
            }

            if ((SynonymToggles & (uint)SynonymType.StaffScepter) != 0)
            {
                RegisterFlip("Staff", "Scepter");
            }

            if ((SynonymToggles & (uint)SynonymType.StaffScepter) != 0)
            {
                RegisterFlip("Mechanical", "Robotic");
                RegisterFlip("Mech", "Robot");
                RegisterFlip("Mechs", "Robots");
            }
            if ((SynonymToggles & (uint)SynonymType.ChairSeat) != 0)
            {
                RegisterFlip("Chair", "Seat");
            }
            if ((SynonymToggles & (uint)SynonymType.GemJewel) != 0)
            {
                RegisterFlip("Gem", "Jewel");
            }
            if ((SynonymToggles & (uint)SynonymType.BandBrace) != 0)
            {
                RegisterFlip("Band", "Brace");
            }
            if ((SynonymToggles & (uint)SynonymType.CrystalPrism) != 0)
            {
                RegisterFlip("Crystal", "Prism");
            }
            if ((SynonymToggles & (uint)SynonymType.StoneRock) != 0)
            {
                RegisterFlip("Stone", "Rock", canBeSuffix: true);
            }
            if ((SynonymToggles & (uint)SynonymType.KnockbackPushback) != 0)
            {
                RegisterFlip("Knockback", "Pushback", false);
            }


            if (regexUppercaseBuilder.Length > 0)
                regexUppercaseBuilder.Remove(regexUppercaseBuilder.Length - 1, 1);
            if (regexLowercaseBuilder.Length > 0)
                regexLowercaseBuilder.Remove(regexLowercaseBuilder.Length - 1, 1);

            RegexMatchUppercase = regexUppercaseBuilder.ToString();
            RegexMatchLowercase = regexLowercaseBuilder.ToString();
        }

        private static void RegisterFlip(string word1, string word2, bool bothWays = true, bool canBeSuffix = false)
        {
            string lowerWord1 = word1.ToLower();
            string lowerWord2 = word2.ToLower();

            ActiveSynonymReplacements.Add(word1, word2);
            ActiveSynonymReplacements.Add(lowerWord1, lowerWord2);

            regexUppercaseBuilder.Append($@"\b{word1}\b|");
            regexLowercaseBuilder.Append($@"\b{lowerWord1}\b|");

            if (canBeSuffix)
            {
                regexUppercaseBuilder.Append($@"{lowerWord1}\b|");
                regexLowercaseBuilder.Append($@"{lowerWord1}\b|");
            }
            else
            {
                regexUppercaseBuilder.Append($@"\b{word1}\b|");
                regexLowercaseBuilder.Append($@"\b{lowerWord1}\b|");
            }

            if (bothWays)
            {
                ActiveSynonymReplacements.Add(word2, word1);
                ActiveSynonymReplacements.Add(lowerWord2, lowerWord1);

                regexUppercaseBuilder.Append($@"\b{word2}\b|");
                regexLowercaseBuilder.Append($@"\b{lowerWord2}\b|");
            }
        }

        private static string SynonymMatchEvaluator(Match match)
        {
            if (ActiveSynonymReplacements.ContainsKey(match.Value))
                return ActiveSynonymReplacements[match.Value];
            return match.Value;
        }
        #endregion

        public static void ProcessSynonyms(Item item)
        {
            //No synonym fuckery in other languages
            if (Language.ActiveCulture != GameCulture.DefaultCulture || SynonymToggles == 0)
                return;
            ApplySynonyms(item);
        }

        public static void ProcessSynonyms(Item item, List<TooltipLine> tooltips)
        {
            //No synonym fuckery in other languages
            if (Language.ActiveCulture != GameCulture.DefaultCulture || SynonymToggles == 0)
                return;

            foreach (TooltipLine line in tooltips)
            {
                ApplySynonyms(line);
            }
        }

        private static void ApplySynonyms(Object swapObject)
        {
            if (swapObject is Item item)
                 item.SetNameOverride(Regex.Replace(item.Name, RegexMatchUppercase, SynonymMatchEvaluator));

            else if (swapObject is TooltipLine line)
            {
                line.Text = Regex.Replace(line.Text, RegexMatchUppercase, SynonymMatchEvaluator);
                line.Text = Regex.Replace(line.Text, RegexMatchLowercase, SynonymMatchEvaluator);
            }
        }
    }
}
