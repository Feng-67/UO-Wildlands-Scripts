using System;
using Server.Items;

namespace Server.Mobiles
{
    public class HireRangerArcher : BaseHire
    {
        [Constructable]
        public HireRangerArcher() : base(AIType.AI_Archer)
        {
            ControlSlots = 4;
            SpeechHue = Utility.RandomDyedHue();
            Hue       = Utility.RandomSkinHue();

            Title      = "the archer";
            HairItemID = Race.RandomHair(Female);
            HairHue    = Race.RandomHairHue();
            Race.RandomFacialHair(this);

            SetStr(600, 700);
            SetDex(200, 300);
            SetInt(200, 300);

            SetHits(600);
            SetDamage(18, 28);

            SetSkill(SkillName.Archery,     120, 120);
            SetSkill(SkillName.Tactics,     120, 120);
            SetSkill(SkillName.Anatomy,     120, 120);
            SetSkill(SkillName.MagicResist, 120, 120);
            SetSkill(SkillName.Chivalry,    120, 120);

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

            AddItem(new Shirt());
            AddItem(new Boots(Utility.RandomNeutralHue()));
            AddItem(new Bow());
            AddItem(new BaseQuiver());

            switch (Utility.Random(2))
            {
                case 0:
                    AddItem(new LeatherChest()); AddItem(new LeatherArms());
                    AddItem(new LeatherGloves()); AddItem(new LeatherLegs());
                    AddItem(new LeatherGorget());
                    break;
                case 1:
                    AddItem(new StuddedChest()); AddItem(new StuddedArms());
                    AddItem(new StuddedGloves()); AddItem(new StuddedLegs());
                    AddItem(new StuddedGorget());
                    break;
            }

            PackItem(new BookOfChivalry());

            TithingPoints = 10000;
            PackGold(0, 0);
        }

        public HireRangerArcher(Serial serial) : base(serial) { }

        public override bool ClickTitle { get { return false; } }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add("<BASEFONT COLOR=#C0C0C0>Legendary Archer</BASEFONT>");
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