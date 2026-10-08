using ComputerShopAPI.Models;
using ComputerShopAPI.Models.DTOS;
using Microsoft.AspNetCore.Mvc;

namespace ComputerShopAPI.Controllers
{
    [Route("osystem")]
    [ApiController]
    public class OSystemController : ControllerBase
    {


        public ComputerShopDBContext _context = new();
        [HttpPost]
        public object AddnewOsystem([FromBody] AddNewOsystemDTO addnewosystemDTO)
        {
            var osystem = new OSystem
            {
                Id = Guid.NewGuid(),
                Name = addnewosystemDTO.Name,
                Version = addnewosystemDTO.Version,
                RegisterTime = DateTime.Now,
                UpdateTime = DateTime.Now
            };

            if (osystem != null)
            {
                _context.OSystems.Add(osystem);
                _context.SaveChanges();

                return StatusCode(201, new
                {
                    message = "New OSystem added successfully",
                    result = addnewosystemDTO

                });
            }
            return StatusCode(404, new { message = "Failed to add new OSystem", result = addnewosystemDTO });
        }

        [HttpGet]
        public object GetAllOSystems()
        {
            var osystems = _context.OSystems.ToList();
            if (osystems != null)
            {
                return StatusCode(200, new
                {
                    message = "All OSystems retrieved successfully",
                    result = osystems
                });
            }
            return StatusCode(404, new { message = "Failed to retrieve OSystems", result = osystems });
        }

        [HttpDelete]
        public object DeleteOsystem([FromQuery] Guid id)
        {
            var osystem = _context.OSystems.FirstOrDefault(o => o.Id == id);
            if (osystem != null)
            {
                _context.OSystems.Remove(osystem);
                _context.SaveChanges();
                return StatusCode(200, new { message = "OSystem deleted successfully", result = osystem });
            }
            return StatusCode(404, new { message = "Failed to delete OSystem", result = osystem });
        }

        [HttpPut]
        public object UpdateOsystem([FromQuery] Guid id, [FromBody] AddNewOsystemDTO updateosystemDTO)
        {
            var osystem = _context.OSystems.Find(id);

            if(osystem != null)
            {
                osystem.Name = updateosystemDTO.Name;
                osystem.Version = updateosystemDTO.Version;
                osystem.UpdateTime = DateTime.Now;

                _context.OSystems.Update(osystem);
                _context.SaveChanges();
                return StatusCode(200, new
                {
                    message = "OSystem updated successfully",
                    result = osystem
                });
            }
            return StatusCode(404, new { message = "Failed to update OSystem", result = osystem });
        }
    }
}
