/*
 * UO Wildlands Custom Script
 * Derived from ServUO Core and Community scripts (Original Author Zigholtul)
 * Compiled & Modified by: [Feng / UO Wildlands Team]
 * * Licensed under the GNU General Public License v3.0 (GPL-3.0)
 */
namespace Server.Mobiles
{
    [CorpseName("a capybara corpse")]
    public class Capybara : BaseMount
    {
        [Constructable]
        public Capybara(): this("Capybara")
        {
        }

        [Constructable]
        public Capybara(string name): base(name, 0x5F7, 0x3ED3, AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            BaseSoundID = 0x188;

            SetStr(1200, 1300);
            SetDex(284, 384);
            SetInt(226, 250);

            SetHits(1200, 1250);

            SetDamage(20, 25);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 50);

            SetResistance(ResistanceType.Physical, 70, 85);
            SetResistance(ResistanceType.Fire, 70, 85);
            SetResistance(ResistanceType.Cold, 25, 45);
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.Wrestling, 90.1, 105.8);
            SetSkill(SkillName.Tactics, 89.3, 98.3);
            SetSkill(SkillName.MagicResist, 59.3, 69.0);
            SetSkill(SkillName.Anatomy, 55.5, 70.4);

            Fame = 300;
            Karma = 300;

            Tamable = true;
            ControlSlots = 3;
            MinTameSkill = 108;
        }

        public Capybara(Serial serial): base(serial)
        {
        }
                
                
        public override FoodType FavoriteFood { get { return FoodType.Meat; } }
        public override bool CanAngerOnTame { get { return true; } }
        public override bool StatLossAfterTame { get { return true; } }
        public override int Hides { get { return 20; } }
        public override int Meat { get { return 16; } }
        
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
