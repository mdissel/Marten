using System;
using System.Linq;
using System.Threading.Tasks;
using Marten.Schema;
using Marten.Testing.Documents;
using Shouldly;
using StructureMap;
using Xunit;

namespace Marten.Testing
{
    public class query_by_interface_Tests
	{

		public interface IHasAddressID
		{
			string AddressID { get; set; }
		}
		public class Location : IHasAddressID
		{
			public Location() {
				Id = Guid.NewGuid();
			}

			public Guid Id { get; set; }
			public string AddressID { get; set; }
		}

		public class Person : IHasAddressID
		{
			public Person() {
				Id = Guid.NewGuid();
			}

			public Guid Id { get; set; }
			public string AddressID { get; set; }
		}


		public query_by_interface_Tests()
        {
            using (var container = Container.For<DevelopmentModeRegistry>())
            {
                container.GetInstance<DocumentCleaner>().CompletelyRemoveAll();
            }
        }

        [Fact]
        public void query()
        {
            using (var container = Container.For<DevelopmentModeRegistry>())
            {
                using (var session = container.GetInstance<IDocumentStore>().OpenSession())
                {
                    session.Store(new Location {AddressID = "1"});
										session.Store(new Person { AddressID = "1" });
                    session.SaveChanges();

										var addresses = session.Query<IHasAddressID>().Where(x => x.AddressID == "1").ToArray();

                    addresses.Length.ShouldBe(2);
                }
            }
        }
   }
}