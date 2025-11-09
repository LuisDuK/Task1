using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QLHangHoa.Models;

namespace QLHangHoa.Controllers
{
    public class PhieuNhapKhoController : Controller
    {
        private readonly QLThuocContext _context;

        public PhieuNhapKhoController(QLThuocContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();

        }
    }
}
