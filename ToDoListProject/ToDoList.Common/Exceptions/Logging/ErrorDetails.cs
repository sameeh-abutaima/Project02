using Newtonsoft.Json;

namespace ToDoList.Common.Exceptions.Logging
{
    public class ErrorDetails
    {
        #region Props
        [JsonProperty("status")]
        public int Status { get; set; }
        [JsonProperty("errorCode")]
        public int ErrorCode { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
        #endregion Props

        #region Methods
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion Methods
    }
}
