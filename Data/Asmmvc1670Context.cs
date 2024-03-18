using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Asmmvc1670.Models;
using System.Collections;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Asmmvc1670.Data
{
    public class Asmmvc1670Context : DbContext
    {
        public Asmmvc1670Context(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Asmmvc1670.Models.Product> Product { get; set; } = default!;

        public DbSet<Asmmvc1670.Models.Category> Category { get; set; } = default!;
        public IEnumerable Categories { get; internal set; }
        public DbSet<Cart> Cart { get; set; }
    }
}
