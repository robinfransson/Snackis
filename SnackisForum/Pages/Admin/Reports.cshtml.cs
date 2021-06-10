using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SnackisDB.Models;
using SnackisDB.Models.Identity;
using SnackisForum.Injects;

namespace SnackisForum.Pages.Admin
{
    public class ReportsModel : PageModel
    {
        private readonly SnackisContext _context;
        private readonly ILogger<ReportsModel> _logger;
        private readonly UserProfile _profile;



        public ReportsModel(SnackisContext context, ILogger<ReportsModel> logger, UserProfile userProfile)
        {
            _context = context;
            _logger = logger;
            _profile = userProfile;
        }


        public List<Report> Reports { get; set; }
        public int Users { get; set; }


        public IActionResult OnGet()
        {
            if(_profile.IsAdmin)
            {

                Reports = _context.Reports.Include(report => report.Reporter)
                                          .Include(report => report.ReportedReply)
                                            .ThenInclude(reply => reply.Author)
                                          .Include(report => report.ReportedThread)
                                            .ThenInclude(thread => thread.CreatedBy)
                                          .OrderByDescending(report => report.DateReported)
                                          .OrderByDescending(report => !report.ActionTaken).ToList();
                Users = _context.Users.Count();

                return Page();
            }
            return RedirectToPage("../Index");
        }


        public async Task<IActionResult> OnPostTakeActionAsync(string type, int id, bool remove, int reportID)
        {
            if (_profile.IsAdmin)
            {
                object reported = type == "thread" ? await _context.Threads.FirstOrDefaultAsync(thread => thread.ID == id) : await _context.Replies.FirstOrDefaultAsync(thread => thread.ID == id);
                if(reported is not null)
                {
                    if(remove)
                    {
                        UpdateForumContent(reported);
                    }


                    var report = await _context.Reports.FirstOrDefaultAsync(report => report.ID == reportID);
                    report.ActionTaken = true;
                    report.Removed = remove;
                    int rowsChanged = await _context.SaveChangesAsync();
                    _logger.LogInformation($"Report {reportID} updated!");

                }
                else
                {
                    _logger.LogInformation("no changes made, reported content was not found");
                }

            }
            return RedirectToPage();
        }


        public void UpdateForumContent(object obj)
        {
            switch (obj)
            {
                case ForumThread thread:
                    thread.Body = "Borttagen";
                    thread.Title = "Borttagen";
                    _logger.LogInformation($"Updated thread {thread.Title} to removed");
                    break;
                case ForumReply reply:
                    reply.Title = "Borttagen";
                    reply.Body = "Borttagen";
                    _logger.LogInformation($"Updated thread {reply.Title} to removed");
                    break;
                default:
                    throw new NotImplementedException("Type " + obj.GetType().ToString() + " is not supported");
            }
        }
    }
}
