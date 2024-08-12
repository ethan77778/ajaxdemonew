using ajaxdemonew.Models;
using ajaxdemonew.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.EntityFrameworkCore.Migrations;
using System.Text;

namespace ajaxdemonew.Controllers
{
    public class ApiController : Controller
    {
        MyDBContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ApiController(MyDBContext context, IWebHostEnvironment environment)
        {
      
            _context = context;
            _webHostEnvironment = environment;
        }

        public IActionResult CheckAccountAction(UserDTO _user)
        {

            var exists = _context.Members.Any(x => x.Name == _user.userName);//Any 方法用檢查是否存在至少一个滿足條件的紀錄


            if (exists) 
            { 
               string name = $"{_user.userName} 已經存在";
                  return Content(name, "text/plain", Encoding.UTF8);

            }
            else 
            {          
                 string success = $"{_user.userName} 無重複可以使用";
               return Content(success,"text/plain",Encoding.UTF8);
            }
             
            //return Content(info,"text/plain",System.Text.Encoding.UTF8);          
      
        }
        public IActionResult Register(UserDTO _user)
        {
            if (string.IsNullOrEmpty(_user.userName))
            {
                _user.userName = "guset";
            }
            //string info=$"{_user.userPhoto.FileName}-{_user.userPhoto.Length}-{_user.userPhoto.ContentType}";
           

            //檔案上傳
            string filePath=Path.Combine(_webHostEnvironment.WebRootPath,"uploads",_user.userPhoto.FileName);
            using (var fileStream=new FileStream(filePath, FileMode.Create))
            {
                _user.userPhoto.CopyTo(fileStream);
            }
            //return Content(info,"text/plain",System.Text.Encoding.UTF8);
            Member member = new Member();            
            member.Name = _user.userName;
            member.Email = _user.userEmail;
            member.Password = _user.userpassword.ToString();
            member.Age = _user.userAge;
            member.FileName = _user.userPhoto.FileName;
            byte[] imgByte = null;
            using (var memorySteam = new MemoryStream())
            {
                _user.userPhoto.CopyTo(memorySteam);
                imgByte = memorySteam.ToArray();
            }
            member.FileData = imgByte;
            _context.Members.Add(member);
            _context.SaveChanges();


            return Content(filePath);
        }
        public IActionResult Spots([FromBody] SearchDTO searchDTO)
        {
            var spots = searchDTO.categoryid == 0 ? _context.SpotImagesSpots :
                _context.SpotImagesSpots.Where(s => s.CategoryId == searchDTO.categoryid);

            //關鍵字搜尋
            if (!string.IsNullOrEmpty(searchDTO.keyword))
            {
                spots = spots.Where(s => s.SpotTitle.Contains(searchDTO.keyword) ||
                s.SpotDescription.Contains(searchDTO.keyword));
            }
            //排序
            switch (searchDTO.sortBy)
            {
                case "spotTitle":
                    spots = searchDTO.sortType == "asc" ? spots.OrderBy(s => s.SpotTitle) : spots.OrderByDescending(s => s.SpotTitle);
                    break;
                case "CategoryId":
                    spots = searchDTO.sortType == "asc" ? spots.OrderBy(s => s.CategoryId) : spots.OrderByDescending(s => s.CategoryId);
                    break;
                default:
                    spots = searchDTO.sortType == "asc" ? spots.OrderBy(s => s.SpotId) : spots.OrderByDescending(s => s.SpotId);
                    break;
            }
            //總共有多少筆
            int TotalCount = spots.Count();
            //設定每頁顯示幾筆資料
            int pageSize = searchDTO.pageSize ?? 9;
            //目前要顯示第幾頁
            int page = searchDTO.page ?? 1;
            //計算總共有幾頁
            int TotalPages = (int)Math.Ceiling((decimal)TotalCount / pageSize);
            //設定分頁
            spots = spots.Skip((int)(page - 1) * pageSize).Take(pageSize);

            SpotsPagingDTO spotsPaging = new SpotsPagingDTO();
            spotsPaging.TotalPages = TotalPages;
            spotsPaging.TotalCount = TotalCount;
            spotsPaging.SpotsResult = spots.ToList();
            return Json(spotsPaging);
        }
    }
}
