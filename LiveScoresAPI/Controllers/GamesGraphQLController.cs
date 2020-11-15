using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Types;
using LiveScoresAPI.Data;
using LiveScoresAPI.GraphQL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Mvc;
namespace LiveScoresAPI.Controllers
{
    [Route("graphql")]
    [ApiController]
    public class GamesGraphQLController : ControllerBase
    {
        private readonly GamesDbContext games;

        public GamesGraphQLController(GamesDbContext games) => this.games = games;

        public async Task<IActionResult> Post([FromBody] GraphQLQuery query)
        {
           // Inputs inputs = query.Variables.ToInputs();

            var schema = new Schema
            {
                Query = new GameQuery(games)
            };

            var result = await new DocumentExecuter().ExecuteAsync(_ =>
            {
                _.Schema = schema;
                _.Query = query.Query;
                _.OperationName = query.OperationName;
                //_.Inputs = inputs;
            });

            if (result.Errors?.Count > 0)
            {
                return BadRequest();
            }

            return Ok(result);
        }
    }
}
