namespace Server.Items
{
    public class RewardTitleDaimyoDeed : BaseRewardTitleDeed
    {
        public override TextDefinition Title => 1161985; //Daimyo

        [Constructable]
        public RewardTitleDaimyoDeed()
        {
        }

        public RewardTitleDaimyoDeed(Serial serial)
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