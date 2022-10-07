using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
namespace Project1
{
    internal class Program
    {

        public async static Task Main(string[] args)
        {
            Knn knn = new Knn();
            await knn.GenerateDataAndPredict();
        }



    }
}
