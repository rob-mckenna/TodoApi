using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace TodoApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TodoListController : ControllerBase
{
    private readonly TodoContext _context;

    public TodoListController(TodoContext context)
    {
        _context = context;
    }

    // GET: api/TodoList
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoListDTO>>> GetTodoItems()
    {
        return await _context.TodoList
            .Select(x => ListToDTO(x))
            .ToListAsync();
    }

    // GET: api/TodoList/5
    // <snippet_GetByID>
    [HttpGet("{id}")]
    public async Task<ActionResult<TodoListDTO>> GetTodoList(long id)
    {
        var todoList = await _context.TodoList.FindAsync(id);

        if (todoList == null)
        {
            return NotFound();
        }

        return ListToDTO(todoList);
    }
    // </snippet_GetByID>

    // PUT: api/TodoList/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    // <snippet_Update>
    [HttpPut("{id}")]
    public async Task<IActionResult> PutTodoList(long id, TodoListDTO todoDTO)
    {
        if (id != todoDTO.Id)
        {
            return BadRequest();
        }

        var todoList = await _context.TodoList.FindAsync(id);
        if (todoList == null)
        {
            return NotFound();
        }

        todoList.Name = todoDTO.Name;
        todoList.Completed = todoDTO.Completed;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException) when (!TodoListExists(id))
        {
            return NotFound();
        }

        return NoContent();
    }
    // </snippet_Update>

    // POST: api/TodoList
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    // <snippet_Create>
    [HttpPost]
    public async Task<ActionResult<TodoListDTO>> PostTodoItem(TodoListDTO todoDTO)
    {
        var todoList = new TodoList
        {
            Completed = todoDTO.Completed,
            Name = todoDTO.Name
        };

        _context.TodoList.Add(todoList);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetTodoList),
            new { id = todoList.Id },
            ListToDTO(todoList));
    }
    // </snippet_Create>

    // DELETE: api/TodoList/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTodoList(long id)
    {
        var todoList = await _context.TodoList.FindAsync(id);
        if (todoList == null)
        {
            return NotFound();
        }

        _context.TodoList.Remove(todoList);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool TodoListExists(long id)
    {
        return _context.TodoList.Any(e => e.Id == id);
    }

    private static TodoListDTO ListToDTO(TodoList todoList) =>
       new TodoListDTO
       {
           Id = todoList.Id,
           Name = todoList.Name,
           Completed = todoList.Completed
       };
}