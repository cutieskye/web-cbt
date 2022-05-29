﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebCbt_Backend.Data;
using WebCbt_Backend.Models;

namespace WebCbt_Backend.Controllers
{
    [ApiController]
    [Authorize]
    [EnableCors("AllOrigins")]
    [Route("[controller]")]
    public class EvaluationController : ControllerBase
    {
        private readonly WebCbtDbContext _context;

        public EvaluationController(WebCbtDbContext context)
        {
            _context = context;
        }

        // POST: /moodtests
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("~/moodtests")]
        public async Task<ActionResult<Evaluation>> PostEvaluation(Evaluation evaluation)
        {
            if (User.Identity?.IsAuthenticated != true || User.FindFirstValue("userId") != evaluation.UserId.ToString())
            {
                return Unauthorized();
            }

            if (_context.Evaluations == null)
            {
                return Problem("Entity set is null.");
            }

            _context.Evaluations.Add(evaluation);

            await _context.SaveChangesAsync();

            return Ok();
        }

        // GET: /evaluation/findByUserId?userId=5
        [HttpGet("findByUserId")]
        public async Task<ActionResult<IEnumerable<Evaluation>>> FindEvaluationsByUserId(int userId)
        {
            if (User.Identity?.IsAuthenticated != true)
            {
                return Unauthorized();
            }

            if (_context.Evaluations == null)
            {
                return NotFound();
            }

            return await _context.Evaluations.Where(x => x.UserId == userId).ToListAsync();
        }

        // GET: /evaluation/5
        [HttpGet("{evaluationId}")]
        public async Task<ActionResult<Evaluation>> FindEvaluationById(int evaluationId)
        {
            if (User.Identity?.IsAuthenticated != true)
            {
                return Unauthorized();
            }

            if (_context.Evaluations == null)
            {
                return NotFound();
            }

            var evaluation = await _context.Evaluations.FindAsync(evaluationId);

            if (evaluation == null)
            {
                return NotFound();
            }

            if (User.FindFirstValue("userId") != evaluation.UserId.ToString())
            {
                return Unauthorized();
            }

            return evaluation;
        }

        // PUT: /evaluation/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{evaluationId}")]
        public async Task<IActionResult> PutEvaluation(int evaluationId, Evaluation evaluation)
        {
            if (User.Identity?.IsAuthenticated != true || User.FindFirstValue("userId") != evaluation.UserId.ToString())
            {
                return Unauthorized();
            }

            if (evaluationId != evaluation.EvaluationId)
            {
                return BadRequest();
            }

            _context.Entry(evaluation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EvaluationExists(evaluationId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        private bool EvaluationExists(int id)
        {
            return (_context.Evaluations?.Any(e => e.EvaluationId == id)).GetValueOrDefault();
        }

        // DELETE: /evaluation/5
        [HttpDelete("{evaluationId}")]
        public async Task<IActionResult> DeleteEvaluation(int evaluationId)
        {
            if (User.Identity?.IsAuthenticated != true)
            {
                return Unauthorized();
            }

            if (_context.Evaluations == null)
            {
                return NotFound();
            }

            var evaluation = await _context.Evaluations.FindAsync(evaluationId);

            if (evaluation == null)
            {
                return NotFound();
            }

            if (User.FindFirstValue("userId") != evaluation.UserId.ToString())
            {
                return Unauthorized();
            }

            _context.Evaluations.Remove(evaluation);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
