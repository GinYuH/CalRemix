using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using CalamityMod;
using CalamityMod.BiomeManagers;
using Terraria.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.GameContent;
using System;
using CalRemix.UI;
using System.Linq;
using CalRemix.Core.World;
using Terraria.Chat;
using Terraria.Localization;

namespace CalRemix.Content.NPCs
{
    public class Fushigi : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fushigi");
            if (Main.dedServ)
                return;
            HelperMessage.New("Fushigi",
                "Look at you, finding a floating glass ball stuck in some rocks! If you break the rocks above it, that ball will rise like it's got somewhere to be. But beware, if it escapes the ocean, you'll be in for some seriously wild wind! Hold onto your hat, or it might fly away faster than my jokes at a comedy show!",
                "FannyIdle",
                (ScreenHelperSceneMetrics scene) => scene.onscreenNPCs.Any(n => n.type == Type));
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.damage = 0;
            NPC.width = 28;
            NPC.height = 28;
            NPC.defense = 600;
            NPC.lifeMax = 22;
            NPC.knockBackResist = 0f;
            NPC.value = 0;
            NPC.dontTakeDamage = true;
            NPC.noGravity = true;
            NPC.noTileCollide = false;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.rarity = 4;
            SpawnModBiomes = new int[3] { ModContent.GetInstance<AbyssLayer1Biome>().Type, ModContent.GetInstance<AbyssLayer2Biome>().Type, ModContent.GetInstance<AbyssLayer3Biome>().Type };
        }

        public override void AI()
        {
            if (NPC.ai[0] == 0 && (Collision.IsWorldPointSolid(NPC.Top - Vector2.UnitY) || Collision.IsWorldPointSolid(NPC.TopRight - Vector2.UnitY)))
            {
                SoundEngine.PlaySound(CalamityMod.NPCs.SunkenSea.GiantClam.SlamSound with { Pitch = 0.4f }, NPC.Center);
                Shard();
                NPC.ai[0] = 1;
            }
            if (NPC.ai[0] == 1 && !(Collision.IsWorldPointSolid(NPC.Top - Vector2.UnitY) || Collision.IsWorldPointSolid(NPC.TopRight - Vector2.UnitY)))
            {
                NPC.ai[0] = 0;
            }
            NPC.velocity.Y -= 0.2f;
            if (NPC.velocity.Y < -12f)
            {
                NPC.velocity.Y = -12f;
            }
            if (NPC.position.Y < 656)
            {
                CalamityUtils.DisplayLocalizedText("Mods.CalRemix.StatusText.GaleforceBegin", Color.LightBlue);
                int oxTime = Main.rand.Next(CalamityUtils.SecondsToFrames(60 * 12), CalamityUtils.SecondsToFrames(60 * 16));
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    CalRemixWorld.oxydayTime = oxTime;
                }
                else
                {
                    ModPacket packet = CalRemix.instance.GetPacket();
                    packet.Write((byte)RemixMessageType.OxydayTime);
                    packet.Write(oxTime);
                    packet.Send();
                }
                CalRemixWorld.UpdateWorldBool();
                NPC.active = false;
            }
            foreach (Player player in Main.ActivePlayers)
            {
                if (NPC.Hitbox.Intersects(player.HitboxForBestiaryNearbyCheck))
                {
                    Main.BestiaryTracker.Kills.RegisterKill(NPC);
                    break;
                }
            }
            foreach (Player player in Main.player)
            {
                if (NPC.getRect().Intersects(player.getRect()))
                {
                    float pushX = 0.3f;
                    float pushY = 0.1f;
                    NPC.velocity.X += player.Center.X > NPC.Center.X ? -pushX : pushX;
                    NPC.velocity.Y += player.Center.Y > NPC.Center.Y ? -pushY : pushY;
                    NPC.ai[1] = player.whoAmI;
                }
            }
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                new FlavorTextBestiaryInfoElement("A strange aerodynamic object sometimes found within the dark depths of the abyss. It is said that if one were to help one of these orbs escape the abyss, a terrible storm would occur.")
            });
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if ((spawnInfo.Player.Calamity().ZoneAbyssLayer3 || spawnInfo.Player.Calamity().ZoneAbyssLayer2 || spawnInfo.Player.Calamity().ZoneAbyssLayer1) && spawnInfo.Water && !NPC.AnyNPCs(ModContent.NPCType<Fushigi>()) && CalRemixWorld.oxydayTime <= 0)
                return Main.remixWorld ? 0.27f : SpawnCondition.CaveJellyfish.Chance * 0.03f;
            return 0f;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (NPC.IsABestiaryIconDummy)
                return true;
            Texture2D texture = TextureAssets.Npc[NPC.type].Value;
            Vector2 position = NPC.Center - Main.screenPosition;
            Vector2 origin = texture.Size() * 0.5f;
            Color color = Color.SkyBlue * NPC.Opacity;
            for (int i = 0; i < 10; i++)
            {
                Vector2 vector2 = (MathF.PI * 2f * (float)i / 10f).ToRotationVector2() + (MathF.PI * 2f * (float)i / 10f).ToRotationVector2() * 2 * Math.Abs((float)Math.Sin(Main.GlobalTimeWrappedHourly));
                Main.spriteBatch.Draw(texture, position + vector2, null, color, NPC.rotation, origin, NPC.scale, SpriteEffects.None, 0f);
            }
            Main.spriteBatch.Draw(texture, position, null, Color.White, NPC.rotation, origin, NPC.scale, SpriteEffects.None, 0f);
            return false;
        }

        public void Shard(int amt = 10)
        {
            for (int i = 0; i < amt; i++)
            {
                Gore.NewGore(NPC.GetSource_FromThis(), NPC.Center, Main.rand.NextVector2Circular(NPC.width, NPC.height).SafeNormalize(Vector2.UnitY) * Main.rand.Next(3, 6), Mod.Find<ModGore>("OxygenShrap" + Main.rand.Next(1, 7)).Type, Main.rand.NextFloat(0.2f, 0.5f));
            }
        }

        public override bool CheckActive()
        {
            if (NPC.ai[1] == -1)
                return true;
            Player p = Main.player[(int)NPC.ai[1]];
            if (p == null || !p.active)
                return true;
            if (Math.Abs(p.position.X - NPC.position.X) > 2000)
                return true;
            return false;
        }
    }
}
