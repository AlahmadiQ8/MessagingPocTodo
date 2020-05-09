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

namespace PublisherDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoListController : ControllerBase
    {
        private readonly TodoContext _context;
        private readonly IMapper _mapper;
        private IMessagingService<SampleMessage> _messagingService;

        public TodoListController(TodoContext context, IMapper mapper, IMessagingService<SampleMessage> messagingService)
        {
            _context = context;
            _mapper = mapper;
            _messagingService = messagingService;
        }

        // GET: api/TodoList
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoListResponse>>> GetTodoList()
        {
            _messagingService.Publish(new SampleMessage());
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

        // PUT: api/TodoList/5/TodoItem
        [HttpPut("{id}/TodoItem")]
        public async Task<IActionResult> PutTodoList(int id, TodoItemRequest todoItem)
        {
            var list = await _context.TodoList.FirstOrDefaultAsync(l => l.Id == id);
            if (list == null)
            {
                return NotFound();
            }
            
            list.TodoItems.Add(new TodoItem
            {
                Description = todoItem.Description,
                DueDate = todoItem.DueDate,
                Done = todoItem.Done,
            });
            
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
