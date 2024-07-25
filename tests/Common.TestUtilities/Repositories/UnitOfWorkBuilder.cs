using Moq;
using MyRecipeBook.Domain.Repositories.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.TestUtilities.Repositories
{
    public class UnitOfWorkBuilder
    {
        public static IUnityOfWork Build()
        {
            var mock = new Mock<IUnityOfWork>();
            return mock.Object;
        }
    }
}
