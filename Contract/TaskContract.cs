using Newtonsoft.Json;

namespace RocketAnt.Contract
{
    public class TaskContract
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("numOfSteps")]
        public int NumOfSteps { get; set; }
        [JsonProperty("currentStep")]
        public int CurrentStep { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("id")]
        public bool IsCompleted { get; set; }
    }
}