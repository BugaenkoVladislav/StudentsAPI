using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentsAPI.Database;
using StudentsAPI.Database.Models;

namespace StudentsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private StudentsContext db;
        public StudentsController(StudentsContext db) 
        {
            this.db = db;
        }
        [HttpGet("GetAllStudents")]
        public IActionResult Get()
        {
            try
            {
                                   
                return Ok(db.Students);                                               
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("GetSubID")]
        public IActionResult GetSub(int subID)
        {
            try
            {
                return Ok(db.Students.Where(x=> x.IdSubject == subID).Select(student => new
                {
                    id = student.Id,    
                    name = student.Name,
                    surname = student.Surname,
                    patronymic = student.Patronymic,
                    dateExam = student.DateExam,
                    idSubject = student.IdSubject,
                    grade = student.Grade
                })); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message); 
            }
        }

        [HttpGet("GetStudentID")]
        public IActionResult GetStud(int studID)
        {
            try
            {
                return Ok(db.Students.Where(x => x.Id == studID).Select(student => new
                {
                    id = student.Id,
                    name = student.Name,
                    surname = student.Surname,
                    patronymic = student.Patronymic,
                    dateExam = student.DateExam,
                    idSubject = student.IdSubject,
                    grade = student.Grade
                }));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("GetStudentName")]
        public IActionResult GetStudName(string name,string surname,string patronymic)
        {
            try
            {
                return Ok(db.Students.Where(x => x.Name == name && x.Surname == surname && x.Patronymic == patronymic).Select(student => new
                {
                    id = student.Id,
                    name = student.Name,
                    surname = student.Surname,
                    patronymic = student.Patronymic,
                    dateExam = student.DateExam,
                    idSubject = student.IdSubject,
                    grade = student.Grade
                }));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost("AddStudent")]
        public IActionResult Post([FromBody] Student student)
        {
            try
            {
                db.Students.Add(student);
                db.SaveChanges();
                return Ok();
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("RemoveStudent")]
        public IActionResult Delete(int id)
        {
            try
            {
                Student? student = db.Students.FirstOrDefault(x => x.Id == id);
                if (student == null)
                {
                    return Conflict("UserWithThisIDNotFound");
                }
                else
                {
                    db.Students.Remove(student);
                    db.SaveChanges();
                    return Ok();
                }
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            
        }
    }
}
