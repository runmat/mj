namespace NHibernateTest
{
    class Program
    {
        static void Main()
        {
            using (var repository = new Repository())
            {
                repository.Action();
            }
        }
    }
}
