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

        [HttpGet("/api/query/os/{id}")]
        public object OsystemWithComputers(Guid id)
        {
            var os = _context.OSystems.FirstOrDefault(o => o.Id == id);
            
            if(os == null)
            {
                return StatusCode(404, new
                {   
                    message = "Valami nem fasza"
                });
            }

            var computers = _context.Comps
                .Where(c => c.OsId == id)
                .Select(c => new { c.Brand, c.Type })
                .ToList();

            return StatusCode(200, new
            {
                message = "Fasza",
                os = os,
                result = computers
            });
        }


        [HttpGet("api/query/computer/{id}")]
        public object CompWithOs(Guid id)
        {
            var compswithos = _context.Comps
                .Where(c => c.Id == id)
                .Select(c => new
                {
                    c.Brand,
                    c.Type,
                    c.Display,
                    c.Memory,
                    c.CreatedTime,
                    c.OsId,
                    osname = c.Os.Name
                })
                .FirstOrDefault();

            if(compswithos == null)
            {
                return StatusCode(404, new
                {
                    message = "nemjo"
                });
            }
            return StatusCode(200, new
            {
                message = "fasza",
                result = compswithos
            });
        }

        [HttpGet("api/query/os-computer-count")]
        public object OsWithCompCount()
        {
            var oswithcompcount = _context.OSystems
                .Select(o => new
                {
                    o.Id,
                    o.Name,
                    o.Version,
                    compcount = o.Comps.Count
                })
                .ToList();

            return StatusCode(200, new
            {
                message = "Fasza",
                result = oswithcompcount
            });
        }

        [HttpGet("api/query/brand/{brand}")]    
        public object CompBrandWithOs(string brand)
        {
            var compbrandwithos = _context.Comps
                .Where(c => c.Brand == brand)
                .Select(c => new
                {
                    c.Brand,
                    c.Type,
                    c.Display,
                    c.Memory,
                    c.CreatedTime,
                    c.OsId,
                    osname = c.Os.Name
                });

            return StatusCode(200, new
            {
                message = "fasza",
                result = compbrandwithos
            });
        }

        [HttpGet("api/query/computers-sorted")]
        public object EveryCompWithOsBrandSorted()
        {
            var sorted = _context.Comps
                .OrderBy(c => c.Brand)
                .ThenBy(c => c.Type)
                .Select(c => new
                {
                    c.Brand,
                    c.Type,
                    c.Display,
                    c.Memory,
                    c.CreatedTime,
                    c.OsId,
                    osname = c.Os.Name
                })
                .ToList();

            return StatusCode(200, new
            {
                message = "fasza",
                result = sorted
            });
        }

        [HttpGet("api/query/min-display/{size}")]
        public object CompByDisplayWithOs(double display)
        {
            var compbydisplay = _context.Comps
                .Where(c => c.Display > display)
                .Select(c => new
                {
                    c.Brand,
                    c.Type,
                    c.Display,
                    osname = c.Os.Name
                });

            return StatusCode(200, new
            {
                message = "fasza",
                result = compbydisplay
            });
        }


        [HttpGet("os-stats")]
        public object OsStats()
        {
            var os = _context.OSystems
                .Select(o => new
                {
                    o.Id,
                    o.Name,
                    o.Version,
                    Compcount = o.Comps.Count,
                    avgmemory = o.Comps.Average(c => (double?)c.Memory),
                    maxmemory = o.Comps.Max(c => (int?)c.Memory),
                    avgdisplay = o.Comps.Average(c => (double?)c.Display)
                })
                .ToList();

            return StatusCode(200, new
            {
                message = "fasza",
                result = os
            });
        }

        [HttpGet("by-os")]
        public object CompByOsName()
        {
            var compbyos = _context.Comps
                .OrderBy(c => c.Os.Name)
                .Select(c => new
                {
                    c.Brand,
                    c.Type,
                    c.Display,
                    c.Memory,
                    c.CreatedTime,
                    c.Os.Name,
                    c.OsId
                })
                .ToList();

            return StatusCode(200, new
            {
                message = "fazsa",
                result = compbyos
            });
        }

        [HttpGet("OsWithoutComp")]
        public object OsWithoutComp()
        {
            var oswithout = _context.OSystems
                .Where(o => !o.Comps.Any())
                .Select(o => new
                {
                    o.Id,
                    o.Name,
                    o.Version
                })
                .ToList();
            return StatusCode(200, new
            {
                message = "Fasza",
                result = oswithout
            });
        }

        [HttpGet("BiggestMemory")]
        public object BiggestMemoryByOs()
        {
            var biggest = _context.OSystems
                .Select(o => new
                {
                    o.Id,
                    Osname = o.Name,
                    o.Version,
                    Computer = o.Comps
                        .OrderByDescending(c => c.Memory)
                        .Select(c => new
                        {
                            c.Id,
                            c.Brand,
                            c.Type,
                            c.Display,
                            c.Memory
                        })
                        .FirstOrDefault()
                });

            return StatusCode(200, new
            {
                message = "fasza",
                result = biggest
            });
        } 
    }
}
