using Project1;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1
{
    public class Knn
    {
        public List<PersonWithAge> personsWithAge;

        public async Task GenerateDataAndPredict()
        {
            await PredictData();
            Console.WriteLine("X------------------------X--------------------------X");
            await PerformOneOutEvaluation(true);
            await PerformOneOutEvaluation(false);
        }

        async Task PerformOneOutEvaluation(bool useAgeInCalculation)
        {
            personsWithAge = new List<PersonWithAge>();
            string[] programData1c1d2c2d = await File.ReadAllLinesAsync("programData1c1d2c2d.txt");
            int k1CorrectPrediction = 0;
            int k1FalsePrediction = 0;
            int k3CorrectPrediction = 0;
            int k3FalsePrediction = 0;
            int k5CorrectPrediction = 0;
            int k5FalsePrediction = 0;
            int k7CorrectPrediction = 0;
            int k7FalsePrediction = 0;
            int k9CorrectPrediction = 0;
            int k9FalsePrediction = 0;
            int k11CorrectPrediction = 0;
            int k11FalsePrediction = 0;
            int correctPrediction = 0;
            int falsePrediction = 0;
            foreach (var item in programData1c1d2c2d)
            {
                var itemDetail = item.Replace("(", "").Replace(")", "").Split(",");
                var newItem = new PersonWithAge()
                {
                    Height = double.Parse(itemDetail[0].Trim()),
                    Weight = double.Parse(itemDetail[1].Trim()),
                    Age = int.Parse(itemDetail[2].Trim()),
                    Gender = itemDetail[3].Trim()
                };
                personsWithAge.Add(newItem);
            }
            for (int i = 0; i < personsWithAge.Count; i++)
            {
                var elementToBeTested = personsWithAge[i];
                List<DistanceFromPoint> distanceFromPoints = new List<DistanceFromPoint>();
                for (int j = 0; j < personsWithAge.Count; j++)
                {
                    if (i == j)
                    {
                        continue;
                    }
                    var cartesianDistance = CalculateCartesianDistance(personsWithAge[j], elementToBeTested, useAgeInCalculation);
                    distanceFromPoints.Add(new DistanceFromPoint()
                    {
                        Person = personsWithAge[j],
                        CartesianDistance = cartesianDistance
                    });
                }
                var distanceSorted = distanceFromPoints.OrderBy(a => a.CartesianDistance).ToArray();
                var s = FinalResultIsMale(distanceSorted, 1, elementToBeTested, "Cartesian");
                if ((s && elementToBeTested.Gender == "M") || (!s && elementToBeTested.Gender == "W"))
                {
                    k1CorrectPrediction++;
                    correctPrediction++;
                }
                else
                {
                    k1FalsePrediction++;
                    falsePrediction++;
                }
                s = FinalResultIsMale(distanceSorted, 3, elementToBeTested, "Cartesian");
                if ((s && elementToBeTested.Gender == "M") || (!s && elementToBeTested.Gender == "W"))
                {
                    k3CorrectPrediction++;
                    correctPrediction++;
                }
                else
                {
                    falsePrediction++;
                    k3FalsePrediction++;
                }
                s = FinalResultIsMale(distanceSorted, 5, elementToBeTested, "Cartesian");
                if ((s && elementToBeTested.Gender == "M") || (!s && elementToBeTested.Gender == "W"))
                {
                    k5CorrectPrediction++;
                    correctPrediction++;
                }
                else
                {
                    k5FalsePrediction++;
                    falsePrediction++;
                }
                s = FinalResultIsMale(distanceSorted, 7, elementToBeTested, "Cartesian");
                if ((s && elementToBeTested.Gender == "M") || (!s && elementToBeTested.Gender == "W"))
                {
                    k7CorrectPrediction++;
                    correctPrediction++;
                }
                else
                {
                    k7FalsePrediction++;
                    falsePrediction++;
                }
                s = FinalResultIsMale(distanceSorted, 9, elementToBeTested, "Cartesian");
                if ((s && elementToBeTested.Gender == "M") || (!s && elementToBeTested.Gender == "W"))
                {
                    k9CorrectPrediction++;
                    correctPrediction++;
                }
                else
                {
                    k9FalsePrediction++;
                    falsePrediction++;
                }
                s = FinalResultIsMale(distanceSorted, 11, elementToBeTested, "Cartesian");
                if ((s && elementToBeTested.Gender == "M") || (!s && elementToBeTested.Gender == "W"))
                {
                    k11CorrectPrediction++;
                    correctPrediction++;
                }
                else
                {
                    k11FalsePrediction++;
                    falsePrediction++;
                }
                //Console.WriteLine($"KNN {i} Actual Value: {elementToBeTested.Gender} , One out Evaluation with Age used: {useAgeInCalculation}");

            }
            Console.WriteLine($"Accuracy of K =1 {(double)k1CorrectPrediction * 100 / (k1CorrectPrediction + k1FalsePrediction)}");
            Console.WriteLine($"Accuracy of K =3 {(double)k3CorrectPrediction * 100 / (k3CorrectPrediction + k3FalsePrediction)}");
            Console.WriteLine($"Accuracy of K =5 {(double)k5CorrectPrediction * 100 / (k5CorrectPrediction + k5FalsePrediction)}");
            Console.WriteLine($"Accuracy of K =7 {(double)k7CorrectPrediction * 100 / (k7CorrectPrediction + k7FalsePrediction)}");
            Console.WriteLine($"Accuracy of K =9 {(double)k9CorrectPrediction * 100 / (k9CorrectPrediction + k9FalsePrediction)}");
            Console.WriteLine($"Accuracy of K =11 {(double)k11CorrectPrediction * 100 / (k11CorrectPrediction + k11FalsePrediction)}");
            Console.WriteLine($"KNN Correct Prediction {correctPrediction} False Prediction {falsePrediction} Total Predictions{correctPrediction + falsePrediction} Age used {useAgeInCalculation}");
            Console.WriteLine("------------------------------");
        }

        async Task PredictData()
        {
            personsWithAge = new List<PersonWithAge>();
            string[] trainingtData1a2a = await File.ReadAllLinesAsync("trainingtData1a2a.txt");
            foreach (var item in trainingtData1a2a)
            {
                var itemDetail = item.Replace("(", "").Replace(")", "").Split(",");
                var newItem = new PersonWithAge()
                {
                    Height = double.Parse(itemDetail[0].Trim()),
                    Weight = double.Parse(itemDetail[1].Trim()),
                    Age = int.Parse(itemDetail[2].Trim()),
                    Gender = itemDetail[3].Trim()
                };
                personsWithAge.Add(newItem);
            }
            Console.WriteLine();
            Console.WriteLine("Enter the respective selection");
            Console.WriteLine("Enter custom test data - 1");
            Console.WriteLine("Enter value K for KNN - 2");
            Console.WriteLine("Run KNN on default mode - 3");
            var input = Console.ReadKey();
            string[] testData1a2a = null;
            int? k = null;
            if (input.KeyChar == '1')
            {
                List<string> data = new List<string>();
                do
                {
                    Console.WriteLine();
                    Console.WriteLine("Enter the test data in the desired format eg ( 1.62065758929, 59.376557437583, 32)");
                    var testData = Console.ReadLine();
                    data.Add(testData);
                    Console.WriteLine("Would you to like to continue? Press 1 else press any other key");
                } while (Console.ReadKey().KeyChar == '1');
                testData1a2a = data.ToArray();
            }
            else
            {
                if (input.KeyChar == '2')
                {
                    Console.WriteLine();
                    Console.WriteLine("Enter the value for K");
                    if (int.TryParse(Console.ReadLine(), out int x))
                    {
                        k = x;
                        // Parse successful. value can be any integer
                    }
                }
                testData1a2a = await File.ReadAllLinesAsync("testData1a2a.txt");
            }
            Console.WriteLine();
            foreach (var item in testData1a2a)
            {
                List<DistanceFromPoint> distanceFromPoints = new List<DistanceFromPoint>();
                var itemDetail = item.Replace("(", "").Replace(")", "").Split(",");
                var newItem = new PersonWithAge()
                {
                    Height = double.Parse(itemDetail[0].Trim()),
                    Weight = double.Parse(itemDetail[1].Trim()),
                    Age = int.Parse(itemDetail[2].Trim())
                };
                foreach (var person in personsWithAge)
                {
                    var manhattanDistance = CalculateManhattanDistance(person, newItem);
                    var minkowskiDistance = CalculateMinkowskiDistance(person, newItem);
                    var cartesianDistance = CalculateCartesianDistance(person, newItem);
                    distanceFromPoints.Add(new DistanceFromPoint()
                    {
                        ManhattanDistance = manhattanDistance,
                        Person = person,
                        MinkowskiDistance = minkowskiDistance,
                        CartesianDistance = cartesianDistance
                    });
                }
                if (k == null)
                {
                    var distanceSorted = distanceFromPoints.OrderBy(a => a.ManhattanDistance).ToArray();
                    FinalResultIsMale(distanceSorted, 1, newItem, "Manhattan");
                    FinalResultIsMale(distanceSorted, 3, newItem, "Manhattan");
                    FinalResultIsMale(distanceSorted, 7, newItem, "Manhattan");
                    distanceSorted = distanceFromPoints.OrderBy(a => a.MinkowskiDistance).ToArray();
                    FinalResultIsMale(distanceSorted, 1, newItem, "Minkowski");
                    FinalResultIsMale(distanceSorted, 3, newItem, "Minkowski");
                    FinalResultIsMale(distanceSorted, 7, newItem, "Minkowski");
                    distanceSorted = distanceFromPoints.OrderBy(a => a.CartesianDistance).ToArray();
                    FinalResultIsMale(distanceSorted, 1, newItem, "Cartesian");
                    FinalResultIsMale(distanceSorted, 3, newItem, "Cartesian");
                    FinalResultIsMale(distanceSorted, 7, newItem, "Cartesian");
                    Console.WriteLine("--------------------------------------");
                    Console.WriteLine();
                }
                else
                {
                    var distanceSorted = distanceFromPoints.OrderBy(a => a.ManhattanDistance).ToArray();
                    FinalResultIsMale(distanceSorted, k.Value, newItem, "Manhattan");
                    distanceSorted = distanceFromPoints.OrderBy(a => a.MinkowskiDistance).ToArray();
                    FinalResultIsMale(distanceSorted, k.Value, newItem, "Minkowski");
                    distanceSorted = distanceFromPoints.OrderBy(a => a.CartesianDistance).ToArray();
                    FinalResultIsMale(distanceSorted, k.Value, newItem, "Cartesian");
                    Console.WriteLine("--------------------------------------");
                    Console.WriteLine();
                }

            }

        }

        bool FinalResultIsMale(DistanceFromPoint[] distanceFromPoints, int k, PersonWithAge p1, string matrixUsed)
        {
            var Mcount = 0;
            var FCount = 0;
            for (int i = 0; i < k; i++)
            {
                if (distanceFromPoints[i].Person.Gender == "M")
                {
                    Mcount++;
                }
                else
                {
                    FCount++;
                }
            }
            if (Mcount > FCount)
            {
                Console.WriteLine($"KNN 1a) Gender Prediction for K = {k} for Person with {p1.Height}, {p1.Weight} and {p1.Age} using {matrixUsed} Distance is M");
                return true;
            }
            else
            {
                Console.WriteLine($"KNN 1a) Gender Prediction for K = {k} for Person with {p1.Height}, {p1.Weight} and {p1.Age} using {matrixUsed} Distance is W");
                return false;
            }
        }

        double CalculateManhattanDistance(PersonWithAge p1, PersonWithAge p2)
        {
            return Math.Abs(p1.Age - p2.Age) + Math.Abs(p1.Height - p2.Height) + Math.Abs(p1.Weight - p2.Weight);
        }

        double CalculateMinkowskiDistance(PersonWithAge p1, PersonWithAge p2)
        {
            double distance = 0;
            distance += Math.Pow(Math.Abs(p1.Height - p2.Height), 3);
            distance += Math.Pow(Math.Abs(p1.Weight - p2.Weight), 3);
            distance += Math.Pow(Math.Abs(p1.Age - p2.Age), 3);
            distance = Math.Pow(distance, (double)1 / 3);
            return distance;
        }

        double CalculateCartesianDistance(PersonWithAge p1, PersonWithAge p2, bool useAgeInCalculation = true)
        {
            double distance = 0;
            distance += Math.Pow(Math.Abs(p1.Height - p2.Height), 2);
            distance += Math.Pow(Math.Abs(p1.Weight - p2.Weight), 2);
            if (useAgeInCalculation)
            {
                distance += Math.Pow(Math.Abs(p1.Age - p2.Age), 2);
            }
            distance = Math.Pow(distance, (double)1 / 2);
            return distance;
        }
    }
}

class DistanceFromPoint
{
    public Person Person { get; set; }
    public double ManhattanDistance { get; set; }

    public double MinkowskiDistance { get; set; }

    public double CartesianDistance { get; set; }
}
