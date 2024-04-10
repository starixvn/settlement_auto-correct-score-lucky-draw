namespace ThirdPartyBroadcast.Consumer.Criteria;

public class UploadBatchMessageCriteria : NonQueryCommand
{
    public string XMLMessage { get; set; }

    public int CreatorId { get; set; } = 10278938;

    public int ErrorCode { get; set; }

    public string ErrorMsg { get; set; }

    public override string GetSettingKey() => "NAP_SpecialDelete_BatchMessage_Upload";

    public override bool IsValid() => !string.IsNullOrEmpty(XMLMessage);
}