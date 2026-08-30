namespace Server.Items
{
    public class RewardTitleFeudalLordDeed : BaseRewardTitleDeed
    {
        public override TextDefinition Title => 1161987; //FeudalLord

        [Constructable]
        public RewardTitleFeudalLordDeed()
        {
        }

        public RewardTitleFeudalLordDeed(Serial serial)
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