using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using System.Collections.Generic;

namespace CalRemix.UI.Games.Boi.BaseClasses
{
    public enum Factions
    {
        ally,
        enemy
    }

    /// <summary>
    /// Used for any entity that can deal damage, such as bullets or enemies with contact damage.
    /// </summary>
    public interface IDamageDealer
    {
        /// <summary>
        /// What faction is this entity able to deal damage to?
        /// </summary>
        public List<Factions> hostileTo
        {
            get;
        }

        /// <summary>
        /// Checks if for any given active hurtbox, this entity collides with it.
        /// </summary>
        /// <param name="hurtbox">The hurtbox of the entity you're checking if you're hittign</param>
        /// <returns>Wether or not a collision happened</returns>
        public bool HitCheck(CircleHitbox hurtbox);

        /// <summary>
        /// Returns the amount of damage this should deal
        /// You can use this function to add extra on-hit effects as well.
        /// Do not reduce the target's health in this method though. This is handled automatically
        /// </summary>
        /// <returns>The damage dealt</returns>
        public float DealDamage(BoiEntity target);

        /// <summary>
        /// Can the entity currently hit any other?
        /// </summary>
        public bool ActiveHitbox => true;
    }

    /// <summary>
    /// Used for any entity that can take damage.
    /// </summary>
    public interface IDamageable
    {
        /// <summary>
        /// The health of the entity
        /// </summary>
        public float Health
        {
            get;
            set;
        }

        /// <summary>
        /// What faction does this entity belong to?
        /// </summary>
        public Factions Faction
        {
            get;
        }

        /// <summary>
        /// Gets the hurtbox of the entity
        /// </summary>
        public CircleHitbox Hurtbox
        {
            get;
        }

        /// <summary>
        /// Can the entity be hit currently?
        /// </summary>
        public bool Vulnerable => true;

        /// <summary>
        /// Called when the entity gets hit. Use for any on hit effects. Do not use this function to remove health from the entity.
        /// </summary>
        public void TakeHit(float damageTaken);

        /// <summary>
        /// Called when the entity hits zero health.
        /// Return true if the entity's health should be checked again after this method is called. Use this if you want ressurection effects / Death prevention.
        /// </summary>
        public bool Die() { return false; }
    }
}