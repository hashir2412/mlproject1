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
                double heightMax = double.MinValue;
                double weightMax = double.MinValue;
                double ageMax = double.MinValue;
                double heightMin = double.MaxValue;
                double weightMin = double.MaxValue;
                double ageMin = double.MaxValue;
                for (int j = 0; j < personsWithAge.Count; j++)
                {
                    if (i == j)
                    {
                        continue;
                    }
                    if (personsWithAge[j].Age > ageMax)
                    {
                        ageMax = personsWithAge[j].Age;
                    }
                    if (personsWithAge[j].Age < ageMin)
                    {
                        ageMin = personsWithAge[j].Age;
                    }

                    if (personsWithAge[j].Height > heightMax)
                    {
                        heightMax = personsWithAge[j].Height;
                    }
                    if (personsWithAge[j].Height < heightMin)
                    {
                        heightMin = personsWithAge[j].Height;
                    }

                    if (personsWithAge[j].Weight > weightMax)
                    {
                        weightMax = personsWithAge[j].Weight;
                    }
                    if (personsWithAge[j].Weight < weightMin)
                    {
                        weightMin = personsWithAge[j].Weight;
                    }
                }
                foreach (var item in personsWithAge)
                {
                    item.HeightNormalized = (item.Height - heightMin) / (heightMax - heightMin);
                    item.WeightNormalized = (item.Weight - weightMin) / (weightMax - weightMin);
                    item.AgeNormalized = (item.Age - ageMin) / (ageMax - ageMin);
                }
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
                var s = FinalResultIsMale(distanceSorted, 1, elementToBeTested, "Cartesian", true);
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
                s = FinalResultIsMale(distanceSorted, 3, elementToBeTested, "Cartesian", true);
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
                s = FinalResultIsMale(distanceSorted, 5, elementToBeTested, "Cartesian", true);
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
                s = FinalResultIsMale(distanceSorted, 7, elementToBeTested, "Cartesian", true);
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
                s = FinalResultIsMale(distanceSorted, 9, elementToBeTested, "Cartesian", true);
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
                s = FinalResultIsMale(distanceSorted, 11, elementToBeTested, "Cartesian", true);
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
            Console.WriteLine($"Accuracy of K =1 {(double)k1CorrectPrediction * 100 / (k1CorrectPrediction + k1FalsePrediction)} %");
            Console.WriteLine($"Accuracy of K =3 {(double)k3CorrectPrediction * 100 / (k3CorrectPrediction + k3FalsePrediction)} %");
            Console.WriteLine($"Accuracy of K =5 {(double)k5CorrectPrediction * 100 / (k5CorrectPrediction + k5FalsePrediction)} %");
            Console.WriteLine($"Accuracy of K =7 {(double)k7CorrectPrediction * 100 / (k7CorrectPrediction + k7FalsePrediction)} %");
            Console.WriteLine($"Accuracy of K =9 {(double)k9CorrectPrediction * 100 / (k9CorrectPrediction + k9FalsePrediction)} %");
            Console.WriteLine($"Accuracy of K =11 {(double)k11CorrectPrediction * 100 / (k11CorrectPrediction + k11FalsePrediction)} %");
            Console.WriteLine($"For Age used in calculation {useAgeInCalculation} KNN Total Accuracy {(double)correctPrediction * 100 / (correctPrediction + falsePrediction)}%");
            Console.WriteLine("------------------------------");
        }

        async Task PredictData()
        {
            await ExtractDataFromTraining();
            double heightMax, weightMax, ageMax, heightMin, weightMin, ageMin;
            NormalizeData(out heightMax, out weightMax, out ageMax, out heightMin, out weightMin, out ageMin);
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
                    }
                }
                testData1a2a = await File.ReadAllLinesAsync("testData1a2a.txt");
            }
            Console.WriteLine();
            CalculatePrediction(heightMax, weightMax, ageMax, heightMin, weightMin, ageMin, testData1a2a, k);

        }

        private void CalculatePrediction(double heightMax, double weightMax, double ageMax, double heightMin, double weightMin, double ageMin, string[] testData1a2a, int? k)
        {
            foreach (var item in testData1a2a)
            {
                List<DistanceFromPoint> distanceFromPoints = new List<DistanceFromPoint>();
                var itemDetail = item.Replace("(", "").Replace(")", "").Split(",");
                var newItem = new PersonWithAge()
                {
                    Height = double.Parse(itemDetail[0].Trim()),
                    Weight = double.Parse(itemDetail[1].Trim()),
                    Age = int.Parse(itemDetail[2].Trim()),
                };
                newItem.HeightNormalized = (newItem.Height - heightMin) / (heightMax - heightMin);
                newItem.WeightNormalized = (newItem.Weight - weightMin) / (weightMax - weightMin);
                newItem.AgeNormalized = (newItem.Age - ageMin) / (ageMax - ageMin);
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

        private void NormalizeData(out double heightMax, out double weightMax, out double ageMax, out double heightMin, out double weightMin, out double ageMin)
        {
            heightMax = personsWithAge.Max(person => person.Height);
            weightMax = personsWithAge.Max(person => person.Weight);
            ageMax = personsWithAge.Max(person => person.Age);
            heightMin = personsWithAge.Min(person => person.Height);
            weightMin = personsWithAge.Min(person => person.Weight);
            ageMin = personsWithAge.Min(person => person.Age);
            foreach (var person in personsWithAge)
            {
                person.HeightNormalized = (person.Height - heightMin) / (heightMax - heightMin);
                person.WeightNormalized = (person.Weight - weightMin) / (weightMax - weightMin);
                person.AgeNormalized = (person.Age - ageMin) / (ageMax - ageMin);
            }
        }

        private async Task ExtractDataFromTraining()
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
        }

        bool FinalResultIsMale(DistanceFromPoint[] distanceFromPoints, int k, PersonWithAge p1, string matrixUsed, bool isOneOutEvaluation = false)
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
                if (!isOneOutEvaluation)
                {
                    Console.WriteLine($"For K = {k}:");
                    PrintNeighbors(distanceFromPoints, k, matrixUsed);
                    Console.WriteLine($"KNN 1a) Gender Prediction for Person with {p1.Height}, {p1.Weight} and {p1.Age} using {matrixUsed} Distance is M");
                }
                return true;
            }
            else
            {
                if (!isOneOutEvaluation)
                {
                    Console.WriteLine($"For K = {k}:");
                    PrintNeighbors(distanceFromPoints, k, matrixUsed);
                    Console.WriteLine($"KNN 1a) Gender Prediction for K = {k} for Person with {p1.Height}, {p1.Weight} and {p1.Age} using {matrixUsed} Distance is W");
                }
                return false;
            }
        }

        private static void PrintNeighbors(DistanceFromPoint[] distanceFromPoints, int k, string matrixUsed)
        {
            Console.WriteLine("Neighbors:");
            for (int i = 0; i < k; i++)
            {
                if (matrixUsed == "Manhattan")
                {
                    Console.WriteLine($"{matrixUsed} Distance : {distanceFromPoints[i].ManhattanDistance} ");
                }
                else if (matrixUsed == "Minkowski")
                {
                    Console.WriteLine($"{matrixUsed} Distance : {distanceFromPoints[i].MinkowskiDistance} ");
                }
                else
                {
                    Console.WriteLine($"{matrixUsed} Distance : {distanceFromPoints[i].CartesianDistance} ");
                }
                Console.WriteLine($"Neighbor {i + 1}: {distanceFromPoints[i].Person.Height}, {distanceFromPoints[i].Person.Weight} and {distanceFromPoints[i].Person.Gender}. ");
            }
        }

        double CalculateManhattanDistance(PersonWithAge p1, PersonWithAge p2)
        {
            return Math.Abs(p1.AgeNormalized - p2.AgeNormalized) + Math.Abs(p1.HeightNormalized - p2.HeightNormalized) + Math.Abs(p1.WeightNormalized - p2.WeightNormalized);
        }

        double CalculateMinkowskiDistance(PersonWithAge p1, PersonWithAge p2)
        {
            double distance = 0;
            distance += Math.Pow(Math.Abs(p1.HeightNormalized - p2.HeightNormalized), 3);
            distance += Math.Pow(Math.Abs(p1.WeightNormalized - p2.WeightNormalized), 3);
            distance += Math.Pow(Math.Abs(p1.AgeNormalized - p2.AgeNormalized), 3);
            distance = Math.Pow(distance, (double)1 / 3);
            return distance;
        }

        double CalculateCartesianDistance(PersonWithAge p1, PersonWithAge p2, bool useAgeInCalculation = true)
        {
            double distance = 0;
            distance += Math.Pow(Math.Abs(p1.HeightNormalized - p2.HeightNormalized), 2);
            distance += Math.Pow(Math.Abs(p1.WeightNormalized - p2.WeightNormalized), 2);
            if (useAgeInCalculation)
            {
                distance += Math.Pow(Math.Abs(p1.AgeNormalized - p2.AgeNormalized), 2);
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
