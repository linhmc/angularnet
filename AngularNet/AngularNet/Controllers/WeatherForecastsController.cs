using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AngularNet.Models;

namespace AngularNet.Controllers
{
    [Produces("application/json")]
    [Route("api/WeatherForecasts")]
    public class WeatherForecastsController : Controller
    {
        private readonly SampleDataContext _context;

        public WeatherForecastsController(SampleDataContext context)
        {
            _context = context;
        }

        // GET: api/WeatherForecasts
        [HttpGet]
        public IEnumerable<WeatherForecast> GetWeatherForecast()
        {
            return _context.WeatherForecast;
        }

        // GET: api/WeatherForecasts/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetWeatherForecast([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var weatherForecast = await _context.WeatherForecast.SingleOrDefaultAsync(m => m.DateFormatted == id);

            if (weatherForecast == null)
            {
                return NotFound();
            }

            return Ok(weatherForecast);
        }

        // PUT: api/WeatherForecasts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWeatherForecast([FromRoute] string id, [FromBody] WeatherForecast weatherForecast)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != weatherForecast.DateFormatted)
            {
                return BadRequest();
            }

            _context.Entry(weatherForecast).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WeatherForecastExists(id))
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

        // POST: api/WeatherForecasts
        [HttpPost]
        public async Task<IActionResult> PostWeatherForecast([FromBody] WeatherForecast weatherForecast)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.WeatherForecast.Add(weatherForecast);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (WeatherForecastExists(weatherForecast.DateFormatted))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetWeatherForecast", new { id = weatherForecast.DateFormatted }, weatherForecast);
        }

        // DELETE: api/WeatherForecasts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWeatherForecast([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var weatherForecast = await _context.WeatherForecast.SingleOrDefaultAsync(m => m.DateFormatted == id);
            if (weatherForecast == null)
            {
                return NotFound();
            }

            _context.WeatherForecast.Remove(weatherForecast);
            await _context.SaveChangesAsync();

            return Ok(weatherForecast);
        }

        private bool WeatherForecastExists(string id)
        {
            return _context.WeatherForecast.Any(e => e.DateFormatted == id);
        }
    }
}