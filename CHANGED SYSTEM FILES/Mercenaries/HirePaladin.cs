using System;
using Server.Items;

namespace Server.Mobiles
{
    public class HirePaladin : BaseHire
    {
        [Constructable]
        public HirePaladin()
        {
            ControlSlots = 4;
            SpeechHue = Utility.RandomDyedHue();
            Hue       = Utility.RandomSkinHue();

            Title      = "the paladin";
            HairItemID = Race.RandomHair(Female);
            HairHue    = Race.RandomHairHue();
            Race.RandomFacialHair(this);

            SetStr(600, 800);
            SetDex(200, 200);
            SetInt(200, 300);

            SetHits(800);
            SetDamage(18, 28);

            SetSkill(SkillName.Swords,   120, 120);
            SetSkill(SkillName.Anatomy,  120, 120);
            SetSkill(SkillName.Tactics,  120, 120);
            SetSkill(SkillName.Parry,    120, 120);
            SetSkill(SkillName.Chivalry, 120, 120);

            SetResistance(ResistanceType.Physical, 70);
            SetResistance(ResistanceType.Fire,     70);
            SetResistance(ResistanceType.Cold,     70);
            SetResistance(ResistanceType.Poison,   70);
            SetResistance(ResistanceType.Energy,   70);

            Fame  = 10000;
            Karma = 10000;

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
            }

            BaseArmor helm;
            switch (Utility.Random(5))
            {
                case 1:  helm = new Bascinet();  break;
                case 2:  helm = new CloseHelm(); break;
                case 3:  helm = new NorseHelm(); break;
                case 4:  helm = new Helmet();    break;
                default: helm = null;            break;
            }
            if (helm != null) EquipItem(helm);

            AddItem(new Shoes(Utility.RandomNeutralHue()));
            AddItem(new Shirt());
            AddItem(new VikingSword());
            AddItem(new MetalKiteShield());

            AddItem(new PlateChest());
            AddItem(new PlateLegs());
            AddItem(new PlateArms());
            AddItem(new LeatherGorget());

            PackItem(new BookOfChivalry());

            TithingPoints = 20000;
            PackGold(0, 0);
        }

        public HirePaladin(Serial serial) : base(serial) { }

        public override bool ClickTitle { get { return false; } }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add("<BASEFONT COLOR=#C0C0C0>Legendary Paladin</BASEFONT>");
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt();
        }
    }
}