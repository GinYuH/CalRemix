using CalRemix.Core;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ID;
using System;
using Terraria;
using Steamworks;
using CalamityMod.Items.Accessories;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using CalamityMod.Items.Mounts;
using System.Collections.Generic;
using Terraria.GameContent;
using CalamityMod.Buffs.Mounts;
using CalRemix.UI;
using CalRemix.Core.Retheme;

namespace CalRemix.UI
{
    public class NetWorthDisplay : InfoDisplay
	{
		public override bool Active() 
		{
            HelperMessage msg = HelperMessage.ByID("SneakersIntroduction");

            if (msg == null)
                return false;

            return msg.alreadySeen;
		}

		public override string DisplayValue(ref Color displayColor, ref Color displayShadowColor)
		{
            NetWorthPlayer player = Main.LocalPlayer.GetModPlayer<NetWorthPlayer>();

            string netWorthValue = "";
            int platinumCount = 0;
            int goldCount = 0;
            int silverCount = 0;
            int copperCount = 0;

            int monerz = player.netWorth;
            if (monerz < 1)
                monerz = 1;

            /*
            if (monerz >= 1000000)
            {
                platinumCount = monerz / 1000000;
                monerz -= platinumCount * 1000000;
            }

            if (monerz >= 10000)
            {
                goldCount = monerz / 10000;
                monerz -= goldCount * 10000;
            }

            if (monerz >= 100)
            {
                silverCount = monerz / 100;
                monerz -= silverCount * 100;
            }

            if (monerz >= 1)
                copperCount = monerz;

            if (platinumCount > 0)
                netWorthValue = netWorthValue + $"[i:{ItemID.PlatinumCoin}][c/" + Colors.AlphaDarken(Colors.CoinPlatinum).Hex3() + " " + platinumCount + "] ";

            if (goldCount > 0)
                netWorthValue = netWorthValue + $"[i:{ItemID.GoldCoin}][c/" + Colors.AlphaDarken(Colors.CoinGold).Hex3() + " " + goldCount + "] ";

            if (silverCount > 0)
                netWorthValue = netWorthValue + $"[i:{ItemID.SilverCoin}][c/" + Colors.AlphaDarken(Colors.CoinSilver).Hex3() + " " + silverCount +  "] ";

            if (copperCount > 0)
                netWorthValue = netWorthValue + $"[i:{ItemID.CopperCoin}][c/" + Colors.AlphaDarken(Colors.CoinCopper).Hex3() + " " + copperCount + "] ";
            */

            netWorthValue = monerz.ToString();
            netWorthValue += " $$$";


            return $"Net Worth: " + netWorthValue;
		}
	}

	public class NetWorthPlayer : ModPlayer
    {
        public int netWorth;
        public int netWorthCap;
        public int netWorthSpeed;

        public override void SaveData(TagCompound tag)
        {
            tag["networth"] = netWorth;
        }
        public override void LoadData(TagCompound tag)
        {
            netWorth = tag.GetInt("networth");
        }

        public override void ResetEffects()
        {
            netWorthCap = 0;
            netWorthSpeed = 0;
        }

        public override void PostUpdateEquips()
        {
            if (netWorth < netWorthCap)
            {
                netWorth += netWorthSpeed;
                if (netWorth > netWorthCap)
                    netWorth = netWorthCap;
            }

            if (netWorth >= 1000000)
            {
                SneakersRetheme.platinumNetWorthMessage.ActivateMessage();
            }
        }

        internal static Dictionary<int, int> netWorthCapPerSneaker = new()
            {
            //Vanilla
             { ItemID.RoyalGel, Item.buyPrice(silver: 50) },
             { ItemType<OceanCrest>(), Item.buyPrice(gold: 1, silver:20) },
             { ItemID.EoCShield, Item.buyPrice(gold: 2, silver:60, copper:36) },
             { ItemType<FungalClump>(), Item.buyPrice(gold: 4, silver:25, copper:78) },
             { ItemID.WormScarf, Item.buyPrice(gold: 5, silver:62, copper:91) },
             { ItemID.BrainOfConfusion, Item.buyPrice(gold: 5, silver:62, copper:91) },
             { ItemType<BloodyWormTooth>(), Item.buyPrice(gold: 10, silver:44, copper:65) },
             { ItemType<RottenBrain>(), Item.buyPrice(gold: 10, silver:44, copper:65) },
             { ItemID.HiveBackpack, Item.buyPrice(gold: 24, silver:27, copper:74) },
             { ItemID.BoneHelm, Item.buyPrice(gold: 36, silver:30, copper:2) },
             { ItemID.BoneGlove, Item.buyPrice(gold: 40, silver:50) },
             { ItemType<ManaPolarizer>(), Item.buyPrice(gold: 45, silver:60, copper:97) },
             { ItemID.DemonHeart, Item.buyPrice(gold: 80) },

             { ItemID.VolatileGelatin, Item.buyPrice(gold: 55, silver:35, copper:67) },
             { ItemType<AquaticEmblem>(), Item.buyPrice(gold: 67, silver:2, copper:44) },
             { ItemType<FlameLickedShell>(), Item.buyPrice(gold: 67, silver:2, copper:44) },
             { ItemID.MechanicalBatteryPiece, Item.buyPrice(gold: 33, silver:54, copper:73) },
             { ItemID.MechanicalWagonPiece, Item.buyPrice(gold: 33, silver:54, copper:73) },
             { ItemID.MechanicalWheelPiece, Item.buyPrice(gold: 33, silver:54, copper:73) },
             { ItemID.MinecartMech, Item.buyPrice(gold: 69, silver:42, copper:23) },
             { ItemID.MinecartPowerup, Item.buyPrice(gold: 69, silver:42, copper:23) },
             { ItemType<VoidofCalamity>(), Item.buyPrice(gold: 83, silver:21, copper:42) },
             { ItemID.SporeSac, Item.buyPrice(gold: 89, silver:46, copper:8) },
             { ItemType<GravistarSabaton>(), Item.buyPrice(gold: 94, silver:27, copper:16) },
             { ItemType<LeviathanAmbergris>(), Item.buyPrice(gold: 97, silver:13, copper:93) },
             { ItemID.ShinyStone, Item.buyPrice(platinum: 1) },

             { ItemType<ToxicHeart>(), Item.buyPrice(platinum: 1, gold: 52, silver: 35, copper: 78)  },
             { ItemID.ShrimpyTruffle, Item.buyPrice(platinum: 2, gold: 3, silver: 16, copper: 29)  },
             { ItemID.EmpressFlightBooster, Item.buyPrice(platinum: 8, gold: 32, silver: 11, copper: 74)  },
             { ItemType<BloodflareCore>(), Item.buyPrice(platinum: 11, gold: 55, silver: 75, copper: 99)  },
             { ItemType<HideofAstrumDeus>(), Item.buyPrice(platinum: 23, gold: 63, silver: 39, copper: 1)  },
             { ItemID.SuspiciousLookingTentacle, Item.buyPrice(platinum: 36, gold: 31, silver: 96, copper: 42)  },
             { ItemType<DynamoStemCells>(), Item.buyPrice(platinum: 30, gold: 13, silver: 65, copper: 53)  },
             { ItemType<SpectralVeil>(), Item.buyPrice(platinum: 42, gold: 86, silver: 66, copper: 23)  },
             { ItemType<TheEvolution>(), Item.buyPrice(platinum: 42, gold: 49, silver: 40, copper: 64)  },
             { ItemType<Affliction>(), Item.buyPrice(platinum: 67, gold: 79, silver: 4, copper: 50)  },
             { ItemType<OldDukeScales>(), Item.buyPrice(platinum: 92, gold: 4, silver: 37, copper: 31)  },
             { ItemType<BlazingCore>(), Item.buyPrice(platinum: 128, gold: 36, silver: 16, copper: 57)  },
             { ItemType<NebulousCore>(), Item.buyPrice(platinum: 232, gold: 69, silver: 3, copper: 77)  },
             { ItemType<YharimsGift>(), Item.buyPrice(platinum: 366, gold: 66, silver: 63, copper: 29) },
             { ItemType<ExoThrone>(), Item.buyPrice(platinum: 499) },
             { ItemType<Calamity>(), Item.buyPrice(platinum: 499) }};
    }
}
