using CalamityMod;
using CalRemix.NPCs;
using CalRemix.Projectiles.Hostile;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json.Linq;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Utilities;

namespace CalRemix
{
    public class ChampionNPC : GlobalNPC
    {
        public int championType = 0;

        public const int ChampionChance = 50;

        public int championTimer = 0;

        public bool globRevived = false;

        public bool kingMinion = false;

        public enum ChampionID
        {
            Red = 1, // Increased health
            Yellow = 2, // Increased speed
            Green = 3, // Spawns damaging creep
            Orange = 4, // Drops your money onhit
            DarkBlue = 5, // Decreased health and speed
            DarkCyan = 6, // Explodes on death
            White = 7, // Invincible until all of same type are dead
            Gray = 8, // Decreased health
            Transluscent = 9, // Falls through tiles
            Black = 10, // Fades in and out
            Magenta = 11, // Shoots projectiles every second
            Violet = 12, // Pulls in player projectiles
            DarkRed = 13, // Revives once upon death
            LightBlue = 14, // Explodes into 8 projectiles on death
            Camouflage = 15, // Takes after the current biome's color
            PulsingGreen = 16, // Splits into two on death
            PulsingGray = 17, // Repels player proejctiles
            LightWhite = 18, // Orbited by eyes
            Small = 19, // Smol
            Large = 20, // Chungus
            PulsingRed = 21, // Heals enemies every second
            Pulsating = 22, // Spawns eyes when damaged
            Crown = 23, // Big health
            Skull = 24, // Double damage, damages all enemies on death
            Brown = 25, // Leaves behind temporary blocks
            Rainbow = 26 // Mix of a bunch
        }

        public static WeightedRandom<int> ChampionWeights = new WeightedRandom<int>();

        public override bool InstancePerEntity => true;

        public override void Load()
        {
            ChampionWeights.Add((int)ChampionID.Red, 1);
            ChampionWeights.Add((int)ChampionID.Yellow, 1);
            ChampionWeights.Add((int)ChampionID.Green, 1);
            ChampionWeights.Add((int)ChampionID.Orange, 0.5f);
            ChampionWeights.Add((int)ChampionID.DarkBlue, 0.5f);
            ChampionWeights.Add((int)ChampionID.DarkCyan, 1);
            ChampionWeights.Add((int)ChampionID.White, 0.1f);
            ChampionWeights.Add((int)ChampionID.Gray, 1);
            ChampionWeights.Add((int)ChampionID.Transluscent, 0.1f);
            ChampionWeights.Add((int)ChampionID.Black, 0.1f);
            ChampionWeights.Add((int)ChampionID.Magenta, 1);
            ChampionWeights.Add((int)ChampionID.Violet, 0.1f);
            ChampionWeights.Add((int)ChampionID.DarkRed, 0.5f);
            ChampionWeights.Add((int)ChampionID.LightBlue, 1);
            ChampionWeights.Add((int)ChampionID.Camouflage, 0.25f);
            ChampionWeights.Add((int)ChampionID.PulsingGreen, 0.25f);
            ChampionWeights.Add((int)ChampionID.PulsingGray, 0.25f);
            ChampionWeights.Add((int)ChampionID.LightWhite, 0.25f);
            ChampionWeights.Add((int)ChampionID.Small, 0.25f);
            ChampionWeights.Add((int)ChampionID.Large, 0.25f);
            ChampionWeights.Add((int)ChampionID.PulsingRed, 0.1f);
            ChampionWeights.Add((int)ChampionID.Pulsating, 0.1f);
            ChampionWeights.Add((int)ChampionID.Crown, 0.1f);
            ChampionWeights.Add((int)ChampionID.Skull, 0.1f);
            ChampionWeights.Add((int)ChampionID.Brown, 0.1f);
            ChampionWeights.Add((int)ChampionID.Rainbow, 0.01f);
        }

        public override void OnSpawn(NPC npc, IEntitySource source)
        {
            // Only naturally spawned enemies can become champions
            if (source is EntitySource_SpawnNPC && !npc.boss && !npc.friendly && !npc.dontTakeDamage && Main.hardMode)
            {
                if (Main.rand.NextBool(ChampionChance))
                {
                    // Grab a champion
                    championType = ChampionWeights.Get();
                    //championType = (int)ChampionID.Pulsating;
                    // All champions except the size based ones default at slightly larger
                    if (championType > 0 && championType != (int)ChampionID.Small && championType != (int)ChampionID.Large)
                    {
                        npc.scale = 1.3f;
                    }

                    switch (championType)
                    {
                        case (int)ChampionID.Red:
                            npc.lifeMax = (int)(npc.lifeMax * 2.6f);
                            npc.life = (int)(npc.life * 2.6f);
                            break;
                        case (int)ChampionID.Yellow:
                            npc.lifeMax = (int)(npc.lifeMax * 1.5f);
                            npc.life = (int)(npc.life * 1.5f);
                            break;
                        case (int)ChampionID.Gray:
                            npc.lifeMax = (int)(npc.lifeMax * 0.66f);
                            npc.life = (int)(npc.life * 0.66f);
                            break;
                        case (int)ChampionID.Small:
                            npc.lifeMax = (int)(npc.lifeMax * 0.66f);
                            npc.life = (int)(npc.life * 0.66f);
                            npc.scale = 0.5f;
                            npc.width *= (int)(npc.width * 0.5f);
                            npc.height = (int)(npc.height * 0.5f);
                            break;
                        case (int)ChampionID.Large:
                            npc.lifeMax = (int)(npc.lifeMax * 3f);
                            npc.life = (int)(npc.life * 3f);
                            npc.scale = 1.5f;
                            break;
                        case (int)ChampionID.Crown:
                            npc.lifeMax = (int)(npc.lifeMax * 6f);
                            npc.life = (int)(npc.life * 6f);
                            break;
                        case (int)ChampionID.Rainbow:
                            npc.lifeMax = (int)(npc.lifeMax * 3f);
                            npc.life = (int)(npc.life * 3f);
                            // Spawn an orbital
                            if (npc.type != ModContent.NPCType<EternalChampEye>())
                            {
                                int n = NPC.NewNPC(npc.GetSource_FromThis(), (int)npc.position.X, (int)npc.position.Y, ModContent.NPCType<EternalChampEye>(), 0, npc.whoAmI, Main.rand.Next(0, 255), npc.type);
                                Main.npc[n].damage = npc.damage / 5;
                            }
                            break;
                        case (int)ChampionID.Transluscent:
                            // only fall through tiles if it ignores gravity for sanity reasons
                            if (npc.noGravity)
                                npc.noTileCollide = true;
                            npc.alpha = 100;
                            break;
                        case (int)ChampionID.LightWhite:
                            // Spawn 2-3 orbitals
                            if (npc.type != ModContent.NPCType<EternalChampEye>())
                            for (int i = 0; i < Main.rand.Next(2, 4); i++)
                            {
                                int n = NPC.NewNPC(npc.GetSource_FromThis(), (int)npc.position.X, (int)npc.position.Y, ModContent.NPCType<EternalChampEye>(), 0, npc.whoAmI, Main.rand.Next(0, 255), npc.type);
                                Main.npc[n].damage = npc.damage / 5;
                            }
                            break;
                    }
                }
            }
        }

        public static Color GetBiomeColor()
        {
            Color color2 = ((Main.waterStyle == 2) ? new Color(124, 118, 242) : ((Main.waterStyle == 3) ? new Color(143, 215, 29) : ((Main.waterStyle == 4) ? new Color(78, 193, 227) : ((Main.waterStyle == 5) ? new Color(189, 231, 255) : ((Main.waterStyle == 6) ? new Color(230, 219, 100) : ((Main.waterStyle == 7) ? new Color(151, 107, 75) : ((Main.waterStyle == 8) ? new Color(128, 128, 128) : ((Main.waterStyle == 9) ? new Color(200, 0, 0) : ((Main.waterStyle == 10) ? new Color(208, 80, 80) : ((Main.waterStyle == 12) ? new Color(230, 219, 100) : ((Main.waterStyle != 13) ? new Color(28, 216, 94) : new Color(28, 216, 94))))))))))));
            if (Main.waterStyle >= 15)
            {
                color2 = LoaderManager.Get<WaterStylesLoader>().Get(Main.waterStyle).BiomeHairColor();
            }
            return color2;
        }

        public override Color? GetAlpha(NPC npc, Color drawColor)
        {
            if (championType > 0)
            {
                Color ret = championType switch
                {
                    (int)ChampionID.Red => Color.Red,
                    (int)ChampionID.Yellow => Color.Yellow,
                    (int)ChampionID.Green => Color.Green,
                    (int)ChampionID.Orange => Color.Orange,
                    (int)ChampionID.DarkBlue => Color.DarkBlue,
                    (int)ChampionID.DarkCyan => Color.DarkCyan,
                    (int)ChampionID.White => Color.White,
                    (int)ChampionID.Gray => Color.Gray,
                    (int)ChampionID.Transluscent => drawColor * 0.2f,
                    (int)ChampionID.Black => Color.Black,
                    (int)ChampionID.Magenta => Color.Magenta,
                    (int)ChampionID.Violet => Color.Violet,
                    (int)ChampionID.DarkRed => Color.DarkRed,
                    (int)ChampionID.LightBlue => Color.LightBlue,
                    (int)ChampionID.Camouflage => GetBiomeColor(),
                    (int)ChampionID.PulsingGreen => Color.Lerp(drawColor, Color.Green, (Main.GlobalTimeWrappedHourly % 1f - 0.1f) / 0.2f),
                    (int)ChampionID.PulsingGray => championTimer % 120 < 59 ? Color.Gray : drawColor,
                    (int)ChampionID.LightWhite => Color.LightGray,
                    (int)ChampionID.Small => drawColor,
                    (int)ChampionID.Large => drawColor,
                    (int)ChampionID.PulsingRed => Color.Lerp(drawColor, Color.Red, (Main.GlobalTimeWrappedHourly % 1f - 0.1f) / 0.2f),
                    (int)ChampionID.Pulsating => drawColor,
                    (int)ChampionID.Crown => drawColor,
                    (int)ChampionID.Skull => Color.DarkGray,
                    (int)ChampionID.Brown => Color.Brown,
                    (int)ChampionID.Rainbow => Main.DiscoColor,
                    _ => drawColor
                };
                if (ret != drawColor)
                {
                    return ret * npc.Opacity;
                }

            }
            if (kingMinion)
            {
                return Color.Blue * npc.Opacity;
            }
            return null;
        }

        public override bool PreAI(NPC npc)
        {
            switch (championType)
            {
                // Move faster
                case (int)ChampionID.Yellow:
                    npc.position += npc.velocity * 2f;
                    break;
                // Leave behind green creep
                case (int)ChampionID.Green:
                    championTimer++;
                    if (championTimer % 30 == 0)
                    Projectile.NewProjectile(npc.GetSource_FromThis(), npc.Bottom, Vector2.Zero, ModContent.ProjectileType<Creep>(), (int)(npc.damage * 0.2f), 0, ai1: npc.noGravity.ToInt());
                    break;
                // Move slower
                case (int)ChampionID.DarkBlue:
                    npc.position -= npc.velocity * 0.22f;
                    break;
                // Don't take damage if it has friends
                case (int)ChampionID.White:
                    npc.dontTakeDamage = NPC.CountNPCS(npc.type) > 1;
                    break;
                // Move slower
                case (int)ChampionID.Gray:
                    npc.position -= npc.velocity * 0.22f;
                    break;
                // Fire projectiles
                case (int)ChampionID.Magenta:
                    championTimer++;
                    if (championTimer % 60 == 0)
                    {
                        if (npc.HasPlayerTarget)
                        Projectile.NewProjectile(npc.GetSource_FromThis(), npc.Center, npc.DirectionTo(Main.player[npc.target].Center) * 8, ProjectileID.BloodNautilusShot, (int)(npc.damage * 0.25f), 0);
                    }
                    break;
                // Absorb projectiles
                case (int)ChampionID.Violet:
                    foreach (Projectile p in Main.projectile)
                    {
                        if (p == null)
                            continue;
                        if (!p.active)
                            continue;
                        if (p.hostile)
                            continue;
                        if (p.Distance(npc.Center) > 600)
                            continue;
                        p.velocity = p.DirectionTo(npc.Center) * p.velocity.Length();
                    }
                    break;
                // Repel projectiles
                case (int)ChampionID.PulsingGray:
                    championTimer++;
                    if (championTimer % 120 < 59)
                    foreach (Projectile p in Main.projectile)
                    {
                        if (p == null)
                            continue;
                        if (!p.active)
                            continue;
                        if (p.hostile)
                            continue;
                        if (p.Distance(npc.Center) > 600)
                            continue;
                        p.velocity = p.DirectionTo(npc.Center) * -p.velocity.Length();
                    }
                    break;
                // Heal non-boss, non-pulsing red champion enemies
                case (int)ChampionID.PulsingRed:
                    championTimer++;
                    if (championTimer % 60 == 0)
                    {
                        SoundEngine.PlaySound(BetterSoundID.ItemDrink);
                        foreach (NPC p in Main.npc)
                        {
                            if (p == null)
                                continue;
                            if (!p.active)
                                continue;
                            if (p.friendly)
                                continue;
                            if (p.dontTakeDamage)
                                continue;
                            if (p.boss)
                                continue;
                            if (p.life <= 0)
                                continue;
                            if (p.GetGlobalNPC<ChampionNPC>().championType == (int)ChampionID.PulsingRed)
                                continue;
                            int heal = (int)MathHelper.Clamp(p.life + p.lifeMax / 100, 0, p.lifeMax - p.life);
                            if (heal > 0)
                            {
                                p.life += heal;
                                p.HealEffect(heal);
                            }
                        }
                    }
                    break;
                // Scale in size
                case (int)ChampionID.Pulsating:
                    championTimer++;
                    if (championTimer % 30 < 15)
                    {
                        npc.scale += 0.05f;
                    }
                    else
                    {
                        npc.scale -= 0.05f;
                    }
                    break;
                // Leave behind blocks
                case (int)ChampionID.Brown:
                    Point tilePos = npc.Bottom.ToTileCoordinates();
                    Tile t = CalamityUtils.ParanoidTileRetrieval(tilePos.X, tilePos.Y);
                    if (t != null)
                    {
                        if (!t.HasTile)
                        {
                            WorldGen.PlaceTile(tilePos.X, tilePos.Y, TileID.MagicalIceBlock);
                        }
                    }
                    break;
                // Schmorgusborg
                case (int)ChampionID.Rainbow:
                    npc.position += npc.velocity * 2f;
                    foreach (Projectile p in Main.projectile)
                    {
                        if (p == null)
                            continue;
                        if (!p.active)
                            continue;
                        if (p.hostile)
                            continue;
                        if (p.Distance(npc.Center) > 600)
                            continue;
                        p.velocity = p.DirectionTo(npc.Center) * p.velocity.Length();
                    }
                    championTimer++;
                    if (championTimer % 60 == 0)
                    {
                        if (npc.HasPlayerTarget)
                            Projectile.NewProjectile(npc.GetSource_FromThis(), npc.Center, npc.DirectionTo(Main.player[npc.target].Center) * 8, ProjectileID.BloodNautilusShot, (int)(npc.damage * 0.25f), 0);
                    }
                    if (championTimer % 30 == 0)
                        Projectile.NewProjectile(npc.GetSource_FromThis(), npc.Bottom, Vector2.Zero, ModContent.ProjectileType<Creep>(), (int)(npc.damage * 0.2f), 0, ai1: npc.noGravity.ToInt());
                    break;
                // Fade in and out
                case (int)ChampionID.Black:
                    championTimer++;
                    if (championTimer % 240 < 119)
                    {
                        if (npc.alpha < 700)
                        npc.alpha += 6;
                    }
                    else
                    {
                        npc.alpha -= 6;
                    }
                    break;
                case (int)ChampionID.Crown:

                    foreach (NPC n in Main.npc)
                    {
                        if (n == null)
                            continue;
                        if (!n.active)
                            continue;
                        if (n.life <= 0)
                            continue;
                        if (n.boss)
                            continue;
                        if (n.dontTakeDamage)
                            continue;
                        if (n.friendly)
                            continue;

                        if (n.TryGetGlobalNPC(out ChampionNPC c))
                        {
                            // Turn all non champions yellow
                            // This is purely visual
                            if (c.championType <= 0)
                            {
                                c.kingMinion = true;
                            }
                        }
                    }
                    break;
            }



            return true;
        }

        public override void ModifyHitPlayer(NPC npc, Player target, ref Player.HurtModifiers modifiers)
        {
            if (championType > 0)
            {
                // All champions at least deal double damage
                modifiers.FinalDamage *= 2;
                // Skull and Large champions deal quadruple
                if (championType == (int)ChampionID.Large || championType == (int)ChampionID.Skull)
                {
                    modifiers.FinalDamage *= 2;
                }
                // Orange champions drop your money
                if (championType == (int)ChampionID.Orange)
                {
                    int coin = Main.rand.Next(10, 60);
                    if (target.CanAfford(Item.buyPrice(silver: coin)))
                    {
                        target.BuyItem(Item.buyPrice(silver: coin));
                        Item.NewItem(npc.GetSource_FromThis(), npc.getRect(), ItemID.SilverCoin, coin);
                    }
                }
            }
        }

        public override void OnKill(NPC npc)
        {
            // Light blue champions explode into projectiles on death
            if (championType == (int)ChampionID.LightBlue || championType == (int)ChampionID.Rainbow)
            {
                int firePoints = 8;
                int fireProjSpeed = 8;
                float variance = MathHelper.TwoPi / firePoints;
                for (int i = 0; i < firePoints; i++)
                {
                    Vector2 velocity = new Vector2(0f, fireProjSpeed);
                    velocity = velocity.RotatedBy(variance * i);
                    Projectile.NewProjectile(npc.GetSource_Death(), npc.Center, velocity, ProjectileID.BloodNautilusShot, (int)(0.25f * npc.damage), 0);
                }
            }
            // Dark cyan champions explode on death
            if (championType == (int)ChampionID.DarkCyan || championType == (int)ChampionID.Rainbow)
            {
                Projectile.NewProjectile(npc.GetSource_Death(), npc.Center, Vector2.Zero, ProjectileID.InfernoHostileBlast, (int)(0.5f * npc.damage), 0);
            }
            // Pulsing green champions spawn two clones on death
            if (championType == (int)ChampionID.PulsingGreen)
            {
                for (int i = 0; i < 2; i++)
                    NPC.NewNPC(npc.GetSource_Death(), (int)npc.position.X, (int)npc.position.Y, npc.type);
            }
            // Skull champions hurt all non-boss enemies on death
            if (championType == (int)ChampionID.Skull)
            {
                foreach (NPC n in Main.npc)
                {
                    if (n == null)
                        continue;
                    if (!n.active)
                        continue;
                    if (n.life <= 0)
                        continue;
                    if (n.dontTakeDamage)
                        continue;
                    if (n.friendly)
                        continue;
                    if (n.boss)
                        continue;
                    n.SimpleStrikeNPC(npc.damage, 1);
                }
            }
            if (kingMinion)
            {
                Item.NewItem(npc.GetSource_Death(), npc.getRect(), ItemID.Star);
            }
        }

        public override bool CheckDead(NPC npc)
        {
            // Dark red champions get a single 10% health revive
            if (!globRevived && championType == (int)ChampionID.DarkRed)
            {
                npc.life = (int)(npc.lifeMax * 0.1f);
                championTimer++;
                globRevived = true;
                return false;
            }
            return true;
        }

        public override void HitEffect(NPC npc, NPC.HitInfo hit)
        {
            if (championType == (int)ChampionID.Pulsating)
            {
                if (NPC.CountNPCS(ModContent.NPCType<ChampEye>()) < 5 && npc.type != ModContent.NPCType<ChampEye>())
                {
                    int n = NPC.NewNPC(npc.GetSource_FromThis(), (int)npc.position.X, (int)npc.position.Y, ModContent.NPCType<ChampEye>());
                    NPC eye = Main.npc[n];
                    eye.lifeMax = eye.life = (int)MathHelper.Max(5, (int)(npc.lifeMax / 20));
                    eye.damage = (int)MathHelper.Max(5, (int)(npc.damage * 0.25f));
                }
            }
        }

        public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Vector2 headPos = npc.Center - Vector2.UnitY * npc.height / 2 - Vector2.UnitY * 22 - screenPos;
            if (championType == (int)ChampionID.Crown)
                spriteBatch.Draw(ModContent.Request<Texture2D>("CalamityMod/Items/Accessories/ScuttlersJewel").Value, headPos, null, drawColor, 0f, ModContent.Request<Texture2D>("CalamityMod/Items/Accessories/ScuttlersJewel").Value.Size() / 2, 0.6f, SpriteEffects.None, 1);
            if (championType == (int)ChampionID.Skull)
                spriteBatch.Draw(ModContent.Request<Texture2D>("CalamityMod/Items/Accessories/OccultSkullCrown").Value, headPos, null, drawColor, 0f, ModContent.Request<Texture2D>("CalamityMod/Items/Accessories/OccultSkullCrown").Value.Size() / 2, 0.4f, SpriteEffects.None, 1);
        }

        public override void SendExtraAI(NPC npc, BitWriter bitWriter, BinaryWriter binaryWriter)
        {
            binaryWriter.Write(championType);
            binaryWriter.Write(championTimer);
            binaryWriter.Write(kingMinion);
            binaryWriter.Write(globRevived);
        }

        public override void ReceiveExtraAI(NPC npc, BitReader bitReader, BinaryReader binaryReader)
        {
            championType = binaryReader.ReadInt32();
            championTimer = binaryReader.ReadInt32();
            kingMinion = binaryReader.ReadBoolean();
            globRevived = binaryReader.ReadBoolean();
        }
    }
}