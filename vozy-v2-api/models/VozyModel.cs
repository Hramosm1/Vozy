namespace vozy_v2_api.models
{
    public class VozyModel
    {
        public VozyModel(string json)
        {
            this.jsonData = json;
        }
        public string? id { get; set; }
        public string jsonData { get; set; }
    }
}
