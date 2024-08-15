using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Utilities;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Items.Tools
{
    public class Aurikia5220 : ModItem
    {
        public bool ringing = true;
        private int ringingCooldown = 900;
        SlotId loopSlot;
        public static readonly SoundStyle Ringtone = new("CalRemix/Sounds/AurikiaRingtone") { MaxInstances = 0, IsLooped = true };
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            DisplayName.SetDefault("Aurikia5220");
        }
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 22;
            Item.useStyle = ItemUseStyleID.HoldUp;
        }
        public override void UpdateInventory(Player player)
        {
            if (!ringing && SoundEngine.TryGetActiveSound(loopSlot, out var activeSound))
                activeSound.Stop();
            if (ringing)
            {
                ringingCooldown--;
                if (ringingCooldown <= 0)
                {
                    ringing = false;
                    ringingCooldown = 900;
                }

                if (!SoundEngine.TryGetActiveSound(loopSlot, out var loopSound))
                    loopSlot = SoundEngine.PlaySound(Ringtone, player.Center);
                else
                    loopSound.Position = player.Center;
            }
        }
        public override void PostUpdate()
        {
            if (!ringing && SoundEngine.TryGetActiveSound(loopSlot, out var activeSound))
                activeSound.Stop();
            if (ringing)
            {
                ringingCooldown--;
                if (ringingCooldown <= 0)
                {
                    ringing = false;
                    ringingCooldown = 900;
                }

                if (!SoundEngine.TryGetActiveSound(loopSlot, out var loopSound))
                    loopSlot = SoundEngine.PlaySound(Ringtone, Item.Center);
                else
                    loopSound.Position = Item.Center;
            }
        }
        public override void UpdateInfoAccessory(Player player)
        {
            player.accWatch = 3;
            player.accDepthMeter = 1;
            player.accCompass = 1;

            player.accThirdEye = true;
            player.accJarOfSouls = true;
            player.accCritterGuide = true;

            player.accStopwatch = true;
            player.accOreFinder = true;
            player.accDreamCatcher = true;

            player.accFishFinder = true;
            player.accWeatherRadio = true;
            player.accCalendar = true;
        }
        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            if (ringing)
            {
                int rotation = Main.rand.Next(-45, 46);
                spriteBatch.Draw(TextureAssets.Item[Type].Value, position, frame, itemColor, rotation, origin, scale, SpriteEffects.None, 0);
            }
            return !ringing;
        }
    }
}
