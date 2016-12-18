using System;
using System.Net;
using System.Web.Http;
using IssuingService.Models;
using ServiceStack.Redis;

namespace IssuingService.Controllers
{
    public class CardHolderController : ApiController
    {
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
        public CardHolder Get(string id)
        {
            if (id == "1")
                return new CardHolder {ID = "1", Firstname = "Marco", Lastname = "Bernasconi"};

            var redis = new RedisClient("redis", 6379);
            var cardHolder = redis.Get<CardHolder>("cardholder_" + id);
            if(cardHolder == null)
                throw  new HttpResponseException(HttpStatusCode.NotFound);

            return cardHolder;
        }
    }
}