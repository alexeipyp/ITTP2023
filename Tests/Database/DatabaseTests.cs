using DAL;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Database
{
    [TestClass]
    public class DatabaseTests
    {
        private DataContext _context = null!;

        [TestInitialize]
        public void TestInitialize()
        {
            var dbContextOptions = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase("DbTest").Options;
            _context = new DataContext(dbContextOptions);
            _context.Database.EnsureCreated();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [TestMethod]
        public async Task InitDatabaseWithAdminCreated_Success()
        {
            bool isAdminCreated = await _context.Users.AnyAsync(x => x.Login == "Admin");
            Assert.IsTrue(isAdminCreated);
        }
    }
}
