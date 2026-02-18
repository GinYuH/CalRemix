using CalamityMod;
using CalamityMod.Items.Materials;
using CalamityMod.NPCs.Yharon;
using CalRemix.Content.Items.Materials;
using CalRemix.Content.Projectiles;
using CalRemix.Content.Projectiles.Hostile;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace CalRemix.Content.Items.Tools
{
    public class PrisonOpal : ModItem
    {
        public int NPCID = -1;
        public string NPCMod = "";
        public string ModNPCName = "";
        public int NPCHealth = 0;
        public float[] NPCAI = new float[34];

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 5;
        }
        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.rare = ItemRarityID.Red;
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.autoReuse = false;
            Item.UseSound = BetterSoundID.ItemCrystalSerpent with { Pitch = 0.3f };
            Item.shoot = ModContent.ProjectileType<PrisonOpalProj>();
            Item.shootSpeed = 20;
            Item.noUseGraphic = true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (NPCID == -1 && ModNPCName == "")
            {
                return true;
            }
            int p = Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<PrisonOpalProj>(), damage, knockback, player.whoAmI, 1);
            Projectile proj = Main.projectile[p];
            if (proj.ModProjectile != null)
            {
                if (proj.ModProjectile is PrisonOpalProj opal)
                {
                    opal.NPCID = NPCID;
                    opal.NPCMod = NPCMod;
                    opal.ModNPCName = ModNPCName;
                    opal.NPCHealth = NPCHealth;
                    for (int i = 0; i < NPCAI.Length; i++)
                    {
                        opal.NPCAI[i] = NPCAI[i];
                    }
                }
            }
            NPCID = -1;
            NPCMod = "";
            ModNPCName = "";
            NPCHealth = 0;
            for (int i = 0; i < NPCAI.Length; i++)
            {
                NPCAI[i] = 0;
            }
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ModContent.ItemType<VoidSingularity>()).
                AddIngredient(ModContent.ItemType<FrozenSealedTear>(), 3).
                AddTile(TileID.MythrilAnvil).
                Register();
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            if (NPCMod != "" || NPCID != -1)
            {
                NPC guy = NPCMod != "" ? ContentSamples.NpcsByNetId[ModLoader.GetMod(NPCMod).Find<ModNPC>(ModNPCName).Type] : ContentSamples.NpcsByNetId[NPCID];
                tooltips.Add(new(Mod, "OpalDesc", "NPC Stored: " + guy.FullName + "\nHealth: " + NPCHealth));
            }
        }

        public override void SaveData(TagCompound tag)
        {
            tag.Add("NPCID", NPCID);
            tag.Add("NPCMod", NPCMod);
            tag.Add("ModNPCName", ModNPCName);
            tag.Add("NPCHealth", NPCHealth);
            for (int i = 0; i < NPCAI.Length; i++)
            {
                tag.Add("NPCAI_" + i, NPCAI[i]);
            }
        }

        public override void LoadData(TagCompound tag)
        {
            NPCID = tag.GetInt("NPCID");
            NPCMod = tag.GetString("NPCMod");
            ModNPCName = tag.GetString("ModNPCName");
            NPCHealth = tag.GetInt("NPCHealth");
            for (int i = 0; i < NPCAI.Length; i++)
            {
                NPCAI[i] = tag.GetInt("NPCAI_" + i);
            }
        }
    }
}
