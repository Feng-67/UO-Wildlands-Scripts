namespace Server.Items
{
    public class CowlOfTheArchlich : AssassinsCowl
    {
        [Constructable]
        public CowlOfTheArchlich()
            : this(0)
        {
        }

        [Constructable]
        public CowlOfTheArchlich(int hue)
            : base(hue)
        {
            Name = "Cowl Of The Archlich";
        }

        public CowlOfTheArchlich(Serial serial)
            : base(serial)
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
        }
    }
}
