using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Activities;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivitiesController : ControllerBase //Controller wäre für MVC View (wir haben Rest, also nur ControllerBase)
    {
        private readonly IMediator _mediator;

        public ActivitiesController(IMediator mediator)
        {
            this._mediator = mediator;
        }

        // GET api/activities
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Activity>>> List()
        {
            return await _mediator.Send(new List.Query()); //Send ist async

        }

         // GET api/activity/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Activity>> Details(Guid id)
        {
            var value = await _mediator.Send(new Details.Query{Id = id});
            return Ok(value);
        }

          // POST api/activity
          //[FromBody kann man sich hier sparen, da das Attribut [ApiController] oben schlau genug ist. Ohne dem bräuchte mans aber.]
        [HttpPost]
        public async Task<ActionResult<Unit>> Create([FromBody]Create.Command command)
        {
            return await _mediator.Send(command);
        }

         // PUT api/activity/5
         // Unit ist einfach ein leeres Objekt von MediatR. Da wir hier keine Activity zurückgeben,
         // sondern lediglich wissen wollen obs erfolgreich geupdated wurde oder nicht.
        [HttpPut("{id}")]
        public async Task<ActionResult<Unit>> Edit (Guid id, Edit.Command command)
        {
            command.Id = id;
            return await _mediator.Send(command);
        }

        // DELETE api/activity/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Delete(Guid id)
        {
            return await _mediator.Send(new Delete.Command{Id = id});
        }
    }
}