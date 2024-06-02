using Microsoft.AspNetCore.Mvc;
using Olimpo;
using Olimpo.Domain;

namespace server.Controllers;

[ApiController]
public class StackController : ControllerBase
{
    Context db;

    public StackController(Context db)
    {
        this.db = db;
    }

    [HttpPost("/stack/")]
    public ActionResult CreateStack(Stack? stack){
        try
        {
            var stack_db = db.stacks.Where(x => x.id == stack.id).FirstOrDefault();
            db.Entry(stack_db).CurrentValues.SetValues(stack);
            db.Update(stack_db);
            db.SaveChanges();
            
            return Created($"/stack/{stack.id}", stack);
        }
        catch (System.Exception error)
        {
            return BadRequest(error);
        }
    }

    
}
