using static Terraria.ModLoader.ModContent;
using static CalRemix.Core.CustomGen;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using CalamityMod.NPCs.Cryogen;
using CalRemix.Core;
using Terraria.ModLoader.IO;

namespace CalRemix.Content.Items.Misc
{
	public class TheGenerator : ModItem
	{
        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 10;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.value = Item.buyPrice(10);
            Item.autoReuse = false;
            Item.uniqueStack = true;
            Item.rare = ItemRarityID.Cyan;
        }
        public override bool AltFunctionUse(Player player) => true;
        public override bool CanUseItem(Player player) => !player.GetModPlayer<GeneratorPlayer>().generating;
        public override bool? UseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                player.GetModPlayer<GeneratorPlayer>().gen = new(0, Color.White, false, true, 0, Color.White, false, true);
                if (Main.myPlayer == player.whoAmI)
                    SoundEngine.PlaySound(Cryogen.DeathSound);
            }
            else if (player.altFunctionUse != 2 && player.ItemAnimationJustStarted && !player.GetModPlayer<GeneratorPlayer>().generating)
                player.GetModPlayer<GeneratorPlayer>().generating = true;
            return true;
        }
        public override bool ConsumeItem(Player player) => false;
        public override bool CanRightClick() => true;
        public override void RightClick(Player player)
        {
            Item.ChangeItemType(ItemType<TheInactiveGenerator>());
        }
        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Texture2D texture = Request<Texture2D>(Texture).Value;
            spriteBatch.Draw(texture, position - new Vector2(texture.Width * 0.5f, texture.Height * 0.5f), Main.DiscoColor);
            return false;
        }
        public override void UpdateInventory(Player player)
        {
            player.GetModPlayer<GeneratorPlayer>().genActive = true;
        }
    }
    public class TheInactiveGenerator : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.IsLavaImmuneRegardlessOfRarity[Type] = true;
        }
        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 10;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.value = 0;
            Item.autoReuse = false;
            Item.uniqueStack = true;
            Item.rare = ItemRarityID.Gray;
        }
        public override bool CanUseItem(Player player) => !player.GetModPlayer<GeneratorPlayer>().generating;
        public override bool? UseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                player.GetModPlayer<GeneratorPlayer>().gen = new(0, Color.White, false, true, 0, Color.White, false, true);
                if (Main.myPlayer == player.whoAmI)
                    SoundEngine.PlaySound(Cryogen.DeathSound);
            }
            else if (player.altFunctionUse != 2 && player.ItemAnimationJustStarted && !player.GetModPlayer<GeneratorPlayer>().generating)
                player.GetModPlayer<GeneratorPlayer>().generating = true;
            return true;
        }
        public override bool ConsumeItem(Player player) => false;
        public override bool CanRightClick() => true;
        public override void RightClick(Player player)
        {
            Item.ChangeItemType(ItemType<TheGenerator>());
        }
    }
    public class GenSerializer : TagSerializer<CustomGen, TagCompound>
    {
        public override TagCompound Serialize(CustomGen value) => new TagCompound
        {
            ["CoreTexture"] = value.CoreTexture,
            ["CoreColor"] = value.CoreColor,
            ["CoreGlow"] = value.CoreGlow,
            ["CoreVisible"] = value.CoreVisible,

            ["ShieldTexture"] = value.ShieldTexture,
            ["ShieldColor"] = value.ShieldColor,
            ["ShieldGlow"] = value.ShieldGlow,
            ["ShieldVisible"] = value.ShieldVisible
        };
        public override CustomGen Deserialize(TagCompound tag)
        {
            return new(tag.GetInt("CoreTexture"), tag.Get<Color>("CoreColor"), tag.GetBool("CoreGlow"), tag.GetBool("CoreVisible"), tag.GetInt("ShieldTexture"), tag.Get<Color>("ShieldColor"), tag.GetBool("ShieldGlow"), tag.GetBool("ShieldVisible"));
        }
    }
    public class GeneratorPlayer : ModPlayer
    {
        internal CustomGen gen = new(1, Color.White, false, true, 3, Color.White, false, true);
        public bool generating = false;
        public bool genActive = false;
        public bool music = true;
        private static readonly List<PlayerDrawLayer> HiddenLayers =
        [
            PlayerDrawLayers.Wings,
            PlayerDrawLayers.HeadBack,
            PlayerDrawLayers.Torso,
            PlayerDrawLayers.Skin,
            PlayerDrawLayers.Leggings,
            PlayerDrawLayers.Shoes,
            PlayerDrawLayers.Robe,
            PlayerDrawLayers.SkinLongCoat,
            PlayerDrawLayers.ArmorLongCoat,
            PlayerDrawLayers.Head,
            PlayerDrawLayers.Shield,
            PlayerDrawLayers.ArmOverItem,
            PlayerDrawLayers.HandOnAcc
        ];
        public override void ResetEffects()
        {
            genActive = false;
        }
        public override void HideDrawLayers(PlayerDrawSet drawInfo)
        {
            if (genActive && gen.CoreVisible)
            {
                foreach (PlayerDrawLayer layer in PlayerDrawLayerLoader.Layers)
                {
                    if (layer == null)
                        continue;
                    if (HiddenLayers.Contains(layer))
                        layer.Hide();
                }
            }
        }
        public override void SaveData(TagCompound tag)
        {
            tag["GeneratorPlayerGen"] = gen;
            tag["GeneratorPlayerMusic"] = music;
        }
        public override void LoadData(TagCompound tag)
        {
            gen = tag.Get<CustomGen>("GeneratorPlayerGen");
            music = tag.GetBool("GeneratorPlayerMusic");
        }
        public override void CopyClientState(ModPlayer targetCopy)
        {
            GeneratorPlayer clone = (GeneratorPlayer)targetCopy;
            Copy(clone.gen, gen);
        }
        public override void SendClientChanges(ModPlayer clientPlayer)
        {
            GeneratorPlayer clone = (GeneratorPlayer)clientPlayer;
            if (gen != clone.gen)
                SyncPlayer(toWho: -1, fromWho: Main.myPlayer, newPlayer: false);
        }
    }
    public class GeneratorLayer : PlayerDrawLayer
    {
        public override bool IsHeadLayer => false;
        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) => drawInfo.drawPlayer.GetModPlayer<GeneratorPlayer>().genActive && !drawInfo.drawPlayer.GetModPlayer<GeneratorPlayer>().generating;
        public override Position GetDefaultPosition() => new Between(PlayerDrawLayers.FaceAcc, PlayerDrawLayers.MountFront);
        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            Player player = drawInfo.drawPlayer;
            CustomGen gen = player.GetModPlayer<GeneratorPlayer>().gen;

            float rotation = MathHelper.TwoPi / 2 * (Main.GlobalTimeWrappedHourly % 2);

            Texture2D shield = GetTexture2D("Shield", gen.ShieldTexture, false);
            Texture2D core = GetTexture2D(string.Empty, gen.CoreTexture, false);
            Texture2D shieldGlow = GetTexture2D("Shield", gen.ShieldTexture, true);
            Texture2D coreGlow = GetTexture2D(string.Empty, gen.CoreTexture, true);


            var position = (player.mount.Type != MountID.None) ? player.MountedCenter - Vector2.UnitY * (player.height * 0.45f) : player.Center;
            position -= Main.screenPosition;
            position = new Vector2((int)position.X, (int)position.Y);

            float lighting = Lighting.GetColor((int)player.Center.X / 16, (int)player.Center.Y / 16).ToVector3().Length();
            Color color = (gen.CoreColor * lighting);
            color.A = 255;
            Color color2 = (gen.ShieldColor * lighting);
            color2.A = 255;
            Color glowColor = Color.White * lighting;
            glowColor.A = 255;

            if (gen.ShieldVisible)
            {
                drawInfo.DrawDataCache.Add(new DrawData(shield, position, null, color2, rotation, shield.Size() * 0.5f, 1f, SpriteEffects.None, 0));
                if (gen.ShieldGlow)
                    drawInfo.DrawDataCache.Add(new DrawData(shieldGlow, position, null, glowColor, rotation, shield.Size() * 0.5f, 1f, SpriteEffects.None, 0));
            }
            if (gen.CoreVisible)
            {
                drawInfo.DrawDataCache.Add(new DrawData(core, position, null, color, player.fullRotation, core.Size() * 0.5f, 1f, SpriteEffects.None, 0));
                if (gen.CoreGlow)
                    drawInfo.DrawDataCache.Add(new DrawData(coreGlow, position, null, glowColor, player.fullRotation, core.Size() * 0.5f, 1f, SpriteEffects.None, 0));
            }

        }
    }
}