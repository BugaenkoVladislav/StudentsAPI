using System;
using System.Collections.Generic;

namespace StudentsAPI.Database.Models;

public partial class Subject
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
}
