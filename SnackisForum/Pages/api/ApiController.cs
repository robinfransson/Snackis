using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SnackisDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SnackisForum.Pages.api
{
    [Route("api/")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly SnackisContext _context;

        public ApiController(SnackisContext context) => _context = context;


        [HttpGet("trad/{id}")]
        public async Task<IActionResult> GetThreadAsync(int id)
        {
            if (!_context.Threads.Any(thread => thread.ID == id))
            {
                return Content($"Ingen tråd med ID {id} hittad.");
            }
            var dbquery = await _context.Threads.Where(thread => thread.ID == id)
                                         .Include(thread => thread.Replies)
                                            .ThenInclude(reply => reply.Author)
                                         .Include(thread => thread.CreatedBy)
                                         .Include(thread => thread.Replies)
                                            .ThenInclude(reply => reply.RepliedComment)
                                         .ToListAsync();


            var thread = dbquery.Select(thread => new
            {
                titel = thread.Title,
                tradstart = thread.Body,
                skapare = thread.CreatedBy.UserName,
                skapad = thread.CreatedOn.ToString("dd/MM/yy HH:mm"),
                svar = thread.Replies.OrderBy(reply => reply.DatePosted).Select((reply, index) => new
                {
                    svar_nr = index + 1,
                    svar_till = reply.RepliedComment == null ? "trådstart" : "inlägg",
                    anvandare = reply.Author.UserName,
                    datum = reply.DatePosted.ToString("dd/MM/yy HH:mm"),
                    titel = reply.Title,
                    text = reply.Body

                }).ToList()
            }).ToList();
            return Ok(thread);
        }

        [HttpGet("forum/all")]
        public async Task<IActionResult> GetAllAsync()
        {
            if (!_context.Forums.Any())
            {
                return Content("Det finns ingen data att hämta för tillfället.");
            }
            var all = await _context.Forums.Include(forum => forum.Subforums)
                                                   .ThenInclude(sub => sub.Threads)
                                                       .ThenInclude(thread => thread.Replies)
                                                           .ThenInclude(reply => reply.Author)
                                               .Include(forum => forum.Subforums)
                                                   .ThenInclude(sub => sub.Threads)
                                                       .ThenInclude(thread => thread.CreatedBy)
                                               .Include(forum => forum.Subforums)
                                                   .ThenInclude(sub => sub.Threads)
                                                       .ThenInclude(thread => thread.Replies)
                                                           .ThenInclude(reply => reply.RepliedComment)
                                               .Include(forum => forum.Subforums)
                                                   .ThenInclude(sub => sub.Threads)
                                                       .ThenInclude(thread => thread.Replies)
                                                           .ThenInclude(reply => reply.Author)
                                               .ToListAsync();


            var allInfo = all.Select(forum => new
            {
                forumnamn = forum.Name,
                subforum = forum.Subforums.Select(sub => new
                {
                    sub_namn = sub.Name,
                    tradar = sub.Threads.Select(thread => new
                    {
                        titel = thread.Title,
                        tradstart = thread.Body,
                        skapare = thread.CreatedBy.UserName,
                        skapad = thread.CreatedOn.ToString("dd/MM/yy HH:mm"),
                        svar = thread.Replies.OrderBy(reply => reply.DatePosted).Select((reply, index) => new
                        {
                            svar_nr = index + 1,
                            svar_till = reply.RepliedComment == null ? "trådstart" : "inlägg",
                            anvandare = reply.Author == null ? "Anonym" : reply.Author.UserName,
                            datum = reply.DatePosted.ToString("dd/MM/yy HH:mm"),
                            titel = reply.Title,
                            text = reply.Body

                        }).ToList()
                    })
                }).ToList()
            }).ToList();

            return Ok(allInfo);
        }
        [HttpGet("sub/{name}")]
        public async Task<IActionResult> GetAllSubforumAsync(string name)
        {
            if (!_context.Subforums.Any(sub => sub.Name.ToLower() == name.ToLower()))
            {
                return Content("Det finns ingen data att hämta för tillfället.");
            }
            var all = await _context.Subforums.Where(sub => sub.Name.ToLower() == name.ToLower())
                                              .Include(sub => sub.Threads)
                                                  .ThenInclude(thread => thread.Replies)
                                                      .ThenInclude(reply => reply.Author)
                                              .Include(sub => sub.Threads)
                                                  .ThenInclude(thread => thread.CreatedBy)
                                              .Include(sub => sub.Threads)
                                                  .ThenInclude(thread => thread.Replies)
                                                       .ThenInclude(reply => reply.RepliedComment)
                                              .Include(sub => sub.Threads)
                                                  .ThenInclude(thread => thread.Replies)
                                                      .ThenInclude(reply => reply.Author)
                                              .ToListAsync();


            var allInfo = all.Select(sub => new
            {
                sub_namn = sub.Name,
                tradar = sub.Threads.Select(thread => new
                {
                    titel = thread.Title,
                    tradstart = thread.Body,
                    skapare = thread.CreatedBy.UserName,
                    skapad = thread.CreatedOn.ToString("dd/MM/yy HH:mm"),
                    svar = thread.Replies.OrderBy(reply => reply.DatePosted).Select((reply, index) => new
                    {
                        svar_nr = index + 1,
                        svar_till = reply.RepliedComment == null ? "trådstart" : "inlägg",
                        anvandare = reply.Author == null ? "Anonym" : reply.Author.UserName,
                        datum = reply.DatePosted.ToString("dd/MM/yy HH:mm"),
                        titel = reply.Title,
                        text = reply.Body

                    }).ToList()
                })
            }).ToList();

            return Ok(allInfo);
        }

        [HttpGet("forum/{name}")]
        public async Task<IActionResult> GetForumAsync(string name)
        {

            var query = await _context.Forums.Where(forum => forum.Name.ToLower() == name.ToLower())
                                             .Include(forum => forum.Subforums)
                                                  .ThenInclude(sub => sub.Threads)
                                                      .ThenInclude(thread => thread.Replies)
                                                          .ThenInclude(reply => reply.Author)
                                             .Include(forum => forum.Subforums)
                                                 .ThenInclude(sub => sub.Threads)
                                                     .ThenInclude(thread => thread.CreatedBy)
                                             .Include(forum => forum.Subforums)
                                                 .ThenInclude(sub => sub.Threads)
                                                     .ThenInclude(thread => thread.Replies)
                                                         .ThenInclude(reply => reply.RepliedComment)
                                             .Include(forum => forum.Subforums)
                                                 .ThenInclude(sub => sub.Threads)
                                                     .ThenInclude(thread => thread.Replies)
                                                         .ThenInclude(reply => reply.Author)
                                             .ToListAsync();
            if (!query.Any() || !query.Any(forum => forum.Subforums.Any()))
            {
                return Content("Det finns ingen data att hämta för tillfället.");
            }


            var allInfo = query.Select(forum => new
            {
                forumnamn = forum.Name,
                subforum = forum.Subforums.Select(sub => new
                {
                    sub_namn = sub.Name,
                    tradar = sub.Threads.Select(thread => new
                    {
                        titel = thread.Title,
                        tradstart = thread.Body,
                        skapare = thread.CreatedBy.UserName,
                        skapad = thread.CreatedOn.ToString("dd/MM/yy HH:mm"),
                        svar = thread.Replies.OrderBy(reply => reply.DatePosted).Select((reply, index) => new
                        {
                            svar_nr = index + 1,
                            svar_id = reply.ID,
                            svar_till = reply.RepliedComment == null ? "trådstart" : "id=" + reply.RepliedComment.ID,
                            anvandare = reply.Author == null ? "Anonym" : reply.Author.UserName,
                            datum = reply.DatePosted.ToString("dd/MM/yy HH:mm"),
                            titel = reply.Title,
                            text = reply.Body

                        }).ToList()
                    })
                }).ToList()
            }).ToList();

            return Ok(allInfo);
        }

    }
}
