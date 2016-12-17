using System.Collections.Generic;
using System.Web.Http;

namespace IssuingService.Controllers
{
    public class CardHolderController : ApiController
    {
        [Route("api/cardholders")]
        [HttpGet]
        public List<string> GetCardHolders()
        {
            return new List<string> {"1", "2"};
        }

        [Route("api/cardholder/{id}")]
        [HttpGet]
        public string Get(string id)
        {
            return id + " ciao";
        }
    }
}