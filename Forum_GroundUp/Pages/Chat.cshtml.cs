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


        public async Task<JsonResult> OnGetLoadMessages([FromServices] HttpClient client)
        {
            //var message = _context.Messages.Where(message => message.HasBeenViewed == false).FirstOrDefault();
            //if(message == null)
            //{
            //    return null;
            //}
            //message.HasBeenViewed = true;
            //int rowsChanged = await _context.SaveChangesAsync();
            //_logger.LogDebug("Rows changed = {0}", rowsChanged);
            List<string> words = await client.GetFromJsonAsync<List<string>>("https://random-word-api.herokuapp.com/word?number=" + rand.Next(6, 20));
            string sentance = string.Join(" ", words);
            bool reciever = rand.Next(1, 101) < 50;
            var returnValue = new { reciever = reciever, message = sentance };
            return new JsonResult(returnValue);

        }
    }
}
