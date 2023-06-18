using System;
using System.Collections.Generic;

namespace StudentsAPI.Database.Models;

public partial class Student
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public string Patronymic { get; set; } = null!;

    public DateTime DateExam { get; set; }

    public long IdSubject { get; set; }

    public int Grade { get; set; }

    public virtual Subject? IdSubjectNavigation { get; set; } = null!;
}
