using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data;
using MessagingService.Interfaces;
using PublisherDemo.DTOs.Requests;
using PublisherDemo.DTOs.Responses;
using PublisherDemo.Models;
using Action = PublisherDemo.Models.Action;

namespace PublisherDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoListController : ControllerBase
    {
        private readonly TodoContext _context;
        private readonly IMapper _mapper;
        private IMessagingService<TodoNotification> _messagingService;

        public TodoListController(TodoContext context, IMapper mapper, IMessagingService<TodoNotification> messagingService)
        {
            _context = context;
            _mapper = mapper;
            _messagingService = messagingService;
        }

        // GET: api/TodoList
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoListResponse>>> GetTodoList()
        {
            return await _context.TodoList.Include(l => l.TodoItems).ProjectTo<TodoListResponse>(_mapper.ConfigurationProvider).ToListAsync();
        }

        // GET: api/TodoList/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoListResponse>> GetTodoList(int id)
        {
            var todoList = await _context.TodoList.Include(l => l.TodoItems).FirstOrDefaultAsync(l => l.Id == id);

            if (todoList == null)
            {
                return NotFound();
            }

            return _mapper.Map<TodoListResponse>(todoList);
        }

        // Post: api/TodoList/5/TodoItem
        [HttpPost("{id}/TodoItem")]
        public async Task<IActionResult> PostTodoList(int id, TodoItemRequest todoItem)
        {
            var list = await _context.TodoList.FirstOrDefaultAsync(l => l.Id == id);
            if (list == null)
            {
                return NotFound();
            }

            var item = new TodoItem
            {
                Description = todoItem.Description,
                DueDate = todoItem.DueDate,
                Done = todoItem.Done,
            };
            
            list.TodoItems.Add(item);
            
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoListExists(id))
                {
                    return NotFound();
                }

                throw;
            }

            await _messagingService.PublishAsync(new TodoNotification
            {
                Action = Action.Created,
                TodoItem = _mapper.Map<TodoItemResponse>(item)
            });
            return NoContent();
        }

        // Put: api/TodoList/5/TodoItem/4
        [HttpPut("{id}/TodoItem/{itemId}")]
        public async Task<IActionResult> PutTodoList(int id, int itemId, TodoItemRequest todoItemRequest)
        {
            var item = await _context.TodoItem.FindAsync(itemId);
            if (item == null)
            {
                return NotFound();
            }
            _mapper.Map(todoItemRequest, item);
            await _context.SaveChangesAsync();
            await _messagingService.PublishAsync(new TodoNotification
            {
                Action = Action.Updated,
                TodoItem = _mapper.Map<TodoItemResponse>(item)
            });
            return NoContent();
        }
        
        // POST: api/TodoList
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<TodoListResponse>> PostTodoList(string name)
        {
            var todo = _context.TodoList.Add(new TodoList { Name = name });
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTodoList", new TodoList{ Id = todo.Entity.Id}, _mapper.Map<TodoListResponse>(todo.Entity));
        }

        private bool TodoListExists(int id)
        {
            return _context.TodoList.Any(e => e.Id == id);
        }
    }
}
