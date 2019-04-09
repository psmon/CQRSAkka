using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDDSample.Entity;
using DDDSample.Repository;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace DDDSampleTest.Repository
{

    //https://www.matheus.ro/2018/03/19/how-to-use-entityframework-core-in-memory-database-for-unit-testing/

    class UserRepositoryTest
    {
        UserRepository repository;

        [SetUp]
        public void Setup()
        {
            var dbOptions = new DbContextOptionsBuilder<UserRepository>()
                    .UseInMemoryDatabase(databaseName: "user")
                    .Options;
            repository = new UserRepository(dbOptions);
        }

        //https://www.learnentityframeworkcore.com/dbset/querying-data

        [Test]
        public void ReadWriteTest()
        {
            //repository.Users.Find
            User tryFindUser = repository.Users
                .Where(u => u.MyId == "psmon")
                .SingleOrDefault();
            Assert.AreEqual(null, tryFindUser);
            
            User user = new User()
            {
                MyId = "psmon",
                NickName = "sam",
                PassWord = "111",
                RegDate = new DateTime()
            };

            repository.Users.Add(user);
            repository.SaveChanges();

            tryFindUser = repository.Users
                .Where(u => u.MyId == "psmon")
                .Single();
            Assert.AreEqual("psmon", tryFindUser.MyId);
            
        }
    }
}
