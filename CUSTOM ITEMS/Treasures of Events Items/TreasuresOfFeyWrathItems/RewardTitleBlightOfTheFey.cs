namespace Server.Items
{
    public class RewardTitleBlightOfTheFeyDeed : BaseRewardTitleDeed
    {
        public override TextDefinition Title => 1159914; //Blight of the Fey

        [Constructable]
        public RewardTitleBlightOfTheFeyDeed()
        {
        }

        public RewardTitleBlightOfTheFeyDeed(Serial serial)
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