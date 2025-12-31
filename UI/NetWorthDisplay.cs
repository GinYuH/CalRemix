using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework.Graphics;
using System;
using CalamityMod.Items.Accessories;
using static Terraria.ModLoader.ModContent;
using CalamityMod.Items.Mounts;
using CalRemix.Core.Retheme;
using Terraria.DataStructures;
using MonoMod.Cil;
using ReLogic.Graphics;
using System.Reflection;
using Terraria.UI.Chat;

namespace CalRemix.UI
{
    public class NetWorthDisplay : InfoDisplay
	{
        public override void Load()
        {
            IL_Main.DrawInfoAccs += NetworthDisplayProperly;
        }

        private void NetworthDisplayProperly(MonoMod.Cil.ILContext il)
        {
            var cursor = new ILCursor(il);


            Type[] paramTypes = [typeof(SpriteBatch), typeof(DynamicSpriteFont), typeof(string), typeof(Vector2), typeof(Color), typeof(float), typeof(Vector2), typeof(Vector2), typeof(SpriteEffects), typeof(float)];
            MethodInfo drawStringMethod = typeof(DynamicSpriteFontExtensionMethods).GetMethod("DrawString", BindingFlags.Public | BindingFlags.Static, paramTypes);

            for (int b = 0; b < 2; b++)
            {
                if (!cursor.TryGotoNext(MoveType.Before,
                    i => i.MatchCall(drawStringMethod)))
                {
                    return;
                }

                cursor.Remove();
                cursor.EmitDelegate(SpoofDrawString);
            }
        }

        public static void SpoofDrawString(SpriteBatch spriteBatch, DynamicSpriteFont font, string text, Vector2 position, Color drawColor, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth)
        {
            if (text == "NETWORTH")
            {
                if (drawColor.R < 20)
                    return;
                NetWorthPlayer player = Main.LocalPlayer.GetModPlayer<NetWorthPlayer>();

                int platinumCount = 0;
                int goldCount = 0;
                int silverCount = 0;
                int copperCount = 0;

                int monerz = player.netWorth;

                if (monerz < 0)
                {
                    Color color = Main.DiscoColor;
                    ChatManager.DrawColorCodedStringWithShadow(spriteBatch, font, "Net Worth: Billion trillion gazillion ultraillion", position, color, rotation, origin, scale);
                    return;
                }

                if (monerz < 1)
                    monerz = 1;

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

                List<string> netWorthText = new();

                if (monerz >= 1)
                    copperCount = monerz;

                if (platinumCount > 0)
                    netWorthText.Add($"[i:{ItemID.PlatinumCoin}][c/" + Colors.AlphaDarken(Colors.CoinPlatinum).Hex3() + ":" + platinumCount + "]");

                if (goldCount > 0 || platinumCount > 0)
                    netWorthText.Add($"[i:{ItemID.GoldCoin}][c/" + Colors.AlphaDarken(Colors.CoinGold).Hex3() + ":" + goldCount + "]");

                if (silverCount > 0 || goldCount > 0 || platinumCount > 0)
                    netWorthText.Add($"[i:{ItemID.SilverCoin}][c/" + Colors.AlphaDarken(Colors.CoinSilver).Hex3() + ":" + silverCount +  "]");

                netWorthText.Add($"[i:{ItemID.CopperCoin}][c/" + Colors.AlphaDarken(Colors.CoinCopper).Hex3() + ":" + copperCount + "]");

                ChatManager.DrawColorCodedStringWithShadow(spriteBatch, font, "Net Worth:", position, drawColor, rotation, origin, scale);
                position.X += 86;

                for (int i = 0; i < netWorthText.Count; i++)
                {
                    ChatManager.DrawColorCodedStringWithShadow(spriteBatch, font, netWorthText[i], position, drawColor, rotation, origin, scale);

                    if (i == 0 && platinumCount > 99)
                        position.X += 10;

                    position.X += 40;
                }
            }
            else
                spriteBatch.DrawString(font, text, position, drawColor, rotation, origin, scale, effects, layerDepth);
        }

        public override bool Active() 
		{
            HelperMessage msg = HelperMessage.ByID("SneakersIntroduction");

            if (msg == null)
                return false;

            return msg.alreadySeen;
		}

		public override string DisplayValue(ref Color displayColor, ref Color displayShadowColor)
		{
            displayShadowColor = Color.Black;
            var netWorthPlayer = Main.LocalPlayer.GetModPlayer<NetWorthPlayer>();

            if (!netWorthPlayer.displayInDollars)
                return "NETWORTH";

            if (netWorthPlayer.laggingNetWorth < 0)
            {
                displayColor = Main.DiscoColor;
                return "Net Worth : INFINITY $$$$$!!!";
            }

            return "Net Worth: " + netWorthPlayer.laggingNetWorth.ToString() + " $$$";
		}
	}

	public class NetWorthPlayer : ModPlayer
    {
        public int netWorth;
        public int netWorthCap;
        public int netWorthSpeed;
        public int laggingNetWorth;
        public int netWorthUpdateTimer;
        public int networthDisplaySwapTimer;
        public bool displayInDollars;
        public bool netWorthGod;
        public int netWorthGodHPBoost;

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
            netWorthGod = false;
        }

        public override void PostUpdateEquips()
        {
            var throneData = MountLoader.GetMount(MountType<DraedonGamerChairMount>()).MountData;
            throneData.textureWidth = throneData.frontTexture.Width() ;

            if (netWorth < netWorthCap)
            {
                netWorth += netWorthSpeed;
                if (netWorth > netWorthCap)
                    netWorth = netWorthCap;
            }

            if (netWorth >= 1000000)
                SneakersRetheme.platinumNetWorthMessage.ActivateMessage();

            if (netWorth >= 999000000)
                SneakersRetheme.ultraRichMessage.ActivateMessage();

            if (!netWorthGod)
                netWorthGodHPBoost = 0;
            else
            {
                if (Math.Abs(Player.velocity.X) > 6)
                {
                    netWorthGodHPBoost++;

                    int posX = (int)(Player.Center.X / 16);
                    int posY = (int)(Player.Center.Y / 16);

                    if (posX > 0 && posX < Main.maxTilesX && posY > 0 && posY < Main.maxTilesY)
                    {
                        Tile t = Main.tile[posX, posY];
                        if (!t.HasTile)
                        {
                            t.HasTile = true;
                            t.TileType = TileID.PlatinumCoinPile;
                            WorldGen.TileFrame(posX, posY);

                            if (Main.netMode == NetmodeID.MultiplayerClient)
                                NetMessage.SendTileSquare(-1, posX, posY);
                        }
                    }
                }
            }


            if (netWorthUpdateTimer <= 0)
            {
                laggingNetWorth = netWorth;
                netWorthUpdateTimer = 15;
            }
            else
                netWorthUpdateTimer--;

            if (networthDisplaySwapTimer <= 0)
            {
                displayInDollars = !displayInDollars;
                networthDisplaySwapTimer = 20 * 60;
            }
            else
                networthDisplaySwapTimer--;

            if (Player.mount.Active && Player.mount.Type == MountType<DraedonGamerChairMount>())
                SneakersRetheme.exoBoxWrongSlotMessage.ActivateMessage();
        }

        public override void PostUpdateRunSpeeds()
        {
            //Slippery
            if (netWorthGod)
            {
                Player.maxRunSpeed *= 5;

                if (Math.Abs(Player.velocity.X) < 5f)
                    Player.runAcceleration *= 3.7f;
                else
                    Player.runAcceleration *= 0.7f;
            }
        }

        public override void ModifyMaxStats(out StatModifier health, out StatModifier mana)
        {
            base.ModifyMaxStats(out health, out mana);
            health.Flat += netWorthGodHPBoost;
        }

        public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
        {
            int originalNetWorth = netWorth;
            netWorth /= 2;

            if (originalNetWorth >= 1000000 && SneakersRetheme.platinumNetWorthMessage.alreadySeen)
                SneakersRetheme.platinumNetWorthLossMessage.ActivateMessage();
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
             { ItemType<InterstellarStompers>(), Item.buyPrice(gold: 94, silver:27, copper:16) },
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
             { ItemType<Calamity>(), Item.buyPrice(platinum: 500) }
        };
    }
}
