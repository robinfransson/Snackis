using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using SnackisDB.Models;
using SnackisDB.Models.Identity;

namespace SnackisForum.Pages.Admin
{
    public class ReportsModel : PageModel
    {
        private readonly SnackisContext _context;
        private readonly ILogger<ReportsModel> _logger;



        public ReportsModel(SnackisContext context, ILogger<ReportsModel> logger)
        {
            _context = context;
            _logger = logger;
        }


        public List<Report> Reports { get; set; }


        public void OnGet()
        {
            Reports = _context.Reports.OrderBy(report => !report.ActionTaken).ToList();
        }
    }
}
