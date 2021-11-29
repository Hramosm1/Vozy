using Microsoft.AspNetCore.Mvc;

namespace vozy_v2_api.models
{
    public class headers
    {
        [FromHeader]
        public string? authorization { get; set; }
    }
}
