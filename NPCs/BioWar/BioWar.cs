using CalamityMod.Dusts;
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
using System.Collections.Generic;
using CalamityMod.Events;
using CalamityMod.UI;
using System.Linq;
using Terraria.ModLoader.Core;
using CalRemix.NPCs.TownNPCs;
using Terraria.ModLoader.IO;
using System.IO;
using CalRemix.Projectiles.Hostile;

namespace CalRemix.NPCs.BioWar
{
    public class BioWar : ModSystem
    {
        /// <summary>
        /// Whether or not the event is active
        /// </summary>
        public static bool IsActive;

        /// <summary>
        /// Enemies considered defenders
        /// </summary>
        public static List<int> DefenderNPCs = new List<int>() { ModContent.NPCType<Eosinine>() };

        /// <summary>
        /// Enemies considered invaders
        /// </summary>
        public static List<int> InvaderNPCs = new List<int>() { };

        /// <summary>
        /// Projectiles considered defenders
        /// </summary>
        public static List<int> DefenderProjectiles = new List<int>() { ModContent.ProjectileType<EosinineProj>() };

        /// <summary>
        /// Projectiles considered invaders
        /// </summary>
        public static List<int> InvaderProjectiles = new List<int>() { };

        /// <summary>
        /// Defender NPC kill count
        /// </summary>
        public static float DefendersKilled = 0;

        /// <summary>
        /// Invader NPC kill count
        /// </summary>
        public static float InvadersKilled = 0;

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
        public static bool DefendersWinning => InvadersKilled > DefendersKilled + KillBuffer;

        /// <summary>
        /// If the player is on the invaders' side
        /// </summary>
        public static bool InvadersWinning => DefendersKilled > InvadersKilled + KillBuffer;

        /// <summary>
        /// If Pathogen has been summoned
        /// </summary>
        public static bool SummonedPathogen = false;

        public override void PreUpdateWorld()
        {
            IsActive = true;
            if (IsActive)
            {
                if (TotalKills > 300 && !SummonedPathogen)
                {
                    NPC.SpawnOnPlayer(Main.LocalPlayer.whoAmI, NPCID.Frankenstein);
                    SummonedPathogen = true;
                }
            }
            if (DefendersKilled > MaxRequired)
            {
                EndEvent();
            }
        }

        public static void EndEvent()
        {
            DefendersKilled = 0;
            InvadersKilled = 0;
            SummonedPathogen = false;
            IsActive = false;
            CalRemixWorld.UpdateWorldBool();
        }

        public override void OnWorldLoad()
        {
            DefendersKilled = 0;
            InvadersKilled = 0;
            SummonedPathogen = false;
            IsActive = false;
        }

        public override void OnWorldUnload()
        {
            DefendersKilled = 0;
            InvadersKilled = 0;
            SummonedPathogen = false;
            IsActive = false;
        }

        public override void SaveWorldData(TagCompound tag)
        {
            tag["BioDefenders"] = DefendersKilled;
            tag["BioInvaders"] = InvadersKilled;
            tag["PathoSummon"] = SummonedPathogen;
            tag["BioActive"] = IsActive;
        }

        public override void LoadWorldData(TagCompound tag)
        {
            IsActive = tag.Get<bool>("BioActive");
            SummonedPathogen = tag.Get<bool>("PathoSummon");
            DefendersKilled = tag.Get<float>("BioDefenders");
            InvadersKilled = tag.Get<float>("BioInvaders");
        }

        public override void NetSend(BinaryWriter writer)
        {
            writer.Write(DefendersKilled);
            writer.Write(InvadersKilled);
            writer.Write(SummonedPathogen);
            writer.Write(IsActive);
        }

        public override void NetReceive(BinaryReader reader)
        {
            DefendersKilled = reader.ReadSingle();
            InvadersKilled = reader.ReadSingle();
            SummonedPathogen = reader.ReadBoolean();
            IsActive = reader.ReadBoolean();
        }
    }
}
