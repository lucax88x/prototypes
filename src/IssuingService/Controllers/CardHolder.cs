using System;
using System.Collections.Generic;
using System.Web.Http;
using ServiceStack.Redis;

namespace IssuingService.Controllers
{
    public class CardHolderController : ApiController
    {

        [Route("api/cardholders")]
        [HttpGet]
        public List<string> GetCardHolders()
        {
            return new List<string> { "1", "2" };
        }

        [Route("api/cardholders/counter")]
        [HttpGet]
        public long GetCouter()
        {
            try
            {
                var redis = new RedisClient("redis", 6379);

                var counter = redis.Increment("counter", 1);
                return counter;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                return -1;
            }
        }

        [Route("api/cardholder/{id}")]
        [HttpGet]
        public string Get(string id)
        {
            return id + " ciao";
        }
    }
}