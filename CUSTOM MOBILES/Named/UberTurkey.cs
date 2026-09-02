/*
 * UO Wildlands Custom Script
 * Derived from ServUO Core and Community scripts
 * Compiled & Modified by: [Feng / UO Wildlands Team]
 * Licensed under the GNU General Public License v3.0 (GPL-3.0)
 */
using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("a turkey corpse")]
    public class UberTurkey : BaseCreature
    {
        private DateTime m_NextGobble;
                
        [Constructable]
        public UberTurkey() : this(true)
        {
        }

        [Constructable]
        public UberTurkey(bool tamable) : base(AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            Name        = "an Uber Turkey";
            Body        = 1026;
            BaseSoundID = 0x66A;

            // 50% chance for a special hue, 50% chance for default (0)
            if (Utility.RandomDouble() < 0.5)
            {
                // One of your 6 favorite special hues
                Hue = Utility.RandomList(2101, 1150, 1109, 1901, 1143, 2012);
            }
            else
            {
                // Normal/Natural turkey color
                Hue = 0;
            }
                        
            Tamable      = tamable;
            ControlSlots = 3;       
            MinTameSkill = 108.0;
                        
            
            SetStr(350, 450);
            SetDex(120, 160);
            SetInt(75,  125);

            SetHits(300, 400);
            SetStam(120, 160);
            SetMana(75,  125);
                        
            SetDamage(14, 20);
            SetDamageType(ResistanceType.Physical, 100);
                       
            
            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire,     30, 40);
            SetResistance(ResistanceType.Cold,     25, 35);
            SetResistance(ResistanceType.Poison,   35, 45);
            SetResistance(ResistanceType.Energy,   30, 40);

            
            SetSkill(SkillName.Wrestling,   90.1, 105.0);
            SetSkill(SkillName.Tactics,     85.0, 100.0);
            SetSkill(SkillName.MagicResist, 65.0,  80.0);
            SetSkill(SkillName.Anatomy,     40.0,  60.0);

            
            Fame  = 6500;
            Karma = 0;

            // --- Gobble cooldown init ---
            m_NextGobble = DateTime.UtcNow;
        }
             
        
        
        public override bool CanAngerOnTame { get { return true; } }
        public override int      Meat         { get { return 4; } }   
        public override MeatType MeatType     { get { return MeatType.Bird; } }
        public override FoodType FavoriteFood { get { return FoodType.Meat; } }
        public override int      Feathers     { get { return 50; } }  
                
        public override int GetIdleSound()  { return 0x66A; }
        public override int GetAngerSound() { return 0x66A; }
        public override int GetHurtSound()  { return 0x66B; }
        public override int GetDeathSound() { return 0x66B; }

        
        // Gobble mechanic
        
        public override void OnThink()
        {
            base.OnThink();

            if (Tamable && !Controlled && m_NextGobble < DateTime.UtcNow)
            {
                Say(1153511); // *gobble* *gobble*
                PlaySound(GetIdleSound());

                m_NextGobble = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 240));
            }
        }
                
        public UberTurkey(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_NextGobble = DateTime.UtcNow;
        }
    }
}
