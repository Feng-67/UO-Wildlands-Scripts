namespace Server.Items
{
    public class RewardTitlePortentOfTheEquinoxDeed : BaseRewardTitleDeed
    {
        public override TextDefinition Title => 1159915; //Portent of the Equinox

        [Constructable]
        public RewardTitlePortentOfTheEquinoxDeed()
        {
        }

        public RewardTitlePortentOfTheEquinoxDeed(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int v = reader.ReadInt();
        }
    }
}