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
        public enum SynonymType : ulong
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
            BlockCube = 1 << 22,
            BandBrace = 1 << 23,
            CrystalPrism = 1 << 24,
            StoneRock = 1 << 25,
            KnockbackPushback = 1 << 26,
            CriticalSpecial = 1 << 27,
            ManaKi = 1 << 28,
            KnockbackPrefixes = 1 << 29,
            DemonDevil = 1 << 30,
            DamageHarm = (ulong)1 << 31,
            ZombieUndead = (ulong)1 << 32,
            WhipLash = (ulong)1 << 33,
            TeamGuild = (ulong)1 << 34,
            SentryTurret = (ulong)1 << 35,
            GodsElder = (ulong)1 << 36,
            BladeSword = (ulong)1 << 37,
            MineDig = (ulong)1 << 38,
            EssenceAnima = (ulong)1 << 39,
            DefenseToughness = (ulong)1 << 40,
            CthulhuAzathoth = (ulong)1 << 41,
            RegenerationRecovery = (ulong)1 << 42,
            GreatStrong = (ulong)1 << 43,
            LesserMinor = (ulong)1 << 44,
            DragonDaemon = (ulong)1 << 45,
            DraedonMechanomaniac = (ulong)1 << 46,
            SpeedPrefixes = (ulong)1 << 47,
            ShimmerGlimmer = (ulong)1 << 48,
            DurationTimeLength = (ulong)1 << 49,
            SerpentWorm = (ulong)1 << 50,
            MaterialComponent = (ulong)1 << 51,
            PlacePutDown = (ulong)1 << 52,
            ConsumableUtilizable = (ulong)1 << 53,
            RestoreRegain = (ulong)1 << 54,
            AmmoAmmunition = (ulong)1 << 55,
            FireFlame = (ulong)1 << 56,
            MoonMars = (ulong)1 << 57,
            PowerStrenght = (ulong)1 << 58,
            LastUltimate = (ulong)1 << 59,
            FlightSoar = (ulong)1 << 60,
            BloodPrana = (ulong)1 << 61,
            PronounsNeutral = (ulong)1 << 62,
            ChanceProbability = (ulong)1 << 63,
            AllSwaps = ~((ulong)0),
        }

        public static ulong SynonymToggles;
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
            if (tag.TryGet("SynonymsEnabled", out ulong toggles) && toggles != 0)
                SynonymToggles = toggles;
            else
            {
                toggles = 0;
                for (int i = 0; i < 64; i++)
                {
                    ulong position = (ulong)(1 << i);
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

            if ((SynonymToggles & (ulong)SynonymType.BarIngot) != 0)
                RegisterFlip("Bar", "Ingot");
            if ((SynonymToggles & (ulong)SynonymType.PickaxePick) != 0)
                RegisterFlip("Pickaxe", "Pick", false);
            if ((SynonymToggles & (ulong)SynonymType.AxeAx) != 0)
            {
                if ((SynonymToggles & (ulong)SynonymType.PickaxePick) == 0)
                    RegisterFlip("Pickaxe", "Pickax", false);
                RegisterFlip("Axe", "Ax", false);
                RegisterFlip("Hamaxe", "Hamax", false);
            }
            if ((SynonymToggles & (ulong)SynonymType.SilkFabric) != 0)
                RegisterFlip("Silk", "Fabric");
            if ((SynonymToggles & (ulong)SynonymType.GreavesLeggings) != 0)
            {
                RegisterFlip("Greaves", "Leggings");
                RegisterFlip("Pants", "Trousers");
            }
            if ((SynonymToggles & (ulong)SynonymType.WoodPlanks) != 0)
            {
                RegisterFlip("Wood", "Planks", canBeSuffix:true);
            }
            if ((SynonymToggles & (ulong)SynonymType.PotionElixir) != 0)
            {
                RegisterFlip("Potion", "Elixir");
                RegisterFlip("Potions", "Elixirs");
                RegisterFlip("Flask", "Brew");
                RegisterFlip("Flasks", "Brews");
            }
            if ((SynonymToggles & (ulong)SynonymType.BottleJar) != 0)
            {
                RegisterFlip("Bottle", "Jar");
            }
            if ((SynonymToggles & (ulong)SynonymType.PixieFairy) != 0)
            {
                RegisterFlip("Pixie", "Fairy");
            }
            if ((SynonymToggles & (ulong)SynonymType.SpecterSpectre) != 0)
            {
                RegisterFlip("Specter", "Spectre");
            }
            if ((SynonymToggles & (ulong)SynonymType.DyePigment) != 0)
            {
                RegisterFlip("Dye", "Pigment");
            }
            if ((SynonymToggles & (ulong)SynonymType.MagmaLava) != 0)
            {
                RegisterFlip("Magma", "Lava");
            }
            if ((SynonymToggles & (ulong)SynonymType.RogueRouge) != 0)
            {
                RegisterFlip("Rogue", "Rouge");
                RegisterFlip("Shadow", "Umbra", true, true);
            }
            if ((SynonymToggles & (ulong)SynonymType.SummonerInvoker) != 0)
            {
                RegisterFlip("Summoner", "Invoker");
                RegisterFlip("Summoning", "Invoking");
                RegisterFlip("Summon", "Invocation");
                RegisterFlip("Summons", "Invokes");
            }
            if ((SynonymToggles & (ulong)SynonymType.RangerHunter) != 0)
            {
                RegisterFlip("Ranger", "Hunter");
                RegisterFlip("Ranged", "Hunt");
            }

            if ((SynonymToggles & (ulong)SynonymType.MageEnchanter) != 0)
            {
                RegisterFlip("Mage", "Enchanter");
                RegisterFlip("Magic", "Enchantment");
            }

            if ((SynonymToggles & (ulong)SynonymType.MeleeBrawl) != 0)
            {
                RegisterFlip("Warrior", "Brawler");
                RegisterFlip("Melee", "Brawl");
                RegisterFlip("True Melee", "Fisticuffs");
            }

            if ((SynonymToggles & (ulong)SynonymType.MinionServant) != 0)
            {
                RegisterFlip("Minion", "Ally");
                RegisterFlip("Minions", "Allies");
                RegisterFlip("Your summons", "Your allies");
            }

            if ((SynonymToggles & (ulong)SynonymType.SoulSpirit) != 0)
            {
                RegisterFlip("Soul", "Spirit");
                RegisterFlip("Souls", "Spirits");
            }

            if ((SynonymToggles & (ulong)SynonymType.StaffScepter) != 0)
            {
                RegisterFlip("Staff", "Scepter");
                RegisterFlip("Kill", "Murder");
            }

            if ((SynonymToggles & (ulong)SynonymType.StaffScepter) != 0)
            {
                RegisterFlip("Mechanical", "Robotic");
                RegisterFlip("Mech", "Robot");
                RegisterFlip("Mechs", "Robots");
            }
            if ((SynonymToggles & (ulong)SynonymType.ChairSeat) != 0)
            {
                RegisterFlip("Auric", "Golden", false);
                RegisterFlip("Chair", "Seat");
            }
            if ((SynonymToggles & (ulong)SynonymType.BlockCube) != 0)
            {
                RegisterFlip("Block", "Cube");
                RegisterFlip("Blocks", "Cubes");
            }
            if ((SynonymToggles & (ulong)SynonymType.BandBrace) != 0)
            {
                RegisterFlip("Band", "Brace");
                RegisterFlip("Tile", "Square", false);
                RegisterFlip("Tiles", "Squares", false);
            }
            if ((SynonymToggles & (ulong)SynonymType.CrystalPrism) != 0)
            {
                RegisterFlip("Crystal", "Prism");
                RegisterFlip("Crystals", "Prisms");
            }
            if ((SynonymToggles & (ulong)SynonymType.StoneRock) != 0)
            {
                RegisterFlip("Stone", "Rock", canBeSuffix: true);
                RegisterFlip("Stones", "Rocks");
            }
            if ((SynonymToggles & (ulong)SynonymType.KnockbackPushback) != 0)
            {
                RegisterFlip("Knockback", "Pushback", false);
            }
            if ((SynonymToggles & (ulong)SynonymType.CriticalSpecial) != 0)
            {
                RegisterFlip("Critical", "Special", false);
            }
            if ((SynonymToggles & (ulong)SynonymType.ManaKi) != 0)
            {
                RegisterFlip("Mana", "Ki", false);
                RegisterFlip("Star", "Sun", true, true);
                RegisterFlip("Stars", "Suns");
            }
            if ((SynonymToggles & (ulong)SynonymType.KnockbackPrefixes) != 0)
            {
                RegisterFlip("Extremely weak", "Laughable", false);
                RegisterFlip("Very weak", "Pitiful", false);
                RegisterFlip("Weak", "Wimpy", false);
                RegisterFlip("Average", "Mediocre", false);
                RegisterFlip("Strong", "Powerful", false);
                RegisterFlip("Very strong", "Mighty", false);
                RegisterFlip("Extremely strong", "Bonkers", false);
                RegisterFlip("Insane", "Lunatic", false);
            }
            if ((SynonymToggles & (ulong)SynonymType.SpeedPrefixes) != 0)
            {
                RegisterFlip("Extremely slow", "Glacial", false);
                RegisterFlip("Very slow", "Sluggish", false);
                RegisterFlip("Slow", "Lethargic", false);
                RegisterFlip("Fast speed", "Rapid speed", false);
                RegisterFlip("Very fast speed", "Whirlwind speed", false);
                RegisterFlip("Insanely fast speed", "Supersonic speed", false);
            }
            if ((SynonymToggles & (ulong)SynonymType.DemonDevil) != 0)
            {
                RegisterFlip("Demon", "Devil");
                RegisterFlip("Demons", "Devils");
            }
            if ((SynonymToggles & (ulong)SynonymType.DamageHarm) != 0)
            {
                RegisterFlip("Damage", "Harm");
                RegisterFlip("Damages", "Harms");
                RegisterFlip("Damage Over Time", "Drawn Out Anguish");
            }
            if ((SynonymToggles & (ulong)SynonymType.WhipLash) != 0)
            {
                RegisterFlip("Whip", "Lash");
                RegisterFlip("Whips", "Lashes");
            }
            if ((SynonymToggles & (ulong)SynonymType.ZombieUndead) != 0)
            {
                RegisterFlip("Zombie", "Undead");
            }
            if ((SynonymToggles & (ulong)SynonymType.TeamGuild) != 0)
            {
                RegisterFlip("Team", "Guild");
                RegisterFlip("Fires", "Shoots");
            }
            if ((SynonymToggles & (ulong)SynonymType.SentryTurret) != 0)
            {
                RegisterFlip("Sentry", "Turret");
                RegisterFlip("Sentries", "Turrets");
            }
            if ((SynonymToggles & (ulong)SynonymType.GodsElder) != 0)
            {
                RegisterFlip("Deity", "Masterul Entity", false);
                RegisterFlip("Deities", "Masterful Beings", false);
                RegisterFlip("God", "Master");
                RegisterFlip("Goddess", "Mistress");
                RegisterFlip("Gods", "Beings of Mastery");
                RegisterFlip("Godly", "Mighty Powerful");
                RegisterFlip("Divine", "Mastery");
            }
            if ((SynonymToggles & (ulong)SynonymType.BladeSword) != 0)
            {
                RegisterFlip("Dagger", "Shortsword");
                RegisterFlip("Broadsword", "Sword", bothWays : false);
                RegisterFlip("Scimitar", "Katana");
                RegisterFlip("Blade", "Sword", canBeSuffix: true);
            }
            if ((SynonymToggles & (ulong)SynonymType.MineDig) != 0)
            {
                RegisterFlip("Mine", "Dig");
                RegisterFlip("Mining", "Digging");
            }
            if ((SynonymToggles & (ulong)SynonymType.EssenceAnima) != 0)
            {
                RegisterFlip("Essence", "Anima");
                RegisterFlip("Casts", "Sends forth", false);
            }
            if ((SynonymToggles & (ulong)SynonymType.DefenseToughness) != 0)
            {
                RegisterFlip("Defence", "Toughness");
                RegisterFlip("Defense", "Toughness", false);
            }
            if ((SynonymToggles & (ulong)SynonymType.CthulhuAzathoth) != 0)
            {
                RegisterFlip("Cthulhu", "Azathoth");
            }
            if ((SynonymToggles & (ulong)SynonymType.RegenerationRecovery) != 0)
            {
                RegisterFlip("Regeneration", "Recovery");
                RegisterFlip("Regen", "Recovery", false);
            }
            if ((SynonymToggles & (ulong)SynonymType.RestoreRegain) != 0)
            {
                RegisterFlip("Restoration", "Renewal");
                RegisterFlip("Restores", "Renews");
            }
            if ((SynonymToggles & (ulong)SynonymType.LesserMinor) != 0)
            {
                RegisterFlip("Lesser", "Minor");
            }
            if ((SynonymToggles & (ulong)SynonymType.GreatStrong) != 0)
            {
                RegisterFlip("Great", "Strong", false);
                RegisterFlip("Greater", "Stronger", false);
            }
            if ((SynonymToggles & (ulong)SynonymType.DragonDaemon) != 0)
            {
                RegisterFlip("Dragonkind", "Daemonkind");
                RegisterFlip("Dragon", "Daemon");
                RegisterFlip("Dragons", "Daemons");
                RegisterFlip("Draconic", "Daemonic");
            }
            if ((SynonymToggles & (ulong)SynonymType.DraedonMechanomaniac) != 0)
            {
                RegisterFlip("Draedon", "Thoroughbred");
            }
            if ((SynonymToggles & (ulong)SynonymType.ShimmerGlimmer) != 0)
            {
                RegisterFlip("Shimmer", "Glimmer");
                RegisterFlip("Shimmers", "Glimmers");
                RegisterFlip("Shimmering", "Glimmering");
            }
            if ((SynonymToggles & (ulong)SynonymType.DurationTimeLength) != 0)
            {
                RegisterFlip("Duration", "Time Length");
            }
            if ((SynonymToggles & (ulong)SynonymType.SerpentWorm) != 0)
            {
                RegisterFlip("Serpent", "Worm", canBeSuffix: true);
                RegisterFlip("Serpents", "Worms", canBeSuffix: true);
            }
            if ((SynonymToggles & (ulong)SynonymType.MaterialComponent) != 0)
            {
                RegisterFlip("Material", "Component");
            }
            if ((SynonymToggles & (ulong)SynonymType.PlacePutDown) != 0)
            {
                RegisterFlip("Places", "Puts Down");
                RegisterFlip("Place", "Put Down");
                RegisterFlip("Placed", "Put Down", false);
                RegisterFlip("Placement", "Deployment");
            }
            if ((SynonymToggles & (ulong)SynonymType.ConsumableUtilizable) != 0)
            {
                RegisterFlip("Consumable", "Utilizable");
                RegisterFlip("Consumes", "Utilizes");
                RegisterFlip("Consume", "Utilize");
            }
            if ((SynonymToggles & (ulong)SynonymType.AmmoAmmunition) != 0)
            {
                RegisterFlip("Ammo", "Ammunition");
                RegisterFlip("Debuff", "Malus", false);
            }
            if ((SynonymToggles & (ulong)SynonymType.FireFlame) != 0)
            {
                RegisterFlip("Fire", "Flame");
                RegisterFlip("Flaming", "Fiery");
                RegisterFlip("Equippable", "Able to be Equipped");
            }
            if ((SynonymToggles & (ulong)SynonymType.MoonMars) != 0)
            {
                RegisterFlip("Moons", "Planets");
                RegisterFlip("Moon", "Mars");
                RegisterFlip("the Moon", "Mars", false);
                RegisterFlip("Lunar", "Martian");
            }
            if ((SynonymToggles & (ulong)SynonymType.PowerStrenght) != 0)
            {
                RegisterFlip("Power", "Strenth", true, true);
                RegisterFlip("Increase", "Augment");
                RegisterFlip("Increases", "Augments");

            }
            if ((SynonymToggles & (ulong)SynonymType.LastUltimate) != 0)
            {
                RegisterFlip("Last", "Ultimate");
                RegisterFlip("Buff", "Bonus", false);
            }
            if ((SynonymToggles & (ulong)SynonymType.FlightSoar) != 0)
            {
                RegisterFlip("Flight", "Soaring");
                RegisterFlip("Flying", "Soaring", false);
            }
            if ((SynonymToggles & (ulong)SynonymType.BloodPrana) != 0)
            {
                RegisterFlip("Blood", "Prana");
                RegisterFlip("Bloody", "Fucking");
            }
            if ((SynonymToggles & (ulong)SynonymType.PronounsNeutral) != 0)
            {
                RegisterFlip("He", "They", false);
                RegisterFlip("Him", "Them", false);
                RegisterFlip("His", "Their", false);
                RegisterFlip("Himself", "Themself", false);

                RegisterFlip("She", "They", false);
                RegisterFlip("Her", "Their", false);
                RegisterFlip("Herself", "Themself", false);
            }
            if ((SynonymToggles & (ulong)SynonymType.ChanceProbability) != 0)
            {
                RegisterFlip("Rune", "Sigil");
                RegisterFlip("Chance", "Probability");
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
                regexUppercaseBuilder.Append($@"{word1}\b|");
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

                if (canBeSuffix)
                {
                    regexUppercaseBuilder.Append($@"{word2}\b|");
                    regexUppercaseBuilder.Append($@"{lowerWord2}\b|");
                    regexLowercaseBuilder.Append($@"{lowerWord2}\b|");
                }
                else
                {
                    regexUppercaseBuilder.Append($@"\b{word2}\b|");
                    regexLowercaseBuilder.Append($@"\b{lowerWord2}\b|");
                }
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

        public static void ApplySynonym(ref string text)
        {
            if (Language.ActiveCulture != GameCulture.DefaultCulture || SynonymToggles == 0)
                return;
            text = Regex.Replace(text, RegexMatchUppercase, SynonymMatchEvaluator);
        }
    }
}
