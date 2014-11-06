using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace EntwicklungsDBConfigurator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("EntwicklungsDBConfigurtor");
            Console.WriteLine("-------------------------");
            Console.WriteLine();
            Console.WriteLine("Setze Portal Links...");
            Console.WriteLine();

            var con = new SqlConnection("Data Source=vms047;initial catalog=CKGPortal_Test;User ID=DADWebAccess;Password=seE?Anemone;persist security info=true;packet size=4096");

            try
            {
                con.Open();

                var sc = new SqlCommand("UPDATE [Customer] " +
                                        "SET [LoginLinkID] = 3 " +
                                        "WHERE [LoginLinkID] = 1", con);

                Console.WriteLine("Portal: " + sc.ExecuteNonQuery() + " Zeilen betroffen.");

                var sc1 = new SqlCommand("UPDATE [Customer] " +
                                         "SET [LoginLinkID] = 5 " +
                                         "WHERE [LoginLinkID] = 2", con);

                Console.WriteLine("Services: " + sc1.ExecuteNonQuery() + " Zeilen betroffen.");

                var sc2 = new SqlCommand("UPDATE [Customer] " +
                                         "SET [LoginLinkID] = 6 " +
                                         "WHERE [LoginLinkID] = 4", con);

                Console.WriteLine("PortalORM: " + sc2.ExecuteNonQuery() + " Zeilen betroffen.");

                Console.WriteLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (con.State != ConnectionState.Closed)
                {
                    con.Close();
                }
            }
            
            // Enter zum beenden
            Console.ReadLine();
        }
    }
}
