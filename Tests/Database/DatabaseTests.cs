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
        private SqliteConnection sqliteConnection = null!;

        [TestInitialize]
        public void TestInitialize()
        {
            sqliteConnection = new SqliteConnection("DataSource=:memory:");
            sqliteConnection.Open();

            var dbContextOptions = new DbContextOptionsBuilder<DataContext>()
                .UseSqlite(sqliteConnection).Options;
            _context = new DataContext(dbContextOptions);
            _context.Database.EnsureCreated();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _context.Database.EnsureDeleted();
            sqliteConnection.Close();
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
