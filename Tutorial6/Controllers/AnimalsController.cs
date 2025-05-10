using Microsoft.AspNetCore.Mvc;
using SchroniskoAPI.Models;

namespace SchroniskoAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnimalsController : ControllerBase
    {
        private static List<Animal> animals = new();
        private static int nextAnimalId = 1;
        private static int nextVisitId = 1;

        // GET: api/animals
        [HttpGet]
        public ActionResult<List<Animal>> GetAll()
        {
            return Ok(animals);
        }

        // GET: api/animals/{idOrName}
        // Za³o¿enie: imie zwierzecia nigdy nie jest liczba (calkowita)
        [HttpGet("{idOrName}")]
        public ActionResult<Animal> GetById(String idOrName)
        {
            var foundAnimals = new List<Animal>();

            try {
                foundAnimals = [.. from animal in animals where animal.Id == int.Parse(idOrName) select animal];
            } catch (FormatException)
            {
                foundAnimals = [animals.FirstOrDefault(a => a.Name == idOrName)];
            }

            if (foundAnimals.Count == 0)
                return NotFound();

            return Ok(foundAnimals);
        }

        // POST: api/animals
        [HttpPost]
        public ActionResult<Animal> Create(Animal animal)
        {
            animal.Id = nextAnimalId++;
            animals.Add(animal);
            return CreatedAtAction(nameof(GetById), new { id = animal.Id }, animal);
        }

        // PUT: api/animals/{id}
        [HttpPut("{id}")]
        public ActionResult Update(int id, Animal updatedAnimal)
        {
            var animal = animals.FirstOrDefault(a => a.Id == id);
            if (animal == null)
                return NotFound();

            animal.Name = updatedAnimal.Name;
            animal.Category = updatedAnimal.Category;
            animal.Weight = updatedAnimal.Weight;
            animal.FurColor = updatedAnimal.FurColor;

            return NoContent();
        }

        // DELETE: api/animals/{id}
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var animal = animals.FirstOrDefault(a => a.Id == id);
            if (animal == null)
                return NotFound();

            animals.Remove(animal);
            return NoContent();
        }

        // GET: api/animals/{animalId}/visits
        [HttpGet("{animalId}/visits")]
        public ActionResult<List<Visit>> GetVisits(int animalId)
        {
            var animal = animals.FirstOrDefault(a => a.Id == animalId);
            if (animal == null)
                return NotFound();

            return Ok(animal.Visits);
        }

        // POST: api/animals/{animalId}/visits
        [HttpPost("{animalId}/visits")]
        public ActionResult<Visit> AddVisit(int animalId, Visit visit)
        {
            var animal = animals.FirstOrDefault(a => a.Id == animalId);
            if (animal == null)
                return NotFound();

            visit.Id = nextVisitId++;
            animal.Visits.Add(visit);

            return CreatedAtAction(nameof(GetVisits), new { animalId = animal.Id }, visit);
        }
    }
}
