using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyPortfolioWebApp.Models;

namespace MyPortfolioWebApp.Controllers
{
    public class BoardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BoardController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Board
        public async Task<IActionResult> Index(int page = 1, string search = "")
        {
            ViewData["Title"] = "게시판";

            var totalCount = _context.Board
                .Where(b => EF.Functions.Like(b.Title, $"%{search}%"))
                .Count();

            var countList = 10;
            var totalPage = totalCount / countList;

            if (totalCount % countList > 0) totalPage++;
            if (totalPage < page) page = totalPage;

            var countPage = 10;
            var startPage = ((page - 1) / countPage) * countPage + 1;
            var endPage = startPage + countPage - 1;

            if (totalPage < endPage) endPage = totalPage;

            var startCount = ((page - 1) * countPage) + 1;
            var endCount = startCount + countList - 1;

            ViewBag.StartPage = startPage;
            ViewBag.EndPage = endPage;
            ViewBag.Page = page;
            ViewBag.TotalPage = totalPage;
            ViewBag.Search = search;

            var boards = await _context.Board
            .FromSql($"CALL Board_PagingBoard({startCount}, {endCount}, {search})")
            .ToListAsync();
            return View(boards);
        }

        // GET: Board/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var board = await _context.Board.FirstOrDefaultAsync(m => m.Id == id);
            if (board == null) return NotFound();

            board.ReadCount++;
            _context.Board.Update(board);
            await _context.SaveChangesAsync();

            return View(board);
        }

        // GET: Board/Create
        public IActionResult Create()
        {
            var board = new Board
            {
                Writer = "익명",
                PostDate = DateTime.Now,
                ReadCount = 0,
            };
            return View(board);
        }

        // POST: Board/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Contents,Email")] Board board)
        {
            if (ModelState.IsValid)
            {
                board.Writer = "관리자";
                board.PostDate = DateTime.Now;
                board.ReadCount = 0;

                _context.Add(board);
                await _context.SaveChangesAsync();

                TempData["success"] = "게시글 작성 완료!";
                return RedirectToAction(nameof(Index));
            }
            return View(board);
        }

        // GET: Board/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var board = await _context.Board.FindAsync(id);
            if (board == null)
            {
                return NotFound();
            }
            return View(board);
        }

        // POST: Board/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Contents,Email")] Board board)
        {
            if (id != board.Id)
            { return NotFound(); }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingBoard = await _context.Board.FindAsync(id);
                    if (existingBoard == null) return NotFound();

                    existingBoard.Title = board.Title;
                    existingBoard.Contents = board.Contents;
                    existingBoard.Email = board.Email;

                    await _context.SaveChangesAsync();
                    TempData["success"] = "게시글 수정 완료!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BoardExists(board.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(board);
        }

        // GET: Board/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var board = await _context.Board.FirstOrDefaultAsync(m => m.Id == id);
            if (board == null) return NotFound();

            return View(board);
        }

        // POST: Board/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var board = await _context.Board.FindAsync(id);
            if (board != null)
            {
                _context.Board.Remove(board);
            }
            await _context.SaveChangesAsync();
            TempData["success"] = "게시글 삭제 완료!";
            return RedirectToAction(nameof(Index));
        }

        private bool BoardExists(int id)
        {
            return _context.Board.Any(e => e.Id == id);
        }
    }
}
