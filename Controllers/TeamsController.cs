using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NBA_App.Data;
using NBA_App.Models;

namespace NBA_App.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeamsController : ControllerBase
    {
        private readonly DataContext _context;

        public TeamsController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Team>>> GetTeams()
        {
            return Ok(await _context.Teams.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List<Team>>> GetTeam(int id)
        {
            var team = await _context.Teams.FindAsync(id);
            if (team == null)
                return NotFound("No team here. :/");

            return Ok(team);
        }

        [HttpPost]
        public async Task<ActionResult<List<Team>>> CreateTeam(Team team)
        {
            _context.Teams.Add(team);
            await _context.SaveChangesAsync();
            return Ok(await _context.Teams.ToListAsync());
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<List<Team>>> UpdateTeam(Team team, int id)
        {
            var dbTeam = await _context.Teams.FindAsync(id);
            if (dbTeam == null)
                return NotFound("No team here. :/");

            dbTeam.Name = team.Name;

            await _context.SaveChangesAsync();
            return Ok(await _context.Teams.ToListAsync());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<Team>>> DeleteTeam(int id)
        {
            var dbTeam = await _context.Teams.FindAsync(id);
            if (dbTeam == null)
                return NotFound("No team here. :/");

            _context.Teams.Remove(dbTeam);
            await _context.SaveChangesAsync();

            return Ok(await _context.Teams.ToListAsync());
        }
    }
}