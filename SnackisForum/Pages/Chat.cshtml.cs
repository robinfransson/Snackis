using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using SnackisDB.Models;
using SnackisDB;

namespace SnackisForum.Pages
{
    public class ChatModel : PageModel
    {
        private readonly SnackisContext _context;
        private readonly ILogger<ChatModel> _logger;
        private readonly Random rand = new();
        public ChatModel(SnackisContext context, ILogger<ChatModel> logger)
        {
            _context = context;
            _logger = logger;
        }
        public void OnGet()
        {
        }


    }
}
