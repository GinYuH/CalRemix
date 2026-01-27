using CalamityMod;
using CalRemix.Content.Items.Tools;
using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles
{
    public class PrisonOpalProj : ModProjectile
    {
        public override string Texture => "CalRemix/Content/Items/Tools/PrisonOpal";

        public Player Owner => Main.player[Projectile.owner];

        public int NPCID = -1;
        public string NPCMod = "";
        public string ModNPCName = "";
        public int NPCHealth = 0;
        public float[] NPCAI = new float[34];

        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.friendly = true;
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.tileCollide = true;
        }


        public override void AI()
        {
            if (Projectile.ai[0] == 0)
            {
                foreach (NPC n in Main.ActiveNPCs)
                {
                    if (n.getRect().Intersects(Projectile.Hitbox))
                    {
                        Item opal = new Item(ModContent.ItemType<PrisonOpal>());
                        if (opal.ModItem != null)
                        {
                            if (opal.ModItem is PrisonOpal pr)
                            {
                                if (n.ModNPC != null)
                                {
                                    pr.ModNPCName = n.ModNPC.Name;
                                    pr.NPCMod = n.ModNPC.Mod.Name;
                                }
                                else
                                {
                                    pr.NPCID = n.type;
                                }
                                pr.NPCHealth = n.life;
                                for (int i = 0; i < 4; i++)
                                    pr.NPCAI[i] = n.ai[i];
                                for (int i = 0; i < 4; i++)
                                    pr.NPCAI[i + 4] = n.localAI[i];
                                for (int i = 0; i < 4; i++)
                                    pr.NPCAI[i + 8] = n.Calamity().newAI[i];
                                for (int i = 0; i < 22; i++)
                                    pr.NPCAI[i + 12] = n.Remix().GreenAI[i];
                                Item.NewItem(Projectile.GetSource_FromThis(), Projectile.Hitbox, opal);
                            }
                        }
                        SoundEngine.PlaySound(BetterSoundID.ItemCrystalSerpentImpact, Projectile.Center);
                        CalRemixHelper.DustExplosionOutward(Projectile.Center, DustID.CrystalSerpent, speedMin: 8, speedMax: 20, amount: 50, color: default, alpha: 0, scale: 1);
                        n.active = false;
                        Projectile.active = false;
                        break;
                    }
                }
            }
        }

        public override void OnKill(int timeLeft)
        {
            if (Projectile.ai[0] == 1)
            {
                int npc = NPC.NewNPC(Projectile.GetSource_FromThis(), (int)Projectile.Center.X, (int)Projectile.Center.Y, NPCID == -1 ? ModLoader.GetMod(NPCMod).Find<ModNPC>(ModNPCName).Type : NPCID, ai0: NPCAI[0], ai1: NPCAI[1], ai2: NPCAI[2], ai3: NPCAI[3]);
                NPC n = Main.npc[npc];
                for (int i = 0; i < 4; i++)
                    n.localAI[i] = NPCAI[i + 4];
                for (int i = 0; i < 4; i++)
                    n.Calamity().newAI[i] = NPCAI[i + 8];
                for (int i = 0; i < 22; i++)
                    n.Remix().GreenAI[i] = NPCAI[i + 12];
                n.life = NPCHealth;
                n.netUpdate = true;
                CalamityNetcode.SyncNPC(npc);
                CalRemixHelper.DustExplosionOutward(Projectile.Center, DustID.CrystalSerpent, speedMin: 8, speedMax: 20, amount: 50, color: default, alpha: 0, scale: 1);
            }
            Item.NewItem(Projectile.GetSource_FromThis(), Projectile.Hitbox, ModContent.ItemType<PrisonOpal>());
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(NPCID);
            writer.Write(NPCMod);
            writer.Write(ModNPCName);
            writer.Write(NPCHealth);
            for (int i = 0; i < NPCAI.Length; i++)
            {
                writer.Write(NPCAI[i]);
            }
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            NPCID = reader.ReadInt32();
            NPCMod = reader.ReadString();
            ModNPCName = reader.ReadString();
            NPCHealth = reader.ReadInt32();
            for (int i = 0; i < NPCAI.Length; i++)
            {
                NPCAI[i] = reader.ReadSingle();
            }
        }
    }
}