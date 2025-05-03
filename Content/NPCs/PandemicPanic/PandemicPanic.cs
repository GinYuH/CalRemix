using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using Terraria.Audio;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.ModLoader.IO;
using System.IO;
using CalRemix.Content.Projectiles.Hostile;
using CalRemix.Content.NPCs.Bosses.Carcinogen;
using CalRemix.Content.NPCs.Bosses.Pathogen;
using CalRemix.Core.World;
using CalamityMod.Projectiles.Typeless;

namespace CalRemix.Content.NPCs.PandemicPanic
{
    public class PandemicPanic : ModSystem
    {
        /// <summary>
        /// Whether or not the event is active
        /// </summary>
        public static bool IsActive;

        /// <summary>
        /// Enemies considered defenders
        /// </summary>
        public static List<int> DefenderNPCs = new List<int>() { ModContent.NPCType<Eosinine>(), ModContent.NPCType<Platelet>(), ModContent.NPCType<WhiteBloodCell>(), ModContent.NPCType<RedBloodCell>(), ModContent.NPCType<Dendritiator>(), ModContent.NPCType<DendtritiatorArm>() };

        /// <summary>
        /// Enemies considered invaders
        /// </summary>
        public static List<int> InvaderNPCs = new List<int>() { ModContent.NPCType<Malignant>(), ModContent.NPCType<Ecolium>(), ModContent.NPCType<Basilius>(), ModContent.NPCType<BasiliusBody>(), ModContent.NPCType<Tobasaia>(), ModContent.NPCType<MaserPhage>(), ModContent.NPCType<Pathogen>() };

        /// <summary>
        /// Projectiles considered defenders
        /// </summary>
        public static List<int> DefenderProjectiles = new List<int>() { ModContent.ProjectileType<EosinineProj>(), ProjectileID.BloodNautilusShot };

        /// <summary>
        /// Projectiles considered invaders
        /// </summary>
        public static List<int> InvaderProjectiles = new List<int>() { ProjectileID.BloodShot, ModContent.ProjectileType<TobaccoSeed>(), ProjectileID.DeathLaser, ModContent.ProjectileType<MaserDeathray>(), ModContent.ProjectileType<PathogenBloodDrop>(), ModContent.ProjectileType<PathogenBloodThorn>(), ModContent.ProjectileType<PathogenCaltrop>() };

        /// <summary>
        /// Defender NPC kill count
        /// </summary>
        public static int DefendersKilled = 0;

        /// <summary>
        /// Invader NPC kill count
        /// </summary>
        public static int InvadersKilled = 0;

        /// <summary>
        /// Total kills, duh
        /// </summary>
        public static float TotalKills => DefendersKilled + InvadersKilled;

        /// <summary>
        /// Defender kills required in order to end the event as an invader
        /// </summary>
        public static float MaxRequired => MinToSummonPathogen + 200;

        /// <summary>
        /// How much higher a faction's kill count has to be to side with them
        /// </summary>
        public const float KillBuffer = 30;

        /// <summary>
        /// The amount of kills required to summon Pathogen
        /// </summary>
        public const float MinToSummonPathogen = 300;

        /// <summary>
        /// If the player is on the defenders' side
        /// </summary>
        public static bool DefendersWinning => ((InvadersKilled > DefendersKilled + KillBuffer) && LockedFinalSide == 0) || LockedFinalSide == 1;

        /// <summary>
        /// If the player is on the invaders' side
        /// </summary>
        public static bool InvadersWinning => ((DefendersKilled > InvadersKilled + KillBuffer) && LockedFinalSide == 0) || LockedFinalSide == -1;

        /// <summary>
        /// If Pathogen has been summoned
        /// </summary>
        public static bool SummonedPathogen = false;

        /// <summary>
        /// This asbestos is some real shit bro
        /// </summary>
        public static float coughTimer = 0;

        /// <summary>
        /// The player's ultimate route. This is decided upon Pathogen's spawn.
        /// </summary>
        public static int LockedFinalSide = 0;

        /// <summary>
        /// All active npcs spawned by the event
        /// </summary>
        public static List<NPC> ActiveNPCs = new List<NPC>();

        /// <summary>
        /// All active projectiles that are used by enemies in the event
        /// </summary>
        public static List<Projectile> ActiveProjectiles = new List<Projectile>();

        public static bool CountsAsActive => IsActive && Main.LocalPlayer.position.Y < Main.worldSurface * 16;

        public override void PreUpdateWorld()
        {
            if (CountsAsActive)
            {
                UpdateLists();
                if (TotalKills >= 300 && (!SummonedPathogen || (!InvadersWinning && !NPC.AnyNPCs(ModContent.NPCType<Pathogen>()))))
                {
                    CalRemixHelper.SpawnNPCOnPlayer(Main.LocalPlayer.whoAmI, ModContent.NPCType<Pathogen>());
                    SummonedPathogen = true;
                }
                if (Main.rand.NextBool((int)MathHelper.Lerp(1200, 300, DefendersKilled / MaxRequired)) && coughTimer <= 0)
                {
                    coughTimer = Main.rand.Next(60, 120);
                }
                if (coughTimer > 0)
                {
                    coughTimer--;
                    if (coughTimer % MathHelper.Lerp(40, 20, DefendersKilled / MaxRequired) == 0)
                    {
                        SoundEngine.PlaySound(Carcinogen.DeathSound);
                        Main.LocalPlayer.Calamity().GeneralScreenShakePower = 100;
                    }
                }
            }
            if (SummonedPathogen && LockedFinalSide == 0)
            {
                if (InvadersWinning)
                {
                    LockedFinalSide = -1;
                }
                if (DefendersWinning)
                {
                    LockedFinalSide = 1;
                }
            }
            if (DefendersKilled >= MaxRequired)
            {
                EndEvent();
            }
        }

        public static void UpdateLists()
        {
            ActiveNPCs.Clear();
            foreach (NPC n in Main.npc)
            {
                if (n == null)
                    continue;
                if (!n.active)
                    continue;
                if (n.life <= 0)
                    continue;
                if (DefenderNPCs.Contains(n.type) || InvaderNPCs.Contains(n.type))
                {
                    ActiveNPCs.Add(n);
                }
            }
            ActiveProjectiles.Clear();
            foreach (Projectile p in Main.projectile)
            {
                if (p == null)
                    continue;
                if (!p.active)
                    continue;
                if (DefenderProjectiles.Contains(p.type) || InvaderProjectiles.Contains(p.type))
                {
                    ActiveProjectiles.Add(p);
                }
            }
        }

        public static void StartEvent()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                IsActive = true;
                DefendersKilled = 0;
                InvadersKilled = 0;
            }
            else
            {
                ModPacket packet = CalRemix.instance.GetPacket();
                packet.Write((byte)RemixMessageType.StartPandemicPanic);
                packet.Send();
            }
            CalamityUtils.DisplayLocalizedText("Mods.CalRemix.StatusText.PandemicPanicBegin", Color.Red);
        }

        public static void EndEvent()
        {
            if (!IsActive)
                return;
            if (NPC.AnyNPCs(ModContent.NPCType<Pathogen>()) && InvadersWinning)
            {
                int path = NPC.FindFirstNPC(ModContent.NPCType<Pathogen>());
                Main.npc[path].NPCLoot();
                Main.npc[path].active = false;
            }
            string winner = InvadersWinning ? "BadEnd" : DefendersWinning ? "GoodEnd" : "NeutralEnd";
            Color c = InvadersWinning ? Color.Red : DefendersWinning? Color.Lime : Color.Ivory;

            CalamityUtils.DisplayLocalizedText("Mods.CalRemix.StatusText.PandemicPanic" + winner, c);

            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                IsActive = false;
                DefendersKilled = 0;
                InvadersKilled = 0;
                LockedFinalSide = 0;
                SummonedPathogen = false;
            }
            else
            {
                ModPacket packet = CalRemix.instance.GetPacket();
                packet.Write((byte)RemixMessageType.EndPandemicPanic);
                packet.Send();
            }
            CalRemixWorld.UpdateWorldBool();
        }

        public override void OnWorldLoad()
        {
            DefendersKilled = 0;
            InvadersKilled = 0;
            LockedFinalSide = 0;
            SummonedPathogen = false;
            IsActive = false;
        }

        public override void OnWorldUnload()
        {
            DefendersKilled = 0;
            InvadersKilled = 0;
            LockedFinalSide = 0;
            SummonedPathogen = false;
            IsActive = false;
        }

        public override void SaveWorldData(TagCompound tag)
        {
            tag["PanDefenders"] = DefendersKilled;
            tag["PanInvaders"] = InvadersKilled;
            tag["PathoSummon"] = SummonedPathogen;
            tag["PanActive"] = IsActive;
            tag["LockedFinalSide"] = LockedFinalSide;
        }

        public override void LoadWorldData(TagCompound tag)
        {
            IsActive = tag.Get<bool>("PanActive");
            SummonedPathogen = tag.Get<bool>("PathoSummon");
            DefendersKilled = tag.Get<int>("PanDefenders");
            InvadersKilled = tag.Get<int>("PanInvaders");
            LockedFinalSide = tag.Get<int>("LockedFinalSide");
        }

        public override void NetSend(BinaryWriter writer)
        {
            writer.Write(DefendersKilled);
            writer.Write(InvadersKilled);
            writer.Write(SummonedPathogen);
            writer.Write(IsActive);
            writer.Write(LockedFinalSide);
        }

        public override void NetReceive(BinaryReader reader)
        {
            DefendersKilled = reader.ReadInt32();
            InvadersKilled = reader.ReadInt32();
            SummonedPathogen = reader.ReadBoolean();
            IsActive = reader.ReadBoolean();
            LockedFinalSide = reader.ReadInt32();
        }

        public static Entity BioGetTarget(bool defender, NPC npc)
        {
            float currentDist = 0;
            Entity targ = null;
            List<int> enemies = defender ? InvaderNPCs : DefenderNPCs;
            if (defender ? DefendersWinning : InvadersWinning)
            {
                foreach (NPC n in ActiveNPCs)
                {
                    if (n == null)
                        continue;
                    if (!n.active)
                        continue;
                    if (n.life <= 0)
                        continue;
                    if (n.type == ModContent.NPCType<DendtritiatorArm>())
                        continue;
                    if (!enemies.Contains(n.type))
                        continue;
                    if (n.Distance(npc.Center) < currentDist)
                        continue;
                    currentDist = n.Distance(npc.Center);
                    targ = n;
                }
            }
            else
            {
                foreach (NPC n in ActiveNPCs)
                {
                    if (n == null)
                        continue;
                    if (!n.active)
                        continue;
                    if (n.life <= 0)
                        continue;
                    if (n.type == ModContent.NPCType<DendtritiatorArm>())
                        continue;
                    if (!enemies.Contains(n.type))
                        continue;
                    if (n.Distance(npc.Center) < currentDist)
                        continue;
                    currentDist = n.Distance(npc.Center);
                    targ = n;
                }
                foreach (Player n in Main.player)
                {
                    if (n == null)
                        continue;
                    if (!n.active)
                        continue;
                    if (n.dead)
                        continue;
                    if (n.Distance(npc.Center) < currentDist)
                        continue;
                    currentDist = n.Distance(npc.Center);
                    targ = n;
                }
            }
            return targ;
        }
    }

    public class PandemicPanicNPC : GlobalNPC
    {
        public float hitCooldown = 0;
        public override bool InstancePerEntity => true;

        public override void ReceiveExtraAI(NPC npc, BitReader bitReader, BinaryReader binaryReader)
        {
            hitCooldown = binaryReader.ReadSingle();
        }

        public override void SendExtraAI(NPC npc, BitWriter bitWriter, BinaryWriter binaryWriter)
        {
            binaryWriter.Write(hitCooldown);
        }

        public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
        {
            return PandemicPanic.DefenderNPCs.Contains(entity.type) || PandemicPanic.InvaderNPCs.Contains(entity.type);
        }

        public override bool PreAI(NPC npc)
        {
            if (npc.life <= 0)
                return true;
            if (hitCooldown > 0)
            {
                hitCooldown--;
                return true;
            }
            if (npc.type != ModContent.NPCType<DendtritiatorArm>() && PandemicPanic.DefenderNPCs.Contains(npc.type))
            {
                npc.chaseable = !PandemicPanic.DefendersWinning;
                foreach (NPC n in PandemicPanic.ActiveNPCs)
                {
                    if (n == null)
                        continue;
                    if (!n.active)
                        continue;
                    if (n.life <= 0)
                        continue;
                    if (!PandemicPanic.InvaderNPCs.Contains(n.type))
                        continue;
                    if (!n.getRect().Intersects(npc.getRect()))
                        continue;
                    int dam = npc.type == ModContent.NPCType<Platelet>() ? (int)(n.damage * 0.33f) : n.damage;
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                        Projectile.NewProjectile(n.GetSource_FromThis(), npc.Center, Vector2.Zero, ModContent.ProjectileType<DirectStrike>(), dam, 0, Main.myPlayer, npc.whoAmI);
                    hitCooldown = 20;
                    if (npc.life <= 0 && n.type == ModContent.NPCType<Malignant>()/* && NPC.CountNPCS(ModContent.NPCType<Malignant>()) < 22*/)
                    {
                        CalRemixHelper.SpawnNewNPC(npc.GetSource_Death(), npc.Center, ModContent.NPCType<Malignant>(), npcTasks: (NPC np) =>
                        {
                            np.npcSlots = 0;
                            np.lifeMax = np.life = (int)MathHelper.Max(1, n.lifeMax / 2);
                            np.damage = (int)MathHelper.Max(1, n.damage * 0.75f);
                            np.scale = MathHelper.Max(0.2f, n.scale * 0.8f);
                        });
                    }
                    break;
                }
                if (hitCooldown > 0)
                {
                    return true;
                }
                foreach (Projectile n in PandemicPanic.ActiveProjectiles)
                {
                    if (n == null)
                        continue;
                    if (!n.active)
                        continue;
                    if (!PandemicPanic.InvaderProjectiles.Contains(n.type))
                        continue;
                    if (!n.getRect().Intersects(npc.getRect()))
                        continue;
                    int dam = npc.type == ModContent.NPCType<Platelet>() ? (int)(n.damage * 0.1f) : n.damage;
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                        Projectile.NewProjectile(n.GetSource_FromThis(), npc.Center, Vector2.Zero, ModContent.ProjectileType<DirectStrike>(), dam * (Main.expertMode ? 2 : 4), 0, Main.myPlayer, npc.whoAmI);
                    n.penetrate--;
                    hitCooldown = 20;
                    break;
                }
            }
            if (PandemicPanic.InvaderNPCs.Contains(npc.type))
            {
                npc.chaseable = !PandemicPanic.InvadersWinning;
                foreach (NPC n in PandemicPanic.ActiveNPCs)
                {
                    if (n == null)
                        continue;
                    if (!n.active)
                        continue;
                    if (n.life <= 0)
                        continue;
                    if (n.type == ModContent.NPCType<Dendritiator>())
                        continue;
                    if (!PandemicPanic.DefenderNPCs.Contains(n.type))
                        continue;
                    bool armhit = false;
                    DendtritiatorArm arm = n.ModNPC<DendtritiatorArm>();
                    if (arm != null)
                    {
                        for (int i = 30 - 1; i > 5; i--)
                        {
                            if (npc.getRect().Intersects(new Rectangle((int)arm.Segments[i].position.X, (int)arm.Segments[i].position.Y, 10, 10)))
                            {
                                armhit = true;
                                break;
                            }
                        }
                    }
                    if (!n.getRect().Intersects(npc.getRect()) && !armhit)
                        continue;
                    if (n.damage <= 0)
                        continue;
                    float damageMult = npc.type == ModContent.NPCType<Pathogen>() ? 0.2f : 1f;
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                        Projectile.NewProjectile(n.GetSource_FromThis(), npc.Center, Vector2.Zero, ModContent.ProjectileType<DirectStrike>(), (int)(n.damage * damageMult), 0, Main.myPlayer, npc.whoAmI);
                    hitCooldown = armhit && npc.type != ModContent.NPCType<Pathogen>() ? 1 : 20;
                    break;
                }
                if (hitCooldown > 0)
                {
                    return true;
                }
                foreach (Projectile n in PandemicPanic.ActiveProjectiles)
                {
                    if (n == null)
                        continue;
                    if (!n.active)
                        continue;
                    if (!PandemicPanic.DefenderProjectiles.Contains(n.type))
                        continue;
                    if (!n.getRect().Intersects(npc.getRect()))
                        continue;
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                        Projectile.NewProjectile(n.GetSource_FromThis(), npc.Center, Vector2.Zero, ModContent.ProjectileType<DirectStrike>(), n.damage * (Main.expertMode ? 2 : 4), 0, Main.myPlayer, npc.whoAmI);
                    n.penetrate--;
                    hitCooldown = 20;
                    if (npc.life <= 0 && !npc.boss && npc.type != ModContent.NPCType<BasiliusBody>())
                    {
                        int killCount = PandemicPanic.InvadersKilled + 1;
                        if (npc.type == ModContent.NPCType<MaserPhage>())
                        {
                            killCount += 4;
                        }
                        if (Main.netMode != NetmodeID.Server)
                        {
                            PandemicPanic.InvadersKilled = killCount;
                        }
                        else
                        {
                            ModPacket packet = Mod.GetPacket();
                            packet.Write((byte)RemixMessageType.KillInvader);
                            packet.Write(killCount);
                            packet.Send();
                        }
                    }
                    break;
                }
            }
            return true;
        }

        public override void OnHitByItem(NPC npc, Player player, Item item, NPC.HitInfo hit, int damageDone)
        {
            if (npc.life <= 0 && !npc.boss && npc.type != ModContent.NPCType<BasiliusBody>())
            {
                int defKill = 0;
                int invKill = 0;
                if (PandemicPanic.InvaderNPCs.Contains(npc.type))
                    invKill = PandemicPanic.InvadersKilled + 1;
                if (PandemicPanic.DefenderNPCs.Contains(npc.type))
                    defKill = PandemicPanic.DefendersKilled + 1;
                if (npc.type == ModContent.NPCType<Dendritiator>())
                {
                    defKill += 4;
                }
                if (npc.type == ModContent.NPCType<MaserPhage>())
                {
                    invKill += 4;
                }


                if (invKill == 0)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        PandemicPanic.DefendersKilled = defKill;
                    }
                    else
                    {
                        ModPacket packet = Mod.GetPacket();
                        packet.Write((byte)RemixMessageType.KillDefender);
                        packet.Write(defKill);
                        packet.Send();
                    }
                }
                else if (defKill == 0)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        PandemicPanic.InvadersKilled = invKill;
                    }
                    else
                    {
                        ModPacket packet = Mod.GetPacket();
                        packet.Write((byte)RemixMessageType.KillInvader);
                        packet.Write(invKill);
                        packet.Send();
                    }
                }
            }
        }

        public override void OnHitByProjectile(NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone)
        {
            if (projectile.friendly)
            {
                if (npc.life <= 0 && !npc.boss && npc.type != ModContent.NPCType<BasiliusBody>())
                {
                    int defKill = 0;
                    int invKill = 0;
                    if (PandemicPanic.InvaderNPCs.Contains(npc.type))
                        invKill = PandemicPanic.InvadersKilled + 1;
                    if (PandemicPanic.DefenderNPCs.Contains(npc.type))
                        defKill = PandemicPanic.DefendersKilled + 1;
                    if (npc.type == ModContent.NPCType<Dendritiator>())
                    {
                        defKill += 4;
                    }
                    if (npc.type == ModContent.NPCType<MaserPhage>())
                    {
                        invKill += 4;
                    }


                    if (invKill == 0)
                    {
                        if (Main.netMode != NetmodeID.Server)
                        {
                            PandemicPanic.DefendersKilled = defKill;
                        }
                        else
                        {
                            ModPacket packet = Mod.GetPacket();
                            packet.Write((byte)RemixMessageType.KillDefender);
                            packet.Write(defKill);
                            packet.Send();
                        }
                    }
                    else if (defKill == 0)
                    {
                        if (Main.netMode != NetmodeID.Server)
                        {
                            PandemicPanic.InvadersKilled = invKill;
                        }
                        else
                        {
                            ModPacket packet = Mod.GetPacket();
                            packet.Write((byte)RemixMessageType.KillInvader);
                            packet.Write(invKill);
                            packet.Send();
                        }
                    }
                }
            }
        }

        public override bool CanHitPlayer(NPC npc, Player target, ref int cooldownSlot)
        {
            if (PandemicPanic.InvaderNPCs.Contains(npc.type) && PandemicPanic.InvadersWinning)
                return false;
            if (PandemicPanic.DefenderNPCs.Contains(npc.type) && PandemicPanic.DefendersWinning)
                return false;
            return true;
        }

        public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
        {
            if (PandemicPanic.CountsAsActive)
            {
                if (PandemicPanic.SummonedPathogen && PandemicPanic.InvadersWinning)
                {
                    maxSpawns += 8;
                    spawnRate = 8;
                }
                else
                {
                    maxSpawns += 4;
                    spawnRate = 16;
                }
            }
        }

        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
            if (PandemicPanic.CountsAsActive && (!NPC.TowerActiveNebula && !NPC.TowerActiveSolar && !NPC.TowerActiveStardust && !NPC.TowerActiveVortex))
            {
                pool.Clear();
                float defMult = PandemicPanic.SummonedPathogen && PandemicPanic.InvadersWinning ? 3f : PandemicPanic.DefendersWinning ? 0.8f : 1f;
                float invMult = PandemicPanic.InvadersWinning ? 0.8f : 1f;
                if (NPC.AnyNPCs(ModContent.NPCType<Pathogen>()) && PandemicPanic.DefendersWinning)
                {
                    defMult = 0.1f;
                    invMult = 0.1f;
                }
                pool.Add(ModContent.NPCType<WhiteBloodCell>(), 0.6f * defMult);
                pool.Add(ModContent.NPCType<RedBloodCell>(), 0.4f * defMult);
                pool.Add(ModContent.NPCType<Platelet>(), 1f * defMult);
                if (!NPC.AnyNPCs(ModContent.NPCType<Eosinine>()))
                    pool.Add(ModContent.NPCType<Eosinine>(), 0.33f * defMult);
                if (!NPC.AnyNPCs(ModContent.NPCType<Dendritiator>()))
                    pool.Add(ModContent.NPCType<Dendritiator>(), 0.025f * defMult);

                pool.Add(ModContent.NPCType<Malignant>(), 0.7f * invMult);
                pool.Add(ModContent.NPCType<Ecolium>(), 0.5f * invMult);
                if (!NPC.AnyNPCs(ModContent.NPCType<Basilius>()))
                    pool.Add(ModContent.NPCType<Basilius>(), 0.1f * invMult);
                pool.Add(ModContent.NPCType<Tobasaia>(), 0.1f * invMult);
                if (!NPC.AnyNPCs(ModContent.NPCType<MaserPhage>()))
                    pool.Add(ModContent.NPCType<MaserPhage>(), 0.025f * invMult);
            }
        }
    }

    public class PandemicPanicProjectile : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        public override bool AppliesToEntity(Projectile entity, bool lateInstantiation)
        {
            return PandemicPanic.DefenderProjectiles.Contains(entity.type) || PandemicPanic.InvaderProjectiles.Contains(entity.type);
        }

        public override bool CanHitPlayer(Projectile projectile, Player target)
        {
            if (!PandemicPanic.IsActive)
                return true;
            if (PandemicPanic.InvaderProjectiles.Contains(projectile.type) && PandemicPanic.InvadersWinning)
                return false;
            if (PandemicPanic.DefenderProjectiles.Contains(projectile.type) && PandemicPanic.DefendersWinning)
                return false;
            return true;
        }
    }
}
