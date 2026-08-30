namespace Server.Items
{
    public class RewardTitleShogunDeed : BaseRewardTitleDeed
    {
        public override TextDefinition Title => 1161986; //Shogun

        [Constructable]
        public RewardTitleShogunDeed()
        {
        }

        public RewardTitleShogunDeed(Serial serial)
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