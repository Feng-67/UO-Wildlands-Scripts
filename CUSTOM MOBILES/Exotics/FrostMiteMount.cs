/*
 * UO Wildlands Custom Script
 * Derived from ServUO Core and Community scripts (Original Author SharpDevelop)
 * Compiled & Modified by: [Feng / UO Wildlands Team]
 * * Licensed under the GNU General Public License v3.0 (GPL-3.0)
 */
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a frost mite corpse")]
    public class FrostMiteMount : BaseMount, IAuraCreature
    {

        private bool isExotic;
    	[Constructable]
        public FrostMiteMount()
            : this("a Frost Mite")
        {
        }
        
        [Constructable]
        public FrostMiteMount(string name) : base(name, 0x590, 0x3EDA, AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Frost Mite";
            Body = 0x590;
            //Female = true;

            SetStr(1017);
            SetDex(164);
            SetInt(283);

            SetHits(800, 1000);

            SetDamage(21, 28);

            SetDamageType(ResistanceType.Physical, 0);
            SetDamageType(ResistanceType.Cold, 100);

            SetResistance(ResistanceType.Physical, 50, 65);
            SetResistance(ResistanceType.Fire, 15, 25);
            SetResistance(ResistanceType.Cold, 85, 95);
            SetResistance(ResistanceType.Poison, 50, 65);
            SetResistance(ResistanceType.Energy, 40, 55);

            SetSkill(SkillName.MagicResist, 50.0, 85.0);
            SetSkill(SkillName.Tactics, 70.0, 105.0);
            SetSkill(SkillName.Wrestling, 70.0, 110.0);
            SetSkill(SkillName.DetectHidden, 60.0, 80.0);
            SetSkill(SkillName.Focus, 100.0, 115.0);

            Tamable = true;
            ControlSlots = 3;
            ControlSlotsMax = 5;
            MinTameSkill = 108;
            //isExotic = true;

            SetWeaponAbility(WeaponAbility.ColdWind);
            SetAreaEffect(AreaEffect.AuraDamage);
			InvalidateProperties();
		}


        public override int GetAngerSound()
        {
            return 0x4E8;
        }

        public override int GetIdleSound()
        {
            return 0x4E7;
        }

        public override int GetAttackSound()
        {
            return 0x4E6;
        }

        public override int GetHurtSound()
        {
            return 0x4E9;
        }

        public override int GetDeathSound()
        {
            return 0x4E5;
        }

        public override int Meat => 5;
        public override FoodType FavoriteFood => FoodType.Meat;

        public override bool CanAngerOnTame => true;
        public override bool StatLossAfterTame => true;

        public void AuraEffect(Mobile m)
        {
            m.FixedParticles(0x374A, 10, 30, 5052, Hue, 0, EffectLayer.Waist);
            m.PlaySound(0x5C6);

            m.SendLocalizedMessage(1008111, false, Name); //  : The intense cold is damaging you!
        }

        public FrostMiteMount(Serial serial) : base(serial)
        {
        }

        public override TrainingDefinition TrainingDefinition
        {
            get
            {
                return new TrainingDefinition(
                    typeof(AncientHellhound),
                    Class.ClawedTailedNecromanticAndTokuno,
                    MagicalAbility.Wolf | MagicalAbility.Poisoning | MagicalAbility.Discordance | MagicalAbility.Necromancy,
                    PetTrainingHelper.SpecialAbilityClawedAndNecromantic,
                    PetTrainingHelper.WepAbility2,
                    PetTrainingHelper.AreaEffectArea2,
                    3, 5);
            }
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add("<BASEFONT COLOR=#FFD700>Exotic</BASEFONT>");
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(1); // version

            writer.Write(isExotic);
        }
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            switch (version)
            {
                case 1:
                {
                    isExotic = reader.ReadBool();
                    goto case 0;
                }
                case 0:
                {
                    break;
                }
            }
        }
    }
}
