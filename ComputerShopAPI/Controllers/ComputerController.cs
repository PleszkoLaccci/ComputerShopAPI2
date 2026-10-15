using ComputerShopAPI.Models;
using ComputerShopAPI.Models.DTOS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ComputerShopAPI.Controllers
{
    [Route("computers")]
    [ApiController]
    public class ComputerController : ControllerBase
    {
        public ComputerShopDBContext _context = new();
        [HttpGet("getAllComputers")]
        public object GetAllCompiuter()
        {
            var computers = _context.Comps.ToList();

            if (computers != null)
            {
                return StatusCode(200, new
                {
                    message = "Itt van az osszes",
                    result = computers
                });
            }
            return StatusCode(404, new
            {
                message = "Hat ez most nem sikerult"
            });
        }

        [HttpGet("getComputerById")]
        public object GetComputerById([FromQuery] Guid id)
        {
            var computer = _context.Comps.Where(c => c.Id == id).FirstOrDefault();
            if (computer != null)
            {
                return StatusCode(200, new
                {
                    message = "Itt van dik",
                    result = computer
                });
            }
            return StatusCode(404, new
            {
                message = "Nincs meg dik"
            });
        }
        [HttpPost("createComputer")]
        public object PostComputer([FromBody] AddNewCompDTO addnewcomputer)
        {
            var newComputer = new Comps
            {
                Id = Guid.NewGuid(),
                Brand = addnewcomputer.Brand,
                Type = addnewcomputer.Type,
                Display = addnewcomputer.Display,
                Memory = addnewcomputer.Memory,
                CreatedTime = DateTime.Now,
                OsId = addnewcomputer.OsId
            };

            if (newComputer != null)
            {
                _context.Comps.Add(newComputer);
                _context.SaveChanges();
                return StatusCode(201, new
                {
                    message = "Fasza",
                    result = newComputer
                });

            }
            else
            {
                return StatusCode(400, new
                {
                    message = "Nem fasza valami"
                });
            }
        }

        [HttpPut("updateComputer")]
        public object UpdateComputer([FromQuery] Guid id, UpdateCompDTO updatecomdto)
        {
            var computer = _context.Comps.FirstOrDefault(c => c.Id == id);

            if(computer != null)
            {
                computer.Brand = updatecomdto.Brand;
                computer.Type = updatecomdto.Type;
                computer.Display = updatecomdto.Display;
                computer.Memory = updatecomdto.Memory;
                computer.OsId = updatecomdto.OsId;

                _context.Comps.Update(computer);
                _context.SaveChanges();

                return StatusCode(200, new
                {
                    message = "Sikeres frissites",
                    result = computer
                });
            }
            else
            {
                return StatusCode(404, new
                {
                    message = "Nincs ilyen gep"
                });
            }
        }

        [HttpDelete("deleteComputer")]
        public object DeleteComputer([FromQuery] Guid id, DeleteCompDTO deletecompdto)
        {
            var computer = _context.Comps.FirstOrDefault(c => c.Id == id);

            if(computer != null)
            {
                _context.Comps.Remove(computer);
                _context.SaveChanges();
                return StatusCode(200, new
                {
                    message = "Sikeres törlés",
                    result = computer
                });
            }
            else
            {
                return StatusCode(404, new
                {
                    message = "Nincs ilyen gep"
                });
            }
        }

        [HttpGet("allOsWithComputers")]
        public object GetAllOsWithComputers()
        {
            var oswithcomps = _context.OSystems.Include(o => o.Comps);

            return StatusCode(200, new
            {
                message = "Itt van az osszes OS es gep",
                result = oswithcomps
            });
        }

        [HttpGet("getComputerWithOs")]
        public object GetComputerWithOs()
        {
            var computerwithos = _context.Comps
            .Join(_context.OSystems,
            c => c.OsId,
            o => o.Id,
            (c, o) => new { c, o.Name, o.Version });
            return StatusCode(200, new
            {
                message = "Itt van a gep es az OS",
                result = computerwithos
            });
        }
    }
}
