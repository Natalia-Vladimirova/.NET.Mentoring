using System.Collections.Generic;
using NorthwindORM;

namespace NorthwindDAL.Interfaces
{
    public interface IEmployeeRepository
    {
        IList<Employee> GetAll();

        Employee Get(int id);

        int Add(Employee employee);
    }
}
